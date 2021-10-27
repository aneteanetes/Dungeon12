using Dungeon;
using Dungeon.Control;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon.Types;
using SidusXII.Models.Map;
using SidusXII.SceneObjects.GUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SidusXII.SceneObjects.Main.Map.Cell
{

    public class MapCellSceneObject : SceneControl<MapCellComponent>
    {
        ImageObject selector;

        BatchTile Batch;

        Fogofwar Fog;

        PathSelector pathSelector;

        public Point MapPosition { get; set; }

        public override bool DrawOutOfSight => false;

        public override bool CacheAvailable => false;

        public MapCellSceneObject(MapCellComponent mapCell) : base(mapCell, true)
        {
            Batch = new BatchTile(this)
            {
                Width = MapSceneObject.TileSize,
                Height = MapSceneObject.TileSize,
                IsBatch = true,
                PerPixelCollision = true,
                CacheAvailable = true,
            };
            AddChild(Batch);


            AddChild(new PlayerPositionHighlight(mapCell));

            selector = new ImageObject("GUI/Parts/tileselector.png".AsmImg())
            {
                Visible = false,
                Width = MapSceneObject.TileSize,
                Height = MapSceneObject.TileSize,
                CacheAvailable = false
            };

            //this.playerCellselector = new ImageObject("GUI/Parts/playercell.png".AsmImg())
            //{
            //    Visible = Component.Player,
            //    Width = MapSceneObject.TileSize,
            //    Height = MapSceneObject.TileSize,
            //    CacheAvailable = false
            //};

            //if (Global.GamePadConnected)
            //{
            //this.AddChild(playerCellselector);
            AddChild(selector);
            //}
            //else
            //{
            //    this.AddChild(selector);
            //    this.AddChild(playerCellselector);
            //}

            pathSelector = new PathSelector(this.Component);
            this.AddChild(pathSelector);

            ReInitFog();
        }

        public void SetEdge(MapCellPart dirCell)
        {
            pathSelector.SetEdge(dirCell);
        }

        public void ClearEdges()
        {
            pathSelector.ResetEdges();
        }

        private void ReInitFog()
        {
            if (Fog != null)
            {
                RemoveChild(Fog);
            }

            if (!Component.Visible)
            {
                Fog = new Fogofwar(Component)
                {
                    Visible = !Component.Visible,
                    CacheAvailable = false,
                };

                AddChild(Fog);
            }
            else Fog = null;
        }

        private ImageObject tile;

        public void AddTile(ImageObject imageObject)
        {
            tile = imageObject;
            imageObject.Width = MapSceneObject.TileSize;
            imageObject.Height = MapSceneObject.TileSize;
            Batch.AddChild(imageObject);
        }

        public void Focus(bool fromDepth)
        {
            if (Fog == default || !Fog.Visible || !Fog.IsDefault)
            {
                Global.Game.Map.FindPath(Component);
                selector.Visible = true;
            }
        }

        public void Unfocus(bool fromDepth)
        {
            if (Fog == default || !Fog.Visible || !Fog.IsDefault)
            {
                //Global.Game.Map.ClearPath();
                selector.Visible = false;
            }
        }

        public void Select()
        {
            selector.Visible = true;
        }

        public void Unselect()
        {
            selector.Visible = false;
        }

        protected override ControlEventType[] Handles => new ControlEventType[]
        {
            ControlEventType.Focus,
            ControlEventType.ClickRelease,
            ControlEventType.Click
        };

        static MapCellSceneObject startedClick = null;

        public override void Click(PointerArgs args)
        {
            if (selector.Visible)
                startedClick = this;
        }

        public override void ClickRelease(PointerArgs args)
        {
            if (startedClick == null)
                return;

            if (startedClick == this && selector.Visible)
            {
                ClickProcess();
                startedClick = null;
            }
        }

        public void ClickProcess(bool first = true)
        {
            if (first)
            {
                Layer.AddObject(new PopupString(MapPosition.ToString().AsDrawText(), ComputedPosition.Pos, speed: 0.5)
                {
                    Time = TimeSpan.FromSeconds(0.7),
                });

                Global.Game.Map.Move(Component);
            }

            if (Fog != null)
            {
                Component.Visible = true;
                ReInitFog();

                //this.Component.InitAround();

                // вариант где соседняя клетка открывает соседние
                Component.Around.ForEach(a =>
                {
                    if (first)
                    {
                        a.Visible = true;
                        a.SceneObject.As<MapCellSceneObject>().ClickProcess(false);
                    }
                    else
                    {
                        //a.InitAround();
                        a.ClearFog();
                        a.SceneObject.As<MapCellSceneObject>().ReInitFog();
                    }
                });
            }
        }
    }
}
