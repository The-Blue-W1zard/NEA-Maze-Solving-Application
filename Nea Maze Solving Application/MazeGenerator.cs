using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nea_Maze_Solving_Application
{
    /// <summary>
    /// Generates solvable mazes using a randomized depth first search or by recursive backtracking using inheritted algorithm functions.
    /// </summary>
    /// <param name="maze">Maze algorithm will be executed on</param>
    internal class MazeGenerator(MazeCell[,] maze) : AlgorithmFunctions 
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
        /// Executes a randomized depth first search on the maze, creating a solvable maze.
        /// </summary>
        /// <param name="start">Cell that maze will be generated from.</param>
        private void RandomizedDFS(Point start)
        {   
            //Queue used to store which cells to visit and list stores cells that have been visited so far
            Queue<Point> queue = new Queue<Point>();
            List<Point> visited = new();
            Point current = start;
            Point none = new(-1, -1);
            queue.Enqueue(current);
            visited.Add(current);

            //While the queue isn't empty...
            while (queue.Count > 0)
            {
                //Gets a random unvisited neighbour of the currently selected cell to explore next
                Point next = RandomUnvisitedNeighbour(maze, current, ref visited);
                //If cell doesnt have any neighbours...
                if (next == none)
                {
                    //Works its way back up the queue of cells to explore till it finds a cell with an unvisted neighbour
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
        /// <param name="start">Cell currently being explored.</param>
        /// <param name="visited">List of visited cells.</param>
        private void RecursiveBacktracker(Point start, ref List<Point> visited)
        {
            //Adds the passed cell to the list of visited then... 
            visited.Add(start);
            //Creates a list of possible unvisited neighbour cells,and shuffles it.
            List<Point> possibleNeighbours = Neighbours(maze,start,2,visited);
            possibleNeighbours = ShuffleList(possibleNeighbours);
             
            //Itterates through each neighbour cell...
            foreach(Point p in possibleNeighbours)
            {
                //Checking that it hasnt been visited in another recursive calls exploration
                if (!visited.Contains(p))
                {
                    //If it hasnt it connects the neighbouring cell to the start cell
                    ConnectCells(maze, start, p);
                    Application.DoEvents();
                    //And starts exploring the the neighbour cell
                    RecursiveBacktracker(p, ref visited);

                }
                //Else the loop makes its way to the next neighbour

            }
            //Until all neighbours have been explored in which case the recursive call ends

        }

        /// <summary>
        /// Accesible function that starts executing the recursive backtracker algorithm on the maze.
        /// </summary>
        /// <param name="start">Cell algorithm starts generating the maze from.</param>
        public void GenerateBacktrackedMaze(Point start)
        {
            List<Point> visited = new();
            GenerateGrid();
            RecursiveBacktracker(start, ref visited);
        }


        /// <summary>
        /// Accesible function that creates a grid then excecutes a randomized DFS on it.
        /// </summary>
        /// <param name="start">Cell algorithm starts generating the maze from.</param>
        public void GenerateDFSMaze(Point start)
        {
            GenerateGrid();
            RandomizedDFS(start);
        }

    }
}
