namespace Dungeon.Map.Objects
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Dungeon.Entites.Animations;
    using Dungeon.Map.Infrastructure;
    using Dungeon.Types;

    [Template(">")]
    public class Portal : MapObject
    {
        public override string Tileset
        {
            get => "Dungeon12.Resources.Images.Objects.portal.png";
            set { }
        }

        public override Rectangle TileSetRegion
        {
            get => new Rectangle
            {
                Height = 32,
                Width = 32,
                X = 0,
                Y = 128
            };
            set { }
        }

        public override AnimationMap Animation => new AnimationMap
        {
            TileSet = "Dungeon12.Resources.Images.Objects.portal.png",
            Size=new Point(32,32),
            Frames=new List<Point>
            {
                new Point(0,0),
                new Point(32,0),
                new Point(64,0),
                new Point(96,0),
                new Point(0,32),
                new Point(32,32),
                new Point(64,32),
                new Point(96,32),
                new Point(0,64),
                new Point(32,64),
                new Point(64,64),
                new Point(96,64),
                new Point(0,128)
            }
        };

        protected override MapObject Self => throw new NotImplementedException();

        //public override void Interact(GameMap gameMap)
        //{
        //    gameMap.Generate();
        //}
    }
}