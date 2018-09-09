using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace Rogue
{
    public static class MechEngine
    {
        private static Random r = new Random();
        public class Character
        {
            public Character()
            {
                this.Buffs = new List<Ability>();
                this.Inventory = new List<Item>();
                this.Equipment = new Equipment();
                this.Ability=new List<Ability>();
                this.CraftAbility=new List<Ability>();
                this.Perks = new List<Perk>();
                this.QuestBook = new List<Quest>();
            }

            public string Name;

            public BattleClass Class;

            public CraftClass Proffession;

            public char Icon;

            public ConsoleColor Color;

            public Race Race;

            public string ManaName
            {
                get
                {
                    switch (Class)
                    {
                        case BattleClass.BloodMage:
                            { return "Кровь: "; }
                        case BattleClass.Inquisitor:
                            { return "Сила кары: "; }
                        case BattleClass.Necromant:
                            { return "Души: "; }
                        case BattleClass.Assassin:
                            { return "Яды: "; }
                        case BattleClass.Alchemist:
                            { return "Элементы: "; }
                        case BattleClass.Monk:
                            { return "Сила духа: "; }
                        case BattleClass.Warrior:
                            { return "Ярость: "; }
                        default:
                            { return "Мана: "; }
                    }
                }
            }

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
            
            public int ARM, MRS,mEXP;

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
                                        { rtrn=_MIDMG +a.Power; }
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
                set { this.QuestGold = (value-_Gold); _Gold = value; }
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
            public List<MonsterAbility> DeBuffs=new List<MonsterAbility>();
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
                if (Rogue.RAM.Player!=null)
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
            public void Kill(Monster WithoutCombat=null)
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

        public class Morphling
        {
            /// <summary>
            /// Create new form
            /// </summary>
            /// <param name="Face">Face-char</param>
            /// <param name="Body">Body-color</param>
            public Morphling(Char Face, ConsoleColor Body)
            { this.Icon = Face; this.Color = Body; }
            /// <summary>
            /// Icon for morph
            /// </summary>
            public char Icon;
            /// <summary>
            /// Color for morph
            /// </summary>
            public ConsoleColor Color;
        }

        public enum BattleClass
        {
            BloodMage = 1,
            Paladin = 9,
            Inquisitor = 7,
            FireMage = 2,
            Assassin = 6,
            Shaman = 3,
            Necromant = 4,
            Monk = 8,
            Alchemist = 5,
            Warrior = 10,
            //Elite
            Warlock = 11,
            LightWarrior = 12,
            Illusionist = 13,
            Valkyrie =14
        }

        public enum CraftClass
        {
            Common = 0,
            Miner = 1,
            Herbalist = 2,
            Blacksmith = 10,
            Alchemist = 20
        }

        public enum Race
        {
            Human = 1,
            Elf = 2,
            Dwarf = 4,
            Gnome = 3,
            Orc = 10,
            DarkElf = 6,
            Undead = 9,
            Troll = 8,
            FallenAngel = 7,
            MoonElf = 5
        }

        public enum Rarity
        {
            /// <summary>
            /// Gray
            /// </summary>
            Poor = 1,
            /// <summary>
            /// White
            /// </summary>
            Common = 2,
            /// <summary>
            /// Blue
            /// </summary>
            Uncommon = 4,
            /// <summary>
            /// Yellow
            /// </summary>
            Rare = 7,
            /// <summary>
            /// Green
            /// </summary>
            Set = 9,
            /// <summary>
            /// DarkMagenta
            /// </summary>
            Epic = 10,
            /// <summary>
            /// Cyan
            /// </summary>
            Legendary = 12,
            /// <summary>
            /// DarkYellow
            /// </summary>
            Artefact = 15,
            /// <summary>
            /// Red
            /// </summary>
            Fired = 20,
            /// <summary>
            /// Blue again
            /// </summary>
            Watered=21
        }

        public enum Kind
        {
            Key = 0,
            Potion = 1,
            Elixir = 2,
            Weapon = 3,
            Helm = 4,
            Armor = 5,
            Boots = 6,
            OffHand = 7,
            Scroll = 8,
            Poison = 9,
            Resource = 10,
            Rune=11
        }
        /// <summary>
        /// Wierd struct
        /// </summary>
        public class Perk
        {
            public int HP, MP, MIDMG, MADMG, AD, AP, ARM, MRS;

            public string Name;
            /// <summary>
            /// new or old type
            /// </summary>
            public bool Type = true;

            public string History;

            public char Icon;

            public ConsoleColor Color;
            /// <summary>
            /// Old Perk adds
            /// </summary>
            /// <param name="Stats">string array like: [AD - 5][ARM + 4] etc</param>
            /// <param name="Name">Name of perk</param>
            /// <param name="History">History of perk</param>
            /// <param name="Icon">Icon of perk</param>
            /// <param name="Color">Color of perk</param>
            public static void AddPerk(string[] Stats, string Name, string History, char Icon, ConsoleColor Color)
            {
                MechEngine.Perk P = new MechEngine.Perk();
                P.Name = Name;
                P.History = History;
                P.Icon = Icon;
                P.Color = Color;
                string[] one = new string[2];
                foreach (string s in Stats)
                {
                    one = s.Split(' ');
                    int znak = 1;
                    if (one[1] == "-")
                    {
                        znak = -1;
                    }
                    else
                    {
                        znak = 1;
                    }
                    if (one[0] == "HP")
                    {
                        P.HP += Convert.ToInt32(one[2])*znak;
                    }
                    else if (one[0] == "MP")
                    {
                        P.MP += Convert.ToInt32(one[2]) * znak;
                    }
                    else if (one[0] == "AP")
                    {
                        P.AP += Convert.ToInt32(one[2])*znak;
                    }
                    else if (one[0] == "AD")
                    {
                        P.AD += Convert.ToInt32(one[2])*znak;
                    }
                    else if (one[0] == "ARM")
                    {
                        P.ARM += Convert.ToInt32(one[2]) * znak;
                    }
                    else if (one[0] == "MADMG")
                    {
                        P.MADMG += Convert.ToInt32(one[2]) * znak;
                    }
                    else if (one[0] == "MIDMG")
                    {
                        P.MIDMG += Convert.ToInt32(one[2]) * znak;
                    }
                    else if (one[0] == "MRS")
                    {
                        P.MRS += Convert.ToInt32(one[2]) * znak;
                    }
                }
                Rogue.RAM.Player.Perks.Add(P);
            }

            public List<PerkStat> Bonus;

            public List<PerkStat> Penalty;
            /// <summary>
            /// New Perk adds
            /// </summary>
            /// <param name="Bonuses">list of PerkStat for plus</param>
            /// <param name="Penaltyes">list of PerkStat for minus</param>
            /// <param name="Name">Name of perk</param>
            /// <param name="History">History of perk</param>
            /// <param name="Icon">Icon of perk</param>
            /// <param name="Color">Color of perk</param>
            public static void AddPerk(List<PerkStat> Bonuses, List<PerkStat> Penaltyes, string Name, string History, char Icon, ConsoleColor Color)
            {
                MechEngine.Perk P = new MechEngine.Perk();
                P.Name = Name;
                P.History = History;
                P.Icon = Icon;
                P.Color = Color;
                if (Bonuses != null)
                {
                    foreach (PerkStat st in Bonuses)
                    {
                        switch (st.Stat)
                        {
                            case AbilityStats.AD: { P.AD += st.Value; break; }
                            case AbilityStats.AP: { P.AP += st.Value; break; }
                            case AbilityStats.ARM: { P.ARM += st.Value; break; }
                            case AbilityStats.MRS: { P.MRS += st.Value; break; }
                            case AbilityStats.MHP: { P.HP += st.Value; break; }
                            case AbilityStats.MMP: { P.MP += st.Value; break; }
                            case AbilityStats.DMG: { P.MIDMG += st.Value; P.MADMG += st.Value; break; }
                        }
                    }
                }
                if (Penaltyes != null)
                {
                    foreach (PerkStat st in Penaltyes)
                    {
                        switch (st.Stat)
                        {
                            case AbilityStats.AD: { P.AD -= st.Value; break; }
                            case AbilityStats.AP: { P.AP -= st.Value; break; }
                            case AbilityStats.ARM: { P.ARM -= st.Value; break; }
                            case AbilityStats.MRS: { P.MRS -= st.Value; break; }
                            case AbilityStats.MHP: { P.HP -= st.Value; break; }
                            case AbilityStats.MMP: { P.MP -= st.Value; break; }
                            case AbilityStats.DMG: { P.MIDMG -= st.Value; P.MADMG -= st.Value; break; }
                        }
                    }
                }
                Rogue.RAM.Player.Perks.Add(P);
                Rogue.RAM.Player.AddPerk(P);
            }
        }

        public class PerkStat
        {
            public PerkStat() { Value = 0; Stat = AbilityStats.MHP; }
            public int Value; public AbilityStats Stat;
        }
        
        public class Item
        {
            public class Potion : Item
            {
                public Potion() { }

                public Potion(Item i)
                {
                    this.Kind = i.Kind;
                    this.Name = i.Name;
                    this.ILvl = i.ILvl;
                    this.Rare = i.Rare;
                    this.HP = 0;
                    this.MP = 0;
                }

                public int HP, MP;

                public int WasCHP, WasCMP;

                public string
                    GetEffect()
                {
                    string stats = string.Empty;                    
                    if (HP != 0)
                    {
                        int healed = WasCHP + this.HP;
                        if (healed > Rogue.RAM.Player.MHP)
                        { healed = Rogue.RAM.Player.MHP - WasCHP; }
                        else { healed = this.HP; }
                        stats += " " + healed.ToString();
                    }            
                    if (MP != 0)
                    {
                        int healed = WasCMP + this.MP;
                        if (healed > Rogue.RAM.Player.MMP)
                        { healed = Rogue.RAM.Player.MMP - WasCMP; }
                        else { healed = this.HP; }
                        stats += " " + healed.ToString();
                    }
                    if (stats == string.Empty)
                    {
                        stats += " Нет";
                    }
                    return stats;
                }

                public override char Icon()
                {
                    return '*';
                }

                public override int Sell
                {
                    get
                    {
                        int rtrn = Rogue.RAM.Player.Level * 5;
                        if (this.HP != 0)
                        { if (Rogue.RAM.Player.CHP < Rogue.RAM.Player.MHP) { int r = Rogue.RAM.Player.MHP - Rogue.RAM.Player.CHP; rtrn += r * 2; } }
                        if (this.MP != 0)
                        { if (Rogue.RAM.Player.CMP < Rogue.RAM.Player.MMP) { int r = Rogue.RAM.Player.MMP - Rogue.RAM.Player.CMP; rtrn += r * 2; } }
                        return rtrn + this.SpentS;
                    }
                }
            }

            public class Elixir : Item
            {
                public int AD, AP, ARM, MRS;

                public DateTime Duration;

                public override char Icon()
                {
                    return '£';
                }
            }

            public class Weapon : Item
            {
                public Weapon() { }

                public Weapon(Item i)
                {
                    this.Kind = i.Kind;
                    this.Name = i.Name;
                    this.ILvl = i.ILvl;
                    this.Rare = i.Rare;
                    this.MIDMG = 0;
                    this.MADMG = 0;
                    this.AD = 0;
                    this.AP = 0;
                    this.ARM = 0;
                    this.Staff = false;
                }

                public int MIDMG, MADMG, AD, AP, ARM;

                public bool Staff=false;

                public override char Icon()
                {
                    return '{';
                }

                public override void Dress()
                {
                    Character Stats = Rogue.RAM.Player;
                    Stats.MIDMG += MIDMG;
                    Stats.MADMG += MADMG;
                    Stats.AD += AD;
                    Stats.AP += AP;
                    Stats.ARM += ARM;
                    Rogue.RAM.Player = Stats;
                    this.Script(true);
                }

                public override void UnDress()
                {
                    Character Stats = Rogue.RAM.Player;
                    Stats.MIDMG -= MIDMG;
                    Stats.MADMG -= MADMG;
                    Stats.AD -= AD;
                    Stats.AP -= AP;
                    Stats.ARM -= ARM;
                    Rogue.RAM.Player = Stats;
                    this.Script(false);
                }

                public override int Sell
                {
                    get
                    {
                        int rtrn = 0;
                        if (Rogue.RAM.Player.Class == BattleClass.Monk || Rogue.RAM.Player.Class == BattleClass.Paladin || Rogue.RAM.Player.Class == BattleClass.Necromant)
                        { if (this.ARM != 0) { rtrn += this.ARM * 12; } }
                        if (Rogue.RAM.Player.Class == BattleClass.Inquisitor || Rogue.RAM.Player.Class == BattleClass.Assassin)
                        { if (this.AD != 0) { rtrn += this.AD * 4; } }
                        if (Rogue.RAM.Player.Class == BattleClass.BloodMage || Rogue.RAM.Player.Class == BattleClass.FireMage || Rogue.RAM.Player.Class == BattleClass.Shaman)
                        { if (this.AP != 0) { rtrn += this.AP * 15; } }
                        rtrn += MIDMG * 10 + MADMG * 10;
                        rtrn += Rogue.RAM.Map.Level * 17;
                        return rtrn + this.SpentS;
                    }
                }
            }

            public class Helm : Item
            {
                public Helm() { }

                public Helm(Item i)
                {
                    this.Kind = i.Kind;
                    this.Name = i.Name;
                    this.ILvl = i.ILvl;
                    this.Rare = i.Rare;
                    this.AD = 0;
                    this.AP = 0;
                    this.HP = 0;
                    this.MP = 0;
                }

                public int HP, MP, AP, AD;

                public override char Icon()
                {
                    return '▲';
                }

                public override void Dress()
                {
                    Character Stats = Rogue.RAM.Player;
                    Stats.CHP += HP;
                    Stats.MHP += HP;

                    if (MP > 0)
                    {
                        if (Rogue.RAM.Player.Class == BattleClass.Assassin || Rogue.RAM.Player.Class == BattleClass.Alchemist || Rogue.RAM.Player.Class == BattleClass.Inquisitor)
                        { Stats.CMP += 1; Stats.MMP += 1; }
                        if (Rogue.RAM.Player.Class == BattleClass.BloodMage)
                        { Stats.CHP += MP; Stats.CMP += MP; }
                        if (Rogue.RAM.Player.Class != BattleClass.Alchemist && Rogue.RAM.Player.Class != BattleClass.Assassin && Rogue.RAM.Player.Class != BattleClass.BloodMage && Rogue.RAM.Player.Class != BattleClass.Inquisitor && Rogue.RAM.Player.Class != BattleClass.Necromant)
                        { Stats.CMP += MP; Stats.MMP += MP; }
                    }

                    Stats.AD += AD;
                    Stats.AP += AP;
                    Rogue.RAM.Player = Stats;
                    this.Script(true);
                }

                public override void UnDress()
                {
                    Character Stats = Rogue.RAM.Player;
                    Stats.CHP -= HP;
                    Stats.MHP -= HP;

                    if (MP > 0)
                    {
                        if (Rogue.RAM.Player.Class == BattleClass.Assassin || Rogue.RAM.Player.Class == BattleClass.Alchemist || Rogue.RAM.Player.Class == BattleClass.Inquisitor)
                        { Stats.CMP -= 1; Stats.MMP -= 1; }
                        if (Rogue.RAM.Player.Class == BattleClass.BloodMage)
                        { Stats.CHP -= MP; Stats.CMP -= MP; }
                        if (Rogue.RAM.Player.Class != BattleClass.Alchemist && Rogue.RAM.Player.Class != BattleClass.Assassin && Rogue.RAM.Player.Class != BattleClass.BloodMage && Rogue.RAM.Player.Class != BattleClass.Inquisitor && Rogue.RAM.Player.Class != BattleClass.Necromant)
                        { Stats.CMP -= MP; Stats.MMP -= MP; }
                    }

                    Stats.AD -= AD;
                    Stats.AP -= AP;
                    Rogue.RAM.Player = Stats;
                    this.Script(false);
                }

                public override int Sell
                {
                    get
                    {
                        int rtrn = 0;
                        if (Rogue.RAM.Player.Class == BattleClass.Inquisitor || Rogue.RAM.Player.Class == BattleClass.Assassin)
                        { if (this.AD != 0) { rtrn += this.AD * 4; } }
                        if (Rogue.RAM.Player.Class == BattleClass.BloodMage || Rogue.RAM.Player.Class == BattleClass.FireMage || Rogue.RAM.Player.Class == BattleClass.Shaman)
                        { if (this.AP != 0) { rtrn += this.AP * 15; } }
                        rtrn += (this.HP * 8) + (this.MP * 8) + 10;
                        rtrn += Rogue.RAM.Map.Level * 17;
                        return rtrn + this.SpentS;
                    }
                }
            }

            public class Armor : Item
            {
                public Armor() { }

                public Armor(Item i)
                {
                    this.Kind = i.Kind;
                    this.Name = i.Name;
                    this.ILvl = i.ILvl;
                    this.Rare = i.Rare;
                    this.HP = 0;
                    this.MP = 0;
                    this.ARM = 0;
                    this.MRS = 0;
                    this._Col = i._Col;
                    this.Color = i.Color;
                }

                public int HP, MP, ARM, MRS;

                public override char Icon()
                {
                    return '♦';
                }

                public override void Dress()
                {
                    Character Stats = Rogue.RAM.Player;
                    Stats.CHP += HP;
                    Stats.MHP += HP;

                    if (MP > 0)
                    {
                        if (Rogue.RAM.Player.Class == BattleClass.Assassin || Rogue.RAM.Player.Class == BattleClass.Alchemist || Rogue.RAM.Player.Class == BattleClass.Inquisitor)
                        { Stats.CMP += 1; Stats.MMP += 1; }
                        else if (Rogue.RAM.Player.Class == BattleClass.BloodMage)
                        { Stats.CHP += MP; Stats.CMP += MP; }
                        else if (Rogue.RAM.Player.Class != BattleClass.Alchemist && Rogue.RAM.Player.Class != BattleClass.Assassin && Rogue.RAM.Player.Class != BattleClass.BloodMage && Rogue.RAM.Player.Class != BattleClass.Inquisitor &&Rogue.RAM.Player.Class!= BattleClass.Necromant)
                        { Stats.CMP += MP; Stats.MMP += MP; }                        
                    }

                    Stats.ARM += ARM;
                    Stats.MRS += MRS;
                    Rogue.RAM.Player = Stats;
                    this.Script(true);
                }

                public override void UnDress()
                {
                    Character Stats = Rogue.RAM.Player;
                    Stats.CHP -= HP;
                    Stats.MHP -= HP;

                    if (MP > 0)
                    {
                        if (Rogue.RAM.Player.Class == BattleClass.Assassin || Rogue.RAM.Player.Class == BattleClass.Alchemist || Rogue.RAM.Player.Class == BattleClass.Inquisitor)
                        { Stats.CMP -= 1; Stats.MMP -= 1; }
                        if (Rogue.RAM.Player.Class == BattleClass.BloodMage)
                        { Stats.CHP -= MP; Stats.CMP -= MP; }
                        if (Rogue.RAM.Player.Class != BattleClass.Alchemist && Rogue.RAM.Player.Class != BattleClass.Assassin && Rogue.RAM.Player.Class != BattleClass.BloodMage && Rogue.RAM.Player.Class != BattleClass.Inquisitor && Rogue.RAM.Player.Class != BattleClass.Necromant)
                        { Stats.CMP -= MP; Stats.MMP -= MP; }
                    }

                    Stats.ARM -= ARM;
                    Stats.MRS -= MRS;
                    Rogue.RAM.Player = Stats;
                    this.Script(false);
                }

                public override int Sell
                {
                    get
                    {
                        int rtrn = 0;
                        if (Rogue.RAM.Player.Class == BattleClass.Monk || Rogue.RAM.Player.Class == BattleClass.Paladin || Rogue.RAM.Player.Class == BattleClass.Necromant)
                        { if (this.ARM != 0 || this.MRS != 0) { rtrn += (this.ARM * 12) + (this.MRS * 20); } }
                        rtrn += (this.HP * 12) + (this.MP * 3);
                        rtrn += Rogue.RAM.Map.Level * 17;
                        return rtrn + this.SpentS;
                    }
                }
            }

            public class Boots : Item
            {
                public Boots() { }

                public Boots(Item i)
                {
                    this.Kind = i.Kind;
                    this.Name = i.Name;
                    this.ILvl = i.ILvl;
                    this.Rare = i.Rare;
                    this.ARM = 0;
                    this.MRS = 0;
                    this._Col = i._Col;
                    this.Color = i.Color;
                }

                public int ARM, MRS;

                public override char Icon()
                {
                    return '▼';
                }

                public override void Dress()
                {
                    Character Stats = Rogue.RAM.Player;
                    Stats.ARM += ARM;
                    Stats.MRS += MRS;
                    Rogue.RAM.Player = Stats;
                    this.Script(true);
                }

                public override void UnDress()
                {
                    Character Stats = Rogue.RAM.Player;
                    Stats.ARM -= ARM;
                    Stats.MRS -= MRS;
                    Rogue.RAM.Player = Stats;
                    this.Script(false);
                }

                public override int Sell
                {
                    get
                    {
                        int rtrn = 0;
                        if (Rogue.RAM.Player.Class == BattleClass.Monk || Rogue.RAM.Player.Class == BattleClass.Paladin || Rogue.RAM.Player.Class == BattleClass.Necromant)
                        { if (this.ARM != 0 || this.MRS != 0) { rtrn += (this.ARM * 12) + (this.MRS * 20); } }
                        rtrn += new Random().Next(99);
                        return rtrn + this.SpentS;
                    }
                }
            }

            public class OffHand : Item
            {
                public OffHand() { }

                public OffHand(Item i)
                {
                    this.Kind = i.Kind;
                    this.Name = i.Name;
                    this.ILvl = i.ILvl;
                    this.Rare = i.Rare;
                    this.ARM = 0;
                    this.MRS = 0;
                    this.ARM = 0;
                    this.AD = 0;
                    this.AP = 0;
                    this.MIDMG = 0;
                    this.MADMG = 0;
                    this._Col = i._Col;
                    this.Color = i.Color;
                }

                public int AD, AP, MIDMG, MADMG, ARM, MRS;

                public override char Icon()
                {
                    return '[';
                }

                public override void Dress()
                {
                    Character Stats = Rogue.RAM.Player;
                    Stats.MIDMG += MIDMG;
                    Stats.MADMG += MADMG;
                    Stats.AD += AD;
                    Stats.AP += AP;
                    Stats.ARM += ARM;
                    Stats.MRS += MRS;
                    Rogue.RAM.Player = Stats;
                }

                public override void UnDress()
                {
                    Character Stats = Rogue.RAM.Player;
                    Stats.MIDMG -= MIDMG;
                    Stats.MADMG -= MADMG;
                    Stats.AD -= AD;
                    Stats.AP -= AP;
                    Stats.ARM -= ARM;
                    Stats.MRS -= MRS;
                    Rogue.RAM.Player = Stats;
                }

                public override int Sell
                {
                    get
                    {
                        int rtrn = 0;
                        if (Rogue.RAM.Player.Class == BattleClass.Monk || Rogue.RAM.Player.Class == BattleClass.Paladin || Rogue.RAM.Player.Class == BattleClass.Necromant)
                        { if (this.ARM != 0 || this.MRS != 0) { rtrn += (this.ARM * 12) + (this.MRS * 20); } }
                        if (Rogue.RAM.Player.Class == BattleClass.Inquisitor || Rogue.RAM.Player.Class == BattleClass.Assassin)
                        { if (this.AD != 0) { rtrn += this.AD * 4; } }
                        if (Rogue.RAM.Player.Class == BattleClass.BloodMage || Rogue.RAM.Player.Class == BattleClass.FireMage || Rogue.RAM.Player.Class == BattleClass.Shaman)
                        { if (this.AP != 0) { rtrn += this.AP * 15; } }
                        rtrn += MIDMG * 10 + MADMG * 10;
                        rtrn += Rogue.RAM.Map.Level * 17;
                        return rtrn + this.SpentS;
                    }
                }
            }

            public class Scroll : Item
            {
                public Ability Spell;

                public Scroll() { }

                public Scroll(Item i)
                {
                    this.Kind = i.Kind;
                    this.Name = i.Name;
                    this.ILvl = i.ILvl;
                    this.Rare = i.Rare;
                    this._Col = i._Col;
                    this.Color = i.Color;
                }

                public override int Sell
                {
                    get
                    {
                        if (this.Kind == MechEngine.Kind.Rune) { return r.Next(Rogue.RAM.Player.Level, (Rogue.RAM.Map.Level + 10)); }
                        else { return base.Sell * this.Spell.Level; }
                    }
                }

                public override char Icon()
                {
                    return '■';
                }
            }

            public class Key : Item
            {
                public Key() { }

                public Key(Item i)
                {
                    this.Kind = i.Kind;
                    this.Name = i.Name;
                    this.ILvl = i.ILvl;
                    this.Rare = i.Rare;
                    this._Col = i._Col;
                    this.Color = i.Color;
                }

                public override char Icon()
                {
                    return '§';
                }
            }

            public class Resource : Item
            {
                public char _Icon;
                public override char Icon()
                {
                    return _Icon;
                }
                public override ConsoleColor Color
                {
                    get
                    {
                        return _Col;
                    }
                    set
                    {
                        _Col = value;
                    }
                }
                public int SetSell;
                public override int Sell
                { get { return SetSell != 0 ? SetSell : base.Sell; } }
            }

            public class TownPortal : Item
            {
                public TownPortal()
                {
                    this.Kind = MechEngine.Kind.Rune;
                }

                public override char Icon()
                {
                    return '⌂';
                }

                public override ConsoleColor Color
                {
                    get
                    {
                        return  ConsoleColor.Blue;
                    }
                    set
                    {
                        base.Color = value;
                    }
                }

                public override int Sell
                {
                    get
                    {
                        return Rogue.RAM.Player.Level * 2000;
                    }
                }                

                public override bool Use()
                {
                    if (Rogue.RAM.Enemy == null)
                    {
                        PlayEngine.EnemyMoves.Move(false);
                        Rogue.RAM.InPortal = Rogue.RAM.Map;
                        LabirinthEngine.Create(1, true);
                        PlayEngine.EnemyMoves.Move(true);
                        SoundEngine.Music.TownTheme();
                        PlayEngine.GamePlay.Play();
                        return true;
                    }
                    else { return false; }
                }

                public class TownScroll : TownPortal
                {
                    public TownScroll()
                    {
                        this.Kind = MechEngine.Kind.Scroll;
                    }

                    public override bool Use()
                    {
                        if (Rogue.RAM.Enemy == null)
                        {                            
                            Rogue.RAM.Player.Inventory.Remove(this);
                            PlayEngine.EnemyMoves.Move(false);
                            Rogue.RAM.InPortal = Rogue.RAM.Map;
                            LabirinthEngine.Create(1, true);
                            PlayEngine.EnemyMoves.Move(true);
                            SoundEngine.Music.TownTheme();
                            PlayEngine.GamePlay.Play();
                            return true;
                        }
                        else { return false; }
                    }

                    public override int Sell
                    {
                        get
                        {
                            return Rogue.RAM.Player.Level * 250;
                        }
                    }
                }
            }

            public class Poison : Item
            {
                public Poison()
                {
                    Random rP = new Random();
                    switch (rP.Next(4))
                    {
                        case 0: { _Icon = '*'; break; }
                        case 1: { _Icon = '+'; break; }
                        case 2: { _Icon = '#'; break; }
                        case 3: { _Icon = '^'; break; }
                        default: { _Icon = '.'; break; }
                    }
                    this.Color = ConsoleColor.Green;
                    this.Kind = Kind.Poison;
                }

                private char _Icon;

                public override char Icon()
                {
                    return _Icon;
                }

                public override void TakePoison()
                {
                    switch (this.Icon())
                    {
                        case '*': { Rogue.RAM.Player.CMP += 1; DrawEngine.InfoWindow.Custom("Вы изготовили 1 яд!"); break; }
                        case '+': { Rogue.RAM.Player.CMP += 1; DrawEngine.InfoWindow.Custom("Вы изготовили 1 яд!"); break; }
                        case '#': { Rogue.RAM.Player.CMP += 2; DrawEngine.InfoWindow.Custom("Вы изготовили 2 яда!"); break; }
                        case '^': { Rogue.RAM.Player.CMP += 2; DrawEngine.InfoWindow.Custom("Вы изготовили 2 яда!"); break; }
                        default: { break; }
                    }
                    DrawEngine.GUIDraw.ReDrawCharStat();
                    if (Rogue.RAM.Player.CMP > Rogue.RAM.Player.MMP)
                    { Thread.Sleep(500); DrawEngine.InfoWindow.Custom("Вы несёте с собой слишком много бутылочек с ядом, ваши движения замедленны!"); }
                }
            }
            /// <summary>
            /// Name of item
            /// </summary>
            public string Name;
            /// <summary>
            /// Ilvl, for eqip
            /// </summary>
            public int ILvl;          
            /// <summary>
            /// Rarity - power and color of item
            /// </summary>
            public Rarity Rare;
            /// <summary>
            /// Kind for equip
            /// </summary>
            public Kind Kind;
            /// <summary>
            /// For Set-Items and unique
            /// </summary>
            public iScript Script = (bool Dress) => { };
            /// <summary>
            /// For know what is the set
            /// </summary>
            public string ArmorSet = string.Empty;
            public int BufStat = 0;
            public int BufLvl = 0;
            /// <summary>
            /// private color
            /// </summary>
            private ConsoleColor _Col = ConsoleColor.DarkCyan;
            /// <summary>
            /// return color eqialent rarity, or _Col. Set _Col     
            /// </summary>
            public virtual ConsoleColor Color
            {
                set { _Col = value; }
                get
                {
                    ConsoleColor Color = ConsoleColor.Black;
                    switch (Rare)
                    {
                        case Rarity.Poor:
                            {
                                Color = ConsoleColor.Gray;
                                break;
                            }
                        case Rarity.Common:
                            {
                                Color = ConsoleColor.White;
                                break;
                            }
                        case Rarity.Uncommon:
                            {
                                Color = ConsoleColor.Blue;
                                break;
                            }
                        case Rarity.Rare:
                            {
                                Color = ConsoleColor.Yellow;
                                break;
                            }
                        case Rarity.Set:
                            {
                                Color = ConsoleColor.Green;
                                break;
                            }
                        case Rarity.Epic:
                            {
                                Color = ConsoleColor.DarkMagenta;
                                break;
                            }
                        case Rarity.Legendary:
                            {
                                Color = ConsoleColor.Cyan;
                                break;
                            }
                        case Rarity.Artefact:
                            {
                                Color = ConsoleColor.DarkYellow;
                                break;
                            }
                        case Rarity.Fired:
                            {
                                Color = ConsoleColor.Red;
                                break;
                            }
                        case Rarity.Watered:
                            {
                                Color = ConsoleColor.Blue;
                                break;
                            }
                    }
                    if (Color == ConsoleColor.Black)
                    { return _Col; }
                    return Color;
                }
            }            
            /// <summary>
            /// Return value in overrride method
            /// </summary>
            /// <returns></returns>
            public virtual char Icon()
            {                
                return ' ';
            }
            /// <summary>
            /// For ovverride
            /// </summary>
            public virtual void UnDress()
            {
                DrawEngine.InfoWindow.Custom("Этот предмет нельзя снять?! Как вы его надели?!");
            }
            /// <summary>
            /// For ovverride
            /// </summary>
            public virtual void Dress()
            {
                DrawEngine.InfoWindow.Custom("Этот предмет нельзя надеть!");
            }
            /// <summary>
            /// For ovverride
            /// </summary>
            public virtual void TakePoison()
            {
                DrawEngine.InfoWindow.Custom("К сожалению этот реагент не подходит для изготовления яда...");
            }
            /// <summary>
            /// Return gearscore
            /// </summary>
            public static int GetGearScore()
            {
                //Вес = (УровеньПредмета — КачествоПредмета) * МультипликаторКачества * МультипликаторВидаПредмета;
                int b = 0;
                int c = 0;
                int h = 0;
                int w = 0;
                int o = 0;
                if (Rogue.RAM.Player.Equipment.Boots != null)
                {
                    b = (Rogue.RAM.Player.Equipment.Boots.ILvl - Rogue.RAM.Player.Level) * GetItemRareGear(Rogue.RAM.Player.Equipment.Boots) * GetItemTypeGear(Rogue.RAM.Player.Equipment.Boots);
                }
                if (Rogue.RAM.Player.Equipment.Armor != null)
                {
                    c = (Rogue.RAM.Player.Equipment.Armor.ILvl - Rogue.RAM.Player.Level) * GetItemRareGear(Rogue.RAM.Player.Equipment.Armor) * GetItemTypeGear(Rogue.RAM.Player.Equipment.Armor);
                }
                if (Rogue.RAM.Player.Equipment.Helm != null)
                {
                    h = (Rogue.RAM.Player.Equipment.Helm.ILvl - Rogue.RAM.Player.Level) * GetItemRareGear(Rogue.RAM.Player.Equipment.Helm) * GetItemTypeGear(Rogue.RAM.Player.Equipment.Helm);
                }
                if (Rogue.RAM.Player.Equipment.Weapon != null)
                {
                    w = (Rogue.RAM.Player.Equipment.Weapon.ILvl - Rogue.RAM.Player.Level) * GetItemRareGear(Rogue.RAM.Player.Equipment.Weapon) * GetItemTypeGear(Rogue.RAM.Player.Equipment.Weapon);
                }
                if (Rogue.RAM.Player.Equipment.OffHand != null)
                {
                    o = (Rogue.RAM.Player.Equipment.OffHand.ILvl - Rogue.RAM.Player.Level) * GetItemRareGear(Rogue.RAM.Player.Equipment.OffHand) * GetItemTypeGear(Rogue.RAM.Player.Equipment.OffHand);
                }
                return b + c + h + w + o;
            }            
            /// <summary>
            /// Return sell value
            /// </summary>
            public virtual int Sell
            {
                get
                {
                    return Rogue.RAM.Map.Level * 7;
                }
            }
            /// <summary>
            /// Return sell value in reputation case
            /// </summary>
            public int ReputationSell;
            /// <summary>
            /// Return name of fraction
            /// </summary>
            public string ReputationName;
            /// <summary>
            /// If item enchanted
            /// </summary>
            public bool Enchanted = false;
            /// <summary>
            /// return can player buy this item or not
            /// </summary>
            public bool CheckReputation
            {
                get
                {
                    foreach (Reputation r in Rogue.RAM.Player.Repute)
                    {
                        if (r.name == this.ReputationName)
                        {
                            if (r.min >= this.ReputationSell)
                            {
                                return true;
                            }
                            else { return false; }
                        }
                        else { return false; }
                    }
                    return false;
                }
            }
            /// <summary>
            /// New void for special effect from activate item
            /// </summary>
            /// <returns></returns>
            public virtual bool Use()
            {
                return false;
            }
            /// <summary>
            /// wtf field?
            /// </summary>
            public int SpentS
            {
                get
                {
                    return 25;
                }
            }
            /// <summary>
            /// Return info about item
            /// </summary>
            public string Info="";
            /// <summary>
            /// For gearscore
            /// </summary>
            private static int GetItemRareGear(Item I)
            {
                int r = 0;
                try
                {
                    if (I.Rare == Rarity.Poor)
                    {
                        return 1;
                    }
                    else if (I.Rare == Rarity.Common)
                    {
                        return 2;
                    }
                    else if (I.Rare == Rarity.Uncommon)
                    {
                        return 4;
                    }
                    else if (I.Rare == Rarity.Rare)
                    {
                        return 7;
                    }
                    else if (I.Rare == Rarity.Set)
                    {
                        return 9;
                    }
                    else if (I.Rare == Rarity.Epic)
                    {
                        return 10;
                    }
                    else if (I.Rare == Rarity.Legendary)
                    {
                        return 12;
                    }
                    else if (I.Rare == Rarity.Artefact)
                    {
                        return 15;
                    }
                }
                catch (NullReferenceException)
                {
                    r = 1;
                }
                return r;
            }
            /// <summary>
            /// For gearscore
            /// </summary>
            private static int GetItemTypeGear(Item I)
            {
                int r = 0; //item icon
                switch (I.Kind)
                {
                    case Kind.Armor:
                        {
                            r = 4;
                            break;
                        }
                    case Kind.Weapon:
                        {
                            r = 5;
                            break;
                        }
                    case Kind.Helm:
                        {
                            r = 4;
                            break;
                        }
                    case Kind.Boots:
                        {
                            r = 2;
                            break;
                        }
                    case Kind.OffHand:
                        {
                            r = 5;
                            break;
                        }
                    case Kind.Potion:
                        {
                            r = 1;
                            break;
                        }
                    case Kind.Elixir:
                        {
                            r = 2;
                            break;
                        }
                    case Kind.Scroll:
                        {
                            r = 5;
                            break;
                        }
                }
                return r;
            }
        }

        public class Equipment
        {
            public Item.Helm Helm;

            public Item.Armor Armor;

            public Item.Boots Boots;

            public Item.Weapon Weapon;

            public Item.OffHand OffHand;
        }

        public class Ability
        {
            /// <summary>
            /// Name of ability
            /// </summary>
            public string Name;
            /// <summary>
            /// Info about ability
            /// </summary>
            public string Info
            {
                set { _Info = value; }
                get
                {
                    string rtrn = _Info;
                    rtrn = rtrn.Replace("&", "&" + this.Power.ToString());
                    rtrn = rtrn.Replace("^", "^" + this.Power.ToString());
                    rtrn = rtrn.Replace("#", "#" + this.Power.ToString());
                    rtrn = rtrn.Replace(";", ";" + this.Power.ToString());
                    if ((this.SummonMonster != null) && (rtrn.IndexOf('‡') > -1))
                    {
                        if (this.SummonMonster.PAD != 0) { rtrn = rtrn.Replace("‡", this.SummonMonster.PAD.ToString()); }
                        else { rtrn = rtrn.Replace("‡", this.SummonMonster.PAH.ToString()); }
                    }
                    if (this.SummonMonster != null) { rtrn = rtrn.Replace("{", "{" + this.SummonMonster.Name); }
                    rtrn = rtrn.Replace("<", "<" + (this.Duration/this.DHoTtiks).ToString());
                    rtrn = rtrn.Replace("@", "@" + this.Level);
                    if (this.Action != null) { rtrn = rtrn.Replace("?", "?" + this.Action.Count.ToString()); }
                    if (this.Stats != null) { rtrn = rtrn.Replace("№", "№" + this.Stats.Count.ToString()); }
                    rtrn = rtrn.Replace("~", "~" + this.Power.ToString());
                    rtrn = rtrn.Replace("$", "$1");
                    rtrn = rtrn.Replace("†", "†" + Rogue.RAM.Player.MMP.ToString());
                    return rtrn;
                }
            }
            /// <summary>
            /// Real info field
            /// </summary>
            public string _Info;
            /// <summary>
            /// Draw info about this ability
            /// </summary>
            public void DrawInfo()
            {
                DrawEngine.AbilityDraw.AdditionalInfoWindow(this.Info, this.Name, SystemEngine.Helper.String.ToString(this.Mode), this.ManaCost().ToString(), SystemEngine.Helper.String.ToString(this.Rate, ADRate, APRate), this.LVRate.ToString(), this.Duration.ToString(), SystemEngine.Helper.String.ToString(this.Location));
            }
            /// <summary>
            /// Field-piece of _Power_ if ability Attack damage
            /// </summary>
            public double ADRate;
            /// <summary>
            /// Field-piece of _Power_ if ability Magic
            /// </summary>
            public double APRate;
            /// <summary>
            /// This field peice of _Power_ this ability
            /// </summary>
            public double LVRate;
            /// <summary>
            /// COE
            /// Coefficient of efficiency
            /// Show how much this ability help character RIGHT NOW
            /// When character grow up this ability COE grow up too
            /// </summary>
            public double COE
            {
                get { return _COE(); }
            }
            /// <summary>
            /// Duration of ability in seconds: 0 - instant effect and permanent time, more then 0 mean ability will something of time, buff, debuff, damage, heal, summon. 
            /// </summary>
            public int Duration;
            public int BaseStat;
            public int BaseStat2;
            public int BaseDuration;
            /// <summary>
            /// Count of ticks DOT/Hot
            /// </summary>
            public double DHoTtiks;
            /// <summary>
            /// Percent of resourse
            /// </summary>
            public double CostRate;
            /// <summary>
            /// for cost
            /// </summary>
            private int RealCost
            {
                get
                {
                    if (Rogue.RAM.Player.Class != BattleClass.Paladin && Rogue.RAM.Player.Class!= BattleClass.Warlock)
                    {
                        return Convert.ToInt32(Math.Round(((double)Rogue.RAM.Player.MMP / 100) * CostRate));
                    }
                    else if (Rogue.RAM.Player.Class== BattleClass.Paladin)
                    {
                        if (this.Name == "Столп Света")
                        {
                            int additionalrage = 0;
                            foreach (SystemEngine.ArmorSet aSet in Rogue.RAM.ArmSet)
                            {
                                if (aSet.Name == "HLight1" && aSet.Active)
                                {
                                    additionalrage = 5;
                                }
                            }
                            return Convert.ToInt32(Math.Round(((double)Rogue.RAM.Player.MMP / 100) * (CostRate - additionalrage)));
                        }
                        else if (this.Name == "Свет Небес")
                        {
                            int additionalrage = 0;
                            foreach (SystemEngine.ArmorSet aSet in Rogue.RAM.ArmSet)
                            {
                                if (aSet.Name == "HLight2" && aSet.Active)
                                {
                                    additionalrage = 10;
                                }
                            }
                            return Convert.ToInt32(Math.Round(((double)Rogue.RAM.Player.MMP / 100) * (CostRate - additionalrage)));
                        }
                        else
                        { return Convert.ToInt32(Math.Round(((double)Rogue.RAM.Player.MMP / 100) * CostRate)); }                        
                    }
                    else if (Rogue.RAM.Player.Class == BattleClass.Warlock)
                    {
                        int additionalrage = 0;
                        foreach (SystemEngine.ArmorSet aSet in Rogue.RAM.ArmSet)
                        {
                            if (aSet.Name == "DW1" && aSet.Active)
                            {
                                additionalrage = 10;
                            }
                        }
                        return Convert.ToInt32(Math.Round(((double)Rogue.RAM.Player.MMP / 100) * (CostRate + additionalrage)));
                    }
                    else
                    { return Convert.ToInt32(Math.Round(((double)Rogue.RAM.Player.MMP / 100) * CostRate)); }
                }
            }
            /// <summary>
            /// Uses for success of craft
            /// </summary>
            public double CraftShance=50;
            /// <summary>
            /// Special field for get manacost of each class
            /// </summary>
            /// <returns>BloodMage=Cost*0.2; Inq=Special; Necr like BloodMage; Assassin and Alchemist = 1; Other = real;</returns>
            public int ManaCost()
            {
                if (this.Scroll) { return -1; }
                switch (Rogue.RAM.Player.Class)
                {
                    case BattleClass.Inquisitor:
                        { return 1; }
                    case BattleClass.Necromant:
                        { return Convert.ToInt32(CostRate); }
                    case BattleClass.Assassin:
                        { return 1; }
                    case BattleClass.Warrior:
                        { return Convert.ToInt32(this.CostRate); }
                    default:
                        { return RealCost; }
                }
            }
            /// <summary>
            /// Cost and minus mana
            /// </summary>
            public bool CanCost
            {
                get
                {                    
                    if (Rogue.RAM.Player.Class == BattleClass.BloodMage)
                    {
                        if ((Rogue.RAM.Player.CHP - this.ManaCost()) > 0)
                        {
                            return true;
                        }
                        else { DrawEngine.InfoWindow.Message = "У вас не хватает ресурсов на использование способности!"; return false; }
                    }
                    else if (Rogue.RAM.Player.CMP >= this.ManaCost())
                    {                        
                        return true;
                    }
                    else { DrawEngine.InfoWindow.Message = "У вас не хватает ресурсов на использование способности!";}
                    return false; 
                }
            }
            public void Cost()
            {
                int t = 0;
                if (Rogue.RAM.Player.Class == BattleClass.BloodMage)
                {
                    Rogue.RAM.Player.CHP -= this.ManaCost();
                    t = this.ManaCost();
                }
                else 
                {
                    if (Rogue.RAM.Player.Class == BattleClass.Alchemist)
                    {
                        int additionalrage = 0;
                        foreach (SystemEngine.ArmorSet aSet in Rogue.RAM.ArmSet)
                        {
                            if (aSet.Name == "ElArm" && aSet.Active)
                            {
                                if (r.Next(99) < 10) { additionalrage = this.ManaCost(); }
                            }
                        }
                        Rogue.RAM.Player.CMP -= (this.ManaCost() + additionalrage);
                        t = this.ManaCost() + additionalrage;
                    }
                    else { Rogue.RAM.Player.CMP -= this.ManaCost(); t = this.ManaCost(); }
                
                }
                DrawEngine.InfoWindow.Warning = "Вы тратите " + Rogue.RAM.Player.ManaName + t + " !";
                DrawEngine.GUIDraw.ReDrawCharStat();
            }
            /// <summary>
            /// Real power
            /// </summary>
            private int _Power;
            /// <summary>
            /// Power of ability, need for Activation
            /// </summary>
            public int Power
            {
                set { _Power = value; }
                get
                {
                    ///Warlock set bonus (+20% damage)
                    if (Rogue.RAM.Player.Class == BattleClass.Warlock)
                    {
                        int additionalrage = 0;
                        foreach (SystemEngine.ArmorSet aSet in Rogue.RAM.ArmSet)
                        {
                            if (aSet.Name == "DW2" && aSet.Active)
                            {
                                additionalrage = 20;
                            }
                        }
                        return (_Power + Convert.ToInt32(Rogue.RAM.Player.AP * this.APRate)) + Convert.ToInt32(Math.Round(((double)_Power + Convert.ToInt32(Rogue.RAM.Player.AP * this.APRate)) * additionalrage));
                    }


                    if (this.Name == "Усмирить") { if (Rogue.RAM.Enemy != null) { if (Rogue.RAM.Enemy.CHP == Rogue.RAM.Enemy.MHP) { return _Power + Convert.ToInt32(Rogue.RAM.Player.AD * this.ADRate) + Rogue.RAM.Enemy.ARM; } else { return 0; } } }
                    if (this.Name == "Добить") { if (Rogue.RAM.Enemy != null) { if (Rogue.RAM.Enemy.CHP <= Convert.ToInt32(Rogue.RAM.Enemy.MHP * 0.25)) { return _Power + Convert.ToInt32(Rogue.RAM.Player.AD * this.ADRate) + Rogue.RAM.Enemy.ARM; } else { return 0; } } }
                    if (this.Name == "Элементализм") { return _Power; }
                    if (this.Name == "Булавка %") { return Convert.ToInt32((Rogue.RAM.Player.CMP * 0.01) * this.Level) + Convert.ToInt32(Rogue.RAM.Player.AD * this.ADRate); }

                    if (this.Rate == AbilityRate.AbilityPower)
                    {
                        return _Power + Convert.ToInt32(Rogue.RAM.Player.AP * this.APRate);
                    }
                    else
                    {
                        return _Power + Convert.ToInt32(Rogue.RAM.Player.AD * this.ADRate);
                    }
                }
            }
            /// <summary>
            /// Level of ability, for activation and rates and for ALL
            /// </summary>
            public int Level;
            /// <summary>
            /// Item for ability if ability created item
            /// </summary>
            public Item CraftItem;
            /// <summary>
            /// Uses for get this craft item
            /// </summary>
            public string CraftGetName;
            /// <summary>
            /// Monster for ability if ability summoned creature
            /// </summary>
            public Summoned SummonMonster;
            /// <summary>
            /// If you want summon once
            /// </summary>
            public bool SummonBlock;
            /// <summary>
            /// If this ability use in scroll, mana cost need be 0;
            /// </summary>
            public bool Scroll = false;
            /// <summary>
            /// Form for change form
            /// </summary>
            public Morphling Form;
            /// <summary>
            /// Class for ability, only Character.Class==Ability.Class can use it
            /// </summary>
            public BattleClass Class;
            /// <summary>
            /// Color of icon ability, combat log for ability, and graphic special
            /// </summary>
            public ConsoleColor Color;
            /// <summary>
            /// Type of ability, active or passive.
            /// </summary>
            public AbilityType Mode;
            /// <summary>
            /// Type of ability rate. It can be AttackDamage of AbilityPower
            /// </summary>
            public AbilityRate Rate;
            /// <summary>
            /// Action for ability, for more simply created new
            /// </summary>
            public List<AbilityAction> Action;
            /// <summary>
            /// Damage-element for ability.
            /// </summary>
            public AbilityElement Elem;
            /// <summary>
            /// Set location for use
            /// </summary>
            public AbilityLocation Location;
            /// <summary>
            /// Uses for debuff/improve and some things
            /// </summary>
            public List<AbilityStats> Stats=new List<AbilityStats>();
            /// <summary>
            /// Uses for destruction objects near
            /// </summary>
            public AbilityDestructionType Dest;
            /// <summary>
            /// Count of fields near character
            /// </summary>
            public int AOE;
            /// <summary>
            /// Uses for ability with choose
            /// </summary>
            public string Menu;
            /// <summary>
            /// Char icon
            /// </summary>
            private char _Icon;
            /// <summary>
            /// This field created for REUSABLE
            /// </summary>
            public char Icon
            {
                set { _Icon = value; }
                get { return _Icon; }
            }


            /// <summary>
            /// Activate ability
            /// </summary>
            public void Activate()
            {
                bool  Success=false;
                if (this.CanCost)
                {
                    Success = true;
                }
                else
                {
                    if (Rogue.RAM.Enemy != null)
                    {
                        if (Rogue.RAM.Enemy.DoT.IndexOf(this) > -1) { Success = true; }
                        else { Success = false; }
                    }
                    if (Rogue.RAM.Player.Buffs.IndexOf(this) > -1)
                    { Success = true; }
                }
                if (Success)
                {
                    if (this.Mode != AbilityType.Active) { DrawEngine.InfoWindow.Custom("Споособность " + this.Name + " нельзя активировать!"); }
                    else
                    {
                        switch (this.Location)
                        {
                            case AbilityLocation.Alltime: { Use(); break; }
                            case AbilityLocation.Combat:
                                {
                                    if (Rogue.RAM.Enemy == null) { DrawEngine.InfoWindow.Custom("Способность " + this.Name + " нельзя использовать на карте!"); }
                                    else
                                    { Use(); } break;
                                }
                            case AbilityLocation.WorldMap:
                                {
                                    if (Rogue.RAM.Enemy != null) { DrawEngine.InfoWindow.Custom("Способность " + this.Name + " нельзя использовать в бою!"); }
                                    else
                                    {
                                        if (this.Menu == "Banish") { Rogue.RAM.PopUpTab.MaxTab = Rogue.RAM.Player.Ability.IndexOf(this); PlayEngine.Menu.AbilityMenu(this.Menu); }
                                        if (this.Menu == "Trap") { Rogue.RAM.PopUpTab.MaxTab = Rogue.RAM.Player.Ability.IndexOf(this); PlayEngine.Menu.AbilityMenu(this.Menu); }
                                        Use();
                                    }
                                    break;
                                }
                        }
                    }
                }
            }
            private bool NeedCost = false;
            public void Use()
            {
                foreach (AbilityAction A in this.Action)
                {
                    switch (A.Act)
                    {
                        case AbilityActionType.Craft: { UseCraft(A); break; }
                        case AbilityActionType.Damage: { UseDamage(A); break; }
                        case AbilityActionType.Debuff: { UseDebuff(A); break; }
                        case AbilityActionType.Destruction: { UseDestruction(); break; }
                        case AbilityActionType.Heal: { UseHeal(A); break; }
                        case AbilityActionType.Improve: { UseBuff(A); break; }
                        case AbilityActionType.Neutral: { UseNeutral(); break; }
                        case AbilityActionType.Summon: { UseSummon(A); break; }
                    }
                }

                if (NeedCost) { this.Cost(); }

                if (Rogue.RAM.Enemy != null)
                {
                    if (Rogue.RAM.Enemy.CHP <= 0)
                    {
                        Rogue.RAM.Log.Add("Вы одержали победу, нажмите {A}!");
                        DrawEngine.FightDraw.DrawEnemyStat();
                        DrawEngine.FightDraw.ReDrawCombatLog();
                    }
                    else
                    {
                        PlayEngine.GamePlay.EnemyAttack();
                        DrawEngine.FightDraw.DrawEnemyStat();
                    }
                }
                
                if (Rogue.RAM.Player.Class == BattleClass.Alchemist)
                {
                    AlchemistPassiveBuff();
                }

                DrawEngine.GUIDraw.ReDrawCharStat();
                //DrawEngine.FightDraw.ReDrawBuffDeBuff();

                if (Rogue.RAM.Player.CHP <= 0)
                {
                    Rogue.RAM.Log.Add("Вы проигрываете битву...");
                    DrawEngine.FightDraw.ReDrawCombatLog();
                    Thread.Sleep(2000);
                    DrawEngine.SplashScreen.EndOfGame();
                    Rogue.Main(new string[0]);
                }
            }

            public void UseCraft(AbilityAction Act)
            {
                if (r.Next(99) <= Convert.ToInt32(Math.Round(this.CraftShance)))
                {
                    if (this.CanCost)
                    {
                        var it = this.CraftItem;
                        bool once = false;
                        bool open = false;
                        var Lab = Rogue.RAM.Map;
                        for (int y = 0; y < 23; y++)
                        {
                            for (int x = 0; x < 71; x++)
                            {
                                if (once == false)
                                {
                                    if (Lab.Map[x][y].Player != null)
                                    {
                                        if (Lab.Map[x + 1][y].Item == null && Lab.Map[x + 1][y].Wall == null && Lab.Map[x + 1][y].Object == null && Lab.Map[x + 1][y].Enemy == null)
                                        {
                                            if (open == false)
                                            {
                                                Lab.Map[x + 1][y].Item = it;
                                                DrawEngine.InfoWindow.Custom("Вы создали предмет: " + this.Name);
                                                DrawEngine.LabDraw.ReDrawObject(x + 1, y, Lab.Map[x + 1][y].Item);
                                                once = true;
                                                open = true;
                                            }
                                        }
                                        if (Lab.Map[x - 1][y].Item == null && Lab.Map[x - 1][y].Wall == null && Lab.Map[x - 1][y].Object == null && Lab.Map[x - 1][y].Enemy == null)
                                        {
                                            if (open == false)
                                            {
                                                Lab.Map[x - 1][y].Item = it;
                                                DrawEngine.InfoWindow.Custom("Вы создали предмет: " + this.Name);
                                                DrawEngine.LabDraw.ReDrawObject(x - 1, y, Lab.Map[x - 1][y].Item);
                                                once = true;
                                                open = true;
                                            }
                                        }
                                        if (Lab.Map[x][y + 1].Item == null && Lab.Map[x][y + 1].Wall == null && Lab.Map[x][y + 1].Object == null && Lab.Map[x][y + 1].Enemy == null)
                                        {
                                            if (open == false)
                                            {
                                                Lab.Map[x][y + 1].Item = it;
                                                DrawEngine.InfoWindow.Custom("Вы создали предмет: " + this.Name);
                                                DrawEngine.LabDraw.ReDrawObject(x, y + 1, Lab.Map[x][y + 1].Item);
                                                once = true;
                                                open = true;
                                            }
                                        }
                                        if (Lab.Map[x][y - 1].Item == null && Lab.Map[x][y - 1].Wall == null && Lab.Map[x][y - 1].Object == null && Lab.Map[x][y - 1].Enemy == null)
                                        {
                                            if (open == false)
                                            {
                                                Lab.Map[x][y - 1].Item = it;
                                                DrawEngine.InfoWindow.Custom("Вы создали предмет: " + this.Name);
                                                DrawEngine.LabDraw.ReDrawObject(x, y - 1, Lab.Map[x][y - 1].Item);
                                                once = true;
                                                open = true;
                                            }
                                        }
                                        if (Lab.Map[x + 1][y + 1].Item == null && Lab.Map[x + 1][y + 1].Wall == null && Lab.Map[x + 1][y + 1].Object == null && Lab.Map[x + 1][y + 1].Enemy == null)
                                        {
                                            if (open == false)
                                            {
                                                Lab.Map[x + 1][y + 1].Item = it;
                                                DrawEngine.InfoWindow.Custom("Вы создали предмет: " + this.Name);
                                                DrawEngine.LabDraw.ReDrawObject(x + 1, y + 1, Lab.Map[x + 1][y + 1].Item);
                                                once = true;
                                                open = true;
                                            }
                                        }
                                        if (Lab.Map[x + 1][y - 1].Item == null && Lab.Map[x + 1][y - 1].Wall == null && Lab.Map[x + 1][y - 1].Object == null && Lab.Map[x + 1][y - 1].Enemy == null)
                                        {
                                            if (open == false)
                                            {
                                                Lab.Map[x + 1][y - 1].Item = it;
                                                DrawEngine.InfoWindow.Custom("Вы создали предмет: " + this.Name);
                                                DrawEngine.LabDraw.ReDrawObject(x + 1, y - 1, Lab.Map[x + 1][y - 1].Item);
                                                once = true;
                                                open = true;
                                            }
                                        }
                                        if (Lab.Map[x - 1][y - 1].Item == null && Lab.Map[x - 1][y - 1].Wall == null && Lab.Map[x - 1][y - 1].Object == null && Lab.Map[x - 1][y - 1].Enemy == null)
                                        {
                                            if (open == false)
                                            {
                                                Lab.Map[x - 1][y - 1].Item = it;
                                                DrawEngine.InfoWindow.Custom("Вы создали предмет: " + this.Name);
                                                DrawEngine.LabDraw.ReDrawObject(x - 1, y - 1, Lab.Map[x - 1][y - 1].Item);
                                                once = true;
                                                open = true;
                                            }
                                        }
                                        if (Lab.Map[x - 1][y + 1].Item == null && Lab.Map[x - 1][y + 1].Wall == null && Lab.Map[x - 1][y + 1].Object == null && Lab.Map[x - 1][y + 1].Enemy == null)
                                        {
                                            if (open == false)
                                            {
                                                Lab.Map[x - 1][y + 1].Item = it;
                                                DrawEngine.InfoWindow.Custom("Вы создали предмет: " + this.Name);
                                                DrawEngine.LabDraw.ReDrawObject(x - 1, y + 1, Lab.Map[x - 1][y + 1].Item);
                                                once = true;
                                                open = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        Rogue.RAM.Map = Lab;
                        if (open == false)
                        {
                            DrawEngine.InfoWindow.Custom("Невозможно создать предмет.");
                            PlayEngine.GamePlay.Play();
                        }
                        else
                        { this.NeedCost = true; }
                    }
                    else { NeedCost = false; }
                }
                else
                { if (this.CanCost) { this.NeedCost = true; DrawEngine.InfoWindow.Warning = "Попытка создать предмет провалилась!"; Thread.Sleep(500); } }
            }

            public void UseDamage(AbilityAction Act)
            {
                #region Instant damage
                if (Act.Atr.IndexOf(AbilityActionAttribute.DmgHealInstant)>-1)
                {                    
                    if (this.CanCost)
                    {
                        if (this.Name == "Взрыв магии")
                        {
                            bool have = false;
                            foreach (Ability a in Rogue.RAM.Enemy.DoT)
                            {
                                if (a.Name == "Метка") { have = true; }
                            }
                            if (have)
                            {
                                int d = this.Power;
                                d = d - Convert.ToInt32(Rogue.RAM.Enemy.MRS * 0.5);
                                //Если ушли в минус по дамагу
                                if (d < 0) { d = 0; }
                                //вычитаем
                                Rogue.RAM.Enemy.CHP -= d;
                                //в лог
                                Rogue.RAM.Log.Add(Rogue.RAM.Player.Name + " взрывает метку и наносит " + d.ToString() + " маг. урона!");
                                DrawEngine.FightDraw.ReDrawCombatLog();
                                //мана
                                this.NeedCost = true;
                            }
                            else
                            {
                                Rogue.RAM.Log.Add("У врага нет метки!");
                                DrawEngine.FightDraw.ReDrawCombatLog();
                                this.NeedCost = false;
                            }
                        }
                        else if (this.Name == "Взрыв силы")
                        {
                            bool have = false;
                            foreach (Ability a in Rogue.RAM.Enemy.DoT)
                            {
                                if (a.Name == "Метка") { have = true; }
                            }
                            if (have)
                            {
                                int d = this.Power;
                                d = d - Convert.ToInt32(Rogue.RAM.Enemy.ARM * 0.25);
                                //Если ушли в минус по дамагу
                                if (d < 0) { d = 0; }
                                //вычитаем
                                Rogue.RAM.Enemy.CHP -= d;
                                //в лог
                                Rogue.RAM.Log.Add(Rogue.RAM.Player.Name + " взрывает метку и наносит " + d.ToString() + " маг. урона!");
                                DrawEngine.FightDraw.ReDrawCombatLog();
                                //мана
                                this.NeedCost = true;
                            }
                            else
                            {
                                Rogue.RAM.Log.Add("У врага нет метки!");
                                DrawEngine.FightDraw.ReDrawCombatLog();
                                this.NeedCost = false;
                            }
                        }
                        if (this.Name == "Кольцо яда")
                        {
                            if (Rogue.RAM.Enemy == null)
                            {
                                UsePoisonNova();                                
                            }
                            //мана
                            this.NeedCost = true;
                        }
                        else if (this.Name == "Щит огня") 
                        {                            
                            if (Rogue.RAM.Enemy == null)
                            {
                                if (Rogue.RAM.Player.Buffs.IndexOf(this) == -1)
                                {
                                    UseFireShield();
                                    //мана
                                    this.NeedCost = true;
                                }
                                else
                                {
                                    //мана
                                    this.NeedCost = false;
                                }
                            }
                            else
                            {
                                int d = this.Power - Convert.ToInt32(Rogue.RAM.Enemy.MRS * 0.5);
                                if (d < 0) { d = 0; }
                                //вычитаем
                                Rogue.RAM.Enemy.CHP -= d;
                                Rogue.RAM.Log.Add(Rogue.RAM.Player.Name + " использует " + this.Name + " и наносит " + d.ToString() + " урона!");
                                DrawEngine.FightDraw.ReDrawCombatLog();
                                //мана
                                this.NeedCost = true;
                            }
                            
                        }
                        else if (this.Name == "Удар Валькирии")
                        {
                            int d = this.Power;

                            int orb = 0;
                            foreach (Ability a in Rogue.RAM.Player.Buffs)
                            {
                                if (a.Name == "Сфера инея") { orb = 1; }
                                if (a.Name == "Сфера льда") { orb = 2; }
                                if (a.Name == "Сфера воды") { orb = 3; }
                            }
                            switch (orb)
                            {
                                case 1:
                                    {
                                        Rogue.RAM.Enemy.MIDMG -= this.Level;
                                        Rogue.RAM.Enemy.MADMG -= this.Level;
                                        Rogue.RAM.Log.Add("Сфера инея уменьшает урон врага на " + this.Level + " !");
                                        DrawEngine.FightDraw.ReDrawCombatLog();
                                        break;
                                    }
                                case 2:
                                    {
                                        if (Rogue.RAM.Player.CHP + this.Level > Rogue.RAM.Player.MHP)
                                        { Rogue.RAM.Player.CHP = Rogue.RAM.Player.MHP; }
                                        else { Rogue.RAM.Player.CHP += this.Level; }
                                        Rogue.RAM.Log.Add("Сфера воды лечит вас на " + this.Level + " !");
                                        DrawEngine.FightDraw.ReDrawCombatLog();
                                        break;
                                    }
                                case 3:
                                    {
                                        d += this.Level;
                                        Rogue.RAM.Log.Add("Сфера льда добавляет " + this.Level + " урона!");
                                        DrawEngine.FightDraw.ReDrawCombatLog();
                                        break;
                                    }
                            }

                            //int d = this.Power;
                            if (this.Rate == AbilityRate.AbilityPower)
                            {
                                if (Rogue.RAM.Enemy.MRS != 0) { d = d - Convert.ToInt32(Rogue.RAM.Enemy.MRS * 0.5); }
                            }
                            else
                            {
                                if (Rogue.RAM.Enemy.ARM != 0) { d = d - Convert.ToInt32(Rogue.RAM.Enemy.ARM * 0.25); }
                            }
                            //Если ушли в минус по дамагу
                            if (d < 0) { d = 0; }
                            //вычитаем
                            Rogue.RAM.Enemy.CHP -= d;
                            //в лог
                            Rogue.RAM.Log.Add(Rogue.RAM.Player.Name + " использует " + this.Name + " и наносит " + d.ToString() + " урона!");
                            DrawEngine.FightDraw.ReDrawCombatLog();
                            //мана
                            this.NeedCost = true;

                        }
                        else if (this.Name == "Изгнать зло")
                        {
                            if (Rogue.RAM.Enemy.Race == MonsterRace.Drow || Rogue.RAM.Enemy.Race == MonsterRace.Human || Rogue.RAM.Enemy.Race == MonsterRace.Undead)
                            {
                                //изгнать зло бьёт сквозь защиту
                                int d = this.Power;
                                //Если ушли в минус по дамагу
                                if (d < 0) { d = 0; }
                                //вычитаем
                                Rogue.RAM.Enemy.CHP -= d;
                                //в лог
                                Rogue.RAM.Log.Add(Rogue.RAM.Player.Name + " использует " + this.Name + " и наносит " + d.ToString() + " урона!");
                                DrawEngine.FightDraw.ReDrawCombatLog();
                                //мана
                                this.NeedCost = true;
                            }
                            else { Rogue.RAM.Log.Add(Rogue.RAM.Enemy.Name + " не является злом!"); }
                        }
                        else if (this.Name == "Последний")
                        {
                            int lostpower = Convert.ToInt32(this.Power / 13);
                            for (int i = 0; i < 13; i++)
                            {
                                int d = lostpower;
                                if (this.Rate == AbilityRate.AbilityPower)
                                {
                                    if (Rogue.RAM.Enemy.MRS != 0) { d = d - Convert.ToInt32(Rogue.RAM.Enemy.MRS * 0.5); }
                                }
                                else
                                {
                                    if (Rogue.RAM.Enemy.ARM != 0) { d = d - Convert.ToInt32(Rogue.RAM.Enemy.ARM * 0.25); }
                                }
                                //Булавка бьёт сквозь защиту
                                //if (this.Name == "Булавка %") { d = this.Power; }
                                //Если ушли в минус по дамагу
                                if (d < 0) { d = 0; }
                                //вычитаем
                                Rogue.RAM.Enemy.CHP -= d;
                                //в лог
                                Rogue.RAM.Log.Add(Rogue.RAM.Player.Name + " использует " + SystemEngine.Helper.Randomer.WarlockSpell + " и наносит " + d.ToString() + " урона!");
                                DrawEngine.FightDraw.ReDrawCombatLog();
                                //мана
                                this.NeedCost = true;
                            }
                        }
                        else if (this.Name == "Каменный кулак")
                        {
                            #region May Power
                            int additionalrage = 0;
                            foreach (SystemEngine.ArmorSet aSet in Rogue.RAM.ArmSet)
                            {
                                if (aSet.Name == "M4" && aSet.Active)
                                {
                                    additionalrage = 1;
                                }
                            }
                            if (additionalrage == 1)
                            {
                                int d = 0;
                                if (Rogue.RAM.Player.AP != 0)
                                {
                                    d = this._Power + Convert.ToInt32(Rogue.RAM.Player.AP * this.APRate);
                                }
                                else
                                {
                                    d = this._Power + Convert.ToInt32(Rogue.RAM.Player.AD * this.ADRate);
                                }
                                if (Rogue.RAM.Player.AP != 0)
                                {
                                    if (Rogue.RAM.Enemy.MRS != 0) { d = d - Convert.ToInt32(Rogue.RAM.Enemy.MRS * 0.5); }
                                }
                                else
                                {
                                    if (Rogue.RAM.Enemy.ARM != 0) { d = d - Convert.ToInt32(Rogue.RAM.Enemy.ARM * 0.25); }
                                }
                                if (d < 0) { d = 0; }
                                //вычитаем
                                Rogue.RAM.Enemy.CHP -= d;
                                //в лог
                                Rogue.RAM.Log.Add(Rogue.RAM.Player.Name + " использует " + this.Name + " и наносит " + d.ToString() + " урона!");
                                DrawEngine.FightDraw.ReDrawCombatLog();
                                //мана
                                this.NeedCost = true;
                            }
                            else
                            {
                                int d = this.Power;
                                if (this.Rate == AbilityRate.AbilityPower)
                                {
                                    if (Rogue.RAM.Enemy.MRS != 0) { d = d - Convert.ToInt32(Rogue.RAM.Enemy.MRS * 0.5); }
                                }
                                else
                                {
                                    if (Rogue.RAM.Enemy.ARM != 0) { d = d - Convert.ToInt32(Rogue.RAM.Enemy.ARM * 0.25); }
                                }
                                //Булавка бьёт сквозь защиту
                                if (this.Name == "Булавка %") { d = this.Power; }
                                //Если ушли в минус по дамагу
                                if (d < 0) { d = 0; }
                                //вычитаем
                                Rogue.RAM.Enemy.CHP -= d;
                                //в лог
                                Rogue.RAM.Log.Add(Rogue.RAM.Player.Name + " использует " + this.Name + " и наносит " + d.ToString() + " урона!");
                                DrawEngine.FightDraw.ReDrawCombatLog();
                                //мана
                                this.NeedCost = true;
                            }
                            #endregion
                        }
                        else
                        {
                            int d = this.Power;
                            if (this.Rate == AbilityRate.AbilityPower)
                            {
                                if (Rogue.RAM.Enemy.MRS != 0) { d = d - Convert.ToInt32(Rogue.RAM.Enemy.MRS * 0.5); }
                            }
                            else
                            {
                                if (Rogue.RAM.Enemy.ARM != 0) { d = d - Convert.ToInt32(Rogue.RAM.Enemy.ARM * 0.25); }
                            }
                            //Булавка бьёт сквозь защиту
                            if (this.Name == "Булавка %") { d = this.Power; }
                            //Если ушли в минус по дамагу
                            if (d < 0) { d = 0; }
                            //вычитаем
                            Rogue.RAM.Enemy.CHP -= d;
                            //в лог
                            Rogue.RAM.Log.Add(Rogue.RAM.Player.Name + " использует " + this.Name + " и наносит " + d.ToString() + " урона!");
                            DrawEngine.FightDraw.ReDrawCombatLog();
                            //мана
                            this.NeedCost = true;
                        }
                    }
                    else { NeedCost = false; }
                }
                #endregion
                #region Damage on time
                if (Act.Atr.IndexOf(AbilityActionAttribute.DmgHealOnTime)>-1)
                {
                    //Если доты ещё нет
                    if (Rogue.RAM.Enemy.DoT.IndexOf(this) == -1)
                    {                        
                        //кидаем врагу доту
                        this.BaseDuration = this.Duration;
                        Rogue.RAM.Enemy.DoT.Add(this);
                        //первоначальный эффект
                        if (this.Name != "Смертельный яд")
                        {
                            int d = this.Power;

                            if (this.Rate == AbilityRate.AbilityPower)
                            {
                                if (Rogue.RAM.Enemy.MRS != 0) { d = d - Convert.ToInt32(Rogue.RAM.Enemy.MRS * 0.5); }
                            }
                            else
                            {
                                if (Rogue.RAM.Enemy.ARM != 0) { d = d - Convert.ToInt32(Rogue.RAM.Enemy.ARM * 0.25); }
                            }
                            //
                            if (d < 0) { d = 0; }

                            Rogue.RAM.Enemy.CHP -= d;
                            Rogue.RAM.Log.Add(this.Name + " наносит " + d.ToString() + " урона!");

                            DrawEngine.FightDraw.ReDrawCombatLog();
                            DrawEngine.FightDraw.DrawEnemyStat();

                            //if (Rogue.RAM.Enemy.CHP <= 0)
                            //{
                            //    Rogue.RAM.Log.Add("Заклинание добило врага! Нажмите {A} для выхода!");
                            //    DrawEngine.FightDraw.ReDrawCombatLog();
                            //}
                        }
                        else
                        {                            
                            if (r.Next(99) < this.Level)
                            {
                                Rogue.RAM.Enemy.CHP = 0;
                                Rogue.RAM.Log.Add("Срабатывает смертельный яд!");
                                Rogue.RAM.Log.Add("Яд убил врага! Нажмите {A} для выхода!");
                                DrawEngine.FightDraw.DrawEnemyStat();
                                DrawEngine.FightDraw.ReDrawCombatLog();
                            }
                        }
                    }
                    //Если дота уже есть
                    else
                    {
                        //если время кончилось
                        if (this.Duration <= 0)
                        {
                            //убираем из дебафов
                            Rogue.RAM.Enemy.DoT.Remove(this);
                            //возвращаем время
                            this.Duration = this.BaseDuration;
                        }
                        //
                        if (this.Name != "Смертельный яд")
                        {
                            int d = this.Power;

                            if (this.Rate == AbilityRate.AbilityPower)
                            {
                                if (Rogue.RAM.Enemy.MRS != 0) { d = d - Convert.ToInt32(Rogue.RAM.Enemy.MRS * 0.5); }
                            }
                            else
                            {
                                if (Rogue.RAM.Enemy.ARM != 0) { d = d - Convert.ToInt32(Rogue.RAM.Enemy.ARM * 0.25); }
                            }
                            //
                            if (d < 0) { d = 0; }

                            Rogue.RAM.Enemy.CHP -= d;
                            Rogue.RAM.Log.Add(this.Name + " наносит " + d.ToString() + " урона!");

                            DrawEngine.FightDraw.ReDrawCombatLog();
                            DrawEngine.FightDraw.DrawEnemyStat();

                            //if (Rogue.RAM.Enemy.CHP <= 0)
                            //{
                            //    Rogue.RAM.Log.Add("Заклинание добило врага! Нажмите {A} для выхода!");
                            //    DrawEngine.FightDraw.ReDrawCombatLog();
                            //}
                        }
                        else
                        {
                            if (r.Next(99) < this.Level)
                            {
                                Rogue.RAM.Enemy.CHP = 0;
                                Rogue.RAM.Log.Add("Срабатывает смертельный яд!");
                                Rogue.RAM.Log.Add("Яд убил врага! Нажмите {A} для выхода!");
                                DrawEngine.FightDraw.DrawEnemyStat();
                                DrawEngine.FightDraw.ReDrawCombatLog();
                            }
                        }
                    }
                }
                #endregion
            }
            public void UseHeal(AbilityAction Act)
            {
                #region Instant heal
                if (Act.Atr.IndexOf(AbilityActionAttribute.DmgHealInstant) > -1)
                {
                    if (this.CanCost)
                    {                        
                        int d = this.Power;                        
                        if (Rogue.RAM.Player.Class == BattleClass.Warlock)
                        { d *= 2; }
                        //if combat
                        if (Rogue.RAM.Enemy != null)
                        {
                            if (this.Rate == AbilityRate.AbilityPower)
                            {
                                if (Rogue.RAM.Enemy.MRS != 0) { d = d - Convert.ToInt32(Rogue.RAM.Enemy.MRS * 0.37); }
                            }
                            else
                            {
                                if (Rogue.RAM.Enemy.ARM != 0) { d = d - Convert.ToInt32(Rogue.RAM.Enemy.ARM * 0.37); }
                            }
                            if (d < 0) { d = 0; }
                            Rogue.RAM.Log.Add(Rogue.RAM.Player.Name + " использует " + this.Name + " и исцеляет " + d + " урона!");
                            DrawEngine.FightDraw.ReDrawCombatLog();
                        }
                        //if world map
                        else
                        {
                            DrawEngine.InfoWindow.Custom("Вы используете " + this.Name + " и исцеляете " + d + " урона!"); Thread.Sleep(500);
                        }
                        //effect
                        if ((Rogue.RAM.Player.CHP + d) > Rogue.RAM.Player.MHP)
                        {
                            Rogue.RAM.Player.CHP = Rogue.RAM.Player.MHP;
                        }
                        else
                        {
                            Rogue.RAM.Player.CHP += d;
                        }
                        //мана
                        this.NeedCost = true;
                    }
                    else { NeedCost = false; }

                }
                #endregion
                #region Heal on time
                if (Act.Atr.IndexOf(AbilityActionAttribute.DmgHealOnTime) > -1)
                {
                    //Если hot ещё нет
                    if (Rogue.RAM.Player.Buffs.IndexOf(this) == -1)
                    {
                        //кидаем player доту
                        this.BaseDuration = this.Duration;
                        Rogue.RAM.Player.Buffs.Add(this);

                        //первоначальный эффект
                        int d = this.Power;
                        //if combat
                        if (Rogue.RAM.Enemy != null)
                        {
                            if (this.Rate == AbilityRate.AbilityPower)
                            {
                                if (Rogue.RAM.Enemy.MRS != 0) { d = d - Convert.ToInt32(Rogue.RAM.Enemy.MRS * 0.37); }
                            }
                            else
                            {
                                if (Rogue.RAM.Enemy.ARM != 0) { d = d - Convert.ToInt32(Rogue.RAM.Enemy.ARM * 0.37); }
                            }
                            if (d < 0) { d = 0; }
                            Rogue.RAM.Log.Add(Rogue.RAM.Player.Name + " использует " + this.Name + " и исцеляет " + d + " урона!");
                            DrawEngine.FightDraw.ReDrawCombatLog();
                        }
                        //if world map
                        else
                        {
                            DrawEngine.InfoWindow.Custom("Вы используете " + this.Name + " и исцеляете " + d + " урона!"); Thread.Sleep(500);
                        }
                        //effect
                        if ((Rogue.RAM.Player.CHP + d) > Rogue.RAM.Player.MHP)
                        {
                            Rogue.RAM.Player.CHP = Rogue.RAM.Player.MHP;
                        }
                        else
                        {
                            Rogue.RAM.Player.CHP += d;
                        }
                        //мана
                        this.NeedCost = true;
                    }
                    //Если дота уже есть
                    else
                    {
                        //если время кончилось
                        if (this.Duration <= 0)
                        {
                            //убираем из дебафов
                            Rogue.RAM.Player.Buffs.Remove(this);
                            //возвращаем время
                            this.Duration = this.BaseDuration;
                        }

                        //effect
                        int d = this.Power;
                        //if combat
                        if (Rogue.RAM.Enemy != null)
                        {
                            if (this.Rate == AbilityRate.AbilityPower)
                            {
                                if (Rogue.RAM.Enemy.MRS != 0) { d = d - Convert.ToInt32(Rogue.RAM.Enemy.MRS * 0.37); }
                            }
                            else
                            {
                                if (Rogue.RAM.Enemy.ARM != 0) { d = d - Convert.ToInt32(Rogue.RAM.Enemy.ARM * 0.37); }
                            }
                            if (d < 0) { d = 0; }
                            Rogue.RAM.Log.Add(Rogue.RAM.Player.Name + " использует " + this.Name + " и исцеляет " + d + " урона!");
                            DrawEngine.FightDraw.ReDrawCombatLog();
                        }
                        //if world map
                        else
                        {
                            DrawEngine.InfoWindow.Custom("Вы используете " + this.Name + " и исцеляете " + d + " урона!"); Thread.Sleep(500);
                        }
                        //effect
                        if ((Rogue.RAM.Player.CHP + d) > Rogue.RAM.Player.MHP)
                        {
                            Rogue.RAM.Player.CHP = Rogue.RAM.Player.MHP;
                        }
                        else
                        {
                            Rogue.RAM.Player.CHP += d;
                        }
                    }
                }
                #endregion
            }

            private int BasePower;
            public void UseDebuff(AbilityAction Act)
            {
                #region Instant Debuff
                if (Act.Atr.IndexOf(AbilityActionAttribute.EffectInstant) > -1)
                {
                    if (this.CanCost)
                    {
                        string stats = string.Empty;
                        foreach (AbilityStats s in this.Stats)
                        {
                            int Powder = this.Power * (-1);
                            switch (s)
                            {
                                case AbilityStats.AD: { Rogue.RAM.Enemy.AD += Powder; break; }
                                case AbilityStats.AP: { Rogue.RAM.Enemy.AP += Powder; break; }
                                case AbilityStats.ARM: { Rogue.RAM.Enemy.ARM += Powder; break; }
                                case AbilityStats.DMG: { Rogue.RAM.Enemy.MADMG += Powder; Rogue.RAM.Enemy.MIDMG += Powder; break; }
                                case AbilityStats.MHP: { Rogue.RAM.Enemy.MHP += Powder; Rogue.RAM.Enemy.CHP += Powder; break; }
                                case AbilityStats.MRS: { Rogue.RAM.Enemy.MRS += Powder; break; }
                            }
                            if (stats == string.Empty)
                            { stats += SystemEngine.Helper.String.ToString(s); }
                            else
                            { stats += "," + SystemEngine.Helper.String.ToString(s); }

                        }
                        if (Rogue.RAM.Enemy != null)
                        {
                            Rogue.RAM.Log.Add(this.Name + " уменьшает '" + stats + "' на " + this.Power + " !");
                            DrawEngine.FightDraw.ReDrawCombatLog();
                            DrawEngine.FightDraw.DrawEnemyStat();
                            //DrawEngine.GUIDraw.ReDrawCharStat();
                        }
                        this.NeedCost = true;
                    }
                    else { NeedCost = false; }
                }
                #endregion
                #region Debuff on time
                if (Act.Atr.IndexOf(AbilityActionAttribute.EffectOfTime) > -1)
                {
                    //enemy have this debuff
                    if (Rogue.RAM.Enemy.DoT.IndexOf(this) > -1)
                    {
                        if (this.Duration <= 0)
                        {
                            //base fields
                            this.Duration = this.BaseDuration;
                            //delete from debuffs
                            Rogue.RAM.Enemy.DoT.Remove(this);
                            string stats = string.Empty;
                            foreach (AbilityStats s in this.Stats)
                            {
                                int Powder = this.BasePower;
                                switch (s)
                                {
                                    case AbilityStats.AD: { Rogue.RAM.Enemy.AD += Powder; break; }
                                    case AbilityStats.AP: { Rogue.RAM.Enemy.AP += Powder; break; }
                                    case AbilityStats.ARM: { Rogue.RAM.Enemy.ARM += Powder; break; }
                                    case AbilityStats.DMG: { Rogue.RAM.Enemy.MADMG += Powder; Rogue.RAM.Enemy.MIDMG += Powder; break; }
                                    case AbilityStats.MHP: { Rogue.RAM.Enemy.MHP += Powder; Rogue.RAM.Enemy.CHP += Powder; break; }
                                    case AbilityStats.MRS: { Rogue.RAM.Enemy.MRS += Powder; break; }
                                }
                            }
                            if (Rogue.RAM.Enemy != null)
                            {
                                Rogue.RAM.Log.Add(this.Name + " заканчивает время действия !");
                                DrawEngine.FightDraw.ReDrawCombatLog();
                                DrawEngine.FightDraw.DrawEnemyStat();
                                //DrawEngine.GUIDraw.ReDrawCharStat();
                            }
                        }
                        else { NeedCost = false; }
                    }
                    //enemy HAVE NOT this debuff
                    else
                    {
                        //add base duration & base power
                        this.BaseDuration = this.Duration;
                        this.BasePower = this.Power;
                        //add to debuff
                        Rogue.RAM.Enemy.DoT.Add(this);
                        //effect
                        string stats = string.Empty;
                        foreach (AbilityStats s in this.Stats)
                        {
                            int Powder = this.BasePower * (-1);
                            switch (s)
                            {
                                case AbilityStats.AD: { Rogue.RAM.Enemy.AD += Powder; break; }
                                case AbilityStats.AP: { Rogue.RAM.Enemy.AP += Powder; break; }
                                case AbilityStats.ARM: { Rogue.RAM.Enemy.ARM += Powder; break; }
                                case AbilityStats.DMG: { Rogue.RAM.Enemy.MADMG += Powder; Rogue.RAM.Enemy.MIDMG += Powder; break; }
                                case AbilityStats.MHP: { Rogue.RAM.Enemy.MHP += Powder; Rogue.RAM.Enemy.CHP += Powder; break; }
                                case AbilityStats.MRS: { Rogue.RAM.Enemy.MRS += Powder; break; }
                            }
                            if (stats == string.Empty)
                            { stats += SystemEngine.Helper.String.ToString(s); }
                            else
                            { stats += "," + SystemEngine.Helper.String.ToString(s); }
                        }
                        if (Rogue.RAM.Enemy != null)
                        {
                            Rogue.RAM.Log.Add(this.Name + " уменьшает '" + stats + "' на " + this.BasePower + " !");
                            DrawEngine.FightDraw.ReDrawCombatLog();
                            DrawEngine.FightDraw.DrawEnemyStat();
                            //DrawEngine.GUIDraw.ReDrawCharStat();
                        }
                        //cost
                        this.NeedCost = true;
                    }
                }
                #endregion
            }
            public void UseBuff(AbilityAction Act)
            {
                #region Instant Buff
                if (Act.Atr.IndexOf(AbilityActionAttribute.EffectInstant) > -1)
                {
                    if (this.CanCost)
                    {
                        string stats = string.Empty;
                        foreach (AbilityStats s in this.Stats)
                        {
                            int Powder = this.Power;
                            switch (s)
                            {
                                case AbilityStats.AD: { Rogue.RAM.Player.AD += Powder; break; }
                                case AbilityStats.AP: { Rogue.RAM.Player.AP += Powder; break; }
                                case AbilityStats.ARM: { Rogue.RAM.Player.ARM += Powder; break; }
                                case AbilityStats.DMG: { Rogue.RAM.Player.MADMG += Powder; Rogue.RAM.Player.MIDMG += Powder; break; }
                                case AbilityStats.MHP: { Rogue.RAM.Player.MHP += Powder; Rogue.RAM.Player.CHP += Powder; break; }
                                case AbilityStats.MMP: { Rogue.RAM.Player.MMP += Powder; Rogue.RAM.Player.CMP += Powder; break; }
                                case AbilityStats.MRS: { Rogue.RAM.Player.MRS += Powder; break; }
                            }
                            if (stats == string.Empty)
                            { stats += SystemEngine.Helper.String.ToString(s); }
                            else
                            { stats += "," + SystemEngine.Helper.String.ToString(s); }
                        }
                        if (Rogue.RAM.Enemy != null)
                        {
                            Rogue.RAM.Log.Add(this.Name + " увеличивает '" + stats + "' на " + this.Power + " !");
                            DrawEngine.FightDraw.ReDrawCombatLog();
                            DrawEngine.FightDraw.DrawEnemyStat();
                            //DrawEngine.GUIDraw.ReDrawCharStat();
                        }
                        else
                        {
                            DrawEngine.InfoWindow.Warning = this.Name + " увеличивает '" + stats + "' на " + this.Power + " !";
                            //DrawEngine.GUIDraw.ReDrawCharStat();
                        }
                        NeedCost = true;
                    }
                    else { NeedCost = false; }
                }
                #endregion
                #region Buff on time
                if (Act.Atr.IndexOf(AbilityActionAttribute.EffectOfTime) > -1)
                {
                    //player have this debuff
                    if (Rogue.RAM.Player.Buffs.IndexOf(this) > -1)
                    {
                        if (this.Duration <= 0)
                        {
                            //delete from buffs
                            Rogue.RAM.Player.Buffs.Remove(this);
                            //base fields
                            this.Duration = this.BaseDuration;

                            if (this.Name == "Молитва смерти")
                            {
                                Rogue.RAM.Player.CHP = this.BaseDuration;
                                if (Rogue.RAM.Enemy != null)
                                {
                                    Rogue.RAM.Log.Add("Боги отзываются на молитву !");
                                    DrawEngine.FightDraw.ReDrawCombatLog();
                                }
                                else
                                {
                                    DrawEngine.InfoWindow.Warning = "Боги отзываются на молитву !";
                                }
                            }
                            else
                            {

                                string stats = string.Empty;
                                //next
                                foreach (AbilityStats s in this.Stats)
                                {
                                    int Powder = this.BasePower * (-1);
                                    switch (s)
                                    {
                                        case AbilityStats.AD: { Rogue.RAM.Player.AD += Powder; break; }
                                        case AbilityStats.AP: { Rogue.RAM.Player.AP += Powder; break; }
                                        case AbilityStats.ARM: { Rogue.RAM.Player.ARM += Powder; break; }
                                        case AbilityStats.DMG: { Rogue.RAM.Player.MADMG += Powder; Rogue.RAM.Player.MIDMG += Powder; break; }
                                        case AbilityStats.MHP: { Rogue.RAM.Player.MHP += Powder; Rogue.RAM.Player.CHP += Powder; break; }
                                        case AbilityStats.MMP: { Rogue.RAM.Player.MMP += Powder; Rogue.RAM.Player.CMP += Powder; break; }
                                        case AbilityStats.MRS: { Rogue.RAM.Player.MRS += Powder; break; }
                                    }
                                }
                                if (Rogue.RAM.Enemy != null)
                                {
                                    Rogue.RAM.Log.Add(this.Name + " заканчивает время действия !");
                                    DrawEngine.FightDraw.ReDrawCombatLog();
                                }
                                else { DrawEngine.InfoWindow.Warning = this.Name + " заканчивает время действия!"; }
                            }

                            //DrawEngine.GUIDraw.ReDrawCharStat();

                            if (this.Form != null)
                            {
                                Rogue.RAM.Player.Icon = Rogue.RAM.GraphHeroSet.Icon;
                                Rogue.RAM.Player.Color = Rogue.RAM.GraphHeroSet.Color;
                                DrawEngine.LabDraw.ReDrawObject();
                            }
                        }
                        else { NeedCost = false; }
                    }
                    //player HAVE NOT this debuff
                    else
                    {

                        //add base duration & base power
                        this.BaseDuration = this.Duration;
                        this.BasePower = this.Power;
                        //add to buffs
                        Rogue.RAM.Player.Buffs.Add(this);

                        //effect
                        if (this.Name == "Молитва смерти")
                        {
                            #region May Bonuses
                            int additionalrage = 0;
                            foreach (SystemEngine.ArmorSet aSet in Rogue.RAM.ArmSet)
                            {
                                if (aSet.Name == "M1" && aSet.Active)
                                {
                                    additionalrage = 1;
                                }
                            }
                            this.Duration += additionalrage;
                            additionalrage = 0;
                            foreach (SystemEngine.ArmorSet aSet in Rogue.RAM.ArmSet)
                            {
                                if (aSet.Name == "M2" && aSet.Active)
                                {
                                    additionalrage = 3;
                                }
                            }
                            this.Duration += additionalrage;
                            additionalrage = 0;
                            foreach (SystemEngine.ArmorSet aSet in Rogue.RAM.ArmSet)
                            {
                                if (aSet.Name == "M3" && aSet.Active)
                                {
                                    additionalrage = 5;
                                }
                            }
                            this.Duration += additionalrage;
                            #endregion

                            this.BaseStat = Rogue.RAM.Player.CHP;
                            if (Rogue.RAM.Enemy != null)
                            {
                                Rogue.RAM.Log.Add(Rogue.RAM.Player.Name + " читает молитву смерти !");
                                DrawEngine.FightDraw.ReDrawCombatLog();
                            }
                            else
                            {
                                DrawEngine.InfoWindow.Warning = Rogue.RAM.Player.Name + " читает молитву смерти !";
                            }
                        }
                        //effect
                        if (this.Name == "Забвение Грегори")
                        {
                            this.BaseStat = Rogue.RAM.Player.CHP;
                            this.BaseStat2 = Rogue.RAM.Player.CMP;
                            DrawEngine.InfoWindow.Message = "Грегори применяет свой навык забвения!";
                        }

                        #region Valkyrie

                        else if (this.Name == "Сфера инея")
                        {
                            Ability a = new Ability();
                            Ability b = new Ability();
                            for (int i = 0; i < Rogue.RAM.Player.Buffs.Count; i++)
                            {
                                if (Rogue.RAM.Player.Buffs[i].Name == "Сфера льда")
                                {
                                    a = Rogue.RAM.Player.Buffs[i];
                                }
                                if (Rogue.RAM.Player.Buffs[i].Name == "Сфера воды")
                                {
                                    b = Rogue.RAM.Player.Buffs[i];
                                }
                            }
                            try
                            {
                                Rogue.RAM.Player.Buffs.Remove(a);
                                Rogue.RAM.Player.Buffs.Remove(b);
                            }
                            catch { }
                        }
                        else if (this.Name == "Сфера льда")
                        {
                            Ability a = new Ability();
                            Ability b = new Ability();
                            for (int i = 0; i < Rogue.RAM.Player.Buffs.Count; i++)
                            {
                                if (Rogue.RAM.Player.Buffs[i].Name == "Сфера инея")
                                {
                                    a = Rogue.RAM.Player.Buffs[i];
                                }
                                if (Rogue.RAM.Player.Buffs[i].Name == "Сфера воды")
                                {
                                    b = Rogue.RAM.Player.Buffs[i];
                                }
                            }
                            try
                            {
                                Rogue.RAM.Player.Buffs.Remove(a);
                                Rogue.RAM.Player.Buffs.Remove(b);
                            }
                            catch { }
                        }
                        else if (this.Name == "Сфера воды")
                        {
                            Ability a = new Ability();
                            Ability b = new Ability();
                            for (int i = 0; i < Rogue.RAM.Player.Buffs.Count; i++)
                            {
                                if (Rogue.RAM.Player.Buffs[i].Name == "Сфера льда")
                                {
                                    a = Rogue.RAM.Player.Buffs[i];
                                }
                                if (Rogue.RAM.Player.Buffs[i].Name == "Сфера инея")
                                {
                                    b = Rogue.RAM.Player.Buffs[i];
                                }
                            }
                            try
                            {
                                Rogue.RAM.Player.Buffs.Remove(a);
                                Rogue.RAM.Player.Buffs.Remove(b);
                            }
                            catch { }
                        }

                        #endregion

                        else if (this.Name == "Жертва")
                        {
                            Rogue.RAM.SecondAbility = new SystemEngine.AbilityShadow() { Abilityes = Rogue.RAM.Player.Ability, Level = Rogue.RAM.Player.Ability[0].Level };
                            Rogue.RAM.Player.Ability = DataBase.EliteAbilityBase.WarlockElite;
                            foreach (Ability a in Rogue.RAM.Player.Ability)
                            {
                                for (int i = 0; i < Rogue.RAM.SecondAbility.Level; i++)
                                {
                                    a.UP();
                                }
                            }
                            DrawEngine.InfoWindow.Warning = "Вы приняли форму смерти!";
                        }
                        else
                        {
                            //effect
                            string stats = string.Empty;
                            foreach (AbilityStats s in this.Stats)
                            {
                                int Powder = this.BasePower;
                                switch (s)
                                {
                                    case AbilityStats.AD: { Rogue.RAM.Player.AD += Powder; break; }
                                    case AbilityStats.AP: { Rogue.RAM.Player.AP += Powder; break; }
                                    case AbilityStats.ARM: { Rogue.RAM.Player.ARM += Powder; break; }
                                    case AbilityStats.DMG: { Rogue.RAM.Player.MADMG += Powder; Rogue.RAM.Player.MIDMG += Powder; break; }
                                    case AbilityStats.MHP: { Rogue.RAM.Player.MHP += Powder; Rogue.RAM.Player.CHP += Powder; break; }
                                    case AbilityStats.MMP: { Rogue.RAM.Player.MMP += Powder; Rogue.RAM.Player.CMP += Powder; break; }
                                    case AbilityStats.MRS: { Rogue.RAM.Player.MRS += Powder; break; }
                                }
                                if (stats == string.Empty)
                                { stats += SystemEngine.Helper.String.ToString(s); }
                                else
                                { stats += "," + SystemEngine.Helper.String.ToString(s); }

                            }

                            if (Rogue.RAM.Enemy != null)
                            {
                                Rogue.RAM.Log.Add(this.Name + " увеличивает '" + stats + "' на " + this.BasePower + " !");
                                DrawEngine.FightDraw.ReDrawCombatLog();
                            }
                            else
                            {
                                DrawEngine.InfoWindow.Warning = this.Name + " увеличивает '" + stats + "' на " + this.BasePower + " !";
                            }
                        }

                        //DrawEngine.GUIDraw.ReDrawCharStat();

                        if (this.Form != null)
                        {
                            Rogue.RAM.Player.Icon = this.Form.Icon;
                            Rogue.RAM.Player.Color = this.Form.Color;
                            DrawEngine.LabDraw.ReDrawObject();
                        }

                        //cost
                        this.NeedCost = true;
                    }
                }
                #endregion
            }

            public void UseSummon(AbilityAction Act)
            {
                #region Summon on time
                if (Act.Atr.IndexOf(AbilityActionAttribute.EffectOfTime) > -1)
                {
                    //player have this debuff
                    if (Rogue.RAM.Player.Buffs.IndexOf(this) > -1)
                    {
                        if (this.Duration <= 0)
                        {
                            //base fields
                            this.Duration = this.BaseDuration;
                            string stats = string.Empty;
                            //delete from buffs
                            Rogue.RAM.Player.Buffs.Remove(this);

                            //summon effect
                            Rogue.RAM.SummonedList.Remove(this.SummonMonster);
                            
                            if (Rogue.RAM.Enemy != null)
                            {
                                Rogue.RAM.Log.Add(this.Name + "заканчивает время действия !");
                                DrawEngine.FightDraw.ReDrawCombatLog();
                            }
                            else { DrawEngine.InfoWindow.Warning = this.Name + " заканчивает время действия!"; }

                            NeedCost = false;
                            //DrawEngine.GUIDraw.ReDrawCharStat();
                        }
                        NeedCost = false;
                    }
                    //player HAVE NOT this debuff
                    else
                    {
                        //add base duration & base power
                        this.BaseDuration = this.Duration;
                        this.BasePower = this.Power;
                        //add to buffs
                        Rogue.RAM.Player.Buffs.Add(this);

                        //summon effect
                        Rogue.RAM.SummonedList.Add(this.SummonMonster);

                        if (Rogue.RAM.Enemy != null)
                        {
                            Rogue.RAM.Log.Add("Вы призываете " + this.SummonMonster.Name + " !");
                            DrawEngine.FightDraw.ReDrawCombatLog();
                        }
                        else
                        {
                            DrawEngine.InfoWindow.Warning = "Вы призываете " + this.SummonMonster.Name + " !";
                        }

                        //DrawEngine.GUIDraw.ReDrawCharStat();

                        //cost
                        this.NeedCost = true;
                    }
                }
                #endregion
            }

            public void UseDestruction()
            {
                if (this.Menu == "Banish") { ActivateBanishment(); }
                if (this.Menu == "Trap") { ActivateTrapPut(); }
                if (this.Name == "Столп света") { ActivateHolyNova(); }
                if (this.Name == "Молитва богам") { DrawEngine.InfoWindow.Message = "Вы молитесь богам, возможно они будут благосклонны к вам!"; }
            }

            public void UseNeutral()
            {
                ActivateSpecialization();
            }

            public void UseEot(AbilityAction Act)
            {
                if (Act.Act == AbilityActionType.Damage && Act.Atr.IndexOf(AbilityActionAttribute.DmgHealOnTime)>-1)
                {
                    if (this.Duration <= 0)
                    {
                        //убираем из дебафов
                        Rogue.RAM.Enemy.DoT.Remove(this);
                        //возвращаем время
                        this.Duration = this.BaseDuration;
                        Rogue.RAM.Log.Add(this.Name + " заканчивает своё действие!");
                        DrawEngine.FightDraw.ReDrawBuffDeBuff();
                    }
                    if (this.Name != "Смертельный яд")
                    {
                        int d = this.Power;

                        if (this.Rate == AbilityRate.AbilityPower)
                        {
                            if (Rogue.RAM.Enemy.MRS != 0) { d = d - Convert.ToInt32(Rogue.RAM.Enemy.MRS * 0.5); }
                        }
                        else
                        {
                            if (Rogue.RAM.Enemy.ARM != 0) { d = d - Convert.ToInt32(Rogue.RAM.Enemy.ARM * 0.25); }
                        }
                        //
                        if (d < 0) { d = 0; }

                        Rogue.RAM.Enemy.CHP -= d;
                        Rogue.RAM.Log.Add(this.Name + " наносит " + d.ToString() + " урона!");

                        DrawEngine.FightDraw.ReDrawCombatLog();
                        DrawEngine.FightDraw.DrawEnemyStat();
                    }
                    else
                    {
                        if (r.Next(99) < this.Level)
                        {
                            Rogue.RAM.Enemy.CHP = 0;
                            Rogue.RAM.Log.Add("Срабатывает смертельный яд!");
                            Rogue.RAM.Log.Add("Яд убил врага! Нажмите {A} для выхода!");

                        }
                    }
                }
                else if (Act.Act == AbilityActionType.Heal && Act.Atr.IndexOf(AbilityActionAttribute.DmgHealOnTime)>-1)
                {
                    //если время кончилось
                    if (this.Duration <= 0)
                    {
                        //убираем из бафов
                        Rogue.RAM.Player.Buffs.Remove(this);
                        //возвращаем время
                        this.Duration = this.BaseDuration;
                        Rogue.RAM.Log.Add(this.Name + " заканчивает своё действие!"); DrawEngine.FightDraw.ReDrawBuffDeBuff();
                    }
                    //effect
                    int d = this.Power;
                    //if combat
                    if (Rogue.RAM.Enemy != null)
                    {
                        if (this.Rate == AbilityRate.AbilityPower)
                        {
                            if (Rogue.RAM.Enemy.MRS != 0) { d = d - Convert.ToInt32(Rogue.RAM.Enemy.MRS * 0.37); }
                        }
                        else
                        {
                            if (Rogue.RAM.Enemy.ARM != 0) { d = d - Convert.ToInt32(Rogue.RAM.Enemy.ARM * 0.37); }
                        }
                        if (d < 0) { d = 0; }
                        Rogue.RAM.Log.Add(Rogue.RAM.Player.Name + " использует " + this.Name + " и исцеляет " + d + " урона!");
                        DrawEngine.FightDraw.ReDrawCombatLog();
                    }
                    //if world map
                    else
                    {
                        DrawEngine.InfoWindow.Custom("Вы используете " + this.Name + " и исцеляете " + d + " урона!"); Thread.Sleep(500);
                    }
                    //effect
                    if ((Rogue.RAM.Player.CHP + d) > Rogue.RAM.Player.MHP)
                    {
                        Rogue.RAM.Player.CHP = Rogue.RAM.Player.MHP;
                    }
                    else
                    {
                        Rogue.RAM.Player.CHP += d;
                    }
                }

                //Рисовалка
                if (Rogue.RAM.Enemy != null)
                {
                    DrawEngine.FightDraw.DrawEnemyStat();
                    DrawEngine.FightDraw.ReDrawCombatLog();
                }
            }
            public void UseIot(AbilityAction Act)
            {
                if (Act.Act == AbilityActionType.Debuff && Act.Atr.IndexOf(AbilityActionAttribute.EffectOfTime) > -1)
                {
                    if (this.Duration <= 0)
                    {
                        //base fields
                        this.Duration = this.BaseDuration;
                        //delete from debuffs
                        Rogue.RAM.Enemy.DoT.Remove(this);
                        if (this.Name == "Жертва")
                        {
                            Rogue.RAM.Player.Ability = Rogue.RAM.SecondAbility.Abilityes;
                            DrawEngine.InfoWindow.Warning = "Вы приняли обычную форму!";
                        }
                        else
                        {                            
                            string stats = string.Empty;
                            foreach (AbilityStats s in this.Stats)
                            {
                                int Powder = this.BasePower;
                                switch (s)
                                {
                                    case AbilityStats.AD: { Rogue.RAM.Enemy.AD += Powder; break; }
                                    case AbilityStats.AP: { Rogue.RAM.Enemy.AP += Powder; break; }
                                    case AbilityStats.ARM: { Rogue.RAM.Enemy.ARM += Powder; break; }
                                    case AbilityStats.DMG: { Rogue.RAM.Enemy.MADMG += Powder; Rogue.RAM.Enemy.MIDMG += Powder; break; }
                                    case AbilityStats.MHP: { Rogue.RAM.Enemy.MHP += Powder; Rogue.RAM.Enemy.CHP += Powder; break; }
                                    case AbilityStats.MRS: { Rogue.RAM.Enemy.MRS += Powder; break; }
                                }
                            }
                            if (Rogue.RAM.Enemy != null)
                            {
                                Rogue.RAM.Log.Add(this.Name + " заканчивает время действия !");
                            }
                        }
                        
                        DrawEngine.FightDraw.ReDrawBuffDeBuff();
                    }
                }
                else if (Act.Act == AbilityActionType.Improve && Act.Atr.IndexOf(AbilityActionAttribute.EffectOfTime) > -1)
                {
                    if (this.Duration <= 0)
                    {
                        //delete from buffs
                        Rogue.RAM.Player.Buffs.Remove(this);
                        //base fields
                        this.Duration = this.BaseDuration;

                        if (this.Name == "Жертва")
                        {
                            Rogue.RAM.Player.Ability = Rogue.RAM.SecondAbility.Abilityes;
                            DrawEngine.InfoWindow.Warning = "Вы приняли обычную форму!";
                        }                                
                        //Necro souls
                        else if (Rogue.RAM.Player.Class == BattleClass.Necromant && Act.Act == AbilityActionType.Summon)
                        {
                            if (Rogue.RAM.Player.CMP + this.CostRate > Rogue.RAM.Player.MMP)
                            { Rogue.RAM.Player.CMP = Rogue.RAM.Player.MMP; }
                            else
                            { Rogue.RAM.Player.CMP += Convert.ToInt32(this.CostRate); }
                        }
                        else if (this.Name == "Молитва смерти")
                        {
                            Rogue.RAM.Player.CHP = this.BaseStat;
                            if (Rogue.RAM.Enemy != null)
                            {
                                Rogue.RAM.Log.Add("Боги отзываются на молитву !");                                
                            }
                            else
                            {
                                DrawEngine.InfoWindow.Warning = "Боги отзываются на молитву !";
                            }
                        }
                        else if (this.Name == "Забвение Грегори")
                        {
                            Rogue.RAM.Player.CHP = this.BaseStat;
                            Rogue.RAM.Player.CMP = this.BaseStat2;
                            DrawEngine.InfoWindow.Warning = "Грегори больше не может контролировать вашу память!";
                        }
                        else
                        {

                            string stats = string.Empty;
                            //next
                            foreach (AbilityStats s in this.Stats)
                            {
                                int Powder = this.BasePower * (-1);
                                switch (s)
                                {
                                    case AbilityStats.AD: { Rogue.RAM.Player.AD += Powder; break; }
                                    case AbilityStats.AP: { Rogue.RAM.Player.AP += Powder; break; }
                                    case AbilityStats.ARM: { Rogue.RAM.Player.ARM += Powder; break; }
                                    case AbilityStats.DMG: { Rogue.RAM.Player.MADMG += Powder; Rogue.RAM.Player.MIDMG += Powder; break; }
                                    case AbilityStats.MHP: { Rogue.RAM.Player.MHP += Powder; Rogue.RAM.Player.CHP += Powder; break; }
                                    case AbilityStats.MMP: { Rogue.RAM.Player.MMP += Powder; Rogue.RAM.Player.CMP += Powder; break; }
                                    case AbilityStats.MRS: { Rogue.RAM.Player.MRS += Powder; break; }
                                }
                            }
                            if (Rogue.RAM.Enemy != null)
                            {
                                Rogue.RAM.Log.Add(this.Name + " заканчивает время действия !");
                            }
                            else { DrawEngine.InfoWindow.Warning = this.Name + " заканчивает время действия!"; }
                        }

                        //DrawEngine.GUIDraw.ReDrawCharStat();

                        if (this.Form != null)
                        {
                            Rogue.RAM.Player.Icon = Rogue.RAM.GraphHeroSet.Icon;
                            Rogue.RAM.Player.Color = Rogue.RAM.GraphHeroSet.Color;
                            DrawEngine.LabDraw.ReDrawObject();
                        }
                        DrawEngine.FightDraw.ReDrawBuffDeBuff();
                    }
                }

                if (Rogue.RAM.Enemy != null)
                {
                    DrawEngine.FightDraw.DrawEnemyStat();
                    DrawEngine.FightDraw.ReDrawCombatLog();
                }                
            }
            public void UseSot(AbilityAction Act)
            {
                if (Act.Atr.IndexOf(AbilityActionAttribute.EffectOfTime) > -1)
                {
                    //player have this debuff
                    if (Rogue.RAM.Player.Buffs.IndexOf(this) > -1)
                    {
                        if (this.Duration <= 0)
                        {
                            //base fields
                            this.Duration = this.BaseDuration;
                            string stats = string.Empty;
                            //delete from buffs
                            Rogue.RAM.Player.Buffs.Remove(this);

                            //summon effect
                            Rogue.RAM.SummonedList.Remove(this.SummonMonster);

                            if (Rogue.RAM.Enemy != null)
                            {
                                Rogue.RAM.Log.Add(this.Name + "заканчивает время действия !");
                                DrawEngine.FightDraw.ReDrawCombatLog();
                            }
                            else { DrawEngine.InfoWindow.Warning = this.Name + " заканчивает время действия!"; }

                            NeedCost = false;
                            //DrawEngine.GUIDraw.ReDrawCharStat();
                            DrawEngine.FightDraw.ReDrawBuffDeBuff();
                        }
                        NeedCost = false;
                    }
                }
            }



            /// <summary>
            /// Alchemist passive power of Elemental helpers
            /// </summary>
            private void AlchemistPassiveBuff()
            {
                foreach (Ability a in Rogue.RAM.Player.Ability)
                {
                    if (a.Name == "Алхимия")
                    {
                        if (a.Level == 0) { return; }
                        else
                        {
                            foreach (Ability buff in Rogue.RAM.Player.Buffs)
                            {
                                if (buff.Name == "Элементализм") { return; }
                            }
                            
                            int rand = r.Next();
                            Thread.Sleep(10);
                            rand = r.Next(4);
                            switch (rand)
                            {
                                case 0:
                                    {
                                        Ability aby = DataBase.BattleAbilityBase.Elementalism;
                                        aby.Stats.Add(AbilityStats.DMG);
                                        aby._Power = a.Level;
                                        aby.Action[0] = new AbilityAction() { Act = AbilityActionType.Improve, Atr = new List<AbilityActionAttribute>() { AbilityActionAttribute.EffectOfTime } };
                                        aby.Color = ConsoleColor.Red;
                                        aby.Activate();
                                        break;
                                    }
                                case 1:
                                    {
                                        Ability aby = DataBase.BattleAbilityBase.Elementalism;
                                        aby.Stats.Add(AbilityStats.MHP);
                                        aby._Power = a.Level;
                                        aby.Action[0] = new AbilityAction() { Act = AbilityActionType.Heal, Atr = new List<AbilityActionAttribute>() { AbilityActionAttribute.EffectOfTime } };
                                        aby.Duration = 0;
                                        aby.Color = ConsoleColor.Blue;
                                        aby.Activate();
                                        break;
                                    }
                                case 2:
                                    {
                                        Ability aby = DataBase.BattleAbilityBase.Elementalism;
                                        aby.Stats.Add(AbilityStats.MMP);
                                        aby._Power = a.Level;
                                        aby.Action[0] = new AbilityAction() { Act = AbilityActionType.Improve, Atr = new List<AbilityActionAttribute>() { AbilityActionAttribute.EffectOfTime } };
                                        aby.Color = ConsoleColor.Cyan;
                                        aby.Activate();
                                        break;
                                    }
                                case 3:
                                    {
                                        Ability aby = DataBase.BattleAbilityBase.Elementalism;
                                        aby.Stats.Add( AbilityStats.ARM);
                                        aby._Power = a.Level;
                                        aby.Action[0] = new AbilityAction() { Act = AbilityActionType.Improve, Atr = new List<AbilityActionAttribute>() { AbilityActionAttribute.EffectOfTime } };
                                        aby.Color = ConsoleColor.DarkYellow;
                                        aby.Activate();
                                        break;
                                    }
                            }
                        }
                    }
                }
            }
        
            /// <summary>
            /// Special for 'bunishment' inquisitor
            /// </summary>
            private void ActivateBanishment()
            {
                int xpos = 0;
                int ypos = 0;

                for (int y = 0; y < 23; y++)
                {
                    for (int x = 0; x < 71; x++)
                    {
                        if (Rogue.RAM.Map.Map[x][y].Player != null)
                        {
                            xpos = x;
                            ypos = y;
                        }
                    }
                }
                switch (this.Direction)
                {
                    case SystemEngine.ArrowDirection.Top: { this.Banish(xpos, ypos-1); break; }
                    case SystemEngine.ArrowDirection.Bot: { this.Banish(xpos, ypos+1); break; }
                    case SystemEngine.ArrowDirection.Left: { this.Banish(xpos-1, ypos); break; }
                    case SystemEngine.ArrowDirection.Right: { this.Banish(xpos + 1, ypos); break; }
                }
            }
            public SystemEngine.ArrowDirection Direction;
            public bool GoodEvilAbility;
            private void Banish(int x, int y)
            {
                if (Rogue.RAM.Map.Map[x][y].Enemy != null)
                {
                    if (!GoodEvilAbility)
                    {

                        if (Rogue.RAM.Map.Map[x][y].Enemy.Race == MonsterRace.Drow || Rogue.RAM.Map.Map[x][y].Enemy.Race == MonsterRace.Human || Rogue.RAM.Map.Map[x][y].Enemy.Race == MonsterRace.Undead)
                        {
                            Rogue.RAM.Map.Map[x][y].Item = Rogue.RAM.Map.Map[x][y].Enemy.Loot;
                            Rogue.RAM.Map.Map[x][y].Enemy = null;
                            DrawEngine.DigitalArt.Banishment(x + 2, y + 1);
                            this.NeedCost = true;
                            DrawEngine.InfoWindow.Custom("Вы изгоняете существо!");
                        }
                        else { DrawEngine.InfoWindow.Custom("Цель противоположного мировоззрения!"); }
                    }
                    else
                    {
                        if (Rogue.RAM.Map.Map[x][y].Enemy.Race == MonsterRace.Animal || Rogue.RAM.Map.Map[x][y].Enemy.Race == MonsterRace.Avariel || Rogue.RAM.Map.Map[x][y].Enemy.Race == MonsterRace.Dragon)
                        {
                            Rogue.RAM.Map.Map[x][y].Item = Rogue.RAM.Map.Map[x][y].Enemy.Loot;
                            Rogue.RAM.Map.Map[x][y].Enemy = null;
                            DrawEngine.DigitalArt.Banishment(x + 2, y + 1);
                            this.NeedCost = true;
                            DrawEngine.InfoWindow.Custom("Вы изгоняете существо!");
                        }
                        else { DrawEngine.InfoWindow.Custom("Цель противоположного мировоззрения!"); }
                    }
                }
                else { DrawEngine.InfoWindow.Custom("В этом месте нет врага для изгнания!"); }
                Thread.Sleep(1000);
                PlayEngine.EnemyMoves.Move(true);
            }
            /// <summary>
            /// Special for 'trap' assasin
            /// </summary>
            private void ActivateTrapPut()
            {
                var Lab = Rogue.RAM.Map;

                int xpos = 0;
                int ypos = 0;

                for (int y = 0; y < 23; y++)
                {
                    for (int x = 0; x < 71; x++)
                    {
                        if (Rogue.RAM.Map.Map[x][y].Player != null)
                        {
                            xpos = x;
                            ypos = y;
                        }
                    }
                }
                switch (this.Direction)
                {
                    case SystemEngine.ArrowDirection.Top: { this.PutTrap(xpos, ypos - 1); break; }
                    case SystemEngine.ArrowDirection.Bot: { this.PutTrap(xpos, ypos + 1); break; }
                    case SystemEngine.ArrowDirection.Left: { this.PutTrap(xpos - 1, ypos); break; }
                    case SystemEngine.ArrowDirection.Right: { this.PutTrap(xpos + 1, ypos); break; }
                }
            }
            /// <summary>
            /// Put one of 4 traps
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            private void PutTrap(int x, int y)
            {
                if (Rogue.RAM.Map.Map[x][y].Vision == ' ')
                {
                    Trap Trap = new Trap();
                    Trap.Damage = this.Power; 
                    switch (this.TypeOfTrap)
                    {
                        case 0: { Trap.Element = AbilityElement.Physical; Trap.Name = "Механическая ловушка"; break; }
                        case 1: { Trap.Element = AbilityElement.ElementalMagic; Trap.Name = "Магическая ловушка"; break; }
                        case 2: { Trap.Element = AbilityElement.FireMagic; Trap.Name = "Огненная ловушка"; break; }
                        case 3: { Trap.Element = AbilityElement.NatureMagic; Trap.Name = "Ядовитая ловушка"; break; }
                        default: { Trap.Element = AbilityElement.Physical; Trap.Name = "Механическая ловушка"; break; }
                    }
                    Rogue.RAM.Map.Map[x][y].Trap = Trap;
                    DrawEngine.InfoWindow.Custom("Вы устанавливаете: " + Trap.Name);
                    this.NeedCost = true;
                    DrawEngine.GUIDraw.DrawLab();
                }
                else { DrawEngine.InfoWindow.Custom("В это место нельзя установить ловушку!"); }
                Thread.Sleep(1000);
                if (!PlayEngine.EnemyMoves.CheckMove) { PlayEngine.EnemyMoves.Move(true); }
            }
            /// <summary>
            /// Type of trap:
            /// 1 - Physical
            /// 2 - Magical
            /// 3 - Fire
            /// 4 - Poison
            /// </summary>
            public int TypeOfTrap;
            /// <summary>
            /// Special for holy nova
            /// </summary>
            private void ActivateHolyNova()
            {
                var Lab = Rogue.RAM.Map;

                int truedamage = 0;
                int truheal = 0;

                if ((Rogue.RAM.Player.CHP + truheal) > Rogue.RAM.Player.MHP) { Rogue.RAM.Player.CHP = Rogue.RAM.Player.MHP; }
                else { Rogue.RAM.Player.CHP += truheal; }

                int truewasdmg = 0;

                for (int y = 0; y < 23; y++)
                {
                    for (int x = 0; x < 71; x++)
                    {
                        if (Rogue.RAM.Map.Map[x][y].Player != null)
                        {
                            if (Rogue.RAM.Map.Map[x + 1][y].Enemy != null)
                            {
                                if (this.Rate == AbilityRate.AbilityPower) { truedamage = Math.Abs((Rogue.RAM.Map.Map[x + 1][y].Enemy.MRS / 2) - this.Power); }
                                else { truedamage = Math.Abs((Rogue.RAM.Map.Map[x + 1][y].Enemy.ARM / 2) - this.Power); }
                                truewasdmg += truedamage;
                                Rogue.RAM.Map.Map[x + 1][y].Enemy.CHP -= truedamage;
                                if (Rogue.RAM.Map.Map[x + 1][y].Enemy.CHP <= 0)
                                {
                                    Rogue.RAM.Map.Map[x + 1][y].Item = Rogue.RAM.Map.Map[x + 1][y].Enemy.Loot;
                                    Rogue.RAM.Map.Map[x + 1][y].Enemy = null;
                                }

                            }
                            if (Rogue.RAM.Map.Map[x - 1][y].Enemy != null)
                            {
                                if (this.Rate == AbilityRate.AbilityPower) { truedamage = Math.Abs((Rogue.RAM.Map.Map[x - 1][y].Enemy.MRS / 2) - this.Power); }
                                else { truedamage = Math.Abs((Rogue.RAM.Map.Map[x - 1][y].Enemy.ARM / 2) - this.Power); }
                                truewasdmg += truedamage;
                                Rogue.RAM.Map.Map[x - 1][y].Enemy.CHP -= truedamage;
                                if (Rogue.RAM.Map.Map[x - 1][y].Enemy.CHP <= 0)
                                {
                                    Rogue.RAM.Map.Map[x - 1][y].Item = Rogue.RAM.Map.Map[x - 1][y].Enemy.Loot;
                                    Rogue.RAM.Map.Map[x - 1][y].Enemy = null;
                                }
                            }
                            if (Rogue.RAM.Map.Map[x][y + 1].Enemy != null)
                            {
                                if (this.Rate == AbilityRate.AbilityPower) { truedamage = Math.Abs((Rogue.RAM.Map.Map[x][y + 1].Enemy.MRS / 2) - this.Power); }
                                else { truedamage = Math.Abs((Rogue.RAM.Map.Map[x][y + 1].Enemy.ARM / 2) - this.Power); }
                                truewasdmg += truedamage;
                                Rogue.RAM.Map.Map[x][y + 1].Enemy.CHP -= truedamage;
                                if (Rogue.RAM.Map.Map[x][y + 1].Enemy.CHP <= 0)
                                {
                                    Rogue.RAM.Map.Map[x][y + 1].Item = Rogue.RAM.Map.Map[x][y + 1].Enemy.Loot;
                                    Rogue.RAM.Map.Map[x][y + 1].Enemy = null;
                                }
                            }
                            if (Rogue.RAM.Map.Map[x][y - 1].Enemy != null)
                            {
                                if (this.Rate == AbilityRate.AbilityPower) { truedamage = Math.Abs((Rogue.RAM.Map.Map[x][y - 1].Enemy.MRS / 2) - this.Power); }
                                else { truedamage = Math.Abs((Rogue.RAM.Map.Map[x][y - 1].Enemy.ARM / 2) - this.Power); }
                                truewasdmg += truedamage;
                                Rogue.RAM.Map.Map[x][y - 1].Enemy.CHP -= truedamage;
                                if (Rogue.RAM.Map.Map[x][y - 1].Enemy.CHP <= 0)
                                {
                                    Rogue.RAM.Map.Map[x][y - 1].Item = Rogue.RAM.Map.Map[x][y - 1].Enemy.Loot;
                                    Rogue.RAM.Map.Map[x][y - 1].Enemy = null;
                                }
                            }
                            if (Rogue.RAM.Map.Map[x + 1][y + 1].Enemy != null)
                            {
                                if (this.Rate == AbilityRate.AbilityPower) { truedamage = Math.Abs((Rogue.RAM.Map.Map[x + 1][y + 1].Enemy.MRS / 2) - this.Power); }
                                else { truedamage = Math.Abs((Rogue.RAM.Map.Map[x + 1][y + 1].Enemy.ARM / 2) - this.Power); }
                                truewasdmg += truedamage;
                                Rogue.RAM.Map.Map[x + 1][y + 1].Enemy.CHP -= truedamage;
                                if (Rogue.RAM.Map.Map[x + 1][y + 1].Enemy.CHP <= 0)
                                {
                                    Rogue.RAM.Map.Map[x + 1][y + 1].Item = Rogue.RAM.Map.Map[x + 1][y + 1].Enemy.Loot;
                                    Rogue.RAM.Map.Map[x + 1][y + 1].Enemy = null;
                                }
                            }
                            if (Rogue.RAM.Map.Map[x - 1][y - 1].Enemy != null)
                            {
                                if (this.Rate == AbilityRate.AbilityPower) { truedamage = Math.Abs((Rogue.RAM.Map.Map[x - 1][y - 1].Enemy.MRS / 2) - this.Power); }
                                else { truedamage = Math.Abs((Rogue.RAM.Map.Map[x - 1][y - 1].Enemy.ARM / 2) - this.Power); }
                                truewasdmg += truedamage;
                                Rogue.RAM.Map.Map[x - 1][y - 1].Enemy.CHP -= truedamage;
                                if (Rogue.RAM.Map.Map[x - 1][y - 1].Enemy.CHP <= 0)
                                {
                                    Rogue.RAM.Map.Map[x - 1][y - 1].Item = Rogue.RAM.Map.Map[x - 1][y - 1].Enemy.Loot;
                                    Rogue.RAM.Map.Map[x - 1][y - 1].Enemy = null;
                                }
                            }
                            if (Rogue.RAM.Map.Map[x + 1][y - 1].Enemy != null)
                            {
                                if (this.Rate == AbilityRate.AbilityPower) { truedamage = Math.Abs((Rogue.RAM.Map.Map[x + 1][y - 1].Enemy.MRS / 2) - this.Power); }
                                else { truedamage = Math.Abs((Rogue.RAM.Map.Map[x + 1][y - 1].Enemy.ARM / 2) - this.Power); }
                                truewasdmg += truedamage;
                                Rogue.RAM.Map.Map[x + 1][y - 1].Enemy.CHP -= truedamage;
                                if (Rogue.RAM.Map.Map[x + 1][y - 1].Enemy.CHP <= 0)
                                {
                                    Rogue.RAM.Map.Map[x + 1][y - 1].Item = Rogue.RAM.Map.Map[x + 1][y - 1].Enemy.Loot;
                                    Rogue.RAM.Map.Map[x + 1][y - 1].Enemy = null;
                                }
                            }
                            if (Rogue.RAM.Map.Map[x - 1][y + 1].Enemy != null)
                            {
                                if (this.Rate == AbilityRate.AbilityPower) { truedamage = Math.Abs((Rogue.RAM.Map.Map[x - 1][y + 1].Enemy.MRS / 2) - this.Power); }
                                else { truedamage = Math.Abs((Rogue.RAM.Map.Map[x - 1][y + 1].Enemy.ARM / 2) - this.Power); }
                                truewasdmg += truedamage;
                                Rogue.RAM.Map.Map[x - 1][y + 1].Enemy.CHP -= truedamage;
                                if (Rogue.RAM.Map.Map[x - 1][y + 1].Enemy.CHP <= 0)
                                {
                                    Rogue.RAM.Map.Map[x - 1][y + 1].Item = Rogue.RAM.Map.Map[x - 1][y + 1].Enemy.Loot;
                                    Rogue.RAM.Map.Map[x - 1][y + 1].Enemy = null;
                                }
                            }
                            if (PlayEngine.EnemyMoves.CheckMove) { PlayEngine.EnemyMoves.Move(false); }
                            DrawEngine.DigitalArt.HolyNova(x + 2, y + 1);
                            if (!PlayEngine.EnemyMoves.CheckMove) { PlayEngine.EnemyMoves.Move(true); }
                        }
                    }
                }
                DrawEngine.InfoWindow.Custom("<<Столп света>> Суммарно нанесённый урон: " + truewasdmg.ToString() + "  Исцелено: " + truheal);
                this.NeedCost = true;
                Thread.Sleep(750);                
            }

            /// <summary>
            /// Activate fire shield at world map, do effect
            /// </summary>
            private void UseFireShield()
            {
                var Lab = Rogue.RAM.Map;

                int truedamage = 0;

                int truewasdmg = 0;

                for (int y = 0; y < 23; y++)
                {
                    for (int x = 0; x < 71; x++)
                    {
                        if (Rogue.RAM.Map.Map[x][y].Player != null)
                        {
                            if (Rogue.RAM.Map.Map[x + 1][y].Enemy != null)
                            {
                                if (this.Rate == AbilityRate.AbilityPower) { truedamage = Math.Abs((Rogue.RAM.Map.Map[x + 1][y].Enemy.MRS / 2) - this.Power); }
                                else { truedamage = Math.Abs((Rogue.RAM.Map.Map[x + 1][y].Enemy.ARM / 2) - this.Power); }
                                truewasdmg += truedamage;
                                Rogue.RAM.Map.Map[x + 1][y].Enemy.CHP -= truedamage;
                                if (Rogue.RAM.Map.Map[x + 1][y].Enemy.CHP <= 0)
                                {
                                    Rogue.RAM.Map.Map[x + 1][y].Item = Rogue.RAM.Map.Map[x + 1][y].Enemy.Loot;
                                    Rogue.RAM.Map.Map[x + 1][y].Enemy = null;
                                }

                            }
                            if (Rogue.RAM.Map.Map[x - 1][y].Enemy != null)
                            {
                                if (this.Rate == AbilityRate.AbilityPower) { truedamage = Math.Abs((Rogue.RAM.Map.Map[x - 1][y].Enemy.MRS / 2) - this.Power); }
                                else { truedamage = Math.Abs((Rogue.RAM.Map.Map[x - 1][y].Enemy.ARM / 2) - this.Power); }
                                truewasdmg += truedamage;
                                Rogue.RAM.Map.Map[x - 1][y].Enemy.CHP -= truedamage;
                                if (Rogue.RAM.Map.Map[x - 1][y].Enemy.CHP <= 0)
                                {
                                    Rogue.RAM.Map.Map[x - 1][y].Item = Rogue.RAM.Map.Map[x - 1][y].Enemy.Loot;
                                    Rogue.RAM.Map.Map[x - 1][y].Enemy = null;
                                }
                            }
                            if (Rogue.RAM.Map.Map[x][y + 1].Enemy != null)
                            {
                                if (this.Rate == AbilityRate.AbilityPower) { truedamage = Math.Abs((Rogue.RAM.Map.Map[x][y + 1].Enemy.MRS / 2) - this.Power); }
                                else { truedamage = Math.Abs((Rogue.RAM.Map.Map[x][y + 1].Enemy.ARM / 2) - this.Power); }
                                truewasdmg += truedamage;
                                Rogue.RAM.Map.Map[x][y + 1].Enemy.CHP -= truedamage;
                                if (Rogue.RAM.Map.Map[x][y + 1].Enemy.CHP <= 0)
                                {
                                    Rogue.RAM.Map.Map[x][y + 1].Item = Rogue.RAM.Map.Map[x][y + 1].Enemy.Loot;
                                    Rogue.RAM.Map.Map[x][y + 1].Enemy = null;
                                }
                            }
                            if (Rogue.RAM.Map.Map[x][y - 1].Enemy != null)
                            {
                                if (this.Rate == AbilityRate.AbilityPower) { truedamage = Math.Abs((Rogue.RAM.Map.Map[x][y - 1].Enemy.MRS / 2) - this.Power); }
                                else { truedamage = Math.Abs((Rogue.RAM.Map.Map[x][y - 1].Enemy.ARM / 2) - this.Power); }
                                truewasdmg += truedamage;
                                Rogue.RAM.Map.Map[x][y - 1].Enemy.CHP -= truedamage;
                                if (Rogue.RAM.Map.Map[x][y - 1].Enemy.CHP <= 0)
                                {
                                    Rogue.RAM.Map.Map[x][y - 1].Item = Rogue.RAM.Map.Map[x][y - 1].Enemy.Loot;
                                    Rogue.RAM.Map.Map[x][y - 1].Enemy = null;
                                }
                            }
                            if (Rogue.RAM.Map.Map[x + 1][y + 1].Enemy != null)
                            {
                                if (this.Rate == AbilityRate.AbilityPower) { truedamage = Math.Abs((Rogue.RAM.Map.Map[x + 1][y + 1].Enemy.MRS / 2) - this.Power); }
                                else { truedamage = Math.Abs((Rogue.RAM.Map.Map[x + 1][y + 1].Enemy.ARM / 2) - this.Power); }
                                truewasdmg += truedamage;
                                Rogue.RAM.Map.Map[x + 1][y + 1].Enemy.CHP -= truedamage;
                                if (Rogue.RAM.Map.Map[x + 1][y + 1].Enemy.CHP <= 0)
                                {
                                    Rogue.RAM.Map.Map[x + 1][y + 1].Item = Rogue.RAM.Map.Map[x + 1][y + 1].Enemy.Loot;
                                    Rogue.RAM.Map.Map[x + 1][y + 1].Enemy = null;
                                }
                            }
                            if (Rogue.RAM.Map.Map[x - 1][y - 1].Enemy != null)
                            {
                                if (this.Rate == AbilityRate.AbilityPower) { truedamage = Math.Abs((Rogue.RAM.Map.Map[x - 1][y - 1].Enemy.MRS / 2) - this.Power); }
                                else { truedamage = Math.Abs((Rogue.RAM.Map.Map[x - 1][y - 1].Enemy.ARM / 2) - this.Power); }
                                truewasdmg += truedamage;
                                Rogue.RAM.Map.Map[x - 1][y - 1].Enemy.CHP -= truedamage;
                                if (Rogue.RAM.Map.Map[x - 1][y - 1].Enemy.CHP <= 0)
                                {
                                    Rogue.RAM.Map.Map[x - 1][y - 1].Item = Rogue.RAM.Map.Map[x - 1][y - 1].Enemy.Loot;
                                    Rogue.RAM.Map.Map[x - 1][y - 1].Enemy = null;
                                }
                            }
                            if (Rogue.RAM.Map.Map[x + 1][y - 1].Enemy != null)
                            {
                                if (this.Rate == AbilityRate.AbilityPower) { truedamage = Math.Abs((Rogue.RAM.Map.Map[x + 1][y - 1].Enemy.MRS / 2) - this.Power); }
                                else { truedamage = Math.Abs((Rogue.RAM.Map.Map[x + 1][y - 1].Enemy.ARM / 2) - this.Power); }
                                truewasdmg += truedamage;
                                Rogue.RAM.Map.Map[x + 1][y - 1].Enemy.CHP -= truedamage;
                                if (Rogue.RAM.Map.Map[x + 1][y - 1].Enemy.CHP <= 0)
                                {
                                    Rogue.RAM.Map.Map[x + 1][y - 1].Item = Rogue.RAM.Map.Map[x + 1][y - 1].Enemy.Loot;
                                    Rogue.RAM.Map.Map[x + 1][y - 1].Enemy = null;
                                }
                            }
                            if (Rogue.RAM.Map.Map[x - 1][y + 1].Enemy != null)
                            {
                                if (this.Rate == AbilityRate.AbilityPower) { truedamage = Math.Abs((Rogue.RAM.Map.Map[x - 1][y + 1].Enemy.MRS / 2) - this.Power); }
                                else { truedamage = Math.Abs((Rogue.RAM.Map.Map[x - 1][y + 1].Enemy.ARM / 2) - this.Power); }
                                truewasdmg += truedamage;
                                Rogue.RAM.Map.Map[x - 1][y + 1].Enemy.CHP -= truedamage;
                                if (Rogue.RAM.Map.Map[x - 1][y + 1].Enemy.CHP <= 0)
                                {
                                    Rogue.RAM.Map.Map[x - 1][y + 1].Item = Rogue.RAM.Map.Map[x - 1][y + 1].Enemy.Loot;
                                    Rogue.RAM.Map.Map[x - 1][y + 1].Enemy = null;
                                }
                            }
                            if (PlayEngine.EnemyMoves.CheckMove) { PlayEngine.EnemyMoves.Move(false); }
                            DrawEngine.DigitalArt.FireShield(x + 2, y + 1);
                            if (!PlayEngine.EnemyMoves.CheckMove) { PlayEngine.EnemyMoves.Move(true); }
                        }
                    }
                }
                DrawEngine.InfoWindow.Custom("<<Огненный щит>> Суммарно нанесённый урон: " + truewasdmg.ToString());                
                Thread.Sleep(750);
            }
            /// <summary>
            /// Sppecial for poison nova
            /// </summary>
            private void UsePoisonNova()
            {
                var Lab = Rogue.RAM.Map;

                int truedamage = 0;

                int truewasdmg = 0;

                for (int y = 0; y < 23; y++)
                {
                    for (int x = 0; x < 71; x++)
                    {
                        if (Rogue.RAM.Map.Map[x][y].Player != null)
                        {
                            if (Rogue.RAM.Map.Map[x + 1][y].Enemy != null)
                            {
                                if (this.Rate == AbilityRate.AbilityPower) { truedamage = Math.Abs((Rogue.RAM.Map.Map[x + 1][y].Enemy.MRS / 2) - this.Power); }
                                else { truedamage = Math.Abs((Rogue.RAM.Map.Map[x + 1][y].Enemy.ARM / 2) - this.Power); }
                                truewasdmg += truedamage;
                                Rogue.RAM.Map.Map[x + 1][y].Enemy.CHP -= truedamage;
                                if (Rogue.RAM.Map.Map[x + 1][y].Enemy.CHP <= 0)
                                {
                                    Rogue.RAM.Map.Map[x + 1][y].Item = Rogue.RAM.Map.Map[x + 1][y].Enemy.Loot;
                                    Rogue.RAM.Map.Map[x + 1][y].Enemy = null;
                                }

                            }
                            if (Rogue.RAM.Map.Map[x - 1][y].Enemy != null)
                            {
                                if (this.Rate == AbilityRate.AbilityPower) { truedamage = Math.Abs((Rogue.RAM.Map.Map[x - 1][y].Enemy.MRS / 2) - this.Power); }
                                else { truedamage = Math.Abs((Rogue.RAM.Map.Map[x - 1][y].Enemy.ARM / 2) - this.Power); }
                                truewasdmg += truedamage;
                                Rogue.RAM.Map.Map[x - 1][y].Enemy.CHP -= truedamage;
                                if (Rogue.RAM.Map.Map[x - 1][y].Enemy.CHP <= 0)
                                {
                                    Rogue.RAM.Map.Map[x - 1][y].Item = Rogue.RAM.Map.Map[x - 1][y].Enemy.Loot;
                                    Rogue.RAM.Map.Map[x - 1][y].Enemy = null;
                                }
                            }
                            if (Rogue.RAM.Map.Map[x][y + 1].Enemy != null)
                            {
                                if (this.Rate == AbilityRate.AbilityPower) { truedamage = Math.Abs((Rogue.RAM.Map.Map[x][y + 1].Enemy.MRS / 2) - this.Power); }
                                else { truedamage = Math.Abs((Rogue.RAM.Map.Map[x][y + 1].Enemy.ARM / 2) - this.Power); }
                                truewasdmg += truedamage;
                                Rogue.RAM.Map.Map[x][y + 1].Enemy.CHP -= truedamage;
                                if (Rogue.RAM.Map.Map[x][y + 1].Enemy.CHP <= 0)
                                {
                                    Rogue.RAM.Map.Map[x][y + 1].Item = Rogue.RAM.Map.Map[x][y + 1].Enemy.Loot;
                                    Rogue.RAM.Map.Map[x][y + 1].Enemy = null;
                                }
                            }
                            if (Rogue.RAM.Map.Map[x][y - 1].Enemy != null)
                            {
                                if (this.Rate == AbilityRate.AbilityPower) { truedamage = Math.Abs((Rogue.RAM.Map.Map[x][y - 1].Enemy.MRS / 2) - this.Power); }
                                else { truedamage = Math.Abs((Rogue.RAM.Map.Map[x][y - 1].Enemy.ARM / 2) - this.Power); }
                                truewasdmg += truedamage;
                                Rogue.RAM.Map.Map[x][y - 1].Enemy.CHP -= truedamage;
                                if (Rogue.RAM.Map.Map[x][y - 1].Enemy.CHP <= 0)
                                {
                                    Rogue.RAM.Map.Map[x][y - 1].Item = Rogue.RAM.Map.Map[x][y - 1].Enemy.Loot;
                                    Rogue.RAM.Map.Map[x][y - 1].Enemy = null;
                                }
                            }
                            if (Rogue.RAM.Map.Map[x + 1][y + 1].Enemy != null)
                            {
                                if (this.Rate == AbilityRate.AbilityPower) { truedamage = Math.Abs((Rogue.RAM.Map.Map[x + 1][y + 1].Enemy.MRS / 2) - this.Power); }
                                else { truedamage = Math.Abs((Rogue.RAM.Map.Map[x + 1][y + 1].Enemy.ARM / 2) - this.Power); }
                                truewasdmg += truedamage;
                                Rogue.RAM.Map.Map[x + 1][y + 1].Enemy.CHP -= truedamage;
                                if (Rogue.RAM.Map.Map[x + 1][y + 1].Enemy.CHP <= 0)
                                {
                                    Rogue.RAM.Map.Map[x + 1][y + 1].Item = Rogue.RAM.Map.Map[x + 1][y + 1].Enemy.Loot;
                                    Rogue.RAM.Map.Map[x + 1][y + 1].Enemy = null;
                                }
                            }
                            if (Rogue.RAM.Map.Map[x - 1][y - 1].Enemy != null)
                            {
                                if (this.Rate == AbilityRate.AbilityPower) { truedamage = Math.Abs((Rogue.RAM.Map.Map[x - 1][y - 1].Enemy.MRS / 2) - this.Power); }
                                else { truedamage = Math.Abs((Rogue.RAM.Map.Map[x - 1][y - 1].Enemy.ARM / 2) - this.Power); }
                                truewasdmg += truedamage;
                                Rogue.RAM.Map.Map[x - 1][y - 1].Enemy.CHP -= truedamage;
                                if (Rogue.RAM.Map.Map[x - 1][y - 1].Enemy.CHP <= 0)
                                {
                                    Rogue.RAM.Map.Map[x - 1][y - 1].Item = Rogue.RAM.Map.Map[x - 1][y - 1].Enemy.Loot;
                                    Rogue.RAM.Map.Map[x - 1][y - 1].Enemy = null;
                                }
                            }
                            if (Rogue.RAM.Map.Map[x + 1][y - 1].Enemy != null)
                            {
                                if (this.Rate == AbilityRate.AbilityPower) { truedamage = Math.Abs((Rogue.RAM.Map.Map[x + 1][y - 1].Enemy.MRS / 2) - this.Power); }
                                else { truedamage = Math.Abs((Rogue.RAM.Map.Map[x + 1][y - 1].Enemy.ARM / 2) - this.Power); }
                                truewasdmg += truedamage;
                                Rogue.RAM.Map.Map[x + 1][y - 1].Enemy.CHP -= truedamage;
                                if (Rogue.RAM.Map.Map[x + 1][y - 1].Enemy.CHP <= 0)
                                {
                                    Rogue.RAM.Map.Map[x + 1][y - 1].Item = Rogue.RAM.Map.Map[x + 1][y - 1].Enemy.Loot;
                                    Rogue.RAM.Map.Map[x + 1][y - 1].Enemy = null;
                                }
                            }
                            if (Rogue.RAM.Map.Map[x - 1][y + 1].Enemy != null)
                            {
                                if (this.Rate == AbilityRate.AbilityPower) { truedamage = Math.Abs((Rogue.RAM.Map.Map[x - 1][y + 1].Enemy.MRS / 2) - this.Power); }
                                else { truedamage = Math.Abs((Rogue.RAM.Map.Map[x - 1][y + 1].Enemy.ARM / 2) - this.Power); }
                                truewasdmg += truedamage;
                                Rogue.RAM.Map.Map[x - 1][y + 1].Enemy.CHP -= truedamage;
                                if (Rogue.RAM.Map.Map[x - 1][y + 1].Enemy.CHP <= 0)
                                {
                                    Rogue.RAM.Map.Map[x - 1][y + 1].Item = Rogue.RAM.Map.Map[x - 1][y + 1].Enemy.Loot;
                                    Rogue.RAM.Map.Map[x - 1][y + 1].Enemy = null;
                                }
                            }
                            if (PlayEngine.EnemyMoves.CheckMove) { PlayEngine.EnemyMoves.Move(false); }
                            DrawEngine.DigitalArt.PoisonNova(x + 2, y + 1);
                            if (!PlayEngine.EnemyMoves.CheckMove) { PlayEngine.EnemyMoves.Move(true); }
                        }
                    }
                }
                DrawEngine.InfoWindow.Custom("<<Кольцо яда>> Суммарно нанесённый урон: " + truewasdmg.ToString());
                Thread.Sleep(750);
            }

            private void ActivateSpecialization()
            {
                if (Rogue.RAM.Player.MonkSpec == AbilityRate.AttackDamage)
                { Rogue.RAM.Player.MonkSpec = AbilityRate.AbilityPower; DrawEngine.InfoWindow.Warning = "Вы вступили на путь мудрости."; }
                else
                { Rogue.RAM.Player.MonkSpec = AbilityRate.AttackDamage; DrawEngine.InfoWindow.Warning = "Вы вступили на путь силы."; }
            }
        
            /// <summary>
            /// Void for level up ability
            /// </summary>
            public void UP()
            {
                this.Level += 1;
                this.Power += Convert.ToInt32(this.Level * this.LVRate);
                if (this.Class != BattleClass.Warrior)
                { this.CostRate += Convert.ToInt32(this.CostRate * 0.25); }
                //this.COE = this._COE();
                //CRAFT ITEMS NEED DO WITH DYNAMIC STAT GENERATE
                if (this.Duration != 0)
                {
                    this.Duration += Convert.ToInt32(this.LVRate);
                }
                if (this.SummonMonster != null)
                {
                    if (this.SummonMonster.Name == "Скелет") { this.SummonMonster = DataBase.SummonedBase.Sceleton(); }
                    if (this.SummonMonster.Name == "Призрак") { this.SummonMonster = DataBase.SummonedBase.Spirit(); }
                    if (this.SummonMonster.Name == "Дух огня") { this.SummonMonster = DataBase.SummonedBase.FireElemental(); }
                    if (this.SummonMonster.AttackSpeed == 12874) { this.SummonMonster = DataBase.SummonedBase.InquisitorUnit(); }
                }
                if (this.Name == "Алхимия")
                {
                    foreach (Ability Alb in Rogue.RAM.Player.Ability)
                    {
                        if (Alb.Name == "Радужные брызги")
                        {
                            Alb.Action.Add(new AbilityAction() { Act = AbilityActionType.Damage, Atr = new List<AbilityActionAttribute>() { AbilityActionAttribute.DmgHealInstant } });
                        }
                        if (Alb.Name == "Бутыль стихий")
                        {
                            Alb.Stats.Add(SystemEngine.Helper.Stat.Gets);
                        }
                    }
                }
            }
            /// <summary>
            /// Get ability COE for now time
            /// </summary>
            /// <returns>For each class and ability it will be special result</returns>
            private double _COE()
            {
                double resultOFresult = 0;
                double result = 0;
                foreach (AbilityAction a in this.Action)
                {
                    switch (a.Act)
                    {
                        case AbilityActionType.Craft:
                            {
                                if (Rogue.RAM.Player.Gold < (Rogue.RAM.Player.Level * 100)) { result += (Rogue.RAM.Player.Level * 0.34) + (Rogue.RAM.Player.Level * 0.21); }
                                if (Rogue.RAM.Player.Class == BattleClass.Alchemist || Rogue.RAM.Player.Class == BattleClass.Shaman) { result += Rogue.RAM.Player.Level * 0.37; }
                                break;
                            }
                        case AbilityActionType.Damage:
                            {
                                if ((Rogue.RAM.Player.MHP < Rogue.RAM.Player.Level * 10) && (Rogue.RAM.Player.ARM < Rogue.RAM.Player.Level * 5) && (Rogue.RAM.Player.MRS < Rogue.RAM.Player.Level * 5))
                                { result += Rogue.RAM.Player.AD * 0.24; }
                                else { result += Rogue.RAM.Player.AD * 0.0999; }
                                if (Rogue.RAM.Player.Equipment.Weapon == null)
                                { result += 15; }
                                else { result += Rogue.RAM.Player.Equipment.Weapon.AD * 0.34; }
                                if (Rogue.RAM.Player.Class == BattleClass.Assassin || Rogue.RAM.Player.Class == BattleClass.FireMage) { result += Rogue.RAM.Player.Level * 0.7; }
                                break;
                            }
                        case AbilityActionType.Destruction:
                            {
                                if ((Rogue.RAM.Player.MHP < Rogue.RAM.Player.Level * 10) && (Rogue.RAM.Player.ARM < Rogue.RAM.Player.Level * 5) && (Rogue.RAM.Player.MRS < Rogue.RAM.Player.Level * 5))
                                { result += (Rogue.RAM.Player.MHP * 0.37) + (Rogue.RAM.Player.MHP * 0.29); }
                                else { result += Rogue.RAM.Player.MHP * 0.41; }
                                if (Rogue.RAM.Player.Equipment.Weapon == null)
                                { result += 5; }
                                else { result += 4; }
                                if (Rogue.RAM.Player.Class == BattleClass.BloodMage) { result += Rogue.RAM.Player.Level * 0.6123; }
                                break;
                            }
                        case AbilityActionType.Heal:
                            {
                                if ((Rogue.RAM.Player.MHP < Rogue.RAM.Player.Level * 10)) { result += Rogue.RAM.Player.AP * 0.3; } else { result += 10; }
                                if (Rogue.RAM.Player.AD > Rogue.RAM.Player.AP) { result += Rogue.RAM.Player.Level * 0.27; } else { result += Rogue.RAM.Player.AP * 0.47; }
                                if (Rogue.RAM.Player.Class == BattleClass.Paladin) { result += Rogue.RAM.Player.Level * 0.1267; }
                                break;
                            }
                        case AbilityActionType.Improve:
                            { result += 20; if (Rogue.RAM.Player.Class == BattleClass.Monk) { result += Rogue.RAM.Player.Level * 0.3; } break; }
                        case AbilityActionType.Debuff:
                            { result += 20; if (Rogue.RAM.Player.Class == BattleClass.Monk) { result += Rogue.RAM.Player.Level * 0.3; } break; }
                        //case AbilityActionType.Neutral:
                        //    { result = this.COE; break; }
                        case AbilityActionType.Summon:
                            { result += 1; if (Rogue.RAM.Player.Class == BattleClass.Necromant || Rogue.RAM.Player.Class == BattleClass.Inquisitor) { result += Rogue.RAM.Player.Level * 0.3; } break; }
                    }                    
                    resultOFresult += result;
                }
                if (resultOFresult < 1) { resultOFresult = resultOFresult * 100; }
                if (resultOFresult < 10) { resultOFresult = resultOFresult * 4.6; }
                if (resultOFresult < 20) { resultOFresult = resultOFresult * 1.87; }
                Random WeNeedSomeMagic = new Random();
                resultOFresult = ((resultOFresult * 0.1)+(this.Level*0.75));
                return resultOFresult;
            }
        }

        public enum AbilityLocation
        {
            /// <summary>
            /// Ability can be used only at world map
            /// </summary>
            WorldMap = 0,
            /// <summary>
            /// Ability can be used only in combat
            /// </summary>
            Combat = 1,
            /// <summary>
            /// Ability can be used each time
            /// </summary>
            Alltime=2
        }

        public enum AbilityStats
        {
            DMG = 0,
            MHP = 1,
            MMP = 2,
            AD = 3,
            AP = 4,
            ARM = 5,
            MRS = 6
        }

        public enum AbilityType
        {
            /// <summary>
            /// Passive ability, character can't use it
            /// </summary>
            Passive = 0,
            /// <summary>
            /// Active ability, character can use it
            /// </summary>
            Active = 1
        }

        public enum AbilityElement
        {
            Physical = 0,
            BloodMagic = 1,
            FireMagic = 2,
            FrostMagic = 3,
            NatureMagic = 4,
            HolyMagic = 5,
            DeadMagic = 6,
            DemonMagic = 8,
            ElementalMagic = 9
        }

        public enum AbilityDestructionType
        {
            Map=0,
            Objects=1
        }

        public enum AbilityRate
        {
            AttackDamage = 0,
            AbilityPower = 1
        }
        /// <summary>
        /// For uses action
        /// </summary>
        public class AbilityAction
        {
            public AbilityActionType Act; public List<AbilityActionAttribute> Atr;
        }

        public class AbilityActionOne
        {
            public AbilityActionType Action; public AbilityActionAttribute Attribute;
        }
        
        public enum AbilityActionType
        {
            /// <summary>
            /// Ability deal damage enemy
            /// </summary>
            Damage = 0,
            /// <summary>
            /// Ability heal character
            /// </summary>
            Heal = 1,
            /// <summary>
            /// Ability create item
            /// </summary>
            Craft = 3,
            /// <summary>
            /// Ability do something special at map
            /// </summary>
            Neutral = 4,
            /// <summary>
            /// Ability buff character
            /// </summary>
            Improve = 5,
            /// <summary>
            /// Ability summon creature
            /// </summary>
            Summon = 6,
            /// <summary>
            /// Special action of ability for blood mage
            /// </summary>
            Debuff = 7,
            /// <summary>
            /// Ability destroy somebody or something
            /// </summary>
            Destruction=8
        }

        public enum AbilityActionAttribute
        {
            DmgHealOnTime = 0,
            DmgHealInstant = 1,
            Special = 2,
            EffectInstant = 8,
            EffectOfTime = 9,
            DebuffStack = 10
        }

        public class Monster
        {
            public class Tank : Monster
            {
                public Tank(MechEngine.Monster M)
                {
                    this.Cast = new MonsterAbility[2];
                    MHP = Convert.ToInt32(M.MHP * 2);
                    CHP = Convert.ToInt32(M.CHP * 2);
                    MIDMG = M.MIDMG;
                    MADMG = M.MIDMG;
                    ARM = Convert.ToInt32(M.ARM * 1.1);
                    MRS = Convert.ToInt32(M.MRS * 1.1);
                    AD = Convert.ToInt32(M.AD * 0.4);
                    AP = Convert.ToInt32(M.AP * 0.4);
                    LVL = M.LVL;
                    Affix = " - Защитник";
                    Name = M.Name;
                    Icon = M.Icon;
                    EXP = M._EXP;
                    Loot = M.Loot;
                    Race = M.Race;
                    Chest = M.Chest;
                    this.Cast[0] = M.GetAbil();
                    MonsterAbility a = new MonsterAbility();
                    a.Action = new AbilityActionOne() { Action = AbilityActionType.Improve, Attribute = AbilityActionAttribute.DmgHealInstant };
                    a.Name = "Оборона";
                    a.Icon = 'D';
                    a.Type = AbilityRate.AttackDamage;
                    a.Stats = new List<AbilityStats>() { AbilityStats.ARM, AbilityStats.MRS };
                    a.Power = Convert.ToInt32(this.ARM * 0.15);
                    Cast[1] = a;
                    if (M.Boss) { Cast[0] = M.BossAblity; }
                }
            }

            public class DamageDealer : Monster
            {
                public DamageDealer (MechEngine.Monster M)
                {
                    this.Cast = new MonsterAbility[2];
                    MHP = M.MHP;
                    CHP = M.CHP;
                    MIDMG = M.MIDMG * 2;
                    MADMG = M.MIDMG * 2;
                    ARM = M.ARM;
                    MRS = 0;
                    AD = M.AD * 2;
                    AP = 0;
                    LVL = M.LVL;
                    Affix = " - Воин";
                    EXP = M._EXP;
                    Name = M.Name;
                    Icon = M.Icon;
                    Loot = M.Loot;
                    Race = M.Race;
                    Chest = M.Chest;
                    this.Cast[0] = M.GetAbil();
                    MonsterAbility a = new MonsterAbility();
                    a.Action = new AbilityActionOne() { Action = AbilityActionType.Damage, Attribute = AbilityActionAttribute.DmgHealInstant };
                    a.Name = "Сильный удар";
                    a.Icon = 'P';
                    a.Type = AbilityRate.AttackDamage;
                    a.Power = Convert.ToInt32(this.AD * 0.25);
                    Cast[1] = a;
                    if (M.Boss) { Cast[0] = M.BossAblity; }
                }
            }

            public class Healer : Monster
            {
                public Healer(MechEngine.Monster M)
                {
                    this.Cast = new MonsterAbility[2];
                    MHP = M.MHP;
                    CHP = M.CHP;
                    MIDMG = M.MIDMG;
                    MADMG = M.MADMG;
                    ARM = M.ARM;
                    MRS = M.MRS;
                    AD = 0;
                    AP = M.AP*2;
                    LVL = M.LVL;
                    Affix = " - Лекарь";
                    EXP = M._EXP;
                    Name = M.Name;
                    Icon = M.Icon;
                    Loot = M.Loot;
                    Race = M.Race;
                    Chest = M.Chest;
                    this.Cast[0] = M.GetAbil();
                    MonsterAbility a = new MonsterAbility();
                    a.Action = new AbilityActionOne() { Action = AbilityActionType.Heal, Attribute = AbilityActionAttribute.DmgHealInstant };
                    a.Name = "Целительство";
                    a.Icon = 'H';
                    a.Type = AbilityRate.AbilityPower;
                    a.Power = Convert.ToInt32((this.AP * 0.25));  
                    Cast[1] = a;
                    if (M.Boss) { Cast[0] = M.BossAblity; }
                }
            }

            public class Debuffer : Monster
            {
                public Debuffer(MechEngine.Monster M)
                {
                    this.Cast = new MonsterAbility[2];
                    MHP = Convert.ToInt32(M.MHP * 2);
                    CHP = Convert.ToInt32(M.CHP * 2);
                    MIDMG = M.MIDMG;
                    MADMG = M.MIDMG;
                    ARM = Convert.ToInt32(M.ARM * 0.3);
                    MRS = Convert.ToInt32(M.ARM * 0.2);
                    AD = 0;
                    AP = M.AP;
                    LVL = M.LVL;
                    Affix = " - Колдун";
                    EXP = M._EXP;
                    Name = M.Name;
                    Icon = M.Icon;
                    Loot = M.Loot;
                    Race = M.Race;
                    Chest = M.Chest;
                    this.Cast[0] = M.GetAbil();
                    MonsterAbility a = new MonsterAbility();
                    a.Action = new AbilityActionOne() { Action = AbilityActionType.Debuff, Attribute = AbilityActionAttribute.DmgHealInstant };
                    a.Name = "Проклятье";
                    a.Icon = 'C';
                    a.Type = AbilityRate.AbilityPower;
                    a.Stats = new List<AbilityStats>() { AbilityStats.DMG, AbilityStats.AD };
                    a.Power = Convert.ToInt32(this.AP * 0.05);
                    Cast[1] = a;
                    if (M.Boss) { Cast[0] = M.BossAblity; }
                }
            }

            public class Mage : Monster
            {
                public Mage(MechEngine.Monster M)
                {
                    this.Cast = new MonsterAbility[2];
                    MHP = M.MHP;
                    CHP = M.CHP;
                    MIDMG = M.MIDMG;
                    MADMG = M.MIDMG;
                    ARM = 0;
                    MRS = M.MRS * 2;
                    AD = 0;
                    AP = M.AP;
                    LVL = M.LVL;
                    Affix = " - Чародей";
                    EXP = M._EXP;
                    Name = M.Name;
                    Icon = M.Icon;
                    Loot = M.Loot;
                    Race = M.Race;
                    Chest = M.Chest;
                    this.Cast[0] = M.GetAbil();
                    MonsterAbility a = new MonsterAbility();
                    a.Action = new AbilityActionOne() { Action = AbilityActionType.Damage, Attribute = AbilityActionAttribute.DmgHealInstant };
                    a.Name = "Огненный шар";
                    a.Icon = 'F';
                    a.Type = AbilityRate.AbilityPower;
                    a.Power = Convert.ToInt32(this.AP * 0.25);
                    Cast[1] = a;
                    if (M.Boss) { Cast[0] = M.BossAblity; }
                }
            }

            public class RogueDealer : Monster
            {
                public RogueDealer(MechEngine.Monster M)
                {
                    this.Cast = new MonsterAbility[2];
                    MHP = M.MHP;
                    CHP = M.CHP;
                    MIDMG = M.MIDMG;
                    MADMG = M.MIDMG * 3;
                    ARM = 0;
                    MRS = 0;
                    AD = M.AD*2;
                    AP = 0;
                    LVL = M.LVL;
                    Affix = " - Бродяга";
                    EXP = M._EXP;
                    Name = M.Name;
                    Icon = M.Icon;
                    Loot = M.Loot;
                    Race = M.Race;
                    Chest = M.Chest;
                    this.Cast[0] = M.GetAbil();
                    MonsterAbility a = new MonsterAbility();
                    a.Action = new AbilityActionOne() { Action = AbilityActionType.Damage, Attribute = AbilityActionAttribute.DmgHealInstant };
                    a.Name = "Расправа";
                    a.Icon = 'E';
                    a.Type = AbilityRate.AttackDamage;
                    a.Power = Convert.ToInt32(this.AD * 0.15);
                    Cast[1] = a;
                    if (M.Boss) { Cast[0] = M.BossAblity; }
                }
            }
            /// <summary>
            /// Moster take damage from trap
            /// </summary>
            /// <param name="Dmg">Trap's damage</param>
            public void TakeTrap(Trap Trap)
            {
                int truedamage = Math.Abs((this.ARM / 2) - Trap.Damage);
                this.CHP -= truedamage;
                DrawEngine.InfoWindow.Custom(this.Name + " активирует " + Trap.Name + " и получает " + truedamage + " урона от стихии: " + SystemEngine.Helper.String.ToString(Trap.Element) + " !");
                if (this.CHP <= 0)
                {
                    for (int y = 0; y < 23; y++)
                    {
                        for (int x = 0; x < 71; x++)
                        {
                            if (Rogue.RAM.Map.Map[x][y].Trap == Trap)
                            {
                                Rogue.RAM.Map.Map[x][y].Item = Rogue.RAM.Map.Map[x][y].Enemy.Loot;
                                Rogue.RAM.Map.Map[x][y].Enemy = null;
                                Rogue.RAM.Map.Map[x][y].Vision = Rogue.RAM.Map.Map[x][y].Item.Icon();
                                DrawEngine.LabDraw.ReDrawObject(x, y, Rogue.RAM.Map.Map[x][y].Item, null, null, null);
                            }
                        }
                    }
                    Rogue.RAM.Player.Kill(this);
                }
            }

            public Monster()
            {
                this.DoT = new List<Ability>();
                this.Cast = new MonsterAbility[2];
            }

            public Monster(MonsterRace Racce)
            {
                BoneRace = Racce;
            }

            private MonsterAbility GetAbil()
            {
                MonsterAbility a = new MonsterAbility(); 
                switch (this.BoneRace)
                {
                    case MechEngine.MonsterRace.Animal:
                        {
                            a.Action = new AbilityActionOne() { Action = AbilityActionType.Damage, Attribute = AbilityActionAttribute.DmgHealInstant };
                            a.Name = "Животный укус";
                            a.Type = AbilityRate.AttackDamage;
                            a.Power = Convert.ToInt32(this.AD * 0.25) + Convert.ToInt32(this.LVL / 2);
                            break;
                        }
                    case MechEngine.MonsterRace.Avariel:
                        {
                            a.Action = new AbilityActionOne() { Action = AbilityActionType.Heal, Attribute = AbilityActionAttribute.DmgHealInstant };
                            a.Name = "Восстановление";
                            a.Type = AbilityRate.AbilityPower;
                            a.Power = Convert.ToInt32(this.AP * 0.25) + Convert.ToInt32(this.LVL / 2);
                            break;
                        }
                    case MechEngine.MonsterRace.Dragon:
                        {
                            a.Action = new AbilityActionOne() { Action = AbilityActionType.Damage, Attribute = AbilityActionAttribute.DmgHealInstant };
                            a.Name = "Пламя";
                            a.Type = AbilityRate.AbilityPower;
                            a.Power = Convert.ToInt32(this.AP * 0.25) + Convert.ToInt32(this.LVL / 2);
                            break;
                        }
                    case MechEngine.MonsterRace.Drow:
                        {
                            a.Action = new AbilityActionOne() { Action = AbilityActionType.Debuff, Attribute = AbilityActionAttribute.DmgHealInstant };
                            a.Name = "Отравленный выстрел";
                            a.Type = AbilityRate.AttackDamage;
                            a.Stats = new List<AbilityStats>() { AbilityStats.AP, AbilityStats.AD };
                            a.Power = Convert.ToInt32(this.AD * 0.15) + Convert.ToInt32(this.LVL / 2);
                            break;
                        }
                    case MechEngine.MonsterRace.Human:
                        {
                            a.Action = new AbilityActionOne() { Action = AbilityActionType.Improve, Attribute = AbilityActionAttribute.DmgHealInstant };
                            a.Name = "Выживание";
                            a.Type = AbilityRate.AttackDamage;
                            a.Stats = new List<AbilityStats>() { AbilityStats.MHP, AbilityStats.AD };
                            a.Power = Convert.ToInt32(this.AD * 0.15) + Convert.ToInt32(this.LVL / 2);
                            break;
                        }
                    case MechEngine.MonsterRace.Undead:
                        {
                            a.Action = new AbilityActionOne() { Action = AbilityActionType.Damage, Attribute = AbilityActionAttribute.DmgHealInstant };
                            a.Name = "Гниль";
                            a.Type = AbilityRate.AbilityPower;
                            a.Power = Convert.ToInt32(this.AP * 0.25) + Convert.ToInt32(this.LVL / 2) + Convert.ToInt32(Rogue.RAM.Player.MHP * 0.01);
                            break;
                        }
                }
                return a;
            }

            private MonsterRace BoneRace;

            public int CHP, MHP, MADMG, MIDMG, ARM, MRS, AD, AP, LVL;

            public double EXPRate;
            private int _EXP;
            public int EXP
            {
                set { this._EXP = value; }
                get { return Convert.ToInt32(this._EXP + (this.EXPRate * this.LVL)); }
            }

            public double _HP
            {
                set { int vl = Convert.ToInt32(value); CHP = vl; MHP = vl; }
                get { return Convert.ToDouble(CHP); }
            }
            public double _ARM
            {
                set { ARM = Convert.ToInt32(value * Rogue.RAM.Map.Level + 10); }
                get { return Convert.ToDouble(ARM); }
            }
            public double _MRS
            {
                set { MRS = Convert.ToInt32(value * Rogue.RAM.Map.Level + 10); }
                get { return Convert.ToDouble(MRS); }
            }
            public double _AD
            {
                set { AD = Convert.ToInt32(value * Rogue.RAM.Map.Level); }
                get { return Convert.ToDouble(AD); }
            }
            public double _AP
            {
                set { AP = Convert.ToInt32(value * Rogue.RAM.Map.Level); }
                get { return Convert.ToDouble(AP); }
            }
            public double _MIDMG
            {
                set { MIDMG = Convert.ToInt32(value * (Rogue.RAM.Map.Level + 10)); }
                get { return Convert.ToDouble(MIDMG); }
            }
            public double _MADMG
            {
                set { MADMG = Convert.ToInt32(value * (Rogue.RAM.Map.Level + 10)); }
                get { return Convert.ToDouble(MADMG); }
            }
            public double __EXP
            {
                set { _EXP = Convert.ToInt32(value * (Rogue.RAM.Map.Level + 10)); }
                get { return Convert.ToDouble(_EXP); }
            }
            public double _EXPRate
            {
                set { EXPRate = Convert.ToInt32(value * (Rogue.RAM.Map.Level + 10)); }
                get { return Convert.ToDouble(EXPRate); }
            }

            public bool Aggressive = true;

            public List<Ability> DoT;
            public List<MonsterAbility> EoF=new List<MonsterAbility>();

            public string Name;

            public string Affix;

            public char Icon;
            /// <summary>
            /// return info about monster
            /// </summary>
            public string Info="";

            public Item Loot;

            public MonsterAbility[] Cast;

            public bool Boss = false;
            public MonsterAbility BossAblity;

            public ConsoleColor Chest;

            public Script Script;

            public MonsterRace Race;

            public string GetRace()
            {
                string s = string.Empty;
                switch (Race)
                {
                    case MechEngine.MonsterRace.Animal:
                        {
                            s = "Животное"+Affix;
                            break;
                        }
                    case MechEngine.MonsterRace.Avariel:
                        {
                            s = "Санглеф" + Affix;
                            break;
                        }
                    case MechEngine.MonsterRace.Dragon:
                        {
                            s = "Дракон" + Affix;
                            break;
                        }
                    case MechEngine.MonsterRace.Drow:
                        {
                            s = "Дроу" + Affix;
                            break;
                        }
                    case MechEngine.MonsterRace.Human:
                        {
                            s = "Гуманойд" + Affix;
                            break;
                        }
                    case MechEngine.MonsterRace.Undead:
                        {
                            s = "Нежить" + Affix;
                            break;
                        }
                }
                if (this.Name == "Валоран")
                { return "Хранитель мира [Предатель]"; }
                else { return s; }
            }
        }

        public class Summoned
        {
            /// <summary>
            /// Avalible actions for summoned
            /// </summary>
            public AbilityActionType[] Actions;
            /// <summary>
            /// Enable or Disable Summoned
            /// </summary>
            public bool Enabled
            {
                set
                {
                   Activate();
                }
            }
            /// <summary>
            /// Name of summoned
            /// </summary>
            public string Name
            {
                set { _Name = value; }
                get { return __Name(); }
            }
            /// <summary>
            /// Real field
            /// </summary>
            private string _Name;
            /// <summary>
            /// Special field for INquisitor
            /// </summary>
            /// <returns></returns>
            private string __Name()
            {
                if (this._Name == "Unit") 
                {
                    foreach (Ability A in Rogue.RAM.Player.Ability)
                    {
                        if (A.Name == "Вызов")
                        {
                            if (A.GoodEvilAbility) { return "Ангел"; }
                            else { return "Демон"; }
                        }                        
                    }
                }
                if (this._Name == "Дух огня")
                {
                    return "Огненный элементаль";
                }
                return _Name;
            }
            /// <summary>
            /// Power of attack damage
            /// </summary>
            public int PAD;
            /// <summary>
            /// Uses for attacking
            /// </summary>
            public AbilityRate MagicOrPhysic;
            /// <summary>
            /// Power of healing attack
            /// </summary>
            public int PAH;
            /// <summary>
            /// Power of attack DeMagic
            /// </summary>
            public int PAM;
            /// <summary>
            /// Target-ability for DeMagic
            /// </summary>
            public AbilityStats PAMa;
            /// <summary>
            /// How often summoned will be use one of actions.
            /// Example: 1000 {Summoned will be attack every 1 second (it's SO FUCKING FAST!!!)}
            /// </summary>
            public double AttackSpeed;
            /// <summary>
            /// Activate summoned
            /// </summary>
            private void Activate()
            {
                foreach (AbilityActionType Act in this.Actions)
                {
                    switch (Act)
                    {
                        case AbilityActionType.Damage: { SummonedAttacking(); break; }
                        case AbilityActionType.Heal: { SummonedHealing(); break; }
                        case AbilityActionType.Debuff: { SummonedDemagic(); break; }
                    }
                }

                if (Rogue.RAM.Enemy != null)
                {
                    if (Rogue.RAM.Enemy.CHP <= 0) { Rogue.RAM.Log.Add("Ваш союзнк одержал победу, нажмите {A} для выхода!"); }// SystemEngine.Helper.Deactivation.EndBattle(); }
                    DrawEngine.FightDraw.ReDrawCombatLog();
                }
            }
            /// <summary>
            /// Summoned attack enemy
            /// </summary>
            private void SummonedAttacking()
            {
                if (Rogue.RAM.Enemy != null)
                {
                    int truedamage = this.PAD;
                    if (this.MagicOrPhysic == AbilityRate.AbilityPower) { if (Rogue.RAM.Enemy.MRS != 0) { truedamage = this.PAD - Convert.ToInt32(Rogue.RAM.Enemy.MRS * 0.51); } }
                    else { if (Rogue.RAM.Enemy.ARM != 0) { truedamage = this.PAD - Convert.ToInt32(Rogue.RAM.Enemy.ARM * 0.71); } }
                    if (truedamage < 0) { truedamage = 0; }
                    Rogue.RAM.Enemy.CHP -= truedamage;
                    Rogue.RAM.Log.Add(this.Name + " атакует " + Rogue.RAM.Enemy.Name + " и наносит " + truedamage + " урона!");
                    DrawEngine.FightDraw.ReDrawCombatLog();
                }
            }
            /// <summary>
            /// Summoned healing character
            /// </summary>
            private void SummonedHealing()
            {
                if (Rogue.RAM.Enemy != null)
                {
                    if ((Rogue.RAM.Player.CHP + this.PAH) >= Rogue.RAM.Player.MHP) { Rogue.RAM.Player.CHP = Rogue.RAM.Player.MHP; }
                    else { Rogue.RAM.Player.CHP += this.PAH; }
                    Rogue.RAM.Log.Add(this.Name + " исцеляет " + Rogue.RAM.Player.Name + " на " + PAH + " жизни!");
                }
            }
            /// <summary>
            /// Summoned cast negative magic at enemy
            /// </summary>
            private void SummonedDemagic()
            {
                if (Rogue.RAM.Enemy != null)
                {
                    switch (this.PAMa)
                    {
                        case AbilityStats.AD: { Rogue.RAM.Enemy.AD -= this.PAM; break; }
                        case AbilityStats.AP: { Rogue.RAM.Enemy.AP -= this.PAM; break; }
                        case AbilityStats.ARM: { Rogue.RAM.Enemy.ARM -= this.PAM; break; }
                        case AbilityStats.DMG: { Rogue.RAM.Enemy.MADMG -= this.PAM; Rogue.RAM.Enemy.MIDMG += this.PAM; break; }
                        case AbilityStats.MHP: { Rogue.RAM.Enemy.MHP -= this.PAM; Rogue.RAM.Enemy.CHP += this.PAM; break; }
                        case AbilityStats.MRS: { Rogue.RAM.Enemy.MRS -= this.PAM; break; }
                    }
                    Rogue.RAM.Log.Add(this.Name + " уменьшает '" + SystemEngine.Helper.String.ToString(this.PAMa) + "' на " + this.PAM + " !");                    
                }
            }            
        }

        public enum MonsterRace
        {
            Undead = 0,
            Animal = 1,
            Drow = 2,
            Dragon = 4,
            Avariel = 5,
            Human = 6
        }

        public enum MonsterClass
        {
            Tank = 0,
            DPS = 1,
            Heal = 2,
            Mage = 3
        }

        public enum MonsterType
        {
            Normal=0,
            Usual=1,
            Special=2,
            Hard=3
        }

        public class MonsterAbility
        {
            public string Name;

            public char Icon;

            public int Power;
            public int PowerBefore;
            public int PowerBefore2;
            

            public string Activate
            {
                get
                {
                    string str = string.Empty;
                    switch (this.Action.Action)
                    {
                        case AbilityActionType.Damage:
                            {
                                if (this.Action.Attribute == AbilityActionAttribute.DmgHealInstant)
                                {
                                    int d = this.Power;
                                    if (this.Type == AbilityRate.AbilityPower)
                                    {
                                        if (Rogue.RAM.Player.MRS != 0) { d = d - Convert.ToInt32(Rogue.RAM.Player.MRS * 0.5); }
                                    }
                                    else
                                    {
                                        if (Rogue.RAM.Player.ARM != 0) { d = d - Convert.ToInt32(Rogue.RAM.Player.ARM * 0.25); }
                                    }
                                    //Если ушли в минус по дамагу
                                    if (d < 0) { d = 0; }
                                    //вычитаем
                                    Rogue.RAM.Player.CHP -= d;
                                    //в лог
                                    str = Rogue.RAM.Enemy.Name + " использует " + this.Name + " и наносит " + d.ToString() + " урона!";
                                    DrawEngine.FightDraw.ReDrawCombatLog();                                    
                                }
                                break;
                            }
                        case AbilityActionType.Debuff:
                            {
                                Rogue.RAM.Player.DeBuffs.Add(this);
                                string stats = string.Empty;
                                foreach (AbilityStats s in this.Stats)
                                {
                                    PowerBefore=this.Power;
                                    int Powder = this.Power * (-1);
                                    switch (s)
                                    {
                                        case AbilityStats.AD: { Rogue.RAM.Player.AD += Powder; break; }
                                        case AbilityStats.AP: { Rogue.RAM.Player.AP += Powder; break; }
                                        case AbilityStats.ARM: { Rogue.RAM.Player.ARM += Powder; break; }
                                        case AbilityStats.DMG: { Rogue.RAM.Player.MADMG += Powder; Rogue.RAM.Player.MIDMG += Powder; break; }
                                        case AbilityStats.MHP: { Rogue.RAM.Player.MHP += Powder; Rogue.RAM.Player.CHP += Powder; break; }
                                        case AbilityStats.MMP: { Rogue.RAM.Player.MMP += Powder; Rogue.RAM.Player.CMP += Powder; break; }
                                        case AbilityStats.MRS: { Rogue.RAM.Player.MRS += Powder; break; }
                                    }
                                    if (stats == string.Empty)
                                    { stats += SystemEngine.Helper.String.ToString(s); }
                                    else
                                    { stats += "," + SystemEngine.Helper.String.ToString(s); }
                                }
                                str = this.Name + " уменьшает вам '" + stats + "' на " + this.Power + " !";
                                DrawEngine.FightDraw.ReDrawCombatLog();
                                DrawEngine.FightDraw.DrawEnemyStat();
                                break;
                            }
                        case AbilityActionType.Heal:
                            {
                                int d = this.Power;
                                if (this.Type == AbilityRate.AbilityPower)
                                {
                                    if (Rogue.RAM.Player.MRS != 0) { d = d - Convert.ToInt32(Rogue.RAM.Player.MRS * 0.37); }
                                }
                                else
                                {
                                    if (Rogue.RAM.Player.ARM != 0) { d = d - Convert.ToInt32(Rogue.RAM.Player.ARM * 0.37); }
                                }
                                //Если ушли в минус по дамагу
                                if (d < 0) { d = 0; }
                                //добавляем
                                if ((Rogue.RAM.Enemy.CHP + d) > Rogue.RAM.Enemy.MHP)
                                {
                                    Rogue.RAM.Enemy.CHP = Rogue.RAM.Enemy.MHP;
                                }
                                else { Rogue.RAM.Enemy.CHP += d; }
                                //в лог
                                str = Rogue.RAM.Enemy.Name + " использует " + this.Name + " и исцеляет " + d + " урона!";
                                DrawEngine.FightDraw.ReDrawCombatLog();
                                break;
                            }
                        case AbilityActionType.Improve:
                            {
                                Rogue.RAM.Enemy.EoF.Add(this);
                                string stats = string.Empty;
                                foreach (AbilityStats s in this.Stats)
                                {
                                    PowerBefore2 = this.Power;
                                    int Powder = this.Power;
                                    switch (s)
                                    {
                                        case AbilityStats.AD: { Rogue.RAM.Enemy.AD += Powder; break; }
                                        case AbilityStats.AP: { Rogue.RAM.Enemy.AP += Powder; break; }
                                        case AbilityStats.ARM: { Rogue.RAM.Enemy.ARM += Powder; break; }
                                        case AbilityStats.DMG: { Rogue.RAM.Enemy.MADMG += Powder; Rogue.RAM.Enemy.MIDMG += Powder; break; }
                                        case AbilityStats.MHP: { Rogue.RAM.Enemy.MHP += Powder; Rogue.RAM.Enemy.CHP += Powder; break; }
                                        case AbilityStats.MRS: { Rogue.RAM.Enemy.MRS += Powder; break; }
                                    }
                                    if (stats == string.Empty)
                                    { stats += SystemEngine.Helper.String.ToString(s); }
                                    else
                                    { stats += "," + SystemEngine.Helper.String.ToString(s); }

                                }
                                str = this.Name + " увеличивает врагу '" + stats + "' на " + this.Power + " !";
                                DrawEngine.FightDraw.ReDrawCombatLog();
                                DrawEngine.FightDraw.DrawEnemyStat();
                                break;
                            }
                    }
                    return str;
                }
            }

            public void DeActivate()
            {
                if (this.Action.Action == AbilityActionType.Debuff)
                {
                    //Rogue.RAM.Player.DeBuffs.Add(this);
                    string stats = string.Empty;
                    foreach (AbilityStats s in this.Stats)
                    {
                        int Powder = this.PowerBefore;
                        switch (s)
                        {
                            case AbilityStats.AD: { Rogue.RAM.Player.AD += Powder; break; }
                            case AbilityStats.AP: { Rogue.RAM.Player.AP += Powder; break; }
                            case AbilityStats.ARM: { Rogue.RAM.Player.ARM += Powder; break; }
                            case AbilityStats.DMG: { Rogue.RAM.Player.MADMG += Powder; Rogue.RAM.Player.MIDMG += Powder; break; }
                            case AbilityStats.MHP: { Rogue.RAM.Player.MHP += Powder; Rogue.RAM.Player.CHP += Powder; break; }
                            case AbilityStats.MMP: { Rogue.RAM.Player.MMP += Powder; Rogue.RAM.Player.CMP += Powder; break; }
                            case AbilityStats.MRS: { Rogue.RAM.Player.MRS += Powder; break; }
                        }
                        if (stats == string.Empty)
                        { stats += SystemEngine.Helper.String.ToString(s); }
                        else
                        { stats += "," + SystemEngine.Helper.String.ToString(s); }
                    }
                    //str = this.Name + " увеличивает '" + stats + "' на " + this.Power + " !";
                    DrawEngine.FightDraw.ReDrawCombatLog();
                    DrawEngine.FightDraw.DrawEnemyStat();
                }
                if (this.Action.Action == AbilityActionType.Improve)
                {
                    //Rogue.RAM.Enemy.EoF.Add(this);
                    string stats = string.Empty;
                    foreach (AbilityStats s in this.Stats)
                    {
                        int Powder = this.PowerBefore2 * (-1);
                        switch (s)
                        {
                            case AbilityStats.AD: { Rogue.RAM.Enemy.AD += Powder; break; }
                            case AbilityStats.AP: { Rogue.RAM.Enemy.AP += Powder; break; }
                            case AbilityStats.ARM: { Rogue.RAM.Enemy.ARM += Powder; break; }
                            case AbilityStats.DMG: { Rogue.RAM.Enemy.MADMG += Powder; Rogue.RAM.Enemy.MIDMG += Powder; break; }
                            case AbilityStats.MHP: { Rogue.RAM.Enemy.MHP += Powder; Rogue.RAM.Enemy.CHP += Powder; break; }
                            case AbilityStats.MRS: { Rogue.RAM.Enemy.MRS += Powder; break; }
                        }
                        if (stats == string.Empty)
                        { stats += SystemEngine.Helper.String.ToString(s); }
                        else
                        { stats += "," + SystemEngine.Helper.String.ToString(s); }

                    }
                    //str = this.Name + " уменьшает  вам '" + stats + "' на " + this.Power + " !";
                    DrawEngine.FightDraw.ReDrawCombatLog();
                    DrawEngine.FightDraw.DrawEnemyStat();
                }
            }

            public AbilityActionOne Action;
            public AbilityRate Type;
            public List<AbilityStats> Stats;
        }

        public class Wall
        {
            public bool Obstruct;
        }
        /// <summary>
        /// Rogues traps
        /// </summary>
        public class Trap
        {
            public string Name;
            public int Damage;
            public AbilityElement Element;
            public ConsoleColor Color = ConsoleColor.Gray;
            public char icon ='?';
        }

        public class ActiveObject
        {
            public string Name;

            public bool Move = true;

            public char Icon;

            public Item Key;

            public Script Script;

            public char IconExit = '▓';
            /// <summary>
            /// return info about object
            /// </summary>
            public string Info="";

            public ConsoleColor Color;

            public bool UseScript = false;
            
            public virtual bool Use()
            {
                if (UseScript)
                { Script(); return false; }
                else
                {                    
                    bool Enter = false;
                    if (Name != "Exit")
                    {
                        if (this.Key != null) { SoundEngine.Sound.OpenDoor(); }
                        MechEngine.Item[] I = Rogue.RAM.Player.Inventory.ToArray();
                        foreach (Item k in I)
                        {
                            if (k.Name == Key.Name)
                            {
                                
                                if (r.Next(100) > 45) //шанс не сломать ключ 65
                                {
                                    Rogue.RAM.Player.Inventory.Remove(k);
                                    DrawEngine.InfoWindow.DestroyLoot(k);
                                    Thread.Sleep(1000);
                                }
                                Enter = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                        SoundEngine.Music.DungeonTheme();
                        SoundEngine.Sound.Teleport();
                        Rogue.RAM.Map.Level = Rogue.RAM.Map.Level + 1;
                        PlayEngine.EnemyMoves.Move(false);
                        //DrawEngine.GUIDraw.DrawGUI();
                        LabirinthEngine.Create(Rogue.RAM.Map.Level);//Shablon
                        //DrawEngine.GUIDraw.DrawLab();
                        PlayEngine.EnemyMoves.Move(true);
                        PlayEngine.GamePlay.Play();
                    }
                    if (Enter)
                    {
                        DrawEngine.InfoWindow.Message = "Вы открыли: " + this.Name;
                    }
                    else
                    {
                        DrawEngine.InfoWindow.Message = "Дверь заперта! Нужен ключ!";
                    }
                    return Enter;
                }
            }
        }
        /// <summary>
        /// Merchants and sellers
        /// </summary>
        public class Merchant : ActiveObject
        {
            public List<Item> Goods;

            public ConsoleColor SpeachColor;

            public char SpeachIcon;

            public int MaxGold,CurGold;

            public override bool Use()
            {
                PlayEngine.EnemyMoves.Move(false);
                Rogue.RAM.Merch = this;
                Rogue.RAM.MerchTab.MaxTab = this.Goods.Count;
                Rogue.RAM.MerchTab.NowTab = 1;
                DrawEngine.MerchantDraw.DrawGoodsWindow(true);
                PlayEngine.GamePlay.Merch();           
                PlayEngine.EnemyMoves.Move(true);
                DrawEngine.InfoWindow.Message = "Вы повстречали " + this.Name + " !";
                return false;
            }
        }
        /// <summary>
        /// Altar of Blessing
        /// </summary>
        public class Altar : ActiveObject
        {
            public new ConsoleColor Color = ConsoleColor.Yellow;

            public char MapIcon = '♣';

            public override bool Use()
            {
                Script();
                return false;
            }
        }
        /// <summary>
        /// Fountains uses for unlimited actions
        /// </summary>
        public class Fountain : ActiveObject
        {
            public DrawEngine.ColoredWord sStat;
            public AbilityStats aStat;
            public override bool Use()
            {
                SoundEngine.Sound.Drink();
                List<DrawEngine.ColoredWord> words = new List<DrawEngine.ColoredWord>();
                words.Add(new DrawEngine.ColoredWord() { Color = ConsoleColor.White, Word = "Вы полностью восстановили " });
                words.Add(sStat);
                words.Add(new DrawEngine.ColoredWord() { Color = ConsoleColor.White, Word = "!" });
                DrawEngine.InfoWindow.cMessage = words;
                switch (this.aStat)
                {
                    case AbilityStats.MHP: { Rogue.RAM.Player.CHP = Rogue.RAM.Player.MHP; break; }
                    case AbilityStats.MMP: { Rogue.RAM.Player.CMP = Rogue.RAM.Player.MMP; break; }
                }
                DrawEngine.GUIDraw.ReDrawCharStat();
                return false;
            }
        }
        /// <summary>
        /// new class only for new drawning
        /// </summary>
        public class Chest : ActiveObject
        {
            public List<Item> Items=new List<Item>();
            public void Clear()
            {
                this.Items.Clear();
            }
        }
        /// <summary>
        /// Questgivers
        /// </summary>
        public class Questgiver : ActiveObject
        {
            public Quest Quest;

            public ConsoleColor SpeachColor;

            public char SpeachIcon;

            public override bool Use()
            {
                PlayEngine.EnemyMoves.Move(false);
                Rogue.RAM.qGiver = this;
                DrawEngine.QuestgiverDraw.DrawGiverWindow();
                DrawEngine.InfoWindow.Warning = "Вы согласны принять данное задание? Y/N";
                PlayEngine.GamePlay.qGiver();
                Rogue.RAM.qGiver = null;
                PlayEngine.EnemyMoves.Move(true);
                DrawEngine.InfoWindow.Message = "Вы повстречали " + this.Name + " !";
                return true;
            }
        }

        public class CapitalDoor : ActiveObject
        {
            public CapitalDoor() { this.Move = false; }
            public int Quarter;
            public override bool Use()
            {
                if (this.Icon == '☻')
                {
                    DrawEngine.InfoWindow.Warning = "Квартал ярости закрыт на реконструкцию!";
                    return false;
                }
                else if (this.Icon == '☼')
                {
                    DrawEngine.InfoWindow.Warning = "Квартал братства закрыт на реконструкцию!";
                    return false;
                }
                else
                {
                    PlayEngine.EnemyMoves.Move(false);
                    LabirinthEngine.Create(1, true, this.Quarter);
                    PlayEngine.EnemyMoves.Move(true);
                    PlayEngine.GamePlay.Play();
                }
                return true;
            }

            public class GateKeeper : CapitalDoor
            {
                public int Location;
                public override bool Use()
                {
                    PlayEngine.EnemyMoves.Move(false);
                    this.Quarter = 2;
                    DrawEngine.ActiveObjectDraw.Draw(new List<string>() 
                    {
                        "{1} - Верхний зал",
                        "{2} - Зал огня",
                        "{3} - Зал воды",
                        "{4} - Зал земли",
                        "{5} - Зал воздуха",
                        "{6} - Нижний зал",
                        "{7} - Приёмная",
                    }, this);
                    if (PlayEngine.GateKeeperGamePlay.Main(this))
                    {                        
                        LabirinthEngine.Create(1, true, this.Quarter);
                        if (this.Quarter == 2)
                        {
                            Rogue.RAM.Map.Map[68][11].Player = null; Rogue.RAM.Map.Map[34][11].Vision = ' ';
                        }
                        int x = 0, y = 0;
                        switch (this.Location)
                        {
                            case 0: { x = 33; y = 3; break; }
                            case 1: { x = 5; y = 10; break; }
                            case 2: { x = 17; y = 8; break; }
                            case 3: { x = 33; y = 8; break; }
                            case 4: { x = 26; y = 13; break; }
                            case 5: { x = 38; y = 17; break; }
                            case 6: { x = 52; y = 11; break; }
                        }
                        Rogue.RAM.Map.Map[x][y].Player = Rogue.RAM.Player; Rogue.RAM.Map.Map[34][11].Vision = Rogue.RAM.Player.Icon;
                        PlayEngine.EnemyMoves.Move(true);
                        PlayEngine.GamePlay.Play();
                        return true;
                    }
                    else
                    {
                        PlayEngine.EnemyMoves.Move(true);
                        PlayEngine.GamePlay.Play(); 
                        return false;
                    }
                }
            }

            public class TownPortal : CapitalDoor
            {
                public override bool Use()
                {
                    //SoundEngine.CapitalTheme = true;
                    SoundEngine.Music.DungeonTheme();
                    PlayEngine.EnemyMoves.Move(false);
                    Rogue.RAM.Map = Rogue.RAM.InPortal;
                    Rogue.RAM.InPortal = null;
                    PlayEngine.EnemyMoves.Move(true);
                    PlayEngine.GamePlay.Play();
                    return true;
                }
            }
        }

        public delegate void Script();
        public delegate void iScript(bool Dress);

        public class Member : Merchant
        {
            public Int16 ForegroundColor;
            public Int16 BackgroundColor;
            public string Affix;
            public override bool Use()
            {
                if (PlayEngine.EnemyMoves.CheckMove) { PlayEngine.EnemyMoves.Move(false); }
                DrawEngine.InfoWindow.cMessage = new List<DrawEngine.ColoredWord>()
                {
                    new DrawEngine.ColoredWord(){ Word="Вы начинаете разговор с", Color=Rogue.RAM.Map.Biom},
                    new DrawEngine.ColoredWord(){ Word=this.Affix, Color=ConsoleColor.Yellow},
                    new DrawEngine.ColoredWord(){ Word=this.Name, Color=this.Color}
                };
                Script();
                if (!PlayEngine.EnemyMoves.CheckMove) { PlayEngine.EnemyMoves.Move(true); }
                DrawEngine.InfoWindow.cMessage = new List<DrawEngine.ColoredWord>()
                {
                    new DrawEngine.ColoredWord(){ Word="Вы закончили разговор с", Color=Rogue.RAM.Map.Biom},
                    new DrawEngine.ColoredWord(){ Word=this.Affix, Color=ConsoleColor.Yellow},
                    new DrawEngine.ColoredWord(){ Word=this.Name, Color=this.Color}
                };
                return false;
            }
        }

        public class NPC : ActiveObject
        {
            public string Affix;

            public override bool Use()
            {
                if (this.Name != "Подопытный") { SoundEngine.Sound.NPC(); }

                if (PlayEngine.EnemyMoves.CheckMove) { PlayEngine.EnemyMoves.Move(false); }
                DrawEngine.InfoWindow.cMessage = new List<DrawEngine.ColoredWord>()
                {
                    new DrawEngine.ColoredWord(){ Word="Вы начинаете разговор с", Color=Rogue.RAM.Map.Biom},
                    new DrawEngine.ColoredWord(){ Word=this.Affix, Color=ConsoleColor.Yellow},
                    new DrawEngine.ColoredWord(){ Word=this.Name, Color=this.Color}
                };
                Script();
                if (!PlayEngine.EnemyMoves.CheckMove) { PlayEngine.EnemyMoves.Move(true); }
                DrawEngine.InfoWindow.cMessage = new List<DrawEngine.ColoredWord>()
                {
                    new DrawEngine.ColoredWord(){ Word="Вы закончили разговор с", Color=Rogue.RAM.Map.Biom},
                    new DrawEngine.ColoredWord(){ Word=this.Affix, Color=ConsoleColor.Yellow},
                    new DrawEngine.ColoredWord(){ Word=this.Name, Color=this.Color}
                };
                return false;
            }

            //public Script Script;
        }

        /// <summary>
        /// Create quest
        /// </summary>
        public class Quest
        {
            public Quest()
            {
                M = new List<Monster>();
                I = new List<Item>();
            }
            /// <summary>
            /// Name of Quest
            /// </summary>
            public string Name;
            /// <summary>
            /// Difficult of quest
            /// </summary>
            public ConsoleColor Difficult;
            /// <summary>
            /// Color of icon
            /// </summary>
            public ConsoleColor Color;
            /// <summary>
            /// Icon
            /// </summary>
            public char Icon;
            /// <summary>
            /// Speach for questgiver
            /// </summary>
            public string Speach;
            /// <summary>
            /// Rewards
            /// </summary>
            public Reward Rewards;
            /// <summary>
            /// String target
            /// </summary>
            public string Target;
            /// <summary>
            /// Maximum target
            /// </summary>
            public int TargetCount;
            /// <summary>
            /// Progress of this quest
            /// </summary>
            public int Progress;
            /// <summary>
            /// For kill monsters progress
            /// </summary>
            public List<Monster> M;
            /// <summary>
            /// For items progress
            /// </summary>
            public List<Item> I;
            /// <summary>
            /// For other progress
            /// </summary>
            public int G;
            ///// <summary>
            ///// Timer for quest dids
            ///// </summary>
            //public System.Timers.Timer ActionTime = new System.Timers.Timer();
            ///// <summary>
            ///// Activate quest timer
            ///// </summary>
            //public bool Active
            //{
            //    set
            //    {
            //        if (value)
            //        {
            //            //Progress timer                        
            //            this.ProgressTimer.Interval = 1;
            //            foreach (SystemEngine.Timers t in Rogue.RAM.Timers)
            //            {
            //                if (t.Type == SystemEngine.TimerType.Pro && t.Activated == false)
            //                { this.ProgressTimer.Elapsed += this.QuestTimerProgress; t.Activated = true; }
            //                if (t.Type == SystemEngine.TimerType.Pro && t.Activated == true)
            //                { this.ProgressTimer.Elapsed -= this.QuestTimerProgress; this.ProgressTimer.Elapsed += this.QuestTimerProgress; }
            //            }
            //            this.ProgressTimer.Enabled = true;

            //            //Time timer 1000
            //            this.ActionTime.Interval = 300000;
            //            foreach (SystemEngine.Timers t in Rogue.RAM.Timers)
            //            {
            //                if (t.Type == SystemEngine.TimerType.Que && t.Activated == false)
            //                { this.ActionTime.Elapsed += this.QuestTimeIsOver; t.Activated = true; }
            //                if (t.Type == SystemEngine.TimerType.Que && t.Activated == true)
            //                { this.ActionTime.Elapsed -= this.QuestTimeIsOver; this.ActionTime.Elapsed += this.QuestTimeIsOver; }
            //            }
            //            this.ActionTime.Enabled = true;
            //        }
            //    }
            //}
            ///// <summary>
            ///// When quest time is over
            ///// </summary>
            ///// <param name="sender"></param>
            ///// <param name="e"></param>
            //private void QuestTimeIsOver(object sender, EventArgs e)
            //{
            //    DrawEngine.InfoWindow.Warning = "Вы не успели выполнить задание " + this.Name + "!";
            //    foreach (Quest Q in Rogue.RAM.Player.QuestBook) { if (Q == this) { Rogue.RAM.Player.QuestBook.Remove(this); break; } }
            //    this.ProgressTimer.Enabled = false;
            //    this.ActionTime.Enabled = false;
            //}
            ///// <summary>
            ///// Timer for progress of Quest
            ///// </summary>
            //public System.Timers.Timer ProgressTimer = new System.Timers.Timer();
            ///// <summary>
            ///// Tick progress
            ///// </summary>
            ///// <param name="sender"></param>
            ///// <param name="e"></param>
            //private void QuestTimerProgress(object sender, EventArgs e)
            //{
            //    if (this.CheckProgress)
            //    {
            //        DrawEngine.InfoWindow.Warning = "Вы успешно выполнили задание " + this.Name + "!";
            //        foreach (Quest Q in Rogue.RAM.Player.QuestBook) { if (Q == this) { Rogue.RAM.Player.QuestBook.Remove(this); break; } }
            //        this.ProgressTimer.Enabled = false;
            //        this.ActionTime.Enabled = false;
            //        this.GiveReward = true;
            //    }
            //}
            /// <summary>
            /// Give reward for character
            /// </summary>
            private bool GiveReward
            {
                set
                {
                    //foreach (Item i in this.Rewards.Items)
                    //{
                    //    PlayEngine.GamePlay.DropItem(i);
                    //}
                    ////Thread.Sleep(250);
                    //foreach (Perk p in this.Rewards.Perks)
                    //{
                    //    Perk.AddPerk(p.Bonus, p.Penalty, p.Name, p.History, p.Icon, p.Color);
                    //    DrawEngine.InfoWindow.Custom("Вам добавлена новая особенность - " + p.Name);
                    //}
                    ////Thread.Sleep(250);
                    //foreach (int e in this.Rewards.Exp)
                    //{ Rogue.RAM.Player.EXP += e; DrawEngine.InfoWindow.Message = "Вы получаете " + e.ToString() + " опыта!"; DrawEngine.GUIDraw.ReDrawCharStat(); }
                    ////Thread.Sleep(250);
                    //foreach (int g in this.Rewards.Gold)
                    //{ Rogue.RAM.Player.Gold += g; DrawEngine.InfoWindow.Message = "Вы получаете " + g.ToString() + " золота!"; DrawEngine.GUIDraw.ReDrawCharStat(); }
                    ////Thread.Sleep(250);
                }
            }
            /// <summary>
            /// check progress
            /// </summary>
            private bool CheckProgress
            {
                get
                {
                    if (this.Progress >= this.TargetCount) { return true; }
                    else { return false; }
                }
            }
        }
        /// <summary>
        /// Can use for quests, main quests, chests of treasure, etc. Any way when u need much objects to give character
        /// NO MORE 4 REWARDS!!! (item==item can be more 1; ex: Health potion x20)
        /// </summary>
        public class Reward
        {
            public Reward()
            { Items = new List<Item>(); Abilityes = new List<Ability>(); Perks = new List<Perk>(); Exp = new List<int>(); Gold = new List<int>(); }
            public List<Item> Items; public List<Ability> Abilityes; public List<Perk> Perks; public List<int> Exp; public List<int> Gold;
        }

        

        public class InventoryTab
        {
            public string Type;

            public int NowTab;

            public int MaxTab;
        }

        public class StepManager
        {
            public void Step()
            {
                for (int i = 0; i < Rogue.RAM.Player.Buffs.Count; i++)
                {
                    Rogue.RAM.Player.Buffs[i].Duration -= 1;

                    foreach (AbilityAction a in Rogue.RAM.Player.Buffs[i].Action)
                    {
                        if (a.Act == AbilityActionType.Heal || a.Act == AbilityActionType.Damage)
                        {
                            Rogue.RAM.Player.Buffs[i].UseEot(a);
                        }
                        if (a.Act == AbilityActionType.Debuff || a.Act == AbilityActionType.Improve)
                        {
                            Rogue.RAM.Player.Buffs[i].UseIot(a);
                        }
                        if (a.Act == AbilityActionType.Summon)
                        {
                            Rogue.RAM.Player.Buffs[i].UseSot(a);
                        }
                    }
                }

                if (Rogue.RAM.Enemy != null)
                {
                    foreach (Summoned s in Rogue.RAM.SummonedList)
                    {
                        s.Enabled = true;
                    }
                    for (int i = 0; i < Rogue.RAM.Enemy.DoT.Count; i++)
                    {
                        Rogue.RAM.Enemy.DoT[i].Duration -= 1;
                        foreach (AbilityAction a in Rogue.RAM.Enemy.DoT[i].Action)
                        {
                            if (a.Act == AbilityActionType.Heal || a.Act == AbilityActionType.Damage)
                            {
                                Rogue.RAM.Enemy.DoT[i].UseEot(a);
                            }
                            if (a.Act == AbilityActionType.Debuff || a.Act == AbilityActionType.Improve)
                            {
                                Rogue.RAM.Enemy.DoT[i].UseIot(a);
                            }
                        }
                    }
                }
            }
        }

        public class Reputation
        {
            public int min = 0;
            public int max = 0;
            public string name = "";
            public MonsterRace race = MonsterRace.Animal;
            public ConsoleColor biom = ConsoleColor.Black;
        }

        public class Biom
        {
            public string Map;
            public string Name;
            public string Affix;
            public ConsoleColor Color;
        }
    }
}
