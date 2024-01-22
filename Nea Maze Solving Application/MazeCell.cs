using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nea_Maze_Solving_Application
{
    internal class MazeCell:Button
    {
        public Point location;   
        private Size size = new Size(32, 32);
        public Button btn;
        public bool isWall {  get; private set; }
        public bool isStartCell { get; private set; }
        public bool isEndCell { get; private set; }

        public bool isOnPath { get; private set; }
        public bool isExplored { get; private set; }

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

        public void Intialize()
        {
            Size = size;
            FlatStyle = FlatStyle.Flat;
            Location = new Point((location.Y+1)*32, (location.X+1)*32);
            Click += new EventHandler(CellClicked);
            isWall = false;
            isOnPath = false;
            isExplored = false;
            BackColor = Color.White;
            Name = Convert.ToString(location);

        }

        public void CellClicked(object sender, EventArgs e)
        {
            Visible = false;
            //ToggleWall();


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
            CellState(isStartCell, Color.Green);
            CellState(isEndCell, Color.Red);
            CellState(isWall, Color.Black);
            CellState(isOnPath, Color.Blue);
            CellState(isExplored, Color.Orange);
            
        }

        public void OverwriteCell(MazeCell cell)
        {
            isWall = cell.isWall;
            isStartCell = cell.isStartCell;
            isEndCell = cell.isEndCell;
            isOnPath = cell.isOnPath;
            isExplored = cell.isExplored;
            CellState(isStartCell, Color.Green);
            CellState(isEndCell, Color.Red);
            CellState(isWall, Color.Black);
            CellState(isOnPath, Color.Blue);
            CellState(isExplored, Color.Orange);


        }

        //public void IsStartCell()
        //{
        //    if (isStartCell == false)
        //    {
        //        isStartCell= true;
        //        btn.BackColor = Color.Green; ;
        //    }
        //    else if (isStartCell == true)
        //    {
        //        isStartCell = false;
        //        btn.BackColor = Color.White;
        //    }

        //}
        //public void IsEndCell()
        //{
        //    if (isEndCell == false)
        //    {
        //        isEndCell = true;
        //        btn.BackColor = Color.Green; ;
        //    }
        //    else if (isEndCell == true)
        //    {
        //        isEndCell = false;
        //        btn.BackColor = Color.White;
        //    }

        //}




    }
}
