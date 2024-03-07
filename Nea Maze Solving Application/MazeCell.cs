namespace Nea_Maze_Solving_Application
{
    internal class MazeCell:Button
    {
        private readonly Size size = new(32, 32);
        public Point location { get; private set; }   
        public bool isWall {  get; private set; }
        public bool isStartCell { get; private set; }
        public bool isEndCell { get; private set; }
        public bool isOnPath { get; private set; }
        public bool isExplored { get; private set; }

        /// <summary>
        /// Class constructor sets the initial values of various attributes and configures button features and location
        /// </summary>
        /// <param name="location">Point coordinates of the cell.</param>
        /// <param name="isWall">Whether the cell is a wall.</param>
        /// <param name="isStartCell">Whether the cell is the start cell.</param>
        /// <param name="isEndCell">Whether the cell is the end cell.</param>
        /// <param name="isOnPath">Whether the cell is on the found path.</param>
        /// <param name="isExplored">Whether the cell has been explored by a maze solving algorithm.</param>
        public MazeCell(Point location, bool isWall = false, bool isStartCell = false, bool isEndCell = false, bool isOnPath = false, bool isExplored = false)
        {
            this.location = location;
            this.isWall = isWall;
            this.isStartCell = isStartCell;
            this.isEndCell = isEndCell;
            this.isOnPath = isOnPath;
            this.isExplored = isExplored;
            Size = size;
            FlatStyle = FlatStyle.Flat;
            Location = new Point((location.Y + 1) * 32, (location.X + 1) * 32);
            Click += new EventHandler(CellClicked);
            BackColor = Color.White;
            Name = Convert.ToString(location);
        }
        
        /// <summary>
        /// Toggles whether cell is a wall when clicked
        /// </summary>
        /// <param name="sender">Reference to control/object which caused click event.</param>
        /// <param name="e">Contains event data.</param>
        public void CellClicked(object sender, EventArgs e)
        {
            //Checks if start/end so can't change them
            if (isStartCell || isEndCell) {}
            else { ToggleWall(); }
            
        }

        /// <summary>
        /// Changes cells colour dependent on boolean value
        /// </summary>
        /// <param name="state">Whether colour should be changed.</param>
        /// <param name="colour">Colour to be changed too.</param>
        private void CellState(bool state, Color colour)
        {
            if (state) { BackColor = colour; }
            else { BackColor = Color.White; }
        }

        /// <summary>
        /// Changes cell colour to green if start cell.
        /// </summary>
        public void ToggleStartCell()
        {
            isStartCell = !isStartCell;
            CellState(isStartCell, Color.Green);
        }
        /// <summary>
        /// Changes cell colour to red if end cell.
        /// </summary>
        public void ToggleEndCell()
        {
            isEndCell = !isEndCell;
            CellState(isEndCell, Color.Red);
        }
        /// <summary>
        /// Changes cell colour to black if is a wall cell.
        /// </summary>
        public void ToggleWall()
        {
            isWall = !isWall;
            CellState(isWall, Color.Black);
        }
        /// <summary>
        /// Changes cell colour to blue if is on the found path.
        /// </summary>
        public void TogglePath()
        {
            isOnPath = !isOnPath;
            CellState(isOnPath, Color.Blue);
        }
        /// <summary>
        /// Changes cell colour to orange if has been explored by an algorithm
        /// </summary>
        public void ToggleExplored()
        {
            isExplored = !isExplored;
            CellState(isExplored, Color.Orange);
        }

        /// <summary>
        /// Updates Maze Cells attributes to inputted values and updates each maze cells colour.
        /// </summary>
        public void RefreshCell(bool isWall, bool isStartCell, bool isEndCell, bool isOnPath, bool isExplored)
        {
            this.isWall = isWall;
            this.isStartCell = isStartCell;
            this.isEndCell = isEndCell;
            this.isOnPath = isOnPath;
            this.isExplored = isExplored;
            if (isStartCell) { CellState(isStartCell, Color.Green); }
            else if (isEndCell) { CellState(isEndCell, Color.Red); }      
            else if (isWall) { CellState(isWall, Color.Black); }
            else if (isOnPath) { CellState(isOnPath, Color.Blue); }
            else if (isExplored) { CellState(isExplored, Color.Orange); }
            else { BackColor = Color.White; }
            
        }


    }
}
