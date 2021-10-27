namespace Dungeon12.Map
{
    using Dungeon12.Events.Events;
    using Dungeon.GameObjects;
    using Dungeon12.Map.Objects;
    using Dungeon.Physics;
    using Dungeon.Scenes.Manager;
    using Dungeon.Types;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Dungeon;

    public partial class GameMap : GameComponent
    {
        public GameMap()
        {
            MapObject = new GameMapObject(this);
        }

        public bool Loaded { get; set; } = false;

        public Action<MapObject> PublishObject;

        public bool First = true;

        public string Name { get; set; }

        public string _Name, _Affics;

        public ConsoleColor Biom;

        public int Level = 1;

        public GameMapObject MapObject;

        public HashSet<MapObject> Objects = new HashSet<MapObject>();

        public HashSet<MapObject> SaveableObjects => Objects.Where(x => x.Saveable).ToHashSet();

        public List<List<List<MapObject>>> MapOld = new List<List<List<MapObject>>>();
        
        public Action<MapObject, Direction,bool> OnMoving;

        public bool Disabled { get; set; }

        public bool Move(MapObject @object, Direction direction)
        {
            if (Disabled)
                return false;

            var moveAvailable = true;

            var moveAreas = MapObject.QueryContainer(@object);
            if (moveAreas.Count > 0)
            {
                try
                {
                    foreach (var area in moveAreas)
                    {
                        foreach (var node in area.Nodes)
                        {
                            if (node != @object && node.IntersectsWith(@object))
                            {
                                if (node.Interactable)
                                {
                                    node.Dispatch((x, y) => x.Interact(y), @object);
                                }
                                if (node.Obstruction)
                                {
                                    moveAvailable = false;
                                    goto moveDetected;
                                }
                            }
                        }
                    }
                }
                catch (InvalidOperationException)               
                {
                    return false; // поправили блядь карту
                }
            }

            moveDetected:

            if (moveAvailable)
            {
                var wasAreas = MapObject.QueryReference(@object);

                bool eq = wasAreas.SequenceEqual(moveAreas);
                
                if (!eq)
                {
                    MapObject.Remove(@object);
                    MapObject.Add(@object);
                }
            }

            OnMoving(@object,direction, moveAvailable);

            return moveAvailable;
        }
        
        /// <summary>
        /// Получить информацию о том что объекты такого типа есть
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="object"></param>
        /// <returns></returns>
        public bool Any<T>(MapObject @object, Func<T,bool> filter=default)
            where T : PhysicalObject
        {
            return MapObject.QueryContainer(@object).SelectMany(x=>x.Nodes).Where(node => node is T)
                   .Select(node => node as T)
                   .Any(node => @object.IntersectsWith(node) && (filter?.Invoke(node) ?? true));
        }

        /// <summary>
        /// Возвращает всех найденных
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="object"></param>
        /// <returns></returns>
        public IEnumerable<T> All<T>(MapObject @object, Func<T, bool> filter = default)
            where T : MapObject
        {
            return MapObject.QueryContainer(@object).SelectMany(x => x.Nodes).Where(node => node is T)
                    .Select(node => node as T)
                    .Where(node => @object.IntersectsWith(node) && (filter?.Invoke(node) ?? true))
                    .ToArray();
        }

        /// <summary>
        /// Возвращает первого найденого T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="object"></param>
        /// <returns></returns>
        public T One<T>(MapObject @object, Func<T, bool> filter = default)
            where T : MapObject
        {
            return MapObject.QueryContainer(@object).SelectMany(x => x.Nodes).FirstOrDefault(node =>
                  {
                      if (node is T nodeT)
                      {
                          return nodeT.IntersectsWith(@object) && (filter?.Invoke(nodeT) ?? true);
                      }
                      return false;
                  }) as T;        
        }

        public MapObject Range(Point location, Point size)
        {
            return new MapObject()
            {
                Position = new PhysicalPosition()
                {
                    X = location.X * 32,
                    Y = location.Y * 32
                },
                Size = new PhysicalSize()
                {
                    Height = size.Y * 32,
                    Width = size.X * 32
                }
            };
        }

        public MapObject Range(double x, double y, double width, double height) => Range(new Point(x, y), new Point(width, height));
        
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

        public void SetPlayerLocation(Point playerPos = default)
        {
            playerPos = playerPos.Copy();
            playerPos = playerPos ?? new Point { X = 42, Y = 45 };

            var avatar = Global.GameState.Player.Component;
            avatar.Location = playerPos;
            avatar.SceenPosition = new Point(0, 0);

            double xOffset = 0;
            double yOffset = 0;

            CenterOnCharacter(playerPos, ref xOffset, ref yOffset);

            Global.DrawClient.SetCamera(xOffset * 32, yOffset * 32);

            Global.GameState.Player.Left = avatar.Location.X;
            Global.GameState.Player.Top = avatar.Location.Y;
        }

        /// <summary>
        /// Центрирует камеру на персонаже
        /// </summary>
        /// <param name="playerPos"></param>
        /// <param name="xOffset"></param>
        /// <param name="yOffset"></param>
        private static void CenterOnCharacter(Point playerPos, ref double xOffset, ref double yOffset)
        {
            xOffset -= playerPos.X - 20;
            yOffset -= playerPos.Y - 11.25;
        }
    }

    public class GameMapObject : MapObject
    {
        private GameMap _gameMap;

        public GameMapObject(GameMap gameMap)
        {
            _gameMap = gameMap;
            Position = new PhysicalPosition
            {
                X = 0,
                Y = 0
            };

            Size = new PhysicalSize
            {
                Height = 3200,
                Width = 3200
            };
            
            Nodes = new List<MapObject>();


            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    var node = new GameMapContainerObject()
                    {
                        Size = new PhysicalSize
                        {
                            Width = 1600,
                            Height = 1600
                        },
                    };
                    node.Position = new PhysicalPosition
                    {
                        X = i * 1600,
                        Y = j * 1600
                    };
                    Nodes.Add(node);
                }
            }

            foreach (var item in this.Nodes)
            {
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        var n800 = new GameMapContainerObject()
                        {
                            Size = new PhysicalSize
                            {
                                Width = 800,
                                Height = 800
                            },
                            Position = new PhysicalPosition
                            {
                                X = item.Position.X + i * 800,
                                Y = item.Position.Y + j * 800
                            }
                        };

                        for (int ii = 0; ii < 5; ii++)
                        {
                            for (int jj = 0; jj < 5; jj++)
                            {
                                n800.Nodes.Add(new GameMapContainerObject()
                                {
                                    Size = new PhysicalSize
                                    {
                                        Width = 160,
                                        Height = 160
                                    },
                                    Position = new PhysicalPosition
                                    {
                                        X = n800.Position.X + ii * 160,
                                        Y = n800.Position.Y + jj * 160
                                    }
                                });
                            }
                        }

                        item.Nodes.Add(n800);
                    }
                }
            }
        }

        protected override bool Containable => true;

        public override PhysicalPosition Position { get; set; }

        public override PhysicalSize Size { get; set; }

        protected override MapObject Self => this;

        public override void Add(MapObject physicalObject)
        {
            physicalObject.Gamemap = _gameMap;
            base.Add(physicalObject);
            if (physicalObject is Totem totem)
            {
                Global.Events.Raise(new TotemArrivedEvent(totem));
            }
        }
    }

    public class GameMapContainerObject : MapObject
    {
        protected override bool Containable => true;
    }

}
