using System;
using System.Collections.Generic;
using System.Text;

namespace Rogue.Abilities
{
    public class OldAbility
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
                rtrn = rtrn.Replace("<", "<" + (this.Duration / this.DHoTtiks).ToString());
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
                if (Rogue.RAM.Player.Class != BattleClass.Paladin && Rogue.RAM.Player.Class != BattleClass.Warlock)
                {
                    return Convert.ToInt32(Math.Round(((double)Rogue.RAM.Player.MMP / 100) * CostRate));
                }
                else if (Rogue.RAM.Player.Class == BattleClass.Paladin)
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
        public double CraftShance = 50;
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
                else { DrawEngine.InfoWindow.Message = "У вас не хватает ресурсов на использование способности!"; }
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
        public List<AbilityStats> Stats = new List<AbilityStats>();
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
            bool Success = false;
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
                                { Use(); }
                                break;
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
            if (Act.Atr.IndexOf(AbilityActionAttribute.DmgHealInstant) > -1)
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
            if (Act.Atr.IndexOf(AbilityActionAttribute.DmgHealOnTime) > -1)
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
            if (Act.Act == AbilityActionType.Damage && Act.Atr.IndexOf(AbilityActionAttribute.DmgHealOnTime) > -1)
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
            else if (Act.Act == AbilityActionType.Heal && Act.Atr.IndexOf(AbilityActionAttribute.DmgHealOnTime) > -1)
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
                                    aby.Stats.Add(AbilityStats.ARM);
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
                case SystemEngine.ArrowDirection.Top: { this.Banish(xpos, ypos - 1); break; }
                case SystemEngine.ArrowDirection.Bot: { this.Banish(xpos, ypos + 1); break; }
                case SystemEngine.ArrowDirection.Left: { this.Banish(xpos - 1, ypos); break; }
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
            resultOFresult = ((resultOFresult * 0.1) + (this.Level * 0.75));
            return resultOFresult;
        }
    }
}
