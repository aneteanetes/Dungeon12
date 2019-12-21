namespace Dungeon12.Scenes.Menus
{
    using Dungeon.Drawing;
    using Dungeon12.SceneObjects; using Dungeon.SceneObjects;
    using Dungeon.Scenes;
    using Dungeon.Scenes.Manager;
    using Dungeon12.Drawing.SceneObjects;
    using Dungeon12.Scenes.Menus.Creation;
    using System;
    using Dungeon12.Scenes.Game;
    using Dungeon;

    public class End : GameScene<Start, Game.Main>
    {
        public End(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => true;
        
        public override void Init()
        {
            var endText = new TextControl("Вы проиграли".AsDrawText().InSize(70).Triforce());
            endText.Left = 12;
            endText.Top = 9;
            this.AddObject(endText);

            this.AddObject(new MetallButtonControl("Ок")
            {
                Left = 15.5f,
                Top = 17,
                OnClick = () =>
                {
                    SceneManager.Destroy<Main>();
                    this.Switch<Start>();
                }
            });
        }
    }
}