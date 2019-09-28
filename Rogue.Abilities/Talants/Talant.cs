namespace Rogue.Abilities.Talants
{
    using Rogue.Classes;
    using Rogue.Map;
    using Rogue.Map.Objects;
    using Rogue.Transactions;
    using Rogue.Types;
    using Rogue.View.Interfaces;
    using System;

    public abstract class Talant<TClass> : Applicable, IDrawable
         where TClass : Character
    {
        /// <summary>
        /// Метод вызывается для того что бы забиндить параметры для <see cref="Applicable.Apply(object)"/> и <see cref="Applicable.Discard(object)"/>
        /// </summary>
        /// <param name="gameMap"></param>
        /// <param name="avatar"></param>
        /// <param name="class"></param>
        public void Bind(GameMap gameMap, Avatar avatar, TClass @class)
        {
            this.Class = @class;
            this.GameMap = gameMap;
            this.Avatar = avatar;
        }

        public TClass Class { get; set; }

        public Avatar Avatar { get; set; }

        public GameMap GameMap { get; set; }

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

        public bool Active { get; set; }
        
        public virtual bool CanUse(object @object)
        {
            return this.CallCanUse(@object as dynamic);
        }

        protected abstract bool CallCanUse(dynamic obj);

        public virtual TalantInfo TalantInfo(object @object)
        {
            return this.CallTalantInfo(@object as dynamic);
        }

        protected abstract TalantInfo CallTalantInfo(dynamic obj);
    }
}