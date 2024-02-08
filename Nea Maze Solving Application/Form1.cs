using System.Diagnostics;
using System.Text;

namespace Nea_Maze_Solving_Application
{
    public partial class Form1 : Form
    {

        MazeCell[,] maze = new MazeCell[30, 50];
        MazeSolver solver;
        MazeGenerator generator;
        MazeFileHandler mazeFileHandler;
        MazeFunctions mazeFunctions;
        Button[] speedButtons;
        Button[] algorithmButtons;

        Point startCell = new Point(0, 0);
        Point endCell = new Point(28, 48);
        Point tempCell = new Point();

        Stack<string> mazeHistory = new Stack<string>();

        public bool changingStartCell = false;
        public bool changingEndCell = false;
        public bool clicked = false;
        public int animationDelay = 0;
        public string algorithm = "None";

        public Form1()
        {
            InitializeComponent();
            CreateBlankMaze();
            solver = new MazeSolver();
            generator = new MazeGenerator();
            mazeFunctions = new MazeFunctions();
            mazeFileHandler = new MazeFileHandler(maze);
            mazeFileHandler.GenerateAppData();
            InitializeGroupedButtons();

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //hello
        }

        private void InitializeGroupedButtons()
        {
            speedButtons = new Button[3] { Slow, Medium, Fast };
            algorithmButtons = new Button[3] { Dijkstra, BreadthFirst, AStar };
            foreach (Button button in speedButtons) { button.Click += speedButton_Click; }
            foreach (Button button in algorithmButtons) { button.Click += algorithmButton_Click; }

        }

        private void speedButton_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            Dictionary<string, int> speeds = new Dictionary<string, int> { { "Fast", 0 }, { "Medium", 10 }, { "Slow", 20 } };

            foreach (Button button in speedButtons)
            {
                if (button == clickedButton)
                {
                    button.BackColor = Color.Gray;
                    animationDelay = speeds[button.Name];
                }
                else
                {
                    button.BackColor = Color.FromArgb(224, 238, 249);
                }
            }

        }
        private void algorithmButton_Click(Object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;

            foreach (Button button in algorithmButtons)
            {
                if (button == clickedButton)
                {
                    button.BackColor = Color.Gray;
                    algorithm = button.Name;

                }
                else
                {
                    button.BackColor = Color.FromArgb(224, 238, 249);
                }
            }
        }

        private void CreateBlankMaze()
        {
            for (int row = 0; row < maze.GetLength(0); row++)
            {
                for (int col = 0; col < maze.GetLength(1); col++)
                {
                    maze[row, col] = new MazeCell(new Point(row, col));
                    //maze[row, col].location = new Point(row, col);
                    //maze[row, col].Intialize();
                    Controls.Add(maze[row, col]);
                    Invalidate();
                }
            }
            maze[startCell.X, startCell.Y].ToggleStartCell();
            maze[endCell.X, endCell.Y].ToggleEndCell();
        }
        public void AlgorithmPrequisites()
        {
            string undoQueuePath = mazeFileHandler.GetDefaultFolderPath("UndoQueue");
            mazeFileHandler.ExportTimedFile(ref mazeHistory, undoQueuePath);

        }
        private void generateMaze_Click(object sender, EventArgs e)
        {
            mazeFunctions.ClearMaze(maze);
            generator.GenerateDFSMaze(maze, startCell);
            mazeFileHandler.UpdateGeneratedMazeHistory(ref mazeHistory);
            MessageBox.Show("Change the start and end cells using the buttons highlighted to the right.");
            ChangeStart.BackColor = Color.Yellow; ChangeEnd.BackColor = Color.Yellow;

        }
        //private void Dijkstra_Click(object sender, EventArgs e)
        //{
        //    AlgorithmPrequisites();
        //    List<Point> animationSteps;
        //    List<Point> path = solver.DijkstraSearch(maze, startCell, endCell, out animationSteps);
        //    mazeFunctions.AnimateMaze(maze, animationSteps, animationDelay);
        //    mazeFunctions.UpdateMaze(maze, path);
        //}
        //private void AStar_Click(object sender, EventArgs e)
        //{
        //    AlgorithmPrequisites();
        //    List<Point> animationSteps;
        //    List<Point> path = solver.AStarSearch(maze, startCell, endCell, out animationSteps);
        //    mazeFunctions.AnimateMaze(maze, animationSteps, animationDelay);
        //    mazeFunctions.UpdateMaze(maze, path);
        //}
        //private void BreadthFirst_Click(object sender, EventArgs e)
        //{
        //    AlgorithmPrequisites();
        //    List<Point> animationSteps;
        //    List<Point> path = solver.BreadthFirstSearch(maze, startCell, endCell, out animationSteps);
        //    mazeFunctions.AnimateMaze(maze, animationSteps, animationDelay);
        //    mazeFunctions.UpdateMaze(maze, path);
        //}
        private void ReloadMaze_Click(object sender, EventArgs e)
        {
            try
            {
                string prevMazeFilePath = mazeHistory.Pop();
                mazeFileHandler.CSVToMaze(prevMazeFilePath);

            }
            catch { MessageBox.Show("Error - Nothing to undo"); }
        }
        private void Clear_Click(object sender, EventArgs e)
        {
            AlgorithmPrequisites();
            mazeFunctions.ClearMaze(maze);
            //CSVToMaze();
        }
        private void ChangeStart_Click(object sender, EventArgs e)
        {
            if (!changingStartCell)
            {
                ChangeStart.BackColor = Color.Green;
                changingStartCell = true;
                mazeFunctions.ToggleAllMazeCells(false, maze);
                return;
                //MessageBox.Show("Button was pressed");

            }
            else if (changingStartCell)
            {
                ChangeStart.BackColor = Color.White;
                mazeFunctions.ToggleAllMazeCells(true, maze);
                return;
            }

        }
        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            clicked = true;
            if (changingStartCell)
            {
                //MessageBox.Show($"Mouse click at X: {e.X}, Y: {e.Y} after button press.");
                changingStartCell = false;
                mazeFunctions.ToggleAllMazeCells(true, maze);
                ChangeStart.BackColor = Color.White;
                tempCell = mazeFunctions.CoordsToButtonPoint(e.X, e.Y);
                maze[startCell.X, startCell.Y].ToggleStartCell();
                startCell = tempCell;
                maze[startCell.X, startCell.Y].ToggleStartCell();
            }
            else if (changingEndCell)
            {
                changingEndCell = false;
                mazeFunctions.ToggleAllMazeCells(true, maze);
                ChangeEnd.BackColor = Color.White;
                tempCell = mazeFunctions.CoordsToButtonPoint(e.X, e.Y);
                maze[endCell.X, endCell.Y].ToggleEndCell();
                endCell = tempCell;
                maze[endCell.X, endCell.Y].ToggleEndCell();
            }
        }
        private void ChangeEnd_Click(object sender, EventArgs e)
        {
            if (!changingEndCell)
            {
                ChangeEnd.BackColor = Color.Red;
                changingEndCell = true;
                mazeFunctions.ToggleAllMazeCells(false, maze);
                //MessageBox.Show("Button was pressed");

            }
            else if (changingEndCell)
            {
                ChangeEnd.BackColor = Color.White;
                mazeFunctions.ToggleAllMazeCells(true, maze);
            }
        }
        private void ReloadFromFile_Click(object sender, EventArgs e)
        {
            mazeFileHandler.OpenFileExplorer("NEAMazeSolver");

        }
        private void ExportToFile_Click(object sender, EventArgs e)
        {
            //string temp = GetCurrentTime() + ".csv";
            //MessageBox.Show(temp);
            mazeFileHandler.ExportTimedFile(ref mazeHistory);
        }

        private void RecentMazeHistory_Click(object sender, EventArgs e)
        {
            string path = Path.Combine("NEAMazeSolver", "MazeHistory");
            mazeFileHandler.OpenFileExplorer(path);
        }

        private void SolveMaze_Click(object sender, EventArgs e)
        {
            AlgorithmPrequisites();
            List<Point> animationSteps = new();
            List<Point> path = new();
            if(algorithm == "None") { MessageBox.Show("No algorithm selected."); return; }
            else if (algorithm == "AStar"){path = solver.AStarSearch(maze, startCell, endCell, out animationSteps);}
            else if (algorithm == "Dijkstra") { path = solver.DijkstraSearch(maze, startCell, endCell, out animationSteps); }
            else if (algorithm == "BreadthFirst") { path = solver.BreadthFirstSearch(maze, startCell, endCell, out animationSteps); }
            mazeFunctions.AnimateMaze(maze, animationSteps, animationDelay);
            mazeFunctions.UpdateMaze(maze, path);
            FinishedAnimating();

        }
        private void FinishedAnimating()
        {
            Form2 finished = new Form2();
            finished.ShowDialog();
            if (finished.clearMaze == true) { mazeFunctions.ClearMaze(maze); }
            else if (finished.revertToPrev == true) {
                string prevMazeFilePath = mazeHistory.Pop();
                mazeFileHandler.CSVToMaze(prevMazeFilePath);
            }

        }
    }
}
