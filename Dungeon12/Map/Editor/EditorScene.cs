namespace Dungeon12.Map.Editor
{
    using Dungeon.Control.Keys;
    using Dungeon.Drawing.SceneObjects;
    using Dungeon12.Map.Editor.Camera;
    using Dungeon12.Map.Editor.Field;
    using Dungeon12.Map.Editor.Toolbox;
    using Dungeon.Scenes;
    using Dungeon.Scenes.Manager;
    using Dungeon.Settings;
    using Dungeon;
    using Dungeon12.Drawing.SceneObjects;

    public class EditorScene : GameScene
    {
        public override bool AbsolutePositionScene => false;

        //public override bool CameraAffect => true;

        public EditorScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => false;

        private SaveBtn saveBtn;

        public override void Init()
        {
            Global.DrawClient.SetCameraSpeed(5);

            this.AddObject(new Background());

            var field = new EditedGameField
            {
                Left = 20
            };
            this.AddObject(field);
            this.AddObject(new ToolboxControl(field.Selecting,field.SetLevel,field.SetObstruct,field.SetFullTile));

            saveBtn = new SaveBtn(field)
            {
                Top = 1
            };
            this.AddObject(saveBtn);

            this.AddObject(new CameraScroller(Dungeon.Types.Direction.Left, "<", Dungeon.Control.Keys.Key.A)
            {
                Left = 1,
                Top = 21.5,
            });
            this.AddObject(new CameraScroller(Dungeon.Types.Direction.Right, ">", Dungeon.Control.Keys.Key.D)
            {
                Left = 3,
                Top = 21.5,
            });
            this.AddObject(new CameraScroller(Dungeon.Types.Direction.Down, "?", Dungeon.Control.Keys.Key.S)
            {
                Left = 2,
                Top = 21.5,
            });
            this.AddObject(new CameraScroller(Dungeon.Types.Direction.Up, "^", Dungeon.Control.Keys.Key.W)
            {
                Left = 2,
                Top = 20.5,
            });
        }

        protected override void KeyPress(Key keyPressed, KeyModifiers keyModifiers, bool hold)
        {
            if(!hold)
            {
                if(keyPressed== Key.S && keyModifiers== KeyModifiers.Control)
                {
                    saveBtn.Click(null);
                }
            }
        }
    }
}
