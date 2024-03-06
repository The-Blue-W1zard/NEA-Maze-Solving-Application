using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nea_Maze_Solving_Application
{
    internal class UpPriQu
    {
        private Dictionary<Point, int> sortedDict;
        private int capacity;

        /// <summary>
        /// Returns number of items in the priority queue 
        /// </summary>
        public int Count { get { return sortedDict.Count; } }

        /// <summary>
        /// Sets the default capacity to 1500 if no other value specified, as this is the capacity the application is designed to hold.
        /// </summary>
        public UpPriQu() : this(capacity: 1500) { }

        /// <summary>
        /// Initialises the dictionary and sets the capacity and number of items
        /// </summary>
        /// <param name="capacity">Max number of key value pairs in the dictionary</param>
        public UpPriQu(int capacity)
        {
            sortedDict = new Dictionary<Point, int>(capacity);
            this.capacity = capacity;
        }

        /// <summary>
        /// Empties the dictionary
        /// </summary>
        public void Clear() { sortedDict.Clear(); }

        /// <summary>
        /// Adds an item to the dictionary then sorts it so that item is in the correct position.
        /// </summary>
        /// <param name="point">Coordinates of maze cell</param>
        /// <param name="value">Value associated with maze cell</param>
        public void Enqueue(Point point, int value)
        {
            if (sortedDict.Count <= capacity)
            {
                sortedDict.Add(point, value);
                //Uses LinQ to order items in the dictionary by associated value
                var tempDict = from entry in sortedDict orderby entry.Value ascending select entry;
                sortedDict = tempDict.ToDictionary();
            }
            else { Debug.Write("Priority Queue Full"); }
           
        }

        /// <summary>
        /// Changes the value of specified item and resorts the list so correctly ordered.
        /// </summary>
        /// <param name="point">Coordinates of maze cell changing</param>
        /// <param name="newValue">New value to replace old</param>
        public void Update(Point point, int newValue)
        {
            sortedDict[point] = newValue;
            //Uses LinQ to order items in the dictionary by associated value
            var tempDict = from entry in sortedDict orderby entry.Value ascending select entry;
            sortedDict = tempDict.ToDictionary();
        }

        /// <summary>
        /// Checks if the dictionary contains a maze cell
        /// </summary>
        /// <param name="point">Coordinates of maze cell checking</param>
        /// <returns>Bool of whether dictionary contains cell</returns>
        public bool Contains(Point point)
        {
            return sortedDict.ContainsKey(point);
        }

        /// <summary>
        /// Gets value associated with coordinates of maze cell 
        /// </summary>
        /// <param name="point">Maze cell trying to get value from</param>
        /// <returns>Value associated with maze cell</returns>
        public int GetValue(Point point)
        {
            return sortedDict.GetValueOrDefault(point, -1);
        }

        /// <summary>
        /// Dequeues coordinates of maze cell with the shortest associated value.
        /// </summary>
        /// <returns>Location of maze cell</returns>
        public Point Dequeue()
        {
            //Returns the first item in the dictionary, found by using foreach loop which does one iteration
            Point shortest = new Point();
            foreach (Point point in sortedDict.Keys)
            {
                shortest = point;
                sortedDict.Remove(point);
                break;
            }

            return shortest;

        }
    }
}
