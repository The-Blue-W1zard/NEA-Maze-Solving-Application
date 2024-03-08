using System.Diagnostics;
using System.Text;

namespace Nea_Maze_Solving_Application
{
    public partial class MainForm : Form
    {

        MazeCell[,] maze = new MazeCell[30, 50];
        MazeFileHandler mazeFileHandler;
        MazeFunctions mazeFunctions;
        Button[] speedButtons;
        Button[] algorithmButtons;
        Button[] generatorButtons;

        Point startCell = new(0, 0);
        Point endCell = new(28, 48);

        Stack<string> mazeHistory = new();

        public bool changingStartCell = false;
        public bool changingEndCell = false;
        public int animationDelay = 0;
        public string algorithm = "None";
        public string generatorAlgorithm = "None";

        /// <summary>
        /// Constructor creates all the buttons and initializes all click event handlers
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            CreateBlankMaze();
            mazeFunctions = new MazeFunctions(maze);
            mazeFileHandler = new MazeFileHandler(maze);
            mazeFileHandler.GenerateAppData();
            InitializeGroupedButtons();

        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Adds different click event handlers to relevant buttons.
        /// </summary>
        private void InitializeGroupedButtons()
        {
            speedButtons = [Slow, Medium, Fast];
            algorithmButtons = [Dijkstra, BreadthFirst, AStar];
            generatorButtons = [RandomDFS, RecursiveBacktracker];
            foreach (Button button in speedButtons) { button.Click += SpeedButton_Click; }
            foreach (Button button in algorithmButtons) { button.Click += AlgorithmButton_Click; }
            foreach (Button button in generatorButtons) { button.Click += GeneratorButton_Click; }

        }

        /// <summary>
        /// Changes animation speed dependent on which speed button is clicked
        /// </summary>
        private void SpeedButton_Click(object sender, EventArgs e)
        {
            Button? clickedButton = sender as Button;
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

        /// <summary>
        /// Changes maze solve algorithm being run dependent on which algorithm button is clicked
        /// </summary>
        private void AlgorithmButton_Click(Object sender, EventArgs e)
        {
            Button? clickedButton = sender as Button;

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

        /// <summary>
        /// Changes maze solve algorithm being run dependent on which algorithm button is clicked
        /// </summary>
        private void GeneratorButton_Click(Object sender, EventArgs e)
        {
            Button? clickedButton = sender as Button;

            foreach (Button button in generatorButtons)
            {
                if (button == clickedButton)
                {
                    button.BackColor = Color.Gray;
                    generatorAlgorithm = button.Name;

                }
                else
                {
                    button.BackColor = Color.FromArgb(224, 238, 249);
                }
            }
        }

        /// <summary>
        /// Iterates through maze creating MazeCell/Button grid.
        /// </summary>
        private void CreateBlankMaze()
        {
            for (int row = 0; row < maze.GetLength(0); row++)
            {
                for (int col = 0; col < maze.GetLength(1); col++)
                {
                    maze[row, col] = new MazeCell(new Point(row, col));
                    Controls.Add(maze[row, col]);
                    Invalidate();
                }
            }
            maze[startCell.X, startCell.Y].ToggleStartCell();
            maze[endCell.X, endCell.Y].ToggleEndCell();
        }

        /// <summary>
        /// Exports timed file to UndoQueue folder 
        /// </summary>
        private void UpdateUndoQueue()
        {
            mazeFileHandler.ExportTimedFile(ref mazeHistory, "UndoQueue");
        }

        /// <summary>
        /// Handles generate maze button clicks.
        /// </summary>
        private void GenerateMaze_Click(object sender, EventArgs e)
        {
            //Stores maze before anything has happened in undo queue.
            UpdateUndoQueue();
            MazeGenerator generator = new MazeGenerator(maze);
            mazeFunctions.ClearMaze();
            //Checks algorithm has been selected and runs relevant one
            if (generatorAlgorithm == "None") { MessageBox.Show("No algorithm selected."); }
            else if (generatorAlgorithm == "RandomDFS") { generator.GenerateDFSMaze(startCell); }
            else if (generatorAlgorithm == "RecursiveBacktracker") { generator.GenerateBacktrackedMaze(startCell); }
            //Updates history of generated mazes
            mazeFileHandler.UpdateGeneratedMazeHistory(ref mazeHistory);
            //And prompts user to change the start and end cells
            MessageBox.Show("Change the start and end cells using the buttons highlighted to the right.");
            ChangeStart.BackColor = Color.Yellow; ChangeEnd.BackColor = Color.Yellow;

        }

        /// <summary>
        /// Handles clear maze button clicks.
        /// </summary>
        private void Clear_Click(object sender, EventArgs e)
        {
            UpdateUndoQueue();
            mazeFunctions.ClearMaze();
        }

        /// <summary>
        /// Handles change start cell button clicks.
        /// </summary>
        private void ChangeStart_Click(object sender, EventArgs e)
        {
            UpdateUndoQueue();
            //Disables all buttons so click generates a location which can be converted to a button point
            if (!changingStartCell)
            {
                ChangeStart.BackColor = Color.Green;
                changingStartCell = true;
                mazeFunctions.ToggleAllMazeCells(false);

            }
            else if (changingStartCell)
            {
                ChangeStart.BackColor = Color.White;
                mazeFunctions.ToggleAllMazeCells(true);
            }

        }
        /// <summary>
        /// Handles clicks to form not directed at active button.
        /// </summary>
        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            //Only runs if user is changing the start/end cell else nothing happens when user clicks background/de-activated buttons.
            if (changingStartCell)
            {
                changingStartCell = false;
                mazeFunctions.ToggleAllMazeCells(true);
                ChangeStart.BackColor = Color.White;
                Point tempCell = mazeFunctions.CoordsToButtonPoint(e.X, e.Y);
                maze[startCell.X, startCell.Y].ToggleStartCell();
                startCell = tempCell;
                maze[startCell.X, startCell.Y].ToggleStartCell();
            }
            else if (changingEndCell)
            {
                changingEndCell = false;
                mazeFunctions.ToggleAllMazeCells(true);
                ChangeEnd.BackColor = Color.White;
                Point tempCell = mazeFunctions.CoordsToButtonPoint(e.X, e.Y);
                maze[endCell.X, endCell.Y].ToggleEndCell();
                endCell = tempCell;
                maze[endCell.X, endCell.Y].ToggleEndCell();
            }
        }
        /// <summary>
        /// Handles change end cell button clicks.
        /// </summary>
        private void ChangeEnd_Click(object sender, EventArgs e)
        {
            //Operates the same as when changing the start cell
            UpdateUndoQueue();
            if (!changingEndCell)
            {
                ChangeEnd.BackColor = Color.Red;
                changingEndCell = true;
                mazeFunctions.ToggleAllMazeCells(false);

            }
            else if (changingEndCell)
            {
                ChangeEnd.BackColor = Color.White;
                mazeFunctions.ToggleAllMazeCells(true);
            }
        }

        /// <summary>
        /// Handles clicks where user wants to load a maze from a file.
        /// </summary>
        private void ReloadFromFile_Click(object sender, EventArgs e)
        {
            UpdateUndoQueue();
            mazeFileHandler.OpenFileExplorer("NEAMazeSolver");

        }
        /// <summary>
        /// Handles export maze button clicks.
        /// </summary>
        private void ExportToFile_Click(object sender, EventArgs e)
        {
            UpdateUndoQueue();
            mazeFileHandler.ExportTimedFile(ref mazeHistory);
            //Informs the user when maze has finished exporting
            MessageBox.Show("Finished Exporting Maze"); 
        }

        /// <summary>
        /// Opens file explorer when user wants to reload a file from generated maze history.
        /// </summary>
        private void RecentMazeHistory_Click(object sender, EventArgs e)
        {
            UpdateUndoQueue();
            mazeFileHandler.OpenFileExplorer("MazeHistory");
        }

        /// <summary>
        /// Handles button clicks when user wants to solve the maze.
        /// </summary>
        private void SolveMaze_Click(object sender, EventArgs e)
        {
            //Creates new maze solver object to work on the maze
            MazeSolver solver = new MazeSolver(maze, startCell, endCell);
            UpdateUndoQueue();
            //Creates list of points to store order cells are explored and to store the discovered path
            List<Point> animationSteps = new();
            List<Point> path = new();
            //Checks algorithm has been selected then runs relevant one
            if (algorithm == "None") { MessageBox.Show("No algorithm selected."); return; }
            else if (algorithm == "AStar") { path = solver.AStarSearch(out animationSteps); }
            else if (algorithm == "Dijkstra") { path = solver.DijkstraSearch(out animationSteps); }
            else if (algorithm == "BreadthFirst") { path = solver.BreadthFirstSearch(out animationSteps); }
            //Animates maze exploration using animationSteps list, with delay specified by speed button pressed previously
            mazeFunctions.AnimateMaze(animationSteps, animationDelay);
            //Checks whether path has been found 
            if (path.Count == 1) { MessageBox.Show("No path found"); }
            else
            {
                //And animates it if has been
                mazeFunctions.UpdateMaze(path);
                FinishedAnimating();
            }
            UpdateUndoQueue();

        }
        
        /// <summary>
        /// Displays finished solving form and executes relevant instruction from user input on the maze.
        /// </summary>
        private void FinishedAnimating()
        {
            FinishedForm finished = new FinishedForm();
            finished.ShowDialog();
            if (finished.clearMaze) { mazeFunctions.ClearMaze(); }
            else if (finished.revertToPrev)
            {
                string prevMazeFilePath = mazeHistory.Pop();
                mazeFileHandler.JsonToMaze(prevMazeFilePath);
            }

        }

        /// <summary>
        /// Handles undo action button clicks.
        /// </summary>
        private void UndoAction_Click(object sender, EventArgs e)
        {
            //Tries to pop element from queue of file paths, if its empty outputs message saying so.
            bool tryPop= mazeHistory.TryPop(out string? prevMazeFilePath);
            if(tryPop)
            {
                mazeFileHandler.JsonToMaze(prevMazeFilePath);
            }
            else
            {
                MessageBox.Show("Error - Nothing to undo");
            }


        }
    }
}
