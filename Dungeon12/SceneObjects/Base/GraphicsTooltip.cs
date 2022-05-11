using Dungeon;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon.Types;
using Dungeon.View.Interfaces;
using Dungeon12.Entities;
using System.Linq;

namespace Dungeon12.SceneObjects.Base
{
    public class GraphicsTooltip : EmptySceneObject
    {
        public GraphicsTooltip(string title, string text, GraphicsTooltipSize size = GraphicsTooltipSize.Two, AbilityArea area = default, int cooldown = -1, params string[] leftparams)
        {
            this.Width = 355;

            var desc = text.AsDrawText().Gabriela().InColor(Global.CommonColorLight).InSize(FontSize).WithWordWrap();
            var descMeasure = this.MeasureText(desc, new EmptySceneObject() { Width = 320 });

            var ttle = title.AsDrawText().Gabriela().InColor(Global.CommonColorLight).InSize(16);
            var ttleMeasure = this.MeasureText(ttle, new EmptySceneObject() { Width = 320 });


            this.Height=
                descMeasure.Y +15
                + ttleMeasure.Y + (leftparams.Length==0 ? 15 : 0)
                + MeasureParams(leftparams);

            this.AddBorder();

            var header = this.AddTextCenter(ttle, vertical: false);
            header.Top = 5;

            var description = this.AddTextCenter(desc, vertical: false, parentWidth: 320);
            description.Left = 18;
            description.Top = 40;

            //descMeasure.Y += 90;


            //BindHeight(size, descMeasure, leftparams);
            //BindArea(area,cooldown);
            BindParams(leftparams, description.Top+descMeasure.Y+5);
        }

        private int FontSize = 12;

        private double MeasureParams(string[] leftparams )
        {
            return leftparams.Sum(x=> this.MeasureText(x.AsDrawText().SegoeUIBold().InColor(Global.CommonColorLight).InSize(FontSize)).Y +5);
        }

        private void BindParams(string[] leftparams, double paramtop)
        {
            //double paramtop = 120;
            foreach (var leftparam in leftparams)
            {
                var paramtext = this.AddTextCenter(leftparam.AsDrawText().SegoeUIBold().InColor(Global.CommonColorLight).InSize(FontSize));
                paramtext.Left = 18;
                paramtext.Top = paramtop;

                paramtop += 20;
            }
        }

        private void BindArea(AbilityArea area, int cooldown)
        {
            if (cooldown > -1)
            {
                var cooldowntext = this.AddTextCenter($"{Global.Strings.Cooldown}: {cooldown}".AsDrawText().SegoeUIBold().InColor(Global.CommonColorLight).InSize(FontSize));
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
                    var areatext = this.AddTextCenter($"{Global.Strings.Area}:".AsDrawText().SegoeUIBold().InColor(Global.CommonColorLight).InSize(FontSize));
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