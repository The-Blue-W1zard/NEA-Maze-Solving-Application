using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nea_Maze_Solving_Application
{
    internal class UpPriQu
    {
        private Dictionary<Point, int> sortedDict;
        private int numItems;
        private int capacity;
        private Point None = new Point(-1, -1);

        /// <summary>
        /// Returns number of items in the priority queue 
        /// </summary>
        public int Count { get { return numItems; } }

        /// <summary>
        /// Sets the default capacity to 1500 if no other value specified
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
            numItems = 0;
        }

        /// <summary>
        /// Empties the dictionary
        /// </summary>
        public void Clear() { sortedDict.Clear(); }

        /// <summary>
        /// Adds an item to the dictionary then sorts it so that item is in the correct position.
        /// </summary>
        /// <param name="point">Coordinates of maze cell</param>
        /// <param name="value">Value asociated with maze cell</param>
        public void Enqueue(Point point, int value)
        {
            sortedDict.Add(point, value);
            numItems++;
            //Uses LinQ to order items in the dictionary
            var tempDict = from entry in sortedDict orderby entry.Value ascending select entry;
            sortedDict = tempDict.ToDictionary();
        }

        /// <summary>
        /// Changes the value of specified item and resorts the list so correctly ordered.
        /// </summary>
        /// <param name="point">Coordinates of maze cell changing</param>
        /// <param name="newValue">New value to replace old</param>
        public void Update(Point point, int newValue)
        {
            sortedDict[point] = newValue;
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
            if (sortedDict.ContainsKey(point)) return true;
            return false;
        }

        /// <summary>
        /// Gets value asociated with coordinates of maze cell 
        /// </summary>
        /// <param name="point">Maze cell trying to get value from</param>
        /// <returns>Value asociated with maze cell</returns>
        public int Distance(Point point)
        {
            if (sortedDict.TryGetValue(point, out int result)) return result;
            return -1;
        }

        /// <summary>
        /// Dequeues coordinates of maze cell with shortest asociated value.
        /// </summary>
        /// <returns>Location of maze cell</returns>
        public virtual Point Dequeue()
        {
            Point shortest = new Point();
            foreach (Point point in sortedDict.Keys)
            {
                shortest = point;
                sortedDict.Remove(point);
                break;
            }

            numItems--;
            return shortest;



        }
    }
}
