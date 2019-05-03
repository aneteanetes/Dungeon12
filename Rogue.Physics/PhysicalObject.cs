using System;
using System.Collections.Generic;
using System.Linq;

namespace Rogue.Physics
{
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
            var x2 = Math.Min(a.Position.X  + a.Size.Width, b.Position.X  + b.Size.Width);
            var y1 = Math.Max(a.Position.Y , b.Position.Y );
            var y2 = Math.Min(a.Position.Y  + a.Size.Height, b.Position.Y  + b.Size.Height);

            if (x2 >= x1 && y2 >= y1)
            {
                return true;
            }

            return false;
        }
    }

    public abstract class PhysicalObject<T> : PhysicalObject
        where T: PhysicalObject<T>, new()
    {
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

                    foreach (var node in Nodes)
                    {
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

                        var queryDeepNode = node.Query(physicalObject,true);
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
            this.Query(physicalObject,true)
                .ForEach(node => node.Nodes.Add(physicalObject));
        }

        public bool Remove(T physicalObject)
        {
            this.QueryReference(physicalObject)
                .ForEach(node => node.Nodes.Remove(physicalObject));

            return true;
        }

        public IEnumerable<T> InVision(IEnumerable<T> available)
            => available.Where(physObj => this.IntersectsWith(physObj));

        public T this[int index] => Nodes[index];

        public bool InVision(T available) => this.IntersectsWith(available);

        public T ClonePhysicalObject() => new T
        {
            Size = new PhysicalSize { Height = this.Size.Height, Width = this.Size.Width },
            Position = new PhysicalPosition { X = this.Position.X, Y = this.Position.Y }
        };
    }

}