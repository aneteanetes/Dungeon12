namespace Rogue.Drawing.SceneObjects.Inventories
{
    using Rogue.Drawing.GUI;
    using Rogue.Drawing.Impl;
    using Rogue.Items;
    using Rogue.Items.Enums;
    using Rogue.Types;
    using System.Collections.Generic;

    public class ItemTooltip : Tooltip
    {
        public override bool CacheAvailable => false;

        public override bool Interface => true;

        private Item item;
        private double textPosTop = 0;

        public ItemTooltip(Item item, Point position) : base("", position, new DrawColor(System.ConsoleColor.Black))
        {
            this.item = item;
            Opacity = 0.8;

            this.Children.ForEach(c => c.Destroy?.Invoke());

            base.Left = position.X - this.Width / 2.4;
            this.Top = position.Y;

            Stats.ForEach(AddLine);
        }

        private IEnumerable<DrawText> Stats
        {
            get
            {
                List<DrawText> stats = new List<DrawText>();

                stats.Add(Title);
                stats.Add(SubType);
                stats.AddRange(BaseStats);
                stats.AddRange(Additional);
                stats.AddRange(ClassStats);
                stats.AddRange(ItemSet);

                return stats;
            }
        }

        private DrawText Title => new DrawText(item.Name, item.Rare.Color()) { Size = 24 };

        private DrawText SubType => item.SubType == null
            ? new DrawText(item.Kind.ToDisplay(), new DrawColor(130, 99, 7))
            : new DrawText(item.SubType, new DrawColor(130, 99, 7));

        private DrawText[] BaseStats
        {
            get
            {

                return default;
            }
        }

        private DrawText[] Additional
        {
            get
            {

                return default;
            }
        }

        private DrawText[] ClassStats
        {
            get
            {

                return default;
            }
        }

        private DrawText[] ItemSet
        {
            get
            {

                return default;
            }
        }

        private void AddLine(DrawText drawText)
        {
            if (drawText == null)
                return;

            var txt = this.AddTextCenter(drawText, vertical: false);
            textPosTop += MeasureText(txt.Text).Y / 32;
        }
    }
}