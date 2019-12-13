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
    using System;
    using Dungeon12.Scenes.Menus;

    public class EditorScene : GameScene<Start>
    {
        public override bool AbsolutePositionScene => false;

        //public override bool CameraAffect => true;

        public EditorScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => false;

        private SaveBtn saveBtn;

        private EditedGameField field;

        public override void Init()
        {
            Global.DrawClient.SetCameraSpeed(5);

            this.AddObject(new Background());

            this.AddObject(new DarkRectangle
            {
                Fill = false,
                Color = ConsoleColor.White,
                Opacity = 1,
                Left = 19.5,
                Top = -0.5,
                Width = 100.5,
                Height = 100.5
            });

            field = new EditedGameField
            {
                Left = 20
            };
            this.AddObject(field);
            this.AddObject(new ToolboxControl(field.Selecting, field.SetLevel, field.SetObstruct, field.SetFullTile));

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
            if (!hold)
            {
                if (keyPressed == Key.S && keyModifiers == KeyModifiers.Control)
                {
                    saveBtn.Click(null);
                }
                if (keyPressed == Key.Z && keyModifiers == KeyModifiers.Control)
                {
                    field.Cancel();
                }
            }

            if (keyPressed == Key.Escape)
            {
                Switch<Start>();
            }
        }
    }
}