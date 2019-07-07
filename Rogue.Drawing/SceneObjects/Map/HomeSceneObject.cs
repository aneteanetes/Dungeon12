namespace Rogue.Drawing.SceneObjects.Map
{
    using Rogue.Control.Keys;
    using Rogue.Control.Pointer;
    using Rogue.Drawing.SceneObjects.Dialogs.NPC;
    using Rogue.Map.Objects;
    using Rogue.View.Interfaces;

    public class HomeSceneObject : ClickActionSceneObject<Home>
    {
        public HomeSceneObject(PlayerSceneObject playerSceneObject, Home home, string tooltip) 
            : base(playerSceneObject,home, tooltip)
        {
            Left = home.Location.X;
            Top = home.Location.Y;
            Width = 1;
            Height = 1;
        }

        private NPCDialogue NPCDialogue;

        protected override void Action(MouseButton mouseButton)
        {
            playerSceneObject.StopMovings();
            NPCDialogue = new NPCDialogue(playerSceneObject,@object, this.DestroyBinding, this.ControlBinding);

            ShowEffects?.Invoke(NPCDialogue.InList<ISceneObject>());
        }

        protected override void StopAction() { }

        protected override Key[] KeyHandles => new Key[] { Key.LeftAlt };
    }
}