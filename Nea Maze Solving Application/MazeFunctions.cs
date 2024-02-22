using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nea_Maze_Solving_Application
{
    /// <summary>
    /// Group of functions used to change various aspects of related maze
    /// </summary>
    /// <param name="maze">Maze that functions will be applied to</param>
    internal class MazeFunctions(MazeCell[,] maze)
    {
        /// <summary>
        /// Ittertaes through maze enabling/disabling all buttons
        /// </summary>
        /// <param name="mode">Boolean value buttons will be toggled too</param>
        public void ToggleAllMazeCells(bool mode)
        {
            for (int r = 0; r < maze.GetLength(0); r++)
            {
                for (int c = 0; c < maze.GetLength(1); c++)
                {
                    maze[r, c].Enabled = mode;

                }
            }
        }

        /// <summary>
        /// Converts (x,y) coordinates of click to location of a button. 
        /// </summary>
        /// <param name="x">X coordinate of click</param>
        /// <param name="y">Y coordinate of click</param>
        /// <returns>Point sepcifying which button was clicked</returns>
        public Point CoordsToButtonPoint(int x, int y)
        {
            int col = x / 32 - 1;
            int row = y / 32 - 1;
            //MessageBox.Show($"r = {row} c = {col}");
            return new Point(row, col);
        }

        /// <summary>
        /// itterate sthrough the maze disabling all features of maze cells.
        /// </summary>
        public void ClearMaze()
        {
            foreach (MazeCell cell in maze)
            {
                if (cell.isWall) { cell.ToggleWall(); }
                if (cell.isOnPath) { cell.TogglePath(); }
                if (cell.isExplored) { cell.ToggleExplored(); }
                if (cell.isStartCell || cell.isEndCell) { continue; }
            }

            //Invalidate();
        }
        /// <summary>
        /// Updates maze to have path displayed on it.
        /// </summary>
        /// <param name="path">List of points path follows</param>
        public void UpdateMaze(List<Point> path)
        {

            for (int i = 0; i < path.Count; i++)
            {
                try
                {
                    if (maze[path[i].X, path[i].Y].isWall) { Debug.WriteLine("mucked up here"); continue; }
                    if (maze[path[i].X, path[i].Y].isStartCell || maze[path[i].X, path[i].Y].isEndCell) { continue; }
                    maze[path[i].X, path[i].Y].TogglePath();
                    //Thread.Sleep(1);
                    Application.DoEvents();
                }
                catch { Debug.WriteLine("coordinates bad"); }
            }

        }
        /// <summary>
        /// Updates maze with cells explored, animating them in order.
        /// </summary>
        /// <param name="path">List of points showing order cells where explored</param>
        /// <param name="delay">Delay between changing each cell</param>
        public void AnimateMaze(List<Point> path, int delay)
        {
            for (int i = 0; i < path.Count; i++)
            {
                try
                {
                    if (maze[path[i].X, path[i].Y].isWall) { Console.WriteLine("mucked up here"); continue; }
                    if (maze[path[i].X, path[i].Y].isStartCell || maze[path[i].X, path[i].Y].isEndCell) { continue; }
                    maze[path[i].X, path[i].Y].ToggleExplored();
                    Thread.Sleep(delay);
                    Application.DoEvents();
                }
                catch { Console.WriteLine("coordinates bad"); }
            }
            
            //Console.WriteLine("After Update");
            //OutputMaze(maze);


        }

        





    }
}
