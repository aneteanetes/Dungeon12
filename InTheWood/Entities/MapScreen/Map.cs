using Dungeon;
using Dungeon.GameObjects;
using MathNet.Numerics.LinearAlgebra.Complex;
using System.Collections.Generic;
using System.Linq;

namespace InTheWood.Entities.MapScreen
{
    public class Map : GameComponent
    {
        public List<SectorConnection> Connections { get; set; } = new List<SectorConnection>();

        public List<Sector> Sectors { get; set; } = new List<Sector>();

        public void AddSector(Sector sector)
        {
            Sectors.Add(sector);
        }

        public readonly List<Segment> Segments = new List<Segment>();
        public List<List<Segment>> SegmentMap = new List<List<Segment>>();

        public void SetMap(int width, int height) // w:9, h:7
        {
            for (int y = 0; y < height; y++)
            {
                SegmentMap.Add(new List<Segment>());
                for (int x = 0; x < width; x++)
                {
                    bool is_odd = false;
                    if (y % 2 != 0 && x == 0)
                    {
                        is_odd = true;
                        x++;
                        SegmentMap[y].Add(null);
                    }
                    var seg = new Segment(x, y);
                    if (y == 0 || y == height - 1)
                    {
                        seg.IsEdge = true;
                    }
                    if (is_odd)
                    {
                        seg.IsEdge = true;
                    }
                    if (x == width - 1)
                    {
                        seg.IsEdge = true;
                    }
                    Segments.Add(seg);
                    SegmentMap[y].Add(seg);

                    if (y == 1 && x == 7)
                    {
                        seg.Status = MapStatus.Hostile;
                    }
                }
            }
        }

        public MapStage Stage { get; private set; }

        public void Turn()
        {
            if (Stage != MapStage.Gameplay)
            {
                Stage = (MapStage)(((int)Stage) + 1);
                return;
            }

            //debug
            //var immuned = this.Segments.FirstOrDefault(s => s.IsAlive && s.Immune);
            //if (immuned != default)
            //    GetNeighbors(immuned).ForEach(s => s.IsAlive = true);
            //this.Segments.Where(s => s.Immune).ForEach(s => s.Immune = false);




            return;
            var aliveLimit = 0;

            var deads = this.Segments.Where(s => !s.IsAlive && !s.Immune).Shuffle();
            foreach (var dead in deads)
            {
                if (aliveLimit == 2)
                    break;

                var neighbors = GetNeighbors(dead);
                var neighborsAlives = neighbors.Count(s => s.IsAlive);
                if (neighborsAlives >= 2)
                {
                    aliveLimit++;
                    dead.IsAlive = true;
                }
            }

            aliveLimit = 0;

            var alives = this.Segments.Where(s => s.IsAlive && !s.Immune).Shuffle();
            foreach (var alive in alives)
            {
                if (aliveLimit == 2)
                    break;
                var neighbors = GetNeighbors(alive);
                var neighborsAlives = neighbors.Count(s => s.IsAlive);
                if (neighborsAlives >= 3)
                {
                    aliveLimit++;
                    alive.IsAlive = false;
                }
            }

            this.Segments.Where(s => s.Immune).ForEach(s => s.Immune = false);
        }

        public IEnumerable<Segment> GetNeighbors(Segment alive)
        {
            return new Segment[]
            {
                SegmentMap[alive.Y].ElementAtOrDefault(alive.X-1),
                SegmentMap[alive.Y].ElementAtOrDefault(alive.X+1),
                SegmentMap.ElementAtOrDefault(alive.Y-1)?.ElementAtOrDefault(alive.X),
                SegmentMap.ElementAtOrDefault(alive.Y-1)?.ElementAtOrDefault(alive.X+(alive.Y%2==0 ? 1 : -1)* 1),
                SegmentMap.ElementAtOrDefault(alive.Y+1)?.ElementAtOrDefault(alive.X),
                SegmentMap.ElementAtOrDefault(alive.Y+1)?.ElementAtOrDefault(alive.X+(alive.Y%2==0 ? 1 : -1)* 1),
            }.Where(s => s != default/* && !s.Immune*/);
        }

        public void AddSector(Sector sector, SectorConnection connection)
        {
            this.AddSector(sector);
            this.Connections.Add(connection);
        }
    }
}