namespace Dungeon12.Scenes.Menus.Creation
{
    using Dungeon.Control.Keys;
    using Dungeon.Drawing.Impl;
    using Dungeon.Drawing.SceneObjects;
    using Dungeon.Scenes;
    using Dungeon.Scenes.Manager;
    using Dungeon12.Scenes.Game;
    using System;

    public class PlayerSummaryScene : GameScene<Main,PlayerOriginScene>
    {
        public PlayerSummaryScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => true;

        public override void Init()
        {
            this.AddObject(new Prologue());
        }

        private class Prologue : ColoredRectangle
        {
            public Prologue()
            {
                this.Width = 40;
                this.Height = 22.5;
               var txt = new DrawText("Пролог", ConsoleColor.White);
                txt.Size = 72;

                this.AddTextCenter(txt);
            }
        }

        protected override void KeyPress(Key keyPressed, KeyModifiers keyModifiers, bool hold)
        {
            if (!hold)
            {
                this.Switch<Main>();
            }
        }
    }
}
