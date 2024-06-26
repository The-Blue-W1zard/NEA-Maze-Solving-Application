﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace Nea_Maze_Solving_Application
{
    /// <summary>
    /// Basic functions that maze generator and solver classes require to run
    /// </summary>
    internal class AlgorithmFunctions
    {
        /// <summary>
        /// Checks for unvisited, accessible cells in 4 dimensions around the inputted cell.
        /// </summary>
        /// <param name="maze">Maze to be checked.</param>
        /// <param name="current">Initial cell to search from.</param>
        /// <param name="dist">How far out cells should be searched.</param>
        /// <param name="visited">Hashset containing all previously explored cells.</param>
        /// <returns>List of possible neighbour cell locations.</returns>
        public static List<Point> Neighbours(MazeCell[,] maze, Point current, int dist, HashSet<Point>? visited = default)
        {
            //Arrays store distance to explore horizontally and vertically, dependent on dist passed attribute 
            int[] checksRows = [0, 0, dist, -dist];
            int[] checksCols = [dist, -dist, 0, 0];
            //Gets the number of rows and columns in the maze
            int row = current.X;
            int col = current.Y;
            List<Point> neighbours = [];

            //If no visited hashset is passed, sets to empty set so neighbour cells are never contained within visited.
            if (visited == default) { visited = new HashSet<Point>(); }

            //Iterates through the 4 directions being explored
            for (int t = 0; t < 4; t++)
            {
                try
                {
                    //Nested in a try loop in case tries to access maze cell outside the boundaries of the 2D array 
                    if (!maze[row + checksRows[t], col + checksCols[t]].isWall && !visited.Contains(new Point(row + checksRows[t], col + checksCols[t])))
                    {
                        //If cell isn't a wall and isn't contained in visited it is added to the list of possible neighbours
                        neighbours.Add(new Point(row + checksRows[t], col + checksCols[t]));
                    }

                }
                catch
                {
                    // ignored
                }
            }
            return neighbours;
        }

        /// <summary>
        /// Gets a random visited neighbour cell (required for prims algorithm) 
        /// </summary>
        /// <param name="maze">Maze to be checked.</param>
        /// <param name="current">Initial cell to search from.</param>
        /// <param name="dist">How far out cells should be searched.</param>
        /// <param name="visited">List containing all previously explored cells.</param>
        /// <returns>Point specifying visited neighbour cell.</returns>
        public Point RandomVisitedNeighbour(MazeCell[,] maze, Point current, int dist, HashSet<Point> visited)
        {
            //Arrays store distance to explore horizontally and vertically, dependent on dist passed attribute 
            int[] checksRows = [0, 0, dist, -dist];
            int[] checksCols = [dist, -dist, 0, 0];
            //Gets the number of rows and columns in the maze
            int row = current.X;
            int col = current.Y;
            List<Point> neighbours = [];
          
            //Iterates through the 4 directions being explored
            for (int t = 0; t < 4; t++)
            {
                try
                {
                    //Nested in a try loop in case tries to access maze cell outside the boundaries of the 2D array 
                    if (!maze[row + checksRows[t], col + checksCols[t]].isWall && visited.Contains(new Point(row + checksRows[t], col + checksCols[t])))
                    {
                        //If cell isn't a wall and is contained in visited set it is added to the list of possible neighbours
                        neighbours.Add(new Point(row + checksRows[t], col + checksCols[t]));
                    }

                }
                catch
                {
                    // ignored
                }
            }
            neighbours = ShuffleList(neighbours);
            return neighbours[0];
        }


        /// <summary>
        /// Gets a random, unvisited neighbour cell. 
        /// </summary>
        /// <param name="maze">Maze to be checked.</param>
        /// <param name="current">Initial cell searching from.</param>
        /// <param name="dist">How far out cells should be searched.</param>
        /// <param name="visited">Hashset containing all previously explored cells.</param>
        /// <returns>Point specifying cell if one is possible, else a default none value.</returns>
        public Point RandomNeighbour(MazeCell[,] maze, Point current,int dist, HashSet<Point>? visited = null)
        {
            //Gets a list of unvisited neighbours and shuffles it
            List<Point> neighbours = Neighbours(maze, current, dist, visited);
            neighbours = ShuffleList(neighbours);
            //Returns a neighbouring cells point if one is available.
            if(neighbours.Count != 0){ return neighbours[0]; }
            //Else returns no point signified by(-1,-1)
            return new Point(-1, -1);


        }

        
        /// <summary>
        /// Works through dictionary of previous cells finding path from end to start if possible.
        /// </summary>
        /// <param name="prev">Dictionary of each cells previous cell.</param>
        /// <param name="initial">Cell backtracking from, target cell of solving algorithm.</param>
        /// <returns>List containing coordinates of cells that make the found path.</returns>
        public List<Point> RecallPath(MazeCell[,] maze, Dictionary<Point, Point> prev, Point initial)
        {
            Point none = new(-1, -1);
            Point current = initial;
            List<Point> path = [];
            path.Clear();
            //While hasn't reached dead end (none) adds current cell to path then sets new current to be previous cell of the current cell
            while (current != none)
            {
                if (maze[current.X,current.Y].isWall) {break;}
                path.Add(current);
                current = prev[current];
            }
            //Path reversed so animated from start to end
            path.Reverse();
            return path;
        }

        /// <summary>
        /// Calculates the euclidean distance between two inputted points.
        /// </summary>
        /// <param name="start">First point.</param>
        /// <param name="goal">Second point.</param>
        /// <returns>Distance between the points.</returns>
        public int EuclideanDistance(Point start, Point goal)
        {
            //Gets the square root of the sum of the differences in x and y positions squared
            double h = Math.Sqrt(Math.Pow(start.X - goal.X, 2) + Math.Pow(start.Y - goal.Y, 2));
            return (int)Math.Round(h);
        }

        /// <summary>
        /// Toggles wall between two specified cells.
        /// </summary>
        /// <param name="maze">Maze being changed.</param>
        /// <param name="first">First cell.</param>
        /// <param name="second">Second cell.</param>
        public void ConnectCells(MazeCell[,] maze, Point first, Point second)
        {
            int row = (first.X + second.X) / 2;
            int col = (first.Y + second.Y) / 2;
            if (maze[row, col].isWall) { maze[row, col].ToggleWall(); }
        }

        /// <summary>
        /// Randomizes the locations of items in an inputted list.
        /// </summary>
        /// <param name="list">List of Points to be shuffled.</param>
        /// <returns>Shuffled list of points.</returns>
        public List<Point> ShuffleList(List<Point> list)
        {
            Random rand = new Random();
            int n = list.Count;
            while(n > 1)
            {
                n--;
                int k = rand.Next(n+1);
                //Swaps two items in  the list using tuples
                (list[n], list[k]) = (list[k], list[n]);
            }
            return list;
        }

        /// <summary>
        /// Sets previous value of each maze cell to default value.
        /// </summary>
        /// <param name="maze">Maze being iterated through.</param>
        /// <returns>Dictionary with default previous values.</returns>
        public Dictionary<Point, Point> SetDefaultPreviousDictionary(MazeCell[,] maze)
        {
            Dictionary<Point, Point> prev = new Dictionary<Point, Point>();
            //Iterates through every maze cell setting previous to be (-1,-1)
            for (int row = 0; row < maze.GetLength(0); row++)
            {
                for (int col = 0; col < maze.GetLength(1); col++)
                {
                    Point point = new(row, col);
                    prev.Add(point, new Point(-1, -1));
;
                }
            }

            return prev;
        }
    }
}
