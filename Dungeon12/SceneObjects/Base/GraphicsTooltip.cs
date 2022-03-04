using Dungeon;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon.Types;
using Dungeon12.Entities;

namespace Dungeon12.SceneObjects.Base
{
    public class GraphicsTooltip : EmptySceneObject
    {
        public GraphicsTooltip(string title, string text, GraphicsTooltipSize size = GraphicsTooltipSize.Two, AbilityArea area = default, params string[] leftparams)
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
            BindHeight(size, descMeasure);
            BindArea(area);
            BindParams(leftparams);
        }

        private int FontSize = 12;

        private void BindParams(string[] leftparams)
        {
            double paramtop = 120;
            foreach (var leftparam in leftparams)
            {
                var paramtext = this.AddTextCenter(leftparam.AsDrawText().Gabriela().InColor(Global.DarkColor).InSize(FontSize));
                paramtext.Left = 18;
                paramtext.Top = paramtop;

                paramtop += 20;
            }
        }

        private void BindHeight(GraphicsTooltipSize size, Point descMeasure)
        {
            double height = 0;

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

        private void BindArea(AbilityArea area)
        {
            if (area != default)
            {
                var areatext = this.AddTextCenter(Global.Strings.Area.AsDrawText().Gabriela().InColor(Global.DarkColor).InSize(FontSize));
                areatext.Left = 197;
                areatext.Top = 128;

                var posimg = "UI/Tooltips/radiusicon.png";

                if (area.Left)
                {
                    this.AddChild(new ImageObject(posimg)
                    {
                        Left = 273,
                        Top = 134
                    });
                }

                if (area.Center)
                {
                    this.AddChild(new ImageObject(posimg)
                    {
                        Left = 286,
                        Top = 134
                    });
                }

                if (area.Right)
                {
                    this.AddChild(new ImageObject(posimg)
                    {
                        Left = 299,
                        Top = 134
                    });
                }

                if (area.LeftBack)
                {
                    this.AddChild(new ImageObject(posimg)
                    {
                        Left = 279,
                        Top = 122
                    });
                }

                if (area.RightBack)
                {
                    this.AddChild(new ImageObject(posimg)
                    {
                        Left = 292,
                        Top = 122
                    });
                }
            }
        }
    }

    public enum GraphicsTooltipSize
    {
        Auto=0,
        Two = 2,
        Four = 4,
        Six = 6,
        Eight = 8
    }
}