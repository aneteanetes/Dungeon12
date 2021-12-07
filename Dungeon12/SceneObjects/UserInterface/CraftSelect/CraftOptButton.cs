using Dungeon;
using Dungeon.Control;
using Dungeon.SceneObjects;
using System;

namespace Dungeon12.SceneObjects.UserInterface.CraftSelect
{
    public class CraftOptButton : EmptySceneControl
    {
        public CraftOptButton(bool close = false)
        {
            if (close)
            {
                this.Width = 66;
                this.Height = 85;
                this.Image = "Other/close.png".AsmImg();
            }
            else
            {
                this.Width = 74;
                this.Height = 70;
                this.Image = "Other/mark.png".AsmImg();
            }
        }

        public Action OnClick { get; set; }

        public override void Click(PointerArgs args)
        {
            OnClick?.Invoke();
        }

        public override void Focus()
        {
            Image = Image.Replace(".png", "_f.png");
        }

        public override void Unfocus()
        {
            Image = Image.Replace("_f.png", ".png");
        }
    }
}