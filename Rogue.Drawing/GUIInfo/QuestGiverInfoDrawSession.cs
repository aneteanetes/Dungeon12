using System;
using Rogue.View.Interfaces;

namespace Rogue.Drawing.GUIInfo
{
    public class QuestGiverInfoDrawSession : RightInfoDrawSession
    {
        public IDrawable QuestGiver { get; set; }

        protected override void Draw()
        {
            this.WriteStatFull(QuestGiver.Name, 2, QuestGiver.ForegroundColor);

            this.WriteAvatar(QuestGiver.Icon, ConsoleColor.Yellow);

            var pos = (23 / 2) - (" Задание:".Length / 2);
            this.Write(12, pos + 1, " Задание:", ConsoleColor.DarkYellow);

            //text of quest
            int forCount = 14;
            foreach (string s in DrawHelp.TextBlock(QuestGiver.Quest.Speach, 21))
            {
                this.Write(forCount, 3, s, ConsoleColor.DarkGreen);
            }


            pos = (23 / 2) - (" Награда:".Length / 2);
            this.Write(forCount++, pos + 1, " Награда:", ConsoleColor.DarkYellow);

            string stringBuffer = string.Empty;
            foreach (var itm in QuestGiver.Quest.Rewards.Items)
            {
                int exemplar = 0;

                foreach (var search in QuestGiver.Quest.Rewards.Items)
                {
                    if (itm.Name == search.Name)
                    {
                        exemplar++;
                    }
                }

                string nameofreward = string.Empty;
                if (itm.Name.Length > 14)
                {
                    nameofreward = itm.Name.Substring(0, 14);
                }
                else
                {
                    nameofreward = itm.Name;
                }

                pos = (23 / 2) - ((nameofreward + " (" + exemplar + ")").Length / 2);
                this.Write(forCount++, pos + 1, nameofreward + " (" + exemplar + ")", itm.Color);
            }

            foreach (var itm in QuestGiver.Quest.Rewards.Perks)
            {
                string nameofreward = string.Empty;
                if (itm.Name.Length > 14)
                {
                    nameofreward = itm.Name.Substring(0, 14);
                }
                else
                {
                    nameofreward = itm.Name;
                }

                stringBuffer = "Перк: " + nameofreward;
                pos = (23 / 2) - (stringBuffer.Length / 2);
                this.Write(forCount++, pos + 1, stringBuffer, itm.Color);
            }

            foreach (var itm in QuestGiver.Quest.Rewards.Abilityes)
            {
                string nameofreward = string.Empty;
                if (itm.Name.Length > 14)
                {
                    nameofreward = itm.Name.Substring(0, 14);
                }
                else
                {
                    nameofreward = itm.Name;
                }

                stringBuffer = "Нав: <" + nameofreward + ">";
                pos = (23 / 2) - (stringBuffer.Length / 2);
                this.Write(forCount++, pos + 1, stringBuffer, itm.Color);
            }

            foreach (int itm in QuestGiver.Quest.Rewards.Exp)
            {
                stringBuffer = "EXP: " + itm.ToString();
                pos = (23 / 2) - (stringBuffer.Length / 2);
                this.Write(forCount++, pos + 1, stringBuffer, ConsoleColor.DarkCyan);
            }

            foreach (int itm in QuestGiver.Quest.Rewards.Gold)
            {
                stringBuffer = "$: " + itm.ToString();
                pos = (23 / 2) - (stringBuffer.Length / 2);
                this.Write(forCount++, pos + 1, stringBuffer, ConsoleColor.Yellow);
            }
        }
    }
}
