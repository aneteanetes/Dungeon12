namespace Rogue.Abilities.Talants
{
    using Rogue.Classes;
    using Rogue.Map;
    using Rogue.Map.Objects;
    using Rogue.Types;
    using Rogue.View.Interfaces;
    using System;

    public abstract class Talant<TClass> : IDrawable
         where TClass : Character
    {
        public string Icon { get; set; }
        public virtual string Name { get; set; }
        public IDrawColor BackgroundColor { get; set; }
        public IDrawColor ForegroundColor { get; set; }

        public virtual string Description { get; set; }

        public int Tier { get; set; }

        public int Level { get; set; }

        public bool Available => Level > 0;

        public string Tileset => "";

        public Rectangle TileSetRegion => default;

        public Rectangle Region { get; set; }

        public bool Container => false;

        /// <summary>
        /// Массив наименований других талантов в дереве от которого зависит этот
        /// </summary>
        public string[] DependsOn { get; set; }
        
        public bool Opened => Level > 0;

        public abstract bool CanUse(TClass @class, Ability ability);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameMap"></param>
        /// <param name="avatar"></param>
        /// <param name="class"></param>
        /// <param name="base"></param>
        /// <param name="ability"></param>
        /// <returns>Возвращает true если базовый навык использовать не нужно</returns>
        public abstract bool Use(GameMap gameMap, Avatar avatar, TClass @class, Action<GameMap, Avatar, TClass> @base, Ability ability);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameMap"></param>
        /// <param name="avatar"></param>
        /// <param name="class"></param>
        /// <param name="base"></param>
        /// <param name="talantTree"></param>
        /// <returns></returns>
        public abstract bool Dispose(GameMap gameMap, Avatar avatar, TClass @class, Action<GameMap, Avatar, TClass> @base, Ability ability);
    }
}