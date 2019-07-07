namespace Rogue.Drawing.SceneObjects.Map
{
    using Rogue.Control.Events;
    using Rogue.Control.Pointer;
    using Rogue.Drawing.SceneObjects.Dialogs.NPC;
    using Rogue.Drawing.SceneObjects.Effects;
    using Rogue.Map;
    using Rogue.Map.Objects;
    using Rogue.Types;
    using Rogue.View.Interfaces;
    using System.Collections.Generic;

    public class NPCSceneObject : MoveableSceneObject<NPC>
    {
        public NPCSceneObject(PlayerSceneObject playerSceneObject, GameMap location, NPC mob, Rectangle defaultFramePosition)
            : base(playerSceneObject, mob, location, mob, mob.NPCEntity, defaultFramePosition)
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

            LightTrigger = Global.Time
                .After(18).Do(AddTorchlight)
                .After(8).Do(RemoveTorchlight);
        }

        private TorchlightInHandsSceneObject torchlight;

        private void AddTorchlight()
        {
            torchlight = new TorchlightInHandsSceneObject();
            this.AddChild(torchlight);
        }

        private void RemoveTorchlight()
        {
            this.RemoveChild(torchlight);
            torchlight?.Destroy?.Invoke();
        }

        private readonly TimeTrigger LightTrigger;

        protected override void DrawLoop()
        {
            LightTrigger.Trigger();
            base.DrawLoop();
        }

        private NPCDialogue NPCDialogue;

        protected override void Action(MouseButton mouseButton)
        {
            playerSceneObject.StopMovings();
            NPCDialogue = new NPCDialogue(playerSceneObject,@object,this.DestroyBinding, this.ControlBinding);

            ShowEffects?.Invoke(NPCDialogue.InList<ISceneObject>());
        }

        protected override void StopAction()
        {
        }
    }
}