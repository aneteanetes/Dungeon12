namespace Dungeon12.Noone
{
    using Dungeon;
    using Dungeon.Drawing;
    using Dungeon.View.Interfaces;
    using Dungeon12;
    using Dungeon12.Abilities;
    using Dungeon12.Abilities.Talants.TalantTrees;
    using Dungeon12.Classes;
    using Dungeon12.Entities.Alive;
    using Dungeon12.Noone.Abilities;
    using Dungeon12.Noone.Proxies;
    using Dungeon12.Noone.Talants;
    using Dungeon12.SceneObjects;
    using System;

    public class Noone : Character
    {
        TimerTrigger acttimer;

        public Noone(bool @new) : base(true)
        {
            acttimer = Global.Time.Timer("NooneActions")
                .After(2500)
                .Do(RestoreActions)
                .Repeat()
                .SceneFree()
                .Auto();

            this.MinDMG = 1;
            this.MaxDMG = 2;

            this.Actions = 5;
        }

        public override void Destroy()
        {
            acttimer?.Dispose();
            base.Destroy();
        }

        public Noone()
        {
            Global.Time.Timer("NooneActions")
                .After(2500)
                .Do(RestoreActions)
                .Repeat()
                .Auto();

            this.MinDMG = 1;
            this.MaxDMG = 2;

            this.Actions = 5;
        }

        public override double HitPointsPercentPlus => 10;

        public override string Avatar => "Images/noone.png".NoonePath();

        public override string ClassName { get => "Страж"; }

        public override IDrawColor ClassColor => DrawColor.SaddleBrown;

        public override string ResourceName => "Действия";

        public override string Resource => this.Actions.ToString();

        public override ConsoleColor ResourceColor => ConsoleColor.White;

        private void RestoreActions()
        {
            if (Actions >= 5)
            {
                return;
            }

            Actions++;
        }

        private int _actions = 5;
        public int Actions
        {
            get => _actions;
            set
            {
                _actions = value;
                if (_actions < 0)
                {
                    _actions = 0;
                }
            }
        }

        public override string Tileset => "Images/sprite.png".NoonePath();

        [ClassStat("Блок", ConsoleColor.DarkGreen, 1, "При получении урона есть шанс равный блоку уменьшить урон на процентное соотношение равное блоку.")]
        public long Block { get; set; } = 0;

        [ClassStat("Паррирование", ConsoleColor.Yellow, 1, "При атаке полученные удары в ближнем бою станут скользящими - будут наносить только половину урона, а время восстановления атаки сбросится.")]
        public long Parry { get; set; } = 0;

        public bool InParry { get; set; }

        [ClassStat("Выносливость", ConsoleColor.DarkRed, "Каждая единица выносливости увеличивает здоровье на 2 еденицы")]
        public long Stamina { get; set; } = 10;

        [ClassStat("Броня", ConsoleColor.DarkCyan, "Броня уменьшает урон от критических и других особых видов атак в ближнем бою на прямое кол-во урона.")]
        public long Armor { get; set; } = 0;

        public override void InitProxyProperties()
        {
            AddProxyProperty(nameof(MaxHitPoints), new StaminaProxyProperty());
        }

        public AbsorbingTalants Absorbing { get; set; } = new AbsorbingTalants();

        public DamageTalants DamageTalants { get; set; } = new DamageTalants();

        public Attack Attack { get; set; } = new Attack();

        public Defstand Defstand { get; set; } = new Defstand();

        public ShockWave ShockWave { get; set; } = new ShockWave();

        public Defaura Defaura { get; set; } = new Defaura();

        public override string MainAbilityDamageText => $"Атака: {Attack.ScaledValue(this, Attack.Value)}";

        public override IDrawText MainAbilityDamageView => MainAbilityDamageText.AsDrawText().Montserrat().InColor(DrawColor.SandyBrown);

        public override T PropertyOfType<T>()
        {
            switch (typeof(T))
            {
                case Type abs when abs == typeof(AbsorbingTalants): return Absorbing as T;
                case Type abs when abs == typeof(DamageTalants): return DamageTalants as T;
                default: return default;
            }
        }

        public override T[] PropertiesOfType<T>()
        {
            switch (typeof(T))
            {
                case Type t when t.IsAssignableFrom(typeof(Ability)):
                    return new T[]
{
                        Attack as T,
                        Defstand as T,
                        ShockWave as T,
                        Defaura as T
};
                case Type t when t.IsAssignableFrom(typeof(TalantTree)):
                    return new T[]
{
                        Absorbing as T,
                        DamageTalants as T
};
                default: return default;
            }
        }

        protected override long DamageProcess(Damage dmg, long amount)
        {
            if (this.InParry)
            {
                amount /= 2;

                var text = $"Паррировано: {amount}!".AsDrawText().InColor(DrawColor.Red).Montserrat();

                this.SceneObject.ShowInScene(new PopupString(text, this.MapObject.Location).InList<ISceneObject>());

                return amount;
            }

            if (RandomDungeon.Chance(this.Block))
            {
                var block = (long)Math.Floor(amount * (this.Block / 100d));

                var text = $"Блок: {block}!".AsDrawText().InColor(DrawColor.Red).Montserrat();

                this.SceneObject.ShowInScene(new PopupString(text, this.MapObject.Location).InList<ISceneObject>());

                return amount - block;
            }

            return amount;
        }

        public bool InDefstand { get; set; }

        protected long DamagePhysical(Damage dmg)
        {
            if (!InDefstand)
            {
                RestoreActions();
            }
            return dmg.Amount - this.Armor / 2;
        }
    }
}