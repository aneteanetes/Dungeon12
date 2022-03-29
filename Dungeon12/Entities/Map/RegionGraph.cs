using System;
using System.Collections.Generic;

namespace Dungeon12.Entities.Map
{
    public class RegionGraph
    {
        public RegionGraph() { }
        public RegionGraph(IEnumerable<int> vertices, IEnumerable<(int,int)> edges)
        {
            foreach (var vertex in vertices)
                AddVertex(vertex);

            foreach (var edge in edges)
                AddEdge(edge);
        }

        public Dictionary<int, HashSet<int>> AdjacencyList { get; } = new Dictionary<int, HashSet<int>>();

        public void AddVertex(int vertex)
        {
            AdjacencyList[vertex] = new HashSet<int>();
        }

        public void AddEdge((int, int) edge)
        {
            if (AdjacencyList.ContainsKey(edge.Item1) && AdjacencyList.ContainsKey(edge.Item2))
            {
                AdjacencyList[edge.Item1].Add(edge.Item2);
                AdjacencyList[edge.Item2].Add(edge.Item1);
            }
        }

        public Func<int, IEnumerable<int>> ShortestPathFunction(int start)
        {
            var previous = new Dictionary<int, int>();

            var queue = new Queue<int>();
            queue.Enqueue(start);

            while (queue.Count > 0)
            {
                var vertex = queue.Dequeue();
                foreach (var neighbor in this.AdjacencyList[vertex])
                {
                    if (previous.ContainsKey(neighbor))
                        continue;

                    previous[neighbor] = vertex;
                    queue.Enqueue(neighbor);
                }
            }

            Func<int, IEnumerable<int>> shortestPath = v => {
                var path = new List<int> { };

                var current = v;
                while (!current.Equals(start))
                {
                    path.Add(current);
                    current = previous[current];
                };

                path.Add(start);
                path.Reverse();

                return path;
            };

            return shortestPath;
        }
    }
}
