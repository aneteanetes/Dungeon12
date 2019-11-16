using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using System;
using Dungeon;
using Dungeon12.Drawing.SceneObjects.Main.CharacterBar;

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
        }

        public override void Update()
        {
            this.Children.Clear();

            var value = _source?.Invoke().ToString();
            double left = 0;
            foreach (var @char in value)
            {
                var img = $"Cards/Numbers/{@char}.png".AsmImgRes();

                this.AddChild(new ImageControl(img)
                {
                    AbsolutePosition = this.AbsolutePosition,
                    CacheAvailable = this.CacheAvailable,
                    Left = left,
                    Width=numberWidth,
                    Height=numberHeight
                });
                left += charSpacing;
                this.Width += left;
            }
        }
    }
}