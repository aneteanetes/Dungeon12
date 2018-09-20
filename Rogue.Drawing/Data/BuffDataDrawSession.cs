namespace Rogue.Drawing.Data
{
    using System.Collections.Generic;
    using Rogue.Drawing.Impl;
    using Rogue.Entites.Alive.Character;
    using Rogue.Entites.Enemy;
    using Rogue.View.Interfaces;

    public class BuffDataDrawSession : DrawSession
    {
        public BuffDataDrawSession()
        {
            this.AutoClear = false;
        }

        public Enemy Enemy { get; set; }

        public Player Player { get; set; }

        public override IDrawSession Run()
        {
            // здесь будут applicable modifiers
            //this.DrawObject(3, Enemy.States);
            //this.DrawObject(96, Player.States);

            return base.Run();
        }

        private void DrawObject(int left, IEnumerable<IDrawable> states)
        {
            int pos = 5;
            if (states != null)
            {
                foreach (var state in states)
                {
                    pos++;
                    this.Write(pos, left, state.Icon, state.ForegroundColor);
                }
            }
        }
    }
}
