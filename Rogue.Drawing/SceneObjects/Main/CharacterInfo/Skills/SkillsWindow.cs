namespace Rogue.Drawing.SceneObjects.Main.CharacterInfo
{
    using Rogue.Abilities;
    using Rogue.Control.Keys;
    using Rogue.Control.Pointer;
    using Rogue.Drawing.Impl;
    using Rogue.Drawing.SceneObjects.Base;
    using Rogue.Drawing.SceneObjects.Map;
    using Rogue.Drawing.SceneObjects.UI;
    using Rogue.Entites.Alive;
    using Rogue.View.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class SkillsWindow : DraggableControl<SkillsWindow>
    {
        public override bool CacheAvailable => false;

        public override bool AbsolutePosition => true;

        private PlayerSceneObject playerSceneObject;

        public SkillsWindow(PlayerSceneObject playerSceneObject)
        {
            playerSceneObject.BlockMouse = true;
            this.Destroy += () => playerSceneObject.BlockMouse = false;
            this.playerSceneObject = playerSceneObject;

            this.Image = "Rogue.Resources.Images.ui.vertical_title(17x12).png";

            this.Height = 17;
            this.Width = 12;

            this.Left = 22.5;
            this.Top = 2;

            var abils = playerSceneObject.GetAbilities();

            var left = abils.FirstOrDefault(x => x.AbilityPosition == Rogue.Abilities.Enums.AbilityPosition.Left);
            var right = abils.FirstOrDefault(x => x.AbilityPosition == Rogue.Abilities.Enums.AbilityPosition.Right);
            var q = abils.FirstOrDefault(x => x.AbilityPosition == Rogue.Abilities.Enums.AbilityPosition.Q);
            var e = abils.FirstOrDefault(x => x.AbilityPosition == Rogue.Abilities.Enums.AbilityPosition.E);

            this.AddChild(new SkillButton(left, OpenSkillInfo, this.ShowEffects,true));
            this.AddChild(new SkillButton(right, OpenSkillInfo, this.ShowEffects)
            {
                Left=3
            });
            this.AddChild(new SkillButton(q, OpenSkillInfo, this.ShowEffects)
            {
                Left = 6
            });
            this.AddChild(new SkillButton(e, OpenSkillInfo, this.ShowEffects)
            {
                Left = 9
            });

            OpenSkillInfo(left);
        }

        private SkillInfo skillInfo = null;

        private void OpenSkillInfo(Ability a)
        {
            if (skillInfo != null)
            {
                skillInfo.Destroy?.Invoke();
                this.RemoveChild(skillInfo);
            }

            skillInfo = new SkillInfo(playerSceneObject.Avatar.Character, a);
            this.AddChild(skillInfo);
        }
        
        protected override Key[] OverrideKeyHandles => new Key[] { Key.V, Key.X };

        public override void KeyDown(Key key, KeyModifiers modifier, bool hold)
        {
            if (key == Key.V || key== Key.X)
            {
                base.KeyDown(Key.Escape, modifier, hold);
            }

            base.KeyDown(key, modifier, hold);
        }

        private class SkillInfo : SceneObject //пока что т.к. дерево покажу пожжжжжжже
        {
            public SkillInfo(Character c, Ability ability)
            {
                this.Height = 15;
                this.Width = 12;
                this.Top = 2;

                var title = this.AddTextCenter(new DrawText(ability.Name), true, false);
                var top = MeasureText(title.Text).Y / 32 + 0.5;

                var descr = this.AddTextCenter(new DrawText(ability.Description).Montserrat());
                descr.Top = top;
                descr.Left = 0.5;

                top += MeasureText(descr.Text).Y/32 + 0.5;

                this.AddChild(new DarkRectangle() { Color = ConsoleColor.White, Opacity = 1, Left = 0.5, Width = this.Width - 1, Height = 0.05, Top = top - 0.25 });                

                var castType = this.AddTextCenter(new DrawText(ability.CastType.ToDisplay(), new DrawColor(ConsoleColor.Red)).Montserrat(), true);
                castType.Top = top;

                top += MeasureText(castType.Text).Y / 32 + 0.5;

                this.AddChild(new DarkRectangle() { Color = ConsoleColor.White, Opacity = 1, Left = 0.5, Width = this.Width - 1, Height = 0.05, Top = top - 0.25 });

                var resource = this.AddTextCenter(new DrawText($"{c.ResourceName}: {ability.Spend}", new DrawColor(c.ResourceColor)).Montserrat(), true);
                resource.Top = top;

                top += MeasureText(resource.Text).Y / 32 + 0.5;

                this.AddChild(new DarkRectangle() { Color = ConsoleColor.White, Opacity = 1, Left = 0.5, Width = this.Width - 1, Height = 0.05, Top = top - 0.25 });

                var usage = this.AddTextCenter(new DrawText($"Время действия: {ability.ActionType.ToDisplay()}", new DrawColor(ConsoleColor.Yellow)).Montserrat(), true);
                usage.Top = top;

                top += MeasureText(usage.Text).Y / 32 + 0.5;

                this.AddChild(new DarkRectangle() { Color = ConsoleColor.White, Opacity = 1, Left = 0.5, Width = this.Width - 1, Height = 0.05, Top = top - 0.25 });
                
                //ВОТ ТУТ СКЕЙЛЫ

                var location = this.AddTextCenter(new DrawText($"Где используется: {ability.CastLocation.ToDisplay()}", new DrawColor(ConsoleColor.Cyan)).Montserrat(), true);
                location.Top = top;

                top += MeasureText(location.Text).Y / 32 + 0.5;

                this.AddChild(new DarkRectangle() { Color = ConsoleColor.White, Opacity = 1, Left = 0.5, Width = this.Width - 1, Height = 0.05, Top = top - 0.25 });
            }
        }

        private class SkillButton : TooltipedSceneObject
        {
            private static Action<SkillButton> InactiveOther;

            private bool active = false;

            public override bool CacheAvailable => false;
            public override bool AbsolutePosition => true;

            private PlayerSceneObject playerSceneObject;
            private readonly Action<Ability> open;
            private bool disabled;
            private Ability ability;

            public SkillButton(Ability ability, Action<Ability> open, Action<List<ISceneObject>> showEffects, bool active = false) : base("Характеристики", showEffects)
            {
                InactiveOther += SetInactive;
                Destroy += () => { InactiveOther -= SetInactive; };

                this.ability = ability;
                this.active = active;
                this.disabled = ability == null;
                this.open = open;

                this.Height = 2;
                this.Width = 3;

                if (ability != null)
                {
                    this.AddTextCenter(new DrawText(ability.AbilityPosition.ToString()));
                }

                this.Image = SquareTexture(false);
            }

            private void SetInactive(SkillButton sb)
            {
                if (sb != this)
                {
                    this.active = false;
                    this.Image = SquareTexture(false);
                }
            }

            private string SquareTexture(bool focus)
            {
                if(disabled)
                    return $"Rogue.Resources.Images.ui.squareWeapon_h_d.png";

                var f = focus || active
                    ? "_f"
                    : "";

                return $"Rogue.Resources.Images.ui.squareWeapon_h{f}.png";
            }

            public override void Focus()
            {
                this.Image = SquareTexture(true);
                base.Focus();
            }

            public override void Unfocus()
            {
                this.Image = SquareTexture(false);
                base.Unfocus();
            }

            public override void KeyDown(Key key, KeyModifiers modifier, bool hold) => DoOpen();

            public override void Click(PointerArgs args) => DoOpen();

            private void DoOpen()
            {
                if (!disabled)
                {
                    active = true;
                    InactiveOther(this);
                    open?.Invoke(ability);
                }
            }
        }
    }
}
