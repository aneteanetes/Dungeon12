namespace Rogue.Map.Editor
{
    using Rogue.Drawing.SceneObjects;
    using Rogue.Map.Editor.Camera;
    using Rogue.Map.Editor.Field;
    using Rogue.Map.Editor.Toolbox;
    using Rogue.Scenes;
    using Rogue.Scenes.Manager;

    public class EditorScene : GameScene
    {
        public override bool AbsolutePositionScene => false;

        //public override bool CameraAffect => true;

        public EditorScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => false;

        public override void Init()
        {
            this.AddObject(new Background());

            var field = new EditedGameField
            {
                Left = 20
            };
            this.AddObject(field);
            this.AddObject(new ToolboxControl(field.Selecting));

            this.AddObject(new SaveBtn(field)
            {
                Top=1
            });

            this.AddObject(new CameraScroller(Types.Direction.Left, "<")
            {
                Left = 1,
                Top = 21.5,
            });
            this.AddObject(new CameraScroller(Types.Direction.Right, ">")
            {
                Left = 3,
                Top = 21.5,
            });
            this.AddObject(new CameraScroller(Types.Direction.Down, "?")
            {
                Left = 2,
                Top = 21.5,
            });
            this.AddObject(new CameraScroller(Types.Direction.Up, "^")
            {
                Left = 2,
                Top = 20.5,
            });
        }
    }
}
