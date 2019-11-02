namespace Dungeon.Abilities
{
    using Dungeon.Abilities.Enums;
    using Dungeon.Abilities.Scaling;
    using Dungeon.Abilities.Talants;
    using Dungeon.Classes;
    using Dungeon.Control.Pointer;
    using Dungeon.Entites.Alive;
    using Dungeon.Map;
    using Dungeon.Map.Objects;
    using Dungeon.Physics;
    using Dungeon.Types;
    using Dungeon.View.Interfaces;
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

        public MapObject Owner { get; set; }

        /// <summary>
        /// Размер области где действует способность
        /// </summary>
        public virtual PhysicalObject Range => Owner.Grow(RangeMultipler);

        protected virtual double RangeMultipler => 1;

        /// <summary>
        /// Позиция способности от 1 до 4
        /// </summary>
        public virtual int Position { get; } = -1;

        public abstract AbilityPosition AbilityPosition { get; }

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
            var abilityType = FindAbilityGenericType(thisType);
            
            if (abilityType==default)
            {
                return null;
            }

            var type = thisType.FullName;
            var generic = abilityType.GetGenericArguments().First().Name.ToString();

            var b = big
                ? "b"
                : "";

            var val = type.Replace(generic, $"{generic}.Resources.Images") + $".img{b}.png";

            return val;
        }

        private static Dictionary<Type, Type> AbilTypeCache = new Dictionary<Type, Type>();
        private Type Cached(Type key, Type value)
        {
            if (!AbilTypeCache.ContainsKey(key))
            {
                AbilTypeCache.Add(key, value);
            }

            return AbilTypeCache[key];
        }

        private Type FindAbilityGenericType(Type type, Type originalType = null)
        {
            originalType = originalType ?? type;

            if (AbilTypeCache.TryGetValue(originalType, out var value))
            {
                return value;
            }

            if (type?.BaseType?.IsGenericType ?? false)
            {
                var abilityTalant = type?.BaseType.GetGenericTypeDefinition() == typeof(Ability<,>);
                var abilityClass = type?.BaseType.GetGenericTypeDefinition() == typeof(Ability<>);

                if (abilityTalant)
                {
                    return Cached(originalType, type?.BaseType);
                }

                if (abilityClass)
                {
                    return Cached(originalType, type?.BaseType);
                }
            }


            if (type?.BaseType == default || type?.BaseType == typeof(object))
            {
                return Cached(originalType, default);
            }

            return FindAbilityGenericType(type.BaseType, originalType);
        }

        public virtual IDrawColor BackgroundColor { get; set; }

        public virtual IDrawColor ForegroundColor { get; set; }

        /// <summary>
        /// Коэффицент полезности навыка
        /// </summary>
        public int COE => 0;

        public virtual double Value => 0;

        public int Level { get; set; } = 1;

        public virtual bool Hold => ActionType == AbilityActionAttribute.Hold;

        public bool PassiveWorking { get; set; }

        public virtual void Release(GameMap map, Avatar avatar) { }

        protected virtual bool CastAvailable(Avatar avatar) => true;

        /// <summary>
        /// Проверить возможность использования способности учитывая <see cref="Cooldown"/>
        /// </summary>
        /// <param name="avatar"></param>
        public bool CastAvailableCooldown(Avatar avatar)
        {
            if (this.Cooldown != default)
            {
                if (!this.Cooldown.Check())
                {
                    return false;
                }
            }

            return CastAvailable(avatar);
        }

        protected PointerArgs PointerLocation => Global.PointerLocation;

        /// <summary>
        /// Использвоание способности учитывая <see cref="Cooldown"/>, для <see cref="AbilityCastType.Passive"/> вызывается когда биндится
        /// </summary>
        /// <param name="map"></param>
        /// <param name="avatar"></param>
        public void CastCooldown(GameMap map, Avatar avatar)
        {
            if (this.Cooldown != default)
            {
                this.Cooldown.Cast();
            }

            Cast(map, avatar);
        }

        /// <summary>
        /// Использвоание способности, для <see cref="AbilityCastType.Passive"/> вызывается когда биндится
        /// </summary>
        /// <param name="map"></param>
        /// <param name="avatar"></param>
        protected virtual void Cast(GameMap map, Avatar avatar)
        {

        }

        public Action OnCast { get; set; }

        public Action OnCastEnd { get; set; }

        public Action OnCastRelease { get; set; }

        public virtual Location CastLocation { get; set; }

        public abstract AbilityActionAttribute ActionType { get; }

        public virtual AbilityCastType CastType { get; set; }

        public abstract AbilityTargetType TargetType { get; }

        public Capable Target { get; set; }

        public virtual double Spend { get; set; }

        public List<(string, string)> Scales { get; set; }

        public virtual Cooldown Cooldown { get; }
    }

    /// <summary>
    /// Навык принадлежащий классу
    /// </summary>
    /// <typeparam name="TClass">Класс которому принадлежит способность</typeparam>
    /// <typeparam name="TTalants">Тип талантов этой способности</typeparam>
    public abstract class Ability<TClass, TTalants> : Ability<TClass>
        where TClass: Character
        where TTalants : TalantTree<TClass>, new()
    {
        protected override bool CastAvailable(Avatar avatar)
        {
            if (avatar.Character is TClass @class)
            {
                var @base = CanUse(@class);
                var talants = avatar.Character.PropertyOfType<TTalants>().CanUse(@class, this);

                return @base && talants;
            }

            return false;
        }

        protected override void Cast(GameMap map, Avatar avatar)
        {
            if (avatar.Character is TClass @class)
            {
                OnCast?.Invoke();
                avatar.Character.PropertyOfType<TTalants>().Use(map, avatar, @class, this.Use,this);
                OnCastEnd?.Invoke();
            }
        }

        public override void Release(GameMap map, Avatar avatar)
        {
            if (avatar.Character is TClass @class)
            {
                avatar.Character.PropertyOfType<TTalants>().Dispose(map, avatar, @class, this.Dispose,this);
                OnCastRelease?.Invoke();
            }
        }
    }

    /// <summary>
    /// Навык принадлежащий классу без таланта
    /// </summary>
    /// <typeparam name="TClass">Класс которому принадлежит способность</typeparam>
    public abstract class Ability<TClass> : Ability
        where TClass : Character
    {
        protected abstract bool CanUse(TClass @class);

        protected override bool CastAvailable(Avatar avatar)
        {
            return CanUse(avatar.Character as TClass);
        }

        protected abstract void Use(GameMap gameMap, Avatar avatar, TClass @class);

        protected abstract void Dispose(GameMap gameMap, Avatar avatar, TClass @class);

        protected override void Cast(GameMap map, Avatar avatar)
        {
            OnCast?.Invoke();
            Use(map, avatar, avatar.Character as TClass);
            OnCastEnd?.Invoke();
        }

        public override void Release(GameMap map, Avatar avatar)
        {
            Dispose(map, avatar, avatar.Character as TClass);
            OnCastRelease?.Invoke();
        }
    }

    public abstract class BaseCooldownAbility<TClass> : Ability<TClass>
        where TClass : Character
    {
        public override Cooldown Cooldown { get; } = BaseCooldown;

        public static Cooldown BaseCooldown => new Cooldown(500, typeof(TClass).Name);
    }
}