using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nea_Maze_Solving_Application
{
    internal class MazeFunctions
    {
        
        public void ToggleAllMazeCells(bool mode, MazeCell[,] maze)
        {
            for (int r = 0; r < maze.GetLength(0); r++)
            {
                for (int c = 0; c < maze.GetLength(1); c++)
                {
                    maze[r, c].Enabled = mode;

                }
            }
        }
        public Point CoordsToButtonPoint(int x, int y)
        {
            int col = x / 32 - 1;
            int row = y / 32 - 1;
            //MessageBox.Show($"r = {row} c = {col}");
            return new Point(row, col);
        }
        public void ClearMaze(MazeCell[,] maze)
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
        public void UpdateMaze(MazeCell[,] maze, List<Point> path)
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
        public void AnimateMaze(MazeCell[,] maze, List<Point> path, int delay)
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
