namespace Dungeon.Physics
{
    using Dungeon.Types;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class PhysicalObject : VisualObject
    {
        public bool IsPolygon => Size == default;

        public List<Dot> Polygons { get; set; } = new List<Dot>();

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

        /// <summary>
        /// Теперь рассчитыввается правильно
        /// </summary>
        /// <param name="by"></param>
        /// <returns></returns>
        public PhysicalObject Grow(double by)
        {
            var rangeObject = new PhysicalObject
            {
                Position = new Dungeon.Physics.PhysicalPosition
                {
                    X = this.Position.X - ((this.Size.Width * by)- this.Size.Width) / 2,
                    Y = this.Position.Y - ((this.Size.Height * by)- this.Size.Height) / 2
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

    public abstract class PhysicalObject<T> : PhysicalObject
        where T : PhysicalObject<T>, new() 
    {
        [Newtonsoft.Json.JsonIgnore]
        public PhysicalObject<T> Root
        {
            get;
            set;
        }

        protected abstract T Self { get; }

        [Newtonsoft.Json.JsonIgnore]
        public List<T> Nodes { get; set; } = new List<T>();

        protected virtual bool Containable { get; set; } = false;

        /// <summary>
        /// Находит конечную ноду
        /// </summary>
        /// <param name="physicalObject"></param>
        /// <returns></returns>
        public T Query(T physicalObject, bool contains=false)
        {
            if (contains ? this.IntersectsWithOrContains(physicalObject) : this.IntersectsWith(physicalObject))
            {
                if (this.Nodes.Count == 0)
                {
                    return Self;
                }
                else
                {
                    var copyNodes = new List<T>(Nodes);
                    foreach (var node in copyNodes)
                    {
                        var queryDeepNode = node.Query(physicalObject);
                        if (queryDeepNode != null)
                        {
                            return queryDeepNode.Query(physicalObject);
                        }
                    }
                }
            }

            return null;
        }

        public bool Exists(T physicalObject)
        {
            try
            {
                return Query(physicalObject) != default;
            }
            catch (InvalidOperationException)
            {
                return true;
            }
        }

        /// <summary>
        /// Находит последние ноды-контейнеры
        /// </summary>
        /// <param name="physicalObject"></param>
        /// <returns></returns>
        public List<T> QueryContainer(T physicalObject)
        {
            List<T> nodes = new List<T>();

            if (this.IntersectsWith(physicalObject))
            {
                if (this.Nodes.Count == 0 || !this.Nodes.Any(x => x.Containable))
                {
                    nodes.Add(Self);
                }
                else
                {
                    var copyNodes = new List<T>(Nodes);
                    foreach (var node in copyNodes)
                    {
                        var queryDeepNode = node.QueryContainer(physicalObject);
                        if (queryDeepNode.Count > 0)
                        {
                            nodes.AddRange(queryDeepNode);
                        }
                    }
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

        /// <summary>
        /// Находит последние ноды-контейнеры
        /// </summary>
        /// <param name="physicalObject"></param>
        /// <returns></returns>
        public List<T> QueryPhysical(T physicalObject)
        {
            List<T> nodes = new();

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
                        var queryDeepNode = node.QueryPhysical(physicalObject);
                        if (queryDeepNode.Count > 0)
                        {
                            nodes.AddRange(queryDeepNode);
                        }
                    }
                }
            }

            return nodes;
        }

        public virtual void Add(T physicalObject)
        {
            physicalObject.Root = this;

            bool end = false;

            foreach (var node in this.QueryContainer(physicalObject))
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
            bool removed = false;
            var areas = QueryReference(physicalObject);
            foreach (var area in areas)
            {
                area.Nodes.Remove(physicalObject);
                removed = true;
            }

            if(!removed)
            {
                areas = this.QueryContainer(physicalObject);
                foreach (var area in areas)
                {
                    if (area.Contains(physicalObject))
                    {
                        area.Nodes.Remove(physicalObject);
                        removed = true;
                    }
                }
            }

            return removed;
        }
        
        public T this[int index] => Nodes[index];

        /// <summary>
        /// Теперь рассчитыввается правильно
        /// </summary>
        /// <param name="by"></param>
        /// <returns></returns>
        public new T Grow(double by)
        {
            var rangeObject = new T()
            {
                Position = new Dungeon.Physics.PhysicalPosition
                {
                    X = this.Position.X - ((this.Size.Width * by) - this.Size.Width) / 2,
                    Y = this.Position.Y - ((this.Size.Height * by) - this.Size.Height) / 2
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
    }
}