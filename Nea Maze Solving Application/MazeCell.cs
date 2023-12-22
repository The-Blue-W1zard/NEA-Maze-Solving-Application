using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nea_Maze_Solving_Application
{
    internal class MazeCell
    {
        public Point location;   
        private Size size = new Size(32, 32);
        public Button btn;
        public bool isWall {  get; private set; }
        public bool isStartCell { get; private set; }
        public bool isEndCell { get; private set; }

        public MazeCell()
        {
            btn = new Button();

        }

        public void Intialize()
        {
            btn.Size = size;
            btn.FlatStyle = FlatStyle.Flat;
            btn.Location = new Point((location.Y+1)*32, (location.X+1)*32);
            btn.Click += new EventHandler(CellClicked);
            isWall = false;
            btn.BackColor = Color.White;

        }

        public void CellClicked(object sender, EventArgs e)
        {
            ToggleWall();

        }

        private void CellState(bool state, Color color)
        {
            if (state) { btn.BackColor = color; }
            else { btn.BackColor = Color.White; }
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
