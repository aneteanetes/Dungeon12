using Rogue.Control.Keys;
using Rogue.Drawing.Impl;
using Rogue.Drawing.SceneObjects.Main.CharacterInfo.Journal;
using Rogue.Drawing.SceneObjects.Main.CharacterInfo.Talants;
using Rogue.Drawing.SceneObjects.Map;
using Rogue.Drawing.SceneObjects.UI;
using Rogue.Events;
using System.Collections.Generic;

namespace Rogue.Drawing.SceneObjects.Main.CharacterInfo
{
    public class JournalWindow : DraggableControl<JournalWindow>
    {
        public override bool CacheAvailable => false;

        public override bool AbsolutePosition => true;

        private readonly PlayerSceneObject playerSceneObject;

        public JournalWindow(PlayerSceneObject playerSceneObject)
        {
            playerSceneObject.BlockMouse = true;
            this.Destroy += () => playerSceneObject.BlockMouse = false;
            this.playerSceneObject = playerSceneObject;

            //this.Image = "Rogue.Resources.Images.ui.vertical(17x24).png";

            this.Height = 17;
            this.Width = 12; //can resize to 24 with content

            this.Left = 3.5;
            this.Top = 2;

            this.AddChild(new JournalList(playerSceneObject));
        }

        protected override Key[] OverrideKeyHandles => new Key[] { Key.L };

        public override void KeyDown(Key key, KeyModifiers modifier, bool hold)
        {
            if (key == Key.L)
            {
                base.KeyDown(Key.Escape, modifier, hold);
            }

            base.KeyDown(key, modifier, hold);
        }

        public void OnEvent(ClassChangeEvent @event)
        {

        }

        protected override void CallOnEvent(dynamic obj)
        {
            OnEvent(obj);
        }
    }
}
