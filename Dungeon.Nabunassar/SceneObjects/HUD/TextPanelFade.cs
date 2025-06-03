using Dungeon.Drawing;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon.SceneObjects.Base;
using Dungeon.View.Enums;

namespace Nabunassar.SceneObjects.HUD
{
    internal class TextPanelFade : EmptySceneObject
    {
        public TextPanelFade(string text, int textSize = 20, double opacity = 0)
        {
            var drawtext = text.Calibri().InColor(Global.CommonColorLight).InSize(textSize);
            var m = this.MeasureText(drawtext, this);

            var deviderWidth = 96;
            var deviderHeight = 22;

            var deviderMargin = 20;

            var minimalHeight = 50;

            double bordersHeight = 4;

            var textWidth = m.X;
            var textHeight = m.Y;
            var panelheight = textHeight + minimalHeight;

            this.Height = m.Y + minimalHeight + (bordersHeight * 2);
            this.Width = m.X + deviderWidth*2;

            var w = this.Width;
            var h = this.Height;

            var imgpath = "UI/panelmin/";

            var up = this.AddChild(new ImageObject(imgpath+ "paneltop.png")
            {
                Width = textWidth,
                Left = deviderWidth,
                Height = bordersHeight,
                Mode = DrawMode.Tiled,
                Color = Global.CommonColor

            });

            var bot = this.AddChild(new ImageObject(imgpath + "panelbot.png")
            {
                Width = textWidth,
                Left = deviderWidth,
                Top = h-bordersHeight,
                Height = bordersHeight,
                Mode = DrawMode.Tiled,
                Color = Global.CommonColor
            });

            var leftFadeUp = this.AddChild(new ImageObject(imgpath + "paneltopfade.png")
            {
                Width = deviderWidth,
                Height = bordersHeight,
                Color = Global.CommonColor
            });

            var leftFade = this.AddChild(new ImageObject(imgpath + "panelfade.png")
            {
                Width = Width/2,
                Height = h - bordersHeight /*- bordersHeight*2*/,
                Top = bordersHeight-2 /*+ bordersHeight*/,
                Color = new DrawColor(0,0,0,DrawColor.CalculateOpacity(opacity)),
                Mode = DrawMode.Tiled,
            });

            var rightFade = this.AddChild(new ImageObject(imgpath + "panelfade.png")
            {
                Width = Width / 2,
                Left = Width/2-2,
                Height = h - bordersHeight /*- bordersHeight*2*/,
                Top = bordersHeight - 2 /*+ bordersHeight*/,
                Color = new DrawColor(0, 0, 0, DrawColor.CalculateOpacity(opacity)),
                Mode = DrawMode.Tiled,
                Flip = FlipStrategy.Horizontally
            });

            var rightFadeUp = this.AddChild(new ImageObject(imgpath + "paneltopfade.png")
            {
                Width = deviderWidth,
                Left = textWidth + deviderWidth,
                Height = bordersHeight,
                Color = Global.CommonColor,
                Flip = FlipStrategy.Horizontally
            });

            var leftFadeBot = this.AddChild(new ImageObject(imgpath + "panelbotfade.png")
            {
                Width = deviderWidth,
                Top = panelheight+bordersHeight,
                Height = bordersHeight,
                Color = Global.CommonColor
            });

            var rightFadeBot = this.AddChild(new ImageObject(imgpath + "panelbotfade.png")
            {
                Width = deviderWidth,
                Top = panelheight + bordersHeight,
                Left = textWidth + deviderWidth,
                Height = bordersHeight,
                Color = Global.CommonColor,
                Flip = FlipStrategy.Horizontally
            });

            var textbox = this.AddTextCenter(drawtext);

            var deviderLeft = this.AddChild(new ImageObject(imgpath + "deviderfade.png")
            {
                Width = deviderWidth,
                Top = this.CalculateVerticalCenterPoint(deviderHeight),
                Left = textbox.Left - deviderMargin - deviderWidth,
                Height = deviderHeight,
                Color = Global.CommonColor,
            });

            var deviderRight = this.AddChild(new ImageObject(imgpath + "deviderfade.png")
            {
                Width = deviderWidth,
                Top = this.CalculateVerticalCenterPoint(deviderHeight),
                Left = textbox.Left + textbox.Width + deviderMargin,
                Height = deviderHeight,
                Color = Global.CommonColor,
                Flip = FlipStrategy.Horizontally
            });

        }
    }
}