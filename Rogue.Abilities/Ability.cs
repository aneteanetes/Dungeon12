namespace Rogue.Abilities
{
    using Rogue.Abilities.Enums;
    using Rogue.Abilities.Scaling;
    using Rogue.Abilities.Talants;
    using Rogue.Entites.Alive.Character;
    using Rogue.Map;
    using Rogue.Map.Objects;
    using Rogue.Physics;
    using Rogue.View.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// тут кто-то явно сэкономил на времени и въебал виртуальные свойства вместо абстрактных
    /// </summary>
    public abstract class Ability
    {
        public Ability()
        {
            this.Image = DefaultPath();

            this.Image_B = DefaultPath(true);
        }

        /// <summary>
        /// Навык доступен в защищённой зоне
        /// </summary>
        public virtual bool AvailableInSafeZone => false;

        /// <summary>
        /// Добавить эффекты при использовании
        /// </summary>
        public Action<List<ISceneObject>> UseEffects;

        /// <summary>
        /// Размер области где действует способность
        /// </summary>
        public virtual PhysicalObject Range { get; }

        /// <summary>
        /// Позиция способности от 1 до 4
        /// </summary>
        public abstract int Position { get; }

        public virtual AbilityPosition AbilityPosition { get; }

        /// <summary>
        /// вот блядь виртуал почти всегда лень ебаная
        /// </summary>
        public virtual string Description { get; }

        /// <summary>
        /// Наименование навыка
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Скалирование навыка от характеристики
        /// </summary>
        public abstract ScaleRate Scale { get; }

        public virtual string Icon { get; set; }

        public virtual string Image { get; set; }

        public virtual string Image_B { get; set; }
        
        private string DefaultPath(bool big = false)
        {
            var thisType = this.GetType();
            var baseType = thisType.BaseType;

            if(!baseType.IsGenericType)
            {
                return null;
            }

            var type = thisType.FullName;
            var generic = baseType.GetGenericArguments().First().Name.ToString();

            var b = big
                ? "b"
                : "";

            var val = type.Replace(generic, $"{generic}.Images")
                + $".img{b}.png";

            return val;
        }

        public virtual IDrawColor BackgroundColor { get; set; }

        public virtual IDrawColor ForegroundColor { get; set; }

        /// <summary>
        /// Коэффицент полезности навыка
        /// </summary>
        public int COE => 0;

        public virtual double Value => 0;

        public virtual bool Hold => false;

        public virtual void Release(GameMap map, Avatar avatar) { }

        public virtual bool CastAvailable(Avatar avatar) => true;

        public virtual void Cast(GameMap map, Avatar avatar)
        {

        }
    }


    /// <summary>
    /// Навык принадлежащий классу
    /// </summary>
    /// <typeparam name="TClass">Класс которому принадлежит способность</typeparam>
    /// <typeparam name="TTalants">Тип талантов этой способности</typeparam>
    public abstract class Ability<TClass, TTalants> : Ability
        where TClass: Player
        where TTalants : TalantTree
    {
        /// <summary>
        /// Скастовать способность
        /// </summary>
        /// <param name="class"></param>
        /// <param name="talants"></param>
        public void Cast(TClass @class, TTalants talants)
        {
            if (CastAvailable(@class, talants))
                InternalCast(@class, talants);
        }

        protected abstract void InternalCast(TClass @class, TTalants talants);

        /// <summary>
        /// Проверить может ли способность выполнена
        /// </summary>
        /// <param name="class"></param>
        /// <param name="talants"></param>
        /// <returns></returns>
        public abstract bool CastAvailable(TClass @class, TTalants talants);
    }

    /// <summary>
    /// Навык принадлежащий классу без таланта
    /// </summary>
    /// <typeparam name="TClass">Класс которому принадлежит способность</typeparam>
    public abstract class Ability<TClass> : Ability
        where TClass : Player
    {
        protected abstract bool CanUse(TClass @class);

        public override bool CastAvailable(Avatar avatar)
        {
            return CanUse(avatar.Character as TClass);
        }

        protected abstract void Use(GameMap gameMap, Avatar avatar, TClass @class);

        protected abstract void Dispose(GameMap gameMap, Avatar avatar, TClass @class);

        public override void Cast(GameMap map, Avatar avatar)
            => Use(map, avatar, avatar.Character as TClass);

        public override void Release(GameMap map, Avatar avatar)
            => Dispose(map, avatar, avatar.Character as TClass);
    }
}