using Dungeon;
using Dungeon.Control;
using Dungeon.Drawing;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using System;

namespace SidusXII.SceneObjects.ItemSelector
{
    public class ImgBtn : EmptySceneControl
    {
        ImageObject ArrowImage;

        public ImgBtn(string img)
        {
            ArrowImage = AddChild(new ImageObject(img.AsmImg()));
            Width = 50;
            Height = 50;
        }

        public Action OnClick { get; set; }

        public override double Angle
        {
            get => ArrowImage.Angle;
            set => ArrowImage.Angle = value;
        }

        public override void Focus()
        {
            ArrowImage.Color = DrawColor.Yellow;
            base.Focus();
        }

        public override void Unfocus()
        {
            ArrowImage.Color = DrawColor.White;
            base.Unfocus();
        }

        public override void Click(PointerArgs args)
        {
            OnClick?.Invoke();
        }
    }
}