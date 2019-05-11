using System;
using Rogue.Entites.Enums;

namespace Rogue.Entites.Alive.Character
{
    /// <summary>
    /// Абстрактный класс персонажа
    /// </summary>
    public abstract class Player : Moveable
    {
        public Race Race { get; set; }

        public long EXP { get; set; }

        public long MaxExp => EXP * 2;

        public long Gold { get; set; }

        public virtual string ClassName { get; }

        public virtual string Resource => "";

        public virtual string ResourceName => "Мана";

        public virtual void AddToResource(double value) { }

        public virtual void RemoveToResource(double value) { }

        public virtual ConsoleColor ClassColor { get; }

        public virtual void AddClassPerk() { }

        public virtual string Avatar => "Rogue.Resources.Images.ui.player.noone.png";

        /// <summary>
        /// это пиздец, выпили это нахуй
        /// </summary>
        /// <returns></returns>
        public virtual ConsoleColor ResourceColor => ConsoleColor.Blue;

        #region Здесь часть которая имеет отношение к "безклассовому" персонажу

        public Player()
        {
            var timer = new System.Timers.Timer(3000);
            timer.Elapsed += RestoreActions;
            timer.Start();
        }

        private void RestoreActions(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (Actions >= 5)
            {
                return;
            }

            Actions++;
        }

        public int Actions { get; set; } = 5;


        #endregion
    }
}