using Dungeon;
using Dungeon.Control;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon.Types;
using SidusXII.SceneObjects.GUI;
using System;

namespace SidusXII.SceneObjects.Main.Map
{

    public class MapCell : EmptySceneControl
    {
        private class BatchTile : EmptySceneControl
        {
            private MapCell imageTile;

            public BatchTile(MapCell imageTile)
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

        ImageObject Fog;

        public Point MapPosition { get; set; }

        public MapCell()
        {
            Batch = new BatchTile(this)
            {
                Width = MapObject.TileSize,
                Height = MapObject.TileSize,
                DrawOutOfSight = true,
                IsBatch = true,
                PerPixelCollision = true,
                CacheAvailable = true,
            };
            this.AddChild(Batch);

            this.selector = new ImageObject("GUI/Parts/tileselector.png".AsmImg())
            {
                Visible = false,
                Width = MapObject.TileSize,
                Height = MapObject.TileSize,
                CacheAvailable = false
            };

            this.playerCellselector = new ImageObject("GUI/Parts/playercell.png".AsmImg())
            {
                Visible = false,
                Width = MapObject.TileSize,
                Height = MapObject.TileSize,
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
                Visible = false
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
            imageObject.Width = MapObject.TileSize;
            imageObject.Height = MapObject.TileSize;
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

        static MapCell startedClick = null;

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
