using Rogue.Physics;
using Rogue.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rogue.Map
{
    public partial class GameMap
    {
        public bool First = true;

        public string Name;

        public string _Name, _Affics;

        public ConsoleColor Biom;

        public int Level = 1;

        public GameMapObject Map = new GameMapObject();

        public HashSet<MapObject> Objects = new HashSet<MapObject>();

        public List<List<List<MapObject>>> MapOld = new List<List<List<MapObject>>>();

        public bool Move(MapObject @object, MapObject old = null)
        {
            var moveArea = Map.Query(@object);
            var mapObjs = moveArea.Nodes.Where(node => @object.IntersectsWith(node));

            foreach (var mObj in mapObjs)
            {
                mObj.Interact(this);
            }

            return !mapObjs.Any(x => x.Obstruction);

            //? node.Obstruction : false);
                       
            //if (old != null)
            //{
            //    var oldArea = Map.Query(old);
            //    if (oldArea != moveArea)
            //    {
            //        moveArea.Nodes.Add(@object);
            //        oldArea.Nodes.Remove(@object);
            //    }
            //}

            //return canMove;
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

        private bool needReloadCache = false;

        public bool ReloadCache
        {
            get
            {
                if (needReloadCache)
                {
                    needReloadCache = false;
                    return true;
                }
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

            Nodes = Enumerable.Range(0, 8).Select(num => new GameMapContainerObject()
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
                },
            } as MapObject).ToList();
        }

        protected override bool Containable => true;

        public override PhysicalPosition Position { get; set; }

        public override PhysicalSize Size { get; set; }

        protected override MapObject Self => this;

        public override void Interact(GameMap gameMap) { }
    }

    public class GameMapContainerObject : MapObject
    {
        protected override bool Containable => true;
    }

}
