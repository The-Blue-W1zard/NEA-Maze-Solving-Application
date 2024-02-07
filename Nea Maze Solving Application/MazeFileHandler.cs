using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nea_Maze_Solving_Application
{
    internal class MazeFileHandler(MazeCell[,] maze)
    {
        public void MazeToCSVFile(string filePath)
        {
            //var csv = new StringBuilder();
            //string path = @"C:\Users\josep\Downloads\Output2.csv";#
            //MessageBox.Show(filePath);
            using (var w = new StreamWriter(filePath))
            {
                for (int r = 0; r < maze.GetLength(0); r++)
                {
                    for (int c = 0; c < maze.GetLength(1); c++)
                    {
                        MazeCell tempMazeCell = maze[r, c];
                        var location = tempMazeCell.location;
                        var isWall = tempMazeCell.isWall;
                        var isStartCell = tempMazeCell.isStartCell;
                        var isEndCell = tempMazeCell.isEndCell;
                        var isOnPath = tempMazeCell.isOnPath;
                        var isExplored = tempMazeCell.isExplored;
                        var newline = $"{location.X},{location.Y},{isWall},{isStartCell},{isEndCell},{isOnPath},{isExplored}";
                        //Debug.WriteLine(newline);   
                        w.WriteLine(newline);
                        w.Flush();

                    }
                }
            }
            MessageBox.Show("Finished Exporting");
            //File.WriteAllText(path, csv.ToString());
        }
        public void CSVToMaze(string filePath)
        {

            using (var r = new StreamReader(File.OpenRead(filePath)))
            {

                while (!r.EndOfStream)
                {
                    var line = r.ReadLine();
                    var splitLine = line.Replace(" ", string.Empty).Split(',');
                    Point location = new Point(Convert.ToInt32(splitLine[0]), Convert.ToInt32(splitLine[1]));
                    bool isWall = checkBool(splitLine[2]);
                    bool isStartCell = checkBool(splitLine[3]);
                    bool isEndCell = checkBool(splitLine[4]);
                    bool isOnPath = checkBool(splitLine[5]);
                    bool isExplored = checkBool(splitLine[6]);
                    maze[location.X, location.Y].RefreshCell(isWall, isStartCell, isEndCell, isOnPath, isExplored);


                }
            }
        }
        public void OpenFileExplorer(string initialDirectory)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                InitialDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), initialDirectory),
                Title = "Select Maze File",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "csv",
                Filter = "CSV Files (*.csv)|*.csv",
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true,

            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                CSVToMaze(ofd.FileName);
            }
        }
        public void GenerateAppData()
        {
            //string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            //string appDataFolder = Path.Combine(localAppData, "NEAMazeSolver");
            string appDataFolder = GetDefaultFolderPath();
            //string mazeHistoryFolder = Path.Combine(appDataFolder, "MazeHistory");
            string mazeHistoryFolder = GetDefaultFolderPath("MazeHistory");
            string UndoQueueFolder = GetDefaultFolderPath("UndoQueue");

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
        public string GetDefaultFolderPath(string folderExtension = null)
        {
            string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string appDataFolder = Path.Combine(localAppData, "NEAMazeSolver");
            try { return Path.Combine(appDataFolder, folderExtension); }
            catch { return appDataFolder; }
        }
        public void UpdateGeneratedMazeHistory(ref Stack<string> mazeHistory)
        {
            string mazeHistoryPath = GetDefaultFolderPath("MazeHistory");
            DirectoryInfo dir = new DirectoryInfo(mazeHistoryPath);
            if (dir.GetFiles().Length < 10) { ExportTimedFile(ref mazeHistory, mazeHistoryPath); }
            else
            {
                FileInfo oldestFile = new FileInfo(@"C:\NotRealFile.txt");
                foreach (FileInfo file in dir.GetFiles())
                {
                    if (file.LastWriteTime.CompareTo(oldestFile.LastWriteTime) > 0) { oldestFile = file; }
                }
                Debug.WriteLine(oldestFile.FullName);
                oldestFile.Delete();
                ExportTimedFile(ref mazeHistory, mazeHistoryPath);
            }

        }
        public void ExportTimedFile(ref Stack<string> mazeHistory, string folderPath = null )
        {
            DateTime currentDate = DateTime.Now;
            string time = currentDate.ToString("dd-MM-yyyy-HH-mm-ss");
            if (string.IsNullOrEmpty(folderPath)) { folderPath = GetDefaultFolderPath(); }
            string filePath = Path.Combine(folderPath, time + ".csv");
            mazeHistory.Push(filePath);
            MazeToCSVFile(filePath);

        }
        private bool checkBool(string str)
        {
            if (str == "True") { return true; }
            return false;
        }

    }
}
