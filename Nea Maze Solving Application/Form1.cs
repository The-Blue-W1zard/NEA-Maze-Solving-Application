using System.Diagnostics;

namespace Nea_Maze_Solving_Application
{
    public partial class Form1 : Form
    {
        MazeCell[,] maze = new MazeCell[30, 50];
        MazeCell[,] backupMaze = new MazeCell[30, 50];
        Point startCell = new Point(0, 0);
        Point endCell = new Point(28, 48);
        public Form1()
        {
            InitializeComponent();
            CreateBlankMaze();
        }

        private void CreateBlankMaze()
        {
            for (int row = 0; row < maze.GetLength(0); row++)
            {
                for (int col = 0; col < maze.GetLength(1); col++)
                {
                    maze[row, col] = new MazeCell();
                    maze[row, col].location = new Point(row, col);
                    maze[row, col].Intialize();
                    Controls.Add(maze[row, col].btn);
                    Invalidate();
                }
            }
            maze[startCell.X, startCell.Y].ToggleStartCell();
            maze[endCell.X, endCell.Y].ToggleEndCell();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //hello
        }

        private List<Point> DijkstraVersion2(MazeCell[,] maze, Point start, Point goal, out List<Point> animationSteps)
        {
            UpPriQu Q = new();
            Dictionary<Point, Point> prev = [];
            Point None = new(-1, -1);
            int numExplored = 0;
            animationSteps = new List<Point>();

            for (int row = 0; row < maze.GetLength(0); row++)
            {
                for (int col = 0; col < maze.GetLength(1); col++)
                {
                    Point point = new(row, col);
                    Q.Enqueue(point, int.MaxValue);
                    prev.Add(point, None);

                }
            }

            Q.Update(start, 0);
            Point current;
            while (Q.Count > 0)
            {
                numExplored++;
                current = Q.Dequeue();
                animationSteps.Add(current);


                if (current == goal)
                {
                    break;
                }

                foreach (Point neighbour in Neighbours(maze, current, 1))
                {
                    if (!Q.Contains(neighbour)) { continue; }
                    int altDist = Q.Distance(current) + 1;
                    if (altDist < Q.Distance(neighbour))
                    {
                        Q.Update(neighbour, altDist);
                        prev[neighbour] = current;
                    }
                }
            }
            //Console.WriteLine(numExplored);
            return RecallPath(prev, goal);
        }

        private List<Point> BreadthFirstSearch(MazeCell[,] maze, Point start, Point goal, out List<Point> animationSteps)


        {
            Queue<Point> Q = new();
            Dictionary<Point, Point> cameFrom = [];
            List<Point> visitedCells = [];
            animationSteps = new List<Point>();
            Point none = new(-1, -1);

            visitedCells.Add(start);
            Q.Enqueue(start);
            cameFrom[start] = none;

            Point currentCell;
            int numExplored = 0;
            do
            {
                numExplored++;
                currentCell = Q.Dequeue();
                if (currentCell.Equals(goal))
                {
                    //Console.WriteLine("Reached End");
                    break;
                }
                animationSteps.Add(currentCell);
                foreach (Point nextCell in Neighbours(maze, currentCell, 1))
                {
                    if (!visitedCells.Contains(nextCell))
                    {
                        Q.Enqueue(nextCell);
                        visitedCells.Add(nextCell);
                        cameFrom.Add(nextCell, currentCell);
                    }
                }

            }
            while (Q.Count > 0);

            //Console.WriteLine(numExplored);
            return RecallPath(cameFrom, goal);

        }

        private List<Point> AStarSearch(MazeCell[,] maze, Point start, Point goal, out List<Point> animationSteps)
        {
            //This rendition of Updatable priority queue has the point then the fscore as thats what have to get minimum from
            UpPriQu Q = new UpPriQu();
            Dictionary<Point, int> gScore = new();
            Dictionary<Point, int> fScore = new();
            Dictionary<Point, Point> prev = new();
            animationSteps = new List<Point>();
            Point None = new(-1, -1);
            int numExplored = 0;

            for (int row = 0; row < maze.GetLength(0); row++)
            {
                for (int col = 0; col < maze.GetLength(1); col++)
                {
                    Point point = new(row, col);
                    gScore.Add(point, int.MaxValue);
                    fScore.Add(point, -1);
                    prev.Add(point, None);
                }
            }
            gScore[start] = 0;
            fScore[start] = EuclidianDistance(start, goal);
            Q.Enqueue(start, fScore[start]);

            while (Q.Count > 0)
            {
                numExplored++;
                Point current = Q.Dequeue();
                if (current == goal) { break; }
                animationSteps.Add(current);

                foreach (Point neighbour in Neighbours(maze, current, 1))
                {
                    int tentative_gScore = gScore[current] + 1;
                    if (tentative_gScore < gScore[neighbour])
                    {
                        prev[neighbour] = current;
                        gScore[neighbour] = tentative_gScore;
                        fScore[neighbour] = tentative_gScore + EuclidianDistance(current, goal);
                        if (!Q.Contains(neighbour))
                        {
                            Q.Enqueue(neighbour, fScore[neighbour]);
                        }
                    }

                }

            }

            //Console.WriteLine(numExplored);
            return RecallPath(prev, goal);

        }

        private void RandomizedDFS(MazeCell[,] maze, Point start)
        {
            Queue<Point> queue = new Queue<Point>();
            //Stack<Point> stack = new Stack<Point>();
            List<Point> visited = new();
            Point current = start;
            Point next = new();
            Point none = new(-1, -1);
            queue.Enqueue(current);
            visited.Add(current);

            while (queue.Count > 0)
            {
                next = randomUnvisitedNeighbour(maze, current, ref visited);
                //Console.WriteLine(next);
                if (next == none)
                {
                    while (queue.Count > 0)
                    {
                        next = queue.Dequeue();
                        //Console.WriteLine(next);
                        if (Neighbours(maze, next, 2, visited).Count() > 0)
                        {
                            //Console.WriteLine(NeighboursMazeGen(maze, next, visited).Count());
                            current = next;
                            break;
                        }
                    }
                    if (0 == queue.Count)
                    {
                        break;
                    }

                }

                ConnectCells(maze, current, next);
                current = next;
                queue.Enqueue(current);
                visited.Add(current);

                //foreach (Point p in queue) { Console.Write(p); }
                //Console.WriteLine();

            }
        }
        private Point randomUnvisitedNeighbour(MazeCell[,] maze, Point vertex, ref List<Point> visited)
        {
            Random rand = new();
            Point next = new(-1, -1);
            Point none = new(-1, -1);
            List<Point> neighbours = Neighbours(maze, vertex, 2, visited);
            while (neighbours.Count > 0)
            {
                next = neighbours[rand.Next(neighbours.Count)];
                neighbours.Remove(next);
                if (!visited.Contains(next)) { break; }
                next = none;
            }
            return next;

        }
        private List<Point> Neighbours(MazeCell[,] maze, Point current, int dist, List<Point> visited = default)
        {
            int[] checksRows = [0, 0, dist, -dist];
            int[] checksCols = [dist, -dist, 0, 0];
            int row = current.X;
            int col = current.Y;
            List<Point> neighbours = [];

            if (visited == default) { visited = new List<Point>(); }

            for (int t = 0; t < 4; t++)
            {
                try
                {
                    if (!maze[row + checksRows[t], col + checksCols[t]].isWall && !visited.Contains(new Point(row + checksRows[t], col + checksCols[t])))
                    {
                        neighbours.Add(new Point(row + checksRows[t], col + checksCols[t]));
                    }

                }
                catch { }
            }
            return neighbours;
        }
        public int EuclidianDistance(Point start, Point goal)
        {
            double h = Math.Sqrt(Math.Pow(start.X - goal.X, 2) + Math.Pow(start.Y - goal.Y, 2));
            //return (int)h;
            return (int)Math.Round(h);
        }
        public List<Point> RecallPath(Dictionary<Point, Point> prev, Point goal)
        {
            Point None = new(-1, -1);
            Point current = goal;
            List<Point> path = [];
            path.Clear();

            if (prev[goal] == None)
            {
                Console.WriteLine("No Path Found");
                //when integrated with UI will make this display pop up message
            }

            while (current != None)
            {
                path.Add(current);
                current = prev[current];
            }
            path.Reverse();
            return path;
        }
        private void UpdateMaze(MazeCell[,] maze, List<Point> path)
        {
            ////foreach (Point p in path) { Console.WriteLine(p); }
            //Console.WriteLine("Before Update");
            //OutputMaze(maze);
            //MazeCell[,] tempMaze = new MazeCell[30, 50];
            //Array.Copy(maze, tempMaze, maze.Length);

            for (int i = 0; i < path.Count; i++)
            {
                try
                {
                    if (maze[path[i].X, path[i].Y].isWall) { Console.WriteLine("mucked up here"); continue; }
                    if (maze[path[i].X, path[i].Y].isStartCell || maze[path[i].X, path[i].Y].isEndCell) { continue; }
                    maze[path[i].X, path[i].Y].TogglePath();
                    Thread.Sleep(1);
                    Application.DoEvents();
                }
                catch { Console.WriteLine("coordinates bad"); }
            }
            //Console.WriteLine("After Update");
            //OutputMaze(maze);


        }
        private void AnimateMaze(MazeCell[,] maze, List<Point> path)
        {
            for (int i = 0; i < path.Count; i++)
            {
                try
                {
                    if (maze[path[i].X, path[i].Y].isWall) { Console.WriteLine("mucked up here"); continue; }
                    if (maze[path[i].X, path[i].Y].isStartCell || maze[path[i].X, path[i].Y].isEndCell) { continue; }
                    maze[path[i].X, path[i].Y].ToggleExplored();
                    Thread.Sleep(10);
                    Application.DoEvents();
                }
                catch { Console.WriteLine("coordinates bad"); }
            }
            //Console.WriteLine("After Update");
            //OutputMaze(maze);


        }
        private void ConnectCells(MazeCell[,] maze, Point first, Point second)
        {
            int row = (first.X + second.X) / 2;
            int col = (first.Y + second.Y) / 2;
            if (maze[row, col].isWall) { maze[row, col].ToggleWall(); }
        }
        private void GenerateRowsCols(MazeCell[,] maze)
        {
            //int r = maze.GetLength(0);
            //int c = maze.GetLength(1);

            for (int r = 0; r < maze.GetLength(0); r++)
            {
                for (int c = 0; c < maze.GetLength(1); c++)
                {
                    if (r % 2 == 1) { maze[r, c].ToggleWall(); }
                    else if (c % 2 == 1) { maze[r, c].ToggleWall(); }
                }
            }
        }
        private void AlgorithmPrequisites()
        {
            //Things to run before actual algorithms
            Array.Copy(maze, backupMaze, maze.Length);
        }
        private void ClearMaze()
        {
            foreach(MazeCell cell in maze) {
            if (cell.isWall) {cell.ToggleWall(); }
            if (cell.isOnPath) { cell.TogglePath(); }
            if (cell.isExplored) { cell.ToggleExplored(); }
            if (cell.isStartCell || cell.isEndCell) { continue; }
            }
            
            //Invalidate();
        }
        private void generateMaze_Click(object sender, EventArgs e)
        {
            AlgorithmPrequisites();
            GenerateRowsCols(maze);
            RandomizedDFS(maze, startCell);
        }
        private void Dijkstra_Click(object sender, EventArgs e)
        {
            AlgorithmPrequisites();
            List<Point> animationSteps;
            List<Point> path = DijkstraVersion2(maze, startCell, endCell, out animationSteps);
            AnimateMaze(maze, animationSteps);
            UpdateMaze(maze, path);
        }

        private void AStar_Click(object sender, EventArgs e)
        {
            AlgorithmPrequisites();
            List<Point> animationSteps;
            List<Point> path = AStarSearch(maze, startCell, endCell, out animationSteps);
            AnimateMaze(maze, animationSteps);
            UpdateMaze(maze, path);
        }

        private void BreadthFirst_Click(object sender, EventArgs e)
        {
            AlgorithmPrequisites();
            List<Point> animationSteps;
            List<Point> path = BreadthFirstSearch(maze, startCell, endCell, out animationSteps);
            AnimateMaze(maze, animationSteps);
            UpdateMaze(maze, path);
        }

        private void ReloadMaze_Click(object sender, EventArgs e)
        {
            for(int r = 0; r < maze.GetLength(0);r++)
            {
                for (int c = 0; c < maze.GetLength(1); c++)
                {
                    maze[r,c] = backupMaze[r,c];
                    Debug.WriteLine(maze[r,c].isWall);
                }
            }
            Invalidate();   
        }

        private void Clear_Click(object sender, EventArgs e)
        {
            AlgorithmPrequisites();
            ClearMaze();
        }
    }
}
