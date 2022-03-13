using Dungeon;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon.Types;
using Dungeon.View.Interfaces;
using Dungeon12.Entities;

namespace Dungeon12.SceneObjects.Base
{
    public class GraphicsTooltip : EmptySceneObject
    {
        public GraphicsTooltip(string title, string text, GraphicsTooltipSize size = GraphicsTooltipSize.Two, AbilityArea area = default,int cooldown=-1, params string[] leftparams)
        {
            this.Width = 355;

            var header = this.AddTextCenter(title.AsDrawText().Gabriela().InColor(Global.CommonColor).InSize(16), vertical: false);
            header.Top = 5;

            var desc = text.AsDrawText().Gabriela().InColor(Global.DarkColor).InSize(FontSize).WithWordWrap();

            var descMeasure = this.MeasureText(desc, new EmptySceneObject() { Width = 320 });

            var description = this.AddTextCenter(desc, vertical: false, parentWidth: 320);
            description.Left = 18;
            description.Top = 45;

            descMeasure.Y += 90;
            BindHeight(size, descMeasure, leftparams);
            BindArea(area,cooldown);
            BindParams(leftparams);
        }

        private int FontSize = 12;

        private void BindParams(string[] leftparams)
        {
            double paramtop = 120;
            foreach (var leftparam in leftparams)
            {
                var paramtext = this.AddTextCenter(leftparam.AsDrawText().SegoeUIBold().InColor(Global.DarkColor).InSize(FontSize));
                paramtext.Left = 18;
                paramtext.Top = paramtop;

                paramtop += 20;
            }
        }

        private void BindHeight(GraphicsTooltipSize size, Point descMeasure,string[] leftparams)
        {
            double height = 0;

            if (size == GraphicsTooltipSize.AutoByParams)
            {
                if (leftparams.Length <= 2)
                {
                    size = GraphicsTooltipSize.Two;
                }
                else if (leftparams.Length > 2 && leftparams.Length<5)
                {
                    size = GraphicsTooltipSize.Four;
                }
                else if (leftparams.Length > 4 && leftparams.Length < 7)
                {
                    size = GraphicsTooltipSize.Six;
                }
                else if (leftparams.Length > 6)
                {
                    size = GraphicsTooltipSize.Eight;
                }
            }

            if (size == GraphicsTooltipSize.Auto)
            {
                if (descMeasure.Y < 226)
                {
                    size = GraphicsTooltipSize.Two;
                }
                else if (descMeasure.Y < 271)
                {
                    size = GraphicsTooltipSize.Four;
                }
                else if (descMeasure.Y < 317)
                {
                    size = GraphicsTooltipSize.Six;
                }
                else
                {
                    size = GraphicsTooltipSize.Eight;
                }
            }

            switch (size)
            {
                case GraphicsTooltipSize.Two:
                    height = 180;
                    break;
                case GraphicsTooltipSize.Four:
                    height = 226;
                    break;
                case GraphicsTooltipSize.Six:
                    height = 271;
                    break;
                case GraphicsTooltipSize.Eight:
                    height = 317;
                    break;
                default:
                    break;
            }

            this.Height = height;

            this.Image = $"UI/Tooltips/note{((int)size)}.png".AsmImg();
        }

        private void BindArea(AbilityArea area, int cooldown)
        {
            if (cooldown > -1)
            {
                var cooldowntext = this.AddTextCenter($"{Global.Strings.Cooldown}: {cooldown}".AsDrawText().SegoeUIBold().InColor(Global.DarkColor).InSize(FontSize));
                cooldowntext.Left = 175;
                cooldowntext.Top = 142;
            }

            if (area != default)
            {
                var posimg = "UI/Tooltips/radiusicon.png";

                bool haveRadius = false;

                if (area.Left)
                {
                    haveRadius = true;
                    this.AddChild(new ImageObject(posimg)
                    {
                        Left = 273,
                        Top = 134
                    });
                }

                if (area.Center)
                {
                    haveRadius = true;
                    this.AddChild(new ImageObject(posimg)
                    {
                        Left = 286,
                        Top = 134
                    });
                }

                if (area.Right)
                {
                    haveRadius = true;
                    this.AddChild(new ImageObject(posimg)
                    {
                        Left = 299,
                        Top = 134
                    });
                }

                if (area.LeftBack)
                {
                    haveRadius = true;
                    this.AddChild(new ImageObject(posimg)
                    {
                        Left = 279,
                        Top = 122
                    });
                }

                if (area.RightBack)
                {
                    haveRadius = true;
                    this.AddChild(new ImageObject(posimg)
                    {
                        Left = 292,
                        Top = 122
                    });
                }

                if (haveRadius)
                {
                    var areatext = this.AddTextCenter($"{Global.Strings.Area}:".AsDrawText().SegoeUIBold().InColor(Global.DarkColor).InSize(FontSize));
                    areatext.Left = 175;
                    areatext.Top = 120;
                }
            }
        }

        public ISceneObject Host { get; set; }
    }

    public enum GraphicsTooltipSize
    {
        Auto = 0,
        AutoByParams = 1,
        Two = 2,
        Four = 4,
        Six = 6,
        Eight = 8
    }
}