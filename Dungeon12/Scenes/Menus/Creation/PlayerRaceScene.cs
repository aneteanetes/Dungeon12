namespace Dungeon12.Scenes.Menus.Creation
{
    using Dungeon;
    using Dungeon.Control.Keys;
    using Dungeon.Drawing;
    using Dungeon.Drawing.SceneObjects;
    using Dungeon12.Drawing.SceneObjects.UI;
    using Dungeon12.SceneObjects; using Dungeon.SceneObjects;
    using Dungeon.Scenes;
    using Dungeon.Scenes.Manager;
    using Dungeon12.Drawing.SceneObjects;
    using System;
    using System.Linq;

    public class PlayerRaceScene : GameScene<PlayerNameScene, PlayerOriginScene>
    {
        public PlayerRaceScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => true;

        public override void Init()
        {
            this.AddObject(new ImageControl("Dungeon12.Resources.Images.d12back.png"));

            this.AddObject(new HorizontalWindow()
            {
                Top = 3f,
                Left = 10f,
            });

            this.AddObject(new TextControl(new DrawText("Выберите расу", new DrawColor(ConsoleColor.White)) { Size = 50 })
            {
                Left=15.5,
                Top=3.5
            });

            double left = 11.25;
            var top = 6f;
            var column = 0;

            foreach (var race in Enum.GetValues(typeof(Race)).Cast<Race>().OrderBy(x=>(int)x))
            {
                this.AddObject(new MetallButtonControl(race.ToDisplay())
                {
                    Left = left,
                    Top = top,
                    OnClick = () =>
                     {
                         //this.PlayerAvatar.Character.Race = race;
                         this.Switch<PlayerOriginScene>();
                     }
                });

                top += 3;
                column++;

                if (column == 3)
                {
                    top = 6f;
                    left = 20.5;
                };
            }
        }

        protected override void KeyPress(Key keyPressed, KeyModifiers keyModifiers, bool hold)
        {
            if (keyPressed == Key.Escape && !hold)
            {
                this.Switch<PlayerNameScene>();
            }
        }
    }
}
