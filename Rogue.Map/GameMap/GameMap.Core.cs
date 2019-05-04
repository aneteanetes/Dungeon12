using Rogue.Map.Objects;
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
        
        public Action<MapObject, Direction,bool> OnMoving;

        public bool Move(MapObject @object, Direction direction)
        {
            var moveAvailable = true;

            var moveAreas = Map.Query(@object,true);
            if (moveAreas.Count > 0)
            {
                var mapObjs = moveAreas.SelectMany(x=>x.Nodes)
                    .Where(node => node != @object)
                    .Where(node => @object.IntersectsWith(node));

                if (typeof(Avatar).IsAssignableFrom(@object.GetType()))
                {
                    foreach (var mObj in mapObjs)
                    {
                        mObj.Interact(this);
                    }
                }

                moveAvailable = !mapObjs.Any(x => x.Obstruction);
            }

            if (moveAvailable)
            {
                var wasAreas = Map.QueryReference(@object);

                bool eq = wasAreas.SequenceEqual(moveAreas);
                
                if (!eq)
                {
                    Map.Remove(@object);
                    Map.Add(@object);
                }
            }

            //if (moveAvailable)
            //{
            //    var wasAreas = Map.QueryReference(@object);

            //    if (!wasAreas.SequenceEqual(moveAreas))
            //    {
            //        Map.Remove(@object);
            //        Map.Add(@object);
            //    }
            //}

            OnMoving(@object,direction, moveAvailable);

            return moveAvailable;


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
        
        public IEnumerable<Mob> Enemies(MapObject @object)
        {
            IEnumerable<Mob> mobs = Enumerable.Empty<Mob>();

            var moveArea = Map.Query(@object);
            if (moveArea != null)
            {
                mobs = moveArea.Nodes.Where(node => node is Mob).Select(node => node as Mob)
                    .Where(node => @object.IntersectsWith(node));
            }

            return mobs.ToArray();
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
