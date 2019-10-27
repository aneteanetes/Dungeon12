using Rogue.Control.Pointer;
using Rogue.Drawing.Impl;
using Rogue.Drawing.SceneObjects;
using Rogue.Drawing.SceneObjects.Map;
using Rogue.Entites.Animations;
using Rogue.Map.Objects;
using Rogue.Physics;
using Rogue.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rogue.Classes.Servant.Effects.FaithShot
{
    public class Smash : AnimatedSceneObject<Avatar>
    {
        protected override bool Loop => false;

        public Smash(Avatar @object) : base(null, @object, "", new Rectangle(0, 0, 32, 32), null)
        {
            this.Left = @object.Position.X / 32;
            this.Top = @object.Position.Y / 32;

            var tileset = "Effects/faithshot.png".PathAsmImg();
            this.Image = tileset;
            this.Width = 1;
            this.Height = 1;
            this.SetAnimation(new AnimationMap()
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

        private class Might : SceneObject
        {
            public Might()
            {
                this.Width = 1;
                this.Height = 1;

                this.Effects = new List<View.Interfaces.IEffect>()
                {
                    new ParticleEffect()
                    {
                        Name="FaithShot",
                        Scale = 1,
                        Assembly="Rogue.Classes.Servant"
                    }
                };
            }
        }
    }
}