using Dungeon;
using Dungeon.Drawing;
using Dungeon.View.Interfaces;
using Dungeon12.Drawing.SceneObjects.UI;
using System;

namespace Dungeon12.Bowman
{
    public class EnergyBar : ResourceBar<Bowman>
    {
        public EnergyBar(Bowman avatar):base(avatar)
        {
            var leftBar = new EnergyBarHand(avatar)
            {
                Left = 0.031,
                Top = 0.031,
                Height = 0.46875
            };
            this.AddChild(leftBar);
            this.AddChild(new EnergyBarHand(avatar,false)
            {
                Left = 0.031+ MeasureImage("energy.png".AsmImg()).X+0.1,
                Top = 0.031,
                Height = 0.46875
            });
        }

        protected override string BarTile => "Dungeon12.Resources.Images.ui.player.hp_back.png";

        private class EnergyBarHand : Dungeon.Drawing.SceneObjects.ImageObject
        {
            private Bowman archer;

            private IDrawText energyText;

            public bool left;

            public EnergyBarHand(Bowman archer,bool left=true) : base("energy.png".AsmImg())
            {
                this.left = left;
                this.archer = archer;

                if (left)
                {
                    energyText = new DrawText($"{archer.Energy.LeftHand}/{archer.Energy.LeftHand}", ConsoleColor.DarkYellow)
                    {
                        Size = 14
                    }.Montserrat();
                }
                else
                {
                    energyText = new DrawText($"{archer.Energy.RightHand}/{archer.Energy.RightHand}", ConsoleColor.DarkYellow)
                    {
                        Size = 14
                    }.Montserrat();
                }

                var text = this.AddTextCenter(energyText);
                text.Top += 0.2;
                text.Left += 0.2;
            }

            public override double Width
            {
                get
                {
                    if (left)
                    {
                        energyText.SetText($"{archer.Energy.LeftHand}/50");
                        return (4.75 / 2) * (((double)archer.Energy.LeftHand / 50 * 100) / 100);
                    }
                    else
                    {
                        energyText.SetText($"{archer.Energy.RightHand}/50");
                        return (4.75 / 2) * (((double)archer.Energy.RightHand / 50 * 100) / 100);
                    }
                }
                set { }
            }

            public override bool CacheAvailable => false;
        }
    }
}
