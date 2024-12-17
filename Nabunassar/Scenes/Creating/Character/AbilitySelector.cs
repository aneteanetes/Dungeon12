using MoreLinq.Extensions;
using Nabunassar.Entities;
using Nabunassar.Entities.Abilities;
using Nabunassar.SceneObjects.Base;
using Nabunassar.Scenes.Creating.Character.Names;

namespace Nabunassar.Scenes.Creating.Character
{
    internal class AbilitySelector : CreatePart
    {
        public AbilitySelector(Hero component) : base(component,Global.Strings["guide"]["abilities"])
        {
            Width = 325;
            Height = 700;
            Top = 300;
            Left = 50;

            this.AddBorderMapBack(new BorderConfiguration()
            {
                ImagesPath = "UI/bordermin/panel-border-022.png",
                Size = 16,
                Padding = 2
            });
        }

        public override bool Visible
        {
            get => base.Visible; set
            {
                if (value == true)
                    Init();
                base.Visible = value;
            }
        }

        public void Init()
        {

            var title = AddTextCenter(Global.Strings["AbilityChoose"].ToString().DefaultTxt(20));
            title.Top = 20;

            var combat = AddTextCenter(Global.Strings["CombatAbilities"].ToString().DefaultTxt(20));
            combat.Top = title.TopMax + 20;

            var desc = new NameDescriptionBlock();

            var t = combat.TopMax + 20;
            var left = 25;
            var combatabils = GetCombatAbilities().Batch(4);
            foreach (var part in combatabils)
            {
                foreach (var abil in part)
                {
                    this.AddChild(new NameAbilSelector(Component, abil.Icon, abil.Description, Global.Game.Creation.SelectedAbilityName, desc)
                    {
                        Left = left,
                        Top = t
                    });
                    left += 50;
                }
                left = 25;
                t += 50;
            }

            var globally = AddTextCenter(Global.Strings["GloballyAbilities"].ToString().DefaultTxt(20));
            globally.Top = t;

            t += 30;

            var globalabils = GetGloballyAbilities().Batch(4);
            foreach (var part in globalabils)
            {
                foreach (var abil in part)
                {
                    this.AddChild(new NameAbilSelector(Component, abil.Icon, abil.Description, Global.Game.Creation.SelectedAbilityName, desc)
                    {
                        Left = left,
                        Top = t
                    });
                    left += 50;
                }
                left = 25;

                t += 50;
            }
        }

        private IEnumerable<Ability> GetCombatAbilities()
        {
            return Ability.GetClassCombatAbilies(Component.Archetype.Value)
                .Concat(Ability.GetRaceCombatAbilities(Component));
        }

        private IEnumerable<Ability> GetGloballyAbilities()
        {
            return Ability.GetClassGloballyAbilies(Component.Archetype.Value)
                .Concat(Ability.GetRaceGloballyAbilities(Component))
                .Concat(Ability.GetFractionAbilies(Component));
        }
    }
}
