using System;
using System.Collections.Generic;
using System.Text;
using Rogue.Abilities;
using Rogue.Drawing.Impl;
using Rogue.View.Interfaces;

namespace Rogue.Drawing.Character
{
    //DrawMainInfoCharWindow special ability border
    public class CharAbilityInfoDrawSession : CharPerksInfoDrawSession
    {
        public bool ReDraw { get; set; }

        public IEnumerable<Ability> Abilities { get; set; }

        public int NowTab { get; set; }

        public override IDrawSession Run()
        {
            this.WriteHeader("Способности персонажа");

            int count = 0;
            foreach (Ability a in Abilities)
            {
                count += 1;
                DrawAbility(count, a, false);
            }

            count = 0;
            //foreach (Ability a in Player.CraftAbility)
            //{
            //    count += 1;
            //    DrawAbility(count, a, true);
            //}

            //if (ReDraw)
            //{
            //    DrawAbility(NowTab, true, true);
            //}

            return base.Run();
        }


        private void DrawAbility(int position, Ability Ability, bool LeftRight)
        {
            switch (position) { case 1: { position = 5; break; } case 2: { position = 10; break; } case 3: { position = 15; break; } case 4: { position = 20; break; } }

            int Side = 0;
            if (LeftRight)
            {
                Side = 52;
            }
            else
            {
                Side = 28;
            }

            //Title
            if (LeftRight)
            {
                this.Write(3, Side + 4, new DrawText("Способности", ConsoleColor.DarkGreen));
            }
            else
            {
                this.Write(3, Side + 4, new DrawText("Навыки", ConsoleColor.DarkGreen));
            }

            //name of ability
            this.Write(position, Side, new DrawText("┌───┐ ", ConsoleColor.Gray));
            this.Write(position, Side+6, new DrawText(Ability.Name, Ability.ForegroundColor));

            //
            this.Write(position+1, Side, new DrawText("│   │ ", ConsoleColor.Gray));

            //level
            //у способностей больше нет уровня, сасай
            //this.Write(position+1, Side+6, new DrawText("Lvl: ", ConsoleColor.DarkCyan));
            //this.Write(position + 1, Side + 11, new DrawText(Ability.Level.ToString(), ConsoleColor.DarkYellow));

            //icon
            this.Write(position + 1, Side + 2, Ability.Icon, Ability.ForegroundColor);

            //
            this.Write(position + 2, Side, "└───┘", ConsoleColor.Gray);

            //rate of COE
            ConsoleColor color = 0;            
            if (Ability.COE < 10) { color = ConsoleColor.DarkGray; }
            if (Ability.COE > 25) { color = ConsoleColor.Green; }
            if (Ability.COE > 50) { color = ConsoleColor.Yellow; }
            if (Ability.COE > 80) { color = ConsoleColor.Red; }
            
            this.Write(position + 2, Side + 6, "COE: " + Ability.COE.ToString() + "%", color);
        }
    }
}
