using Dungeon;
using Dungeon12.Abilities.Talants.NotAPI;
using Dungeon12.Classes;
using Dungeon.Control;
using Dungeon.Drawing;
using Dungeon.Drawing.SceneObjects;
using Dungeon12.Drawing.SceneObjects.Map;
using Dungeon.Events;
using Dungeon.GameObjects;
using Dungeon12.SceneObjects; using Dungeon.SceneObjects;
using Dungeon.Types;
using Dungeon.View.Interfaces;
using System;
using System.Collections.Generic;

namespace Dungeon12.Drawing.SceneObjects.Main.CharacterInfo.Talants
{
    public class TalantInfoSceneControl : TooltipedSceneObject<TalantBase>
    {
        public override bool CacheAvailable => false;

        public override bool AbsolutePosition => true;

        protected override ControlEventType[] Handles => new ControlEventType[] { ControlEventType.Focus, ControlEventType.Click };

        private readonly bool activatable = false;

        private TalantBase talant;

        private bool Active => talant.Active;

        private readonly Character character;

        private bool blocked = false;

        public override bool HideCursor => true;

        public TalantInfoSceneControl(TalantBase talant, Character character, bool blocked) : base(talant, talant.Name)
        {
            this.blocked = blocked;
            this.character = character;
            this.talant = talant;
            var measure = this.MeasureImage("Dungeon12.Resources.Images.ui.square.png");

            this.AddChild(new ImageControl(talant.Image)
            {
                CacheAvailable=false,
                AbsolutePosition=true
            });

            if (!blocked)
            {
                this.AddChild(new DarkRectangle()
                {
                    Opacity = 0.7,
                    CacheAvailable = false,
                    AbsolutePosition = true,
                    Height = 0.75,
                    Width = 1,
                    Top = 1.25,
                    Left = 1
                }.WithText(new DrawText($"{talant.Level}/{talant.MaxLevel}", new DrawColor(ConsoleColor.White)).Montserrat(), true));
            }
            else
            {
                this.AddChild(new ImageControl("Talants/blocked.png".AsmImgRes())
                {
                    CacheAvailable = false,
                    AbsolutePosition = true
                });
            }

            this.Height = measure.Y;
            this.Width = measure.X;

            activatable = talant.Activatable && talant.Opened && !blocked;

            this.Image = SquareTexture(activatable ? talant.Active : false);

            if (activatable)
            {
                talant.ActiveChanged += value => this.Image = SquareTexture(value);
            }
        }

        protected override bool ProvidesTooltip => true;

        protected override Tooltip ProvideTooltip(Point position)
        {
            if (!this.blocked)
            {
                return new TalantTooltip(talant, character, position);
            }

            return new Tooltip("Заблокировано".AsDrawText().InSize(12).Montserrat(), position);
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

            return $"Dungeon12.Resources.Images.ui.square{f}.png";
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
    }
}