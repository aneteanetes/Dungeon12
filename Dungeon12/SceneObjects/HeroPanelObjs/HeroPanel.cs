using Dungeon;
using Dungeon.Control;
using Dungeon.Drawing;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon12.ECS.Components;
using Dungeon12.Entities;
using System.Reflection.Emit;

namespace Dungeon12.SceneObjects.HeroPanelObjs
{
    internal class HeroPanel : SceneControl<Hero>
    {
        public override void Throw(Exception ex)
        {
            throw ex;
        }

        public HeroPanel(Hero component, bool isPeaceful=false) : base(component)
        {
            this.Width = 360;
            this.Height = 230;

            this.AddChild(new ImageControl("UI/char/backcut.png")
            {
                Width=360,
                Height=150,
                Top=75
            });

            this.AddChild(new ImageControl(component.Avatar)
            {
                Width = 87,
                Height = 140,
                Top = 80,
                Left = 4
            });

            var name = this.AddTextCenter(component.Name.AsDrawText().Gabriela().InColor(Global.CommonColor).InSize(20));
            name.Left = 105;
            name.Top = 85;

            var abilLeft = 0d;

            this.AddChild(new ValueBar(component, true)
            {
                Left=110,
                Top=160
            });
            this.AddChild(new ValueBar(component, false)
            {
                Left=110,
                Top=190
            });

            int i = 1;

            foreach (var abil in component.Abilities)
            {
                var abilscnobj = this.AddChild(new AbilityItemBig(Component, abil)
                {
                    Left = abilLeft
                });                

                abilLeft += abilscnobj.Width+7;
                i++;
            }

            this.AddChild(new SmallBtn(component,"Обороняться","def.tga") { Left = this.Width-32 });
            this.AddChild(new SmallBtn(component,"Ждать","wait.tga") { Left = this.Width-64 });
            this.AddChild(new SkipBtn(component) { Left=this.Width-64, Top=32 });
        }



        private class WaitBtn : SceneControl<Hero>, ITooltiped, ITooltipedPositionByComponent
        {
            public WaitBtn(Hero component) : base(component)
            {
                this.Width = 32;
                this.Height = 32;
                this.Image="UI/start/icon.png";
                this.AddChild(new ImageObject("Icons/MUD/wait.tga") { Width=30, Height=30, Left=1, Top=1, IsMonochrome=true });
            }

            public string TooltipText => "Ждать";

            public override void Focus()
            {
                this.Image="UI/start/classselector.png";
                base.Focus();
            }

            public override void Unfocus()
            {
                this.Image="UI/start/icon.png";
                base.Unfocus();
            }
        }

        private class SmallBtn : SceneControl<Hero>, ITooltiped, ITooltipedPositionByComponent
        {
            ImageObject icon;

            public SmallBtn(Hero component, string tooltip, string iconFileTga) : base(component)
            {
                TooltipText=tooltip;
                this.Width = 32;
                this.Height = 32;
                this.Image="UI/start/icon.png";
                icon = this.AddChild(new ImageObject($"Icons/MUD/{iconFileTga}") { Width=30, Height=30, Left=1, Top=1 });
            }

            private bool IsDisabled = false;

            public override void Update(GameTimeLoop gameTime)
            {
                icon.IsMonochrome=!Component.IsActive;
                IsDisabled=!Component.IsActive;

                base.Update(gameTime);
            }

            public string TooltipText { get; set; }

            public override void Focus()
            {
                if (IsDisabled)
                    return;

                this.Image="UI/start/classselector.png";
                base.Focus();
            }

            public override void Unfocus()
            {
                if (IsDisabled)
                    return;

                this.Image="UI/start/icon.png";
                base.Unfocus();
            }
        }

        private class SkipBtn : SceneControl<Hero>, ITooltiped, ITooltipedPositionByComponent
        {
            TextObject txt;
            ImageObject focus;

            public SkipBtn(Hero component) : base(component)
            {
                this.Width = 64;
                this.Height = 32;
                this.Image="UI/start/icon.png";
                focus = this.AddChild(new ImageObject("UI/start/classselector.png") { Width=64, Height=32, Visible=false });
                txt = this.AddTextCenter("Пропустить".AsDrawText().InBold().Calibri().InSize(11));
            }

            private bool isDisabled = false;

            public override void Update(GameTimeLoop gameTime)
            {
                isDisabled=!Component.IsActive;

                txt.Text.ForegroundColor = Component.IsActive == true
                    ? DrawColor.White
                    : DrawColor.Gray;

                base.Update(gameTime);
            }

            public string TooltipText => "Пропустить ход";

            public override void Focus()
            {
                if (isDisabled)
                    return;

                txt.Text.ForegroundColor = Global.CommonColor;
                focus.Visible=true;
                base.Focus();
            }

            public override void Unfocus()
            {
                if (isDisabled)
                    return;

                txt.Text.ForegroundColor = DrawColor.White;
                focus.Visible=false;
                base.Unfocus();
            }
        }

        public override void Click(PointerArgs args)
        {
        }

        public override void Drawing()
        {
            base.Drawing();
        }
    }
}
