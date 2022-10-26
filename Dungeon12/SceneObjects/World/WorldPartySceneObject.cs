using Dungeon;
using Dungeon.Drawing;
using Dungeon.Types;

namespace Dungeon12.SceneObjects.World
{
    public class WorldPartySceneObject : WorldTileSceneObject
    {
        public override bool Updatable => true;

        public WorldPartySceneObject()
        {
            Color = DrawColor.White;
            Width = WorldSettings.cellSize;
            Height = WorldSettings.cellSize;
            Image = "units1.png";

            ImageRegion = new Square()
            {
                Width = 32,
                Height = 32,
                X=1*32,
                Y=3*32
            };

            this.Light=new Light()
            {
                Color = DrawColor.White
                //Type= Dungeon.View.Interfaces.LightType.Texture,
                //Image = "TexturedLight.png".AsmImg()
            };
            Range=1;
        }

        public Dot Coords { get; set; } = new Dot();

        bool plus = true;

        private float speed = .0005f;
        private float limit = .03f;

        private float _range;
        public float Range
        {
            get => _range;
            set
            {
                if (value<_range)
                    limit-=.05f;
                else if (value>_range)
                    limit+=.05f;

                _range = value;
                this.Light.Range=value;
            }
        }

        public override void Update(GameTimeLoop gameTime)
        {
            this.Light.Range+=speed*(plus ? 1 : -1);

            if (this.Light.Range>(Range+limit))
                plus=false;
            if (this.Light.Range<(Range-limit))
                plus=true;


            base.Update(gameTime);
        }
    }
}
