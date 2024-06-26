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
    /// Generates solvable mazes using a randomized depth first search or by recursive backtracking using inherited algorithm functions.
    /// </summary>
    /// <param name="maze">Maze algorithm will be executed on</param>
    internal class MazeGenerator(MazeCell[,] maze, Point start) : AlgorithmFunctions 
    {
        /// <summary>
        /// Creates grid of walls using modulo operator.
        /// </summary>
        private void GenerateGrid()
        {

            for (int r = 0; r < maze.GetLength(0); r++)
            {
                for (int c = 0; c < maze.GetLength(1); c++)
                {
                    //Makes sure the start and end cell aren't turned into walls
                    if (maze[r,c].isStartCell || maze[r, c].isEndCell) { continue; }
                    if (r % 2 == 1) { maze[r, c].ToggleWall(); }
                    else if (c % 2 == 1) { maze[r, c].ToggleWall(); }
                }
            }
        }

        /// <summary>
        /// Executes a randomised depth first search on the maze, creating a solvable maze.
        /// </summary>
        private void RandomisedDFS()
        {   
            //Queue used to store which cells to visit and hashset stores cells that have been visited so far
            Queue<Point> queue = new Queue<Point>();
            HashSet<Point> visited = new();
            Point current = start;
            Point none = new(-1, -1);
            queue.Enqueue(current);
            visited.Add(current);

            //While the queue isn't empty...
            while (queue.Count > 0)
            {
                //Gets a random unvisited neighbour of the currently selected cell to explore next
                Point next = RandomNeighbour(maze, current,2, visited);
                //If cell doesn't have any neighbours...
                if (next == none)
                {
                    //Works its way back up the queue of cells to explore till it finds a cell with an unvisited neighbour
                    while (queue.Count > 0)
                    {
                        next = queue.Dequeue();
                        if (Neighbours(maze, next, 2, visited).Count > 0)
                        {
                            current = next;
                            break;
                        }
                    }
                    //Or ends the algorithm if there isn't one
                    if (0 == queue.Count)
                    {
                        break;
                    }

                }
                //Else the algorithm connects (breaks the wall between) the current cell to the randomly selected one...
                ConnectCells(maze, current, next);
                //And continues exploring from the randomly explored cell.
                current = next;
                queue.Enqueue(current);
                visited.Add(current);
                Application.DoEvents();
            }
        }

        /// <summary>
        /// Randomly works its way through the maze recursively connecting cells together.
        /// </summary>
        /// <param name="current">Cell currently being explored.</param>
        /// <param name="visited">List of visited cells.</param>
        private void RecursiveBacktracker(Point current, ref HashSet<Point> visited)
        {
            //Adds the passed cell to the list of visited then... 
            visited.Add(current);
            //Creates a list of possible unvisited neighbour cells,and shuffles it.
            List<Point> possibleNeighbours = Neighbours(maze,current,2,visited);
            possibleNeighbours = ShuffleList(possibleNeighbours);
             
            //Iterates through each neighbour cell...
            foreach(Point p in possibleNeighbours)
            {
                //Checking that it hasn't been visited in another recursive calls exploration
                if (!visited.Contains(p))
                {
                    //If it hasn't it connects the neighbouring cell to the start cell
                    ConnectCells(maze, current, p);
                    Application.DoEvents();
                    //And starts exploring the neighbour cell
                    RecursiveBacktracker(p, ref visited);

                }
                //Else the loop makes its way to the next neighbour

            }
            //Until all neighbours have been explored in which case the recursive call ends

        }

        /// <summary>
        /// Executes kruskals algorithm on the maze, randomly connecting sets of cells together.
        /// </summary>
        private void KruskalsAlgorithm()
        {
            //List stores walls to be connected in maze and hash set stores hashsets that will be grouped together
            List<Point> walls = new List<Point>();
            HashSet<HashSet<Point>> sets = new HashSet<HashSet<Point>>();

            //Iterates through maze adding relevant wall cells to list, and creates hashset for every initially blank cell
            for (int row = 0; row < maze.GetLength(0); row++)
            {
                for (int col = 0; col < maze.GetLength(1); col++)
                {
                    if(row%2 == 0 && maze[row,col].isWall) { walls.Add(new(row, col)); }
                    else if (col%2 == 0 && maze[row,col].isWall) { walls.Add(new(row, col)); }
                    else if (!maze[row, col].isWall) { sets.Add([new(row, col)]); }
                }
            }

            //Shuffles list of walls so connects sets of cells together randomly
            walls = ShuffleList(walls);

            //Iterates through every wall cell...
            foreach (Point wall in walls)
            {
                //Finding neighbour cells to potentially connect.
                Point cellOne;
                Point cellTwo;
                //Direction of cells depends on location of wall 
                //If on even row sets to horizontal neighbours
                if (wall.X % 2 == 0)
                {
                    cellOne = wall with { Y = wall.Y - 1 };
                    cellTwo = wall with { Y = wall.Y + 1 };
                }
                //Else gets vertical neighbours 
                else
                {
                    cellOne = wall with { X = wall.X - 1 };
                    cellTwo = wall with { X = wall.X + 1 };
                }
                
                //Two hashsets temporarily store the sets containing the selected neighbour cells.
                HashSet<Point> setOne = new HashSet<Point>();
                HashSet<Point> setTwo = new HashSet<Point>();
                foreach(HashSet<Point> points in sets)
                {
                    if (points.Contains(cellOne)) { setOne = points; }
                    if (points.Contains(cellTwo)) {  setTwo = points;}
                }

                //If the two sets are equal skips this iteration
                if (setOne.SetEquals(setTwo)) { continue; }
                //Else connects the two neighbouring cells (breaks wall between)
                ConnectCells(maze, cellOne, cellTwo);
                Application.DoEvents();
                //Then merges the two sets together
                HashSet<Point> newSet = new HashSet<Point>(setOne.Union(setTwo));
                //And updates the set of sets to only contain the new combined set
                sets.Remove(setOne);
                sets.Remove(setTwo);
                sets.Add(newSet);

            }
        }

        /// <summary>
        /// Executes prims algorithm on the maze, randomly branching different paths forwards
        /// </summary>
        private void PrimsAlgorithm()
        {
            //Hash-sets used to store which cells have been visited and which cells are on the frontier of exploration 
            HashSet<Point> visited = new HashSet<Point>();
            HashSet<Point> frontier = new HashSet<Point>();
            //Creates random generator to randomly select cells
            Random r = new Random();
            //Initializes the current cell as the start cell and adds it to the list of visited cells 
            Point current = start;
            visited.Add(current);

            //Does...
            do
            {
                //Finds the unvisited neighbours of the current cell and adds them to the set of frontier cells
                foreach (Point neighbours in Neighbours(maze, current, 2, visited)) { frontier.Add(neighbours); }
                //Then randomly selects a frontier cell, setting it to be the new current cell
                int index = r.Next(frontier.Count);
                current = frontier.ElementAt(index);
                //Adding it to the set of visited cells, and removing it from the frontier cells
                visited.Add(current);
                frontier.Remove(current);
                //Then finds a random visited neighbour of the frontier cell
                Point visitedCell = RandomVisitedNeighbour(maze, current, 2, visited);
                //And connects it to the frontier cell
                ConnectCells(maze, current, visitedCell);
                Application.DoEvents();

            } while (frontier.Count > 0);
            //While there are still frontier cells to explore



        }

        /// <summary>
        /// Accessible function that starts executing prims algorithm on the maze.
        /// </summary>
        public void GeneratePrimsMaze()
        {
            GenerateGrid();
            PrimsAlgorithm ();
        }
        /// <summary>
        /// Accessible function that starts executing the recursive backtracked algorithm on the maze.
        /// </summary>
        public void GenerateKruskalMaze()
        {
            GenerateGrid();
            KruskalsAlgorithm();
        }
        /// <summary>
        /// Accessible function that starts executing the recursive backtracked algorithm on the maze.
        /// </summary>
        public void GenerateBacktrackedMaze()
        {
            HashSet<Point> visited = new();
            GenerateGrid();
            RecursiveBacktracker(start, ref visited);
        }


        /// <summary>
        /// Accessible function that creates a grid then executes a randomized DFS on it.
        /// </summary>
        public void GenerateDFSMaze()
        {
            GenerateGrid();
            RandomisedDFS();
        }

    }
}
