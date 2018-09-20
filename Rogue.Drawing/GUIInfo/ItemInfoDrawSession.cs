using System;
using System.Collections.Generic;
using System.Text;
using Rogue.Drawing.Impl;
using Rogue.Items;
using Rogue.View.Interfaces;

namespace Rogue.Drawing.GUIInfo
{
    public class ItemInfoDrawSession : RightInfoDrawSession
    {
        public Item Item { get; set; }

        protected override void Draw()
        {
            string nname = "";
            try
            {
                if (Item.Name.Length > 20)
                {
                    nname = Item.Name.Substring(0, 20);
                }
                else
                {
                    nname = Item.Name;
                };
            }
            catch
            {
                nname = "Ингридиент для яда";
            }


            int pos = (25 / 2) - (nname.Length / 2);
            this.Write(2, pos, nname, Item.ForegroundColor);

            string type = "";

            List<string> bonuses = new List<string>();

            //string arm = DrawHelp.Sign(Item.ARM);
            //string mrs = DrawHelp.Sign(Item.MRS);
            //string hp = DrawHelp.Sign(Item.HP);
            //string mp = DrawHelp.Sign(Item.MP);
            //string mi = DrawHelp.Sign((Item.MIDMG);
            //string ma = DrawHelp.Sign(Item.MADMG);

            //if (null != null)
            //{
            //    arm = DrawHelp.Sign(Item.ARM - Rogue.RAM.Player.Equipment.Armor.ARM);
            //    mrs = DrawHelp.Sign(Item.MRS - Rogue.RAM.Player.Equipment.Armor.MRS);
            //    hp = DrawHelp.Sign(Item.HP - Rogue.RAM.Player.Equipment.Armor.HP);
            //    mp = DrawHelp.Sign(Item.MP - Rogue.RAM.Player.Equipment.Armor.MP);
            //    hp = DrawHelp.Sign(Item.HP - Rogue.RAM.Player.Equipment.Armor.HP);
            //    mp = DrawHelp.Sign(Item.MP - Rogue.RAM.Player.Equipment.Armor.MP);
            //}

            //bonuses.Add("Физ. Защита: " + arm);
            //bonuses.Add("Маг. Защита: " + mrs);
            //bonuses.Add("Здоровье: " + hp);
            //bonuses.Add("Ресурсы: " + mp);
            //bonuses.Add("Здоровье: " + hp);
            //bonuses.Add("Ресурсы: " + mp);
            //bonuses.Add("Мин. Урон: " + mi);
            //bonuses.Add("Макс. Урон: " + ma);


            pos = (25 / 2) - (type.Length / 2);
            this.Write(pos, 3, type, ConsoleColor.Yellow);

            this.WriteAvatar(Item.Icon, Item.ForegroundColor);

            //if (Item.ReputationSell != 0)
            //{
            //    var reputation = (Item.ReputationName + ": " + Item.ReputationSell.ToString());
            //    pos = (25 / 2) - (reputation.Length / 2);
            //    this.Write(pos, 12, reputation, ConsoleColor.DarkGray);
            //}
            //if (Item.Enchanted)
            //{
            //    var enchant = "Зачарованно";
            //    pos = (25 / 2) - (enchant.Length / 2);
            //    this.Write(pos, 11, enchant, ConsoleColor.Magenta);
            //}

            int forCount = 14;
            foreach (string s in bonuses)
            {
                ConsoleColor color = 0;
                if (s.IndexOf("+") > -1) { color = ConsoleColor.Green; }
                if (s.IndexOf("+0") > -1) { color = ConsoleColor.Gray; }
                if (s.IndexOf("-") > -1) { color = ConsoleColor.Red; }

                pos = (25 / 2) - (s.Length / 2);
                this.Write(forCount++, pos, s, color);
            }
        }
    }
}