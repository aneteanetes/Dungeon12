using Dungeon;
using Dungeon.Drawing;
using Dungeon.SceneObjects;
using Dungeon.Types;
using Dungeon12.Attributes;
using Dungeon12.Entities;
using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Dungeon12.SceneObjects.World
{
    internal class WorldHeroSceneObject : SceneObject<Hero>
    {
        public override bool Updatable => true;

        public WorldHeroSceneObject(Hero component):base(component)
        {
            Color = DrawColor.White;

            this.Width=WorldSettings.cellSize;
            this.Height=WorldSettings.cellSize;

            this.AddChild(new Cirlce(component)
            {
                Top=24
            });
            this.AddChild(new Impl(component));
        }

        public void SetSlot(Compass compass)
        {
            var value = WorldSettings.cellSize/2;
            switch (compass)
            {
                case Compass.North:
                    Left=0;
                    Top=-value;
                    break;
                case Compass.South:
                    Left=0;
                    Top=value;
                    break;
                case Compass.West:
                    Left=-value;
                    Top=0;
                    break;
                case Compass.East:
                    Left=value;
                    Top=0;
                    break;
                default:
                    break;
            }
        }

        private class Impl : SceneObject<Hero>
        {
            public Impl(Hero component) : base(component)
            {
                this.Width=WorldSettings.cellSize;
                this.Height=WorldSettings.cellSize;
                this.Image = "units1.png";

                this.Light=new Light()
                {
                    Color = DrawColor.White
                    //Type= Dungeon.View.Interfaces.LightType.Texture,
                    //Image = "TexturedLight.png".AsmImg()
                };
                Range=1f;
                SetModel();
            }

            private void SetModel()
            {
                var offset = Component.Class.ValueAttr<EnumDotAttribute, Dot>();
                this.ImageRegion = new Square() { X=offset.X*32, Y=offset.Y*32, Height=32, Width=32 };
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

        private class Cirlce : SceneObject<Hero>
        {
            public Cirlce(Hero component) : base(component)
            {
                this.Width=WorldSettings.cellSize;
                this.Height=29;// 14.5;
                this.Image = "effects/herocircle.png";
                this.ImageRegion=new Square(0, 0, WorldSettings.cellSize, 29);

                var anim = new Dungeon.View.Animation() { Frames=new System.Collections.Generic.List<Dot>() };
                anim.Frames.AddRange(Enumerable.Range(0, 5).Select(x => new Dot(x*32,0)));
                anim.Frames.AddRange(Enumerable.Range(0, 5).Select(x => new Dot(x*32,29)));
                anim.Frames.AddRange(Enumerable.Range(0, 5).Select(x => new Dot(x*32,29*2)));

                anim.TileSet="effects/herocircle.png".AsmImg();
                anim.Size=new Dot(64, 29);
                anim.Loop=true;

                this.PlayAnimation(anim);
            }

            public override bool Visible => true;
        }       

    }
}
