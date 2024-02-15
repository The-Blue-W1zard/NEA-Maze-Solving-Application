using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Nea_Maze_Solving_Application
{
    /// <summary>
    /// Excecutes Maze Search algorithms on the specified maze, returning the path found from the start to goal cell if possible.
    /// </summary>
    /// <param name="maze">Maze that the algorithms will be excecuted on</param>
    /// <param name="start">Start cell location</param>
    /// <param name="goal">Goal cell location</param>
    internal class MazeSolver(MazeCell[,] maze, Point start, Point goal) : AlgorithmFunctions
    {
        /// <summary>
        /// Excecutes a Dijkstra Search on the maze.
        /// Algorithm starts at start point and searches till it reaches the end point or all cells are explored.
        /// </summary>
        /// <param name="animationSteps">Order that cells where explored so can be animated correctly</param>
        /// <returns>List of points specifying the path found between the two points</returns>
        public List<Point> DijkstraSearch(out List<Point> animationSteps)
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
        /// <summary>
        /// Excecutes Breadth First Search on the maze.
        /// </summary>
        /// <param name="animationSteps">Order that cells where explored so can be animated correctly</param>
        /// <returns>List of points specifying the path found between the two points</returns>
        public List<Point> BreadthFirstSearch(out List<Point> animationSteps)
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
        /// <summary>
        /// Exxcecutes an A Star search on the maze using a euclidian distance heuristic.
        /// </summary>
        /// <param name="animationSteps">Order that cells where explored so can be animated correctly</param>
        /// <returns>List of points specifying the path found between the two pointsz</returns>
        public List<Point> AStarSearch(out List<Point> animationSteps)
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

    }
}
