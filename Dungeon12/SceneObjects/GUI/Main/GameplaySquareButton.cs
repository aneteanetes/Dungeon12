using Dungeon;
using Dungeon.Control;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using System;

namespace Dungeon12.SceneObjects.GUI.Main
{
    public class GameplaySquareButton : EmptySceneControl
    {
        public GameplaySquareButton(string img)
        {
            this.Width = 75;
            this.Height = 75;

            this.Image = "GUI/Buttons/setup_btn_on.png".AsmImg();

            //this.AddChildCenter(new ImageControl(img)
            //{
            //    Width = 53,
            //    Height = 51
            //});
        }

        public Action OnClick { get; set; }

        public override void Click(PointerArgs args)
        {
            OnClick?.Invoke();
            base.Click(args);
        }

        public override void Focus()
        {
            this.Image = "GUI/Buttons/setup_btn_off.png".AsmImg();
            base.Focus();
        }

        public override void Unfocus()
        {
            this.Image = "GUI/Buttons/setup_btn_on.png".AsmImg();
            base.Unfocus();
        }
    }
}