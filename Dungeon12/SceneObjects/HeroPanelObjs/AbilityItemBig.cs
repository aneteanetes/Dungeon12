using Dungeon;
using Dungeon.Drawing;
using Dungeon.Drawing.SceneObjects;
using Dungeon.Localization;
using Dungeon.View.Interfaces;
using Dungeon12.ECS.Components;
using Dungeon12.Entities;
using Dungeon12.Entities.Abilities;
using Dungeon12.Entities.Enums;
using Dungeon12.SceneObjects.Base;

namespace Dungeon12.SceneObjects.HeroPanelObjs
{
    internal class AbilityItemBig : SceneControl<Hero>,/* ITooltipedDrawText, IMouseHint,*/ ICursored, ITooltipedCustom
    {
        public override void Throw(Exception ex)
        {
            throw ex;
        }
        Ability _ability;

        private string _title;

        private ImageObject _icon;

        public AbilityItemBig(Hero component, Ability ability) : base(component)
        {
            _ability = ability;
            _title = _ability.ClassName;
            this.Width = 64;
            this.Height = 64;

            this.Image = "UI/start/icon.png".AsmImg();

            var img = $"Abilities/{ability.ClassName}.tga";

            _icon = this.AddChild(new ImageObject(img)
            {
                Width = this.Width-4,
                Height = this.Height-4,
                Left = 2,
                Top = 2
            });
        }

        public override double Width
        {
            get => base.Width;
            set
            {
                if (_icon?.Width!=null)
                {
                    _icon.Width = value-4;
                }

                base.Width=value;
            }
        }

        public override double Height
        {
            get => base.Height;
            set
            {
                if (_icon?.Height!=null)
                {
                    _icon.Height = value-4;
                }
                base.Height=value;
            }
        }

        public override void Focus()
        {
            Image = "UI/start/classselector.png".AsmImg();
            base.Focus();
        }

        public override void Unfocus()
        {
            Image = "UI/start/icon.png".AsmImg();
            base.Unfocus();
        }

        public override bool Visible => Component.Archetype == _ability.Class;

        public IDrawText TooltipText => $"{Global.Strings[_title]}".AsDrawText().WithOpacity(1.1).Gabriela();

        public bool ShowTooltip => true;

        public CursorImage Cursor => CursorImage.Question;

        public ISceneObjectHosted CreateMouseHint()
            => new ObjectPanel(_ability.Name, _ability.Description,_ability.Area, _ability.Cooldown, _ability.GetTextParams());

        public ISceneObject GetTooltip()
        {
            var dur =  RandomGlobal.Next(0, 2);
            var cd = RandomGlobal.Next(0, 5);

            return new GenericPanel(new Entities.Plates.GenericData()
            {
                Icon = $"Abilities/{_ability.ClassName}.tga".AsmImg(),
                Title=_ability.Name,
                Subtype = "Ability".Localized(),
                Rank = "1 Уровень",
                Resources=new List<Entities.Plates.ResourceData>()
                {
                    new Entities.Plates.ResourceData()
                    {
                        Title = "энергии",
                        Amount = "20%"
                    }
                },
                Radius = 3,
                Duration=new Entities.Plates.DurationData()
                {
                    Duration = (Duration)dur,
                    Value =  dur == 2 ? RandomGlobal.Next(1, 3) : 0
                },
                Cooldown=new Entities.Cooldowns.Cooldown()
                {
                    Type = (Entities.Cooldowns.CooldownType)cd,
                    Value = cd == 1 ? 0 : RandomGlobal.Next(1, 3)
                },
                Charges = 2,
                Requires=new List<Entities.Plates.RequiredData>()
                {
                    new Entities.Plates.RequiredData()
                    {
                        Text="Требуется: "+Component.Archetype.Display()
                    }
                },
                RequiresLevel = 1,
                Fraction = Fraction.Vanguard,
                Text = _ability.Description,
                //Rune=new Entities.Runes.Rune()
                //{
                //    Name="Руна Жара",
                //    SetName = "Цветение огня"
                //}
            });
        }
    }
}
