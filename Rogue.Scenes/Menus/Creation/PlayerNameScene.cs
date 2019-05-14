using System;
using Rogue.Classes.Noone;
using Rogue.Control.Keys;
using Rogue.Drawing;
using Rogue.Drawing.Controls;
using Rogue.Drawing.Impl;
using Rogue.Drawing.SceneObjects;
using Rogue.Drawing.SceneObjects.Base;
using Rogue.Drawing.SceneObjects.Dialogs;
using Rogue.Entites.Alive.Character;
using Rogue.Scenes.Scenes;
using Rogue.Types;

namespace Rogue.Scenes.Menus.Creation
{
    public class PlayerNameScene : GameScene<PlayerRaceScene,Start>
    {
        public PlayerNameScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => true;
        
        public override void Init()
        {
            this.AddObject(new ImageControl("Rogue.Resources.Images.d12back.png"));

            this.AddObject(new TypeNameDialogue(Next,Back)
            {
                Top = 3f,
                Left = 14f,
            });
        }

        private void Next(string value)
        {
            if (this.PlayerAvatar == null)
            {
                this.PlayerAvatar = new Map.Objects.Avatar
                {
                    Character = new Rogue.Classes.Noone.Noone()
                };
            }

            this.PlayerAvatar.Character.Name = value[0].ToString().ToUpper() + value.Substring(1);

            this.Switch<PlayerRaceScene>();
        }

        private void Back()
        {
            this.Switch<Start>();
        }

        protected override void KeyPress(Key keyPressed, KeyModifiers keyModifiers, bool hold)
        {
            if (keyPressed == Key.Escape && !hold)
            {
                this.Switch<Start>();
            }
        }
    }
}
