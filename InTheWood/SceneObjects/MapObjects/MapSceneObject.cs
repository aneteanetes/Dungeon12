using Dungeon;
using Dungeon.Control.Pointer;
using Dungeon.SceneObjects;
using InTheWood.Entities.MapScreen;
using System.Linq;

namespace InTheWood.SceneObjects.MapObjects
{
    public class MapSceneObject : ControlSceneObject<Map>
    {
        public MapSceneObject(Map component) : base(component, true)
        {
            this.Width = SegmentSceneObject.TileWidth * 3 * component.Sectors.Count;
            this.Height = SegmentSceneObject.TileHeight * 3 * component.Sectors.Count;

            var first = component.Sectors.FirstOrDefault();
            if (first != default)
            {
                this.AddChild(new SectorSceneObject(first));

                foreach (var sector in component.Sectors)
                {
                    AddConnectedSectors(sector);
                }
            }
            else
            {
                component.Segments.ForEach((segm, i) =>
                {
                    var segmentObj = new SegmentSceneObject(segm,this)
                    {
                        Left = segm.X * SegmentSceneObject.TileWidth + (segm.Y % 2 == 0 ? SegmentSceneObject.TileWidth / 2 : 0),
                        Top = segm.Y * SegmentSceneObject.TileHeight * 0.75
                    };
                    segmentObj.Left += segm.X * 4;
                    segmentObj.Top += segm.Y * 4;

                    if (segm.Y % 2 != 0)
                    {
                        segmentObj.Left -= 2.7;
                    }

                    this.AddChild(segmentObj);
                });
            }
        }

        public override double Scale
        {
            get => base.Scale;
            set
            {
                base.Scale = value;
                this.RecalculateComputedAndBounds();
            }
        }

        private void AddConnectedSectors(Sector sector)
        {
            var connections = Component.Connections.Where(x => x.To == sector);
            foreach (var connection in connections)
            {
                if (connection.What.SceneObject == default)
                {
                    var to = sector.SceneObject.As<SectorSceneObject>();

                    switch (connection.ConnectDirection)
                    {
                        case Dungeon.Types.SimpleDirection.Up:
                        case Dungeon.Types.SimpleDirection.Down:
                            ConnectVertical(connection.ConnectDirection == Dungeon.Types.SimpleDirection.Up, to, connection);
                            break;
                        case Dungeon.Types.SimpleDirection.Left:
                        case Dungeon.Types.SimpleDirection.Right:
                            ConnectHorizontal(connection.ConnectDirection == Dungeon.Types.SimpleDirection.Left, to, connection);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void ConnectHorizontal(bool toLeft, SectorSceneObject to, SectorConnection connection)
        {
            //базовое расположение
            var x = (toLeft ? -1 : 1) * to.Width; //to.width будет равен размеру нового сектора
            var y = 0d;

            x += to.Left;
            y += to.Top;

            //расположение на основе позиции
            switch (connection.Position)
            {
                case 0:
                    y -= SegmentSceneObject.TileHeight * 0.75;
                    x += (toLeft ? 1 : -1) * (SegmentSceneObject.TileWidth / 2);
                    break;
                case 1:
                    break;
                case 2:
                    y += SegmentSceneObject.TileHeight * 0.75;
                    x += (toLeft ? 1 : -1) * (SegmentSceneObject.TileWidth / 2);
                    break;
                default:
                    break;
            }

            //расположение с учётом оффсета
            if (connection.Offset != 0)
            {
                if (connection.Offset < 0)
                {
                    y -= SegmentSceneObject.TileHeight * 0.75;
                    x += (toLeft ? -1 : 1) * (SegmentSceneObject.TileWidth / 2);
                }
                else
                {
                    y += SegmentSceneObject.TileHeight * 0.75;
                    x -= (toLeft ? -1 : 1) * (SegmentSceneObject.TileWidth / 2);
                }
            }

            this.AddChild(new SectorSceneObject(connection.What)
            {
                Left = x,
                Top = y
            });
        }

        private void ConnectVertical(bool toTop, SectorSceneObject to, SectorConnection connection)
        {
            //базовое расположение
            var y = (toTop ? -1 : 1) * to.Height; //to.width будет равен размеру нового сектора
            var x = 0d;

            switch (connection.Position)
            {
                case 0:
                    x -= SegmentSceneObject.TileWidth / 2;
                    if (connection.Offset == 1)
                    {
                        x -= SegmentSceneObject.TileWidth + (SegmentSceneObject.TileWidth / 2);
                    }
                    break;
                case 1:
                    x += SegmentSceneObject.TileWidth / 2;
                    if (connection.Offset == 1)
                    {
                        x -= SegmentSceneObject.TileWidth;
                    }
                    break;
                case 2:
                    x += SegmentSceneObject.TileWidth * 2;
                    if (connection.Offset == 1)
                    {
                        x -= SegmentSceneObject.TileWidth;
                    }
                    break;
                default:
                    break;
            }

            this.AddChild(new SectorSceneObject(connection.What)
            {
                Left = x,
                Top = y
            });
        }
    }
}