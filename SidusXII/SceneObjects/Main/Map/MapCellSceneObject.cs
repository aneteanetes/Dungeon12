using Dungeon;
using Dungeon.Control;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon.Types;
using SidusXII.Models.Map;
using SidusXII.SceneObjects.GUI;
using System;

namespace SidusXII.SceneObjects.Main.Map
{

    public class MapCellSceneObject : SceneControl<MapCellComponent>
    {
        private class BatchTile : EmptySceneControl
        {
            private MapCellSceneObject imageTile;

            public BatchTile(MapCellSceneObject imageTile)
            {
                this.imageTile = imageTile;
            }

            public override void Focus()
            {
                imageTile.Focus(true);
            }

            public override void Unfocus()
            {
                imageTile.Unfocus(true);
            }
        }

        ImageObject selector;

        ImageObject playerCellselector;

        BatchTile Batch;

        public ImageObject Fog;

        public Point MapPosition { get; set; }

        public MapCellSceneObject(MapCellComponent mapCell):base(mapCell,true)
        {
            Batch = new BatchTile(this)
            {
                Width = MapSceneObject.TileSize,
                Height = MapSceneObject.TileSize,
                DrawOutOfSight = true,
                IsBatch = true,
                PerPixelCollision = true,
                CacheAvailable = true,
            };
            this.AddChild(Batch);

            this.selector = new ImageObject("GUI/Parts/tileselector.png".AsmImg())
            {
                Visible = false,
                Width = MapSceneObject.TileSize,
                Height = MapSceneObject.TileSize,
                CacheAvailable = false
            };

            this.playerCellselector = new ImageObject("GUI/Parts/playercell.png".AsmImg())
            {
                Visible = false,
                Width = MapSceneObject.TileSize,
                Height = MapSceneObject.TileSize,
                CacheAvailable = false
            };

            if (Global.GamePadConnected)
            {
                this.AddChild(playerCellselector);
                this.AddChild(selector);
            }
            else
            {
                this.AddChild(selector);
                this.AddChild(playerCellselector);
            }

            Fog = new ImageObject("GUI/Parts/fogofwar.png".AsmImg())
            {
                Visible = !Component.Visible,
                CacheAvailable=false
            };
            this.AddChild(Fog);
        }

        private bool playercell = false;

        public bool PlayerCell
        {
            get => playercell;
            set
            {
                playercell = value;
                playerCellselector.Visible = value;
            }
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
            if (!Fog.Visible)
                selector.Visible = true;
        }

        public void Unfocus(bool fromDepth)
        {
            if (!Fog.Visible)
                selector.Visible = false;
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
                this.Layer.AddObject(new PopupString(MapPosition.ToString().AsDrawText(), this.ComputedPosition.Pos, speed: 0.5)
                {
                    Time = TimeSpan.FromSeconds(0.7),
                });
                startedClick = null;
            }
        }
    }
}
