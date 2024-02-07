using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nea_Maze_Solving_Application
{
    internal class MazeGenerator : AlgorithmFunctions
    {
        private void GenerateRowsCols(MazeCell[,] maze)
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
        private void RandomizedDFS(MazeCell[,] maze, Point start)
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

        public void GenerateDFSMaze(MazeCell[,] maze, Point start)
        {
            GenerateRowsCols(maze);
            RandomizedDFS(maze, start);
        }

    }
}
