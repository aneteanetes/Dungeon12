using Dungeon;
using Dungeon.SceneObjects;
using Dungeon12.Entities.Talks;
using Dungeon12.SceneObjects.UserInterface.CraftSelect;

namespace Dungeon12.SceneObjects.Talk
{
    public class DialogueSceneObject : SceneControl<Dialogue>
    {
        public DialogueSceneObject(Dialogue component) : base(component)
        {
            Global.Freezer.Freeze(this);

            if (component.Id == "NPCApprenticeShip")
                DynamicDialogue(component);

            this.Width = 1360;
            this.Height = 900;

            var reply = this.AddChild(new ReplySceneObject(null)
            {
                Top = 623
            });

            this.AddChild(new MainTextSceneObject(reply));

            this.AddChild(new ThemeSceneObject(Component)
            {
                Left = 959
            });

            this.AddChild(new CraftOptButton(true)
            {
                Left = 1326,
                Top = -38,
                PerPixelCollision = true,
                OnClick = Close
            });
        }

        private void DynamicDialogue(Dialogue component)
        {
            var spec = Global.DemoSpecNPC();
            component.Name = component.Name.Replace("%name%", spec.ToValue<string>());
            component.Avatar = spec.ToString() + ".png";
        }

        private void Close()
        {
            Global.Freezer.Unfreeze();
            this.Destroy?.Invoke();
        }
    }
}