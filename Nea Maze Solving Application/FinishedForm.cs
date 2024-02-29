using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nea_Maze_Solving_Application
{
    public partial class FinishedForm : Form
    {
        public bool revertToPrev = false;
        public bool clearMaze = false;
        public FinishedForm()
        {
            InitializeComponent();
        }

        private void Ignore_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RevertMaze_Click(object sender, EventArgs e)
        {
            revertToPrev = true;
            this.Close();
        }

        private void ClearMaze_Click(object sender, EventArgs e)
        {
            clearMaze = true;
            this.Close();
        }
    }
}
