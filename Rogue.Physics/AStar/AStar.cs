namespace Rogue.Physics
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    /// <summary>
    /// Generic Implementation of the A* algorithm.
    /// </summary>
    public class AStar
    {

        #region ProfilingCollection
        // Profiling Info
        static public bool CollectProfiling = true;
        static public Dictionary<string, float> LastRunProfilingInfo = new Dictionary<string, float>();
        //---------------
        #endregion

        /// <summary>
        /// Finds the optimal path between start and destionation TNode.
        /// </summary>
        /// <returns>The path.</returns>
        /// <param name="start">Starting Node.</param>
        /// <param name="destination">Destination Node.</param>
        /// <param name="distance">Function to compute distance beween nodes.</param>
        /// <param name="estimate">Function to estimate the remaining cost for the goal.</param>
        /// <typeparam name="TNode">Any class implement IHasNeighbours.</typeparam>
        static public Path<TNode> FindPath<TNode>(
            IHasNeighbours<TNode> dataStructure,
            TNode start,
            TNode destination,
            double speed,
            Func<TNode, TNode, double> distance,
            Func<TNode, double> estimate,
            Func<TNode, TNode, bool> richDestination)
        {
            // Profiling Information
            float expandedNodes = 0;
            float elapsedTime = 0;
            Stopwatch st = new Stopwatch();
            //----------------------
            var closed = new HashSet<TNode>();
            var queue = new PriorityQueue<double, Path<TNode>>();
            queue.Enqueue(0, new Path<TNode>(start));
            if (CollectProfiling) st.Start();
            while (!queue.IsEmpty)
            {
                var path = queue.Dequeue();
                if (closed.Contains(path.LastStep))
                    continue;
                if (richDestination(path.LastStep, destination))
                {
                    if (CollectProfiling)
                    {
                        st.Stop();
                        LastRunProfilingInfo["Expanded Nodes"] = expandedNodes;
                        LastRunProfilingInfo["Elapsed Time"] = st.ElapsedTicks;
                    }
                    return path;
                }
                closed.Add(path.LastStep);
                expandedNodes++;

                foreach (TNode n in dataStructure.Neighbours(path.LastStep, speed,destination))
                {
                    double d = distance(path.LastStep, n);
                    if (richDestination(n, destination))
                        d = 0;
                    var newPath = path.AddStep(n, d);
                    queue.Enqueue(newPath.TotalCost + estimate(n), newPath);
                }
            }
            return null;
        }

    }

    public class PriorityQueue<TPriority, TItem>
    {
        readonly SortedDictionary<TPriority, Queue<TItem>> _subqueues;

        public PriorityQueue(IComparer<TPriority> priorityComparer)
        {
            _subqueues = new SortedDictionary<TPriority, Queue<TItem>>(priorityComparer);
        }

        public PriorityQueue() : this(Comparer<TPriority>.Default) { }

        public bool HasItems
        {
            get { return _subqueues.Any(); }
        }

        public bool IsEmpty => Count <= 0;

        public int Count
        {
            get { return _subqueues.Sum(q => q.Value.Count); }
        }

        public void Enqueue(TPriority priority, TItem item)
        {
            if (!_subqueues.ContainsKey(priority))
            {
                AddQueueOfPriority(priority);
            }

            _subqueues[priority].Enqueue(item);
        }

        private void AddQueueOfPriority(TPriority priority)
        {
            _subqueues.Add(priority, new Queue<TItem>());
        }

        public TItem Dequeue()
        {
            if (_subqueues.Any())
                return DequeueFromHighPriorityQueue();
            else
                throw new InvalidOperationException("The queue is empty");
        }

        private TItem DequeueFromHighPriorityQueue()
        {
            KeyValuePair<TPriority, Queue<TItem>> first = _subqueues.First();
            TItem nextItem = first.Value.Dequeue();
            if (!first.Value.Any())
            {
                _subqueues.Remove(first.Key);
            }
            return nextItem;
        }

        public TItem Peek()
        {
            if (HasItems)
                return _subqueues.First().Value.Peek();
            else
                throw new InvalidOperationException("The queue is empty");
        }
    }

    /// <summary>
    /// Interface that rapresent data structures that has the ability to find node neighbours.
    /// </summary>
    public interface IHasNeighbours<T>
    {
        /// <summary>
        /// Gets the neighbours of the instance.
        /// </summary>
        /// <value>The neighbours.</value>
        IEnumerable<T> Neighbours(T node, double speed, T dest);
    }

    /// <summary>
    /// Represent a generic Path along a graph.
    /// </summary>
    public class Path<TNode> : IEnumerable<TNode>
    {

        #region PublicProperties
        /// <summary>
        /// Gets the last step.
        /// </summary>
        /// <value>The last step.</value>
        public TNode LastStep { get; private set; }

        /// <summary>
        /// Gets the previous steps.
        /// </summary>
        /// <value>The previous steps.</value>
        public Path<TNode> PreviousSteps { get; private set; }

        /// <summary>
        /// Gets the total cost.
        /// </summary>
        /// <value>The total cost.</value>
        public double TotalCost { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Path`1"/> class.
        /// </summary>
        /// <param name="lastStep">Last step.</param>
        /// <param name="previousSteps">Previous steps.</param>
        /// <param name="totalCost">Total cost.</param>
        Path(TNode lastStep, Path<TNode> previousSteps, double totalCost)
        {
            LastStep = lastStep;
            PreviousSteps = previousSteps;
            TotalCost = totalCost;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Path`1"/> class.
        /// </summary>
        /// <param name="start">Start.</param>
        public Path(TNode start) : this(start, null, 0) { }
        #endregion

        /// <summary>
        /// Adds a step to the path.
        /// </summary>
        /// <returns>The new path.</returns>
        /// <param name="step">The step.</param>
        /// <param name="stepCost">The step cost.</param>
        public Path<TNode> AddStep(TNode step, double stepCost)
        {
            return new Path<TNode>(step, this, TotalCost + stepCost);
        }

        #region	EnumerableImplementation
        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>The enumerator.</returns>
        public IEnumerator<TNode> GetEnumerator()
        {
            for (Path<TNode> p = this; p != null; p = p.PreviousSteps)
                yield return p.LastStep;
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>The enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        #endregion

    }
}
