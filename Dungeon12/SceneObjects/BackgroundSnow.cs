using Dungeon.Drawing.Impl;
using Dungeon.SceneObjects;

namespace Dungeon12.Drawing.SceneObjects
{
    public class BackgroundSnow : EmptySceneObject
    {
        public override bool CacheAvailable => false;

        public BackgroundSnow()
        {
            this.AddChild(new BackgroundSnowParticle());
            this.AddChild(new BackgroundSnowParticle()
            {
                Left = Global.Resolution.Width / 2
            });
        }

        private class BackgroundSnowParticle : EmptySceneObject
        {
            public BackgroundSnowParticle()
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