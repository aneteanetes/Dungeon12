using Dungeon;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using Dungeon12.ECS.Components;
using Dungeon12.Entities;
using Dungeon12.Entities.Abilities;
using Dungeon12.Entities.Enums;
using Dungeon12.SceneObjects.Base;
using System.Linq;

namespace Dungeon12.SceneObjects.Create
{
    internal class Abilities : SceneControl<Hero>
    {
        public override void Throw(Exception ex)
        {
            throw ex;
        }
        public Abilities(Hero component) : base(component)
        {
            this.Height = 110;
            this.Width = 315;

            var title = this.AddTextCenter(Global.Strings["Abilities"].AsDrawText().Gabriela().InColor(Global.CommonColor).InSize(35), vertical: false);
            title.Top = -50;

            foreach (var archtype in typeof(Archetype).All<Archetype>())
            {
                var abils = Ability.ByClass(archtype);

                double left = 0;
                foreach (var abil in abils)
                {
                    var abilitem = this.AddChildCenter(new AbilityItem(Component, abil));
                    abilitem.Left = left;
                    left += 25 + 60;
                }
            }
        }

        internal class AbilityItem : SceneControl<Hero>, ITooltipedCustom, IMouseHint
        {
            Ability _ability;

            public override void Throw(Exception ex)
            {
                throw ex;
            }
            public AbilityItem(Hero component, Ability ability) : base(component)
            {
                _ability = ability;
                this.Width = 60;
                this.Height = 60;

                this.Image = "UI/start/icon.png".AsmImg();

                this.AddChild(new ImageObject($"Abilities/{ability.ClassName}.tga")
                {
                    Width = 56,
                    Height = 56,
                    Left = 2,
                    Top = 2
                });
            }

            public override bool Visible => Component.Archetype == _ability.Class;

            public IDrawText TooltipText => $"{Global.Strings[_ability.ClassName]} ({Global.Strings["RightMouseButton"]} - {Global.Strings["Info"]})".AsDrawText().WithOpacity(1.1).Gabriela();

            public bool ShowTooltip => true;

            public ISceneObjectHosted CreateMouseHint()
                => new ObjectPanel(_ability.Name, _ability.Description, _ability.Area,_ability.Cooldown,_ability.GetTextParams());

            public void RefreshTooltip() { }

            public ISceneObject GetTooltip()
            {
                var dur = RandomGlobal.Next(0, 2);
                var cd = RandomGlobal.Next(0, 5);

                return new GenericPanel(new Entities.Plates.GenericData()
                {
                    Icon = $"Abilities/{_ability.ClassName}.tga".AsmImg(),
                    Title=_ability.Name,
                    Rank = Global.Strings[Ranks.Novice],
                    Resources=new List<Entities.Plates.ResourceData>()
                    {
                        new Entities.Plates.ResourceData()
                        {
                            Title = "энергии",
                            Amount = "25%"
                        }
                    },
                    Radius = 2,
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
}
