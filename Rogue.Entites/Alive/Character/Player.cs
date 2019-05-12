using System;
using System.Collections.Generic;
using Rogue.Entites.Animations;
using Rogue.Entites.Enums;
using Rogue.Types;

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
    }
}