namespace Rogue.Drawing.Data
{
    using System;
    using System.Linq;
    using Rogue.Drawing.Impl;
    using Rogue.Entites.Items;
    using Rogue.View.Interfaces;

    public class InventoryDrawSession : DrawSession
    {
        public InventoryDrawSession()
        {
            AutoClear = false;
        }

        public Inventory Inventory { get; set; }

        public override IDrawSession Run()
        {
            char[] oldchar = new char[] { '\0', '\0', '\0', '\0', '\0', '\0' };
            IDrawColor[] oldcol = new IDrawColor[] { new DrawColor(0), new DrawColor(0), new DrawColor(0), new DrawColor(0), new DrawColor(0), new DrawColor(0) };

            for (int i = 0; i < 6; i++)
            {
                try
                {
                    oldchar[i] = Inventory.Items[i].Icon.First();
                    oldcol[i] = Inventory.Items[i].ForegroundColor;
                }
                catch { }
            }


            this.Write(19, 81, new DrawText(oldchar[0].ToString(), oldcol[0]));
            this.Write(19, 85, new DrawText(oldchar[1].ToString(), oldcol[1]));
            this.Write(19, 89, new DrawText(oldchar[2].ToString(), oldcol[2]));

            this.Write(21, 81, new DrawText(oldchar[3].ToString(), oldcol[3]));
            this.Write(21, 85, new DrawText(oldchar[4].ToString(), oldcol[4]));
            this.Write(21, 89, new DrawText(oldchar[5].ToString(), oldcol[5]));

            return this;
        }
    }
}
