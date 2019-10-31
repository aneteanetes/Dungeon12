namespace Dungeon12.Drawing.SceneObjects.Main
{
    using Dungeon.Abilities;
    using Dungeon.Abilities.Enums;
    using Dungeon.Abilities.Scaling;
    using Dungeon.Control.Keys;
    using Dungeon.Drawing.SceneObjects.Common;
    using Dungeon.Drawing.SceneObjects.Map;
    using Dungeon.Events;
    using Dungeon.Map;
    using Dungeon.Map.Objects;
    using Dungeon.View.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class SkillBar : HandleSceneControl
    {
        public override bool AbsolutePosition => true;

        private GameMap gameMap;
        private Action<List<ISceneObject>> abilityEffects;
        private Action<ISceneObject> destroyBinding;
        private Action<ISceneObjectControl> controlBinding;
        private PlayerSceneObject player;

        public SkillBar(PlayerSceneObject player, GameMap gameMap, Action<List<ISceneObject>> abilityEffects, Action<ISceneObject> destroyBinding, Action<ISceneObjectControl> controlBinding)
        {
            this.gameMap = gameMap;
            this.player = player;
            this.abilityEffects = abilityEffects;
            this.controlBinding = controlBinding;
            this.destroyBinding = destroyBinding;

            BindAbilities(player, gameMap, abilityEffects, destroyBinding, controlBinding);
        }

        protected override void CallOnEvent(dynamic obj)
        {
            OnEvent(obj);
        }

        public override bool Visible => !gameMap.InSafe(player.Avatar);

        public void OnEvent(ClassChangeEvent @event)
        {            
            this.ClearChildrens();
            BindAbilities(@event.PlayerSceneObject.As<PlayerSceneObject>(), @event.GameMap.As<GameMap>(), abilityEffects, destroyBinding, controlBinding);
        }

        private void BindAbilities(PlayerSceneObject player, GameMap gameMap, Action<List<ISceneObject>> abilityEffects, Action<ISceneObject> destroyBinding, Action<ISceneObjectControl> controlBinding)
        {
            var x = 4.9;

            var abilities = player.GetAbilities();
            foreach (var ability in abilities)
            {
                ability.UseEffects = this.ShowEffects;
            }

            var left = abilities.FirstOrDefault(a => a.AbilityPosition == AbilityPosition.Left);
            var leftSkill = new SkillControl(gameMap, player, left, AbilityPosition.Left, abilityEffects, destroyBinding, controlBinding)
            {
                Left = x,
                Top = 2
            };
            this.AddChild(leftSkill);

            var q = abilities.FirstOrDefault(a => a.AbilityPosition == AbilityPosition.Q);
            var QSkill = new SkillControl(gameMap, player, q, AbilityPosition.Q, abilityEffects, destroyBinding, controlBinding)
            {
                Left = leftSkill.Left + 2.5,
                Top = 2
            };
            this.AddChild(QSkill);

            var e = abilities.FirstOrDefault(a => a.AbilityPosition == AbilityPosition.E);
            var ESkill = new SkillControl(gameMap, player, e, AbilityPosition.E, abilityEffects, destroyBinding, controlBinding)
            {
                Left = QSkill.Left + 2,
                Top = 2
            };

            this.AddChild(ESkill);

            var right = abilities.FirstOrDefault(a => a.AbilityPosition == AbilityPosition.Right);
            var rightSkill = new SkillControl(gameMap, player, right, AbilityPosition.Right, abilityEffects, destroyBinding, controlBinding)
            {
                Left = ESkill.Left + 2.5,
                Top = 2
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
