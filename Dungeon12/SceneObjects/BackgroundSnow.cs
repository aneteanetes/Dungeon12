using Dungeon.Drawing.Impl;
using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;

namespace Nabunassar.Drawing.SceneObjects
{
    internal class BackgroundSnow : EmptySceneObject
    {
        public override bool CacheAvailable => false;

        public BackgroundSnow() : base()
        {
            this.AddChild(new BackgroundSnowParticle());
            this.AddChild(new BackgroundSnowParticle()
            {
                Left = Global.Resolution.Width / 2
            });
        }

        private class BackgroundSnowParticle : EmptySceneObject
        {
            public BackgroundSnowParticle() : base()
            {
                this.ParticleEffects.Add(new ParticleEffect()
                {
                    Name = "SnowFast",
                    Scale = 1.4
                });
            }
        }
    }
}