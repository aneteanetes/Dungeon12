namespace Rogue.Physics
{
    using MoreLinq;
    using Rogue.Network;
    using Rogue.Types;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class PhysicalObject : NetObject
    {
        public virtual PhysicalSize Size { get; set; }

        public virtual PhysicalPosition Position { get; set; }

        public virtual PhysicalPosition MaxPosition => new PhysicalPosition
        {
            X = Position.X + Size.Width,
            Y = Position.Y + Size.Height
        };

        public bool Contains(PhysicalObject one)
        {
            var xContains = one.Position.X >= this.Position.X
                && one.Position.X < this.Size.Width;

            var yCOntains = one.Position.Y >= this.Position.Y
                && one.Position.Y < this.Size.Height;

            return xContains && yCOntains;
        }

        public bool IntersectsWith(PhysicalObject @object) => Intersect(this, @object);

        public bool IntersectsWithOrContains(PhysicalObject @object) => this.IntersectsWith(@object) || this.Contains(@object);

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

        public static PhysicalObject operator *(PhysicalObject source, int b)
        {
            var a = new PhysicalObject()
            {
                Size = new PhysicalSize(),
                Position = new PhysicalPosition()
            };

            a.Size.Height = source.Size.Height * b;
            a.Size.Width = source.Size.Width * b;
            a.Position.X = source.Position.X * b;
            a.Position.Y = source.Position.Y * b;

            return a;
        }

        public PhysicalObject Grow(double by)
        {
            var rangeObject = new PhysicalObject
            {
                Position = new Physics.PhysicalPosition
                {
                    X = this.Position.X - ((this.Size.Width * by) / 2),
                    Y = this.Position.Y - ((this.Size.Height * by) / 2)
                },
                Size = new PhysicalSize()
                {
                    Height = this.Size.Height,
                    Width = this.Size.Width
                }
            };

            rangeObject.Size.Height *= by;
            rangeObject.Size.Width *= by;

            return rangeObject;
        }

        public override string ToString()
        {
            return $"{this.GetType().Name} x:{Position.X} y:{Position.Y} ({Size.Width}x{Size.Height})";
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

        public bool Exists(T physicalObject)
        {
            try
            {
                return Query(physicalObject).Nodes.Any(node => node.IntersectsWith(physicalObject));
            }
            catch (InvalidOperationException)
            {
                return true;
            }
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
                    var copyNodes = new List<T>(Nodes);
                    foreach (var node in copyNodes)
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

            if (this.Nodes.Count == 0)
            {
                return nodes;
            }

            if (!this.Nodes.Contains(physicalObject))
            {
                foreach (var node in Nodes)
                {
                    if (!node.Containable)
                    {
                        continue;
                    }

                    var queryDeepNode = node.QueryReference(physicalObject);
                    if (queryDeepNode.Count > 0)
                    {
                        nodes.AddRange(queryDeepNode);
                    }
                }
            }
            else
            {
                nodes.Add(Self);
            }

            return nodes;
        }

        public virtual void Add(T physicalObject)
        {
            physicalObject.Root = this;

            bool end = false;

            foreach (var node in this.Query(physicalObject, true))
            {
                if (node == this)
                {
                    end = true;
                    break;
                }
                node.Add(physicalObject);
            }

            if (end)
            {
                this.Nodes.Add(physicalObject);
            }
        }
        
        /// <summary>
        /// Удаляет по ссылке в верхних нодах
        /// </summary>
        /// <param name="physicalObject"></param>
        /// <returns></returns>
        public bool Remove(T physicalObject)
        {
            var areas = QueryReference(physicalObject);
            foreach (var area in areas)
            {
                area.Nodes.Remove(physicalObject);
            }

            //foreach (var nodeInRoot in this.Nodes)
            //{
            //    if (nodeInRoot.Nodes.Contains(physicalObject))
            //    {
            //        nodeInRoot.Nodes.Remove(physicalObject);
            //    }
            //}

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

         private double distance(double x1, double y1, double x2, double y2)
        {
            double dx = x2 - x1;
            double dy = y2 - y1;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public IEnumerable<T> Neighbours(T node, double speed, T dest)
        {           
            List<T> miniMap = new List<T>();
            
            foreach (var coord in Coords(10))
            {
                var point = new T
                {
                    Size = new PhysicalSize
                    {
                        Height = 10,
                        Width =10
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

                var existedNode = node.Root.Query(point);

                var intersected = existedNode==null
                    ? false
                    : existedNode.Nodes.ToArray()
                    .Where(x => x != point.StartObject && x!=dest)
                    .Where(x=>x!=null)
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

            var distance = ComputeHScore(start, target);
            
            while (openList.Count > 0)
            {
                if(closedList.Count/5> distance)
                {
                    break;
                }

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

                var adjacentSquares = current.Neighbours(current,speed,target);
                
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
                        adjacentSquare.H = ComputeHScore(adjacentSquare, target);
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
                
                current = current.Parent;
            }

            return path.DistinctBy(x => x.X + x.Y).Reverse().ToList(); //return list of dots
        }

        static double getCount(Point p, Point q)
        {
            // If line joining p and q is parallel to 
            // x axis, then count is difference of y 
            // values 
            if (p.X == q.X)
                return Math.Abs(p.Y - q.Y) - 1;

            // If line joining p and q is parallel to 
            // y axis, then count is difference of x 
            // values 
            if (p.Y == q.Y)
                return Math.Abs(p.X - q.X) - 1;

            return gcd(Math.Abs(p.X - q.X), Math.Abs(p.Y - q.Y)) - 1;
        }

        static double gcd(double a, double b)
        {
            if (b == 0)
                return a;
            return gcd(b, a % b);
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

        // I've used Tupple<Double, Double> to represent a point;
        // You, probably have your own type for it
        public static IList<Point> SplitLine(Point a, Point b, double count)
        {
            count = count + 1;

            Double d = Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y)) / count;
            Double fi = Math.Atan2(b.Y - a.Y, b.X - a.X);

            List<Point> points = new List<Point>();

            for (int i = 0; i <= count; ++i)
                points.Add(new Point(a.X + i * d * Math.Cos(fi), a.Y + i * d * Math.Sin(fi)));

            return points;
        }

        private static List<Point> ExtendPoints(Point pt1, Point pt4, double numberOfPoints)
        {
            var extendedPoints = new List<Point>();

            for (double d = 1; d < numberOfPoints - 1; d++)
            {
                double a = (Math.Max(pt1.X, pt4.X) - Math.Min(pt1.X, pt4.X)) * d / (double)(numberOfPoints - 1) + Math.Min(pt1.X, pt4.X);
                double b = (Math.Max(pt1.Y, pt4.Y) - Math.Min(pt1.Y, pt4.Y)) * d / (double)(numberOfPoints - 1) + Math.Min(pt1.Y, pt4.Y);
                var pt2 = new Point(a, b);
                extendedPoints.Add(pt2);
            }

            return extendedPoints;
        }

        static double ComputeHScore(PhysicalObject po1, PhysicalObject po2)
        {
            double x = po1.Position.X - (po1.Size.Width / 2);
            double targetX = po2.Position.X - (po2.Size.Width / 2);

            double y = po1.Position.Y - (po1.Size.Height / 2);
            double targetY = po2.Position.Y - (po2.Size.Height / 2);

            return Math.Abs(targetX - x) + Math.Abs(targetY - y);
        }
    }
}