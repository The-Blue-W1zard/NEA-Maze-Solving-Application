using System;
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
        /// Temporary cell class used for (de)serlialization. Stores key maze cell attributes. 
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
        /// Itterates though every cell storing attributes to a JSON file.
        /// </summary>
        /// <param name="filePath">Location file will be stored</param>
        public void MazeToJSONFile(string filePath)
        {
            List<TempCell> cells = new List<TempCell>();
            for (int r = 0; r < maze.GetLength(0); r++)
            {
                for (int c = 0; c < maze.GetLength(1); c++)
                {
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
            string jsonString = JsonConvert.SerializeObject(cells, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore });
            File.WriteAllText(filePath, jsonString);

        }

        /// <summary>
        /// Deserealizes JSON file then updates and outputs maze.
        /// </summary>
        /// <param name="filePath">File being loaded.</param>
        public void JSONToMaze(string filePath)
        {

            string jsonString = File.ReadAllText(filePath);
            if (!jsonString.EndsWith(".json")) { MessageBox.Show("Wrong File Type");return; }

            try
            {
                var list = JsonConvert.DeserializeObject<List<TempCell>>(jsonString);
                foreach (TempCell t in list)
                {
                    maze[t.location.X, t.location.Y].RefreshCell(t.isWall, t.isStartCell, t.isEndCell, t.isOnPath, t.isExplored);
                }
            }
            catch { MessageBox.Show("Erroneous File Inputted"); }
                                              
        }
       

        /// <summary>
        /// Opens a file explorer window where user can select a json file to load maze from.
        /// </summary>
        /// <param name="initialDirectory">Folder file explorer will start at.</param>
        public void OpenFileExplorer(string initialDirectory)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                InitialDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), initialDirectory),
                Title = "Select Maze File",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "json",
                Filter = "JSON Files (*.json)|*.json",
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true,

            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                JSONToMaze(ofd.FileName);
            }
        }
        /// <summary>
        /// Verifies/Creates required directories/folders.
        /// </summary>
        public void GenerateAppData()
        {
            string appDataFolder = GetDefaultFolderPath();
            string mazeHistoryFolder = GetDefaultFolderPath("MazeHistory");
            string UndoQueueFolder = GetDefaultFolderPath("UndoQueue");
            //Checks that all the desired directories exist
            if (!Directory.Exists(appDataFolder))
            {
                Directory.CreateDirectory(appDataFolder);
            }
            if (!Directory.Exists(mazeHistoryFolder))
            {
                Directory.CreateDirectory(mazeHistoryFolder);
            }
            if (!Directory.Exists(UndoQueueFolder))
            {
                Directory.CreateDirectory(UndoQueueFolder);
            }
            foreach (FileInfo file in new DirectoryInfo(UndoQueueFolder).GetFiles()) { file.Delete(); }


        }
        /// <summary>
        /// Gets local data folder file path and combines it with application name as well as any folder extension if provided. 
        /// </summary>
        /// <param name="folderExtension">Folder in app data folder to be accessed.</param>
        /// <returns>String containing whole folder path</returns>
        public string GetDefaultFolderPath(string folderExtension = null)
        {
            string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string appDataFolder = Path.Combine(localAppData, "NEAMazeSolver");
            try { return Path.Combine(appDataFolder, folderExtension); }
            catch { return appDataFolder; }
        }

        /// <summary>
        /// Updates the stored collection of the 10 most recently generated mazes, removing any older ones. 
        /// </summary>
        /// <param name="mazeHistory">Stack of file paths for recent changes</param>
        public void UpdateGeneratedMazeHistory(ref Stack<string> mazeHistory)
        {   
            string mazeHistoryPath = GetDefaultFolderPath("MazeHistory");
            DirectoryInfo dir = new DirectoryInfo(mazeHistoryPath);
            //If theres less than 10 files stored simply exports it
            if (dir.GetFiles().Length < 10) { ExportTimedFile(ref mazeHistory, mazeHistoryPath); }
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
                ExportTimedFile(ref mazeHistory, mazeHistoryPath);
            }

        }
        /// <summary>
        /// Exports the maze setting the file name to the current date and time.
        /// </summary>
        /// <param name="mazeHistory">Stack of file paths showing locations of exported files.</param>
        /// <param name="folderPath">Optional folder path extension to store file at.</param>
        public void ExportTimedFile(ref Stack<string> mazeHistory, string folderPath = null )
        {            
            DateTime currentDate = DateTime.Now;
            string time = currentDate.ToString("dd-MM-yyyy-HH-mm-ss");
            if (string.IsNullOrEmpty(folderPath)) { folderPath = GetDefaultFolderPath(); }
            string filePath = Path.Combine(folderPath, time + ".json");
            mazeHistory.Push(filePath);
            //MazeToCSVFile(filePath);
            MazeToJSONFile(filePath);

        }

    }
}
