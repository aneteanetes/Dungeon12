namespace Rogue.Drawing.Labirinth
{
    using System.Linq;
    using Rogue.Drawing.Animations;
    using Rogue.Entites;
    using Rogue.Map;
    using Rogue.Types;
    using Rogue.View.Interfaces;

    public class MapAnimationSession : AnimationSession
    {
        public MapObject MapObject { get; set; }

        public override int Speed => 100;

        public override IAnimationSession Run()
        {
            var drawableFrames = MapObject.Animation.Frames.Select(f =>
            {
                return new Drawable
                {
                    Tileset = MapObject.Animation.TileSet,
                    Region = new Rectangle
                    {
                        X = MapObject.Location.X+1,
                        Y = MapObject.Location.Y+2,
                        Height = 1,
                        Width = 1
                    },
                    TileSetRegion = new Rectangle
                    {
                        X = f.X,
                        Y = f.Y,
                        Height = MapObject.Animation.Size.Y,
                        Width = MapObject.Animation.Size.X
                    }
                };
            });

            foreach (var drawableFrame in drawableFrames)
            {
                this.AddFrame(new IDrawable[] { drawableFrame });
            }


            return this;
        }
    }
}
