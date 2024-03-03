using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// <param name="visited">List containing all previously explored cells.</param>
        /// <returns>List of possible neighbour cell locations.</returns>
        public static List<Point> Neighbours(MazeCell[,] maze, Point current, int dist, List<Point> visited = default)
        {
            int[] checksRows = [0, 0, dist, -dist];
            int[] checksCols = [dist, -dist, 0, 0];
            int row = current.X;
            int col = current.Y;
            List<Point> neighbours = [];

            if (visited == default) { visited = new List<Point>(); }

            for (int t = 0; t < 4; t++)
            {
                try
                {
                    //Nested in a try loop in case tries to access maze cell outside the boundaries of the 2D array 
                    if (!maze[row + checksRows[t], col + checksCols[t]].isWall && !visited.Contains(new Point(row + checksRows[t], col + checksCols[t])))
                    {
                        neighbours.Add(new Point(row + checksRows[t], col + checksCols[t]));
                    }

                }
                catch { }
            }
            return neighbours;
        }

        /// <summary>
        /// Gets a random unvisited neighbour cell. 
        /// </summary>
        /// <param name="maze">Maze being checked.</param>
        /// <param name="current">Initial cell searching from.</param>
        /// <param name="visited">List containing all previously explored cells.</param>
        /// <returns>Point specifying cell if one is possible, else a default none value.</returns>
        public Point RandomUnvisitedNeighbour(MazeCell[,] maze, Point current, ref List<Point> visited)
        {
            Random rand = new();
            Point next = new(-1, -1);
            Point none = new(-1, -1);
            List<Point> neighbours = Neighbours(maze, current, 2, visited);
            while (neighbours.Count > 0)
            {
                next = neighbours[rand.Next(neighbours.Count)];
                neighbours.Remove(next);
                if (!visited.Contains(next)) { break; }
                next = none;
            }
            return next;

        }

        /// <summary>
        /// Works through dictionary of previous cells finding path from end to start.
        /// </summary>
        /// <param name="prev">Dictionary of each cells previous cell.</param>
        /// <param name="goal">Cell backtracking from, target cell of solving algorithm.</param>
        /// <returns>List containing coordinates of cells that make the found path.</returns>
        public List<Point> RecallPath(Dictionary<Point, Point> prev, Point initial)
        {
            Point None = new(-1, -1);
            Point current = initial;
            List<Point> path = [];
            path.Clear();


            while (current != None)
            {
                path.Add(current);
                current = prev[current];
            }
            path.Reverse();
            return path;
        }

        /// <summary>
        /// Calculates the eucilidan distance between two inputted points.
        /// </summary>
        /// <param name="start">First point.</param>
        /// <param name="goal">Second point.</param>
        /// <returns>Distance between the points.</returns>
        public int EuclidianDistance(Point start, Point goal)
        {
            //Gets the square root of the sum of the differences in x and y posisitons squared
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
                (list[n], list[k]) = (list[k], list[n]);
            }
            return list;
        }
    }
}
