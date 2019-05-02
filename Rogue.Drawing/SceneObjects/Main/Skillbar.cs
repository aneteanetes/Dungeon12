namespace Rogue.Drawing.SceneObjects.Main
{
    using Rogue.Abilities;
    using Rogue.Abilities.Scaling;
    using Rogue.Control.Keys;
    using Rogue.Drawing.SceneObjects.Common;
    using Rogue.Map;
    using Rogue.Utils.ReflectionExtensions;
    using Rogue.View.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class SkillBar : SceneObject
    {
        public override bool AbsolutePosition => true;

        public SkillBar(Rogue.Map.Objects.Avatar player, GameMap gameMap, Action<IEnumerable<ISceneObject>> abilityEffects)
        {
            //this.Children.Add(new ImageControl("Rogue.Resources.Images.ui.sphere.png"));
            //this.Children.Add(new ImageControl("Rogue.Resources.Images.ui.sphere.png")
            //{
            //    Left = 15.5f
            //});

            var x = 4.9;

            var typeName = player.Character.GetType().Name;
            var abilities = "Ability".GetInstancesFromAssembly<Ability>($"Rogue.Classes.{typeName}", (string abil, Assembly asm) =>
            {
                return asm.GetTypes().Where(t => typeof(Ability).IsAssignableFrom(t));
            })
            .Select(a =>
            {
                a.UseEffects = abilityEffects;
                return a;
            })
            .OrderBy(a => a.Position).ToList();

            abilities.Add(new abil());

            for (int i = 0; i < 6; i++)
            {
                this.AddChild(new SkillControl(KeyMapping[i], gameMap, player, abilities[i])
                {
                    Left = x,
                    Top = 2
                });

                x += 2;
            }
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

        private class abil : Ability
        {
            public override int Position => throw new System.NotImplementedException();

            public override string Name => throw new System.NotImplementedException();

            public override ScaleRate Scale => throw new System.NotImplementedException();
        }
    }
}
