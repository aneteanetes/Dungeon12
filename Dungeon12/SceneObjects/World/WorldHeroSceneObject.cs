using Dungeon;
using Dungeon.Drawing;
using Dungeon.SceneObjects;
using Dungeon.Types;
using Dungeon12.Attributes;
using Dungeon12.Entities;

namespace Dungeon12.SceneObjects.World
{
    internal class WorldHeroSceneObject : SceneObject<Hero>
    {
        public override bool Updatable => true;

        public WorldHeroSceneObject(Hero component):base(component)
        {
            Color = DrawColor.White;

            this.Width=32;
            this.Height=32;
            this.Image = "units1.png";

            this.Light=new Light()
            {
                Color = DrawColor.White
                //Type= Dungeon.View.Interfaces.LightType.Texture,
                //Image = "TexturedLight.png".AsmImg()
            };
            Range=.7f;
            SetModel();
        }

        private void SetModel()
        {
            var offset = Component.Class.ValueAttr<EnumDotAttribute, Dot>();
            this.ImageRegion = new Square() { X=offset.X*32, Y=offset.Y*32, Height=32, Width=32 };
        }

        public void SetSlot(Compass compass)
        {
            switch (compass)
            {
                case Compass.North:
                    Left=0;
                    Top=-16;
                    break;
                case Compass.South:
                    Left=0;
                    Top=16;
                    break;
                case Compass.West:
                    Left=-16;
                    Top=0;
                    break;
                case Compass.East:
                    Left=16;
                    Top=0;
                    break;
                default:
                    break;
            }
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
