using Rogue.Drawing.SceneObjects;
using Rogue.Drawing.SceneObjects.UI;
using System.Linq;

namespace Dungeon12.Classes.Servant
{
    public class FaithPowerBar : ResourceBar<Servant>
    {
        public FaithPowerBar(Servant avatar) : base(avatar)
        {
            this.Width = 5;
            this.Height = 1;

            var left = 0.031+.25;
            var top = 0.031;
            var square= 0.9375;

            for (int i = 0; i < 4; i++)
            {
                this.AddChild(new FaithPowerOrb(avatar, i + 1)
                {
                    Left = left,
                    Top = top,
                    Height = square,
                    Width = square
                });

                left += square + (0.15625);
            }
        }

        protected override string BarTile => "";

        private class FaithPowerOrb : ImageControl
        {
            private Servant servant;
            int _count;
            public FaithPowerOrb(Servant servant,int count) : base("fpower.png".PathAsmImg())
            {
                _count = count;
                this.servant = servant;
            }

            public override bool CacheAvailable => false;

            public override bool Visible => servant.FaithPower.Value >= _count;
        }
    }
}
