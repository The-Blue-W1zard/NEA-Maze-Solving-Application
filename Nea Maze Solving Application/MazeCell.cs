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
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <param name="isWall"></param>
        /// <param name="isStartCell"></param>
        /// <param name="isEndCell"></param>
        /// <param name="isOnPath"></param>
        /// <param name="isExplored"></param>
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

        public void CellClicked(object sender, EventArgs e)
        {

            if (isStartCell || isEndCell) {}
            else { ToggleWall(); }
            
        }

        private void CellState(bool state, Color color)
        {
            if (state) { BackColor = color; }
            else { BackColor = Color.White; }
        }

        public void ToggleStartCell()
        {
            isStartCell = !isStartCell;
            CellState(isStartCell, Color.Green);
        }

        public void ToggleEndCell()
        {
            isEndCell = !isEndCell;
            CellState(isEndCell, Color.Red);
        }

        public void ToggleWall()
        {
            isWall = !isWall;
            CellState(isWall, Color.Black);
        }

        public void TogglePath()
        {
            isOnPath = !isOnPath;
            CellState(isOnPath, Color.Blue);
        }

        public void ToggleExplored()
        {
            isExplored = !isExplored;
            CellState(isExplored, Color.Orange);
        }

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
            //Application.DoEvents();

            
        }

        




    }
}
