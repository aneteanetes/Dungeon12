namespace Rogue.Drawing.SceneObjects.Map
{
    using Rogue.Drawing.GUI;
    using Rogue.Drawing.SceneObjects.Base;
    using Rogue.Drawing.SceneObjects.UI;
    using Rogue.Entites.Alive;
    using Rogue.Entites.Animations;
    using Rogue.Entites.Enemy;
    using Rogue.Map;
    using Rogue.Map.Objects;
    using Rogue.Types;
    using Rogue.View.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Timers;

    public class NPCSceneObject : MoveableSceneObject
    {
        public NPCSceneObject(GameMap location, NPC mob, Rectangle defaultFramePosition) : base(location, mob, mob.NPCEntity, defaultFramePosition, null)
        {
            this.Image = mob.Tileset;
            Left = mob.Location.X;
            Top = mob.Location.Y;
            Width = 1;
            Height = 1;

            mob.Die += () =>
            {
                this.Destroy?.Invoke();
            };

            if (mob.NPCEntity.Idle != null)
            {
                this.SetAnimation(mob.NPCEntity.Idle);
            }
        }
    }
}