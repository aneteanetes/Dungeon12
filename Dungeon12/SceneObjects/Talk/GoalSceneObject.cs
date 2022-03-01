using Dungeon;
using Dungeon.Drawing;
using Dungeon.Drawing.Impl;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon.Types;
using Dungeon.View.Interfaces;
using Dungeon12.ECS.Components;
using Dungeon12.Entities.Talks;
using Dungeon12.SceneObjects.Base;
using System.Collections.Generic;

namespace Dungeon12.SceneObjects.Talk
{
    public class GoalSceneObject : SceneControl<Goal>, ITooltipedCustom
    {
        public GoalSceneObject(string dialogueId, Goal component) : base(component, true)
        {
            this.Image = $"Talk/Goals/{dialogueId}/{component.Icon}".AsmImg();
            Width = 50;
            Height = 50; Blur = true;

            this.AddChild(new EmptySceneObject()
            {
                Width = 25,
                Height = 25,
                Left = 25,
                Top = 25,
                ParticleEffects = new List<IEffectParticle>()
                {
                    new ParticleEffect()
                    {
                        Name="TalkGoal",
                        Once=true,
                        TriggerCount = 6,
                        Scale = .4,
                    }
                }
            });
        }

        public override bool IsMonochrome => !Component.IsAchived;

        public bool ShowTooltip => true;

        public override void Focus()
        {
            base.Focus();
        }

        public Tooltip GetTooltip() => new GoalTooltip(Component.Name, Component.Type);

        private class GoalTooltip : Tooltip
        {
            public GoalTooltip(string text, ReplicaType type) : base("      " + text, new Point(0, 0), DrawColor.White)
            {
                var t = ((int)type);
                this.AddChild(new ImageObject($"Talk/{t}.png")
                {
                    Width=25,
                    Height=25
                });
                //this.Image = .AsmImg();
            }
        }
    }
}
