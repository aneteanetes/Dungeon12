using Dungeon;
using Dungeon.Control.Pointer;
using Dungeon.Drawing.Impl;
using Dungeon.Entities.Animations;
using Dungeon.SceneObjects;
using Dungeon.Types;
using Dungeon.View.Interfaces;
using Dungeon12.Drawing.SceneObjects;
using Dungeon12.Map.Objects;
using System.Collections.Generic;

namespace Dungeon12.Servant.Effects.FaithShot
{
    public class Smash : AnimatedSceneObject<Avatar>
    {
        protected override bool Loop => false;

        public Smash(Avatar @object) : base(null, @object, "", new Rectangle(0, 0, 32, 32), null,false)
        {
            this.Left = @object.Position.X / 32;
            this.Top = @object.Position.Y / 32;

            var tileset = "Effects/faithshot.png".AsmImg();
            this.Image = tileset;
            this.Width = 1;
            this.Height = 1;
            this.SetAnimation(new Animation()
            {
                Size = new Point(32, 32),
                Frames = new List<Point>()
                {
                    new Point(0,0),
                    new Point(32,0),
                    new Point(64,0),
                    new Point(96,0),
                    new Point(128,0)
                },
                TileSet = tileset
            });

            this.AddChild(new Might());
        }

        protected override void OnAnimationStop()
        {
            this.Destroy?.Invoke();
        }

        protected override void Action(MouseButton mouseButton)
        {
        }

        protected override void DrawLoop()
        {
        }

        private class Might : EmptySceneObject
        {
            public Might()
            {
                this.Width = 1;
                this.Height = 1;

                this.Effects = new List<IEffect>()
                {
                    new ParticleEffect()
                    {
                        Name="FaithShot",
                        Scale = 1
                    }
                };
            }
        }
    }
}