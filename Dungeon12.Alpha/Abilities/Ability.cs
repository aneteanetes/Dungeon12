namespace Dungeon12.Abilities
{
    using Dungeon12.Abilities.Enums;
    using Dungeon12.Abilities.Scaling;
    using Dungeon12.Abilities.Talants;
    using Dungeon12.Classes;
    using Dungeon.Control;
    using Dungeon12.Entities.Alive;
    using Dungeon.GameObjects;
    using Dungeon12.Map;
    using Dungeon12.Map.Objects;
    using Dungeon.Physics;
    using Dungeon12.SceneObjects;
    using Dungeon.SceneObjects;
    using Dungeon.Types;
    using Dungeon.View.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Dungeon;
    using Dungeon12;
    using Newtonsoft.Json;

    /// <summary>
    /// тут кто-то явно сэкономил на времени и въебал виртуальные свойства вместо абстрактных
    /// </summary>
    public abstract class Ability : GameComponent
    {
        public Ability()
        {
            this.Image = DefaultPath().Cache();

            this.Image_B = DefaultPath(true).Cache();
        }

        public virtual void BuildScales() { }

        /// <summary>
        /// Навык доступен в защищённой зоне
        /// </summary>
        public virtual bool AvailableInSafeZone => false;

        /// <summary>
        /// Флаг указывающий что надо говорить о том что способность не может быть использована
        /// </summary>
        public virtual bool Notifying { get; set; } = true;

        [Newtonsoft.Json.JsonIgnore]
        /// <summary>
        /// Добавить эффекты при использовании
        /// </summary>
        public Action<List<ISceneObject>> UseEffects;

        [Newtonsoft.Json.JsonIgnore]
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

        public virtual IEnumerable<ScaleRateInfo> Rates => Enumerable.Empty<ScaleRateInfo>();


        public virtual long ScaledValue() => Value;

        /// <summary>
        /// Наименование навыка
        /// </summary>
        public abstract string Name { get; }

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

        public virtual long Value => 0;

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
                    if (this.Notifying)
                    {
                        var txt = "Способность восстанавливается".AsDrawText().InSize(10).Montserrat();
                        Toast.Show(txt);
                    }
                    return false;
                }
            }

            var available = CastAvailable(avatar);

            if (!available)
            {
                if (this.Notifying)
                {
                    var txt = "Невозможно использовать способность!".AsDrawText().InSize(10).Montserrat();
                    Toast.Show(txt);
                }
            }

            return available;
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

        [JsonIgnore]
        public Capable Target { get; set; }

        /// <summary>
        /// Стоимость навыка
        /// </summary>
        public virtual string Spend { get; set; }

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

        private ScaleRateBuilded<TClass> scale;

        public Ability()
        {
            scale = this.Scale.Build();
        }

        public override void BuildScales()
        {
            scale = this.Scale.Build();
        }

        public long ScaledValue(TClass @class, long value) => scale.Scale(@class, value);

        public long ScaledValue(TClass @class) => scale.Scale(@class, Value);

        public override long ScaledValue() => ScaledValue(Global.GameState.Character as TClass);

        public override IEnumerable<ScaleRateInfo> Rates => scale.Scales;

        /// <summary>
        /// Скалирование навыка от характеристик
        /// </summary>
        public abstract ScaleRate<TClass> Scale { get; }

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