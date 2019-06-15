using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Rogue.Control.Events;
using Rogue.Control.Pointer;
using Rogue.Drawing.GUI;
using Rogue.Drawing.Impl;
using Rogue.Drawing.SceneObjects;
using Rogue.Drawing.SceneObjects.Map;
using Rogue.Map;
using Rogue.Map.Objects;
using Rogue.Settings;
using Rogue.Types;
using Rogue.View.Interfaces;

namespace Rogue.Drawing.Labirinth
{
    public class GameMapSceneObject : HandleSceneControl
    {
        public override bool IsBatch => true;

        private GameMap gamemap;

        public Action<List<ISceneObject>> drawpath;

        public override bool Expired => gamemap.ReloadCache;

        private PlayerSceneObject player;

        public GameMapSceneObject(GameMap location, PlayerSceneObject avatar)
        {
            location.PublishObject = PublishMapObject;
            this.player = avatar;
            gamemap = location;
            gamemap.OnGeneration = () =>
            {
                gamemap.Level += 1;
                Init();
            };
        }

        public Action<List<ISceneObject>, List<ISceneObject>> OnReload;

        private List<ISceneObject> currentAdditionalObjects = new List<ISceneObject>();

        public void Init()
        {
            this.Children.Clear();

            List<ISceneObject> newSceneObjects = new List<ISceneObject>();

            for (int y = 0; y < gamemap.MapOld.Count; y++)
            {
                var line = gamemap.MapOld[y];
                for (int x = 0; x < line.Count; x++)
                {
                    MapObject[] cell = line[x].ToArray();
                    var pos = new Point { X = x, Y = y };

                    if (cell[0].Icon == "#")
                    {
                        AddWall(pos);
                        cell = cell.Skip(1).ToArray();
                    }
                    else if (cell[0].Icon == ">")
                    {
                        var portal = new Portal
                        {
                            Location = new Point(x, y)
                        };
                        portal.Region = new Rectangle
                        {
                            Height = 32,
                            Width = 32,
                            Pos = portal.Location
                        };

                        //var portalSceneObject = new StandaloneSceneObject(portal, (frameCounter, animMap) =>
                        //{

                        //    return frameCounter % (180 / animMap.Frames.Count) == 0;
                        //})
                        //{
                        //    Left = portal.Location.X,
                        //    Top = portal.Location.Y,
                        //    Width = 1,
                        //    Height = 1
                        //};

                        //newSceneObjects.Add(portalSceneObject);
                        //gamemap.Map.Add(portal);

                        var first = cell[0];

                        cell = new MapObject[]
                        {
                            new Empty()
                            {
                                Location = first.Location,
                                Region = first.Region
                            }
                        };
                    }

                    AddObject(cell, pos);
                }
            }

            Height = 0.1;// gamemap.MapOld.Count;
            Width = 0.1; //gamemap.MapOld.First().Count;

            newSceneObjects.AddRange(AddAsSceneObjects(gamemap.Objects));

            OnReload(this.currentAdditionalObjects, newSceneObjects);
            currentAdditionalObjects = newSceneObjects;
        }

        private IEnumerable<ISceneObject> AddAsSceneObjects(HashSet<MapObject> mapObjects)
        {
            List<ISceneObject> sceneObjects = new List<ISceneObject>();

            foreach (var obj in mapObjects)
            {
                switch (obj)
                {
                    case Mob mob:
                        {
                            sceneObjects.Add(new EnemySceneObject(this.player, this.gamemap, mob, mob.TileSetRegion));
                            break;
                        }
                    case NPC npc:
                        {
                            sceneObjects.Add(new NPCSceneObject(this.player, this.gamemap, npc, npc.TileSetRegion));
                            break;
                        }
                    case Home home:
                        {
                            sceneObjects.Add(new HomeSceneObject(this.player, home, home.Name));
                            break;
                        }
                    default:
                        break;
                }
            }

            return sceneObjects;
        }

        private void PublishMapObject(MapObject mapObject)
        {
            SceneObject sceneObject = null;

            switch (mapObject)
            {
                case Mob mob:
                    {
                        sceneObject = new EnemySceneObject(this.player, this.gamemap, mob, mob.TileSetRegion);
                        break;
                    }
                case NPC npc:
                    {
                        sceneObject = new NPCSceneObject(this.player, this.gamemap, npc, npc.TileSetRegion);
                        break;
                    }
                case Home home:
                    {
                        sceneObject = new HomeSceneObject(this.player, home, home.Name);
                        break;
                    }
                case Corpse corpse:
                    {
                        sceneObject = new CorpseSceneObject(this.player, corpse);
                        break;
                    }
                default:
                    break;
            }

            this.ShowEffects?.Invoke(new List<ISceneObject>()
            {
                sceneObject
            });
        }

        private void AddObject(MapObject[] cell, Point pos)
        {
            foreach (var item in cell)
            {
                item.Region = new Rectangle
                {
                    X = pos.X + 1,
                    Y = pos.Y + 2,
                    Height = 1,
                    Width = 1
                };

                this.AddChild(new ImageControl(item.Tileset)
                {
                    Left = pos.X,
                    Top = pos.Y,
                    Height = 1,
                    Width = 1,
                    ImageRegion = item.TileSetRegion
                });
            }
        }

        private void AddWall(Point pos)
        {
            List<bool[]> square = new List<bool[]>();

            TopSquare(pos, square);
            square.Add(GetLine((int)pos.X, gamemap.MapOld[(int)pos.Y]));
            BotSquare(pos, square);
            PosSquare(pos, square);

            MapWallTile(square, pos);
        }

        private void PosSquare(Point pos, List<bool[]> square)
        {
            if (pos.Y < gamemap.MapOld.Count-2)
            {
                var positionalLine = gamemap.MapOld[(int)pos.Y + 2];
                square.Add(GetLine((int)pos.X, positionalLine));
            }
            else if (pos.Y == gamemap.MapOld.Count - 1)
            {
                square.Add(EmptyFalseLine);
            }
            else
            {
                square.Add(EmptyTrueLine);
            }
        }

        private void BotSquare(Point pos, List<bool[]> square)
        {
            if (pos.Y == gamemap.MapOld.Count - 1)
            {
                square.Add(EmptyTrueLine);
            }
            else
            {
                var botLine = gamemap.MapOld[(int)pos.Y +1];
                square.Add(GetLine((int)pos.X, botLine));
            }
        }

        private void TopSquare(Point pos, List<bool[]> square)
        {
            if (pos.Y == 0)
            {
                square.Add(EmptyFalseLine);
            }
            else
            {
                var topLine = gamemap.MapOld[(int)pos.Y - 1];
                square.Add(GetLine((int)pos.X, topLine));
            }
        }

        private bool[] EmptyFalseLine => Enumerable.Range(0, 3).Select(x => false).ToArray();
        private bool[] EmptyTrueLine => Enumerable.Range(0, 3).Select(x => true).ToArray();

        private bool[] GetLine(int xPos, List<List<MapObject>> data)
        {
            bool IsWall(List<MapObject> cell)
            {
                return cell[0].Icon == "#";
            }

            var result = new List<bool>();

            if (xPos == 0)
            {
                result.Add(false);
            }
            else
            {
                result.Add(IsWall(data[xPos - 1]));
            }

            result.Add(IsWall(data[xPos]));

            if (xPos == gamemap.MapOld.First().Count - 1)
            {
                result.Add(false);
            }
            else
            {
                result.Add(IsWall(data[xPos + 1]));
            }

            return result.ToArray();
        }

        private void MapWallTile(List<bool[]> wallMap, Point pos)
        {
            var isometricMap = IsometricMap(wallMap);
            var tile = DetermineTitle(isometricMap);
            var mapObj = gamemap.MapOld[(int)pos.Y][(int)pos.X][0];
            
            mapObj.TileSetRegion = new Rectangle
            {
                X = 24*tile.X,
                Y = 24*tile.Y,
                Height = 24,
                Width = 24
            };

            mapObj.Region = new Rectangle
            {
                X = pos.X + 1,
                Y = pos.Y + 2,
                Height = 1,
                Width = 1
            };

            this.AddChild(new ImageControl(mapObj.Tileset)
            {
                Left = pos.X,
                Top = pos.Y,
                Height = 1,
                Width = 1,
                ImageRegion = mapObj.TileSetRegion
            });
        }

        private List<bool[]> IsometricMap(List<bool[]> wallMap)
        {
            bool IsTop(int x, int y)
            {
                return wallMap[y][x] && (wallMap[y + 1][x]);
            }

            var topMap = new List<bool[]>();

            for (int i = 0; i < wallMap.Count - 1; i++)
            {
                var item = wallMap[i];
                var line = new List<bool>();

                for (int j = 0; j < item.Length; j++)
                {
                    var itm = item[j];

                    line.Add(IsTop(j, i));
                }

                topMap.Add(line.ToArray());
            }

            return topMap;
        }

        private Point DetermineTitle(List<bool[]> isometricMap)
        {
            var topLeft = isometricMap[0][0];
            var top = isometricMap[0][1];
            var topRight = isometricMap[0][2];
            var left = isometricMap[1][0];
            var mid = isometricMap[1][1];
            var right = isometricMap[1][2];
            var botLeft = isometricMap[2][0];
            var bot = isometricMap[2][1];
            var botRight = isometricMap[2][2];

            var yTexture = 1;
            var xTexture = 0;

            if (mid)
            {

                if (!topLeft && !topRight && !botLeft && !botRight
                    /*&& left && right && top && bot*/)
                {
                    yTexture = 2;
                    xTexture = 8;
                }

                if (!left/*&& right && bot*/)
                {
                    yTexture = 3;
                    xTexture = 0;
                }

                if (!bot)
                {
                    yTexture = 5;
                    xTexture = 2;
                }

                if (!left && !bot /*&& top && left*/ /*&& !topRight*/)
                {
                    yTexture = 2;
                    xTexture = 7;
                }

                if (!topLeft && !botLeft && !right
                    /*&& left && top && bot*/)
                {
                    yTexture = 3;
                    xTexture = 3;
                }

                if (!top && !botLeft && !botRight
                    /*&& left && right && bot*/)
                {
                    yTexture = 3;
                    xTexture = 2;
                }
                
                if (!bot && !right /*&& !topLeft*//* && left && top*/)
                {
                    yTexture = 3;
                    xTexture = 1;
                }


                if (!top && !right /*&& !botLeft*/ /*&& left && bot*/)
                {
                    yTexture = 2;
                    xTexture = 9;
                }

                if (!top && !bot /*&& left*/ /*&& right*/)
                {
                    yTexture = 2;
                    xTexture = 3;
                }


                if (!top && !left /*&& right && bot*/)
                {
                    yTexture = 2;
                    xTexture = 5;
                }

                if (!left && !right /*&& top && bot*/)
                {
                    yTexture = 2;
                    xTexture = 6;
                }

                if (!left && !top && !right /*&& bot*/)
                {
                    yTexture = 2;
                    xTexture = 1;
                }

                if (!top && !bot && !right /*&& left*/)
                {
                    yTexture = 2;
                    xTexture = 4;
                }

                if (!left && !top && !bot /*&& right*/)
                {
                    yTexture = 2;
                    xTexture = 2;
                }

                if (!left && !right && !bot /*&& top*/)
                {
                    yTexture = 2;
                    xTexture = 0;
                }

                if (!bot && !top && !left && !right)
                {
                    yTexture = 1;
                    xTexture = 9;
                }
            }

            if (yTexture == 1 && xTexture == 0)
            {
                if (left && right)
                {
                    yTexture = 1;
                    xTexture = 0;
                }

                if (!left && !right)
                {
                    yTexture = 1;
                    xTexture = 5;
                }

                if(left && !right)
                {
                    yTexture = 1;
                    xTexture = 4;
                }

                if(!left && right)
                {
                    yTexture = 1;
                    xTexture = 6;
                }
            }

            return new Point
            {
                X = xTexture,
                Y = yTexture
            };
        }

        protected override ControlEventType[] Handles => new ControlEventType[0];

        private List<Point> path = new List<Point>();

        public override void Click(PointerArgs args)
        {           
        }

        //public override void Focus()
        //{
        //    //System.Console.WriteLine("focused map");
        //}

        //public override void Unfocus()
        //{
        //    //System.Console.WriteLine("unfocused map");
        //}
    }    
}