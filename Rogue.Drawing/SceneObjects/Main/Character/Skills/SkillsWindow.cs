namespace Rogue.Drawing.SceneObjects.Main.Character
{
    using Rogue.Control.Keys;
    using Rogue.Drawing.SceneObjects.UI;
    using Rogue.View.Interfaces;
    using System.Collections.Generic;

    public class SkillsWindow : DraggableControl
    {
        public override bool CacheAvailable => false;

        public override bool AbsolutePosition => true;

        public SkillsWindow()
        {
            this.Image = "Rogue.Resources.Images.ui.vertical_title(17x12).png";

            this.Height = 17;
            this.Width = 12;

            this.Left = 22.5;
            this.Top = 2;
        }

        public static void OpenCloseSkillsWindow()
        {

        }

        protected override Key[] OverrideKeyHandles => new Key[] { Key.X };

        public override void KeyDown(Key key, KeyModifiers modifier, bool hold)
        {
            if (key == Key.X)
            {
                base.KeyDown(Key.Escape, modifier, hold);
            }

            base.KeyDown(key, modifier, hold);
        }
    }
}
