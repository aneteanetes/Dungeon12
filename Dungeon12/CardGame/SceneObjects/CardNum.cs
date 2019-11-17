using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using System;
using Dungeon;
using Dungeon12.Drawing.SceneObjects.Main.CharacterBar;
using System.Linq;

namespace Dungeon12.CardGame.SceneObjects
{
    public class CardNum : SlideComponent
    {
        private Func<int> _source;

        private double numberWidth;
        private double numberHeight;
        private double charSpacing;

        public int Value => _source?.Invoke() ?? 0;

        public CardNum(Func<int> source, double width=4, double height=4, double charSpacing=1)
        {
            _source = source;
            numberHeight = height;
            numberWidth = width;
            this.charSpacing = charSpacing;
            this.Height = numberHeight;

            double left = 0;
            for (int i = 0; i < 3; i++)
            {
                var img = $"Cards/Numbers/0.png".AsmImgRes();

                this.AddChild(new ImageControl(img)
                {
                    AbsolutePosition = this.AbsolutePosition,
                    CacheAvailable = this.CacheAvailable,
                    Left = left,
                    Width = numberWidth,
                    Height = numberHeight,
                    Visible = false
                });
                left += charSpacing;
                this.Width += left;
            }
        }

        public override void Update()
        {
            var value = _source?.Invoke().ToString();

            for (int i = 0; i < 3; i++)
            {
                var @char =  value.ElementAtOrDefault(i);
                var num = Children.ElementAtOrDefault(i) as ImageControl;
                if (@char != default)
                {
                    num.Image= $"Cards/Numbers/{@char}.png".AsmImgRes();
                    num.Visible = true;
                }
                else
                {
                    num.Visible = false;
                }
            }
        }
    }
}