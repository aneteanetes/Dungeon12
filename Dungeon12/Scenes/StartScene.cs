using Dungeon;
using Dungeon.Control.Gamepad;
using Dungeon.Control.Keys;
using Dungeon.Drawing.SceneObjects;
using Dungeon.Scenes;
using Dungeon.Scenes.Manager;
using Dungeon.Tiled;
using Dungeon.Types;
using Dungeon.View.Interfaces;
using Dungeon12.Drawing.SceneObjects;
using Dungeon12.SceneObjects.UserInterface.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon12.Scenes
{
    public class StartScene : Dungeon.Scenes.StartScene<MainScene,MapScene>
    {
        public StartScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => true;

        IEffect fogofwar;

        public override void Initialize()
        {
            var layerBack = this.CreateLayer("back");
            layerBack.AddObject(new ImageObject("d12backl.png".AsmImg())
            {
                Width = Global.Resolution.Width,
                Height = Global.Resolution.Height
            });

            layerBack.AddObjectCenter(new ImageObject("d12textM.png".AsmImg()), vertical: false);

            //fogofwar = this.sceneManager.DrawClient.GetEffect("FogOfWar");
            //fogofwar.Image = "Effects/fow.png".AsmImg();
            //fogofwar.Size = new Point(1600, 900);

            //layerBack.AddGlobalEffect(fogofwar);

            var ui = this.CreateLayer("ui");
            InitButtons(ui);

            var snow = this.CreateLayer("snow");
            snow.AddObject(new BackgroundSnow());
        }

        private void InitButtons(SceneLayer ui)
        {
            var data = new (string text, Action click, bool disabled)[]
           {
                (Global.Strings.NewGame,NewGame,false),
                (Global.Strings.Save,SaveGame,!InGame),
                (Global.Strings.Load,LoadGame,true),
                (Global.Strings.Settings,Settings,false),
                (Global.Strings.FastGame,FastGame,false),
                (Global.Strings.ExitGame,Exit,false)
           };

            var y = 200;
            var x = 60;

            foreach (var (text, click, disabled) in data)
            {
                var btn = new MetallButton(text)
                {
                    Left = x,
                    Top = y,
                    Disabled = disabled,
                    OnClick = click
                };
                btns.Add(btn);
                ui.AddObject(btn);
                y += 80;
            }

            if (DungeonGlobal.GamePadConnected)
            {
                btns.FirstOrDefault().Focus();
                currentFocus = btns.FirstOrDefault();
            }
        }

        protected override void StickMoveOnce(Direction direction, GamePadStick stick)
        {
            if (stick == GamePadStick.RightStick)
                return;

            var currentIdx = button_idx;
            bool up = false;

            if (direction == Direction.Up || direction == Direction.UpLeft || direction == Direction.UpRight)
            {
                up = true;
                button_idx -= 1;
            }


            if (direction == Direction.Down || direction == Direction.DownLeft || direction == Direction.DownRight)
            {
                button_idx += 1;
            }

            if (button_idx < 0)
                button_idx = 0;

            if (button_idx > btns.Count - 1)
                button_idx = btns.Count - 1;

            if (currentIdx == button_idx)
                return;

            currentFocus.Unfocus();
            currentFocus = FindIdx(up);
            currentFocus.Focus();

            base.StickMoveOnce(direction, stick);
        }

        private MetallButton FindIdx(bool up)
        {
            currentFocus = btns.ElementAtOrDefault(button_idx);
            if (currentFocus.Disabled)
            {
                if (up)
                    button_idx--;
                else
                    button_idx++;

                if (button_idx > btns.Count - 1)
                    button_idx = btns.Count - 1;

                return FindIdx(up);
            }
            return currentFocus;
        }

        protected override void GamePadButtonPress(GamePadButton[] btns)
        {
            if (btns.Contains(GamePadButton.A))
            {
                currentFocus.Click(default);
            }

            base.GamePadButtonPress(btns);
        }

        private int button_idx = 0;
        List<MetallButton> btns = new List<MetallButton>();
        MetallButton currentFocus;

        private void NewGame()
        {
            this.Switch<MapScene>();
        }

        private void SaveGame()
        {

        }

        private void LoadGame()
        {

        }

        private void Settings()
        {

        }

        private void FastGame()
        {
            //this.Switch<MainScene>();
        }

        private void Exit()
        {
            DungeonGlobal.Exit?.Invoke();
        }
    }
}