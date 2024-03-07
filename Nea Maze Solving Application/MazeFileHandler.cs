using System.Diagnostics;
using Newtonsoft.Json;

namespace Nea_Maze_Solving_Application
{
    /// <summary>
    /// Contains methods related to importing/exporting maze files and generating folders they will be stored at.
    /// </summary>
    /// <param name="maze">Maze methods will be applied to.</param>
    internal class MazeFileHandler(MazeCell[,] maze)
    {
        /// <summary>
        /// Temporary cell class used for (de)serialization. Stores key maze cell attributes. 
        /// </summary>
        private class TempCell
        {
            public Point location { get; set; }
            public bool isWall { get; set; }
            public bool isStartCell { get; set; }
            public bool isEndCell { get; set; }
            public bool isOnPath { get; set; }
            public bool isExplored { get; set; }

        }

        /// <summary>
        /// Iterates though every cell storing attributes to a JSON file.
        /// </summary>
        /// <param name="filePath">Location file will be stored</param>
        public void MazeToJsonFile(string filePath)
        {
            //Creates list to store all TempCells before serialization
            List<TempCell> cells = new List<TempCell>();
            for (int r = 0; r < maze.GetLength(0); r++)
            {
                for (int c = 0; c < maze.GetLength(1); c++)
                {
                    //Creates a TempCell from each Maze Cells attributes and adds it to the list of TempCells
                    var temp = new TempCell()
                    {
                        location = maze[r, c].location,
                        isWall = maze[r, c].isWall,
                        isStartCell = maze[r, c].isStartCell,
                        isEndCell = maze[r, c].isEndCell,
                        isOnPath = maze[r, c].isOnPath,
                        isExplored = maze[r, c].isExplored,
                    };
                    cells.Add(temp);
                }
            }
            //Serializes the list using newtonsofts' Json Convert
            string jsonString = JsonConvert.SerializeObject(cells, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore });
            //And writes it to a file
            File.WriteAllText(filePath, jsonString);

        }

        /// <summary>
        /// Deserializes JSON file then updates and outputs maze.
        /// </summary>
        /// <param name="filePath">File being loaded.</param>
        public void JsonToMaze(string filePath)
        {

            string jsonString = File.ReadAllText(filePath);
            //Makes sure that the inputted file type ends with json so doesn't try to load wrong
            //CHANGE BACK BEFORE HAND IN
            //if (!jsonString.EndsWith("json")) { MessageBox.Show(filePath);return; }

            //Converting back from TempCells to MazeCells nested in try catch in case erroneous data is inputted and serialization/conversion breaks
            try
            {
                var list = JsonConvert.DeserializeObject<List<TempCell>>(jsonString);
                foreach (TempCell t in list)
                {
                    maze[t.location.X, t.location.Y].RefreshCell(t.isWall, t.isStartCell, t.isEndCell, t.isOnPath, t.isExplored);
                }
            }
            //Outputs error message if loading maze fails
            catch { MessageBox.Show("Erroneous File Inputted"); }
                                              
        }
       

        /// <summary>
        /// Opens a file explorer window where user can select a json file to load maze from.
        /// </summary>
        /// <param name="initialDirectory">Folder file explorer will start at.</param>
        public void OpenFileExplorer(string initialDirectory)
        {
            //Opens file explorer window with settings to make sure...
            OpenFileDialog ofd = new OpenFileDialog()
            {
                //It opens in the correct directory
                InitialDirectory = GetDefaultFolderPath(initialDirectory),
                Title = "Select Maze File",

                CheckFileExists = true,
                CheckPathExists = true,

                //Only the correct file types are shown and can be selected
                DefaultExt = "json",
                Filter = "JSON Files (*.json)|*.json",
                RestoreDirectory = true,

                //File isn't readonly 
                ReadOnlyChecked = true,
                ShowReadOnly = true,

            };

            //If the user selects an ok file
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //Loads the file to the maze
                JsonToMaze(ofd.FileName);
            }
        }

        /// <summary>
        /// Verifies/Creates required directories/folders.
        /// </summary>
        public void GenerateAppData()
        {
            string appDataFolder = GetDefaultFolderPath();
            string mazeHistoryFolder = GetDefaultFolderPath("MazeHistory");
            string undoQueueFolder = GetDefaultFolderPath("UndoQueue");
            //Checks that all the desired directories exist, and if they don't creates them
            if (!Directory.Exists(appDataFolder))
            {
                Directory.CreateDirectory(appDataFolder);
            }
            if (!Directory.Exists(mazeHistoryFolder))
            {
                Directory.CreateDirectory(mazeHistoryFolder);
            }
            if (!Directory.Exists(undoQueueFolder))
            {
                Directory.CreateDirectory(undoQueueFolder);
            }
            //Also clears the undoQueue folder to prevent it from getting filled up with lots of files
            foreach (FileInfo file in new DirectoryInfo(undoQueueFolder).GetFiles()) { file.Delete(); }


        }
        /// <summary>
        /// Gets local data folder file path and combines it with application name as well as any folder extension provided. 
        /// </summary>
        /// <param name="folderExtension">Folder in app data folder to be accessed.</param>
        /// <returns>String containing whole folder path</returns>
        public string GetDefaultFolderPath(string? folderExtension = null)
        {
            string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string appDataFolder = Path.Combine(localAppData, "NEAMazeSolver");
            //If no folder extension is provided returns just the default folder path
            if(string.IsNullOrEmpty(folderExtension)){ return appDataFolder; }
            //Else returns the combined one
            return Path.Combine(appDataFolder, folderExtension);
        }

        /// <summary>
        /// Updates the stored collection of the 10 most recently generated mazes, removing any older ones. 
        /// </summary>
        /// <param name="mazeHistory">Stack of file paths for recent changes</param>
        public void UpdateGeneratedMazeHistory(ref Stack<string> mazeHistory)
        {   
             
            DirectoryInfo dir = new DirectoryInfo(GetDefaultFolderPath("MazeHistory"));
            //If there's less than 10 files stored simply exports it
            if (dir.GetFiles().Length < 10) { ExportTimedFile(ref mazeHistory, "MazeHistory"); }
            else
            {
                //Else deletes the oldest one then exports the file
                FileInfo oldestFile = new FileInfo(@"C:\NotRealFile.txt");
                foreach (FileInfo file in dir.GetFiles())
                {
                    //Uses last write time to compare file ages
                    if (file.LastWriteTime.CompareTo(oldestFile.LastWriteTime) > 0) { oldestFile = file; }
                }
                Debug.WriteLine(oldestFile.FullName);
                oldestFile.Delete();
                ExportTimedFile(ref mazeHistory, "MazeHistory");
            }

        }
        /// <summary>
        /// Exports the maze setting the file name to the current date and time.
        /// </summary>
        /// <param name="mazeHistory">Stack of file paths showing locations of exported files.</param>
        /// <param name="extensionPath">Optional folder path extension to store file at.</param>
        public void ExportTimedFile(ref Stack<string> mazeHistory, string? extensionPath = null )
        {           
            //Gets the current date to use as the file name using DateTime
            DateTime currentDate = DateTime.Now;
            string time = currentDate.ToString("dd-MM-yyyy-HH-mm-ss");
            //Gets the full file path from the folder path with the extension combined with the filename from the time prepended to .json                           
            string filePath = Path.Combine(GetDefaultFolderPath(extensionPath), time + ".json");
            //Adds the file path to the stack of file paths, and then converts to the maze to a json stored at the file paths location
            mazeHistory.Push(filePath);
            MazeToJsonFile(filePath);

        }

    }
}
