namespace Rogue.Drawing.SceneObjects.Main.CharacterInfo
{
    using Rogue.Drawing.SceneObjects.Base;
    using Rogue.Drawing.SceneObjects.UI;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class Inventory : DropableControl
    {
        public override bool CacheAvailable => false;

        public override bool AbsolutePosition => true;
        
        public Inventory(int zIndex)
        {
            var xPoint = 1;
            var yPoint = 1;

            for (int y = 0; y < 6; y++)
            {
                for (int x = 0; x < 11; x++)
                {
                    this.AddChild(new InventoryCell()
                    {
                        Left = x * xPoint,
                        Top = y * yPoint,
                        ZIndex= zIndex
                    });
                }
            }
        }

        private class InventoryCell : DarkRectangle
        {
            public override bool CacheAvailable => false;

            public override bool AbsolutePosition => true;

            public InventoryCell()
            {
                this.AddChild(new DarkRectangle() { Opacity = 1, Fill = false, Color= ConsoleColor.Black, Height=1, Width=1 });

                Opacity = 0.7;

                this.Height =1;
                this.Width = 1;
            }

            public override void Focus()
            {
                this.Opacity = 0.2;
                base.Focus();
            }

            public override void Unfocus()
            {
                this.Opacity = 0.7;
                base.Unfocus();
            }
        }
    }
}
