﻿namespace Dungeon12.Noone
{
    using Dungeon.Abilities;
    using Dungeon.Abilities.Talants.TalantTrees;
    using Dungeon12.Noone.Abilities;
    using Dungeon12.Noone.Talants;
    using Dungeon12.Noone.Talants.Defensible;
    using Dungeon.Entites.Animations;
    using Dungeon.Types;
    using System;
    using Dungeon;
    using Dungeon.Drawing.SceneObjects;
    using System.Collections.Generic;
    using Dungeon.Classes;
    using Dungeon.Drawing;

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

        public override string ClassName { get => "Приключенец"; }
        
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

        public int Block { get; set; }

        public int Parry { get; set; }

        public int Stamina { get; set; }

        public int CritChance { get; set; }

        public AbsorbingTalants Absorbing { get; set; } = new AbsorbingTalants();

        public DefensibleTalants Defensible { get; set; } = new DefensibleTalants();

        public Attack Attack { get; set; } = new Attack();

        public Defstand Defstand { get; set; } = new Defstand();

        public ShieldSkill ShieldSkill { get; set; } = new ShieldSkill();

        public Defaura Defaura { get; set; } = new Defaura();

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

        public override IEnumerable<ClassStat> ClassStats => new ClassStat[]
        {
            new ClassStat("Блок",this.Block.ToString(), new DrawColor(ConsoleColor.White)),
            new ClassStat("Парирование",$"{this.Parry}%", new DrawColor(ConsoleColor.DarkCyan)),
            new ClassStat("Выносливость", $"{this.Stamina}",new DrawColor(ConsoleColor.Yellow)){  Group=1},
            new ClassStat("Шанс крит.", $"{this.CritChance}%",new DrawColor(ConsoleColor.DarkRed)){  Group=1},
        };
    }
}