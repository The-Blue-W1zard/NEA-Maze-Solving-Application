using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Nea_Maze_Solving_Application
{
    /// <summary>
    /// Executes Maze Search algorithms on the specified maze, returning the path found from the start to goal cell if possible.
    /// </summary>
    /// <param name="maze">Maze that the algorithms will be excecuted on.</param>
    /// <param name="start">Location algorithms start exploring from.</param>
    /// <param name="goal">Goal cell algorithms searching for.</param>
    internal class MazeSolver(MazeCell[,] maze, Point start, Point goal) : AlgorithmFunctions   
    {
        /// <summary>
        /// Executes a Dijkstra Search on the maze.
        /// Algorithm starts at start point and searches till it reaches the end point or all cells have been explored.
        /// </summary>
        /// <param name="animationSteps">Order that cells where explored so they can be animated correctly</param>
        /// <returns>List of points specifying the path found between the start and end points</returns>
        public List<Point> DijkstraSearch(out List<Point> animationSteps)
        {
            //Creates priority queue used to store cells to explore next and dictionary to store cells connected to each other
            UpPriQu priorityQueue = new();
            Dictionary<Point, Point> prev = [];
            Point None = new(-1, -1);
            //List to store order cells are explored
            animationSteps = new List<Point>();

            //Itterates through the maze setting each cells priority to a maximum value
            for (int row = 0; row < maze.GetLength(0); row++)
            {
                for (int col = 0; col < maze.GetLength(1); col++)
                {
                    Point point = new(row, col);
                    priorityQueue.Enqueue(point, int.MaxValue);
                    prev.Add(point, None);

                }
            }


            //Changes the priority of the start cell to 0
            priorityQueue.Update(start, 0);
            Point current;

            //While the queue isn't empty...
            while (priorityQueue.Count > 0)
            {
                //Dequeues the cell with the shortest asociated distance and adds it to the list of cells to animate
                current = priorityQueue.Dequeue();
                animationSteps.Add(current);

                //Then checks if the deuqued cell is the goal cell
                if (current == goal)
                {
                    //And ends the exploration if it is
                    break;
                }

                //Else itterates though each neighbouring cell of the dequeued cell
                foreach (Point neighbour in Neighbours(maze, current, 1))
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
            return RecallPath(prev, goal);


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
            Dictionary<Point, Point> prev = [];
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

            Point currentCell;
            //Does...
            do
            {
                //Dequeus a cell from the queue 
                currentCell = queue.Dequeue();
                //And checks wether its the goal cell
                if (currentCell.Equals(goal))
                {
                    //If it is ends the search algorithm
                    break;
                }
                //Else it adds the cell to list of cells to animate
                animationSteps.Add(currentCell);
                //Then itterates through each neighbouring cell
                foreach (Point nextCell in Neighbours(maze, currentCell, 1))
                {
                    //Checking that the hashet of visited cells doesn't contain each cell
                    if (!visited.Contains(nextCell))
                    {
                        //If it doesnt adds the cell to the queue of cells to explore and marks it as visited
                        queue.Enqueue(nextCell);
                        visited.Add(nextCell);
                        //And stores the neighbour cells previous cell as the dequeued cell
                        prev.Add(nextCell, currentCell);
                    }
                }

            }
            while (queue.Count > 0);
            //While the queue isn't empty
            return RecallPath(prev, goal);

        }
        /// <summary>
        /// Executes an A Star search on the maze using a euclidian distance heuristic.
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
            //Creates dictionary to store cell each cell is connected to and list to store order cells are explored to animate algorithm searching
            Dictionary<Point, Point> prev = new();
            animationSteps = new List<Point>();
            Point None = new(-1, -1);

            //Itterates through every cell in the maze setting there gScores to a max value, and fScores to -1.
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

            //Sets the start cells gScore to 0 and calculates the fScore using chosen heuristic
            gScore[start] = 0;
            fScore[start] = EuclidianDistance(start, goal);
            //Adds the start cell to the queue with its asociated fScore
            priorityQueue.Enqueue(start, fScore[start]);

            //WHile the priority queue isn't empty...
            while (priorityQueue.Count > 0)
            {
                //Dequeues the cell with the lowest fScore
                Point current = priorityQueue.Dequeue();
                //And checks whether the chosen cell is the end cell
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
                        if (!priorityQueue.Contains(neighbour))
                        {
                            priorityQueue.Enqueue(neighbour, fScore[neighbour]);
                        }
                    }

                }

            }

            //Console.WriteLine(numExplored);
            return RecallPath(prev, goal);

        }

    }
}
