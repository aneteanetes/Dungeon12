namespace Rogue.Scenes.Menus
{
    using Rogue.Drawing.Impl;
    using Rogue.Drawing.SceneObjects;
    using Rogue.Scenes.Manager;
    using Rogue.Scenes.Menus.Creation;
    using System;

    public class End : GameScene<PlayerNameScene, Game.Main>
    {
        public End(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => true;
        
        public override void Init()
        {
            var endText = new TextControl(new DrawText("ВАС ОТПИЗДИЛИ", ConsoleColor.Red));
            endText.Text.Size = 72;
            endText.Left = 8;
            endText.Top = 9;
            this.AddObject(endText);

            this.AddObject(new MetallButtonControl("Ок")
            {
                Left = 15.5f,
                Top = 17,
                OnClick = () =>
                {
                    Environment.Exit(0);
                }
            });
        }
    }
}