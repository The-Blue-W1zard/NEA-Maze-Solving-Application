using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nea_Maze_Solving_Application
{
    /// <summary>
    /// Generates solvable mazes using a randomized depth first search with help of inheritted algorithm functions.
    /// </summary>
    /// <param name="maze">Maze algorithm will be excecuted on</param>
    internal class MazeGenerator(MazeCell[,] maze) : AlgorithmFunctions 
    {
        /// <summary>
        /// Creates grid of walls using modulo operator
        /// </summary>
        private void GenerateGrid()
        {
            //int r = maze.GetLength(0);
            //int c = maze.GetLength(1);

            for (int r = 0; r < maze.GetLength(0); r++)
            {
                for (int c = 0; c < maze.GetLength(1); c++)
                {
                    if (maze[r,c].isStartCell || maze[r, c].isEndCell) { continue; }
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
        /// Randomly works its way through the maze recursively connecting cells together.
        /// </summary>
        /// <param name="start">Cell currently being explored</param>
        /// <param name="visited">List of visited cells</param>
        private void RecursiveBacktracker(Point start, ref List<Point> visited)
        {
            visited.Add(start);
            List<Point> possibleNeighbours = Neighbours(maze,start,2,visited.ToList());
            possibleNeighbours = ShuffleList(possibleNeighbours);
             
            foreach(Point p in possibleNeighbours)
            {
                if (!visited.Contains(p))
                {
                    ConnectCells(maze, start, p);
                    Application.DoEvents();
                    RecursiveBacktracker(p, ref visited);

                }

            }

        }

        /// <summary>
        /// Accesible function that starts excecuting the recursive backtracker algorithm on the maze
        /// </summary>
        /// <param name="start">Cell algorithm starts from</param>
        public void GenerateBacktrackedMaze(Point start)
        {
            List<Point> visited = new();
            //List<Point> visited = new();
            GenerateGrid();
            RecursiveBacktracker(start, ref visited);
        }




        /// <summary>
        /// Accesible function that creates grid then maze.
        /// </summary>
        /// <param name="start"></param>
        public void GenerateDFSMaze(Point start)
        {
            GenerateGrid();
            RandomizedDFS(start);
        }

    }
}
