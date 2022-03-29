using System;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon12.Entities.Map
{
    public class MapRegion
    {
        public string Name { get; set; }

        public List<MapPoint> Points { get; set; }

        public Dictionary<int, MapPoint> PointMap { get; set; } = new Dictionary<int, MapPoint>();

        public RegionGraph Graph { get; set; }

        public void BuildGraph()
        {
            var vertices = Points.Select(x => x.Id);

            List<(int, int)> edges = new List<(int, int)>();

            foreach (var point in Points)
            {
                PointMap.Add(point.Id, point);
                foreach (var idx in point.Joins)
                {
                    var edge = (point.Id, idx);

                    bool ContainsExisted((int,int) existed, (int,int) added)
                    {
                        if (existed.Item1 == added.Item1 && existed.Item2 == added.Item2)
                            return true;

                        if (existed.Item1 == added.Item2 && existed.Item2 == added.Item1)
                            return true;

                        return false;
                    }

                    if (!edges.Any(x => ContainsExisted(x, edge)))
                    {
                        edges.Add(edge);
                    }
                }
            }

            Graph = new RegionGraph(vertices, edges);
        }
    }
}
