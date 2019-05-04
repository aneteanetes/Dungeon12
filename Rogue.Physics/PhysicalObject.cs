namespace Rogue.Physics
{
    using MoreLinq;
    using Rogue.Types;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class PhysicalObject
    {
        public virtual PhysicalSize Size { get; set; }

        public virtual PhysicalPosition Position { get; set; }

        public bool Contains(PhysicalObject one)
        {
            var xContains = one.Position.X >= this.Position.X
                && one.Position.X < this.Size.Width;

            var yCOntains = one.Position.Y >= this.Position.Y
                && one.Position.Y < this.Size.Height;

            return xContains && yCOntains;
        }

        public bool IntersectsWith(PhysicalObject @object) => Intersect(this, @object);

        public static bool Intersect(PhysicalObject a, PhysicalObject b)
        {
            var x1 = Math.Max(a.Position.X, b.Position.X);
            var x2 = Math.Min(a.Position.X + a.Size.Width, b.Position.X + b.Size.Width);
            var y1 = Math.Max(a.Position.Y, b.Position.Y);
            var y2 = Math.Min(a.Position.Y + a.Size.Height, b.Position.Y + b.Size.Height);

            if (x2 >= x1 && y2 >= y1)
            {
                return true;
            }

            return false;
        }
    }

    public abstract class PhysicalObject<T> : PhysicalObject, IHasNeighbours<T>
        where T : PhysicalObject<T>, new()
    {
        public PhysicalObject<T> Root
        {
            get;
            set;
        }

        /// <summary>
        /// Объект с которого начинается путь
        /// </summary>
        private PhysicalObject<T> StartObject;

        protected abstract T Self { get; }

        public List<T> Nodes { get; set; } = new List<T>();

        protected virtual bool Containable => false;

        public virtual T Vision { get; set; }

        public T Query(T physicalObject)
        {
            if (this.IntersectsWith(physicalObject))
            {
                if (this.Nodes.Count == 0)
                {
                    return Self;
                }
                else
                {
                    T queryNode = Self;

                    for (int i = 0; i < Nodes.Count; i++)
                    {
                        var node = Nodes.ElementAtOrDefault(i);
                        if (!node.Containable)
                        {
                            continue;
                        }

                        var queryDeepNode = node.Query(physicalObject);
                        if (queryDeepNode != null)
                        {
                            return queryDeepNode;
                        }
                    }

                    return queryNode;
                }
            }

            return null;
        }

        public List<T> Query(T physicalObject, bool multiple)
        {
            List<T> nodes = new List<T>();

            if (this.IntersectsWith(physicalObject))
            {
                if (this.Nodes.Count == 0)
                {
                    nodes.Add(Self);
                }
                else
                {
                    foreach (var node in Nodes)
                    {
                        if (!node.Containable)
                        {
                            continue;
                        }

                        var queryDeepNode = node.Query(physicalObject, true);
                        if (queryDeepNode.Count > 0)
                        {
                            nodes.AddRange(queryDeepNode);
                        }
                    }
                }

                //this is backward direction
                if (nodes.Count == 0)
                {
                    nodes.Add(Self);
                }
            }

            return nodes;
        }

        public List<T> QueryReference(T physicalObject)
        {
            List<T> nodes = new List<T>();

            if (physicalObject.Nodes.Contains(physicalObject))
            {
                if (this.Nodes.Count == 0)
                {
                    nodes.Add(Self);
                }
                else
                {
                    foreach (var node in Nodes)
                    {
                        if (!node.Containable)
                        {
                            continue;
                        }

                        var queryDeepNode = node.Query(physicalObject, true);
                        if (queryDeepNode.Count > 0)
                        {
                            nodes.AddRange(queryDeepNode);
                        }
                    }
                }

                //this is backward direction
                if (nodes.Count == 0)
                {
                    nodes.Add(Self);
                }
            }

            return nodes;
        }

        public void Add(T physicalObject)
        {
            physicalObject.Root = this;

            foreach (var node in this.Query(physicalObject, true))
            {
                node.Nodes.Add(physicalObject);
            }
        }

        /// <summary>
        /// Удаляет по ссылке в верхних нодах
        /// </summary>
        /// <param name="physicalObject"></param>
        /// <returns></returns>
        public bool Remove(T physicalObject)
        {
            foreach (var nodeInRoot in this.Nodes)
            {
                if (nodeInRoot.Nodes.Contains(physicalObject))
                {
                    nodeInRoot.Nodes.Remove(physicalObject);
                }
            }

            return true;
        }

        public IEnumerable<T> InVision(IEnumerable<T> available)
            => available.Where(physObj => this.IntersectsWith(physObj));

        public T this[int index] => Nodes[index];

        public bool InVision(T available) => this.IntersectsWith(available);

        public T ClonePhysicalObject() => new T
        {
            Size = new PhysicalSize { Height = this.Size.Height, Width = this.Size.Width },
            Position = new PhysicalPosition { X = this.Position.X, Y = this.Position.Y },
            Root=this.Root
        };

        public Path<T> GetPath(T one, T another,double speed)
        {
            return AStar.FindPath(this, one, another, speed, CalculateDistance, n => 1,CheckDestinationRiched);
        }

        private bool CheckDestinationRiched(T path, T target)
        {
            return path.IntersectsWith(target);
        }

        private double CalculateDistance(T one, T another)
            => distance(one.Position.X, one.Position.Y, another.Position.X, another.Position.Y);

        double distance(double x1, double y1, double x2, double y2)
        {
            double dx = x2 - x1;
            double dy = y2 - y1;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public IEnumerable<T> Neighbours(T node, double speed)
        {           
            List<T> miniMap = new List<T>();
            
            foreach (var coord in Coords(speed))
            {
                var point = new T
                {
                    Size = new PhysicalSize
                    {
                        Height = 5,
                        Width = 5
                    },
                    Position = new PhysicalPosition
                    {
                        X = node.Position.X + coord.X,
                        Y = node.Position.Y + coord.Y
                    }
                };
                point.Root = node.Root;
                point.StartObject = node.StartObject == null
                    ? node
                    : node.StartObject;

                var wtf = node.Root.Query(point);

                var intersected = wtf==null
                    ? false
                    : wtf.Nodes
                    .Where(x => x != point.StartObject)
                    .Any(x => x.IntersectsWith(point));

                if (!intersected)
                {
                    miniMap.Add(point);
                }
            }

            return miniMap;
        }

        private Point[] Coords(double speed) => new Point[]
        {
                new Point(-speed,-speed),
                new Point(0,-speed),
                new Point(speed,-speed),
                new Point(-speed,0),
                new Point(speed,0),
                new Point(-speed,speed),
                new Point(0,speed),
                new Point(speed,speed),
        };
                
        private double F;
        private double G;
        private double H;
        private T Parent;
        
        public List<Point> FindPath(T start, T target,double speed =30, double range=1)
        {
            var openList = new List<T>();
            var closedList = new List<T>();
            double g = 0;

            T current = null;
            // start by adding the original position to the open list  
            openList.Add(start);

            while (openList.Count > 0)
            {
                // get the square with the lowest F score  
                var lowest = openList.Min(l => l.F);
                current = openList.First(l => l.F == lowest);

                // add the current square to the closed list  
                closedList.Add(current);
                
                // remove it from the open list  
                openList.Remove(current);

                // if we added the destination to the closed list, we've found a path  
                if (closedList.FirstOrDefault(l=>l.IntersectsWith(target))!=null)
                    break;

                var adjacentSquares = current.Neighbours(current,speed);
                
                g = current.G + 1;

                foreach (var adjacentSquare in adjacentSquares)
                {
                    // if this adjacent square is already in the closed list, ignore it  
                    if (closedList.FirstOrDefault(l => l.Position.X == adjacentSquare.Position.X
                        && l.Position.Y == adjacentSquare.Position.Y) != null)
                        continue;

                    // if it's not in the open list...  
                    if (openList.FirstOrDefault(l => l.Position.X == adjacentSquare.Position.X
                        && l.Position.Y == adjacentSquare.Position.Y) == null)
                    {
                        // compute its score, set the parent  
                        adjacentSquare.G = g;
                        adjacentSquare.H = ComputeHScore(adjacentSquare.Position.X, adjacentSquare.Position.Y, target.Position.X, target.Position.Y);
                        adjacentSquare.F = adjacentSquare.G + adjacentSquare.H;
                        adjacentSquare.Parent = current;

                        // and add it to the open list  
                        openList.Insert(0, adjacentSquare);
                    }
                    else
                    {
                        // test if using the current G score makes the adjacent square's F score  
                        // lower, if yes update the parent because it means it's a better path  
                        if (g + adjacentSquare.H < adjacentSquare.F)
                        {
                            adjacentSquare.G = g;
                            adjacentSquare.F = adjacentSquare.G + adjacentSquare.H;
                            adjacentSquare.Parent = current;
                        }
                    }
                }
            }

            List<Point> path = new List<Point>();

            while (current != null)
            {
                path.Add(new Point(current.Position.X,current.Position.Y));

                //if (current.Parent != null)
                //{
                //    var points = GetPointsOnLine(current.Position.X, current.Position.Y, current.Parent.Position.X, current.Parent.Position.Y, range);

                //    path.AddRange(points);
                //}

                current = current.Parent;
            }

            return path.DistinctBy(x => x.X + x.Y).Reverse().ToList(); //return list of dots
        }

        public static IEnumerable<Point> GetPointsOnLine(double x0, double y0, double x1, double y1,double range)
        {
            bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
            if (steep)
            {
                double t;
                t = x0; // swap x0 and y0
                x0 = y0;
                y0 = t;
                t = x1; // swap x1 and y1
                x1 = y1;
                y1 = t;
            }
            if (x0 > x1)
            {
                double t;
                t = x0; // swap x0 and x1
                x0 = x1;
                x1 = t;
                t = y0; // swap y0 and y1
                y0 = y1;
                y1 = t;
            }
            double dx = x1 - x0;
            double dy = Math.Abs(y1 - y0);
            double error = dx / 2;
            double ystep = (y0 < y1) ? 1 : -1;
            double y = y0;
            for (double x = x0; x <= x1; x+= range)
            {
                yield return new Point((steep ? y : x), (steep ? x : y));
                error = error - dy;
                if (error < 0)
                {
                    y += ystep;
                    error += dx;
                }
            }
            yield break;
        }

        static double ComputeHScore(double x, double y, double targetX, double targetY)
        {
            return Math.Abs(targetX - x) + Math.Abs(targetY - y);
        }
    }
}