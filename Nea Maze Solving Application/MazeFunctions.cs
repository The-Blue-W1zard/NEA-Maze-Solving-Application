using System;
using System.Diagnostics;


namespace Nea_Maze_Solving_Application
{
    /// <summary>
    /// Group of methods used to change various aspects of the desired maze.
    /// </summary>
    /// <param name="maze">Maze that functions will be applied to.</param>
    internal class MazeFunctions(MazeCell[,] maze)
    {
        /// <summary>
        /// Itterates through maze enabling/disabling all buttons.
        /// </summary>
        /// <param name="mode">Boolean value buttons will be toggled too.</param>
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
        /// Converts (x,y) coordinates of a click to location of a button. 
        /// </summary>
        /// <param name="x">X coordinate of click.</param>
        /// <param name="y">Y coordinate of click.</param>
        /// <returns>Point sepcifying which button was clicked.</returns>
        public Point CoordsToButtonPoint(int x, int y)
        {
            MessageBox.Show($"{maze.GetLength(0)},{maze.GetLength(1)}");
            int col = x / 32 - 1;
            int row = y / 32 - 1;
            //Checks that the user has clicked within the bounds of the maze, if not sets the location to be the closest possible cell.
            int possibleRows = maze.GetLength(0) -1;
            int possibleCols = maze.GetLength(1) -1;
            if (col > possibleCols) { col = possibleCols; }
            else if (col < 0 ) { col = 0; }
            if (row > possibleRows) {  row = possibleRows; }
            else if(row < 0 ) {  row = 0; }

            return new Point(row, col);
        }

        /// <summary>
        /// Itterates through the maze disabling all features of each cells.
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
        }
        /// <summary>
        /// Updates maze to display inputted path on it.
        /// </summary>
        /// <param name="path">List of points path follows.</param>
        public void UpdateMaze(List<Point> path)
        {
            for (int i = 0; i < path.Count; i++)
            {
                try
                {
                    //checks if the cell is the start or end cell, preventing the program from changing its colour.
                    if (maze[path[i].X, path[i].Y].isStartCell || maze[path[i].X, path[i].Y].isEndCell) { continue; }
                    maze[path[i].X, path[i].Y].TogglePath();
                    //Forces the program to update the colours of the maze cells, so path is smoothly animated on.
                    Application.DoEvents();
                }
                catch { Debug.WriteLine("coordinates bad"); }
            }

        }
        /// <summary>
        /// Updates maze with explored cells, animating them in order.
        /// </summary>
        /// <param name="path">List of points showing order cells where explored.</param>
        /// <param name="delay">Delay between changing each cell.</param>
        public void AnimateMaze(List<Point> path, int delay)
        {
            for (int i = 0; i < path.Count; i++)
            {
                try
                {
                    if (maze[path[i].X, path[i].Y].isStartCell || maze[path[i].X, path[i].Y].isEndCell) { continue; }
                    maze[path[i].X, path[i].Y].ToggleExplored();
                    Thread.Sleep(delay);
                    Application.DoEvents();
                }
                catch { Debug.WriteLine("coordinates bad"); }
            }

        }

        





    }
}
