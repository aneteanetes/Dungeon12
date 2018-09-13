using System;
using System.Collections.Generic;
using Rogue.Entites.Alive;

namespace Rogue.Entites.Character
{
    public class Character : Drawable
    {
        public Class Class;

        public Profession Proffession;

        public Race Race;

        public ClassInfo Class { get; set; }
                
        public int Level;

        private int _CHP, _MHP;

        public int MHP
        {
            set { _MHP = value; }
            get
            {
                return _MHP;
            }
        }

        public int CHP
        {
            set { _CHP = value; }
            get
            {
                foreach (Ability a in Rogue.RAM.Player.Buffs)
                {
                    if (a.Name == "Молитва смерти")
                    {
                        if (_CHP <= 0) { return 1; }
                        else { return _CHP; }
                    }
                }
                return _CHP;
            }
        }

        private int _EXP;

        public int EXP
        {
            set
            {
                _EXP = value;
                this.CheckingUp();
            }
            get { return _EXP; }
        }

        private int _CMP, _MMP;

        public int CMP
        {
            get { if (this.Class == BattleClass.BloodMage) { return CHP; } else if (this.Class == BattleClass.Warrior) { if (this._CMP > 100) { this._CMP = 100; } return _CMP; } else { return _CMP; } }
            set { _CMP = value; }
        }

        public int MMP
        {
            get { if (this.Class == BattleClass.BloodMage) { return MHP; } else if (this.Class == BattleClass.Warrior) { if (this._CMP > 100) { this._CMP = 100; } return _CMP; } else { return _MMP; } }
            set { _MMP = value; }
        }

        private int _AP;

        public int Rage
        {
            get
            {
                if (this.Class != BattleClass.Warrior) { return 0; }
                else
                {
                    return this.Ability[3].Power;
                }
            }
        }

        private int _AD;

        public int AP
        {
            set { _AP = value; }
            get
            {
                if (this.Class == BattleClass.Monk)
                {
                    if (this.MonkSpec == AbilityRate.AbilityPower) { return _AP + _AD; }
                    else { return 0; }
                }
                else { return _AP; }
            }
        }

        public int AD
        {
            set { _AD = value; }
            get
            {
                if (this.Class == BattleClass.Monk)
                {
                    if (this.MonkSpec == AbilityRate.AttackDamage) { return _AP + _AD; }
                    else { return 0; }
                }
                else { return _AD; }
            }
        }

        public int ARM, MRS, mEXP;

        private int _MIDMG;

        private int _MADMG;

        public int MIDMG
        {
            set { _MIDMG = value; }
            get
            {
                int additionalrage = 0;
                foreach (SystemEngine.ArmorSet aSet in Rogue.RAM.ArmSet)
                {
                    if (aSet.Name == "M5" && aSet.Active)
                    {
                        additionalrage = 1;
                    }
                }

                int rtrn = _MIDMG;
                if (this.Class == BattleClass.Monk)
                {
                    if (this.Equipment != null)
                    {
                        if (this.Equipment.Weapon != null)
                        {
                            if (this.Equipment.Weapon.Staff == true || additionalrage == 1)
                            {
                                foreach (Ability a in this.Ability)
                                {
                                    if (a.Name == "Стиль посоха")
                                    { rtrn = _MIDMG + a.Power; }
                                }

                            }
                        }
                    }
                }
                if (rtrn > this.MADMG) { rtrn = this.MADMG; }
                return rtrn;
            }
        }

        public int MADMG
        {
            set { _MADMG = value; }
            get
            {
                int additionalrage = 0;
                foreach (SystemEngine.ArmorSet aSet in Rogue.RAM.ArmSet)
                {
                    if (aSet.Name == "M5" && aSet.Active)
                    {
                        additionalrage = 1;
                    }
                }

                if (this.Class == BattleClass.Monk)
                {
                    if (this.Equipment != null)
                    {
                        if (this.Equipment.Weapon != null)
                        {
                            if (this.Equipment.Weapon.Staff == true || additionalrage == 1)
                            {
                                foreach (Ability a in this.Ability)
                                {
                                    if (a.Name == "Стиль посоха")
                                    { return _MADMG + a.Power; }
                                }
                            }
                        }
                    }
                }
                return _MADMG;
            }
        }

        public int UpExp
        {
            get
            {
                return Convert.ToInt32(this.Level * SystemEngine.Helper.Information.Class) + Convert.ToInt32(this.Level * SystemEngine.Helper.Information.Race) + Convert.ToInt32(this.Level * 1.753);
            }
        }

        public double ReservedManaPoints;

        private int _Gold;

        public int Gold
        {
            set { this.QuestGold = (value - _Gold); _Gold = value; }
            get { return _Gold; }
        }

        public int AbPoint, CrPoint;

        public List<Item> Inventory;
        /// <summary>
        /// Return class or race or profession
        /// </summary>
        /// <param name="Object">1=Race;2=Class;3=Proffession;</param>
        /// <returns></returns>
        public string GetClassRace(int Object)
        {
            string s = string.Empty;
            if (Object == 1)
            {
                #region Race

                switch (Race)
                {
                    case MechEngine.Race.Human:
                        {
                            s = "Человек";
                            break;
                        }
                    case MechEngine.Race.Elf:
                        {
                            s = "Азрай"; //лесной эльф
                            break;
                        }
                    case MechEngine.Race.DarkElf:
                        {
                            s = "Дроу"; // темный эльф
                            break;
                        }
                    case MechEngine.Race.Dwarf:
                        {
                            s = "Дварф";
                            break;
                        }
                    case MechEngine.Race.Gnome:
                        {
                            s = "Гном";
                            break;
                        }
                    case MechEngine.Race.MoonElf:
                        {
                            s = "Калдорай"; // лунный эльф
                            break;
                        }
                    case MechEngine.Race.Orc:
                        {
                            s = "Орк";
                            break;
                        }
                    case MechEngine.Race.Troll:
                        {
                            s = "Тролль";
                            break;
                        }
                    case MechEngine.Race.Undead:
                        {
                            s = "Отрекшийся";
                            break;
                        }
                    case MechEngine.Race.FallenAngel:
                        {
                            s = "Падший";
                            break;
                        }
                }

                #endregion
            }
            if (Object == 2)
            {
                #region Class

                switch (Class)
                {
                    case MechEngine.BattleClass.BloodMage:
                        {
                            s = "Маг крови";
                            break;
                        }
                    case MechEngine.BattleClass.Alchemist:
                        {
                            s = "Алхимик";
                            break;
                        }
                    case MechEngine.BattleClass.Assassin:
                        {
                            s = "Разбойник";
                            break;
                        }
                    case MechEngine.BattleClass.FireMage:
                        {
                            s = "Маг огня";
                            break;
                        }
                    case MechEngine.BattleClass.Inquisitor:
                        {
                            s = "Экзорцист";
                            break;
                        }
                    case MechEngine.BattleClass.Monk:
                        {
                            s = "Монах";
                            break;
                        }
                    case MechEngine.BattleClass.Warrior:
                        {
                            s = "Воин";
                            break;
                        }
                    case MechEngine.BattleClass.Necromant:
                        {
                            s = "Некромант";
                            break;
                        }
                    case MechEngine.BattleClass.Paladin:
                        {
                            s = "Паладин";
                            break;
                        }
                    case MechEngine.BattleClass.Shaman:
                        {
                            s = "Шаман";
                            break;
                        }
                    case MechEngine.BattleClass.Illusionist:
                        {
                            s = "Иллюзионист";
                            break;
                        }
                    case MechEngine.BattleClass.LightWarrior:
                        {
                            s = "Орден Молнии";
                            break;
                        }
                    case MechEngine.BattleClass.Valkyrie:
                        {
                            s = "Валькирия";
                            break;
                        }
                    case MechEngine.BattleClass.Warlock:
                        {
                            s = "Варлок";
                            break;
                        }
                }

                #endregion

            }
            if (Object == 3)
            {
                #region Proffession

                switch (Proffession)
                {
                    case MechEngine.CraftClass.Miner:
                        {
                            s = "Шахтер";
                            break;
                        }
                    case MechEngine.CraftClass.Herbalist:
                        {
                            s = "Собиратель";
                            break;
                        }
                    case MechEngine.CraftClass.Blacksmith:
                        {
                            s = "Кузнец";
                            break;
                        }
                    case MechEngine.CraftClass.Alchemist:
                        {
                            s = "Алхимик";
                            break;
                        }
                }

                #endregion
            }
            return s;
        }

        public void GetClassRacePerk(int Object)
        {
            if (Object == 1)
            {
                #region Race

                switch (Race)
                {
                    case MechEngine.Race.Human:
                        {
                            MechEngine.Perk.AddPerk(new string[] { "HP + 1" }, "Люди", "Самая приспосабливаемая раса.", '#', ConsoleColor.DarkYellow);
                            Rogue.RAM.Player.CraftAbility[2] = DataBase.OtherAbilityBase.Human;
                            break;
                        }
                    case MechEngine.Race.Elf:
                        {
                            MechEngine.Perk.AddPerk(new string[] { "MP + 1" }, "Лесные Эльфы", "Мастера в собирании магии.", '¤', ConsoleColor.Green);
                            Rogue.RAM.Player.CraftAbility[2] = DataBase.OtherAbilityBase.Elf;
                            break;
                        }
                    case MechEngine.Race.DarkElf:
                        {
                            MechEngine.Perk.AddPerk(new string[] { "AP + 1" }, "Темные Эльфы", "Мастера в использовании магии.", '»', ConsoleColor.DarkMagenta);
                            Rogue.RAM.Player.CraftAbility[2] = DataBase.OtherAbilityBase.DarkElf;
                            break;
                        }
                    case MechEngine.Race.Dwarf:
                        {
                            MechEngine.Perk.AddPerk(new string[] { "ARM + 2", "MP - 5" }, "Дварфы", "Лучшая броня, но без магии.", '¶', ConsoleColor.DarkGray);
                            Rogue.RAM.Player.CraftAbility[2] = DataBase.OtherAbilityBase.Dwarf;
                            break;
                        }
                    case MechEngine.Race.Gnome:
                        {
                            MechEngine.Perk.AddPerk(new string[] { "MIDMG + 1" }, "Гномы", "Самая прозорливая раса.", '↨', ConsoleColor.Gray);
                            Rogue.RAM.Player.CraftAbility[2] = DataBase.OtherAbilityBase.Gnome;
                            break;
                        }
                    case MechEngine.Race.MoonElf:
                        {
                            MechEngine.Perk.AddPerk(new string[] { "AP + 3", "HP - 4" }, "Лунные Эльфы", "Тело разъедает магическая сила.", '▀', ConsoleColor.Blue);
                            Rogue.RAM.Player.CraftAbility[2] = DataBase.OtherAbilityBase.MoonElf;
                            break;
                        }
                    case MechEngine.Race.Orc:
                        {
                            MechEngine.Perk.AddPerk(new string[] { "MIDMG + 1", "MADMG + 1" }, "Орки", "Самые сильные войны.", '‼', ConsoleColor.Red);
                            Rogue.RAM.Player.CraftAbility[2] = DataBase.OtherAbilityBase.Orc;
                            break;
                        }
                    case MechEngine.Race.Troll:
                        {
                            MechEngine.Perk.AddPerk(new string[] { "AD + 2", "HP - 2" }, "Тролли", "Настоящие охотники.", '╘', ConsoleColor.DarkYellow);
                            Rogue.RAM.Player.CraftAbility[2] = DataBase.OtherAbilityBase.Troll;
                            break;
                        }
                    case MechEngine.Race.Undead:
                        {
                            MechEngine.Perk.AddPerk(new string[] { "MRS + 3", "ARM - 4", }, "Отрекшиеся", "Врожденный иммунитет к магии.", '░', ConsoleColor.Magenta);
                            Rogue.RAM.Player.CraftAbility[2] = DataBase.OtherAbilityBase.Undead;
                            break;
                        }
                    case MechEngine.Race.FallenAngel:
                        {
                            MechEngine.Perk.AddPerk(new string[] { "MADMG + 1" }, "Падший", "Разгневавший богов.", '☼', ConsoleColor.Cyan);
                            Rogue.RAM.Player.CraftAbility[2] = DataBase.OtherAbilityBase.FallenAngel;
                            break;
                        }
                }

                #endregion
            }
            if (Object == 2)
            {
                #region Class

                switch (Class)
                {
                    case MechEngine.BattleClass.BloodMage:
                        {
                            MechEngine.Perk.AddPerk(new string[] { "HP + 1", "AP + 1" }, "Кровавый", "Магия крови - запрещенная магия.", '°', ConsoleColor.DarkRed);
                            break;
                        }
                    case MechEngine.BattleClass.Warrior:
                        {
                            MechEngine.Perk.AddPerk(new string[] { "HP + 1", "ARM + 1" }, "Опытный", "Ваш опыт заработан в бою.", '{', ConsoleColor.Blue);
                            break;
                        }
                    case MechEngine.BattleClass.Alchemist:
                        {
                            MechEngine.Perk.AddPerk(new string[] { "HP + 1" }, "Пропитанный", "Вся ваша кожа пропитанна эликсирами.", 'Ū', ConsoleColor.Magenta);
                            break;
                        }
                    case MechEngine.BattleClass.Assassin:
                        {
                            MechEngine.Perk.AddPerk(new string[] { "AD + 2" }, "Убийца", "Вся ваша жизнь - убийства.", '√', ConsoleColor.DarkRed);
                            break;
                        }
                    case MechEngine.BattleClass.FireMage:
                        {
                            MechEngine.Perk.AddPerk(new string[] { "AP + 1" }, "Рожденный огнём", "Ваша магическая сила разгорается.", '~', ConsoleColor.Red);
                            break;
                        }
                    case MechEngine.BattleClass.Inquisitor:
                        {
                            MechEngine.Perk.AddPerk(new string[] { "AD + 2", "AP + 2" }, "Ревнивец", "Вы - живая кара богов.", '‡', ConsoleColor.DarkYellow);
                            break;
                        }
                    case MechEngine.BattleClass.Monk:
                        {
                            MechEngine.Perk.AddPerk(new string[] { "ARM + 2", "MRS + 2" }, "Руки защитника", "Вы знакомы с исскуством защиты.", 'W', ConsoleColor.Yellow);
                            break;
                        }
                    case MechEngine.BattleClass.Necromant:
                        {
                            MechEngine.Perk.AddPerk(new string[] { "AP + 1", "MRS + 1" }, "Тёмная энергия", "Некромантия защищает вас.", '╦', ConsoleColor.Green);
                            break;
                        }
                    case MechEngine.BattleClass.Paladin:
                        {
                            MechEngine.Perk.AddPerk(new string[] { "MP + 2", "ARM + 1", "MRS + 1" }, "Светлый", "Благославлен богом защиты.", '⌂', ConsoleColor.Yellow);
                            break;
                        }
                    case MechEngine.BattleClass.Shaman:
                        {
                            MechEngine.Perk.AddPerk(new string[] { "MP + 1", "HP + 1", "ARM + 1", "MRS + 1", "AP + 1", "AD + 1" }, "Духи предки", "Они поддерживают вас во всём.", '♥', ConsoleColor.Cyan);
                            break;
                        }
                }

                #endregion

            }
            if (Object == 3)
            {
                #region Proffession

                switch (Proffession)
                {
                    case MechEngine.CraftClass.Miner:
                        {
                            MechEngine.Perk.AddPerk(new string[] { "MP+2", "AP+2" }, "Кровавый", "Магия крови - запрещенная магия.", '°', ConsoleColor.DarkRed);
                            break;
                        }
                    case MechEngine.CraftClass.Herbalist:
                        {
                            MechEngine.Perk.AddPerk(new string[] { "MP+2", "AP+2" }, "Кровавый", "Магия крови - запрещенная магия.", '°', ConsoleColor.DarkRed);
                            break;
                        }
                    case MechEngine.CraftClass.Blacksmith:
                        {
                            MechEngine.Perk.AddPerk(new string[] { "MP+2", "AP+2" }, "Кровавый", "Магия крови - запрещенная магия.", '°', ConsoleColor.DarkRed);
                            break;
                        }
                    case MechEngine.CraftClass.Alchemist:
                        {
                            MechEngine.Perk.AddPerk(new string[] { "MP+2", "AP+2" }, "Кровавый", "Магия крови - запрещенная магия.", '°', ConsoleColor.DarkRed);
                            break;
                        }
                }

                #endregion
            }
        }

        public Equipment Equipment;
        /// <summary>
        /// Array of character abilityes
        /// </summary>
        public List<Ability> Ability;
        /// <summary>
        /// List of buffs character
        /// </summary>
        public List<Ability> Buffs;
        /// <summary>
        /// List of debuffs character
        /// </summary>
        public List<MonsterAbility> DeBuffs = new List<MonsterAbility>();
        /// <summary>
        /// Array of levels Craft ability
        /// </summary>
        public List<Ability> CraftAbility;
        /// <summary>
        /// Full = false, empty = true
        /// </summary>
        public bool InventorySlots
        {
            get
            {
                bool yes = false;
                int items = 0;
                foreach (MechEngine.Item i in Rogue.RAM.Player.Inventory)
                {
                    if (i != null)
                    {
                        items++;
                    }
                }
                if (items < 6)
                {
                    yes = true;
                }

                return yes;
            }
        }

        public List<Perk> Perks;

        public List<Quest> QuestBook;

        public List<Reputation> Repute = new List<Reputation>();

        public AbilityRate MonkSpec;

        public void CheckingUp()
        {
            if (Rogue.RAM.Player != null)
            {
                if (Rogue.RAM.Player.EXP != 0)
                {
                    if (Rogue.RAM.Player.EXP >= Rogue.RAM.Player.mEXP)
                    {
                        if (PlayEngine.EnemyMoves.CheckMove) { PlayEngine.EnemyMoves.Move(false); }
                        DrawEngine.InfoWindow.Custom("Вы повышаете уровень!");
                        Rogue.RAM.Player.Level++;
                        Rogue.RAM.Player.mEXP = Rogue.RAM.Player.UpExp;
                        int hP = 0;
                        int mP = 0;
                        int mD = 0;
                        int mU = 0;

                        #region Stats
                        switch (Rogue.RAM.Player.Class)
                        {
                            case BattleClass.BloodMage:
                                {
                                    hP = 1;
                                    mD = 0;
                                    mU = 1;
                                    if (Rogue.RAM.Player.ReservedManaPoints == Math.Round(Rogue.RAM.Player.ReservedManaPoints))
                                    {
                                        mP = Convert.ToInt32(Rogue.RAM.Player.ReservedManaPoints);
                                        Rogue.RAM.Player.ReservedManaPoints += 0.1;
                                    }
                                    else
                                    {
                                        Rogue.RAM.Player.ReservedManaPoints += 0.2;
                                    }
                                    break;
                                }
                            case BattleClass.FireMage:
                                {
                                    hP = 1;
                                    mD = 0;
                                    mU = 1;
                                    mP = 3;
                                    break;
                                }
                            case BattleClass.Monk:
                                {
                                    hP = 2;
                                    mD = 1;
                                    mU = 2;
                                    mP = 0;
                                    break;
                                }
                            case BattleClass.Shaman:
                                {
                                    hP = 1;
                                    mD = 0;
                                    mU = 2;
                                    mP = 3;
                                    break;
                                }
                            case BattleClass.Paladin:
                                {
                                    hP = 2;
                                    mD = 1;
                                    mU = 2;
                                    mP = 8;
                                    break;
                                }
                            case BattleClass.Inquisitor:
                                {
                                    hP = 2;
                                    mD = 1;
                                    mU = 3;
                                    if (Rogue.RAM.Player.ReservedManaPoints == Math.Round(Rogue.RAM.Player.ReservedManaPoints))
                                    {
                                        mP = Convert.ToInt32(Rogue.RAM.Player.ReservedManaPoints);
                                        Rogue.RAM.Player.ReservedManaPoints += 0.1;
                                    }
                                    else
                                    {
                                        Rogue.RAM.Player.ReservedManaPoints += 0.1;
                                    }
                                    break;
                                }
                            case BattleClass.Necromant:
                                {
                                    hP = 1;
                                    mD = 0;
                                    mU = 1;
                                    if (Rogue.RAM.Player.ReservedManaPoints == Math.Round(Rogue.RAM.Player.ReservedManaPoints))
                                    {
                                        mP = Convert.ToInt32(Rogue.RAM.Player.ReservedManaPoints);
                                        Rogue.RAM.Player.ReservedManaPoints += 0.5;
                                    }
                                    else
                                    {
                                        Rogue.RAM.Player.ReservedManaPoints += 0.5;
                                    }
                                    break;
                                }
                            case BattleClass.Assassin:
                                {
                                    hP = 2;
                                    mD = 1;
                                    mU = 2;
                                    Random PoisonBottle = new Random();
                                    if (PoisonBottle.Next(2) == 0) { mP = 1; };
                                    break;
                                }
                            case BattleClass.Warrior:
                                {
                                    hP = 3;
                                    mD = 1;
                                    mU = 3;
                                    mP = (Rogue.RAM.Player.CMP * -1);
                                    break;
                                }
                            case BattleClass.Alchemist:
                                {
                                    hP = 1;
                                    mD = 0;
                                    mU = 1;
                                    if (Rogue.RAM.Player.ReservedManaPoints == Math.Round(Rogue.RAM.Player.ReservedManaPoints))
                                    {
                                        mP = Convert.ToInt32(Rogue.RAM.Player.ReservedManaPoints);
                                        Rogue.RAM.Player.ReservedManaPoints += 0.5;
                                    }
                                    else
                                    {
                                        Rogue.RAM.Player.ReservedManaPoints += 0.5;
                                    }
                                    break;
                                }
                        }
                        #endregion

                        Rogue.RAM.Player.Gold += 10 * Rogue.RAM.Player.Level;
                        Rogue.RAM.Player.CHP += hP;
                        Rogue.RAM.Player.MHP += hP;
                        //Rogue.RAM.Player.Level += 1;
                        Rogue.RAM.Player.MIDMG += mD;
                        Rogue.RAM.Player.MADMG += mU;
                        Rogue.RAM.Player.CMP += mP;
                        Rogue.RAM.Player.MMP += mP;
                        Rogue.RAM.Player.AbPoint += 1;
                        Rogue.RAM.Player.CrPoint += 1;

                        Rogue.RAM.Player.CHP = Rogue.RAM.Player.MHP;
                        Rogue.RAM.Player.CMP = Rogue.RAM.Player.MMP;

                        SoundEngine.Sound.LevelUp();
                        DrawEngine.CharacterDraw.DrawLevelUp(10 * (Rogue.RAM.Player.Level - 1), hP, mP, mD, mU);
                    }
                    if (Rogue.RAM.Map != null)
                    {
                        if (!PlayEngine.EnemyMoves.CheckMove) { PlayEngine.EnemyMoves.Move(true); }
                    }
                }
            }
        }

        public void AddPerk(Perk Prk)
        {
            Rogue.RAM.Player.MHP += Prk.HP;
            Rogue.RAM.Player.CHP += Prk.HP;
            Rogue.RAM.Player.MMP += Prk.MP;
            Rogue.RAM.Player.CMP += Prk.MP;
            Rogue.RAM.Player.AD += Prk.AD;
            Rogue.RAM.Player.AP += Prk.AP;
            Rogue.RAM.Player.ARM += Prk.ARM;
            Rogue.RAM.Player.MRS += Prk.MRS;
            Rogue.RAM.Player.MIDMG += Prk.MIDMG;
            Rogue.RAM.Player.MADMG += Prk.MADMG;
            if (Rogue.RAM.Player.MHP < 0) { Rogue.RAM.Player.MHP = 1; }
            if (Rogue.RAM.Player.CHP < 0) { Rogue.RAM.Player.CHP = 1; }
            if (Rogue.RAM.Player.MMP < 0) { Rogue.RAM.Player.MMP = 0; }
            if (Rogue.RAM.Player.CMP < 0) { Rogue.RAM.Player.CMP = 0; }
            if (Rogue.RAM.Player.MADMG < 0) { Rogue.RAM.Player.MADMG = 0; }
            if (Rogue.RAM.Player.MIDMG < 0) { Rogue.RAM.Player.MIDMG = 0; }
        }

        public void RecountPerks()
        {
            foreach (MechEngine.Perk Prk in Rogue.RAM.Player.Perks)
            {
                Rogue.RAM.Player.MHP += Prk.HP;
                Rogue.RAM.Player.CHP += Prk.HP;
                Rogue.RAM.Player.MMP += Prk.MP;
                Rogue.RAM.Player.CMP += Prk.MP;
                Rogue.RAM.Player.AD += Prk.AD;
                Rogue.RAM.Player.AP += Prk.AP;
                Rogue.RAM.Player.ARM += Prk.ARM;
                Rogue.RAM.Player.MRS += Prk.MRS;
                Rogue.RAM.Player.MIDMG += Prk.MIDMG;
                Rogue.RAM.Player.MADMG += Prk.MADMG;

                if (Rogue.RAM.Player.MHP < 0) { Rogue.RAM.Player.MHP = 1; }
                if (Rogue.RAM.Player.CHP < 0) { Rogue.RAM.Player.CHP = 1; }
                if (Rogue.RAM.Player.MMP < 0) { Rogue.RAM.Player.MMP = 0; }
                if (Rogue.RAM.Player.CMP < 0) { Rogue.RAM.Player.CMP = 0; }
                if (Rogue.RAM.Player.MADMG < 0) { Rogue.RAM.Player.MADMG = 0; }
                if (Rogue.RAM.Player.MIDMG < 0) { Rogue.RAM.Player.MIDMG = 0; }
            }
        }
        /// <summary>
        /// Kill enemy
        /// </summary>
        /// <param name="WithoutCombat">If kill not in the combat and want exp</param>
        public void Kill(Monster WithoutCombat = null)
        {
            if (WithoutCombat == null)
            {
                Rogue.RAM.Log.Add(Rogue.RAM.Enemy.Name + " умирает!");
                this.QuestMonster = Rogue.RAM.Enemy.Name;
                int getExp = Rogue.RAM.Enemy.EXP;
                DrawEngine.FightDraw.DrawEnemyStat();
                DrawEngine.InfoWindow.Custom("Вы побеждаете в битве! Вы получаете " + getExp.ToString() + " EXP!");
                DrawEngine.FightDraw.ReDrawCombatLog();
                RemoteMonsterAbil();
                #region Reputation //Thanks for not bad code
                foreach (Reputation rep in Rogue.RAM.Player.Repute)
                {
                    if (rep.min < rep.max)
                    {
                        if (Rogue.RAM.Enemy.Race == rep.race)
                        {
                            if (Rogue.RAM.Map.Biom == rep.biom)
                            {
                                if ((rep.min + 10) > rep.max)
                                {
                                    rep.min = rep.max;
                                }
                                else
                                {
                                    rep.min += 10;
                                    DrawEngine.InfoWindow.Message = "Вы увеличиваете репутацию с фракцией " + rep.name + " на 10!";
                                }

                            }
                        }
                    }
                }
                #endregion
                Rogue.RAM.Enemy = null;
                Thread.Sleep(1000);
                if (!PlayEngine.EnemyMoves.CheckMove) { PlayEngine.EnemyMoves.Move(true); }

                #region ClassBonusAfterCombat
                if (Rogue.RAM.Player.Class == MechEngine.BattleClass.Inquisitor)
                {
                    Rogue.RAM.Player.CMP = Rogue.RAM.Player.MMP;
                    DrawEngine.GUIDraw.ReDrawCharStat();
                    DrawEngine.InfoWindow.Custom("Жажда битвы удовлетворена, сила кары восстановлена!");
                }
                if (Rogue.RAM.Player.Class == BattleClass.Warlock)
                {
                    foreach (Ability a in this.Buffs)
                    {
                        if (a.Name == "Жертва") { a.Duration += 10; DrawEngine.InfoWindow.cMessage = new List<DrawEngine.ColoredWord>() { new DrawEngine.ColoredWord() { Color = ConsoleColor.Magenta, Word = "Вы продлили эффект Жертвы за счет убийства!" } }; }
                    }
                }
                #endregion

                Rogue.RAM.Player.EXP += getExp;
                DrawEngine.GUIDraw.ReDrawCharStat();
                PlayEngine.GamePlay.Play();

            }
            else
            {
                int getExp = WithoutCombat.EXP;
                DrawEngine.InfoWindow.Custom(WithoutCombat.Name + " умирает! Вы получаете " + getExp.ToString() + " EXP!");
                #region Reputation
                foreach (Reputation rep in Rogue.RAM.Player.Repute)
                {
                    if (rep.min < rep.max)
                    {
                        if (WithoutCombat.Race == rep.race)
                        {
                            if (Rogue.RAM.Map.Biom == rep.biom)
                            {
                                if ((rep.min + 10) > rep.max)
                                {
                                    rep.min = rep.max;
                                }
                                else
                                {
                                    rep.min += 10;
                                    DrawEngine.InfoWindow.Message = "Вы увеличиваете репутацию с фракцией " + rep.name + " на 10!";
                                }

                            }
                        }
                    }
                }
                #endregion
                Thread.Sleep(1000);
                Rogue.RAM.Player.EXP += getExp;
                DrawEngine.GUIDraw.ReDrawCharStat();
                if (!PlayEngine.EnemyMoves.CheckMove) { PlayEngine.EnemyMoves.Move(true); }
            }

        }
        public void RemoteMonsterAbil()
        {
            foreach (MonsterAbility a in Rogue.RAM.Enemy.EoF)
            {
                a.DeActivate();
            }
            foreach (MonsterAbility a in Rogue.RAM.Player.DeBuffs)
            {
                a.DeActivate();
            }
            Rogue.RAM.Enemy.EoF = new List<MonsterAbility>();
            Rogue.RAM.Player.DeBuffs = new List<MonsterAbility>();
        }
        /// <summary>
        /// If you killed quest monster, add to progress
        /// </summary>
        public string QuestMonster
        { set { foreach (Quest Q in this.QuestBook) { foreach (Monster M in Q.M) { if (M.Name == value) { Q.Progress += 1; DrawEngine.InfoWindow.Message = "Прогресс в задании " + Q.Name + " !"; Q.M.Remove(M); break; } } } } }
        /// <summary>
        /// If you took quest item, add to progress
        /// </summary>
        public string QuestItem
        { set { foreach (Quest Q in this.QuestBook) { foreach (Item I in Q.I) { if (I.Name == value) { Q.Progress += 1; DrawEngine.InfoWindow.Message = "Прогресс в задании " + Q.Name + " !"; Q.I.Remove(I); break; } } } } }
        /// <summary>
        /// When u took gald, may be u finished quest?
        /// </summary>
        public int QuestGold
        { set { if (this.QuestBook != null) { foreach (Quest Q in this.QuestBook) { if (Q.G != 0) { Q.Progress += value; break; } } } } }
        /// <summary>
        /// CHeck item in player inventory
        /// </summary>
        /// <param name="it">Item</param>
        /// <returns>Index of item or -1</returns>
        public int CheckItem(Item it)
        { foreach (Item i in this.Inventory) { if (i.Name == it.Name) { return this.Inventory.IndexOf(i); } } return -1; }
    }
}
