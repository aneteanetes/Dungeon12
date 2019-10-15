using Rogue.Drawing.Impl;
using Rogue.Drawing.SceneObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rogue.Classes.Bowman.Effects
{
    public class ArrowRain : SceneObject
    {
        public ArrowRain(Physics.PhysicalPosition physicalPosition)
        {
            this.Left = physicalPosition.X / 32;
            this.Top = physicalPosition.Y / 32;
            this.Width = 5;
            this.Height = 5;

            this.Effects = new List<View.Interfaces.IEffect>()
                {
                    new ParticleEffect()
                    {
                        Name="RainOfArrow",
                        Scale = 0.1,
                        Assembly="Rogue.Classes.Bowman"
                    }
                };
        }
    }
}
