namespace Rogue.Abilities.Talants
{
    using Rogue.Abilities.Talants.NotAPI;
    using Rogue.Classes;
    using Rogue.Map;
    using Rogue.Map.Objects;
    using Rogue.Transactions;
    using Rogue.Types;
    using Rogue.View.Interfaces;
    using System;


    public abstract class Talant<TClass> : TalantBase
         where TClass : Character
    {
        public Talant(int order):base(order)
        {

        }

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