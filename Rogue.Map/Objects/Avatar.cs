namespace Rogue.Map.Objects
{
    using System;
    using Rogue.Map.Infrastructure;
    using Rogue.Physics;
    using Rogue.Settings;
    using Rogue.Types;

    [Template("@")]
    public class Avatar : MapObject
    {
        public Entites.Alive.Character.Player Character { get; set; }

        public override string Tileset => Character.Tileset;

        public override Rectangle TileSetRegion => Character.TileSetRegion;

        public override string Icon { get => "@"; set { } }

        public Avatar()
        {
            this.ForegroundColor = new MapObjectColor
            {
                R = 255,
                A = 255
            };
        }

        public override PhysicalSize Size
        {
            get => new PhysicalSize
            {
                Height = 18,
                Width = 18
            };
            set { }
        }

        public override PhysicalPosition Position
        {
            get => new PhysicalPosition
            {
                X = base.Position.X + 8,
                Y = base.Position.Y + 14
            };
            set { }
        }


        //public override PhysicalObject Vision
        //{
        //    get => new PhysicalObject
        //    {
        //        Size = new PhysicalSize
        //        {
        //            Height = DrawingSize.Cell + DrawingSize.Cell / 2,
        //            Width = DrawingSize.Cell + DrawingSize.Cell / 2
        //        },
        //        Position = base.Vision.Position
        //    };

        //    set { }
        //}

        public override void Interact()
        {
            throw new NotImplementedException();
        }
    }
}