using Rogue.Physics;
using Rogue.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rogue.Map
{
    public class GameMap
    {
        public string Name;

        public string _Name, _Affics;

        public ConsoleColor Biom;

        public int Level = 1;

        public GameMapObject Map = new GameMapObject();

        public HashSet<MapObject> Objects = new HashSet<MapObject>();

        public List<List<List<MapObject>>> MapOld = new List<List<List<MapObject>>>();

        public bool MayMove(MapObject @object)
        {
            var moveArea = Map.Query(@object);
            return !moveArea.Nodes.Any(node => @object.IntersectsWith(node) ? node.Obstruction : false);
        }

        public bool MayMoveOld(MapObject @object)
        {
            var inVision = @object.InVision(Objects);
            return !inVision.Any(x => x.Obstruction);
        }

        public bool MoveObject(Point now, int Level, Point next)
        {
            try
            {
                //MapObject moved = this.Map[(int)now.Y][(int)now.X][Level];

                //var nextObj = this.Map[(int)next.Y][(int)next.X];

                //if (nextObj.First().Obstruction)
                //{
                //    return false;
                //}


                //this.Map[(int)now.Y][(int)now.X].Remove(moved);
                //this.Map[(int)next.Y][(int)next.X].Add(moved);

                //Console.WriteLine($"was: x={now.X}, y={now.Y}");
                //Console.WriteLine($"now: x={next.X}, y={next.Y}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("EXCEPTION ARGUMENT");
                return false;
            }
        }
    }

    public class GameMapObject : MapObject
    {
        public GameMapObject()
        {
            Position = new PhysicalPosition
            {
                X = 0,
                Y = 0
            };

            Size = new PhysicalSize
            {
                Height = 720,
                Width = 1280
            };

            Nodes = Enumerable.Range(0, 8).Select(num => new MapObject()
            {
                Size = new PhysicalSize
                {
                    Width = 320,
                    Height = 352
                },
                Position = new PhysicalPosition
                {
                    Y = num < 4 ? 0 : 352,
                    X = num < 4
                        ? num * 320
                        : (num - 4) * 320
                }
            }).ToList();
        }

        protected override bool Containable => true;

        public override PhysicalPosition Position { get; set; }

        public override PhysicalSize Size { get; set; }

        protected override MapObject Self => this;

        public override void Interact() { }
    }
}
