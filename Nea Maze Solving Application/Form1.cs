namespace Nea_Maze_Solving_Application
{
    public partial class Form1 : Form
    {
        MazeCell[,] maze = new MazeCell[30, 50];
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

        private void RandomizedDFS( MazeCell[,] maze, Point start)
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
                next = randomUnvisitedNeighbour( maze, current, ref visited);
                //Console.WriteLine(next);
                if (next == none)
                {
                    while (queue.Count > 0)
                    {
                        next = queue.Dequeue();
                        //Console.WriteLine(next);
                        if (NeighboursMazeGen( maze, next, ref visited).Count() > 0)
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

                ConnectCells( maze, current, next);
                current = next;
                queue.Enqueue(current);
                visited.Add(current);

                //foreach (Point p in queue) { Console.Write(p); }
                //Console.WriteLine();

            }
        }

        private Point randomUnvisitedNeighbour( MazeCell[,] maze, Point vertex,  ref List<Point> visited)
        {
            Random rand = new();
            Point next = new(-1, -1);
            Point none = new(-1, -1);
            List<Point> neighbours = NeighboursMazeGen( maze, vertex, ref visited);
            while (neighbours.Count > 0)
            {
                next = neighbours[rand.Next(neighbours.Count)];
                neighbours.Remove(next);
                if (!visited.Contains(next)) { break; }
                next = none;
            }
            return next;

        }

        private List<Point> NeighboursMazeGen( MazeCell[,] maze, Point current, ref List<Point> visited)
        {
            int[] checksRows = [0, 0, 2, -2];
            int[] checksCols = [2, -2, 0, 0];
            int row = current.X;
            int col = current.Y;
            List<Point> neighbours = [];

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

        private void ConnectCells( MazeCell[,] maze, Point first, Point second)
        {
            int row = (first.X + second.X) / 2;
            int col = (first.Y + second.Y) / 2;
            if (maze[row, col].isWall) { maze[row, col].ToggleWall(); }
        }

        private void generateMaze_Click(object sender, EventArgs e)
        {
            GenerateRowsCols(maze);
            RandomizedDFS( maze, startCell);
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
    }
}
