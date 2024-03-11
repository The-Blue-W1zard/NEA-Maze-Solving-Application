using System.Collections;
using static System.Formats.Asn1.AsnWriter;
using System.Security.Policy;

namespace Nea_Maze_Solving_Application
{
    /// <summary>
    /// Executes Maze Search algorithms on the specified maze, returning the path found from the start to goal cell if possible.
    /// </summary>
    /// <param name="maze">Maze that the algorithms will be executed on.</param>
    /// <param name="start">Location algorithms start exploring from.</param>
    /// <param name="goal">Goal cell algorithms searching for.</param>
    internal class MazeSolver(MazeCell[,] maze, Point start, Point goal) : AlgorithmFunctions   
    {
        /// <summary>
        /// Executes a Dijkstra Search on the maze.
        /// Algorithm starts at start point and searches till it reaches the end point or all cells have been explored.
        /// </summary>
        /// <param name="animationSteps">Order that cells where explored, so they can be animated correctly</param>
        /// <returns>List of points specifying the path found between the start and end points</returns>
        public List<Point> DijkstraSearch(out List<Point> animationSteps)
        {
            //Creates priority queue used to store cells to explore next, and dictionary to store cells connected to each other
            UpPriQu priorityQueue = new();
            Dictionary<Point, Point> prev = SetDefaultPreviousDictionary(maze);
            HashSet<Point> visited = new HashSet<Point>();
            //List to store order cells are explored
            animationSteps = new List<Point>();


            //Iterates through the maze setting each cells priority to a maximum value
            for (int row = 0; row < maze.GetLength(0); row++)
            {
                for (int col = 0; col < maze.GetLength(1); col++)
                {
                    Point point = new(row, col);
                    if (maze[row, col].isWall) { continue; }
                    priorityQueue.Enqueue(point, int.MaxValue);

                }
            }

            //Changes the priority of the start cell to 0
            priorityQueue.Update(start, 0);

            //While the queue isn't empty...
            while (priorityQueue.Count > 0)
            {
                //Dequeues the cell with the shortest associated distance and adds it to the list of cells to animate
                Point current = priorityQueue.Dequeue();
                animationSteps.Add(current);
                visited.Add(current);

                //Then checks if the dequeued cell is the goal cell
                if (current == goal)
                {
                    //And ends the exploration if it is
                    break;
                }

                //Else iterates though each neighbouring cell of the dequeued cell
                foreach (Point neighbour in Neighbours(maze, current, 1,visited))
                {
                    //Checks that the priority queue contains the neighbour cell
                    if (!priorityQueue.Contains(neighbour)) { continue; }
                    //Gets the shortest dist to the neighbour by adding one to the shortest distance of the dequeued cell
                    int altDist = priorityQueue.GetValue(current) + 1;
                    //Compares the new "shortest" distance to the neighbour cells current stored shortest distance
                    if (altDist < priorityQueue.GetValue(neighbour))
                    {
                        //If the new distance is shorter, updates the neighbour cell with the new distance
                        priorityQueue.Update(neighbour, altDist);
                        //And stores the neighbour cells previous cell as the dequeued cell
                        prev[neighbour] = current;
                    }
                }
                //Loops until end cell found or all cells have been explored
            }
            //Returns the path from goal to start using the prev dictionary
            return RecallPath(maze,prev, goal);


        }
        /// <summary>
        /// Executes Breadth First Search on the maze.
        /// Algorithm starts at start point and searches till it reaches the end point or all cells have been explored.
        /// </summary>
        /// <param name="animationSteps">Order that cells where explored so can be animated correctly.</param>
        /// <returns>List of points specifying the path found between the start and end points.</returns>
        public List<Point> BreadthFirstSearch(out List<Point> animationSteps)
        {
            //Creates a queue to store which cells to explore next and a dictionary to store cells that are connected to each other 
            Queue<Point> queue = new();
            Dictionary<Point, Point> prev = SetDefaultPreviousDictionary(maze);
            //Hashset created to store cells that have been explored
            HashSet<Point> visited = new HashSet<Point>();
            //List stores order cells are explored to animate algorithm searching
            animationSteps = new List<Point>();
            Point none = new(-1, -1);

            //Adds the start cell to the list of visited cells and to the queue.
            visited.Add(start);
            queue.Enqueue(start);
            //Sets the cell connected to the start cell to none.
            prev[start] = none;

            //Does...
            do
            {
                //Dequeues a cell from the queue 
                var currentCell = queue.Dequeue();
                //And checks whether it's the goal cell
                if (currentCell.Equals(goal))
                {
                    //If it is ends the search algorithm
                    break;
                }
                //Else it adds the cell to list of cells to animate
                animationSteps.Add(currentCell);
                //Then iterates through each neighbouring cell
                foreach (Point nextCell in Neighbours(maze, currentCell, 1))
                {
                    //Checking that the hashset of visited cells doesn't contain each cell
                    if (!visited.Contains(nextCell))
                    {
                        //If it doesn't, adds the cell to the queue of cells to explore and marks it as visited
                        queue.Enqueue(nextCell);
                        visited.Add(nextCell);
                        //And stores the neighbour cells previous cell as the dequeued cell
                        prev[nextCell] = currentCell;
                    }
                }

            }
            while (queue.Count > 0);
            //While the queue isn't empty
            //Then returns the path from goal to start using the prev dictionary
            return RecallPath(maze, prev, goal);

        }
        /// <summary>
        /// Executes an A Star search on the maze using a euclidean distance heuristic.
        /// </summary>
        /// <param name="animationSteps">Order that cells where explored so can be animated correctly.</param>
        /// <returns>List of points specifying the path found between the start and end points</returns>
        public List<Point> AStarSearch(out List<Point> animationSteps)
        {
            //Creates a priority queue to store which cells to explore next, as well as dictionaries to store every cells fScore and gScore
            UpPriQu priorityQueue = new UpPriQu();
            //gScore = length of cheapest path from start cell to chosen cell currently known
            Dictionary<Point, int> gScore = new();
            //fScore = Current best guess of length of path from chosen cell to end cell
            Dictionary<Point, int> fScore = new();
            //Creates dictionary to store cell each cell is connected to and hashset to store order cells are explored to animate algorithm searching
            Dictionary<Point, Point> prev = SetDefaultPreviousDictionary(maze);
            animationSteps = new List<Point>();
            Point none = new(-1, -1);

            //Iterates through every cell in the maze setting there gScores to a max value, and fScores to -1.
            for (int row = 0; row < maze.GetLength(0); row++)
            {
                for (int col = 0; col < maze.GetLength(1); col++)
                {
                    Point point = new(row, col);
                    gScore.Add(point, int.MaxValue);
                    fScore.Add(point, -1);
                }
            }

            //Sets the start cells gScore to 0 and calculates the fScore using chosen heuristic
            gScore[start] = 0;
            fScore[start] = EuclideanDistance(start, goal);
            //Adds the start cell to the queue with its associated fScore
            priorityQueue.Enqueue(start, fScore[start]);

            //While the priority queue isn't empty...
            while (priorityQueue.Count > 0)
            {
                //Dequeues the cell with the lowest fScore
                Point current = priorityQueue.Dequeue();
                //And checks whether the chosen cell is the end cell
                if (current == goal) { break; }
                //If it isn't the cell is added to list,, of cells to animate
                animationSteps.Add(current);

                //Then the algorithm iterates through each neighbouring cell of the dequeued cell
                foreach (Point neighbour in Neighbours(maze, current, 1))
                {
                    //Calculating a new gScore for the neighbour from the gScore of the current cell
                    int tentative_gScore = gScore[current] + 1;
                    //If the new gScore is less than the neighbour cells previously stored gScore...
                    if (tentative_gScore < gScore[neighbour])
                    {
                        //The algorithm sets the new previous cell of the neighbour cell to be the dequeued cell
                        prev[neighbour] = current;
                        //And updates the neighbours gScore to be the newly calculated one
                        gScore[neighbour] = tentative_gScore;
                        //And updates the neighbours fScore from the sum of the newly calculated gScore
                        //and the distance determined by the heuristic from the neighbour cell to the end cell
                        fScore[neighbour] = tentative_gScore + EuclideanDistance(neighbour, goal);
                        //It then checks if the queue doesn't contain the neighbour cell
                        if (!priorityQueue.Contains(neighbour))
                        {
                            //If it doesn't it is added to the queue with its priority being the associated fScore
                            priorityQueue.Enqueue(neighbour, fScore[neighbour]);
                        }
                    }

                }

            }
            //This loop repeats till the priority queue is empty or the end cell has been found 
            //Then returns the path from goal to start using the prev dictionary

            return RecallPath(maze, prev, goal);

        }

        /// <summary>
        /// Private DFS solver class used to execute a recursive DFS on the maze. 
        /// </summary>
        private class DepthFirstSearchSolver : AlgorithmFunctions
        {
            private HashSet<Point> visited;
            private Dictionary<Point, Point> prev;
            private Point goal;
            private Point start;
            private MazeCell[,] maze;

            /// <summary>
            /// Class constructor initialises variables for DFS solver.
            /// </summary>
            /// <param name="maze">Maze being searched</param>
            /// <param name="start"></param>
            /// <param name="goal"></param>
            public DepthFirstSearchSolver(MazeCell[,] maze, Point start, Point goal)
            {
                this.maze = maze;
                this.start = start;
                this.goal = goal;
                visited = new HashSet<Point>();
                prev = SetDefaultPreviousDictionary(maze);
            }

            /// <summary>
            /// Private recursive function to execute depth first search on the maze
            /// </summary> 
            /// <param name="current">Cell call is searching from.</param>
            /// <returns>Boolean value signifying if end has been found yet.</returns>
            private bool RecursiveDepthFirstSearch(Point current)
            {
                //Adds the current cell to the hash set of visited cells
                visited.Add(current);

                //Checks that the current cell isn't the goal cell
                if (current == goal) { return true; }

                //If it isn't iterates through each neighbour of the current selected cell
                foreach (Point nextCell in Neighbours(maze, current, 1, visited))
                {
                    //Setting the neighbour cells previous cell to be the current cell
                    prev[nextCell] = current;

                    //Then recursively calls itself to explore the neighbouring cell
                    if (RecursiveDepthFirstSearch(nextCell))
                    {
                        //If call returns true goal has been found so returns true to previous recursive call
                        return true;
                    }
                }
                //If goal hasn't been found returns false
                return false;
            }

            /// <summary>
            /// Public function within class used to start executing DFS on the maze.
            /// </summary>
            /// <param name="animationSteps">Order that cells where explored so can be animated correctly.</param>
            /// <returns>List of points specifying the path found between the start and end points.</returns>
            public List<Point> ExecuteDepthFirstSearch(out List<Point> animationSteps)
            {
                RecursiveDepthFirstSearch(start);
                prev[start] = new Point(-1, -1);
                animationSteps = visited.ToList();
                return RecallPath(maze, prev, goal);
            }
        }

        /// <summary>
        /// Public function that creates a DFS object and executes it on the maze.
        /// </summary>
        /// <param name="animationSteps">Order that cells where explored so can be animated correctly.</param>
        /// <returns>List of points specifying the path found between the start and end points.</returns>
        public List<Point> DepthFirstSearch(out List<Point> animationSteps)
        {
            DepthFirstSearchSolver DFSSolver = new DepthFirstSearchSolver(maze, start, goal);
            return DFSSolver.ExecuteDepthFirstSearch(out animationSteps);
            
        }
        
    }
}
