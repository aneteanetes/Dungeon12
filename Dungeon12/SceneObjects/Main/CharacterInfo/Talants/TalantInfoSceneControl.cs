using Dungeon.Abilities.Talants.NotAPI;
using Dungeon.Classes;
using Dungeon.Control.Events;
using Dungeon.Control.Pointer;
using Dungeon.SceneObjects;
using Dungeon.Drawing.Impl;
using Dungeon.Drawing.SceneObjects;
using Dungeon.Drawing.SceneObjects.Map;
using Dungeon.Events;
using Dungeon.Types;
using Dungeon.View.Interfaces;
using System;using Dungeon;using Dungeon.Drawing.SceneObjects;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Drawing.SceneObjects.Main.CharacterInfo.Talants
{
    public class TalantInfoSceneControl : TooltipedSceneObject
    {
        public override bool CacheAvailable => false;

        public override bool AbsolutePosition => true;

        protected override ControlEventType[] Handles => new ControlEventType[] { ControlEventType.Focus, ControlEventType.Click };

        private readonly bool activatable = false;

        private TalantBase talant;

        private bool Active => talant.Active;

        private readonly Character character;

        public TalantInfoSceneControl(TalantBase talant, Character character, Action<List<ISceneObject>> showEffects) : base(talant.Name, showEffects)
        {
            this.character = character;
            this.talant = talant;
            var measure = this.MeasureImage("Dungeon.Resources.Images.ui.square.png");

            this.AddChild(new ImageControl(talant.Image)
            {
                CacheAvailable=false,
                AbsolutePosition=true
            });

            this.AddChild(new DarkRectangle()
            {
                Opacity=0.7,
                CacheAvailable = false,
                AbsolutePosition = true,
                Height = 0.75,
                Width = 1,
                Top=1.25,
                Left=1
            }.WithText(new DrawText($"{talant.Level}/{talant.MaxLevel}", new DrawColor(ConsoleColor.White)).Montserrat(),true));

            this.Height = measure.Y;
            this.Width = measure.X;

            activatable = talant.Activatable && talant.Opened;

            this.Image = SquareTexture(activatable ? talant.Active : false);

            if (activatable)
            {
                talant.ActiveChanged += value => this.Image = SquareTexture(value);
            }
        }

        protected override bool ProvidesTooltip => true;

        protected override Tooltip ProvideTooltip(Point position)
        {
            return new TalantTooltip(talant,character, position);
        }

        public override void Click(PointerArgs args)
        {
            if (activatable)
            {
                talant.Active = !talant.Active;
                if (!talant.Active)
                {
                    this.Image = SquareTexture(false);
                }
            }
        }

        private string SquareTexture(bool focus)
        {
            var f = focus
                ? "_f"
                : "";

            return $"Dungeon.Resources.Images.ui.square{f}.png";
        }

        public override void Focus()
        {
            if(activatable)
            {
                this.Image = SquareTexture(true);
            }
            base.Focus();
        }

        public override void Unfocus()
        {
            if (activatable)
            {
                this.Image = SquareTexture(activatable ? Active : false);
            }
            base.Unfocus();
        }

        protected override void CallOnEvent(dynamic obj)
        {
            OnEvent(obj);
        }

        public void OnEvent(ClassChangeEvent @event)
        {

        }
    }
}