namespace Rogue.Drawing.SceneObjects.Map
{
    using Rogue.Control.Events;
    using Rogue.Map;
    using Rogue.Map.Objects;
    using Rogue.Types;

    public class NPCSceneObject : MoveableSceneObject
    {
        protected override ControlEventType[] Handles =>  new ControlEventType[]
        {
             ControlEventType.Focus
        };

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