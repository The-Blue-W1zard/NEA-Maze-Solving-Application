using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nea_Maze_Solving_Application
{
    /// <summary>
    /// Generates solvable mazes using a randomized depth first search.
    /// </summary>
    /// <param name="maze">Maze algorithm will be excecuted on</param>
    internal class MazeGenerator(MazeCell[,] maze) : AlgorithmFunctions 
    {
        /// <summary>
        /// Creates grid of walls using modulo operator
        /// </summary>
        private void GenerateRowsCols()
        {
            //int r = maze.GetLength(0);
            //int c = maze.GetLength(1);

            for (int r = 0; r < maze.GetLength(0); r++)
            {
                for (int c = 0; c < maze.GetLength(1); c++)
                {
                    if (r % 2 == 1) { maze[r, c].ToggleWall(); }
                    else if (c % 2 == 1) { maze[r, c].ToggleWall(); }
                }
            }
        }

        /// <summary>
        /// Executes a randomized depth first search on the maze, creating a solvable maze.
        /// </summary>
        /// <param name="start">Cell that maze will be generated from</param>
        private void RandomizedDFS(Point start)
        {
            Queue<Point> queue = new Queue<Point>();
            //Stack<Point> stack = new Stack<Point>();
            List<Point> visited = new();
            Point current = start;
            Point next = new();
            Point none = new(-1, -1);
            queue.Enqueue(current);
            visited.Add(current);

            while (queue.Count > 0)
            {
                next = randomUnvisitedNeighbour(maze, current, ref visited);
                //Console.WriteLine(next);
                if (next == none)
                {
                    while (queue.Count > 0)
                    {
                        next = queue.Dequeue();
                        //Console.WriteLine(next);
                        if (Neighbours(maze, next, 2, visited).Count() > 0)
                        {
                            //Console.WriteLine(NeighboursMazeGen(maze, next, visited).Count());
                            current = next;
                            break;
                        }
                    }
                    if (0 == queue.Count)
                    {
                        break;
                    }

                }

                ConnectCells(maze, current, next);
                current = next;
                queue.Enqueue(current);
                visited.Add(current);
                Application.DoEvents();
                //Thread.Sleep(1);

                //foreach (Point p in queue) { Console.Write(p); }
                //Console.WriteLine();

            }
        }
        /// <summary>
        /// Accesible function that creates grid then maze.
        /// </summary>
        /// <param name="start"></param>
        public void GenerateDFSMaze(Point start)
        {
            GenerateRowsCols();
            RandomizedDFS(start);
        }

    }
}
