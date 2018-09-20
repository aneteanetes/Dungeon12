using System;
using Rogue.View.Interfaces;

namespace Rogue.Drawing.GUIInfo
{
    public class TradeInfoDrawSession : RightInfoDrawSession
    {
        public IDrawable Merchant { get; set; }

        protected override void Draw()
        {
            //name
            int pos = (23 / 2) - (Merchant.Name.Length / 2);
            this.Write(1, pos + 1, DrawHelp.FullLine(" ".Length + Merchant.Name.Length, " " + Merchant.Name, " ".Length + Merchant.Name.Length - 1), ConsoleColor.Yellow);

            ////ava
            //this.WriteAvatar(Merchant.SpeachIcon, Merchant.SpeachColor);

            ////gold
            //pos = (23 / 2) - ((9 + Merchant.CurGold.ToString().Length) / 2);
            //string WriteThis = "Золотых: " + Merchant.CurGold.ToString();
            //this.Write(12, pos + 1, DrawHelp.FullLine(WriteThis.Length, WriteThis, WriteThis.Length - 1), ConsoleColor.Yellow);

            ////ap
            //pos = (23 / 2) - ((10 + Merchant.MaxGold.ToString().Length) / 2);
            //WriteThis = "Максимум: " + Merchant.MaxGold.ToString();
            //this.Write(12, pos + 1, DrawHelp.FullLine(WriteThis.Length, WriteThis, WriteThis.Length - 1), ConsoleColor.DarkYellow);
        }
    }
}
