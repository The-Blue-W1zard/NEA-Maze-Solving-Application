

namespace Nea_Maze_Solving_Application
{
    /// <summary>
    /// Presents user with options on what to do next once maze has been solved.
    /// </summary>
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
