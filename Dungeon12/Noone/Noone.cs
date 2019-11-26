namespace Dungeon12.Noone
{
    using Dungeon.Abilities;
    using Dungeon.Abilities.Talants.TalantTrees;
    using Dungeon12.Noone.Abilities;
    using Dungeon12.Noone.Talants;
    using Dungeon12.Noone.Talants.Defensible;
    using Dungeon.Entities.Animations;
    using Dungeon.Types;
    using System;
    using Dungeon;
    using Dungeon.Drawing.SceneObjects;
    using System.Collections.Generic;
    using Dungeon.Classes;
    using Dungeon.Drawing;
    using Dungeon.View.Interfaces;
    using Dungeon12.Noone.Proxies;
    using Dungeon.Entities.Alive;
    using Dungeon.SceneObjects;

    public class Noone : Dungeon12Class
    {
        public Noone()
        {
            var timer = new System.Timers.Timer(3000);
            timer.Elapsed += RestoreActions;
            timer.Start();

            this.HitPoints = this.MaxHitPoints = 100;
            this.Level = 1;
            this.MinDMG = 1;
            this.MaxDMG = 2;

            this.Actions = 5;
        }

        public override string Avatar => "Images/noone.png".NoonePath();

        public override string ClassName { get => "Страж"; }

        public override IDrawColor ClassColor => DrawColor.SaddleBrown;

        public override string ResourceName => "Действия";

        public override string Resource => this.Actions.ToString();

        public override ConsoleColor ResourceColor => ConsoleColor.White;

        private void RestoreActions(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (Actions >= 5)
            {
                return;
            }

            Actions++;
        }

        public int Actions { get; set; } = 5;

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

        public DefensibleTalants Defensible { get; set; } = new DefensibleTalants();

        public Attack Attack { get; set; } = new Attack();

        public Defstand Defstand { get; set; } = new Defstand();

        public ShieldSkill ShieldSkill { get; set; } = new ShieldSkill();

        public Defaura Defaura { get; set; } = new Defaura();

        public override string MainAbilityDamageText => $"Атака: {1 * (this.AttackPower==0 ? 1 : this.AttackPower * 0.25)}-{3 * (this.AttackPower == 0 ? 1 : this.AttackPower * 0.25)}";

        public override IDrawText MainAbilityDamageView => MainAbilityDamageText.AsDrawText().Montserrat().InColor(DrawColor.SandyBrown);

        public override T PropertyOfType<T>()
        {
            switch (typeof(T))
            {
                case Type abs when abs == typeof(AbsorbingTalants): return Absorbing as T;
                case Type abs when abs == typeof(DefensibleTalants): return Defensible as T;
                default: return default;
            }
        }

        public override T[] PropertiesOfType<T>()
        {
            switch (typeof(T))
            {
                case Type t when t.IsAssignableFrom(typeof(Ability)): return new T[]
                    {
                        Attack as T,
                        Defstand as T,
                        ShieldSkill as T,
                        Defaura as T
                    };
                case Type t when t.IsAssignableFrom(typeof(TalantTree)): return new T[]
                    {
                        Absorbing as T,
                        Defensible as T
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

        protected long DamagePhysical(Damage dmg)
        {
            return dmg.Amount - this.Armor / 2;
        }
    }
}
