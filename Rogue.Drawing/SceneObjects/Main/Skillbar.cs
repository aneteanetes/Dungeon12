namespace Rogue.Drawing.SceneObjects.Main
{
    using Rogue.Abilities;
    using Rogue.Abilities.Enums;
    using Rogue.Abilities.Scaling;
    using Rogue.Control.Keys;
    using Rogue.Drawing.SceneObjects.Common;
    using Rogue.Map;
    using Rogue.View.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class SkillBar : SceneObject
    {
        public override bool AbsolutePosition => true;

        public SkillBar(Rogue.Map.Objects.Avatar avatar, GameMap gameMap, Action<IEnumerable<ISceneObject>> abilityEffects)
        {            
            var x = 4.9;

            var abilities = avatar.Character.GetInstancesFromAssembly<Ability>()
                .Select(a =>
                {
                    a.UseEffects = abilityEffects;
                    return a;
                });


            var left = abilities.FirstOrDefault(a => a.AbilityPosition == AbilityPosition.Left);
            var leftSkill = new SkillControl(gameMap, avatar, left, AbilityPosition.Left)
            {
                Left = x,
                Top = 1
            };
            this.AddChild(leftSkill);

            var q = abilities.FirstOrDefault(a => a.AbilityPosition == AbilityPosition.Q);
            var QSkill = new SkillControl(gameMap, avatar, q, AbilityPosition.Q)
            {
                Left = leftSkill.Left+3,
                Top = 2
            };
            this.AddChild(QSkill);

            var e = abilities.FirstOrDefault(a => a.AbilityPosition == AbilityPosition.E);
            var ESkill = new SkillControl(gameMap, avatar, e, AbilityPosition.E)
            {
                Left = QSkill.Left + 2,
                Top = 2
            };

            this.AddChild(ESkill);

            var right = abilities.FirstOrDefault(a => a.AbilityPosition == AbilityPosition.Right);
            var rightSkill = new SkillControl(gameMap, avatar, right, AbilityPosition.Right)
            {
                Left = ESkill.Left + 2,
                Top = 1
            };
            this.AddChild(rightSkill);
        }

        private static Dictionary<int, Key> KeyMapping => new Dictionary<int, Key>()
        {
            {0, Key.D1 },
            {1, Key.D2 },
            {2, Key.D3 },
            {3, Key.D4},
            {4, Key.D5},
            {5, Key.D6}
        };
    }
}
