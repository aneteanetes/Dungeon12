using Rogue.Abilities.Talants.NotAPI;
using Rogue.Control.Events;
using Rogue.Drawing.Impl;
using Rogue.Drawing.SceneObjects.Base;
using Rogue.Drawing.SceneObjects.Map;
using Rogue.View.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rogue.Drawing.SceneObjects.Main.CharacterInfo.Talants
{
    public class TalantInfoSceneControl : TooltipedSceneObject
    {
        public override bool CacheAvailable => false;

        public override bool AbsolutePosition => true;

        protected override ControlEventType[] Handles => new ControlEventType[] { ControlEventType.Focus };

        public TalantInfoSceneControl(TalantBase talant, Action<List<ISceneObject>> showEffects) : base(talant.Name, showEffects)
        {
            this.Image = SquareTexture(false);
            var measure = this.MeasureImage("Rogue.Resources.Images.ui.square.png");

            this.AddChild(new DarkRectangle()
            {
                Opacity=0.7,
                CacheAvailable = false,
                AbsolutePosition = true,
                Height = 0.5,
                Width = 0.5,
                Top=1.25,
                Left=1.25
            }.WithText(new DrawText(talant.Level.ToString(), new DrawColor(ConsoleColor.White)).Montserrat(),true));

            this.Height = measure.Y;
            this.Width = measure.X;
        }

        private string SquareTexture(bool focus)
        {
            var f = focus
                ? "_f"
                : "";

            return $"Rogue.Resources.Images.ui.square{f}.png";
        }
    }
}
