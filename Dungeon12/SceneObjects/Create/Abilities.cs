﻿using Dungeon;
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

        internal class AbilityItem : SceneControl<Hero>, ITooltipedDrawText, IMouseHint
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
        }
    }
}
