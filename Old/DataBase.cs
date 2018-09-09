using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace Rogue
{
    public static class DataBase
    {
        private static Random r = new Random();
        public static class ObjectBase
        {
            public static MechEngine.Fountain CapitalHpFountain
            {
                get
                {
                    MechEngine.Fountain f = new MechEngine.Fountain();
                    f.Name = "Фонтан здоровья";
                    f.Icon = '%';
                    f.Color = ConsoleColor.Red;
                    f.Move = false;
                    f.sStat = new DrawEngine.ColoredWord() { Color = ConsoleColor.Red, Word = "Здоровье" };
                    f.aStat = MechEngine.AbilityStats.MHP;
                    f.Info = "Магические фонтаны полностью восстанавливают характеристики. Этот фонтан полностью восстановит вам " + f.sStat.Word+".";
                    return f;
                }
            }

            public static MechEngine.Fountain CapitalMpFountain
            {
                get
                {
                    MechEngine.Fountain f = new MechEngine.Fountain();
                    f.Name = "Фонтан желаний";
                    f.Icon = '%';
                    f.Color = SystemEngine.Helper.Information.ClassC;
                    f.Move = false;
                    f.sStat = new DrawEngine.ColoredWord() { Color = SystemEngine.Helper.Information.ClassC, Word = Rogue.RAM.Player.ManaName.Replace(':', '\0') };
                    f.aStat = MechEngine.AbilityStats.MMP;
                    f.Info = "Магические фонтаны полностью восстанавливают характеристики. Этот фонтан полностью восстановит вам желаемое.";
                    return f;
                }
            }

            public static MechEngine.ActiveObject MouseDoor
            {
                get
                {
                    MechEngine.ActiveObject o = new MechEngine.ActiveObject();
                    o.Color = ConsoleColor.DarkGray;
                    o.Icon = '▄';
                    o.Info = "Здесь должна быть стена.";
                    o.Move = false;
                    o.Name = "Дыра в стене";
                    o.UseScript = true;
                    o.Script = () =>
                    {
                        PlayEngine.Enemy = false;
                        DrawEngine.Replica R = new DrawEngine.Replica();
                        R.Text = "С этого места дыра выглядит достаточно внушительной.";
                        R.TextColor = ConsoleColor.DarkGray;
                        R.Options.Add("[T] - Починить.");
                        R.Options.Add("[Escape] - Уйти.");
                        R.OptionsColor = ConsoleColor.DarkGray;
                        DrawEngine.ActiveObjectDraw.Draw(R, o);
                        bool end = false;
                        while (!end)
                        {
                            ConsoleKey push = Console.ReadKey(true).Key;
                            switch (push)
                            {
                                case ConsoleKey.T:
                                    {
                                        for (int i = 0; i < 60; i++)
                                        {
                                            DrawEngine.InfoWindow.Message = string.Format("Вы пытаетесь заделать дыру... Осталось {0} сек..", 60 - i);
                                            Thread.Sleep(1000);
                                        }

                                        MechEngine.Quest q = new MechEngine.Quest();
                                        foreach (MechEngine.Quest qq in Rogue.RAM.Player.QuestBook) { if (qq.Name == QuestBase.NPCQuestFEC.MouseDoor.Name) { q = qq; } }

                                        if (r.Next(2) == 0) { if (q.Progress < q.TargetCount) { q.Progress++; DrawEngine.InfoWindow.Message = "Вы починили часть стены!"; } }
                                        else { DrawEngine.InfoWindow.Message = "К сожалению, всё что вы делали в миг разрушилось..."; }

                                        if (q.Progress == q.TargetCount)
                                        {
                                            DrawEngine.InfoWindow.Warning = "Вы заделали дыру в стене!";
                                            Rogue.RAM.Map.Map[9][11].Object = null;
                                            Rogue.RAM.Map.Map[9][11].Wall = new MechEngine.Wall();
                                            Rogue.RAM.Map.Map[9][11].Vision = '#';
                                            DrawEngine.winAPIDraw.Helpers.ReDrawOneChar(9, 11, 9, 11, '\0', '#', 0, Convert.ToInt16(ConsoleColor.DarkGray));
                                            end = true;
                                        }

                                        break;
                                    }
                                case ConsoleKey.Escape: { end = true; break; }
                            }

                        }
                        PlayEngine.Enemy = true;
                    };
                    return o;
                }
            }

            public static MechEngine.Chest SelfChest
            {
                get
                {
                    MechEngine.Chest o = new MechEngine.Chest();
                    o.Color = ConsoleColor.Yellow;
                    o.Icon = '▄';
                    o.Info = "Ваша персональная сокровищница, вмещает 7 вещей.";
                    o.Move = false;
                    o.Name = "Сокровищница";
                    o.Items = Rogue.RAM.SelfChest;
                    o.UseScript = true;
                    o.Script = () =>
                        {
                            SoundEngine.Sound.OpenChest();
                            PlayEngine.Enemy = false;
                            int index = 0;
                            DrawEngine.FightDraw.DrawEnemyGUI();
                            DrawEngine.ActiveObjectDraw.DrawChest(o, index);
                            DrawEngine.CharMap.DrawCMap(new List<string>() 
                                { 
                                    "[↑] - Навигация ",
                                    "[↓] - Навигация ", 
                                    "[I] - Информация ",
                                    "[1-6] - Инвентарь ",
                                    "[Enter] - Забрать ",
                                    "[Escape] - Закрыть",
                                    "                 ",
                                    "                 ",                                    
                                });

                            bool end = false;
                            while (!end)
                            {
                                ConsoleKey key = Console.ReadKey(true).Key;
                                switch (key)
                                {
                                    case ConsoleKey.UpArrow: { if (index > 0) { index--; } DrawEngine.ActiveObjectDraw.DrawChest(o, index); break; }
                                    case ConsoleKey.DownArrow: { if (index < 6) { index++; } DrawEngine.ActiveObjectDraw.DrawChest(o, index); break; }
                                    case ConsoleKey.Enter: { try { if (Rogue.RAM.Player.InventorySlots) { MechEngine.Item it = o.Items[index]; Rogue.RAM.Player.Inventory.Add(it); o.Items.Remove(it); DrawEngine.GUIDraw.ReDrawCharInventory(); DrawEngine.ActiveObjectDraw.DrawChest(o, index); } else { DrawEngine.InfoWindow.Warning = "Инвентарь переполнен!"; } } catch { DrawEngine.InfoWindow.Warning = "Эта ячейка пустая!"; } break; }
                                    case ConsoleKey.I: { try { PlayEngine.GamePlay.GetInfo.Item = o.Items[index]; Console.ReadKey(true); DrawEngine.ActiveObjectDraw.DrawChest(o, index); } catch (ArgumentOutOfRangeException) { } break; }
                                    case ConsoleKey.D1:
                                    case ConsoleKey.NumPad1: { if (o.Items.Count < 7) { DrawEngine.InfoWindow.Message = "[Enter] - Положить предмет в сундук."; if (Console.ReadKey(true).Key == ConsoleKey.Enter) { try { MechEngine.Item it = Rogue.RAM.Player.Inventory[3]; o.Items.Add(it); Rogue.RAM.Player.Inventory.Remove(it); DrawEngine.GUIDraw.ReDrawCharInventory(); DrawEngine.ActiveObjectDraw.DrawChest(o, index); } catch (ArgumentOutOfRangeException) { } } } else { DrawEngine.InfoWindow.Warning = "Сокровищница переполнена!"; } break; }
                                    case ConsoleKey.D2:
                                    case ConsoleKey.NumPad2: { if (o.Items.Count < 7) { DrawEngine.InfoWindow.Message = "[Enter] - Положить предмет в сундук."; if (Console.ReadKey(true).Key == ConsoleKey.Enter) { try { MechEngine.Item it = Rogue.RAM.Player.Inventory[4]; o.Items.Add(it); Rogue.RAM.Player.Inventory.Remove(it); DrawEngine.GUIDraw.ReDrawCharInventory(); DrawEngine.ActiveObjectDraw.DrawChest(o, index); } catch (ArgumentOutOfRangeException) { } } } else { DrawEngine.InfoWindow.Warning = "Сокровищница переполнена!"; } break; }
                                    case ConsoleKey.D3:
                                    case ConsoleKey.NumPad3: { if (o.Items.Count < 7) { DrawEngine.InfoWindow.Message = "[Enter] - Положить предмет в сундук."; if (Console.ReadKey(true).Key == ConsoleKey.Enter) { try { MechEngine.Item it = Rogue.RAM.Player.Inventory[5]; o.Items.Add(it); Rogue.RAM.Player.Inventory.Remove(it); DrawEngine.GUIDraw.ReDrawCharInventory(); DrawEngine.ActiveObjectDraw.DrawChest(o, index); } catch (ArgumentOutOfRangeException) { } } } else { DrawEngine.InfoWindow.Warning = "Сокровищница переполнена!"; } break; }
                                    case ConsoleKey.D4:
                                    case ConsoleKey.NumPad4: { if (o.Items.Count < 7) { DrawEngine.InfoWindow.Message = "[Enter] - Положить предмет в сундук."; if (Console.ReadKey(true).Key == ConsoleKey.Enter) { try { MechEngine.Item it = Rogue.RAM.Player.Inventory[0]; o.Items.Add(it); Rogue.RAM.Player.Inventory.Remove(it); DrawEngine.GUIDraw.ReDrawCharInventory(); DrawEngine.ActiveObjectDraw.DrawChest(o, index); } catch (ArgumentOutOfRangeException) { } } } else { DrawEngine.InfoWindow.Warning = "Сокровищница переполнена!"; } break; }
                                    case ConsoleKey.D5:
                                    case ConsoleKey.NumPad5: { if (o.Items.Count < 7) { DrawEngine.InfoWindow.Message = "[Enter] - Положить предмет в сундук."; if (Console.ReadKey(true).Key == ConsoleKey.Enter) { try { MechEngine.Item it = Rogue.RAM.Player.Inventory[1]; o.Items.Add(it); Rogue.RAM.Player.Inventory.Remove(it); DrawEngine.GUIDraw.ReDrawCharInventory(); DrawEngine.ActiveObjectDraw.DrawChest(o, index); } catch (ArgumentOutOfRangeException) { } } } else { DrawEngine.InfoWindow.Warning = "Сокровищница переполнена!"; } break; }
                                    case ConsoleKey.D6:
                                    case ConsoleKey.NumPad6: { if (o.Items.Count < 7) { DrawEngine.InfoWindow.Message = "[Enter] - Положить предмет в сундук."; if (Console.ReadKey(true).Key == ConsoleKey.Enter) { try { MechEngine.Item it = Rogue.RAM.Player.Inventory[2]; o.Items.Add(it); Rogue.RAM.Player.Inventory.Remove(it); DrawEngine.GUIDraw.ReDrawCharInventory(); DrawEngine.ActiveObjectDraw.DrawChest(o, index); } catch (ArgumentOutOfRangeException) { } } } else { DrawEngine.InfoWindow.Warning = "Сокровищница переполнена!"; } break; }
                                    case ConsoleKey.Escape: { end = true; break; }
                                }
                            }
                            PlayEngine.Enemy = true;
                        };
                    return o;
                }
            }

            public static MechEngine.NPC WorldKeeper
            {
                get
                {
                    if (!(Rogue.RAM.Player.Buffs.IndexOf(DataBase.OtherAbilityBase.CatJoin) > -1))
                    {
                        MechEngine.NPC n = new MechEngine.NPC();
                        n.Icon = '♀';
                        n.Color = ConsoleColor.DarkYellow;
                        n.Name = "Илиус";
                        n.Info = "Дипломат Илиус может показать как к вам относятся другие фракции.";
                        n.Affix = "Дипломат";
                        n.Move = false;
                        #region Script
                        n.Script += delegate()
                        {
                            PlayEngine.Enemy = false;
                            DrawEngine.Replica R = new DrawEngine.Replica();
                            R.Text = "Приветствую тебя, сейчас твои дипломатические дела отражаются на репутации вот так.";
                            R.TextColor = ConsoleColor.DarkCyan;
                            R.Options.Add("[Escape] - Уйти");
                            R.OptionsColor = ConsoleColor.DarkCyan;
                            DrawEngine.ActiveObjectDraw.Draw(R, n);
                            DrawEngine.CharacterDraw.DrawRepute();
                            bool EndDialogue = false;
                            while (!EndDialogue)
                            {
                                ConsoleKey push = Console.ReadKey(true).Key;
                                switch (push)
                                {
                                    case ConsoleKey.Escape:
                                        {
                                            EndDialogue = true;
                                            break;
                                        }
                                }
                            }
                            PlayEngine.Enemy = true;
                        };
                        #endregion
                        return n;
                    }
                    else { return null; }
                }
            }
        }

        public static class UsableObjects
        {
            public static MechEngine.ActiveObject RandomObject
            {
                get
                {
                    switch (r.Next(8))
                    {
                        case 0: { return HpFountain; }
                        case 1: { return VeronaAltar; }
                        case 2:
                            {
                                foreach (MechEngine.Quest qq in Rogue.RAM.Player.QuestBook)
                                {
                                    if (qq.Name == QuestBase.NPCQuestFEC.ColdOrb.Name) { return DeadValkyrie; }
                                    else { return VeronaAltar; }
                                }
                                return VeronaAltar;
                            }
                        case 3: { return MercenaryGuild; }
                        case 4: { return WisdomWitch; }
                        case 5: { if (Rogue.RAM.Map.Biom == ConsoleColor.Gray) { return MembersBase.Dwarf; } else { return HpFountain; } }
                        case 6: { return Beaconn; }
                        case 7: { return TransformAltar; }
                        default: { return HpFountain; }
                    }
                }
            }

            public static MechEngine.Fountain HpFountain
            {
                get
                {
                    MechEngine.Fountain f = new MechEngine.Fountain();
                    f.Name = "Фонтан здоровья";
                    f.Icon = '%';
                    f.Color = ConsoleColor.Red;
                    f.Move = false;
                    f.sStat = new DrawEngine.ColoredWord() { Color = ConsoleColor.Red, Word = "Здоровье" };
                    f.aStat = MechEngine.AbilityStats.MHP;
                    f.Info = "Магические фонтаны полностью восстанавливают характеристики. Этот фонтан полностью восстановит вам " + f.sStat.Word + ".";
                    return f;
                }
            }

            public static MechEngine.ActiveObject DeadValkyrie
            {
                get
                {
                    MechEngine.ActiveObject o = new MechEngine.ActiveObject();
                    o.Color = ConsoleColor.DarkCyan;
                    o.Icon = '†';
                    o.Info = "Труп валькирии. Внутри её тела вы видите разгорающуюся магическую искру.";
                    o.Move = false;
                    o.Name = "Труп валькирии";
                    o.UseScript = true;
                    o.Script = () =>
                    {
                        PlayEngine.Enemy = false;
                        #region Quest
                        int index = 0;
                        bool delete = false;
                        foreach (MechEngine.Quest qq in Rogue.RAM.Player.QuestBook)
                        {
                            if (qq.Name == QuestBase.NPCQuestFEC.ColdOrb.Name)
                            {
                                List<DrawEngine.ColoredWord> cq = new List<DrawEngine.ColoredWord>();
                                cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = "Вы выполнили задание - " });
                                cq.Add(new DrawEngine.ColoredWord() { Color = QuestBase.NPCQuestFEC.ColdOrb.Color, Word = QuestBase.NPCQuestFEC.ColdOrb.Name });
                                cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = " !" });
                                DrawEngine.InfoWindow.cMessage = cq;
                                Thread.Sleep(410);
                                Rogue.RAM.Player.QuestBook.Add(QuestBase.NPCQuestFEC.StayValkyrie);
                                index = Rogue.RAM.Player.QuestBook.IndexOf(qq);
                                delete = true;
                                cq = new List<DrawEngine.ColoredWord>();
                                cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = "Вы получаете новое задание - " });
                                cq.Add(new DrawEngine.ColoredWord() { Color = QuestBase.NPCQuestFEC.StayValkyrie.Color, Word = QuestBase.NPCQuestFEC.StayValkyrie.Name });
                                cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = " !" });
                                DrawEngine.InfoWindow.cMessage = cq;
                            }
                        }
                        if (delete) { Rogue.RAM.Player.QuestBook.Remove(Rogue.RAM.Player.QuestBook[index]); }
                        #endregion
                        PlayEngine.Enemy = true;
                    };
                    return o;
                }
            }

            public static MechEngine.ActiveObject VeronaAltar
            {
                get
                {
                    MechEngine.ActiveObject a = new MechEngine.ActiveObject();
                    a.Move = false;
                    a.UseScript = true;
                    a.Color = ConsoleColor.Yellow;
                    a.Name = "Алтарь Вероны";
                    a.Icon = '♣';
                    a.Info = "Алтарь богини мести - Вероны. Она была живым оружием в руках богов.";
                    #region Script
                    a.Script = () =>
                    {
                        PlayEngine.Enemy = false;
                        DrawEngine.Replica R = new DrawEngine.Replica();
                        if (Rogue.RAM.Player.Inventory.Count != 0) { R.Text = "Вы можете пожертвовать Вероне одну из ваших вещей на выбор:"; }
                        else { R.Text = "У вас нечего принести в жертву Вероне!"; }
                        R.TextColor = ConsoleColor.DarkCyan;
                        for (int i = 0; i < Rogue.RAM.Player.Inventory.Count; i++)
                        {
                            R.Options.Add(string.Format("{0} - {1}", i + 1, Rogue.RAM.Player.Inventory[i].Name));
                        }
                        R.Options.Add("[Escape] - Уйти");
                        R.OptionsColor = ConsoleColor.DarkCyan;
                        DrawEngine.ActiveObjectDraw.Draw(R, a);
                        //DrawEngine.CharacterDraw.DrawRepute();
                        bool EndDialogue = false;
                        int index = -1;

                        #region Sacrifice
                        MechEngine.Script Sacrifice;
                        Sacrifice = () =>
                        {
                            SoundEngine.Sound.DestroyItem();
                            List<DrawEngine.ColoredWord> m = new List<DrawEngine.ColoredWord>();
                            m.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = "Вы приносите в жертву" });
                            m.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Player.Inventory[index].Color, Word = Rogue.RAM.Player.Inventory[index].Name });
                            m.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = "!" });
                            DrawEngine.InfoWindow.cMessage = m;
                            int gs = Rogue.RAM.Player.Inventory[index].ILvl;
                            Rogue.RAM.Player.Inventory.Remove(Rogue.RAM.Player.Inventory[index]);
                            switch (r.Next(10))
                            {
                                case 0:
                                case 1:
                                case 2:
                                case 3:
                                case 4:
                                    {
                                        SoundEngine.Sound.TakeItem();
                                        Rogue.RAM.Player.Gold += gs * 2;
                                        m.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = "Верона дарует вам" });
                                        m.Add(new DrawEngine.ColoredWord() { Color = ConsoleColor.Yellow, Word = (gs * 2).ToString() });
                                        m.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = "золота!" });
                                        DrawEngine.InfoWindow.cMessage = m;
                                        break;
                                    }
                                case 5:
                                case 6:
                                case 7:
                                case 8:
                                    {
                                        SoundEngine.Sound.TakeItem();
                                        MechEngine.Item it = DataBase.ItemBase.GetItem;
                                        Rogue.RAM.Player.Inventory.Add(it);
                                        m.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = "Верона дарует вам" });
                                        m.Add(new DrawEngine.ColoredWord() { Color = it.Color, Word = it.Name });
                                        m.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = "!" });
                                        DrawEngine.InfoWindow.cMessage = m;
                                        break;
                                    }
                                case 9:
                                    {
                                        SoundEngine.Sound.Teleport();
                                        Rogue.RAM.Player.AbPoint++;
                                        m.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = "Верона дарует вам" });
                                        m.Add(new DrawEngine.ColoredWord() { Color = ConsoleColor.Magenta, Word = "Очки навыков (1)" });
                                        m.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = "!" });
                                        DrawEngine.InfoWindow.cMessage = m;
                                        break;
                                    }
                            }
                        };
                        #endregion

                        while (!EndDialogue)
                        {
                            ConsoleKey push = Console.ReadKey(true).Key;
                            switch (push)
                            {
                                case ConsoleKey.D1:
                                case ConsoleKey.NumPad1: { if (R.Options.Count >= 2) { index = 0; Sacrifice(); EndDialogue = true; } break; }
                                case ConsoleKey.D2:
                                case ConsoleKey.NumPad2: { if (R.Options.Count >= 3) { index = 1; Sacrifice(); EndDialogue = true; } break; }
                                case ConsoleKey.D3:
                                case ConsoleKey.NumPad3: { if (R.Options.Count >= 4) { index = 2; Sacrifice(); EndDialogue = true; } break; }
                                case ConsoleKey.D4:
                                case ConsoleKey.NumPad4: { if (R.Options.Count >= 5) { index = 3; Sacrifice(); EndDialogue = true; } break; }
                                case ConsoleKey.D5:
                                case ConsoleKey.NumPad5: { if (R.Options.Count >= 6) { index = 4; Sacrifice(); EndDialogue = true; } break; }
                                case ConsoleKey.D6:
                                case ConsoleKey.NumPad6: { if (R.Options.Count >= 7) { index = 5; Sacrifice(); EndDialogue = true; } break; }
                                case ConsoleKey.Escape:
                                    {
                                        EndDialogue = true;
                                        break;
                                    }
                            }
                        }
                        PlayEngine.Enemy = true;
                    };
                    #endregion
                    return a;
                }
            }

            public static MechEngine.ActiveObject MercenaryGuild
            {
                get
                {
                    MechEngine.ActiveObject a = new MechEngine.ActiveObject();
                    a.Move = false;
                    a.UseScript = true;
                    a.Color = ConsoleColor.Cyan;
                    a.Name = "Гильдия наёмников";
                    a.Icon = '♣';
                    a.Info = "Здесь вы можете нанять себе помошника за золото.";
                    #region Script
                    a.Script = () =>
                    {
                        PlayEngine.Enemy = false;
                        DrawEngine.Replica R = new DrawEngine.Replica();
                        R.Text = "Выберите себе наёмника, и он будет помогать вам. Цена наёмника - " + Rogue.RAM.Map.Level * 50;
                        R.TextColor = ConsoleColor.DarkCyan;
                        R.Options.Add("[H] - Лекарь");
                        R.Options.Add("[W] - Воин");
                        R.Options.Add("[M] - Маг");
                        R.Options.Add("[Escape] - Уйти");
                        R.OptionsColor = ConsoleColor.DarkCyan;
                        DrawEngine.ActiveObjectDraw.Draw(R, a);
                        //DrawEngine.CharacterDraw.DrawRepute();
                        bool EndDialogue = false;
                        while (!EndDialogue)
                        {
                            ConsoleKey push = Console.ReadKey(true).Key;
                            switch (push)
                            {
                                case ConsoleKey.H:
                                    {
                                        if (Rogue.RAM.Player.Gold >= Rogue.RAM.Map.Level * 50)
                                        {
                                            Rogue.RAM.Player.Gold -= Rogue.RAM.Map.Level * 50;
                                            var s=OtherAbilityBase.MercenaryHeal;
                                            s.Activate();
                                            List<DrawEngine.ColoredWord> m = new List<DrawEngine.ColoredWord>();
                                            m.Add(new DrawEngine.ColoredWord() { Color = s.Color, Word = s.SummonMonster.Name });
                                            m.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = "присоединяется к вам!" });
                                            DrawEngine.InfoWindow.cMessage = m;
                                            DrawEngine.GUIDraw.ReDrawCharStat();
                                            EndDialogue = true;
                                        }
                                        else
                                        { DrawEngine.InfoWindow.Warning = "У вас не хватает золота для найма!"; }
                                        break;
                                    }
                                case ConsoleKey.W:
                                    {
                                        if (Rogue.RAM.Player.Gold >= Rogue.RAM.Map.Level * 50)
                                        {
                                            Rogue.RAM.Player.Gold -= Rogue.RAM.Map.Level * 50;
                                            var s = OtherAbilityBase.MercenaryWarrior;
                                            s.Activate();
                                            List<DrawEngine.ColoredWord> m = new List<DrawEngine.ColoredWord>();
                                            m.Add(new DrawEngine.ColoredWord() { Color = s.Color, Word = s.SummonMonster.Name });
                                            m.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = "присоединяется к вам!" });
                                            DrawEngine.InfoWindow.cMessage = m;
                                            DrawEngine.GUIDraw.ReDrawCharStat();
                                            EndDialogue = true;
                                        }
                                        else
                                        { DrawEngine.InfoWindow.Warning = "У вас не хватает золота для найма!"; }
                                        break;
                                    }
                                case ConsoleKey.M:
                                    {
                                        if (Rogue.RAM.Player.Gold >= Rogue.RAM.Map.Level * 50)
                                        {
                                            Rogue.RAM.Player.Gold -= Rogue.RAM.Map.Level * 50;
                                            var s = OtherAbilityBase.MercenaryMage;
                                            s.Activate();
                                            List<DrawEngine.ColoredWord> m = new List<DrawEngine.ColoredWord>();
                                            m.Add(new DrawEngine.ColoredWord() { Color = s.Color, Word = s.SummonMonster.Name });
                                            m.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = "присоединяется к вам!" });
                                            DrawEngine.InfoWindow.cMessage = m;
                                            DrawEngine.GUIDraw.ReDrawCharStat();
                                            EndDialogue = true;
                                        }
                                        else
                                        { DrawEngine.InfoWindow.Warning = "У вас не хватает золота для найма!"; }
                                        break;
                                    }
                                case ConsoleKey.Escape:
                                    {
                                        EndDialogue = true;
                                        break;
                                    }
                            }
                        }
                        PlayEngine.Enemy = true;
                    };
                    #endregion
                    return a;
                }
            }

            public static MechEngine.ActiveObject WisdomWitch
            {
                get
                {
                    MechEngine.ActiveObject a = new MechEngine.ActiveObject();
                    a.Move = false;
                    a.UseScript = true;
                    a.Color = ConsoleColor.Green;
                    a.Name = "Жилище ведьмы";
                    a.Icon = '♣';
                    a.Info = "Молодая ведьма поможет вам советом.";
                    #region Script
                    a.Script = () =>
                    {
                        PlayEngine.Enemy = false;
                        DrawEngine.Replica R = new DrawEngine.Replica();
                        R.Text = "Приветствую тебя, "+Rogue.RAM.Player.Name+" ! Я могу помочь тебе советом, это будет стоить 25 монет.";
                        R.TextColor = ConsoleColor.DarkCyan;
                        R.Options.Add("[I] - Вещи");
                        R.Options.Add("[A] - Навыки");
                        R.Options.Add("[Q] - Жизнь");
                        R.Options.Add("[Escape] - Уйти");
                        R.OptionsColor = ConsoleColor.DarkCyan;
                        DrawEngine.ActiveObjectDraw.Draw(R, a);
                        //DrawEngine.CharacterDraw.DrawRepute();
                        bool EndDialogue = false;
                        while (!EndDialogue)
                        {
                            ConsoleKey push = Console.ReadKey(true).Key;
                            switch (push)
                            {
                                case ConsoleKey.I:
                                    {
                                        if (Rogue.RAM.Player.Gold >= 25)
                                        {
                                            SoundEngine.Sound.DropItem();
                                            DrawEngine.GUIDraw.ReDrawCharStat();
                                            Rogue.RAM.Player.Gold -= 25;
                                            string stat = "";
                                            switch (Rogue.RAM.Player.Class)
                                            {
                                                case MechEngine.BattleClass.Assassin: { stat = "<Силой атаки>"; break; }
                                                case MechEngine.BattleClass.Inquisitor: { stat = "<Силой атаки> и <Магической силой>"; break; }
                                                case MechEngine.BattleClass.LightWarrior: { stat = "<Силой атаки> и <Магической силой>"; break; }
                                                case MechEngine.BattleClass.Monk: { stat = "<Силой атаки>"; break; }
                                                case MechEngine.BattleClass.Paladin: { stat = "<Магической силой> и <Маной>"; break; }
                                                case MechEngine.BattleClass.Shaman: { stat = "<Маной>"; break; }
                                                case MechEngine.BattleClass.Valkyrie: { stat = "<Холодом>"; break; }
                                                case MechEngine.BattleClass.Warrior: { stat = "<Силой атаки>"; break; }
                                                default: { stat = "<Магической силой>"; break; }
                                            }
                                            R.Text = "Ищи вещи с " + stat+". Это поможет тебе в бою! Ещё совет?";
                                            DrawEngine.ActiveObjectDraw.Draw(R, a);

                                        }
                                        else
                                        { DrawEngine.InfoWindow.Message = "Ведьма просит не так уж и много золота!"; }
                                        break;
                                    }
                                case ConsoleKey.A:
                                    {
                                        if (Rogue.RAM.Player.Gold >= 25)
                                        {
                                            SoundEngine.Sound.DropItem();
                                            DrawEngine.GUIDraw.ReDrawCharStat();
                                            Rogue.RAM.Player.Gold -= 25;
                                            string stat = "";
                                            switch (Rogue.RAM.Player.Class)
                                            {
                                                case MechEngine.BattleClass.Assassin: { stat = "Ты лучше меня разбираешься в <ядах>, так что используй их!"; break; }
                                                case MechEngine.BattleClass.Inquisitor: { stat = "В зависимости от силы, используй - <Удар ангела> или <Удар демона>."; break; }
                                                case MechEngine.BattleClass.LightWarrior: { stat = "Невозможно увидеть твою судьбу."; break; }
                                                case MechEngine.BattleClass.Monk: { stat = "Если ты не любишь посохи, используй <Каменный кулак>."; break; }
                                                case MechEngine.BattleClass.Paladin: { stat = "Настоящий праведник сам знает как ему поступить."; break; }
                                                case MechEngine.BattleClass.Shaman: { stat = "Не забывай использовать силы природы и копить ману."; break; }
                                                case MechEngine.BattleClass.Valkyrie: { stat = "Невозможно увидеть твою судьбу."; break; }
                                                case MechEngine.BattleClass.Warrior: { stat = "<Усмиряй>, а затем <Добивай> своих врагов."; break; }
                                                case MechEngine.BattleClass.Alchemist: { stat = "Твоя судьба загадочна, но <Радужные брызги> тебе точно помогут!"; break; }
                                                case MechEngine.BattleClass.BloodMage: { stat = "Балансируй между тем что бы стать <Вампиром> и <Вурдалаком>."; break; }
                                                case MechEngine.BattleClass.FireMage: { stat = "Ты спрашиваем меня о таком? Просто <Жги>!"; break; }
                                                case MechEngine.BattleClass.Necromant: { stat = "Не забывай что твои истинные друзья - <Скелет> и <Призрак>."; break; }
                                                case MechEngine.BattleClass.Warlock: { stat = "Просто. <Убей>. Их. <Всех>."; break; }
                                            }
                                            R.Text = stat + "Ещё совет?";
                                            DrawEngine.ActiveObjectDraw.Draw(R, a);
                                        }
                                        else
                                        { DrawEngine.InfoWindow.Message = "Ведьма просит не так уж и много золота!"; }
                                        break;
                                    }
                                case ConsoleKey.Q:
                                    {
                                        if (Rogue.RAM.Player.Gold >= 25)
                                        {
                                            SoundEngine.Sound.DropItem();
                                            DrawEngine.GUIDraw.ReDrawCharStat();
                                            R.Text = "Твоя главная задача - найти и убить <Валорана>. Ещё совет?";
                                            DrawEngine.ActiveObjectDraw.Draw(R, a);
                                        }
                                        else
                                        { DrawEngine.InfoWindow.Message = "Ведьма просит не так уж и много золота!"; }
                                        break;
                                    }
                                case ConsoleKey.Escape:
                                    {
                                        EndDialogue = true;
                                        break;
                                    }
                            }
                        }
                        PlayEngine.Enemy = true;
                    };
                    #endregion
                    return a;
                }
            }

            public static MechEngine.ActiveObject Beaconn
            {
                get
                {
                    MechEngine.ActiveObject a = new MechEngine.ActiveObject();
                    a.Move = false;
                    a.UseScript = true;
                    a.Color = ConsoleColor.Blue;
                    a.Name = "Магический маяк";
                    a.Icon = '!';
                    a.Info = "Старый маяк магов, с его помощью можно переместиться в Мраумир.";
                    #region Script
                    a.Script = () =>
                    {
                        PlayEngine.Enemy = false;
                        
                        Rogue.RAM.InPortal = Rogue.RAM.Map;

                        LabirinthEngine.Create(1, true);

                        PlayEngine.Enemy = true;

                        SoundEngine.Music.TownTheme();
                        PlayEngine.GamePlay.Play();                        
                    };
                    #endregion
                    return a;
                }
            }

            public static MechEngine.ActiveObject TransformAltar
            {
                get
                {
                    MechEngine.ActiveObject n = new MechEngine.ActiveObject();
                    n.Color = ConsoleColor.DarkCyan;
                    n.Icon = '‼';
                    n.Info = "Такие алтари могут превращать одни ресурсы в другие, однако ими надо уметь пользоваться.";
                    n.Move = false;
                    n.Name = "Алтарь трансформ";
                    n.UseScript = true;
                    n.Script = () =>
                    {
                        PlayEngine.Enemy = false;
                        #region var
                        bool have = false;
                        bool end = false;
                        #endregion
                        #region quest
                        if (Rogue.RAM.Flags.Scrijal) { have = true; }
                        if (Rogue.RAM.Flags.AllTransform) { end = true; }
                        #endregion
                        #region replica
                        DrawEngine.Replica R = new DrawEngine.Replica();

                        if (!have)
                        {
                            R.Text = "Вы не умеете пользоваться алтарем.";
                            R.TextColor = ConsoleColor.DarkCyan;
                            R.Options.Add("[Escape] - Уйти.");
                            R.OptionsColor = ConsoleColor.DarkCyan;
                            DrawEngine.ActiveObjectDraw.Draw(R, n);
                        }
                        else
                        {
                            if (end)
                            {
                                R.Text = "Выберите что хотите трансформировать:";
                                R.TextColor = ConsoleColor.DarkCyan;
                                R.Options.Add("[I] - Железо.");
                                R.Options.Add("[W] - Дерево.");
                                R.Options.Add("[R] - Роза.");
                                R.Options.Add("[Escape] - Уйти.");
                                R.OptionsColor = ConsoleColor.DarkCyan;
                                DrawEngine.ActiveObjectDraw.Draw(R, n);
                            }
                            else
                            {
                                R.Text = "Выберите что хотите сделать с помощью скрижали:";
                                R.TextColor = ConsoleColor.DarkCyan;
                                R.Options.Add("[I] - Железо.");
                                R.Options.Add("[W] - Дерево.");
                                R.Options.Add("[R] - Роза.");
                                R.Options.Add("[S] - Отправить.");
                                R.Options.Add("[Escape] - Уйти.");
                                R.OptionsColor = ConsoleColor.DarkCyan;
                                DrawEngine.ActiveObjectDraw.Draw(R, n);
                            }
                        }
                        #endregion
                        #region main cycle
                        int index = -1;
                        bool EndDialogue = false;
                        while (!EndDialogue)
                        {
                            ConsoleKey push = Console.ReadKey(true).Key;
                            switch (push)
                            {
                                case ConsoleKey.I:
                                    {
                                        if (have)
                                        {
                                            if (Rogue.RAM.Player.CheckItem(ResourseBase.Iron) > -1)
                                            {
                                                Rogue.RAM.Player.Inventory.Remove(Rogue.RAM.Player.Inventory[Rogue.RAM.Player.CheckItem(ResourseBase.Iron)]);
                                                SoundEngine.Sound.DropItem();
                                                Rogue.RAM.Player.Inventory.Add(ResourseBase.Diamond);
                                                SoundEngine.Sound.TakeItem();
                                                DrawEngine.GUIDraw.ReDrawCharInventory();
                                                DrawEngine.InfoWindow.cMessage = new List<DrawEngine.ColoredWord>()
                                                {
                                                    new DrawEngine.ColoredWord(){ Color=Rogue.RAM.Map.Biom, Word="Вы превращаете"},
                                                    new DrawEngine.ColoredWord(){ Color=ResourseBase.Iron.Color, Word=ResourseBase.Iron.Name},
                                                    new DrawEngine.ColoredWord(){ Color=Rogue.RAM.Map.Biom, Word="в"},
                                                    new DrawEngine.ColoredWord(){ Color=ResourseBase.Diamond.Color, Word=ResourseBase.Diamond.Name},
                                                    new DrawEngine.ColoredWord(){ Color=Rogue.RAM.Map.Biom, Word="!"}
                                                };
                                                foreach (MechEngine.Quest q in Rogue.RAM.Player.QuestBook)
                                                {
                                                    if (q.Name == QuestBase.NPCQuestFEC.IronToDiamond.Name)
                                                    {
                                                        index = Rogue.RAM.Player.QuestBook.IndexOf(q);
                                                    }
                                                }
                                                Rogue.RAM.Player.QuestBook.Remove(Rogue.RAM.Player.QuestBook[index]);
                                                List<DrawEngine.ColoredWord> cq = new List<DrawEngine.ColoredWord>();
                                                cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = "Вы выполнили задание - " });
                                                cq.Add(new DrawEngine.ColoredWord() { Color = QuestBase.NPCQuestFEC.IronToDiamond.Color, Word = QuestBase.NPCQuestFEC.IronToDiamond.Name });
                                                cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = " !" });
                                                DrawEngine.InfoWindow.cMessage = cq;
                                            }
                                            else
                                            { DrawEngine.InfoWindow.Warning = "У вас нет нужного предмета для трансформации!"; }
                                        }
                                        break;
                                    }
                                case ConsoleKey.W:
                                    {
                                        if (have)
                                        {
                                            if (Rogue.RAM.Player.CheckItem(ResourseBase.Wood) > -1)
                                            {
                                                Rogue.RAM.Player.Inventory.Remove(Rogue.RAM.Player.Inventory[Rogue.RAM.Player.CheckItem(ResourseBase.Wood)]);
                                                SoundEngine.Sound.DropItem();
                                                Rogue.RAM.Player.Inventory.Add(ResourseBase.Glass);
                                                SoundEngine.Sound.TakeItem();
                                                DrawEngine.GUIDraw.ReDrawCharInventory();
                                                DrawEngine.InfoWindow.cMessage = new List<DrawEngine.ColoredWord>()
                                                {
                                                    new DrawEngine.ColoredWord(){ Color=Rogue.RAM.Map.Biom, Word="Вы превращаете"},
                                                    new DrawEngine.ColoredWord(){ Color=ResourseBase.Wood.Color, Word=ResourseBase.Wood.Name},
                                                    new DrawEngine.ColoredWord(){ Color=Rogue.RAM.Map.Biom, Word="в"},
                                                    new DrawEngine.ColoredWord(){ Color=ResourseBase.Glass.Color, Word=ResourseBase.Glass.Name},
                                                    new DrawEngine.ColoredWord(){ Color=Rogue.RAM.Map.Biom, Word="!"}
                                                };
                                                foreach (MechEngine.Quest q in Rogue.RAM.Player.QuestBook)
                                                {
                                                    if (q.Name == QuestBase.NPCQuestFEC.WoodToGlass.Name)
                                                    {
                                                        index = Rogue.RAM.Player.QuestBook.IndexOf(q);
                                                    }
                                                }
                                                Rogue.RAM.Player.QuestBook.Remove(Rogue.RAM.Player.QuestBook[index]);
                                                List<DrawEngine.ColoredWord> cq = new List<DrawEngine.ColoredWord>();
                                                cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = "Вы выполнили задание - " });
                                                cq.Add(new DrawEngine.ColoredWord() { Color = QuestBase.NPCQuestFEC.WoodToGlass.Color, Word = QuestBase.NPCQuestFEC.WoodToGlass.Name });
                                                cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = " !" });
                                                DrawEngine.InfoWindow.cMessage = cq;
                                            }
                                            else
                                            { DrawEngine.InfoWindow.Warning = "У вас нет нужного предмета для трансформации!"; }
                                        }
                                        break;
                                    }
                                case ConsoleKey.R:
                                    {
                                        if (have)
                                        {
                                            if (Rogue.RAM.Player.CheckItem(ResourseBase.DeadRose) > -1)
                                            {
                                                Rogue.RAM.Player.Inventory.Remove(Rogue.RAM.Player.Inventory[Rogue.RAM.Player.CheckItem(ResourseBase.DeadRose)]);
                                                SoundEngine.Sound.DropItem();
                                                Rogue.RAM.Player.Inventory.Add(ResourseBase.LifeRose);
                                                SoundEngine.Sound.TakeItem();
                                                DrawEngine.GUIDraw.ReDrawCharInventory();
                                                DrawEngine.InfoWindow.cMessage = new List<DrawEngine.ColoredWord>()
                                                {
                                                    new DrawEngine.ColoredWord(){ Color=Rogue.RAM.Map.Biom, Word="Вы превращаете"},
                                                    new DrawEngine.ColoredWord(){ Color=ResourseBase.DeadRose.Color, Word=ResourseBase.DeadRose.Name},
                                                    new DrawEngine.ColoredWord(){ Color=Rogue.RAM.Map.Biom, Word="в"},
                                                    new DrawEngine.ColoredWord(){ Color=ResourseBase.LifeRose.Color, Word=ResourseBase.LifeRose.Name},
                                                    new DrawEngine.ColoredWord(){ Color=Rogue.RAM.Map.Biom, Word="!"}
                                                };
                                                foreach (MechEngine.Quest q in Rogue.RAM.Player.QuestBook)
                                                {
                                                    if (q.Name == QuestBase.NPCQuestFEC.DeathToLife.Name)
                                                    {
                                                        index = Rogue.RAM.Player.QuestBook.IndexOf(q);
                                                    }
                                                }
                                                Rogue.RAM.Player.QuestBook.Remove(Rogue.RAM.Player.QuestBook[index]);
                                                List<DrawEngine.ColoredWord> cq = new List<DrawEngine.ColoredWord>();
                                                cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = "Вы выполнили задание - " });
                                                cq.Add(new DrawEngine.ColoredWord() { Color = QuestBase.NPCQuestFEC.DeathToLife.Color, Word = QuestBase.NPCQuestFEC.DeathToLife.Name });
                                                cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = " !" });
                                                DrawEngine.InfoWindow.cMessage = cq;
                                                Rogue.RAM.Flags.AllTransform = true;
                                            }
                                            else
                                            { DrawEngine.InfoWindow.Warning = "У вас нет нужного предмета для трансформации!"; }
                                        }
                                        break;
                                    }
                                case ConsoleKey.S:
                                    {
                                        if (have)
                                        {
                                            if (
                                                Rogue.RAM.Player.CheckItem(ResourseBase.LifeRose) > -1
                                                &&
                                                Rogue.RAM.Player.CheckItem(ResourseBase.Diamond)>-1
                                                &&
                                                Rogue.RAM.Player.CheckItem(ResourseBase.Glass) > -1
                                               )
                                            {
                                                Rogue.RAM.Player.Inventory.Remove(Rogue.RAM.Player.Inventory[Rogue.RAM.Player.CheckItem(ResourseBase.LifeRose)]);
                                                SoundEngine.Sound.DropItem();
                                                Rogue.RAM.Player.Inventory.Remove(Rogue.RAM.Player.Inventory[Rogue.RAM.Player.CheckItem(ResourseBase.Glass)]);
                                                SoundEngine.Sound.DropItem();
                                                Rogue.RAM.Player.Inventory.Remove(Rogue.RAM.Player.Inventory[Rogue.RAM.Player.CheckItem(ResourseBase.Diamond)]);
                                                SoundEngine.Sound.DropItem();

                                                DrawEngine.GUIDraw.ReDrawCharInventory();
                                                
                                                foreach (MechEngine.Quest q in Rogue.RAM.Player.QuestBook)
                                                {
                                                    if (q.Name == QuestBase.NPCQuestFEC.TransformAllToMage.Name)
                                                    {
                                                        index = Rogue.RAM.Player.QuestBook.IndexOf(q);
                                                    }
                                                }
                                                Rogue.RAM.Player.QuestBook.Remove(Rogue.RAM.Player.QuestBook[index]);
                                                List<DrawEngine.ColoredWord> cq = new List<DrawEngine.ColoredWord>();
                                                cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = "Вы выполнили задание - " });
                                                cq.Add(new DrawEngine.ColoredWord() { Color = QuestBase.NPCQuestFEC.TransformAllToMage.Color, Word = QuestBase.NPCQuestFEC.TransformAllToMage.Name });
                                                cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = " !" });
                                                DrawEngine.InfoWindow.cMessage = cq;
                                            }
                                            else
                                            { DrawEngine.InfoWindow.Warning = "У вас нет всех предметов для отправки!"; }
                                        }
                                        break;
                                    }
                                case ConsoleKey.Escape:
                                    {
                                        EndDialogue = true;
                                        break;
                                    }
                            }
                        }
                        #endregion
                        PlayEngine.Enemy = true;
                    };
                    return n;
                }
            }
        }

        public static class MobBase
        {
            public static MechEngine.Monster GetMob()
            {
                var Biom = Rogue.RAM.Map.Biom;
                var Level = Rogue.RAM.Map.Level;

                if (!Rogue.RAM.YQuestmain && Rogue.RAM.Map.Level > 50)
                {
                    Random randomValoran = new Random();
                    int intValoran = randomValoran.Next(0, Rogue.RAM.Map.Level + 100);
                    if (intValoran < Rogue.RAM.Player.Level)
                    { Rogue.RAM.YQuestmain = true; Rogue.RAM.YQuestmain = true; return Valoran; }
                }
                MechEngine.Monster rtrn = new MechEngine.Monster();
                switch (Biom)
                {
                    case ConsoleColor.DarkGreen: { return GetElf; }
                    case ConsoleColor.DarkYellow: { return GetHell; }
                    case ConsoleColor.DarkMagenta: { return GetDrow; }
                    case ConsoleColor.DarkGray: { return GetUndead; }
                    case ConsoleColor.Gray: { return GetDwarf; }
                    case ConsoleColor.DarkCyan: { return GetHoarfrost; }
                    default: { return Rat; }
                }
            }

            public static MechEngine.Monster GetBoss
            {
                get
                {
                    switch (r.Next(4))
                    {
                        case 0: { return DragonCaptain; }
                        case 1: { return DarkElfSummoner; }
                        case 2: { return DwarfDarkEmissar; }
                        case 3: { return ValoranShadow; }
                        default: { return null; }
                    }
                }
            }

            private static MechEngine.Monster GetClass(MechEngine.Monster M)
            {
                switch (r.Next(5))
                {
                    case 0:
                        {
                            return new MechEngine.Monster.Tank(M);
                        }
                    case 1:
                        {
                            return new MechEngine.Monster.DamageDealer(M);
                        }
                    case 2:
                        {
                            return new MechEngine.Monster.Debuffer(M);
                        }
                    case 3:
                        {
                            return new MechEngine.Monster.Healer(M);
                        }
                    case 4:
                        {
                            return new MechEngine.Monster.Mage(M);
                        }
                    case 5:
                        {
                            return new MechEngine.Monster.RogueDealer(M);
                        }
                    default:
                        {
                            return new MechEngine.Monster.Tank(M);
                        }
                }
            }

            private static Random r = new Random(56778);

            #region Wooden Elves

            private static MechEngine.Monster GetElf
            {
                get
                {
                    ;
                    Thread.Sleep(10);
                    int gr = r.Next(6);
                    switch (gr)
                    {
                        case 0: { return AzraiWarrior; }
                        case 1: { return AzraiElite; }
                        case 2: { return AzraiTiger; }
                        case 3: { return Wisp; }
                        case 4: { return AirSpirit; }
                        case 5: { return Maiden; }
                        default: { return AzraiWarrior; }
                    }
                }
            }

            public static MechEngine.Monster AzraiWarrior
            {
                get
                {
                    MechEngine.Monster R = new MechEngine.Monster(MechEngine.MonsterRace.Avariel);
                    R.Chest = ConsoleColor.Green;
                    R.Loot = DataBase.ItemBase.GetItemLoot();
                    R.LVL = Rogue.RAM.Map.Level;
                    R._HP = Rogue.RAM.Map.Level * 8;
                    R.AD = Rogue.RAM.Map.Level * 3;
                    R.AP = 0;
                    R.Name = "Азрай";
                    R.Icon = 'E';
                    R.EXP = 3;
                    R.EXPRate = 0.1;
                    R.ARM = Convert.ToInt32(Rogue.RAM.Map.Level / 2);
                    R.MRS = Rogue.RAM.Map.Level;
                    R.Race = MechEngine.MonsterRace.Avariel;
                    R.MIDMG = r.Next(1,Rogue.RAM.Map.Level * 4);
                    R.MADMG = r.Next(R.MIDMG+1, Rogue.RAM.Map.Level * 8);
                    return GetClass(R);
                }
            }

            public static MechEngine.Monster AzraiElite
            {
                get
                {
                    MechEngine.Monster R = new MechEngine.Monster(MechEngine.MonsterRace.Avariel);
                    R.Chest = ConsoleColor.DarkGreen;
                    R.Loot = DataBase.ItemBase.GetItemLoot();
                    R.LVL = Rogue.RAM.Map.Level;
                    R._HP = Rogue.RAM.Map.Level*8;
                    R.AD = 0;
                    R.AP = Rogue.RAM.Map.Level*3;
                    R.Name = "Друид";
                    R.Icon = 'D';
                    R.EXP = 2;
                    R.EXPRate = 0.15;
                    R.ARM = 0;
                    R.MRS = Convert.ToInt32(Rogue.RAM.Map.Level * 0.2);
                    R.Race = MechEngine.MonsterRace.Avariel;
                    R.MIDMG = r.Next(1, Rogue.RAM.Map.Level * 3);
                    R.MADMG = r.Next(R.MIDMG + 1, Rogue.RAM.Map.Level * 6);
                    return GetClass(R);
                }
            }

            public static MechEngine.Monster AzraiTiger
            {
                get
                {
                    MechEngine.Monster R = new MechEngine.Monster(MechEngine.MonsterRace.Animal);
                    R.Chest = ConsoleColor.DarkYellow;
                    R.Loot = DataBase.ItemBase.GetItemLoot();
                    R.LVL = Rogue.RAM.Map.Level;
                    R._HP = Rogue.RAM.Map.Level * 8;
                    R.AD = Rogue.RAM.Map.Level * 3;
                    R.AP = 0;
                    R.Name = "Тигр";
                    R.EXP = 2;
                    R.EXPRate = 0.37;
                    R.Icon = 'T';
                    R.ARM = Convert.ToInt32(Rogue.RAM.Map.Level * 0.5);
                    R.MRS = 0;
                    R.Race = MechEngine.MonsterRace.Animal;
                    R.MIDMG = r.Next(1, Rogue.RAM.Map.Level * 3);
                    R.MADMG = r.Next(R.MIDMG + 1, Rogue.RAM.Map.Level * 6);
                    return GetClass(R);
                }
            }

            public static MechEngine.Monster Maiden
            {
                get
                {
                    MechEngine.Monster R = new MechEngine.Monster(MechEngine.MonsterRace.Avariel);
                    R.Chest = ConsoleColor.Magenta;
                    R.Loot = DataBase.ItemBase.GetItemLoot();
                    R.LVL = Rogue.RAM.Map.Level;
                    R._HP = Rogue.RAM.Map.Level*6;
                    R.AD = 0;
                    R.AP = Rogue.RAM.Map.Level * 3;
                    R.Name = "Ведьма";
                    R.Icon = 'V';
                    R.EXP = 3;
                    R.EXPRate = 0.15;
                    R.ARM = 0;
                    R.MRS = Rogue.RAM.Map.Level;
                    R.Race = MechEngine.MonsterRace.Avariel;
                    R.MIDMG = r.Next(1, Rogue.RAM.Map.Level * 4);
                    R.MADMG = r.Next(R.MIDMG + 1, Rogue.RAM.Map.Level * 8);
                    return GetClass(R);
                }
            }

            public static MechEngine.Monster Wisp
            {
                get
                {
                    MechEngine.Monster R = new MechEngine.Monster(MechEngine.MonsterRace.Avariel);
                    R.Chest = ConsoleColor.Cyan;
                    R.Loot = DataBase.ItemBase.GetItemLoot();
                    R.LVL = Rogue.RAM.Map.Level;
                    R._HP = Rogue.RAM.Map.Level*8;
                    R.AD = Rogue.RAM.Map.Level;
                    R.AP = Rogue.RAM.Map.Level;
                    R.Name = "Светлячок";
                    R.Icon = 'W';
                    R.EXP = 1;
                    R.EXPRate = 0.05;
                    R.ARM = Rogue.RAM.Map.Level;
                    R.MRS = Rogue.RAM.Map.Level;
                    R.Race = MechEngine.MonsterRace.Avariel;
                    R.MIDMG = r.Next(1, Rogue.RAM.Map.Level * 2);
                    R.MADMG = r.Next(R.MIDMG + 1, Rogue.RAM.Map.Level * 4);
                    return GetClass(R);
                }
            }

            public static MechEngine.Monster AirSpirit
            {
                get
                {
                    MechEngine.Monster m = new MechEngine.Monster(MechEngine.MonsterRace.Avariel);
                    m.Chest = ConsoleColor.White;
                    m.Loot = DataBase.ItemBase.GetItemLoot();
                    m.LVL = Rogue.RAM.Map.Level;
                    m.CHP = Rogue.RAM.Map.Level * 6;
                    m.MHP = Rogue.RAM.Map.Level * 2;
                    m.AD = Rogue.RAM.Map.Level;
                    m.AP = Rogue.RAM.Map.Level;
                    m.Icon = 'A';
                    m.Name = "Дух воздуха";
                    m.EXP = 1;
                    m.EXPRate = 0.1;
                    m.ARM = Convert.ToInt32(Rogue.RAM.Map.Level / 2);
                    m.MRS = Rogue.RAM.Map.Level;
                    m.Race = MechEngine.MonsterRace.Avariel;
                    m.MIDMG = r.Next(1, Rogue.RAM.Map.Level * 3);
                    m.MADMG = r.Next(m.MIDMG + 1, Rogue.RAM.Map.Level * 6);
                    return GetClass(m);
                }
            }

            #endregion

            #region Dragons-Demons

            private static MechEngine.Monster GetHell
            {
                get
                {
                    ;
                    int gr = r.Next(6);
                    Thread.Sleep(10);
                    switch (gr)
                    {
                        case 0: { return Demon; }
                        case 1: { return Ifrit; }
                        case 2: { return Dragonid; }
                        case 3: { return BlackDragon; }
                        case 4: { return WhiteDragon; }
                        case 5: { return FireSpirit; }
                        default: { return Demon; }
                    }
                }
            }

            public static MechEngine.Monster Demon
            {
                get
                {
                    MechEngine.Monster R = new MechEngine.Monster(MechEngine.MonsterRace.Human);
                    R.Chest = ConsoleColor.Red;
                    R.Loot = DataBase.ItemBase.GetItemLoot();
                    R.LVL = Rogue.RAM.Map.Level;
                    R._HP = Rogue.RAM.Map.Level * 5;
                    R.AD = Rogue.RAM.Map.Level * 4;
                    R.AP = Rogue.RAM.Map.Level * 4;
                    R.Name = "Демон";
                    R.Icon = 'D';
                    R.EXP = 1;
                    R.EXPRate = 0.25;
                    R.ARM = Rogue.RAM.Map.Level;
                    R.MRS = Rogue.RAM.Map.Level;
                    R.Race = MechEngine.MonsterRace.Human;
                    R.MIDMG = r.Next(1, Rogue.RAM.Map.Level * 4);
                    R.MADMG = r.Next(R.MIDMG + 1, Rogue.RAM.Map.Level * 8);
                    return GetClass(R);
                }
            }

            public static MechEngine.Monster Ifrit
            {
                get
                {
                    MechEngine.Monster R = new MechEngine.Monster(MechEngine.MonsterRace.Avariel);
                    R.Chest = ConsoleColor.Yellow;
                    R.Loot = DataBase.ItemBase.GetItemLoot();
                    R.LVL = Rogue.RAM.Map.Level;
                    R._HP = Rogue.RAM.Map.Level*10;
                    R.AD = Rogue.RAM.Map.Level;
                    R.AP = Rogue.RAM.Map.Level;
                    R.Name = "Ифрит";
                    R.Icon = 'I';
                    R.EXP = 2;
                    R.EXPRate = 0.30;
                    R.ARM = Convert.ToInt32(Rogue.RAM.Map.Level * 1.32);
                    R.MRS = Convert.ToInt32(Rogue.RAM.Map.Level * 1.2);
                    R.Race = MechEngine.MonsterRace.Avariel;
                    R.MIDMG = r.Next(1, Rogue.RAM.Map.Level * 4);
                    R.MADMG = r.Next(R.MIDMG + 1, Rogue.RAM.Map.Level * 8);
                    return GetClass(R);
                }
            }

            public static MechEngine.Monster Dragonid
            {
                get
                {
                    MechEngine.Monster R = new MechEngine.Monster(MechEngine.MonsterRace.Dragon);
                    R.Chest = ConsoleColor.DarkRed;
                    R.Loot = DataBase.ItemBase.GetItemLoot();
                    R.LVL = Rogue.RAM.Map.Level;
                    R._HP = Rogue.RAM.Map.Level*7;
                    R.AD = 0;
                    R.AP = 0;
                    R.Name = "Драконид";
                    R.Icon = 'M';
                    R.EXP = 3;
                    R.EXPRate = 0.31;
                    R.ARM = Rogue.RAM.Map.Level;
                    R.MRS = Rogue.RAM.Map.Level;
                    R.Race = MechEngine.MonsterRace.Dragon;
                    R.MIDMG = r.Next(1, Rogue.RAM.Map.Level * 4);
                    R.MADMG = r.Next(R.MIDMG + 1, Rogue.RAM.Map.Level * 8);
                    return GetClass(R);
                }
            }

            public static MechEngine.Monster BlackDragon
            {
                get
                {
                    MechEngine.Monster R = new MechEngine.Monster(MechEngine.MonsterRace.Dragon);
                    R.Chest = ConsoleColor.DarkGray;
                    R.Loot = DataBase.ItemBase.GetItemLoot();
                    R.LVL = Rogue.RAM.Map.Level;
                    R._HP = Rogue.RAM.Map.Level*10;
                    R.AD = 0;
                    R.AP = 0;
                    R.Name = "Черный дракон";
                    R.Icon = 'B';
                    R.EXP =4;
                    R.EXPRate = 0.51;
                    R.ARM = Convert.ToInt32(Rogue.RAM.Map.Level * 1.2);
                    R.MRS = Convert.ToInt32(Rogue.RAM.Map.Level * 1.2);
                    R.Race = MechEngine.MonsterRace.Dragon;
                    R.MIDMG = r.Next(1, Rogue.RAM.Map.Level * 6);
                    R.MADMG = r.Next(R.MIDMG + 1, Rogue.RAM.Map.Level * 12);
                    return GetClass(R);
                }
            }

            public static MechEngine.Monster WhiteDragon
            {
                get
                {
                    MechEngine.Monster R = new MechEngine.Monster(MechEngine.MonsterRace.Dragon);
                    R.Chest = ConsoleColor.White;
                    R.Loot = DataBase.ItemBase.GetItemLoot();
                    R.LVL = Rogue.RAM.Map.Level;
                    R._HP = Rogue.RAM.Map.Level*7;
                    R.AD = 0;
                    R.AP = Rogue.RAM.Map.Level;
                    R.Name = "Костяной дракон";
                    R.Icon = 'H';
                    R.EXP = 3;
                    R.EXPRate = 0.41;
                    R.ARM = Convert.ToInt32(Rogue.RAM.Map.Level * 1.7);
                    R.MRS = Convert.ToInt32(Rogue.RAM.Map.Level * 0.8);
                    R.Race = MechEngine.MonsterRace.Dragon;
                    R.MIDMG = r.Next(1, Rogue.RAM.Map.Level * 4);
                    R.MADMG = r.Next(R.MIDMG + 1, Rogue.RAM.Map.Level * 8);
                    return GetClass(R);
                }
            }

            public static MechEngine.Monster FireSpirit
            {
                get
                {
                    MechEngine.Monster m = new MechEngine.Monster(MechEngine.MonsterRace.Avariel);
                    m.Chest = ConsoleColor.Red;
                    m.Loot = DataBase.ItemBase.GetItemLoot();
                    m.LVL = Rogue.RAM.Map.Level;
                    m.CHP = Rogue.RAM.Map.Level * 6;
                    m.MHP = Rogue.RAM.Map.Level * 6;
                    m.AD = Rogue.RAM.Map.Level;
                    m.AP = Rogue.RAM.Map.Level;
                    m.Icon = 'F';
                    m.Name = "Дух огня";
                    m.EXP = 1;
                    m.EXPRate = 0.1;
                    m.ARM = Convert.ToInt32(Rogue.RAM.Map.Level / 2);
                    m.MRS = Rogue.RAM.Map.Level;
                    m.Race = MechEngine.MonsterRace.Avariel;
                    m.MIDMG = r.Next(1, Rogue.RAM.Map.Level * 4);
                    m.MADMG = r.Next(m.MIDMG + 1, Rogue.RAM.Map.Level * 8);
                    return GetClass(m);
                }
            }

            #endregion

            #region Drow

            private static MechEngine.Monster GetDrow
            {
                get
                {
                    ;
                    Thread.Sleep(10);
                    int gr = r.Next(6);
                    switch (gr)
                    {
                        case 0: { return DrowWarrior; }
                        case 1: { return Shadow; }
                        case 2: { return DrowArcher; }
                        case 3: { return Scorpio; }
                        case 4: { return DrowTitan; }
                        case 5: { return EarthSpirit; }
                        default: { return DrowWarrior; }
                    }
                }
            }

            public static MechEngine.Monster DrowWarrior
            {
                get
                {
                    MechEngine.Monster R = new MechEngine.Monster(MechEngine.MonsterRace.Drow);
                    R.Chest = ConsoleColor.DarkGray;
                    R.Loot = DataBase.ItemBase.GetItemLoot();
                    R.LVL = Rogue.RAM.Map.Level;
                    R._HP = Rogue.RAM.Map.Level*7;
                    R.AD = Rogue.RAM.Map.Level * 2;
                    R.AP = Rogue.RAM.Map.Level * 2;
                    R.Name = "Дроу";
                    R.Icon = 'D';
                    R.EXP = 1;
                    R.EXPRate = 0.28;
                    R.ARM = Rogue.RAM.Map.Level;
                    R.MRS = Rogue.RAM.Map.Level;
                    R.Race = MechEngine.MonsterRace.Drow;
                    R.MIDMG = r.Next(1, Rogue.RAM.Map.Level * 4);
                    R.MADMG = r.Next(R.MIDMG + 1, Rogue.RAM.Map.Level * 8);
                    return GetClass(R);
                }
            }

            public static MechEngine.Monster Shadow
            {
                get
                {
                    MechEngine.Monster R = new MechEngine.Monster(MechEngine.MonsterRace.Drow);
                    R.Chest = ConsoleColor.Black;
                    R.Loot = DataBase.ItemBase.GetItemLoot();
                    R.LVL = Rogue.RAM.Map.Level;
                    R._HP = Rogue.RAM.Map.Level*9;
                    R.AD = Rogue.RAM.Map.Level * 3;
                    R.AP = 0;
                    R.Name = "Тень";
                    R.Icon = 'S';
                    R.EXP = 3;
                    R.EXPRate = 0.5;
                    R.ARM = 0;
                    R.MRS =0;
                    R.Race = MechEngine.MonsterRace.Drow;
                    R.MIDMG = r.Next(1, Rogue.RAM.Map.Level * 8);
                    R.MADMG = r.Next(R.MIDMG + 1, Rogue.RAM.Map.Level * 16);
                    return GetClass(R);
                }
            }

            public static MechEngine.Monster DrowArcher
            {
                get
                {
                    MechEngine.Monster R = new MechEngine.Monster(MechEngine.MonsterRace.Drow);
                    R.Chest = ConsoleColor.DarkGray;
                    R.Loot = DataBase.ItemBase.GetItemLoot();
                    R.LVL = Rogue.RAM.Map.Level;
                    R._HP = Rogue.RAM.Map.Level*7;
                    R.AD = Rogue.RAM.Map.Level * 5;
                    R.AP = 0;
                    R.Name = "Патриарх дроу";
                    R.Icon = 'P';
                    R.EXP = 3;
                    R.EXPRate = 0.18;
                    R.ARM = 0;
                    R.MRS = Rogue.RAM.Map.Level;
                    R.Race = MechEngine.MonsterRace.Drow;
                    R.MIDMG = r.Next(1, Rogue.RAM.Map.Level * 4);
                    R.MADMG = r.Next(R.MIDMG + 1, Rogue.RAM.Map.Level * 8);
                    return GetClass(R);
                }
            }

            public static MechEngine.Monster Scorpio
            {
                get
                {
                    MechEngine.Monster R = new MechEngine.Monster(MechEngine.MonsterRace.Drow);
                    R.Chest = ConsoleColor.DarkGray;
                    R.Loot = DataBase.ItemBase.GetItemLoot();
                    R.LVL = Rogue.RAM.Map.Level;
                    R._HP = Rogue.RAM.Map.Level * 10;
                    R.AD = Rogue.RAM.Map.Level * 3;
                    R.AP = 0;
                    R.Name = "Скорпдроу";
                    R.Icon = 'C';
                    R.EXP = 3;
                    R.EXPRate = 0.17;
                    R.ARM = Convert.ToInt32(Rogue.RAM.Map.Level * 1.87);
                    R.MRS = 0;
                    R.Race = MechEngine.MonsterRace.Drow;
                    R.MIDMG = r.Next(1, Rogue.RAM.Map.Level * 4);
                    R.MADMG = r.Next(R.MIDMG + 1, Rogue.RAM.Map.Level * 8);
                    return GetClass(R);
                }
            }

            public static MechEngine.Monster DrowTitan
            {
                get
                {
                    MechEngine.Monster R = new MechEngine.Monster(MechEngine.MonsterRace.Drow);
                    R.Chest = ConsoleColor.DarkCyan;
                    R.Loot = DataBase.ItemBase.GetItemLoot();
                    R.LVL = Rogue.RAM.Map.Level;
                    R._HP = Rogue.RAM.Map.Level*10;
                    R.AD = 0;
                    R.AP = 0;
                    R.Name = "Падший дроу";
                    R.Icon = 'M';
                    R.EXP = 3;
                    R.EXPRate = 0.49;
                    R.ARM = Convert.ToInt32(Rogue.RAM.Map.Level * 1.94);
                    R.MRS = 0;
                    R.Race = MechEngine.MonsterRace.Drow;
                    R.MIDMG = r.Next(1, Rogue.RAM.Map.Level * 4);
                    R.MADMG = r.Next(R.MIDMG + 1, Rogue.RAM.Map.Level * 8);
                    return GetClass(R);
                }
            }

            public static MechEngine.Monster EarthSpirit
            {
                get
                {
                    MechEngine.Monster m = new MechEngine.Monster(MechEngine.MonsterRace.Avariel);
                    m.Chest = ConsoleColor.DarkYellow;
                    m.Loot = DataBase.ItemBase.GetItemLoot();
                    m.LVL = Rogue.RAM.Map.Level;
                    m.CHP = Rogue.RAM.Map.Level * 5;
                    m.MHP = Rogue.RAM.Map.Level * 5;
                    m.AD = Rogue.RAM.Map.Level;
                    m.AP = Rogue.RAM.Map.Level;
                    m.Icon = 'E';
                    m.Name = "Дух земли";
                    m.EXP = 1;
                    m.EXPRate = 0.1;
                    m.ARM = Convert.ToInt32(Rogue.RAM.Map.Level / 2);
                    m.MRS = Rogue.RAM.Map.Level;
                    m.Race = MechEngine.MonsterRace.Avariel;
                    m.MIDMG = r.Next(1, Rogue.RAM.Map.Level * 3);
                    m.MADMG = r.Next(m.MIDMG + 1, Rogue.RAM.Map.Level * 6);
                    return GetClass(m);
                }
            }

            #endregion

            #region Prison

            private static MechEngine.Monster GetUndead
            {
                get
                {
                    ;
                    int gr = r.Next(6);
                    Thread.Sleep(10);
                    switch (gr)
                    {
                        case 0: { return Sceleton; }
                        case 1: { return Zombie; }
                        case 2: { return Troll; }
                        case 3: { return Spectral; }
                        case 4: { return Lich; }
                        case 5: { return DeathSpirit; }
                        default: { return Sceleton; }
                    }
                }
            }

            public static MechEngine.Monster Sceleton
            {
                get
                {
                    MechEngine.Monster R = new MechEngine.Monster(MechEngine.MonsterRace.Undead);
                    R.Chest = ConsoleColor.White;
                    R.Loot = DataBase.ItemBase.GetItemLoot();
                    R.LVL = Rogue.RAM.Map.Level;
                    R._HP = Rogue.RAM.Map.Level*6;
                    R.AD = Rogue.RAM.Map.Level;
                    R.AP = Rogue.RAM.Map.Level;
                    R.Name = "Скелет";
                    R.Icon = 'S';
                    R.EXP = 1;
                    R.EXPRate = 0.71;
                    R.ARM = Convert.ToInt32(Rogue.RAM.Map.Level * 0.2);
                    R.MRS = 0;
                    R.Race = MechEngine.MonsterRace.Undead;
                    R.MIDMG = r.Next(1, Rogue.RAM.Map.Level * 4);
                    R.MADMG = r.Next(R.MIDMG + 1, Rogue.RAM.Map.Level * 8);
                    return GetClass(R);
                }
            }

            public static MechEngine.Monster Zombie
            {
                get
                {
                    MechEngine.Monster R = new MechEngine.Monster(MechEngine.MonsterRace.Undead);
                    R.Chest = ConsoleColor.Green;
                    R.Loot = DataBase.ItemBase.GetItemLoot();
                    R.LVL = Rogue.RAM.Map.Level;
                    R._HP = Rogue.RAM.Map.Level * 8;
                    R.AD = 0;
                    R.AP = 0;
                    R.Name = "Зомби";
                    R.Icon = 'Z';
                    R.EXP = 1;
                    R.EXPRate = 0.97;
                    R.ARM = 0;
                    R.MRS = 0;
                    R.Race = MechEngine.MonsterRace.Undead;
                    R.MIDMG = r.Next(1, Rogue.RAM.Map.Level * 4);
                    R.MADMG = r.Next(R.MIDMG + 1, Rogue.RAM.Map.Level * 8);
                    return GetClass(R);
                }
            }

            public static MechEngine.Monster Troll
            { get { return Rat; } }

            public static MechEngine.Monster Spectral
            {
                get
                {
                    MechEngine.Monster R = new MechEngine.Monster(MechEngine.MonsterRace.Undead);
                    R.Chest = ConsoleColor.DarkGray;
                    R.Loot = DataBase.ItemBase.GetItemLoot();
                    R.LVL = Rogue.RAM.Map.Level;
                    R._HP = Rogue.RAM.Map.Level * 12;
                    R.AD = 0;
                    R.AP = Rogue.RAM.Map.Level * 3;
                    R.Name = "Призрак";
                    R.Icon = 'E';
                    R.EXP = 3;
                    R.EXPRate = 0.79;
                    R.ARM = Rogue.RAM.Map.Level;
                    R.MRS = Rogue.RAM.Map.Level;
                    R.Race = MechEngine.MonsterRace.Undead;
                    R.MIDMG = r.Next(1, Rogue.RAM.Map.Level * 3);
                    R.MADMG = r.Next(R.MIDMG + 1, Rogue.RAM.Map.Level * 7);
                    return GetClass(R);
                }
            }

            public static MechEngine.Monster Lich
            {
                get
                {
                    MechEngine.Monster R = new MechEngine.Monster(MechEngine.MonsterRace.Undead);
                    R.Chest = ConsoleColor.Magenta;
                    R.Loot = DataBase.ItemBase.GetItemLoot();
                    R.LVL = Rogue.RAM.Map.Level;
                    R._HP = Rogue.RAM.Map.Level*5;
                    R.AD = 0;
                    R.AP = Rogue.RAM.Map.Level * 3;
                    R.Name = "Лич";
                    R.Icon = 'L';
                    R.EXP = 1;
                    R.EXPRate = 3.1;
                    R.ARM = Convert.ToInt32(Rogue.RAM.Map.Level * 0.3);
                    R.MRS = Convert.ToInt32(Rogue.RAM.Map.Level * 1.3);
                    R.Race = MechEngine.MonsterRace.Undead;
                    R.MIDMG = r.Next(1, Rogue.RAM.Map.Level * 4);
                    R.MADMG = r.Next(R.MIDMG + 1, Rogue.RAM.Map.Level * 8);
                    return GetClass(R);
                }
            }

            public static MechEngine.Monster DeathSpirit
            {
                get
                {
                    MechEngine.Monster m = new MechEngine.Monster(MechEngine.MonsterRace.Avariel);
                    m.Chest = ConsoleColor.DarkMagenta;
                    m.Loot = DataBase.ItemBase.GetItemLoot();
                    m.LVL = Rogue.RAM.Map.Level;
                    m._HP = Rogue.RAM.Map.Level * 9;
                    m.AD = Rogue.RAM.Map.Level;
                    m.AP = Rogue.RAM.Map.Level;
                    m.Icon = 'Y';
                    m.Name = "Дух смерти";
                    m.EXP = 1;
                    m.EXPRate = 0.1;
                    m.ARM = Convert.ToInt32(Rogue.RAM.Map.Level / 2);
                    m.MRS = Rogue.RAM.Map.Level;
                    m.Race = MechEngine.MonsterRace.Avariel;
                    m.MIDMG = r.Next(1, Rogue.RAM.Map.Level * 4);
                    m.MADMG = r.Next(m.MIDMG + 1, Rogue.RAM.Map.Level * 8);
                    return GetClass(m);
                }
            }

            #endregion

            #region Dwarfs

            private static MechEngine.Monster GetDwarf
            {
                get
                {
                    ;
                    int gr = r.Next(6);
                    Thread.Sleep(10);
                    switch (gr)
                    {
                        case 0: { return DwarfFortress; }
                        case 1: { return DwarfMerch; }
                        case 2: { return DwarfPriest; }
                        case 3: { return GnomeRioter; }
                        case 4: { return Dune; }
                        case 5: { return DwarfSpirit; ; }
                        default: { return DwarfFortress; }
                    }
                }
            }

            public static MechEngine.Monster DwarfFortress
            {
                get
                {
                    MechEngine.Monster R = new MechEngine.Monster(MechEngine.MonsterRace.Human);
                    R.Chest = ConsoleColor.Gray;
                    R.Loot = DataBase.ItemBase.GetItemLoot();
                    R.LVL = Rogue.RAM.Map.Level;
                    R._HP = Rogue.RAM.Map.Level*8;
                    R.AD = Rogue.RAM.Map.Level;
                    R.AP = 0;
                    R.Name = "Дварф форта";
                    R.Icon = 'D';
                    R.EXP = 1;
                    R.EXPRate = 0.83;
                    R.ARM = Convert.ToInt32(Rogue.RAM.Map.Level * 0.6);
                    R.MRS = 0;
                    R.Race = MechEngine.MonsterRace.Human;
                    R.MIDMG = r.Next(1, Rogue.RAM.Map.Level * 4);
                    R.MADMG = r.Next(R.MIDMG + 1, Rogue.RAM.Map.Level * 8);
                    return GetClass(R);
                }
            }

            public static MechEngine.Monster DwarfMerch
            {
                get
                {
                    MechEngine.Monster R = new MechEngine.Monster(MechEngine.MonsterRace.Human);
                    R.Chest = ConsoleColor.Gray;
                    R.Loot = DataBase.ItemBase.GetItemLoot();
                    R.LVL = Rogue.RAM.Map.Level;
                    R._HP = Rogue.RAM.Map.Level*8;
                    R.AD = Rogue.RAM.Map.Level;
                    R.AP = 0;
                    R.Name = "Ремесленник дварф";
                    R.Icon = 'R';
                    R.EXP = 2;
                    R.EXPRate = 0.83;
                    R.ARM = Convert.ToInt32(Rogue.RAM.Map.Level*0.2);
                    R.MRS = 0;
                    R.Race = MechEngine.MonsterRace.Human;
                    R.MIDMG = r.Next(1, Rogue.RAM.Map.Level * 4);
                    R.MADMG = r.Next(R.MIDMG + 1, Rogue.RAM.Map.Level * 8);
                    return GetClass(R);
                }
            }

            public static MechEngine.Monster DwarfPriest
            {
                get
                {
                    MechEngine.Monster R = new MechEngine.Monster(MechEngine.MonsterRace.Human);
                    R.Chest = ConsoleColor.Gray;
                    R.Loot = DataBase.ItemBase.GetItemLoot();
                    R.LVL = Rogue.RAM.Map.Level;
                    R._HP = Rogue.RAM.Map.Level*8;
                    R.AD = 0;
                    R.AP = Rogue.RAM.Map.Level*3;
                    R.Name = "Дварф жрец";
                    R.Icon = 'H';
                    R.EXP = 3;
                    R.EXPRate = 0.21;
                    R.ARM = 0;
                    R.MRS = Rogue.RAM.Map.Level;
                    R.Race = MechEngine.MonsterRace.Human;
                    R.MIDMG = r.Next(1, Rogue.RAM.Map.Level * 4);
                    R.MADMG = r.Next(R.MIDMG + 1, Rogue.RAM.Map.Level * 8);
                    return GetClass(R);
                }
            }

            public static MechEngine.Monster GnomeRioter
            {
                get
                {
                    MechEngine.Monster R = new MechEngine.Monster(MechEngine.MonsterRace.Human);
                    R.Chest = ConsoleColor.DarkGray;
                    R.Loot = DataBase.ItemBase.GetItemLoot();
                    R.LVL = Rogue.RAM.Map.Level;
                    R._HP = Rogue.RAM.Map.Level*9;
                    R.AD = Rogue.RAM.Map.Level * 3;
                    R.AP = Rogue.RAM.Map.Level * 3;
                    R.Name = "Мятежный гном";
                    R.Icon = 'G';
                    R.EXP = 4;
                    R.EXPRate = 0.83;
                    R.ARM = Rogue.RAM.Map.Level;
                    R.MRS = Rogue.RAM.Map.Level;
                    R.Race = MechEngine.MonsterRace.Human;
                    R.MIDMG = r.Next(1, Rogue.RAM.Map.Level * 4);
                    R.MADMG = r.Next(R.MIDMG + 1, Rogue.RAM.Map.Level * 8);
                    return GetClass(R);
                }
            }

            public static MechEngine.Monster Dune
            {
                get
                {
                    MechEngine.Monster R = new MechEngine.Monster(MechEngine.MonsterRace.Animal);
                    R.Chest = ConsoleColor.DarkYellow;
                    R.Loot = DataBase.ItemBase.GetItemLoot();
                    R.LVL = Rogue.RAM.Map.Level;
                    R._HP = Rogue.RAM.Map.Level*12;
                    R.AD = Rogue.RAM.Map.Level * 2;
                    R.AP = 0;
                    R.Name = "Шаи-хулуд";
                    R.Icon = 'W';
                    R.EXP = 1;
                    R.EXPRate = 5.71;
                    R.ARM = Rogue.RAM.Map.Level;
                    R.MRS = Rogue.RAM.Map.Level;
                    R.Race = MechEngine.MonsterRace.Animal;
                    R.MIDMG = r.Next(1, Rogue.RAM.Map.Level * 4);
                    R.MADMG = r.Next(R.MIDMG + 1, Rogue.RAM.Map.Level * 8);
                    return GetClass(R);
                }
            }

            public static MechEngine.Monster DwarfSpirit
            {
                get
                {
                    MechEngine.Monster m = new MechEngine.Monster(MechEngine.MonsterRace.Avariel);
                    m.Chest = ConsoleColor.DarkMagenta;
                    m.Loot = DataBase.ItemBase.GetItemLoot();
                    m.LVL = Rogue.RAM.Map.Level;
                    m._HP = Rogue.RAM.Map.Level * 4;
                    m.AD = Rogue.RAM.Map.Level;
                    m.AP = Rogue.RAM.Map.Level;
                    m.Icon = 'V';
                    m.Name = "Дух дварфа";
                    m.EXP = 1;
                    m.EXPRate = 0.1;
                    m.ARM = Convert.ToInt32(Rogue.RAM.Map.Level / 2);
                    m.MRS = Rogue.RAM.Map.Level;
                    m.Race = MechEngine.MonsterRace.Avariel;
                    m.MIDMG = r.Next(1, Rogue.RAM.Map.Level * 4);
                    m.MADMG = r.Next(m.MIDMG + 1, Rogue.RAM.Map.Level * 8);
                    return GetClass(m);
                }
            }

            #endregion

            #region Water

            private static MechEngine.Monster GetHoarfrost
            {
                get
                {
                    ;
                    int gr = r.Next(6);
                    Thread.Sleep(10);
                    switch (gr)
                    {
                        case 0: { return Hrimthus; }
                        case 1: { return HighlandTiger; }
                        case 2: { return HighlandRogue; }
                        case 3: { return WaterPriest; }
                        case 4: { return FrostSpirit; }
                        case 5: { return WaterSpirit; ; }
                        default: { return WaterSpirit; }
                    }
                }
            }

            public static MechEngine.Monster Hrimthus
            {
                get
                {
                    MechEngine.Monster R = new MechEngine.Monster(MechEngine.MonsterRace.Human);
                    R.Chest = ConsoleColor.Cyan;
                    R.Loot = DataBase.ItemBase.GetItemLoot();
                    R.LVL = Rogue.RAM.Map.Level;
                    R._HP = Rogue.RAM.Map.Level*8;
                    R.AD = Rogue.RAM.Map.Level*2;
                    R.AP = 0;
                    R.Name = "Инеистый великан";
                    R.Icon = 'G';
                    R.EXP = 3;
                    R.EXPRate = 0.11;
                    R.ARM = 0;
                    R.MRS = Convert.ToInt32(Rogue.RAM.Map.Level * 0.7);
                    R.Race = MechEngine.MonsterRace.Human;
                    R.MIDMG = r.Next(1, Rogue.RAM.Map.Level * 4);
                    R.MADMG = r.Next(R.MIDMG + 1, Rogue.RAM.Map.Level * 8);
                    return GetClass(R);
                }
            }

            public static MechEngine.Monster HighlandTiger
            {
                get
                {
                    MechEngine.Monster R = new MechEngine.Monster(MechEngine.MonsterRace.Animal);
                    R.Chest = ConsoleColor.Yellow;
                    R.Loot = DataBase.ItemBase.GetItemLoot();
                    R.LVL = Rogue.RAM.Map.Level;
                    R._HP = Rogue.RAM.Map.Level*7;
                    R.AD = Rogue.RAM.Map.Level*3;
                    R.AP = 0;
                    R.Name = "Горный тигр";
                    R.Icon = 'F';
                    R.EXP = 1;
                    R.EXPRate = 0.34;
                    R.ARM = Rogue.RAM.Map.Level;
                    R.MRS = 0;
                    R.Race = MechEngine.MonsterRace.Human;
                    R.MIDMG = r.Next(1, Rogue.RAM.Map.Level * 4);
                    R.MADMG = r.Next(R.MIDMG + 1, Rogue.RAM.Map.Level * 8);
                    return GetClass(R);
                }
            }

            public static MechEngine.Monster HighlandRogue
            {
                get
                {
                    MechEngine.Monster R = new MechEngine.Monster(MechEngine.MonsterRace.Human);
                    R.Chest = ConsoleColor.DarkGray;
                    R.Loot = DataBase.ItemBase.GetItemLoot();
                    R.LVL = Rogue.RAM.Map.Level;
                    R._HP = Rogue.RAM.Map.Level*8;
                    R.AD = Rogue.RAM.Map.Level;
                    R.AP = 0;
                    R.Name = "Разбойник гор";
                    R.Icon = 'R';
                    R.EXP = 2;
                    R.EXPRate = 0.77;
                    R.ARM = 0;
                    R.MRS = Rogue.RAM.Map.Level;
                    R.Race = MechEngine.MonsterRace.Human;
                    R.MIDMG = r.Next(1, Rogue.RAM.Map.Level * 4);
                    R.MADMG = r.Next(R.MIDMG + 1, Rogue.RAM.Map.Level * 8);
                    return GetClass(R);
                }
            }

            public static MechEngine.Monster WaterPriest
            {
                get
                {
                    MechEngine.Monster R = new MechEngine.Monster(MechEngine.MonsterRace.Human);
                    R.Chest = ConsoleColor.Blue;
                    R.Loot = DataBase.ItemBase.GetItemLoot();
                    R.LVL = Rogue.RAM.Map.Level;
                    R._HP = Rogue.RAM.Map.Level*6;
                    R.AD = 0;
                    R.AP = Rogue.RAM.Map.Level * 3;
                    R.Name = "Жрец воды";
                    R.Icon = 'O';
                    R.EXP = 4;
                    R.EXPRate = 1.02;
                    R.ARM = 0;
                    R.MRS = Rogue.RAM.Map.Level;
                    R.Race = MechEngine.MonsterRace.Human;
                    R.MIDMG = r.Next(1, Rogue.RAM.Map.Level * 4);
                    R.MADMG = r.Next(R.MIDMG + 1, Rogue.RAM.Map.Level * 8);
                    return GetClass(R);
                }
            }

            public static MechEngine.Monster FrostSpirit
            {
                get
                {
                    MechEngine.Monster R = new MechEngine.Monster(MechEngine.MonsterRace.Avariel);
                    R.Chest = ConsoleColor.DarkBlue;
                    R.Loot = DataBase.ItemBase.GetItemLoot();
                    R.LVL = Rogue.RAM.Map.Level;
                    R._HP = Rogue.RAM.Map.Level * 9;
                    R.AD = Rogue.RAM.Map.Level;
                    R.AP = Rogue.RAM.Map.Level;
                    R.Name = "Ледяной элементаль";
                    R.Icon = 'F';
                    R.EXP = 2;
                    R.EXPRate = 0.22;
                    R.ARM = Convert.ToInt32(Rogue.RAM.Map.Level / 2);
                    R.MRS = Rogue.RAM.Map.Level;
                    R.Race = MechEngine.MonsterRace.Avariel;
                    R.MIDMG = r.Next(1, Rogue.RAM.Map.Level * 4);
                    R.MADMG = r.Next(R.MIDMG + 1, Rogue.RAM.Map.Level * 8);
                    return GetClass(R);
                }
            }

            public static MechEngine.Monster WaterSpirit
            {
                get
                {
                    MechEngine.Monster m = new MechEngine.Monster(MechEngine.MonsterRace.Avariel);
                    m.Chest = ConsoleColor.Blue;
                    m.Loot = DataBase.ItemBase.GetItemLoot();
                    m.LVL = Rogue.RAM.Map.Level;
                    m._HP = Rogue.RAM.Map.Level * 8;
                    m.AD = Rogue.RAM.Map.Level;
                    m.AP = Rogue.RAM.Map.Level;
                    m.Icon = 'Q';
                    m.Name = "Дух воды";
                    m.EXP = 1;
                    m.EXPRate = 0.1;
                    m.ARM = Convert.ToInt32(Rogue.RAM.Map.Level / 2);
                    m.MRS = Rogue.RAM.Map.Level;
                    m.Race = MechEngine.MonsterRace.Avariel;
                    m.MIDMG = r.Next(1, Rogue.RAM.Map.Level * 4);
                    m.MADMG = r.Next(m.MIDMG + 1, Rogue.RAM.Map.Level * 8);
                    return GetClass(m);
                }
            }

            #endregion

            #region Other

            public static MechEngine.Monster Rat
            {
                get
                {
                    MechEngine.Monster R = new MechEngine.Monster(MechEngine.MonsterRace.Animal);
                    R.Chest = ConsoleColor.Yellow;
                    R.Loot = DataBase.ItemBase.GetItemLoot();
                    R.LVL = Rogue.RAM.Map.Level;
                    R._HP = Rogue.RAM.Map.Level * 7;
                    R.AD = 0;
                    R.AP = 0;
                    R.Name = "Крыса";
                    R.Icon = 'R';
                    R.EXP = 1;
                    R.EXPRate = 0.1;
                    R.ARM = 0;
                    R.MRS = 0;
                    R.Race = MechEngine.MonsterRace.Animal;
                    R.MIDMG = r.Next(Rogue.RAM.Map.Level * 3);
                    R.MADMG = r.Next(Rogue.RAM.Map.Level * 4);
                    return GetClass(R);
                }
            }

            public static MechEngine.Monster Goblin
            {
                get
                {
                    MechEngine.Monster R = new MechEngine.Monster(MechEngine.MonsterRace.Human);
                    R.Chest = ConsoleColor.DarkGreen;
                    R.Loot = DataBase.ItemBase.GetItemLoot();
                    R.LVL = Rogue.RAM.Map.Level;
                    R._HP = 10;
                    R.AD = 0;
                    R.AP = 0;
                    R.Name = "Гоблин";
                    R.Icon = 'G';
                    R.EXP = 0;
                    R.EXPRate = 0;
                    R.ARM = 0;
                    R.MRS = 0;
                    R.Race = MechEngine.MonsterRace.Human;
                    R.MIDMG = 1;
                    R.MADMG = 2;
                    return GetClass(R);
                }
            }

            public static MechEngine.Monster Stephan
            {
                get
                {
                    MechEngine.Monster s = new MechEngine.Monster();
                    s.Chest = ConsoleColor.DarkCyan;
                    s.Loot = ResourseBase.UndeadRing;
                    s.LVL = Rogue.RAM.Player.Level;
                    s._HP = Rogue.RAM.Player.MHP * 2;
                    s.AD = Rogue.RAM.Player.Level;
                    s.AP = Rogue.RAM.Player.Level;
                    s.Name = "Стефан";
                    s.Icon = 'S';
                    s.EXP = 2;
                    s.EXPRate = 1;
                    s.ARM = Convert.ToInt32(Rogue.RAM.Player.Level / 4);
                    s.MRS = Convert.ToInt32(Rogue.RAM.Player.Level / 4);
                    s.Race = MechEngine.MonsterRace.Undead;
                    s.MIDMG = r.Next(Rogue.RAM.Player.Level + 1);
                    s.MADMG = r.Next(s.MIDMG, Rogue.RAM.Player.Level + 1);
                    return s;
                }
            }

            #endregion

            #region AREA 51

            //private static MechEngine.Monster GetArea
            //{
            //    get
            //    {
            //        int gr = r.Next(4);
            //        switch (gr)
            //        {
            //            case 0: { return Normal; }
            //            case 1: { return Usual; }
            //            case 2: { return Special; }
            //            case 3: { return Hard; }
            //            default: { return AzraiWarrior; }
            //        }
            //    }
            //}

            //public static MechEngine.Monster Normal
            //{
            //    get
            //    {
            //        MechEngine.Monster R = GenEngine.MobGen.Gen("Азрай", 'E', ConsoleColor.Green, MechEngine.MonsterClass.DPS, ConsoleColor.DarkGreen);                    
            //        R.Loot = DataBase.ItemBase.GetItemLoot();
            //        return R;
            //    }
            //}

            //public static MechEngine.Monster Usual
            //{
            //    get
            //    {
            //        MechEngine.Monster R = GenEngine.MobGen.Gen("Друид", 'D', ConsoleColor.DarkGreen, MechEngine.MonsterClass.Heal, ConsoleColor.DarkGreen);
            //        R.Loot = DataBase.ItemBase.GetItemLoot();
            //        return R;
            //    }
            //}

            //public static MechEngine.Monster Special
            //{
            //    get
            //    {
            //        MechEngine.Monster R = GenEngine.MobGen.Gen("Ведьма", 'V', ConsoleColor.Magenta, MechEngine.MonsterClass.Mage, ConsoleColor.Magenta);
            //        R.Loot = DataBase.ItemBase.GetItemLoot();
            //        return R;
            //    }
            //}

            //public static MechEngine.Monster Hard
            //{
            //    get
            //    {
            //        MechEngine.Monster R = GenEngine.MobGen.Gen("Дух воздуха", 'A', ConsoleColor.White, MechEngine.MonsterClass.Tank, ConsoleColor.Magenta);
            //        R.Loot = DataBase.ItemBase.GetItemLoot();
            //        return R;
            //    }
            //}
            #endregion

            #region MainQuest

            public static MechEngine.Monster Valoran
            {
                get
                {
                    MechEngine.Monster V = new MechEngine.Monster(MechEngine.MonsterRace.Drow);
                    V.Chest = ConsoleColor.DarkMagenta;
                    //V.Loot = DataBase.ItemBase.WorldCrystall;
                    V.LVL = 100;
                    V._HP = 1000;
                    V.AD = 1000;
                    V.AP = 1000;
                    V.Name = "Валоран";
                    V.Affix = " - Хранитель мира [Предатель]";
                    V.Icon = '♀';
                    V.EXP = 10;
                    V.EXPRate = 10.10;
                    V.ARM = 1000;
                    V.MRS = 1000;
                    V.Race = MechEngine.MonsterRace.Drow;                    
                    V.MIDMG = r.Next(1000);
                    V.MADMG = 1000;
                    return GetClass(V);
                }
            }

            #endregion

            #region Bosses

            public static MechEngine.Monster DragonCaptain
            {
                get
                {
                    MechEngine.Monster R = new MechEngine.Monster(MechEngine.MonsterRace.Dragon);
                    R.Chest = ConsoleColor.Red;
                    //R.Loot = DataBase.ItemBase.GetItemLoot();
                    R.LVL = Rogue.RAM.Map.Level;
                    R._HP = (Rogue.RAM.Map.Level * 10) * 2;
                    R.AD = Rogue.RAM.Map.Level;
                    R.AP = Rogue.RAM.Map.Level;
                    R.Name = "Капитан драконов";
                    R.Icon = 'C';
                    R.EXP = 6;
                    R.EXPRate = 0.51;
                    R.ARM = Convert.ToInt32(Rogue.RAM.Map.Level * 1.2);
                    R.MRS = Convert.ToInt32(Rogue.RAM.Map.Level * 1.2);
                    R.Race = MechEngine.MonsterRace.Dragon;
                    R.MIDMG = r.Next(1, Rogue.RAM.Map.Level * 6);
                    R.MADMG = r.Next(R.MIDMG + 1, Rogue.RAM.Map.Level * 12);
                    R.Boss = true;

                    MechEngine.MonsterAbility a = new MechEngine.MonsterAbility();
                    a.Action = new MechEngine.AbilityActionOne() { Action = MechEngine.AbilityActionType.Damage, Attribute = MechEngine.AbilityActionAttribute.DmgHealInstant };
                    a.Name = "Гонение огненное";
                    a.Icon = 'F';
                    a.Type = MechEngine.AbilityRate.AttackDamage;
                    a.Power = Convert.ToInt32(R.AD * 0.15) + Convert.ToInt32(R.AP * 0.15);

                    R.BossAblity = a;

                    return GetClass(R);
                }
            }

            public static MechEngine.Monster DarkElfSummoner
            {
                get
                {
                    MechEngine.Monster R = new MechEngine.Monster(MechEngine.MonsterRace.Drow);
                    R.Chest = ConsoleColor.DarkMagenta;
                    //R.Loot = DataBase.ItemBase.GetItemLoot();
                    R.LVL = Rogue.RAM.Map.Level;
                    R._HP = (Rogue.RAM.Map.Level * 10) * 2;
                    R.AD = 0;
                    R.AP = Rogue.RAM.Map.Level;
                    R.Name = "Тёмный призыватель";
                    R.Icon = 'S';
                    R.EXP = 6;
                    R.EXPRate = 0.51;
                    R.ARM = Convert.ToInt32(Rogue.RAM.Map.Level * 1.2);
                    R.MRS = Convert.ToInt32(Rogue.RAM.Map.Level * 1.2);
                    R.Race = MechEngine.MonsterRace.Dragon;
                    R.MIDMG = r.Next(1, Rogue.RAM.Map.Level * 6);
                    R.MADMG = r.Next(R.MIDMG + 1, Rogue.RAM.Map.Level * 12);
                    R.Boss = true;

                    MechEngine.MonsterAbility a = new MechEngine.MonsterAbility();
                    a.Action = new MechEngine.AbilityActionOne() { Action = MechEngine.AbilityActionType.Debuff, Attribute = MechEngine.AbilityActionAttribute.DmgHealInstant };
                    a.Name = "Темная пелена";
                    a.Stats = new List<MechEngine.AbilityStats>() { MechEngine.AbilityStats.AP, MechEngine.AbilityStats.MRS, MechEngine.AbilityStats.DMG };
                    a.Icon = 'D';                    
                    a.Type = MechEngine.AbilityRate.AttackDamage;
                    a.Power = Convert.ToInt32(R.AP * 0.15);

                    R.BossAblity = a;

                    return GetClass(R);
                }
            }

            public static MechEngine.Monster DwarfDarkEmissar
            {
                get
                {
                    MechEngine.Monster R = new MechEngine.Monster(MechEngine.MonsterRace.Dragon);
                    R.Chest = ConsoleColor.Green;
                    //R.Loot = DataBase.ItemBase.GetItemLoot();
                    R.LVL = Rogue.RAM.Map.Level;
                    R._HP = (Rogue.RAM.Map.Level * 10) * 2;
                    R.AD = Rogue.RAM.Map.Level;
                    R.AP = 0;
                    R.Name = "Эмиссар-предатель";
                    R.Icon = '↨';
                    R.EXP = 6;
                    R.EXPRate = 0.51;
                    R.ARM = Convert.ToInt32(Rogue.RAM.Map.Level * 1.2);
                    R.MRS = Convert.ToInt32(Rogue.RAM.Map.Level * 1.2);
                    R.Race = MechEngine.MonsterRace.Dragon;
                    R.MIDMG = r.Next(1, Rogue.RAM.Map.Level * 6);
                    R.MADMG = r.Next(R.MIDMG + 1, Rogue.RAM.Map.Level * 12);
                    R.Boss = true;

                    MechEngine.MonsterAbility a = new MechEngine.MonsterAbility();
                    a.Action = new MechEngine.AbilityActionOne() { Action = MechEngine.AbilityActionType.Damage, Attribute = MechEngine.AbilityActionAttribute.DmgHealInstant };
                    a.Name = "Хлыст";
                    a.Icon = 'Z';
                    a.Type = MechEngine.AbilityRate.AttackDamage;
                    a.Power = Convert.ToInt32(R.AD * 0.15);

                    R.BossAblity = a;

                    return GetClass(R);
                }
            }

            public static MechEngine.Monster ValoranShadow
            {
                get
                {
                    MechEngine.Monster R = new MechEngine.Monster(MechEngine.MonsterRace.Dragon);
                    R.Chest = ConsoleColor.Magenta;
                    //R.Loot = DataBase.ItemBase.GetItemLoot();
                    R.LVL = Rogue.RAM.Map.Level;
                    R._HP = (Rogue.RAM.Map.Level * 10) * 2;
                    R.AD = 0;
                    R.AP = Rogue.RAM.Map.Level*5;
                    R.Name = "Тень Валорана";
                    R.Icon = '♀';
                    R.EXP = 6;
                    R.EXPRate = 0.51;
                    R.ARM = Convert.ToInt32(Rogue.RAM.Map.Level * 1.2);
                    R.MRS = Convert.ToInt32(Rogue.RAM.Map.Level * 1.2);
                    R.Race = MechEngine.MonsterRace.Dragon;
                    R.MIDMG = r.Next(1, Rogue.RAM.Map.Level * 6);
                    R.MADMG = r.Next(R.MIDMG + 1, Rogue.RAM.Map.Level * 12);
                    R.Boss = true;

                    MechEngine.MonsterAbility a = new MechEngine.MonsterAbility();
                    a.Action = new MechEngine.AbilityActionOne() { Action = MechEngine.AbilityActionType.Damage, Attribute = MechEngine.AbilityActionAttribute.DmgHealInstant };
                    a.Name = "Сила";
                    a.Icon = 'P';
                    a.Type = MechEngine.AbilityRate.AbilityPower;
                    a.Power = (Convert.ToInt32(R.AD * 0.15)) * 2;

                    R.BossAblity = a;

                    return GetClass(R);
                }
            }

            #endregion

        }

        public static class NpcBase
        {
            public static MechEngine.NPC Qurel
            {
                get
                {
                    MechEngine.NPC n = new MechEngine.NPC();
                    n.Icon = 'Q';
                    n.Color = ConsoleColor.Yellow;
                    n.Name = "Курел";
                    n.Info = "Простолюдин-крестьянин. Как они появляются в Мраумире?";
                    n.Affix = "Крестьянин";
                    n.Script += delegate()
                    {
                        if (PlayEngine.EnemyMoves.CheckMove) { PlayEngine.EnemyMoves.Move(false); }
                        string info = n.Info;
                        n.Info = "Приветствую тебя, "+Rogue.RAM.Player.Name+"! Я обычный крестьянин, и не могу тебе помочь в сражениях, поэтому ступай своей дорогой...";
                        DrawEngine.InfoDraw.NPC = n;
                        Console.ReadKey(true);
                        n.Info = info;
                        if (!PlayEngine.EnemyMoves.CheckMove) { PlayEngine.EnemyMoves.Move(true); }
                    };
                    return n;
                }
            }

            public static MechEngine.NPC Lir
            {
                get
                {
                    MechEngine.NPC n = new MechEngine.NPC();
                    n.Icon = 'L';
                    n.Color = ConsoleColor.White;
                    n.Name = "Лир";
                    n.Info = "Один из самых старых жителей города, в молодости был ужасным кузнецом.";
                    n.Affix = "Портной";
                    #region Script
                    n.Script += delegate()
                    {
                        PlayEngine.Enemy = false;
                        DrawEngine.Replica R = new DrawEngine.Replica();
                        R.Text = "Привет! Я портной, если хочешь купить вещи изготовленные мной, то обратись к Элле. Если у тебя есть <Иголка>, <Нитки>, и <Набор портного> то я могу научить тебя шить одежду.";
                        R.TextColor = ConsoleColor.DarkYellow;
                        R.Options.Add("[T] - Научи меня.");
                        R.Options.Add("[Escape] - Уйти.");
                        R.OptionsColor = ConsoleColor.Red;
                        DrawEngine.ActiveObjectDraw.Draw(R, n);

                        bool EndDialogue = false;
                        while (!EndDialogue)
                        {
                            ConsoleKey push = Console.ReadKey(true).Key;
                            switch (push)
                            {                                    
                                case ConsoleKey.T:
                                    {
                                        bool flag = false;                                        
                                        foreach (MechEngine.Ability a in Rogue.RAM.Player.CraftAbility)
                                        {
                                            if (a.CraftItem != null)
                                            {
                                                if (a.CraftItem.Name == DataBase.OtherAbilityBase.TailorAmor.CraftItem.Name) { flag = true; }
                                            }
                                        }
                                        if (!flag)
                                        {
                                            int ingr = 0;
                                            List<MechEngine.Item> arr = new List<MechEngine.Item>();
                                            for (int i = 0; i < Rogue.RAM.Player.Inventory.Count; i++)
                                            {
                                                    if (Rogue.RAM.Player.Inventory[i].Name == "Иголка" || Rogue.RAM.Player.Inventory[i].Name == "Нитки" || Rogue.RAM.Player.Inventory[i].Name == "Набор портного")
                                                    {                                                        
                                                        ingr++;
                                                        arr.Add(Rogue.RAM.Player.Inventory[i]);
                                                    }
                                            }

                                            if (ingr == 3)
                                            {
                                                foreach (MechEngine.Item i in arr)
                                                {
                                                    Rogue.RAM.Player.Inventory.Remove(i);                                                    
                                                }
                                                arr = null;
                                                DrawEngine.GUIDraw.ReDrawCharInventory();

                                                int currentint = 0;
                                                DrawEngine.PopUpWindowDraw.SwitchCraftAbilWindow(0, new List<MechEngine.Ability>() { Rogue.RAM.Player.CraftAbility[0], Rogue.RAM.Player.CraftAbility[1], Rogue.RAM.Player.CraftAbility[3] });
                                                bool endd = false;
                                                while (!endd)
                                                {
                                                    ConsoleKey pushh = Console.ReadKey(true).Key;
                                                    switch (pushh)
                                                    {
                                                        case ConsoleKey.RightArrow: { if (currentint != 2) { currentint++; DrawEngine.PopUpWindowDraw.SwitchCraftAbilWindow(currentint, new List<MechEngine.Ability>() { Rogue.RAM.Player.CraftAbility[0], Rogue.RAM.Player.CraftAbility[1], Rogue.RAM.Player.CraftAbility[3] }); } break; }
                                                        case ConsoleKey.LeftArrow: { if (currentint != 0) { currentint--; DrawEngine.PopUpWindowDraw.SwitchCraftAbilWindow(currentint, new List<MechEngine.Ability>() { Rogue.RAM.Player.CraftAbility[0], Rogue.RAM.Player.CraftAbility[1], Rogue.RAM.Player.CraftAbility[3] }); } break; }
                                                        case ConsoleKey.Enter:
                                                            {
                                                                if (currentint == 2) { currentint = 3; }
                                                                string oldabil = Rogue.RAM.Player.CraftAbility[currentint].Name;
                                                                ConsoleColor oldcolorabil = Rogue.RAM.Player.CraftAbility[currentint].Color;
                                                                Rogue.RAM.Player.CraftAbility[currentint] = DataBase.OtherAbilityBase.TailorAmor;

                                                                DrawEngine.InfoWindow.cMessage = new List<DrawEngine.ColoredWord>()
                                                            {
                                                                new DrawEngine.ColoredWord(){Color=Rogue.RAM.Map.Biom, Word="Вы заменяете "},
                                                                new DrawEngine.ColoredWord() { Color=oldcolorabil, Word=oldabil},
                                                                new DrawEngine.ColoredWord(){Color=Rogue.RAM.Map.Biom, Word=" на "},
                                                                new DrawEngine.ColoredWord() { Color=Rogue.RAM.Player.CraftAbility[currentint].Color, Word=Rogue.RAM.Player.CraftAbility[currentint].Name},
                                                                new DrawEngine.ColoredWord(){Color=Rogue.RAM.Map.Biom, Word=" !"}
                                                            };
                                                                DrawEngine.GUIDraw.DrawLab();
                                                                R.Text = "На данный момент я обучил тебя всему что мог! Возможно позже я смогу научить тебя чему ни будь другому..."; DrawEngine.ActiveObjectDraw.Draw(R, n);
                                                                endd = true;
                                                                break;
                                                            }
                                                        case ConsoleKey.Escape: { DrawEngine.GUIDraw.DrawLab(); DrawEngine.ActiveObjectDraw.Draw(R, n); endd = true; break; }
                                                        default: { break; }
                                                    }
                                                }
                                            }
                                            else
                                            { R.Text = "У тебя нет всего набора инструментов для портняжного дела! Я смогу научить тебя только со <всем> набором!"; DrawEngine.ActiveObjectDraw.Draw(R, n); }
                                        }
                                        else
                                        { R.Text = "Пока что я не могу научить тебя ничему новому, но если ты забудешь как шить одежду я смогу научить тебя снова..."; DrawEngine.ActiveObjectDraw.Draw(R, n); }
                                        break;
                                    }
                                case ConsoleKey.Escape:
                                    {
                                        EndDialogue = true;
                                        break;
                                    }
                            }
                        }

                        PlayEngine.Enemy = true;
                    };
                    #endregion
                    return n;
                }
            }

            public static MechEngine.NPC Cat
            {
                get
                {
                    if (!(Rogue.RAM.Player.Buffs.IndexOf(DataBase.OtherAbilityBase.CatJoin) > -1))
                    {
                        MechEngine.NPC n = new MechEngine.NPC();
                        n.Icon = 'C';
                        n.Color = ConsoleColor.DarkYellow;
                        n.Name = "Кот";
                        n.Info = "С виду это обычный кот, но маленький шлем на его голове может сказать о противоположном...";
                        n.Affix = "Кошка";
                        #region Script
                        n.Script += delegate()
                        {                           
                            DrawEngine.ActiveObjectDraw.Draw(new List<string>() { "[F] - Покормить", "[T] - Взять с собой", "[Escape] - Отмена" }, n, ConsoleColor.DarkYellow);
                            bool EndDialogue = false;
                            while (!EndDialogue)
                            {
                                ConsoleKey push = Console.ReadKey(true).Key;
                                switch (push)
                                {
                                    case ConsoleKey.F:
                                        {
                                            for (int i = 0; i < Rogue.RAM.Player.Inventory.Count; i++)
                                            {
                                                if (Rogue.RAM.Player.Inventory[i].Name == "Корм для кошек")
                                                {
                                                    Rogue.RAM.Player.Inventory.Remove(Rogue.RAM.Player.Inventory[i]);
                                                    MechEngine.Ability cs = OtherAbilityBase.CatJoin;
                                                    cs.Activate();
                                                    SystemEngine.Helper.Information.Point p = SystemEngine.Helper.Information.FindObject(n);
                                                    Rogue.RAM.Map.Map[p.x][p.y].Object = null;
                                                    DrawEngine.winAPIDraw.Helpers.ReDrawOneChar((Int16)p.x, (Int16)p.y, (Int16)p.x, (Int16)p.y);
                                                    DrawEngine.InfoWindow.Warning = "Кот благодарит вас и присоединяется к вам!";
                                                    DrawEngine.FightDraw.ReDrawBuffDeBuff();
                                                    EndDialogue = true;
                                                    break;                                                    
                                                }
                                                else
                                                { DrawEngine.InfoWindow.Warning = "Без еды кот даже не посмотрит в вашу сторону!"; }
                                            }
                                            break;
                                        }
                                    case ConsoleKey.T:
                                        {
                                            DrawEngine.InfoWindow.Warning = "Кот отказывается идти с вами!";
                                            break;
                                        }
                                    case ConsoleKey.Escape:
                                        {
                                            EndDialogue = true;
                                            break;
                                        }
                                }
                            }                            
                        };
                        #endregion
                        return n;
                    }
                    else { return null; }
                }
            }

            public static MechEngine.NPC Polus
            {
                get
                {
                    MechEngine.NPC n = new MechEngine.NPC();
                    n.Icon = 'P';
                    n.Color = ConsoleColor.DarkYellow;
                    n.Name = "Полус";
                    n.Info = "Полус всю жизнь пытается стать богаче, но пока он не начнет брать золото за работу у него это не получится...";
                    n.Affix = "Плотник";
                    #region Script
                    n.Script = () =>
                    {
                        PlayEngine.Enemy = false;
                        DrawEngine.Replica R = new DrawEngine.Replica();
                        R.Text = "Приветствую, герой. Я могу предложить тебе изготовление магических посохов, выбирай любой:";
                        R.TextColor = ConsoleColor.Gray;
                        R.Options.Add("[A] - Дряхлый посох.");
                        R.Options.Add("[B] - Обычный посох.");
                        R.Options.Add("[C] - Посох из дуба.");
                        R.Options.Add("[D] - Красный посох.");
                        R.Options.Add("[S] - Черный посох.");
                        R.Options.Add("[Escape] - Уйти.");
                        R.OptionsColor = ConsoleColor.DarkGray;
                        DrawEngine.ActiveObjectDraw.Draw(R, n);

                        bool EndDialogue = false;
                        while (!EndDialogue)
                        {
                            ConsoleKey push = Console.ReadKey(true).Key;
                            switch (push)
                            {
                                case ConsoleKey.A:
                                    {
                                        bool itgr = false;
                                        MechEngine.Item it = new MechEngine.Item();
                                        for (int i = 0; i < Rogue.RAM.Player.Inventory.Count; i++)
                                        {
                                            if (Rogue.RAM.Player.Inventory[i].Name == "Дерево")
                                            {
                                                itgr = true;
                                                it = Rogue.RAM.Player.Inventory[i];
                                            }
                                        }

                                        if (itgr)
                                        {
                                            Rogue.RAM.Player.Inventory.Remove(it);
                                            it = null;
                                            DrawEngine.GUIDraw.ReDrawCharInventory();

                                            for (int i = 0; i < 5; i++)
                                            {
                                                DrawEngine.InfoWindow.Message = string.Format("Полус изготавливает вам оружие... Осталось {0} сек..", 5-i);
                                                Thread.Sleep(1000);
                                            }
                                            it = ItemBase.CraftItemsFromNPC.LostStaff;
                                            Rogue.RAM.Player.Inventory.Add(it);
                                            DrawEngine.InfoWindow.cMessage = new List<DrawEngine.ColoredWord>()
                                                            {
                                                                new DrawEngine.ColoredWord() { Color=n.Color, Word=n.Name},
                                                                new DrawEngine.ColoredWord(){Color=Rogue.RAM.Map.Biom, Word=" изготовил для вас "},
                                                                new DrawEngine.ColoredWord() { Color=it.Color, Word=it.Name},
                                                                new DrawEngine.ColoredWord(){Color=Rogue.RAM.Map.Biom, Word=" !"}
                                                            };
                                            DrawEngine.GUIDraw.DrawLab();
                                            R.Text = "Твоё оружие готово, если хочешь ещё что-то, просто выбери!"; DrawEngine.ActiveObjectDraw.Draw(R, n);
                                        }
                                        else
                                        { R.Text = "У тебя нет нужного мне материала - <Дерево>!"; DrawEngine.ActiveObjectDraw.Draw(R, n); }

                                        break;
                                    }
                                case ConsoleKey.B:
                                    {
                                        bool itgr = false;
                                        MechEngine.Item it = new MechEngine.Item();
                                        for (int i = 0; i < Rogue.RAM.Player.Inventory.Count; i++)
                                        {
                                            if (Rogue.RAM.Player.Inventory[i].Name == "Дерево")
                                            {
                                                itgr = true;
                                                it = Rogue.RAM.Player.Inventory[i];
                                            }
                                        }

                                        if (itgr)
                                        {
                                            Rogue.RAM.Player.Inventory.Remove(it);
                                            it = null;
                                            DrawEngine.GUIDraw.ReDrawCharInventory();

                                            for (int i = 0; i < 5; i++)
                                            {
                                                DrawEngine.InfoWindow.Message = string.Format("Полус изготавливает вам оружие... Осталось {0} сек..", 5-i);
                                                Thread.Sleep(1000);
                                            }
                                            it = ItemBase.CraftItemsFromNPC.JustStaff;
                                            Rogue.RAM.Player.Inventory.Add(it);
                                            DrawEngine.InfoWindow.cMessage = new List<DrawEngine.ColoredWord>()
                                                            {
                                                                new DrawEngine.ColoredWord() { Color=n.Color, Word=n.Name},
                                                                new DrawEngine.ColoredWord(){Color=Rogue.RAM.Map.Biom, Word=" изготовил для вас "},
                                                                new DrawEngine.ColoredWord() { Color=it.Color, Word=it.Name},
                                                                new DrawEngine.ColoredWord(){Color=Rogue.RAM.Map.Biom, Word=" !"}
                                                            };
                                            DrawEngine.GUIDraw.DrawLab();
                                            R.Text = "Твоё оружие готово, если хочешь ещё что-то, просто выбери!"; DrawEngine.ActiveObjectDraw.Draw(R, n);
                                        }
                                        else
                                        { R.Text = "У тебя нет нужного мне материала - <Дерево>!"; DrawEngine.ActiveObjectDraw.Draw(R, n); }

                                        break;
                                    }
                                case ConsoleKey.C:
                                    {
                                        bool itgr = false;
                                        MechEngine.Item it = new MechEngine.Item();
                                        for (int i = 0; i < Rogue.RAM.Player.Inventory.Count; i++)
                                        {
                                            if (Rogue.RAM.Player.Inventory[i].Name == "Дерево(Дуб)")
                                            {
                                                itgr = true;
                                                it = Rogue.RAM.Player.Inventory[i];
                                            }
                                        }

                                        if (itgr)
                                        {
                                            Rogue.RAM.Player.Inventory.Remove(it);
                                            it = null;
                                            DrawEngine.GUIDraw.ReDrawCharInventory();

                                            for (int i = 0; i < 5; i++)
                                            {
                                                DrawEngine.InfoWindow.Message = string.Format("Полус изготавливает вам оружие... Осталось {0} сек..", 5-i);
                                                Thread.Sleep(1000);
                                            }
                                            it = ItemBase.CraftItemsFromNPC.JustStaffD;
                                            Rogue.RAM.Player.Inventory.Add(it);
                                            DrawEngine.InfoWindow.cMessage = new List<DrawEngine.ColoredWord>()
                                                            {
                                                                new DrawEngine.ColoredWord() { Color=n.Color, Word=n.Name},
                                                                new DrawEngine.ColoredWord(){Color=Rogue.RAM.Map.Biom, Word=" изготовил для вас "},
                                                                new DrawEngine.ColoredWord() { Color=it.Color, Word=it.Name},
                                                                new DrawEngine.ColoredWord(){Color=Rogue.RAM.Map.Biom, Word=" !"}
                                                            };
                                            DrawEngine.GUIDraw.DrawLab();
                                            R.Text = "Твоё оружие готово, если хочешь ещё что-то, просто выбери!"; DrawEngine.ActiveObjectDraw.Draw(R, n);
                                        }
                                        else
                                        { R.Text = "У тебя нет нужного мне материала - <Дерево(Дуб)>!"; DrawEngine.ActiveObjectDraw.Draw(R, n); }

                                        break;
                                    }
                                case ConsoleKey.D:
                                    {
                                        bool itgr = false;
                                        MechEngine.Item it = new MechEngine.Item();
                                        for (int i = 0; i < Rogue.RAM.Player.Inventory.Count; i++)
                                        {
                                            if (Rogue.RAM.Player.Inventory[i].Name == "Красное дерево")
                                            {
                                                itgr = true;
                                                it = Rogue.RAM.Player.Inventory[i];
                                            }
                                        }

                                        if (itgr)
                                        {
                                            Rogue.RAM.Player.Inventory.Remove(it);
                                            it = null;
                                            DrawEngine.GUIDraw.ReDrawCharInventory();

                                            for (int i = 0; i < 5; i++)
                                            {
                                                DrawEngine.InfoWindow.Message = string.Format("Полус изготавливает вам оружие... Осталось {0} сек..", 5-i);
                                                Thread.Sleep(1000);
                                            }
                                            it = ItemBase.CraftItemsFromNPC.RedWoodStaff;
                                            Rogue.RAM.Player.Inventory.Add(it);
                                            DrawEngine.InfoWindow.cMessage = new List<DrawEngine.ColoredWord>()
                                                            {
                                                                new DrawEngine.ColoredWord() { Color=n.Color, Word=n.Name},
                                                                new DrawEngine.ColoredWord(){Color=Rogue.RAM.Map.Biom, Word=" изготовил для вас "},
                                                                new DrawEngine.ColoredWord() { Color=it.Color, Word=it.Name},
                                                                new DrawEngine.ColoredWord(){Color=Rogue.RAM.Map.Biom, Word=" !"}
                                                            };
                                            DrawEngine.GUIDraw.DrawLab();
                                            R.Text = "Твоё оружие готово, если хочешь ещё что-то, просто выбери!"; DrawEngine.ActiveObjectDraw.Draw(R, n);
                                        }
                                        else
                                        { R.Text = "У тебя нет нужного мне материала - <Красное дерево>!"; DrawEngine.ActiveObjectDraw.Draw(R, n); }

                                        break;
                                    }
                                case ConsoleKey.S:
                                    {
                                        bool itgr = false;
                                        MechEngine.Item it = new MechEngine.Item();
                                        for (int i = 0; i < Rogue.RAM.Player.Inventory.Count; i++)
                                        {
                                            if (Rogue.RAM.Player.Inventory[i].Name == "Мертвое дерево")
                                            {
                                                itgr = true;
                                                it = Rogue.RAM.Player.Inventory[i];
                                            }
                                        }

                                        if (itgr)
                                        {
                                            Rogue.RAM.Player.Inventory.Remove(it);
                                            it = null;
                                            DrawEngine.GUIDraw.ReDrawCharInventory();

                                            for (int i = 0; i < 5; i++)
                                            {
                                                DrawEngine.InfoWindow.Message = string.Format("Полус изготавливает вам оружие... Осталось {0} сек..", 5-i);
                                                Thread.Sleep(1000);
                                            }
                                            it = ItemBase.CraftItemsFromNPC.DeadStaff;
                                            Rogue.RAM.Player.Inventory.Add(it);
                                            DrawEngine.InfoWindow.cMessage = new List<DrawEngine.ColoredWord>()
                                                            {
                                                                new DrawEngine.ColoredWord() { Color=n.Color, Word=n.Name},
                                                                new DrawEngine.ColoredWord(){Color=Rogue.RAM.Map.Biom, Word=" изготовил для вас "},
                                                                new DrawEngine.ColoredWord() { Color=it.Color, Word=it.Name},
                                                                new DrawEngine.ColoredWord(){Color=Rogue.RAM.Map.Biom, Word=" !"}
                                                            };
                                            DrawEngine.GUIDraw.DrawLab();
                                            R.Text = "Твоё оружие готово, если хочешь ещё что-то, просто выбери!"; DrawEngine.ActiveObjectDraw.Draw(R, n);
                                        }
                                        else
                                        { R.Text = "У тебя нет нужного мне материала - <Мертвое дерево>!"; DrawEngine.ActiveObjectDraw.Draw(R, n); }

                                        break;
                                    }
                                case ConsoleKey.Escape:
                                    {
                                        EndDialogue = true;
                                        break;
                                    }
                            }
                        }

                        PlayEngine.Enemy = true;
                    };
                    #endregion
                    return n;
                }
            }

            public static MechEngine.Merchant Ella
            {
                get
                {
                    MechEngine.Merchant m = new MechEngine.Merchant();
                    m.Icon = 'E';
                    m.MaxGold = 1000;
                    m.Color = ConsoleColor.Red;
                    m.Info = "Элла продаёт одежду которую шьёт Лир. К тому же, она необыкновенно красива...";
                    m.SpeachColor = ConsoleColor.DarkCyan;
                    m.SpeachIcon = '"';
                    m.CurGold = r.Next(500);
                    m.Color = ConsoleColor.Yellow;
                    m.Name = "Элла - Торговка одеждой";
                    m.Goods = new List<MechEngine.Item>() 
                    { 
                        ItemBase.SimplyCloth,
                        ItemBase.SimplyCloth,
                        ItemBase.SimplyCloth,
                        ItemBase.SimplyCloth,
                        ItemBase.SimplyCloth,
                        ItemBase.SimplyCloth,
                        ItemBase.SimplyCloth,
                        ItemBase.SimplyCloth,
                        ItemBase.SimplyBoots,
                        ItemBase.SimplyBoots,
                        ItemBase.SimplyHelm,
                        ItemBase.SimplyHelm
                    };
                    return m;
                }
            }

            public static MechEngine.NPC Rutger
            {
                get
                {
                    MechEngine.NPC n = new MechEngine.NPC();
                    n.Icon = 'R';
                    n.Color = ConsoleColor.DarkCyan;
                    n.Info = "Великолепный кузнец недавно прибывший в Мраумир. Говорят, что он может выковать даже особые доспехи.";
                    n.Name = "Рутгер";
                    n.Affix = "Кузнец";
                    #region Script
                    n.Script = () =>
                    {
                        PlayEngine.Enemy = false;
                        DrawEngine.Replica R = new DrawEngine.Replica();
                        R.Text = "Здравствуй, моя работа стоит 250 золотых плюс материалы. Что нибудь закажешь?";
                        R.TextColor = ConsoleColor.Gray;
                        R.Options.Add("[A] - Броня.");
                        R.Options.Add("[W] - Оружие.");
                        R.Options.Add("[H] - Шлем.");
                        R.Options.Add("[O] - Щит.");
                        R.Options.Add("[B] - Обувь");
                        R.Options.Add("[Escape] - Уйти.");
                        R.OptionsColor = ConsoleColor.DarkGray;
                        DrawEngine.ActiveObjectDraw.Draw(R, n);

                        bool EndDialogue = false;
                        while (!EndDialogue)
                        {
                            ConsoleKey push = Console.ReadKey(true).Key;
                            switch (push)
                            {
                                case ConsoleKey.A:
                                    {
                                        bool itgr = false;
                                        MechEngine.Item it = new MechEngine.Item();
                                        for (int i = 0; i < Rogue.RAM.Player.Inventory.Count; i++)
                                        {
                                            if (Rogue.RAM.Player.Inventory[i].Name == "Железо")
                                            {
                                                itgr = true;
                                                it = Rogue.RAM.Player.Inventory[i];
                                            }
                                        }

                                        if (Rogue.RAM.Player.Gold >= 250)
                                        {
                                            if (itgr)
                                            {
                                                Rogue.RAM.Player.Inventory.Remove(it);
                                                Rogue.RAM.Player.Gold -= 250;
                                                it = null;
                                                DrawEngine.GUIDraw.ReDrawCharInventory();

                                                for (int i = 0; i < 10; i++)
                                                {
                                                    DrawEngine.InfoWindow.Message = string.Format("Рутгер изготавливает броню... Осталось {0} сек..", 10 - i);
                                                    Thread.Sleep(1000);
                                                }
                                                it = ItemBase.CraftItemsFromNPC.Rutger.RutgerArmor;
                                                Rogue.RAM.Player.Inventory.Add(it);
                                                DrawEngine.InfoWindow.cMessage = new List<DrawEngine.ColoredWord>()
                                                            {
                                                                new DrawEngine.ColoredWord() { Color=n.Color, Word=n.Name},
                                                                new DrawEngine.ColoredWord(){Color=Rogue.RAM.Map.Biom, Word=" изготовил для вас "},
                                                                new DrawEngine.ColoredWord() { Color=it.Color, Word=it.Name},
                                                                new DrawEngine.ColoredWord(){Color=Rogue.RAM.Map.Biom, Word=" !"}
                                                            };
                                                DrawEngine.GUIDraw.DrawLab();
                                                R.Text = "Броня для тебя готова, на этот раз у меня получилось сделать <" + it.Name + ">!"; DrawEngine.ActiveObjectDraw.Draw(R, n);
                                            }
                                            else
                                            { R.Text = "У тебя нет нужного мне материала - <Железо>!"; DrawEngine.ActiveObjectDraw.Draw(R, n); }
                                        }
                                        else
                                        { R.Text = "У тебя не хватает золота на оплату моей работы!"; DrawEngine.ActiveObjectDraw.Draw(R, n); }
                                        break;
                                    }
                                case ConsoleKey.W:
                                    {
                                        bool itgr = false;
                                        MechEngine.Item it = new MechEngine.Item();
                                        for (int i = 0; i < Rogue.RAM.Player.Inventory.Count; i++)
                                        {
                                            if (Rogue.RAM.Player.Inventory[i].Name == "Железо")
                                            {
                                                itgr = true;
                                                it = Rogue.RAM.Player.Inventory[i];
                                            }
                                        }

                                        if (Rogue.RAM.Player.Gold >= 250)
                                        {
                                            if (itgr)
                                            {
                                                Rogue.RAM.Player.Inventory.Remove(it);
                                                Rogue.RAM.Player.Gold -= 250;
                                                it = null;
                                                DrawEngine.GUIDraw.ReDrawCharInventory();

                                                for (int i = 0; i < 10; i++)
                                                {
                                                    DrawEngine.InfoWindow.Message = string.Format("Рутгер изготавливает оружие... Осталось {0} сек..", 10 - i);
                                                    Thread.Sleep(1000);
                                                }
                                                it = ItemBase.CraftItemsFromNPC.Rutger.RutgerWeapon;
                                                Rogue.RAM.Player.Inventory.Add(it);
                                                DrawEngine.InfoWindow.cMessage = new List<DrawEngine.ColoredWord>()
                                                            {
                                                                new DrawEngine.ColoredWord() { Color=n.Color, Word=n.Name},
                                                                new DrawEngine.ColoredWord(){Color=Rogue.RAM.Map.Biom, Word=" изготовил для вас "},
                                                                new DrawEngine.ColoredWord() { Color=it.Color, Word=it.Name},
                                                                new DrawEngine.ColoredWord(){Color=Rogue.RAM.Map.Biom, Word=" !"}
                                                            };
                                                DrawEngine.GUIDraw.DrawLab();
                                                R.Text = "Оружие для тебя готово, на этот раз у меня получилось сделать <" + it.Name + ">!"; DrawEngine.ActiveObjectDraw.Draw(R, n);
                                            }
                                            else
                                            { R.Text = "У тебя нет нужного мне материала - <Железо>!"; DrawEngine.ActiveObjectDraw.Draw(R, n); }
                                        }
                                        else
                                        { R.Text = "У тебя не хватает золота на оплату моей работы!"; DrawEngine.ActiveObjectDraw.Draw(R, n); }
                                        break;
                                    }
                                case ConsoleKey.H:
                                    {
                                        bool itgr = false;
                                        MechEngine.Item it = new MechEngine.Item();
                                        for (int i = 0; i < Rogue.RAM.Player.Inventory.Count; i++)
                                        {
                                            if (Rogue.RAM.Player.Inventory[i].Name == "Железо")
                                            {
                                                itgr = true;
                                                it = Rogue.RAM.Player.Inventory[i];
                                            }
                                        }

                                        if (Rogue.RAM.Player.Gold >= 250)
                                        {
                                            if (itgr)
                                            {
                                                Rogue.RAM.Player.Inventory.Remove(it);
                                                Rogue.RAM.Player.Gold -= 250;
                                                it = null;
                                                DrawEngine.GUIDraw.ReDrawCharInventory();

                                                for (int i = 0; i < 10; i++)
                                                {
                                                    DrawEngine.InfoWindow.Message = string.Format("Рутгер изготавливает шлем... Осталось {0} сек..", 10 - i);
                                                    Thread.Sleep(1000);
                                                }
                                                it = ItemBase.CraftItemsFromNPC.Rutger.RutgerHelm;
                                                Rogue.RAM.Player.Inventory.Add(it);
                                                DrawEngine.InfoWindow.cMessage = new List<DrawEngine.ColoredWord>()
                                                            {
                                                                new DrawEngine.ColoredWord() { Color=n.Color, Word=n.Name},
                                                                new DrawEngine.ColoredWord(){Color=Rogue.RAM.Map.Biom, Word=" изготовил для вас "},
                                                                new DrawEngine.ColoredWord() { Color=it.Color, Word=it.Name},
                                                                new DrawEngine.ColoredWord(){Color=Rogue.RAM.Map.Biom, Word=" !"}
                                                            };
                                                DrawEngine.GUIDraw.DrawLab();
                                                R.Text = "Шлем для тебя готов, на этот раз у меня получилось сделать <" + it.Name + ">!"; DrawEngine.ActiveObjectDraw.Draw(R, n);
                                            }
                                            else
                                            { R.Text = "У тебя нет нужного мне материала - <Железо>!"; DrawEngine.ActiveObjectDraw.Draw(R, n); }
                                        }
                                        else
                                        { R.Text = "У тебя не хватает золота на оплату моей работы!"; DrawEngine.ActiveObjectDraw.Draw(R, n); }
                                        break;
                                    }
                                case ConsoleKey.O:
                                    {
                                        bool itgr = false;
                                        MechEngine.Item it = new MechEngine.Item();
                                        for (int i = 0; i < Rogue.RAM.Player.Inventory.Count; i++)
                                        {
                                            if (Rogue.RAM.Player.Inventory[i].Name == "Железо")
                                            {
                                                itgr = true;
                                                it = Rogue.RAM.Player.Inventory[i];
                                            }
                                        }

                                        if (Rogue.RAM.Player.Gold >= 250)
                                        {
                                            if (itgr)
                                            {
                                                Rogue.RAM.Player.Inventory.Remove(it);
                                                Rogue.RAM.Player.Gold -= 250;
                                                it = null;
                                                DrawEngine.GUIDraw.ReDrawCharInventory();

                                                for (int i = 0; i < 10; i++)
                                                {
                                                    DrawEngine.InfoWindow.Message = string.Format("Рутгер изготавливает щит... Осталось {0} сек..", 10 - i);
                                                    Thread.Sleep(1000);
                                                }
                                                it = ItemBase.CraftItemsFromNPC.Rutger.RutgerOffhand;
                                                Rogue.RAM.Player.Inventory.Add(it);
                                                DrawEngine.InfoWindow.cMessage = new List<DrawEngine.ColoredWord>()
                                                            {
                                                                new DrawEngine.ColoredWord() { Color=n.Color, Word=n.Name},
                                                                new DrawEngine.ColoredWord(){Color=Rogue.RAM.Map.Biom, Word=" изготовил для вас "},
                                                                new DrawEngine.ColoredWord() { Color=it.Color, Word=it.Name},
                                                                new DrawEngine.ColoredWord(){Color=Rogue.RAM.Map.Biom, Word=" !"}
                                                            };
                                                DrawEngine.GUIDraw.DrawLab();
                                                R.Text = "Щит для тебя готов, на этот раз у меня получилось сделать <" + it.Name + ">!"; DrawEngine.ActiveObjectDraw.Draw(R, n);
                                            }
                                            else
                                            { R.Text = "У тебя нет нужного мне материала - <Железо>!"; DrawEngine.ActiveObjectDraw.Draw(R, n); }
                                        }
                                        else
                                        { R.Text = "У тебя не хватает золота на оплату моей работы!"; DrawEngine.ActiveObjectDraw.Draw(R, n); }
                                        break;
                                    }
                                case ConsoleKey.B:
                                    {
                                        bool itgr = false;
                                        MechEngine.Item it = new MechEngine.Item();
                                        for (int i = 0; i < Rogue.RAM.Player.Inventory.Count; i++)
                                        {
                                            if (Rogue.RAM.Player.Inventory[i].Name == "Железо")
                                            {
                                                itgr = true;
                                                it = Rogue.RAM.Player.Inventory[i];
                                            }
                                        }

                                        if (Rogue.RAM.Player.Gold >= 250)
                                        {
                                            if (itgr)
                                            {
                                                Rogue.RAM.Player.Inventory.Remove(it);
                                                Rogue.RAM.Player.Gold -= 250;
                                                it = null;
                                                DrawEngine.GUIDraw.ReDrawCharInventory();

                                                for (int i = 0; i < 10; i++)
                                                {
                                                    DrawEngine.InfoWindow.Message = string.Format("Рутгер изготавливает обувь... Осталось {0} сек..", 10 - i);
                                                    Thread.Sleep(1000);
                                                }
                                                it = ItemBase.CraftItemsFromNPC.Rutger.RutgerBoots;
                                                Rogue.RAM.Player.Inventory.Add(it);
                                                DrawEngine.InfoWindow.cMessage = new List<DrawEngine.ColoredWord>()
                                                            {
                                                                new DrawEngine.ColoredWord() { Color=n.Color, Word=n.Name},
                                                                new DrawEngine.ColoredWord(){Color=Rogue.RAM.Map.Biom, Word=" изготовил для вас "},
                                                                new DrawEngine.ColoredWord() { Color=it.Color, Word=it.Name},
                                                                new DrawEngine.ColoredWord(){Color=Rogue.RAM.Map.Biom, Word=" !"}
                                                            };
                                                DrawEngine.GUIDraw.DrawLab();
                                                R.Text = "Обувь для тебя готова, на этот раз у меня получилось сделать <" + it.Name + ">!"; DrawEngine.ActiveObjectDraw.Draw(R, n);
                                            }
                                            else
                                            { R.Text = "У тебя нет нужного мне материала - <Железо>!"; DrawEngine.ActiveObjectDraw.Draw(R, n); }
                                        }
                                        else
                                        { R.Text = "У тебя не хватает золота на оплату моей работы!"; DrawEngine.ActiveObjectDraw.Draw(R, n); }
                                        break;
                                    }
                                case ConsoleKey.Escape:
                                    {
                                        EndDialogue = true;
                                        break;
                                    }
                            }
                        }

                        PlayEngine.Enemy = true;
                    };
                    #endregion
                    return n;
                }
            }

            public static MechEngine.Merchant Simantek
            {
                get
                {
                    MechEngine.Merchant m = new MechEngine.Merchant();
                    m.Icon = 'S';
                    m.MaxGold = 1000;
                    m.Color = ConsoleColor.Red;
                    m.Info = "Симантек посвятил всю свою жизнь свиткам, однако выучить он не смог ни один. Поэтому теперь он только продаёт их.";
                    m.SpeachColor = ConsoleColor.DarkCyan;
                    m.SpeachIcon = '"';
                    m.CurGold = r.Next(500);
                    m.Color = ConsoleColor.Yellow;
                    m.Name = "Симантек - Библиотекарь";
                    m.Goods = new List<MechEngine.Item>() 
                    { 
                        ItemBase.GetScroll,                        
                        ItemBase.GetScroll, 
                        ItemBase.GetScroll, 
                        ItemBase.GetScroll, 
                        ItemBase.GetScroll, 
                        ItemBase.GetScroll, 
                        ItemBase.GetScroll, 
                        ItemBase.GetScroll, 
                        ItemBase.GetScroll, 
                        ItemBase.GetScroll, 
                        ItemBase.GetScroll,  
                        ItemBase.AmuletOfMraumir, 
                    };
                    return m;
                }
            }

            public static MechEngine.NPC Tot
            {
                get
                {
                    MechEngine.NPC n = new MechEngine.NPC();
                    n.Icon = 'T';
                    n.Color = ConsoleColor.Magenta;
                    n.Name = "Тот";
                    n.Affix = "Мудрец";
                    n.Info = "Мудрый последователь валькирий. Тот охраняет потерянные знания валькирий.";
                    #region Script
                    n.Script = () =>
                        {
                            PlayEngine.Enemy = false;
                            DrawEngine.Replica R = new DrawEngine.Replica();
                            R.Text = "Да? Что ты хочешь? Пройти <испытание мудрости>?";
                            R.TextColor = ConsoleColor.DarkMagenta;
                            R.Options.Add("[Y] - Да.");
                            R.Options.Add("[N] - Нет.");
                            bool Stone = false;
                            foreach (MechEngine.Item itm in Rogue.RAM.Player.Inventory)
                            {
                                if (itm.Name == "Камень мудрости")
                                {
                                    R.Options.Add("[I] - Камень?");
                                    Stone = true;
                                }
                            }
                            R.Options.Add("[Escape] - Уйти.");
                            R.OptionsColor = ConsoleColor.DarkCyan;
                            DrawEngine.ActiveObjectDraw.Draw(R, n);

                            bool EndDialogue = false;
                            while (!EndDialogue)
                            {
                                ConsoleKey push = Console.ReadKey(true).Key;
                                switch (push)
                                {
                                    case ConsoleKey.Y:
                                        {
                                            R.Text = "Хорошо. Я задам тебе три загадки, ответом на них будет слово, если ты отгадаешь все <3> то я отдам тебе <Камень мудрости>. Ты готов?";
                                            DrawEngine.ActiveObjectDraw.Draw(R, n);
                                            ConsoleKey pushh = Console.ReadKey(true).Key;
                                            switch (pushh)
                                            {
                                                case ConsoleKey.Y:
                                                    {
                                                        List<string> quests = new List<string>();
                                                        quests.Add("Что вращает всё вокруг но не двигается?"); //зеркало
                                                        quests.Add("Что идёт вверх и вниз но никогда не движется?"); // лестница
                                                        quests.Add("Чем больше её есть, тем меньше вы видите?"); // темнота
                                                        List<string> answ = new List<string>();
                                                        answ.Add("зеркало");
                                                        answ.Add("лестница");
                                                        answ.Add("темнота");
                                                        int q = 0;
                                                        for (int i = 0; i < 3; i++)
                                                        {
                                                            R.Text = string.Format("{0} загадка: {1}",i+1,quests[i]);
                                                            R.Options.Clear();
                                                            DrawEngine.ActiveObjectDraw.Draw(R, n);
                                                            if (DrawEngine.ConsoleDraw.ReadFromInfoWindow("Введите ответ: ").ToLower() == answ[i])
                                                            {
                                                                q++;
                                                                continue;
                                                            }
                                                            else
                                                            {
                                                                break;
                                                            }
                                                        }
                                                        if (q == 3)
                                                        {
                                                            if (Rogue.RAM.Player.InventorySlots)
                                                            {
                                                                Rogue.RAM.Player.Inventory.Add(ResourseBase.IStone);
                                                                DrawEngine.GUIDraw.ReDrawCharInventory();
                                                                List<DrawEngine.ColoredWord> cww = new List<DrawEngine.ColoredWord>();
                                                                cww.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = "Вы получаете " });
                                                                cww.Add(new DrawEngine.ColoredWord() { Color = ConsoleColor.Magenta, Word = "Камень мудрости" });
                                                                cww.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = " !" });
                                                                DrawEngine.InfoWindow.cMessage = cww;
                                                                R.Text = "Поздравляю, ты разгадал все мои загадки! Хочешь ещё раз?";
                                                                R.Options.Clear();
                                                                R.Options.Add("[Y] - Да.");
                                                                R.Options.Add("[N] - Нет.");
                                                                R.Options.Add("[Escape] - Уйти.");
                                                                DrawEngine.ActiveObjectDraw.Draw(R, n);

                                                                #region Quest
                                                                bool here = false;
                                                                int index = 0;
                                                                bool delete = false;
                                                                foreach (MechEngine.Quest qq in Rogue.RAM.Player.QuestBook)
                                                                {
                                                                    if (qq == QuestBase.NPCQuestFEC.ThreeOrb)
                                                                    {
                                                                        here = true; qq.Progress++;
                                                                        if (qq.TargetCount == qq.Progress)
                                                                        {
                                                                            List<DrawEngine.ColoredWord> cq = new List<DrawEngine.ColoredWord>();
                                                                            cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = "Вы выполнили задание - " });
                                                                            cq.Add(new DrawEngine.ColoredWord() { Color = QuestBase.NPCQuestFEC.ThreeOrb.Color, Word = QuestBase.NPCQuestFEC.ThreeOrb.Name });
                                                                            cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = " !" });
                                                                            DrawEngine.InfoWindow.cMessage = cq;
                                                                            Thread.Sleep(410);
                                                                            Rogue.RAM.Player.QuestBook.Add(QuestBase.NPCQuestFEC.ColdOrb);
                                                                            index = Rogue.RAM.Player.QuestBook.IndexOf(qq);
                                                                            delete = true;
                                                                            cq = new List<DrawEngine.ColoredWord>();
                                                                            cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = "Вы получаете новое задание - " });
                                                                            cq.Add(new DrawEngine.ColoredWord() { Color = QuestBase.NPCQuestFEC.ColdOrb.Color, Word = QuestBase.NPCQuestFEC.ColdOrb.Name });
                                                                            cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = " !" });
                                                                            DrawEngine.InfoWindow.cMessage = cq;
                                                                        }
                                                                    }
                                                                }
                                                                if (delete) { Rogue.RAM.Player.QuestBook.Remove(Rogue.RAM.Player.QuestBook[index]); }
                                                                if (!here)
                                                                {
                                                                    Rogue.RAM.Player.QuestBook.Add(QuestBase.NPCQuestFEC.ThreeOrb);
                                                                    foreach (MechEngine.Quest qq in Rogue.RAM.Player.QuestBook)
                                                                    {
                                                                        if (qq == QuestBase.NPCQuestFEC.ThreeOrb)
                                                                        { qq.Progress++; }
                                                                    }
                                                                    List<DrawEngine.ColoredWord> cq = new List<DrawEngine.ColoredWord>();
                                                                    cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = "Вы получаете новое задание - " });
                                                                    cq.Add(new DrawEngine.ColoredWord() { Color = QuestBase.NPCQuestFEC.ThreeOrb.Color, Word = QuestBase.NPCQuestFEC.ThreeOrb.Name });
                                                                    cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = " !" });
                                                                    DrawEngine.InfoWindow.cMessage = cq;
                                                                }
                                                                #endregion
                                                            }
                                                            else
                                                            { DrawEngine.InfoWindow.Warning = "У вас нет места в инвентаре!"; }
                                                        }
                                                        else
                                                        {
                                                            R.Text = "На этот раз ты не смог разгадать все загадки, можешь попробовать ещё раз...";
                                                            R.Options.Clear();
                                                            R.Options.Add("[Y] - Да.");
                                                            R.Options.Add("[N] - Нет.");
                                                            R.Options.Add("[Escape] - Уйти.");
                                                            DrawEngine.ActiveObjectDraw.Draw(R, n);
                                                        }
                                                        break;
                                                    }
                                                case ConsoleKey.N:
                                                    {
                                                        R.Text = "Очень хорошо, потому что ты... не очень смышлен... Загадать их ещё раз?";
                                                        R.Options.Clear();
                                                        R.Options.Add("[Y] - Да.");
                                                        R.Options.Add("[N] - Нет.");
                                                        R.Options.Add("[Escape] - Уйти.");
                                                        DrawEngine.ActiveObjectDraw.Draw(R, n); break;
                                                    }
                                                case ConsoleKey.Escape: { break; }
                                            }
                                            break;
                                        }
                                    case ConsoleKey.N:
                                        {
                                            R.Text = "Очень хорошо, потому что ты... не очень смышлен... Загадать их ещё раз?";
                                            DrawEngine.ActiveObjectDraw.Draw(R, n);
                                            break;
                                        }
                                    case ConsoleKey.I:
                                        {
                                            if (Stone)
                                            {
                                                R.Text = "С помощью этого камня ты можешь создать камень сопряжения и попросить Бас превратить тебя в валькирию.";
                                                DrawEngine.ActiveObjectDraw.Draw(R, n);
                                            }
                                            break;
                                        }
                                    case ConsoleKey.Escape:
                                        {
                                            EndDialogue = true;
                                            break;
                                        }
                                }
                            }
                            PlayEngine.Enemy = true;
                        };
                    #endregion
                    return n;
                }
            }

            public static MechEngine.NPC Anu
            {
                get
                {
                    MechEngine.NPC n = new MechEngine.NPC();
                    n.Icon = 'A';
                    n.Color = ConsoleColor.DarkYellow;
                    n.Name = "Ану";
                    n.Affix = "Воин";
                    n.Info = "Последний воин-союзник валькирий, всю свою жизнь защищает последнюю из них - Бас.";
                    #region Script
                    n.Script = () =>
                    {
                        PlayEngine.Enemy = false;
                        DrawEngine.Replica R = new DrawEngine.Replica();
                        R.Text = "А ты готов к <испытанию силы>?";
                        R.TextColor = ConsoleColor.DarkMagenta;
                        R.Options.Add("[Y] - Да.");
                        R.Options.Add("[N] - Нет.");
                        bool Stone = false;
                        foreach (MechEngine.Item itm in Rogue.RAM.Player.Inventory)
                        {
                            if (itm.Name == "Камень силы")
                            {
                                R.Options.Add("[I] - Камень?");
                                Stone = true;
                            }
                        }
                        R.Options.Add("[Escape] - Уйти.");
                        R.OptionsColor = ConsoleColor.DarkCyan;
                        DrawEngine.ActiveObjectDraw.Draw(R, n);

                        bool EndDialogue = false;
                        while (!EndDialogue)
                        {
                            ConsoleKey push = Console.ReadKey(true).Key;
                            switch (push)
                            {
                                case ConsoleKey.Y:
                                    {
                                        R.Text = "Хорошо. Я дам тебе три боевых теоретических задачи, ответом на них будет слово, правильный ответ позволит тебе получить <Камень силы>. Ты готов?";
                                        DrawEngine.ActiveObjectDraw.Draw(R, n);
                                        ConsoleKey pushh = Console.ReadKey(true).Key;
                                        switch (pushh)
                                        {
                                            case ConsoleKey.Y:
                                                {
                                                    List<string> quests = new List<string>();
                                                    quests.Add("Враг дроу использует против тебя огонь или яд?"); //яд
                                                    quests.Add("Добивание стоит использовать когда у врага n% здоровья?"); // 25
                                                    quests.Add("Некроманты могут вернуть души призванных существ?"); // да
                                                    List<string> answ = new List<string>();
                                                    answ.Add("яд");
                                                    answ.Add("25");
                                                    answ.Add("да");
                                                    int q = 0;
                                                    for (int i = 0; i < 3; i++)
                                                    {
                                                        R.Text = string.Format("{0} загадка: {1}", i + 1, quests[i]);
                                                        R.Options.Clear();
                                                        DrawEngine.ActiveObjectDraw.Draw(R, n);
                                                        if (DrawEngine.ConsoleDraw.ReadFromInfoWindow("Введите ответ: ").ToLower() == answ[i])
                                                        {
                                                            q++;
                                                            continue;
                                                        }
                                                        else
                                                        {
                                                            break;
                                                        }
                                                    }
                                                    if (q == 3)
                                                    {
                                                        if (Rogue.RAM.Player.InventorySlots)
                                                        {
                                                            Rogue.RAM.Player.Inventory.Add(ResourseBase.SStone);
                                                            DrawEngine.GUIDraw.ReDrawCharInventory();
                                                            List<DrawEngine.ColoredWord> cww = new List<DrawEngine.ColoredWord>();
                                                            cww.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = "Вы получаете " });
                                                            cww.Add(new DrawEngine.ColoredWord() { Color = ConsoleColor.Magenta, Word = "Камень силы" });
                                                            cww.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = " !" });
                                                            DrawEngine.InfoWindow.cMessage = cww;
                                                            R.Text = "Ты смог пройти испытание силы, закрепим материал?";
                                                            R.Options.Clear();
                                                            R.Options.Add("[Y] - Да.");
                                                            R.Options.Add("[N] - Нет.");
                                                            R.Options.Add("[Escape] - Уйти.");
                                                            DrawEngine.ActiveObjectDraw.Draw(R, n);

                                                            #region Quest
                                                            bool here = false;
                                                            int index = 0;
                                                            bool delete = false;
                                                            foreach (MechEngine.Quest qq in Rogue.RAM.Player.QuestBook)
                                                            {
                                                                if (qq == QuestBase.NPCQuestFEC.ThreeOrb)
                                                                {
                                                                    here = true; qq.Progress++;
                                                                    if (qq.TargetCount == qq.Progress)
                                                                    {
                                                                        List<DrawEngine.ColoredWord> cq = new List<DrawEngine.ColoredWord>();
                                                                        cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = "Вы выполнили задание - " });
                                                                        cq.Add(new DrawEngine.ColoredWord() { Color = QuestBase.NPCQuestFEC.ThreeOrb.Color, Word = QuestBase.NPCQuestFEC.ThreeOrb.Name });
                                                                        cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = " !" });
                                                                        DrawEngine.InfoWindow.cMessage = cq;
                                                                        Thread.Sleep(410);
                                                                        Rogue.RAM.Player.QuestBook.Add(QuestBase.NPCQuestFEC.ColdOrb);
                                                                        index = Rogue.RAM.Player.QuestBook.IndexOf(qq);
                                                                        delete = true;
                                                                        cq = new List<DrawEngine.ColoredWord>();
                                                                        cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = "Вы получаете новое задание - " });
                                                                        cq.Add(new DrawEngine.ColoredWord() { Color = QuestBase.NPCQuestFEC.ColdOrb.Color, Word = QuestBase.NPCQuestFEC.ColdOrb.Name });
                                                                        cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = " !" });
                                                                        DrawEngine.InfoWindow.cMessage = cq;
                                                                    }
                                                                }
                                                            }
                                                            if (delete) { Rogue.RAM.Player.QuestBook.Remove(Rogue.RAM.Player.QuestBook[index]); }
                                                            if (!here)
                                                            {
                                                                Rogue.RAM.Player.QuestBook.Add(QuestBase.NPCQuestFEC.ThreeOrb);
                                                                foreach (MechEngine.Quest qq in Rogue.RAM.Player.QuestBook)
                                                                {
                                                                    if (qq == QuestBase.NPCQuestFEC.ThreeOrb)
                                                                    { qq.Progress++; }
                                                                }
                                                                List<DrawEngine.ColoredWord> cq = new List<DrawEngine.ColoredWord>();
                                                                cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = "Вы получаете новое задание - " });
                                                                cq.Add(new DrawEngine.ColoredWord() { Color = QuestBase.NPCQuestFEC.ThreeOrb.Color, Word = QuestBase.NPCQuestFEC.ThreeOrb.Name });
                                                                cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = " !" });
                                                                DrawEngine.InfoWindow.cMessage = cq;
                                                            }
                                                            #endregion
                                                        }
                                                        else
                                                        { DrawEngine.InfoWindow.Warning = "У вас нет места в инвентаре!"; }
                                                    }
                                                    else
                                                    {
                                                        R.Text = "Твоя боевая подготовка оставляет желать лучшего!";
                                                        R.Options.Clear();
                                                        R.Options.Add("[Y] - Да.");
                                                        R.Options.Add("[N] - Нет.");
                                                        R.Options.Add("[Escape] - Уйти.");
                                                        DrawEngine.ActiveObjectDraw.Draw(R, n);
                                                    }
                                                    break;
                                                }
                                            case ConsoleKey.N:
                                                {
                                                    R.Text = "Замечательно, можешь идти умирать! Или проверимся снова?";
                                                    R.Options.Clear();
                                                    R.Options.Add("[Y] - Да.");
                                                    R.Options.Add("[N] - Нет.");
                                                    R.Options.Add("[Escape] - Уйти.");
                                                    DrawEngine.ActiveObjectDraw.Draw(R, n); break;
                                                }
                                            case ConsoleKey.Escape: { break; }
                                        }
                                        break;
                                    }
                                case ConsoleKey.N:
                                    {
                                        R.Text = "Замечательно, можешь идти умирать! Или проверимся снова?";
                                        DrawEngine.ActiveObjectDraw.Draw(R, n);
                                        break;
                                    }
                                case ConsoleKey.I:
                                    {
                                        if (Stone)
                                        {
                                            R.Text = "Камень силы как и другие камни нужно использовать на алтаре сопряжения, и только потом идти к Бас.";
                                            DrawEngine.ActiveObjectDraw.Draw(R, n);
                                        }
                                        break;
                                    }
                                case ConsoleKey.Escape:
                                    {
                                        EndDialogue = true;
                                        break;
                                    }
                            }
                        }
                        PlayEngine.Enemy = true;
                    };
                    #endregion
                    return n;
                }
            }

            public static MechEngine.NPC Bas
            {
                get
                {
                    MechEngine.NPC n = new MechEngine.NPC();
                    n.Icon = 'B';
                    n.Color = ConsoleColor.Cyan;
                    n.Name = "Бас";
                    n.Affix = "Валькирия";
                    n.Info = "Последняя из рода Валькирий. Для того что бы понимать её речь нужен особый прибор, говорят такой есть у Уильяма-Мертвой руки.";
                    #region Script
                    n.Script = () =>
                    {
                        PlayEngine.Enemy = false;
                        DrawEngine.Replica R = new DrawEngine.Replica();

                        bool translate = false;
                        foreach (MechEngine.Item i in Rogue.RAM.Player.Inventory)
                        {
                            if (i.Name == "Ретронслятор V")
                            {
                                translate = true;
                            }
                        }

                        bool EndStone = false;
                        foreach (MechEngine.Item i in Rogue.RAM.Player.Inventory)
                        {
                            if (i.Name == "Камень сопряжения")
                            {
                                EndStone = true;
                            }
                        }

                        if (translate) { R.Text = "Привет? Подойди ближе, не бойся смертельного холода вокруг меня..."; }
                        else { R.Text = "◊╘┐╠∆╥≈⌐⌂<▬>?"; }

                        R.TextColor = ConsoleColor.DarkMagenta;

                        if (!translate)
                        {
                            R.Options.Add("[≈⌐⌂<] - ┐╠∆.");
                            R.Options.Add("[◊╘┐╠] - ∆╥≈.");
                            R.Options.Add("[╘┐▬>╥≈] - ⌡♀╪┬└▄.");
                        }
                        else
                        {
                            R.Options.Add("[Q] - Кто ты такая?.");
                            R.Options.Add("[V] - Камень?");
                            R.Options.Add("[S] - Валькирия.");
                            R.Options.Add("[Escape] - Уйти.");
                        }
                        
                        R.OptionsColor = ConsoleColor.DarkCyan;
                        DrawEngine.ActiveObjectDraw.Draw(R, n);

                        if (translate)
                        {
                            bool EndDialogue = false;
                            while (!EndDialogue)
                            {
                                ConsoleKey push = Console.ReadKey(true).Key;
                                switch (push)
                                {
                                    case ConsoleKey.Q:
                                        {
                                            R.Text = "Я - Бас. Последняя из выживших валькирий. Долгое время мы были рабами нежити, но после жестоких битв я смогла спастись. Теперь я могу создавать других валькирий.";
                                            DrawEngine.ActiveObjectDraw.Draw(R, n);
                                            break;
                                        }
                                    case ConsoleKey.V:
                                        {
                                            R.Text = "Ты смог достать ретронслятор для того что бы поговорить со мной, поэтому я отдам тебе <Камень ловкости> просто так.";
                                            DrawEngine.ActiveObjectDraw.Draw(R, n);
                                            if (Rogue.RAM.Player.InventorySlots)
                                            {
                                                Rogue.RAM.Player.Inventory.Add(ResourseBase.CStone);
                                                DrawEngine.GUIDraw.ReDrawCharInventory();
                                                List<DrawEngine.ColoredWord> cww = new List<DrawEngine.ColoredWord>();
                                                cww.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = "Вы получаете " });
                                                cww.Add(new DrawEngine.ColoredWord() { Color = ConsoleColor.Magenta, Word = "Камень ловкости" });
                                                cww.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = " !" });
                                                DrawEngine.InfoWindow.cMessage = cww;
                                                DrawEngine.ActiveObjectDraw.Draw(R, n);

                                                #region Quest
                                                bool here = false;
                                                int index = 0;
                                                bool delete = false;
                                                foreach (MechEngine.Quest qq in Rogue.RAM.Player.QuestBook)
                                                {
                                                    if (qq == QuestBase.NPCQuestFEC.ThreeOrb)
                                                    {
                                                        here = true; qq.Progress++;
                                                        if (qq.TargetCount == qq.Progress)
                                                        {
                                                            List<DrawEngine.ColoredWord> cq = new List<DrawEngine.ColoredWord>();
                                                            cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = "Вы выполнили задание - " });
                                                            cq.Add(new DrawEngine.ColoredWord() { Color = QuestBase.NPCQuestFEC.ThreeOrb.Color, Word = QuestBase.NPCQuestFEC.ThreeOrb.Name });
                                                            cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = " !" });
                                                            DrawEngine.InfoWindow.cMessage = cq;
                                                            Thread.Sleep(410);
                                                            Rogue.RAM.Player.QuestBook.Add(QuestBase.NPCQuestFEC.ColdOrb);
                                                            index = Rogue.RAM.Player.QuestBook.IndexOf(qq);
                                                            delete = true;
                                                            cq = new List<DrawEngine.ColoredWord>();
                                                            cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = "Вы получаете новое задание - " });
                                                            cq.Add(new DrawEngine.ColoredWord() { Color = QuestBase.NPCQuestFEC.ColdOrb.Color, Word = QuestBase.NPCQuestFEC.ColdOrb.Name });
                                                            cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = " !" });
                                                            DrawEngine.InfoWindow.cMessage = cq;
                                                        }
                                                    }
                                                }
                                                if (delete) { Rogue.RAM.Player.QuestBook.Remove(Rogue.RAM.Player.QuestBook[index]); }
                                                if (!here)
                                                {
                                                    Rogue.RAM.Player.QuestBook.Add(QuestBase.NPCQuestFEC.ThreeOrb);
                                                    foreach (MechEngine.Quest qq in Rogue.RAM.Player.QuestBook)
                                                    {
                                                        if (qq == QuestBase.NPCQuestFEC.ThreeOrb)
                                                        { qq.Progress++; }
                                                    }
                                                    List<DrawEngine.ColoredWord> cq = new List<DrawEngine.ColoredWord>();
                                                    cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = "Вы получаете новое задание - " });
                                                    cq.Add(new DrawEngine.ColoredWord() { Color = QuestBase.NPCQuestFEC.ThreeOrb.Color, Word = QuestBase.NPCQuestFEC.ThreeOrb.Name });
                                                    cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = " !" });
                                                    DrawEngine.InfoWindow.cMessage = cq;
                                                }
                                                #endregion
                                            }
                                            else
                                            { DrawEngine.InfoWindow.Warning = "У вас нет места в инвентаре!"; }
                                            break;
                                        }
                                    case ConsoleKey.S:
                                        {
                                            if (!EndStone)
                                            {
                                                R.Text = "Если ты принесёшь мне <Камень сопряжения>, тогда я смогу превратить тебя в валькирию.";
                                                DrawEngine.ActiveObjectDraw.Draw(R, n);
                                            }
                                            else
                                            {
                                                R.Text = "После ритуала твоя кожа станет оледенелой, и ты забудешь все свои навыки. Итак, ты готов? ";
                                                R.Options.Clear();
                                                R.Options.Add("[Y] - Да.");
                                                R.Options.Add("[N] - Нет.");
                                                DrawEngine.ActiveObjectDraw.Draw(R, n);
                                                 ConsoleKey pushh = Console.ReadKey(true).Key;
                                                 switch (pushh)
                                                 {
                                                     case ConsoleKey.Y:
                                                         {
                                                             R.Text = "Для того чтобы стать валькирией тебе нужно выбрать своё новое женское имя, скажи мне его.";
                                                             R.Options.Clear();
                                                             DrawEngine.ActiveObjectDraw.Draw(R, n);
                                                             bool endname = false;
                                                             string newname = "";
                                                             while (!endname)
                                                             {
                                                                 try
                                                                 {
                                                                     newname = DrawEngine.ConsoleDraw.ReadFromInfoWindow("Новое имя: ");
                                                                     if (newname[newname.Length - 1] != 'а' && newname[newname.Length - 1] != 'А')
                                                                     {
                                                                         continue;
                                                                     }
                                                                     else
                                                                     { endname = true; }
                                                                 }
                                                                 catch { }
                                                             }

                                                             R.Text = "Итак," + newname + ", мы начинаем ритуал!";
                                                             DrawEngine.ActiveObjectDraw.Draw(R, n);

                                                             for (int i = 0; i < 60; i++)
                                                             {
                                                                 DrawEngine.InfoWindow.Message = string.Format("Ритуал превращения в Валькирию... Осталось {0} сек..", 60 - i);
                                                                 Thread.Sleep(1000);
                                                             }

                                                             MechEngine.Item it = new MechEngine.Item();
                                                             for (int i = 0; i < Rogue.RAM.Player.Inventory.Count; i++)
                                                             {
                                                                 if (Rogue.RAM.Player.Inventory[i].Name == "Камень сопряжения")
                                                                 {
                                                                     it = Rogue.RAM.Player.Inventory[i];
                                                                 }
                                                             }
                                                             Rogue.RAM.Player.Inventory.Remove(it);

                                                             Rogue.RAM.Player.Name = newname;
                                                             Rogue.RAM.Player.Color = ConsoleColor.Cyan;

                                                             MechEngine.Perk.AddPerk(new string[] { "MRS + 10" }, "Валькирия", "Вы были превращены в Валькирию.", 'V', ConsoleColor.Cyan);
                                                             Rogue.RAM.Player.AddPerk(new MechEngine.Perk() { MRS = 10, Name = "Валькирия", History = "Вы были превращены в Валькирию.", Icon = 'V', Color = ConsoleColor.Cyan });
                                                             Rogue.RAM.Player.Class = MechEngine.BattleClass.Valkyrie;
                                                             Rogue.RAM.Player.MMP = (Rogue.RAM.Player.Level * 10) + 100;
                                                             Rogue.RAM.Player.CMP = (Rogue.RAM.Player.Level * 10) + 100;
                                                             Rogue.RAM.Player.Ability = EliteAbilityBase.Valkyrie;
                                                             List<DrawEngine.ColoredWord> cww = new List<DrawEngine.ColoredWord>();
                                                             cww.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = "Ваш класс персонажа изменён на " });
                                                             cww.Add(new DrawEngine.ColoredWord() { Color = ConsoleColor.Cyan, Word = "Валькирия" });
                                                             cww.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = " !" });
                                                             DrawEngine.InfoWindow.cMessage = cww;

                                                             R.Text = newname + ", теперь ты должна охранять Мраумир, а я отправляюсь в другие миры!";
                                                             R.Options.Clear();
                                                             R.Options.Add("[Escape] - Уйти.");
                                                             DrawEngine.ActiveObjectDraw.Draw(R, n);                 

                                                             Console.ReadKey(true);
                                                             #region Quest

                                                             int index = 0;
                                                             bool delete = false;
                                                             foreach (MechEngine.Quest qq in Rogue.RAM.Player.QuestBook)
                                                             {
                                                                 if (qq == QuestBase.NPCQuestFEC.StayValkyrie)
                                                                 {
                                                                     List<DrawEngine.ColoredWord> cq = new List<DrawEngine.ColoredWord>();
                                                                     cww.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = "Вы выполнили задание - " });
                                                                     cww.Add(new DrawEngine.ColoredWord() { Color = QuestBase.NPCQuestFEC.ThreeOrb.Color, Word = QuestBase.NPCQuestFEC.ThreeOrb.Name });
                                                                     cww.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = " !" });
                                                                     DrawEngine.InfoWindow.cMessage = cq;
                                                                     delete = true;
                                                                     index = Rogue.RAM.Player.QuestBook.IndexOf(qq);
                                                                 }
                                                             }
                                                             if (delete) { Rogue.RAM.Player.QuestBook.Remove(Rogue.RAM.Player.QuestBook[index]); }

                                                             #endregion

                                                             SystemEngine.Helper.Information.Point p = SystemEngine.Helper.Information.FindObject(n);
                                                             Rogue.RAM.Map.Map[p.x][p.y].Object = null;
                                                             DrawEngine.winAPIDraw.Helpers.ReDrawOneChar((Int16)p.x, (Int16)p.y, (Int16)p.x, (Int16)p.y);

                                                             EndDialogue = true;

                                                             break;
                                                         }
                                                 }
                                            }
                                            break;
                                        }
                                    case ConsoleKey.Escape:
                                        {
                                            EndDialogue = true;
                                            break;
                                        }
                                }
                            }
                        }
                        else
                        { Console.ReadKey(true); }
                        PlayEngine.Enemy = true;
                    };
                    #endregion
                    return n;
                }
            }

            public static MechEngine.NPC Stephan
            {
                get
                {
                    MechEngine.NPC n = new MechEngine.NPC();
                    n.Icon = 'S';
                    n.Color = ConsoleColor.DarkCyan;
                    n.Name = "Стефан";
                    n.Info = "Этот мёртвый выглядит очень ухоженным, не смотря на отвалившийся глаз и отсутствие кожи он одет в очень приличный костюм.";
                    #region Script
                    n.Script = () =>
                        {
                            PlayEngine.Enemy = false;

                            bool Fight = false;
                            bool haveFarg = false;
                            bool endofquest = false;
                            bool onetime = false;
                            foreach (MechEngine.Quest q in Rogue.RAM.Player.QuestBook)
                            {
                                if (q.Name == "Мертвая верность")
                                {
                                    Fight = true;
                                }
                                if (q.Name == "5 Причин")
                                {
                                    haveFarg = true;
                                    if (q.Progress == q.TargetCount)
                                    {
                                        endofquest = true;
                                    }
                                }
                            }

                            DrawEngine.Replica R = new DrawEngine.Replica();

                            if (!Fight)
                            {
                                if (Rogue.RAM.Flags.Valery) { R.Text = "Ты!? Ты пришёл от Валери? Не смей подходить ко мне если ты от неё!!!"; }
                                else { R.Text = "Ты!? Рад тебя видеть, убийца чужих жён."; }
                                R.TextColor = ConsoleColor.DarkCyan;
                                R.Options.Add("[W] - Кто ты?");
                                R.Options.Add("[R] - Кто Валери?");
                                if (!haveFarg && Rogue.RAM.Flags.Valery) { R.Options.Add("[E] - Нужна помощь?"); }
                                if (endofquest) { R.Options.Add("[U] - Валери мертва."); }
                                R.Options.Add("[Escape] - Уйти.");
                                R.OptionsColor = ConsoleColor.DarkCyan;
                                DrawEngine.ActiveObjectDraw.Draw(R, n);
                            }
                            else
                            {
                                R.Text = "Ты был у неё. Пока ещё не поздно ты можешь поговорить с моим пажем. ИНАЧЕ Я УБЬЮ ТЕБЯ!!!";
                                R.TextColor = ConsoleColor.Red;
                                R.Options.Add("[A] - Взять кольцо.");
                                R.Options.Add("[Escape] - Уйти.");
                                R.OptionsColor = ConsoleColor.DarkCyan;
                                DrawEngine.ActiveObjectDraw.Draw(R, n);
                            }

                            bool EndDialogue = false;
                            while (!EndDialogue)
                            {
                                ConsoleKey push = Console.ReadKey(true).Key;
                                switch (push)
                                {
                                    case ConsoleKey.W:
                                        {
                                            if (Rogue.RAM.Flags.Valery) { R.Text = "Меня зовут Стефан. Я был одним из лордов братства... когда был живым. Но потом Валери убила меня и Грегори..."; }
                                            else { R.Text = "Я твой заказчик, забыл уже как ты убил мою и без того мёртвую женушку?"; }
                                            DrawEngine.ActiveObjectDraw.Draw(R, n);
                                            break;
                                        }
                                    case ConsoleKey.R:
                                        {
                                            if (Rogue.RAM.Flags.Valery) { R.Text = "Валери моя жена... бывшая. Она очень фанатично любила меня, поэтому убила нас троих и воскресила мёртвыми."; }
                                            else { R.Text = "Валери моя жена... мертвая. На этот раз надеюсь полностью."; }
                                            DrawEngine.ActiveObjectDraw.Draw(R, n);
                                            break;
                                        }
                                    case ConsoleKey.E:
                                        {
                                            if (Rogue.RAM.Flags.Valery)
                                            {
                                                if (!haveFarg)
                                                {
                                                    R.Text = "Помощь мне бы не помешала... Знаешь, когда мы были живыми, я был очень сильным гулякой, и однажды встретил Валери...";
                                                    DrawEngine.ActiveObjectDraw.Draw(R, n);
                                                    DrawEngine.InfoWindow.Warning = "Нажмите любую клавишу для продолжения...";
                                                    Console.ReadKey(true);
                                                    R.Text = "Она была просто прекрасна, и вскоре мы поженились. Мы жили долго и счастливо, пока меня, ну знаешь... не понесло на приключения.";
                                                    DrawEngine.ActiveObjectDraw.Draw(R, n);
                                                    DrawEngine.InfoWindow.Message = "Нажмите любую клавишу для продолжения...";
                                                    Console.ReadKey(true);
                                                    R.Text = "За время своего приключения я повстречал около сотни прекрасных девушек, и с ужасом думал что будет когда я вернусь домой.";
                                                    DrawEngine.ActiveObjectDraw.Draw(R, n);
                                                    DrawEngine.InfoWindow.Warning = "Нажмите любую клавишу для продолжения...";
                                                    Console.ReadKey(true);
                                                    R.Text = "Но тут мне улыбнулась удача, и случайно я узнал что у моего пажа есть возможность стирать воспоминания!";
                                                    DrawEngine.ActiveObjectDraw.Draw(R, n);
                                                    DrawEngine.InfoWindow.Message = "Нажмите любую клавишу для продолжения...";
                                                    Console.ReadKey(true);
                                                    R.Text = "Когда мы вернулись домой, Грегори стёр все воспоминания Валери о слухах и доносах на меня, и после этого мы прожили ещё один год...";
                                                    DrawEngine.ActiveObjectDraw.Draw(R, n);
                                                    DrawEngine.InfoWindow.Warning = "Нажмите любую клавишу для продолжения...";
                                                    Console.ReadKey(true);
                                                    R.Text = "В конце этого счастливого года у меня обнаружили страшную болезнь, она называлась: Какая Очень Болезненная Едкая Леди Ь.";
                                                    DrawEngine.ActiveObjectDraw.Draw(R, n);
                                                    DrawEngine.InfoWindow.Message = "Нажмите любую клавишу для продолжения...";
                                                    Console.ReadKey(true);
                                                    R.Text = "Я стремительно умирал, и поэтому Валери нашла способ убить нас троих и воскресить в виде нежити, на что я глупо согласился.";
                                                    DrawEngine.ActiveObjectDraw.Draw(R, n);
                                                    DrawEngine.InfoWindow.Warning = "Нажмите любую клавишу для продолжения...";
                                                    Console.ReadKey(true);
                                                    R.Text = "После ритуала оказалось что сила Грегори работает только на живых, и Валери вспомнила не только слухи, но и то что ей стёрли память.";
                                                    DrawEngine.ActiveObjectDraw.Draw(R, n);
                                                    DrawEngine.InfoWindow.Message = "Нажмите любую клавишу для продолжения...";
                                                    Console.ReadKey(true);
                                                    R.Text = "Теперь она хочет меня убить, и посылает за мной разных 'героев' которые считают меня зомби. И, знаешь ли, мне это надоело!";
                                                    DrawEngine.ActiveObjectDraw.Draw(R, n);
                                                    DrawEngine.InfoWindow.Warning = "Нажмите любую клавишу для продолжения...";
                                                    Console.ReadKey(true);
                                                    R.Text = "Не мог бы ты убить её? Я дам тебе инструкции, там 5 вариантов, если у тебя не получится один из них, сходи к Грегори, он поможет.";
                                                    DrawEngine.ActiveObjectDraw.Draw(R, n);


                                                    Rogue.RAM.Player.QuestBook.Add(QuestBase.NPCQuestFEC.FiveArguments);
                                                    List<DrawEngine.ColoredWord> cq = new List<DrawEngine.ColoredWord>();
                                                    cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = "Вы получаете новое задание - " });
                                                    cq.Add(new DrawEngine.ColoredWord() { Color = QuestBase.NPCQuestFEC.FiveArguments.Color, Word = QuestBase.NPCQuestFEC.FiveArguments.Name });
                                                    cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = " !" });
                                                    DrawEngine.InfoWindow.cMessage = cq;
                                                    haveFarg = true;
                                                }
                                            }
                                            break;
                                        }
                                    case ConsoleKey.A:
                                        {
                                            if (Fight)
                                            {
                                                R.Text = "Что ты делаешь? Ты хочешь забрать у меня моё обручальное кольцо! Этого никогда не случится, готовься к смерти!";
                                                DrawEngine.ActiveObjectDraw.Draw(R, n);
                                                DrawEngine.InfoWindow.Warning = "Нажмите любую клавишу для битвы!";
                                                Console.ReadKey(true);

                                                //step++
                                                Rogue.RAM.Step.Step();
                                                //enemy
                                                Rogue.RAM.Enemy = MobBase.Stephan;
                                                //draw battle
                                                DrawEngine.FightDraw.DrawFight();
                                                //log and x,y
                                                Rogue.RAM.Log = new List<string>();
                                                //find enemy x,y
                                                SystemEngine.Helper.Information.Point p = new SystemEngine.Helper.Information.Point();
                                                p = SystemEngine.Helper.Information.FindEnemy(Rogue.RAM.Enemy);
                                                //for loot backing
                                                Rogue.RAM.EnemyX = 51;
                                                Rogue.RAM.EnemyY = 2;
                                                Rogue.RAM.Map.Map[51][3].Enemy = MobBase.Stephan;
                                                Rogue.RAM.Map.Map[51][3].Item = Rogue.RAM.Enemy.Loot;
                                                Rogue.RAM.Map.Map[51][3].Enemy = null;
                                                Rogue.RAM.Map.Map[51][3].Object = null;
                                                //flag
                                                Rogue.RAM.Flags.Stephan = false;
                                                //next
                                                SystemEngine.Helper.Activation.StartBattle();
                                                DrawEngine.CharMap.DrawCMap(new List<string>() 
                                                { 
                                                    "[Q] - "+Rogue.RAM.Player.Ability[0].Name,
                                                    "[W] - "+Rogue.RAM.Player.Ability[1].Name, 
                                                    "[E] - "+Rogue.RAM.Player.Ability[2].Name,
                                                    "[R] - "+Rogue.RAM.Player.Ability[3].Name,
                                                    "[A] - Удар рукой",
                                                    "[D] - Защищаться ",
                                                    "[S] - Попытаться сбежать",
                                                    "[1-6] - Инвентарь",
                                                });

                                                int d = 0;
                                                foreach (MechEngine.Quest qq in Rogue.RAM.Player.QuestBook)
                                                {
                                                    if (qq.Name == QuestBase.NPCQuestFEC.DeadMonogamy.Name)
                                                    {
                                                        d = Rogue.RAM.Player.QuestBook.IndexOf(qq);
                                                    }
                                                }
                                                Rogue.RAM.Player.QuestBook[d].Progress++;

                                                PlayEngine.GamePlay.Fight(true);

                                                
                                                EndDialogue = true;
                                            }
                                            break;
                                        }
                                    case ConsoleKey.U:
                                        {
                                            if (endofquest && !onetime)
                                            {
                                                R.Text = "Как? О БОЖЕ! Ты убил мою жену?... Надеюсь что это действительно так. В знак благодарности можешь использовать Грегори.";
                                                DrawEngine.ActiveObjectDraw.Draw(R, n);

                                                int index = 0;
                                                foreach (MechEngine.Quest qq in Rogue.RAM.Player.QuestBook)
                                                {
                                                    if (qq.Name == QuestBase.NPCQuestFEC.FiveArguments.Name)
                                                    {
                                                        index = Rogue.RAM.Player.QuestBook.IndexOf(qq);
                                                        List<DrawEngine.ColoredWord> cq = new List<DrawEngine.ColoredWord>();
                                                        cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = "Вы выполнили задание - " });
                                                        cq.Add(new DrawEngine.ColoredWord() { Color = QuestBase.NPCQuestFEC.FiveArguments.Color, Word = QuestBase.NPCQuestFEC.FiveArguments.Name });
                                                        cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = " !" });
                                                        DrawEngine.InfoWindow.cMessage = cq;
                                                    }
                                                }
                                                Rogue.RAM.Player.QuestBook.Remove(Rogue.RAM.Player.QuestBook[index]);
                                                onetime = true;
                                            }
                                            break;
                                        }
                                    case ConsoleKey.Escape:
                                        {
                                            EndDialogue = true;
                                            break;
                                        }
                                }
                            }
                            PlayEngine.Enemy = true;
                        };
                    #endregion
                    n.Move = false;
                    n.Affix = "Мертвый лорд";
                    return n;
                }
            }

            public static MechEngine.NPC Grave
            {
                get
                {
                    MechEngine.NPC n = new MechEngine.NPC();
                    switch (Rogue.RAM.Flags.UndeadRising)
                    {
                        case 0:
                            {
                                n.Icon = '†';
                                n.Color = ConsoleColor.Red; break;
                            }
                        case 1:
                            {
                                n.Icon = '╫';
                                n.Color = ConsoleColor.DarkRed;
                                break;
                            }
                        case 2:
                            {
                                n.Icon = '‡';
                                n.Color = ConsoleColor.DarkYellow; break;
                            }
                        case 3:
                            {
                                n.Icon = '⌂';
                                n.Color = ConsoleColor.Green; break;
                            }
                    }
                    n.Name = "Подопытный";
                    n.Affix = "Мертвый";
                    n.Info = "Попробуйте заговорить с подопытным.";
                    n.UseScript = true;
                    n.Script = () =>
                        {
                            PlayEngine.Enemy = false;
                            int index = -1;
                            bool del = false;
                            foreach (MechEngine.Quest q in Rogue.RAM.Player.QuestBook)
                            {
                                if (q.Name.IndexOf("Опыты") > -1)
                                {
                                    q.Progress++;
                                    if (q.Progress == q.TargetCount)
                                    {
                                        List<DrawEngine.ColoredWord> cq = new List<DrawEngine.ColoredWord>();
                                        cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = "Вы выполнили задание - " });
                                        cq.Add(new DrawEngine.ColoredWord() { Color = q.Color, Word = q.Name });
                                        cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = " !" });
                                        DrawEngine.InfoWindow.cMessage = cq;
                                        index = Rogue.RAM.Player.QuestBook.IndexOf(q);
                                        del = true;
                                    }
                                }
                            }
                            SystemEngine.Helper.Information.Point p = SystemEngine.Helper.Information.FindObject(n);
                            Rogue.RAM.Map.Map[p.x][p.y].Object = null;
                            DrawEngine.winAPIDraw.Helpers.ReDrawOneChar((short)p.x, (short)p.y, (short)p.x, (short)p.y);
                            DrawEngine.InfoWindow.cMessage = new List<DrawEngine.ColoredWord>() { new DrawEngine.ColoredWord() { Color = ConsoleColor.Green, Word = "Вы провели опыты с чумой!" } };
                            if (del)
                            {
                                Rogue.RAM.Flags.UndeadRising++;
                                Rogue.RAM.Player.QuestBook.Remove(Rogue.RAM.Player.QuestBook[index]);
                                DataBase.DoorBase.UndeadDoor.Use();
                                //p = SystemEngine.Helper.Information.FindPlayer(Rogue.RAM.Player);
                                //Rogue.RAM.Map.Map[p.x][p.y].Player = null;
                                //DrawEngine.winAPIDraw.Helpers.ReDrawOneChar((short)p.x, (short)p.y, (short)p.x, (short)p.y, ' ', ' ', 0, 0);
                                //Rogue.RAM.Map.Map[34][11].Player = Rogue.RAM.Player;
                                //DrawEngine.winAPIDraw.Helpers.ReDrawOneChar(11, 34, 11, 34, '@', '@', Convert.ToInt16(Rogue.RAM.Player.Color), Convert.ToInt16(Rogue.RAM.Player.Color));
                            }
                            PlayEngine.Enemy = true;
                        };
                    return n;
                }
            }

            public static MechEngine.NPC Gregory
            {
                get
                {
                    MechEngine.NPC n = new MechEngine.NPC();
                    n.Icon = 'G';
                    n.Color = ConsoleColor.DarkGray;
                    n.Move = false;
                    n.Name = "Грегори";
                    n.Affix = "Мертвый паж";
                    n.Info = "Мёртвый слуга лорда Стефана, поговаривают что при жизни у него были невероятные способности мага.";
                    #region Script
                    n.Script = () =>
                        {
                            PlayEngine.Enemy = false;
                            DrawEngine.Replica R = new DrawEngine.Replica();
                            if (!Rogue.RAM.Flags.Stephan || !Rogue.RAM.Flags.Valery) { R.Text = "Приветствую вас! Всего 500 золотых и я наложу на вас забвение!"; }
                            else
                            { R.Text = "Я мертвец, нельзя ли меня оставить в покое???"; }

                            R.TextColor = ConsoleColor.DarkGray;
                            if (!Rogue.RAM.Flags.Stephan || !Rogue.RAM.Flags.Valery) { R.Options.Add("[S] - Забвение."); }
                            R.Options.Add("[Escape] - Уйти.");
                            R.OptionsColor = ConsoleColor.DarkGray;
                            DrawEngine.ActiveObjectDraw.Draw(R, n);

                            bool EndDialogue = false;
                            while (!EndDialogue)
                            {
                                ConsoleKey push = Console.ReadKey(true).Key;
                                switch (push)
                                {
                                    case ConsoleKey.S:
                                        {
                                            if (!Rogue.RAM.Flags.Stephan || !Rogue.RAM.Flags.Valery)
                                            {
                                                if (Rogue.RAM.Player.Gold >= 500)
                                                {
                                                    Rogue.RAM.Player.Gold -= 500;
                                                    MechEngine.Ability cs = OtherAbilityBase.Gregory;
                                                    cs.Activate();
                                                }
                                            }
                                            break;
                                        }
                                    case ConsoleKey.Escape:
                                        {
                                            EndDialogue = true;
                                            break;
                                        }
                                }
                            }
                        };
                    #endregion
                    return n;
                }
            }

            public static MechEngine.NPC Valery
            {
                get
                {
                    MechEngine.NPC n = new MechEngine.NPC();
                    n.Icon = 'V';
                    n.Color = ConsoleColor.Magenta;
                    n.Name = "Валери";
                    n.Info = "Прекрасная Валери - жена лорда Стефана, она бла словно морская волна, теперь же вы можете рассмотреть её фигуру не вооружённым взглядом...";
                    #region Script
                    n.Script = () =>
                    {
                        PlayEngine.Enemy = false;

                        bool Fight = false;
                        bool haveFarg = false;
                        bool endofquest = false;
                        bool onetime = false;
                        foreach (MechEngine.Quest q in Rogue.RAM.Player.QuestBook)
                        {
                            if (q.Name == "Мертвая верность")
                            {
                                Fight = true;
                                if (q.Progress == 1)
                                {
                                    endofquest = true;
                                }
                            }
                            if (q.Name == "5 Причин")
                            {
                                haveFarg = true;
                            }
                        }

                        DrawEngine.Replica R = new DrawEngine.Replica();

                        if (!haveFarg)
                        {
                            R.Text = "Привет. Меня зовут Валери, я жена лорда Лядуна теперь известного как Стефан.";
                            R.TextColor = ConsoleColor.DarkYellow;
                            R.Options.Add("[W] - Как дела?");
                            R.Options.Add("[R] - Кто такой Стефан?");
                            if (!Fight && Rogue.RAM.Flags.Stephan) { R.Options.Add("[E] - Нужна помощь?"); }
                            if (endofquest) { R.Options.Add("[U] - Отдать кольцо."); }
                            R.Options.Add("[Escape] - Уйти.");
                            R.OptionsColor = ConsoleColor.DarkYellow;
                            DrawEngine.ActiveObjectDraw.Draw(R, n);
                        }
                        else
                        {
                            R.Text = "Валери стоит к вам спиной...";
                            R.TextColor = ConsoleColor.DarkMagenta;
                            R.Options.Add("[A] - Яд в бокале.");
                            R.Options.Add("[S] - Наковальня.");
                            R.Options.Add("[D] - Святая вода.");
                            R.Options.Add("[F] - Лучи солнца.");
                            R.Options.Add("[G] - Пуля в лоб.");
                            R.Options.Add("[Escape] - Уйти.");
                            R.OptionsColor = ConsoleColor.DarkCyan;
                            DrawEngine.ActiveObjectDraw.Draw(R, n);
                        }

                        bool EndDialogue = false;
                        while (!EndDialogue)
                        {
                            ConsoleKey push = Console.ReadKey(true).Key;
                            switch (push)
                            {
                                case ConsoleKey.W:
                                    {
                                        R.Text = "Мой возлюбленный изменил мне с сотней девушек, как ты думаешь у меня дела?";
                                        DrawEngine.ActiveObjectDraw.Draw(R, n);
                                        break;
                                    }
                                case ConsoleKey.R:
                                    {
                                        R.Text = "Бывший лорд трёх башен и самый известный гуляка в Мраумире. Ещё он мой мёртвый муж.";
                                        DrawEngine.ActiveObjectDraw.Draw(R, n);
                                        break;
                                    }
                                case ConsoleKey.E:
                                    {
                                        if (!Fight && Rogue.RAM.Flags.Stephan && !haveFarg)
                                        {
                                            R.Text = "О нашей истории лучше рассказывает Стефан, но я попробую объяснить всё в кратце.";
                                            DrawEngine.ActiveObjectDraw.Draw(R, n);
                                            DrawEngine.InfoWindow.Warning = "Нажмите любую клавишу для продолжения...";
                                            Console.ReadKey(true);
                                            R.Text = "После двух лет брака он изменил мне с целой сотней распутных девок, и стёр мне память что бы я не узнала об этом!";
                                            DrawEngine.ActiveObjectDraw.Draw(R, n);
                                            DrawEngine.InfoWindow.Message = "Нажмите любую клавишу для продолжения...";
                                            Console.ReadKey(true);
                                            R.Text = "Теперь я хочу получить его обручальное кольцо и убить его, так что можешь в этом помочь мне.";
                                            DrawEngine.ActiveObjectDraw.Draw(R, n);

                                            Rogue.RAM.Player.QuestBook.Add(QuestBase.NPCQuestFEC.DeadMonogamy);
                                            List<DrawEngine.ColoredWord> cq = new List<DrawEngine.ColoredWord>();
                                            cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = "Вы получаете новое задание - " });
                                            cq.Add(new DrawEngine.ColoredWord() { Color = QuestBase.NPCQuestFEC.DeadMonogamy.Color, Word = QuestBase.NPCQuestFEC.DeadMonogamy.Name });
                                            cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = " !" });
                                            DrawEngine.InfoWindow.cMessage = cq;
                                            Fight = true;
                                        }
                                        break;
                                    }
                                case ConsoleKey.U:
                                    {
                                        if (endofquest && !onetime)
                                        {
                                            R.Text = "Хорошо, теперь иди и убей его! Подожди-ка, это кольцо на... пальце!? Ладно, будем считать что я хотела этого...";
                                            DrawEngine.ActiveObjectDraw.Draw(R, n);

                                            int index = 0;
                                            foreach (MechEngine.Quest qq in Rogue.RAM.Player.QuestBook)
                                            {
                                                if (qq.Name == QuestBase.NPCQuestFEC.DeadMonogamy.Name)
                                                {
                                                    index = Rogue.RAM.Player.QuestBook.IndexOf(qq);
                                                    List<DrawEngine.ColoredWord> cq = new List<DrawEngine.ColoredWord>();
                                                    cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = "Вы выполнили задание - " });
                                                    cq.Add(new DrawEngine.ColoredWord() { Color = QuestBase.NPCQuestFEC.DeadMonogamy.Color, Word = QuestBase.NPCQuestFEC.DeadMonogamy.Name });
                                                    cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = " !" });
                                                    DrawEngine.InfoWindow.cMessage = cq;
                                                }
                                            }
                                            Rogue.RAM.Player.QuestBook.Remove(Rogue.RAM.Player.QuestBook[index]);
                                            foreach (MechEngine.Item it in Rogue.RAM.Player.Inventory)
                                            { if (it.Name == ResourseBase.UndeadRing.Name) { index = Rogue.RAM.Player.Inventory.IndexOf(it); } }
                                            Rogue.RAM.Player.Inventory.Remove(Rogue.RAM.Player.Inventory[index]);
                                            onetime = true;
                                        }
                                        break;
                                    }
                                case ConsoleKey.A:
                                    {
                                        if (haveFarg)
                                        {
                                            int d = 0;
                                            foreach (MechEngine.Quest qq in Rogue.RAM.Player.QuestBook)
                                            {
                                                if (qq.Name == QuestBase.NPCQuestFEC.FiveArguments.Name)
                                                {
                                                    d = Rogue.RAM.Player.QuestBook.IndexOf(qq);
                                                }
                                            }

                                            if (r.Next(2) == 0)
                                            {
                                                if (Rogue.RAM.Player.QuestBook[d].Progress < 5)
                                                {
                                                    R.Text = "Яд подействовал, Валери стало хуже.";
                                                    DrawEngine.ActiveObjectDraw.Draw(R, n);
                                                    Rogue.RAM.Player.QuestBook[d].Progress++;
                                                }
                                                else
                                                {
                                                    R.Text = "Яд подействовал, Валери позеленела и умерла.";
                                                    DrawEngine.ActiveObjectDraw.Draw(R, n);

                                                    SystemEngine.Helper.Information.Point p = SystemEngine.Helper.Information.FindObject(n);
                                                    Rogue.RAM.Map.Map[p.x][p.y].Object = null;
                                                    DrawEngine.winAPIDraw.Helpers.ReDrawOneChar((Int16)p.x, (Int16)p.y, (Int16)p.x, (Int16)p.y);                                                    

                                                    Rogue.RAM.Flags.Valery = false;
                                                    EndDialogue = true;
                                                }
                                            }
                                            else
                                            {
                                                R.Text = "На этот раз яд не подействовал.";
                                                DrawEngine.ActiveObjectDraw.Draw(R, n);
                                            }
                                        }
                                        break;
                                    }
                                case ConsoleKey.S:
                                    {
                                        if (haveFarg)
                                        {
                                            int d = 0;
                                            foreach (MechEngine.Quest qq in Rogue.RAM.Player.QuestBook)
                                            {
                                                if (qq.Name == QuestBase.NPCQuestFEC.FiveArguments.Name)
                                                {
                                                    d = Rogue.RAM.Player.QuestBook.IndexOf(qq);
                                                }
                                            }

                                            if (r.Next(2) == 0)
                                            {
                                                if (Rogue.RAM.Player.QuestBook[d].Progress < 5)
                                                {
                                                    R.Text = "Наковальня придавила Валери.";
                                                    DrawEngine.ActiveObjectDraw.Draw(R, n);
                                                    Rogue.RAM.Player.QuestBook[d].Progress++;
                                                }
                                                else
                                                {
                                                    R.Text = "Наковальня пролетела мимо.";
                                                    DrawEngine.ActiveObjectDraw.Draw(R, n);

                                                    SystemEngine.Helper.Information.Point p = SystemEngine.Helper.Information.FindObject(n);
                                                    Rogue.RAM.Map.Map[p.x][p.y].Object = null;
                                                    DrawEngine.winAPIDraw.Helpers.ReDrawOneChar((Int16)p.x, (Int16)p.y, (Int16)p.x, (Int16)p.y);
                                                    

                                                    Rogue.RAM.Flags.Valery = false;
                                                    EndDialogue = true;
                                                }
                                            }
                                            else
                                            {
                                                R.Text = "Наковальня пролетела мимо.";
                                                DrawEngine.ActiveObjectDraw.Draw(R, n);
                                            }
                                        }
                                        break;
                                    }
                                case ConsoleKey.D:
                                    {
                                        if (haveFarg)
                                        {
                                            int d = 0;
                                            foreach (MechEngine.Quest qq in Rogue.RAM.Player.QuestBook)
                                            {
                                                if (qq.Name == QuestBase.NPCQuestFEC.FiveArguments.Name)
                                                {
                                                    d = Rogue.RAM.Player.QuestBook.IndexOf(qq);
                                                }
                                            }

                                            if (r.Next(2) == 0)
                                            {
                                                if (Rogue.RAM.Player.QuestBook[d].Progress < 5)
                                                {
                                                    R.Text = "Святая вода жгёт Валери.";
                                                    DrawEngine.ActiveObjectDraw.Draw(R, n);
                                                    Rogue.RAM.Player.QuestBook[d].Progress++;
                                                }
                                                else
                                                {
                                                    R.Text = "Святая вода сожгла Валери кости.";
                                                    DrawEngine.ActiveObjectDraw.Draw(R, n);

                                                    SystemEngine.Helper.Information.Point p = SystemEngine.Helper.Information.FindObject(n);
                                                    Rogue.RAM.Map.Map[p.x][p.y].Object = null;
                                                    DrawEngine.winAPIDraw.Helpers.ReDrawOneChar((Int16)p.x, (Int16)p.y, (Int16)p.x, (Int16)p.y);
                                                    

                                                    Rogue.RAM.Flags.Valery = false;
                                                    EndDialogue = true;
                                                }
                                            }
                                            else
                                            {
                                                R.Text = "Святая вода оказалось не освещенной.";
                                                DrawEngine.ActiveObjectDraw.Draw(R, n);
                                            }
                                        }
                                        break;
                                    }
                                case ConsoleKey.F:
                                    {
                                        if (haveFarg)
                                        {
                                            int d = 0;
                                            foreach (MechEngine.Quest qq in Rogue.RAM.Player.QuestBook)
                                            {
                                                if (qq.Name == QuestBase.NPCQuestFEC.FiveArguments.Name)
                                                {
                                                    d = Rogue.RAM.Player.QuestBook.IndexOf(qq);
                                                }
                                            }

                                            if (r.Next(2) == 0)
                                            {
                                                if (Rogue.RAM.Player.QuestBook[d].Progress < 5)
                                                {
                                                    R.Text = "Лучи солнца негативно влияют на Валери.";
                                                    DrawEngine.ActiveObjectDraw.Draw(R, n);
                                                    Rogue.RAM.Player.QuestBook[d].Progress++;
                                                }
                                                else
                                                {
                                                    R.Text = "Лучи солнца растопили кожу Валери.";
                                                    DrawEngine.ActiveObjectDraw.Draw(R, n);

                                                    SystemEngine.Helper.Information.Point p = SystemEngine.Helper.Information.FindObject(n);
                                                    Rogue.RAM.Map.Map[p.x][p.y].Object = null;
                                                    DrawEngine.winAPIDraw.Helpers.ReDrawOneChar((Int16)p.x, (Int16)p.y, (Int16)p.x, (Int16)p.y);

                                                    EndDialogue = true;
                                                    Rogue.RAM.Flags.Valery = false;
                                                }
                                            }
                                            else
                                            {
                                                R.Text = "Лучи солнца, серьёзно? Она же не вампир!";
                                                DrawEngine.ActiveObjectDraw.Draw(R, n);
                                            }
                                        }
                                        break;
                                    }
                                case ConsoleKey.G:
                                    {
                                        if (haveFarg)
                                        {
                                            int d = 0;
                                            foreach (MechEngine.Quest qq in Rogue.RAM.Player.QuestBook)
                                            {
                                                if (qq.Name == QuestBase.NPCQuestFEC.FiveArguments.Name)
                                                {
                                                    d = Rogue.RAM.Player.QuestBook.IndexOf(qq);
                                                }
                                            }

                                            if (r.Next(2) == 0)
                                            {
                                                if (Rogue.RAM.Player.QuestBook[d].Progress < 5)
                                                {
                                                    R.Text = "Пуля травмирует Валери.";
                                                    DrawEngine.ActiveObjectDraw.Draw(R, n);
                                                    Rogue.RAM.Player.QuestBook[d].Progress++;
                                                }
                                                else
                                                {
                                                    R.Text = "Валери благословили пулей в лоб.";
                                                    DrawEngine.ActiveObjectDraw.Draw(R, n);

                                                    SystemEngine.Helper.Information.Point p = SystemEngine.Helper.Information.FindObject(n);
                                                    Rogue.RAM.Map.Map[p.x][p.y].Object = null;
                                                    DrawEngine.winAPIDraw.Helpers.ReDrawOneChar((Int16)p.x, (Int16)p.y, (Int16)p.x, (Int16)p.y);

                                                    EndDialogue = true;
                                                    Rogue.RAM.Flags.Valery = false;
                                                }
                                            }
                                            else
                                            {
                                                R.Text = "Пуля отрекошетила в вас.";
                                                DrawEngine.ActiveObjectDraw.Draw(R, n);
                                            }
                                        }
                                        break;
                                    }
                                case ConsoleKey.Escape:
                                    {
                                        EndDialogue = true;
                                        break;
                                    }
                            }
                        }
                        PlayEngine.Enemy = true;
                    };
                    #endregion
                    n.Move = false;
                    n.Affix = "Жена лорда";
                    return n;
                }
            }

            public static MechEngine.NPC Rector
            {
                get
                {
                    MechEngine.NPC n = new MechEngine.NPC();
                    n.Icon = 'R';
                    n.Color = ConsoleColor.Green;
                    n.Name = "Дэнфорд";
                    n.Info = "Дэнфорт является ректором некрополя уже на протяжении 27 веков.";
                    n.Affix = "Ректор";
                    n.Move = true;
                    n.Script = () =>
                        {
                            PlayEngine.Enemy = false;
                            #region var
                            bool have = false;
                            bool end = false;
                            #endregion                            
                            #region quest
                            if (Rogue.RAM.Flags.UndeadRising != 0) { have = true; }
                            if (Rogue.RAM.Flags.UndeadRising == 4) { end = true; }
                            #endregion
                            #region replica
                            DrawEngine.Replica R = new DrawEngine.Replica();

                            if (!have)
                            {
                                R.Text = "Приветсссствую тебя. Меня зовут Дэнфорд, я ректор некрополя.";
                                R.TextColor = ConsoleColor.DarkCyan;
                                R.Options.Add("[Q] - Помощь?");
                                R.Options.Add("[I] - Чума.");
                                R.Options.Add("[Escape] - Уйти.");
                                R.OptionsColor = ConsoleColor.DarkCyan;
                                DrawEngine.ActiveObjectDraw.Draw(R, n);
                            }
                            else
                            {
                                if (end)
                                {
                                    R.Text = "Великолепно! Ты очень помог мне в исссследованиях! Есссли тебе нужно бесссплатное обращщщение в варлока, сссскажи мне!";
                                    R.TextColor = ConsoleColor.DarkCyan;
                                    R.Options.Add("[J] - Преврашение.");
                                    R.Options.Add("[I] - Чума.");
                                    R.Options.Add("[Escape] - Уйти.");
                                    R.OptionsColor = ConsoleColor.DarkCyan;
                                    DrawEngine.ActiveObjectDraw.Draw(R, n);
                                }
                                else
                                {
                                    R.Text = "Очень хорошшшо. Теперь мы можем приссступить к сследующщему шшшагу!";
                                    R.TextColor = ConsoleColor.DarkCyan;
                                    R.Options.Add("[W] - Согласен.");
                                    R.Options.Add("[I] - Чума.");
                                    R.Options.Add("[Escape] - Уйти.");
                                    R.OptionsColor = ConsoleColor.DarkCyan;
                                    DrawEngine.ActiveObjectDraw.Draw(R, n);
                                }
                            }
                            #endregion
                            #region main cycle
                            bool EndDialogue = false;
                            while (!EndDialogue)
                            {
                                ConsoleKey push = Console.ReadKey(true).Key;
                                switch (push)
                                {
                                    case ConsoleKey.Q:
                                        {
                                            if (!have)
                                            {
                                                R.Text = "Да... Да. Да! Это то что нужно! Тебе же будет не ссссложно? Думаю нет.";
                                                DrawEngine.ActiveObjectDraw.Draw(R, n);
                                                DrawEngine.InfoWindow.Warning = "Нажмите любую клавишу для продолжения...";
                                                Console.ReadKey(true);
                                                R.Text = "Видишь ли, мы уже долго разрабатываем новую чуму для того что бы вернуть сссвои болота...";
                                                DrawEngine.ActiveObjectDraw.Draw(R, n);
                                                DrawEngine.InfoWindow.Message = "Нажмите любую клавишу для продолжения...";
                                                Console.ReadKey(true);
                                                R.Text = "И ссссейчассс мы отобрали нужное нам количессство подопытных! Мне нужно что бы ты их УБИЛ!";
                                                DrawEngine.ActiveObjectDraw.Draw(R, n);
                                                DrawEngine.InfoWindow.Warning = "Нажмите любую клавишу для продолжения...";
                                                Console.ReadKey(true);
                                                R.Text = "Ты ссссогласееен? Я перенесссу тебя туда!";
                                                DrawEngine.ActiveObjectDraw.Draw(R, n);
                                                DrawEngine.InfoWindow.Warning = "Вы готовы принять задание? Y/N?";
                                                ConsoleKey psh = Console.ReadKey(true).Key;
                                                if (psh == ConsoleKey.Y)
                                                {
                                                    var quest = DataBase.QuestBase.NPCQuestFEC.UndeadRisingOne;
                                                    Rogue.RAM.Player.QuestBook.Add(quest);
                                                    List<DrawEngine.ColoredWord> cq = new List<DrawEngine.ColoredWord>();
                                                    cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = "Вы получаете новое задание - " });
                                                    cq.Add(new DrawEngine.ColoredWord() { Color = quest.Color, Word = quest.Name });
                                                    cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = " !" });
                                                    DrawEngine.InfoWindow.cMessage = cq;
                                                    SystemEngine.Helper.Information.Point p = SystemEngine.Helper.Information.FindPlayer(Rogue.RAM.Player);
                                                    Rogue.RAM.Map.Map[p.x][p.y].Player = null;
                                                    DrawEngine.winAPIDraw.Helpers.ReDrawOneChar((short)p.x, (short)p.y, (short)p.x, (short)p.y, ' ', ' ', 0, 0);
                                                    Rogue.RAM.Map.Map[69][21].Player = Rogue.RAM.Player;
                                                    DrawEngine.winAPIDraw.Helpers.ReDrawOneChar(69, 21, 69, 21, '@', '@', Convert.ToInt16(Rogue.RAM.Player.Color), Convert.ToInt16(Rogue.RAM.Player.Color));
                                                    EndDialogue = true;
                                                }
                                                else
                                                {
                                                    R.Text = "Хорошшшшшшшшо. Кккакк ппожжжеллаеешшшь.";
                                                    DrawEngine.ActiveObjectDraw.Draw(R, n);
                                                    Console.ReadKey(true);
                                                    EndDialogue = true;
                                                }
                                            }
                                            break;
                                        }
                                    case ConsoleKey.I:
                                        {
                                            R.Text = "Я разрабатываю чуму нового поколения, возможно она поможет нам вернуть наши болота. Что-то ещё?";
                                            DrawEngine.ActiveObjectDraw.Draw(R, n);
                                            break;
                                        }
                                    case ConsoleKey.W:
                                        {
                                            if (have && !end)
                                            {
                                                var quest = DataBase.QuestBase.NPCQuestFEC.UndeadRisingOne;
                                                switch (Rogue.RAM.Flags.UndeadRising)
                                                {
                                                    case 1: { quest = DataBase.QuestBase.NPCQuestFEC.UndeadRisingTwo; break; }
                                                    case 2: { quest = DataBase.QuestBase.NPCQuestFEC.UndeadRisingThree; break; }
                                                    case 3: { quest = DataBase.QuestBase.NPCQuestFEC.UndeadRisingFour; break; }
                                                }
                                                Rogue.RAM.Player.QuestBook.Add(quest);
                                                List<DrawEngine.ColoredWord> cq = new List<DrawEngine.ColoredWord>();
                                                cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = "Вы получаете новое задание - " });
                                                cq.Add(new DrawEngine.ColoredWord() { Color = quest.Color, Word = quest.Name });
                                                cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = " !" });
                                                DrawEngine.InfoWindow.cMessage = cq;
                                                SystemEngine.Helper.Information.Point p = SystemEngine.Helper.Information.FindPlayer(Rogue.RAM.Player);
                                                Rogue.RAM.Map.Map[p.x][p.y].Player = null;
                                                DrawEngine.winAPIDraw.Helpers.ReDrawOneChar((short)p.x, (short)p.y, (short)p.x, (short)p.y, ' ', ' ', 0, 0);
                                                Rogue.RAM.Map.Map[69][21].Player = Rogue.RAM.Player;
                                                DrawEngine.winAPIDraw.Helpers.ReDrawOneChar(69, 21, 69, 21, '@', '@', Convert.ToInt16(Rogue.RAM.Player.Color), Convert.ToInt16(Rogue.RAM.Player.Color));
                                                EndDialogue = true;
                                            }
                                            break;
                                        }
                                    case ConsoleKey.J:
                                        {
                                            if (have && end && Rogue.RAM.Player.Class!= MechEngine.BattleClass.Warlock)
                                            {
                                                R.Text = "Я с радостью превращю тебя в варлока! Ты готов?";
                                                DrawEngine.ActiveObjectDraw.Draw(R, n);
                                                DrawEngine.InfoWindow.Warning = "Вы готовы к превращению? Y/N?";
                                                ConsoleKey psh = Console.ReadKey(true).Key;
                                                if (psh == ConsoleKey.Y)
                                                {
                                                    Rogue.RAM.Player.Color = ConsoleColor.DarkRed;
                                                    MechEngine.Perk.AddPerk(new string[] { "AP + 10" }, "Мертвячина", "Вы стали могущественным.", 'W', ConsoleColor.Red);
                                                    Rogue.RAM.Player.AddPerk(new MechEngine.Perk() { AP = 10, Name = "Король-Мертвячина", History = "Вы стали могущественным варлоком.", Icon = 'W', Color = ConsoleColor.Red });
                                                    Rogue.RAM.Player.Class = MechEngine.BattleClass.Warlock;
                                                    Rogue.RAM.Player.MMP = (Rogue.RAM.Player.Level * 10) + 100;
                                                    Rogue.RAM.Player.CMP = (Rogue.RAM.Player.Level * 10) + 100;
                                                    Rogue.RAM.Player.Ability = EliteAbilityBase.Warlock;
                                                    //Rogue.RAM.Player.RecountPerks();
                                                    List<DrawEngine.ColoredWord> cww = new List<DrawEngine.ColoredWord>();
                                                    cww.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = "Ваш класс персонажа изменён на " });
                                                    cww.Add(new DrawEngine.ColoredWord() { Color = ConsoleColor.Red, Word = "Варлок" });
                                                    cww.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = " !" });
                                                    DrawEngine.InfoWindow.cMessage = cww;                                                    
                                                    DrawEngine.GUIDraw.ReDrawCharStat();
                                                    EndDialogue = true;
                                                }
                                                else
                                                {
                                                    R.Text = "Очень жаль. Может быть в другой раз...";
                                                    DrawEngine.ActiveObjectDraw.Draw(R, n);
                                                }
                                            }
                                            break;
                                        }
                                    case ConsoleKey.Escape:
                                        {
                                            EndDialogue = true;
                                            break;
                                        }
                                }
                            }
                            #endregion
                            PlayEngine.Enemy = true;
                        };
                    return n;
                }
            }

            public static MechEngine.NPC Wilyam
            {
                get
                {
                    MechEngine.NPC n = new MechEngine.NPC();
                    n.Icon = 'W';
                    n.Color = ConsoleColor.Green;
                    n.Name = "Уильям";
                    n.Info = "Уильям получил своё звание за впечатляющие заслуги в бою.";
                    #region Script
                    n.Script = () =>
                    {
                        PlayEngine.Enemy = false;
                        #region var
                        bool have = false;
                        bool end=false;
                        int index = -1;
                        #endregion
                        #region questinfo
                        foreach (MechEngine.Quest q in Rogue.RAM.Player.QuestBook)
                        {
                            if (q.Name == "Предотвратить")
                            {
                                index = Rogue.RAM.Player.QuestBook.IndexOf(q);
                                have = true;
                                if (q.Progress == q.TargetCount) { end = true; }
                            }
                        }
                        #endregion
                        #region replica
                        DrawEngine.Replica R = new DrawEngine.Replica();

                        if (!have)
                        {
                            R.Text = "Здравствуй! Надеюсь ты ищешь работку? У меня как раз есть!";
                            R.TextColor = ConsoleColor.DarkCyan;
                            R.Options.Add("[F] - Подробнее?");
                            R.Options.Add("[Escape] - Уйти.");
                            R.OptionsColor = ConsoleColor.DarkCyan;
                            DrawEngine.ActiveObjectDraw.Draw(R, n);
                        }
                        else
                        {
                            if (end)
                            {
                                R.Text = "Я чувствую что ты пришёл с победой, это так?";
                                R.TextColor = ConsoleColor.DarkCyan;
                                R.Options.Add("[W] - Да.");
                                R.Options.Add("[Escape] - Уйти.");
                                R.OptionsColor = ConsoleColor.DarkCyan;
                                DrawEngine.ActiveObjectDraw.Draw(R, n);
                            }
                            else
                            {
                                R.Text = "Ты ещё не уничтожил опасность которая нам грозит!";
                                R.TextColor = ConsoleColor.DarkCyan;
                                R.Options.Add("[Escape] - Уйти.");
                                R.OptionsColor = ConsoleColor.DarkCyan;
                                DrawEngine.ActiveObjectDraw.Draw(R, n);
                            }
                        }
                        #endregion
                        #region main cycle
                        bool EndDialogue = false;
                        while (!EndDialogue)
                        {
                            ConsoleKey push = Console.ReadKey(true).Key;
                            switch (push)
                            {
                                case ConsoleKey.F:
                                    {
                                        if (!have)
                                        {
                                            var quest = DataBase.QuestBase.NPCQuestFEC.KillBoss;
                                            R.Text = "Хорошо. Твоя задача на этот раз - убить <" + quest.Name + "> !";
                                            Rogue.RAM.Player.QuestBook.Add(quest);
                                            List<DrawEngine.ColoredWord> cq = new List<DrawEngine.ColoredWord>();
                                            cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = "Вы получаете новое задание - " });
                                            cq.Add(new DrawEngine.ColoredWord() { Color = quest.Color, Word = quest.Name });
                                            cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = " !" });
                                            DrawEngine.InfoWindow.cMessage = cq;
                                            DrawEngine.ActiveObjectDraw.Draw(R, n);
                                            Console.ReadKey(true);
                                            EndDialogue = true;
                                        }
                                        break;
                                    }
                                case ConsoleKey.W:
                                    {
                                        if (end)
                                        {
                                            var quest = Rogue.RAM.Player.QuestBook[index];
                                            List<DrawEngine.ColoredWord> cq = new List<DrawEngine.ColoredWord>();
                                            cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = "Вы выполнили задание - " });
                                            cq.Add(new DrawEngine.ColoredWord() { Color = quest.Color, Word = quest.Name });
                                            cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = " !" });
                                            DrawEngine.InfoWindow.cMessage = cq;
                                            Rogue.RAM.Player.QuestBook.Remove(Rogue.RAM.Player.QuestBook[index]);
                                            if (!Rogue.RAM.Flags.Retronslate)
                                            {
                                                R.Text = "Я думаю тебе понадобится вот эта вещь - <" + ResourseBase.RetronslateV.Name + ">, забирай.";
                                                if (Rogue.RAM.Player.InventorySlots)
                                                {
                                                    Rogue.RAM.Player.Inventory.Add(ResourseBase.RetronslateV);
                                                    Rogue.RAM.Flags.Retronslate = true;
                                                }
                                                else
                                                {
                                                    if (Rogue.RAM.SelfChest.Count != 7)
                                                    {
                                                        Rogue.RAM.SelfChest.Add(ResourseBase.RetronslateV);
                                                        Rogue.RAM.Flags.Retronslate = true;
                                                        DrawEngine.InfoWindow.Warning = "В вашем инвентаре нет места, поэтому награда перемещена в личный сундук.";
                                                    }
                                                    else
                                                    {
                                                        DrawEngine.InfoWindow.cMessage = new List<DrawEngine.ColoredWord>() { new DrawEngine.ColoredWord() { Color = ConsoleColor.Red, Word = "Ваш инвентарь и личный сундук забиты, возвращайтесь когда сможете забрать награду!" } };                                                        
                                                    }
                                                }
                                                DrawEngine.ActiveObjectDraw.Draw(R, n);                                                
                                            }
                                            else
                                            {
                                                R.Text = "На этот раз твоя награда составила: " + Rogue.RAM.Player.Level * 100;
                                                Rogue.RAM.Player.Gold += Rogue.RAM.Player.Level * 100;
                                                DrawEngine.ActiveObjectDraw.Draw(R, n);
                                            }
                                            DrawEngine.GUIDraw.ReDrawCharStat();
                                            DrawEngine.GUIDraw.ReDrawCharInventory();
                                            Console.ReadKey(true);
                                            EndDialogue = true;
                                        }
                                        break;
                                    }
                                case ConsoleKey.Escape:
                                    {
                                        EndDialogue = true;
                                        break;
                                    }
                            }
                        }
                        #endregion
                        PlayEngine.Enemy = true;
                    };
                    #endregion
                    n.Move = true;
                    n.Affix = "Мертвая рука";
                    return n;
                }
            }
            /// <summary>
            /// Scrolls of Hometown
            /// </summary>
            public static MechEngine.Merchant Biblio
            {
                get
                {
                    MechEngine.Merchant m = new MechEngine.Merchant();
                    m.Icon = 'B';
                    m.MaxGold = 1000;
                    m.Color = ConsoleColor.Green;
                    ;
                    m.SpeachColor = ConsoleColor.Green;
                    m.SpeachIcon = '"';
                    m.CurGold = r.Next(500);
                    m.Name = "Мертвый Библиотекарь";
                    m.Goods = new List<MechEngine.Item>() 
                    { 
                        ItemBase.ScrollOfMraumir,                        
                        ItemBase.ScrollOfMraumir,
                        ItemBase.ScrollOfMraumir,
                        ItemBase.ScrollOfMraumir,
                        ItemBase.ScrollOfMraumir,
                        ItemBase.ScrollOfMraumir,
                        ItemBase.ScrollOfMraumir,
                        ItemBase.ScrollOfMraumir,
                        ItemBase.ScrollOfMraumir,
                        ItemBase.ScrollOfMraumir,
                        ItemBase.ScrollOfMraumir,
                        ItemBase.ScrollOfMraumir
                    };
                    return m;
                }
            }

            public static MechEngine.NPC BiblioHelper
            {
                get
                {
                    MechEngine.NPC n = new MechEngine.NPC();
                    n.Icon = 'H';
                    n.Color = ConsoleColor.Green;
                    n.Name ="Фарт";
                    n.Info = "Обычный зомби созданный из плохих материалов...";
                    n.Affix = "Помощник";
                    n.Script += delegate()
                    {
                        if (PlayEngine.EnemyMoves.CheckMove) { PlayEngine.EnemyMoves.Move(false); }
                        string info = n.Info;
                        n.Info = "Да? Ты не мой хозяйн!";
                        DrawEngine.InfoDraw.NPC = n;
                        Console.ReadKey(true);
                        n.Info = info;
                        if (!PlayEngine.EnemyMoves.CheckMove) { PlayEngine.EnemyMoves.Move(true); }
                    };
                    return n;
                }
            }

            public static MechEngine.NPC Klara
            {
                get
                {
                    MechEngine.NPC n = new MechEngine.NPC();
                    n.Icon = 'K';
                    n.Color = ConsoleColor.Green;
                    n.Name = "Клара";
                    n.Affix = "Мертвая гувернантка";
                    #region Script
                    n.Script = () =>
                    {                        
                        PlayEngine.Enemy = false;
                        DrawEngine.Replica R = new DrawEngine.Replica();
                        R.Text = "Здравствуй, живой. Я сильно занята так что наш разговор будет коротким.";
                        R.TextColor = ConsoleColor.DarkYellow;                        
                        if (!Rogue.RAM.Flags.Klara) { R.Options.Add("[Y] - Помочь?"); }
                        else if (!Rogue.RAM.Flags.KlaraHelp) { R.Options.Add("[N] - Сделано."); }
                        R.Options.Add("[Escape] - Уйти.");
                        R.OptionsColor = ConsoleColor.DarkYellow;
                        DrawEngine.ActiveObjectDraw.Draw(R, n);

                        bool EndDialogue = false;
                        while (!EndDialogue)
                        {
                            ConsoleKey push = Console.ReadKey(true).Key;
                            switch (push)
                            {
                                case ConsoleKey.Y:
                                    {
                                        if (!Rogue.RAM.Flags.Klara)
                                        {
                                            R.Text = "А разве живые могут помогать мертвым? В любом случае, если тебе будет не трудно, собери весь мусор в замке и принеси мне.";
                                            R.Options.Remove("[Y] - Помочь?");
                                            DrawEngine.ActiveObjectDraw.Draw(R, n);
                                            Rogue.RAM.Map.Map[17][10].Item = ResourseBase.Trash;
                                            DrawEngine.winAPIDraw.Helpers.ReDrawOneChar(17, 10, 17, 10, '\0', ResourseBase.Trash.Icon(), 0, Convert.ToInt16(ResourseBase.Trash.Color));
                                            Rogue.RAM.Map.Map[12][5].Item = ResourseBase.Trash;
                                            DrawEngine.winAPIDraw.Helpers.ReDrawOneChar(12, 5, 12, 5, '\0', ResourseBase.Trash.Icon(), 0, Convert.ToInt16(ResourseBase.Trash.Color));
                                            Rogue.RAM.Map.Map[12][16].Item = ResourseBase.Trash;
                                            DrawEngine.winAPIDraw.Helpers.ReDrawOneChar(12, 16, 12, 16, '\0', ResourseBase.Trash.Icon(), 0, Convert.ToInt16(ResourseBase.Trash.Color));
                                            Rogue.RAM.Map.Map[23][3].Item = ResourseBase.Trash;
                                            DrawEngine.winAPIDraw.Helpers.ReDrawOneChar(23, 3, 23, 3, '\0', ResourseBase.Trash.Icon(), 0, Convert.ToInt16(ResourseBase.Trash.Color));
                                            Rogue.RAM.Map.Map[14][16].Item = ResourseBase.Trash;
                                            DrawEngine.winAPIDraw.Helpers.ReDrawOneChar(14, 16, 14, 16, '\0', ResourseBase.Trash.Icon(), 0, Convert.ToInt16(ResourseBase.Trash.Color));
                                            Rogue.RAM.Map.Map[27][11].Item = ResourseBase.Trash;
                                            DrawEngine.winAPIDraw.Helpers.ReDrawOneChar(27, 11, 27, 11, '\0', ResourseBase.Trash.Icon(), 0, Convert.ToInt16(ResourseBase.Trash.Color));
                                            Rogue.RAM.Flags.Klara = true;
                                            Rogue.RAM.Player.QuestBook.Add(QuestBase.NPCQuestFEC.DeleteTrash);
                                            List<DrawEngine.ColoredWord> cq = new List<DrawEngine.ColoredWord>();
                                            cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = "Вы получаете новое задание - " });
                                            cq.Add(new DrawEngine.ColoredWord() { Color = QuestBase.NPCQuestFEC.DeleteTrash.Color, Word = QuestBase.NPCQuestFEC.DeleteTrash.Name });
                                            cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = " !" });
                                            DrawEngine.InfoWindow.cMessage = cq;
                                            
                                        }
                                        break;
                                    }
                                case ConsoleKey.N:
                                    {
                                        if (Rogue.RAM.Flags.Klara && !Rogue.RAM.Flags.KlaraHelp)
                                        {
                                            bool fuck = false;

                                            if (Rogue.RAM.Map.Map[17][10].Item == null && Rogue.RAM.Map.Map[12][5].Item == null && Rogue.RAM.Map.Map[12][16].Item == null &&
                                                Rogue.RAM.Map.Map[23][3].Item == null && Rogue.RAM.Map.Map[14][16].Item == null && Rogue.RAM.Map.Map[27][11].Item == null)
                                            {
                                                fuck = true;
                                            }
                                            if (fuck)
                                            {
                                                int index = 0;
                                                List<int> indexes = new List<int>();
                                                foreach (MechEngine.Item it in Rogue.RAM.Player.Inventory)
                                                { if (it.Name == ResourseBase.Trash.Name) { indexes.Add(Rogue.RAM.Player.Inventory.IndexOf(it)); } }
                                                int delited = 0;
                                                foreach (int i in indexes)
                                                { Rogue.RAM.Player.Inventory.Remove(Rogue.RAM.Player.Inventory[i - delited]); delited++; }
                                                foreach (MechEngine.Quest q in Rogue.RAM.Player.QuestBook)
                                                { if (q.Name == QuestBase.NPCQuestFEC.DeleteTrash.Name) { index = Rogue.RAM.Player.QuestBook.IndexOf(q); } }
                                                Rogue.RAM.Player.QuestBook.Remove(Rogue.RAM.Player.QuestBook[index]);

                                                List<DrawEngine.ColoredWord> cq = new List<DrawEngine.ColoredWord>();
                                                cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = "Вы выполнили задание - " });
                                                cq.Add(new DrawEngine.ColoredWord() { Color = QuestBase.NPCQuestFEC.DeleteTrash.Color, Word = QuestBase.NPCQuestFEC.DeleteTrash.Name });
                                                cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = " !" });
                                                DrawEngine.InfoWindow.cMessage = cq;
                                                Thread.Sleep(410);
                                                Rogue.RAM.Flags.KlaraHelp = true;
                                                R.Options.Remove("[N] - Сделано.");
                                                R.Text = "Спасибо тебе за помощь!";
                                                DrawEngine.ActiveObjectDraw.Draw(R, n);
                                                foreach (MechEngine.Reputation rep in Rogue.RAM.Player.Repute)
                                                {
                                                    if (rep.min < rep.max)
                                                    {
                                                        if (rep.name == "Мертвые")
                                                        {
                                                            if ((rep.min + 10) > rep.max) { rep.min = rep.max; }
                                                            else { rep.min += 400; DrawEngine.InfoWindow.Message = "Вы увеличиваете репутацию с фракцией " + rep.name + " на 400!"; }
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                R.Text = "Ты ещё не убрал весь мусор в замке!";
                                                DrawEngine.ActiveObjectDraw.Draw(R, n);
                                            }
                                        }
                                        break;
                                    }
                                case ConsoleKey.Escape:
                                    {
                                        EndDialogue = true;
                                        break;
                                    }
                            }
                        }
                        PlayEngine.Enemy = true;
                    };
                    #endregion
                    return n;
                }
            }

            public static MechEngine.NPC Sara
            {
                get
                {
                    MechEngine.NPC n = new MechEngine.NPC();
                    n.Icon = 'S';
                    n.Color = ConsoleColor.Green;
                    n.Name = "Сара";
                    n.Affix = "Мертвая управляющая";
                    #region Script
                    n.Script = () =>
                    {
                        PlayEngine.Enemy = false;
                        DrawEngine.Replica R = new DrawEngine.Replica();
                        R.Text = "Здравствуй, живой. У меня здесь небольшая проблема, не мог бы ты мне помочь?";
                        R.TextColor = ConsoleColor.DarkYellow;
                        if (!Rogue.RAM.Flags.Sara) { R.Options.Add("[Y] - Конечно!"); }
                        else if (!Rogue.RAM.Flags.SaraHelp) { R.Options.Add("[N] - Сделано!"); }
                        R.Options.Add("[Escape] - Уйти.");
                        R.OptionsColor = ConsoleColor.DarkYellow;
                        DrawEngine.ActiveObjectDraw.Draw(R, n);

                        bool EndDialogue = false;
                        while (!EndDialogue)
                        {
                            ConsoleKey push = Console.ReadKey(true).Key;
                            switch (push)
                            {
                                case ConsoleKey.Y:
                                    {
                                        if (!Rogue.RAM.Flags.Sara)
                                        {
                                            R.Text = "Прямо рядом со входом в коридор Ректора есть дыра в стене. Пожалуйста заделай её, вот тебе инструменты.";
                                            R.Options.Remove("[Y] - Конечно!");
                                            DrawEngine.ActiveObjectDraw.Draw(R, n);

                                            Rogue.RAM.Map.Map[9][11].Object = ObjectBase.MouseDoor;
                                            DrawEngine.winAPIDraw.Helpers.ReDrawOneChar(9, 11, 9, 11, '\0', ObjectBase.MouseDoor.Icon, 0, Convert.ToInt16(ObjectBase.MouseDoor.Color));

                                            Rogue.RAM.Flags.Sara = true;
                                            Rogue.RAM.Player.QuestBook.Add(QuestBase.NPCQuestFEC.MouseDoor);
                                            List<DrawEngine.ColoredWord> cq = new List<DrawEngine.ColoredWord>();
                                            cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = "Вы получаете новое задание - " });
                                            cq.Add(new DrawEngine.ColoredWord() { Color = QuestBase.NPCQuestFEC.MouseDoor.Color, Word = QuestBase.NPCQuestFEC.MouseDoor.Name });
                                            cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = " !" });
                                            DrawEngine.InfoWindow.cMessage = cq;

                                        }
                                        break;
                                    }
                                case ConsoleKey.N:
                                    {
                                        if (Rogue.RAM.Flags.Sara && !Rogue.RAM.Flags.SaraHelp)
                                        {
                                            bool fuck = false;

                                            if (Rogue.RAM.Map.Map[9][11].Object==null)
                                            {
                                                fuck = true;
                                            }
                                            if (fuck)
                                            {
                                                int index = 0;
                                                
                                                foreach (MechEngine.Quest q in Rogue.RAM.Player.QuestBook)
                                                { if (q.Name == QuestBase.NPCQuestFEC.MouseDoor.Name) { index = Rogue.RAM.Player.QuestBook.IndexOf(q); } }
                                                Rogue.RAM.Player.QuestBook.Remove(Rogue.RAM.Player.QuestBook[index]);

                                                List<DrawEngine.ColoredWord> cq = new List<DrawEngine.ColoredWord>();
                                                cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = "Вы выполнили задание - " });
                                                cq.Add(new DrawEngine.ColoredWord() { Color = QuestBase.NPCQuestFEC.MouseDoor.Color, Word = QuestBase.NPCQuestFEC.MouseDoor.Name });
                                                cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = " !" });
                                                DrawEngine.InfoWindow.cMessage = cq;
                                                Thread.Sleep(410);
                                                Rogue.RAM.Flags.SaraHelp = true;
                                                R.Options.Remove("[N] - Сделано.");
                                                R.Text = "Спасибо тебе за помощь!";
                                                DrawEngine.ActiveObjectDraw.Draw(R, n);
                                                foreach (MechEngine.Reputation rep in Rogue.RAM.Player.Repute)
                                                {
                                                    if (rep.min < rep.max)
                                                    {
                                                        if (rep.name == "Мертвые")
                                                        {
                                                            if ((rep.min + 10) > rep.max) { rep.min = rep.max; }
                                                            else { rep.min += 400; DrawEngine.InfoWindow.Message = "Вы увеличиваете репутацию с фракцией " + rep.name + " на 400!"; }
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                R.Text = "Заделай пожалуйста ту огромную дыру.";
                                                DrawEngine.ActiveObjectDraw.Draw(R, n);
                                            }
                                        }
                                        break;
                                    }
                                case ConsoleKey.Escape:
                                    {
                                        EndDialogue = true;
                                        break;
                                    }
                            }
                        }
                        PlayEngine.Enemy = true;
                    };
                    #endregion
                    return n;
                }
            }
            #region MAY BE SOON
            //public static MechEngine.NPC GoblinKing
            //{
            //    get
            //    {
            //        MechEngine.NPC n = new MechEngine.NPC();
            //        n.Icon = 'G';
            //        n.Color = ConsoleColor.DarkGreen;
            //        n.Name = "Карл";
            //        n.Affix = "Король гоблинов";
            //        return n;
            //    }
            //}

            //public static MechEngine.NPC TrollKing
            //{
            //    get
            //    {
            //        MechEngine.NPC n = new MechEngine.NPC();
            //        n.Icon = 'T';
            //        n.Color = ConsoleColor.Cyan;
            //        n.Name = "Шотус";
            //        n.Affix = "Король троллей";
            //        return n;
            //    }
            //}

            //public static MechEngine.NPC StoneOrcKing
            //{
            //    get
            //    {
            //        MechEngine.NPC n = new MechEngine.NPC();
            //        n.Icon = 'K';
            //        n.Color = ConsoleColor.DarkCyan;
            //        n.Name = "Гробтуд";
            //        n.Affix = "Король кам. орков";
            //        return n;
            //    }
            //}

            //public static MechEngine.NPC ZetKing
            //{
            //    get
            //    {
            //        MechEngine.NPC n = new MechEngine.NPC();
            //        n.Icon = 'Z';
            //        n.Color = ConsoleColor.DarkGreen;
            //        n.Name = "Зул";
            //        n.Affix = "Старый правитель";
            //        return n;
            //    }
            //}

            //public static MechEngine.NPC OrcKing
            //{
            //    get
            //    {
            //        MechEngine.NPC n = new MechEngine.NPC();
            //        n.Icon = 'O';
            //        n.Color = ConsoleColor.DarkGreen;
            //        n.Name = "Грег-Хэк";
            //        n.Affix = "Правитель орков";
            //        return n;
            //    }
            //}

            //public static MechEngine.NPC OrcShaman
            //{
            //    get
            //    {
            //        MechEngine.NPC n = new MechEngine.NPC();
            //        n.Icon = 'S';
            //        n.Color = ConsoleColor.DarkGreen;
            //        n.Name = "Спек-Грог";
            //        n.Affix = "Шаман-Советник";
            //        return n;
            //    }
            //}

            //public static MechEngine.NPC OrcMage
            //{
            //    get
            //    {
            //        MechEngine.NPC n = new MechEngine.NPC();
            //        n.Icon = 'M';
            //        n.Color = ConsoleColor.DarkGreen;
            //        n.Name = "Нер-Зуль";
            //        n.Affix = "Маг-Советник";
            //        return n;
            //    }
            //}

            //public static MechEngine.NPC OrcRogue
            //{
            //    get
            //    {
            //        MechEngine.NPC n = new MechEngine.NPC();
            //        n.Icon = 'R';
            //        n.Color = ConsoleColor.DarkGreen;
            //        n.Name = "Рог-Хат";
            //        n.Affix = "Разбойник-Советник";
            //        return n;
            //    }
            //}

            //public static MechEngine.NPC OrcWar
            //{
            //    get
            //    {
            //        MechEngine.NPC n = new MechEngine.NPC();
            //        n.Icon = 'W';
            //        n.Color = ConsoleColor.DarkGreen;
            //        n.Name = "Дрод-Рог";
            //        n.Affix = "Воин-Советник";
            //        return n;
            //    }
            //}

            //public static MechEngine.NPC Hostes
            //{
            //    get
            //    {
            //        MechEngine.NPC n = new MechEngine.NPC();
            //        n.Icon = 'H';
            //        n.Color = ConsoleColor.DarkGray;
            //        n.Name = "Прислуга";
            //        n.Affix = "Рабыня";
            //        return n;
            //    }
            //}

            //public static MechEngine.NPC LordMagnus
            //{
            //    get
            //    {
            //        MechEngine.NPC n = new MechEngine.NPC();
            //        n.Icon = 'L';
            //        n.Color = ConsoleColor.Cyan;
            //        n.Name = "Магнус";
            //        n.Affix = "Лорд";
            //        return n;
            //    }
            //}

            //public static MechEngine.NPC LordServant
            //{
            //    get
            //    {
            //        MechEngine.NPC n = new MechEngine.NPC();
            //        n.Icon = 'L';
            //        n.Color = ConsoleColor.Cyan;
            //        n.Name = "Серван";
            //        n.Affix = "Лорд";
            //        return n;
            //    }
            //}

            //public static MechEngine.NPC LordPraud
            //{
            //    get
            //    {
            //        MechEngine.NPC n = new MechEngine.NPC();
            //        n.Icon = 'L';
            //        n.Color = ConsoleColor.Cyan;
            //        n.Name = "Прауд";
            //        n.Affix = "Лорд";
            //        return n;
            //    }
            //}

            //public static MechEngine.NPC LordKing
            //{
            //    get
            //    {
            //        MechEngine.NPC n = new MechEngine.NPC();
            //        n.Icon = 'L';
            //        n.Color = ConsoleColor.Cyan;
            //        n.Name = "Корол";
            //        n.Affix = "Лорд";
            //        return n;
            //    }
            //}

            //public static MechEngine.NPC HumanGuardian
            //{
            //    get
            //    {
            //        MechEngine.NPC n = new MechEngine.NPC();
            //        n.Icon = 'G';
            //        n.Color = ConsoleColor.DarkCyan;
            //        n.Name = "Стражник";
            //        n.Move = false;
            //        n.Affix = "Королевский";
            //        return n;
            //    }
            //}
            #endregion
            public static MechEngine.NPC Nadin
            {
                get
                {
                    MechEngine.NPC n = new MechEngine.NPC();
                    n.Icon = 'K';
                    n.Color = ConsoleColor.DarkYellow;
                    n.Move = false;
                    n.Name = "Надин";
                    n.Affix = "Король";
                    return n;
                }
            }

            public static MechEngine.CapitalDoor.GateKeeper GateKeeperMage
            {
                get
                {
                    MechEngine.CapitalDoor.GateKeeper n = new MechEngine.CapitalDoor.GateKeeper();
                    n.Icon = 'M';
                    n.Color = ConsoleColor.Magenta;
                    n.Move = false;
                    n.Name = "Мастер ворот";
                    return n;
                }
            }

            public static MechEngine.NPC Yellow
            {
                get
                {
                    MechEngine.NPC n = new MechEngine.NPC();
                    n.Icon = 'Y';
                    n.Color = ConsoleColor.DarkRed;
                    n.Name = "Юллоу";
                    n.Affix = "Маг крови";
                    n.Info = "Юллоу один из редких магов крови которого приняли в общество. Сейчас он продаёт магические предметы и убирает зачарования с предметов.";
                    #region Script
                    n.Script = () =>
                    {
                        PlayEngine.Enemy = false;

                        DrawEngine.Replica R = new DrawEngine.Replica();
                        R.Text = "Приветствую тебя, я продаю магические предметы и убираю зачарования. Чем тебе помочь?";
                        R.TextColor = ConsoleColor.DarkMagenta;
                        R.Options.Clear();
                        R.Options.Add("[M] - Торговать.");
                        R.Options.Add("[D] - Зачарование.");
                        R.Options.Add("[Escape] - Уйти.");
                        R.OptionsColor = ConsoleColor.DarkCyan;
                        DrawEngine.ActiveObjectDraw.Draw(R, n);

                        CheckGold checkBuy = (int i) =>
                        {
                            if (Rogue.RAM.Player.Gold >= i) { Rogue.RAM.Player.Gold -= i; DrawEngine.GUIDraw.ReDrawCharStat(); return true; }
                            else { DrawEngine.InfoWindow.Warning = "У вас недостаточно золота!"; return false; }
                        };
                        Enchanted checkEnch = (MechEngine.Item it) =>
                        {
                            if (it.Enchanted) { it.Name = it.Name.Remove(it.Name.Length - 2); it.Enchanted = false; return true; }
                            else { DrawEngine.InfoWindow.Warning = "Нельзя убрать зачарование с предмета который не зачарован!"; return false; }
                        };

                        bool EndDialogue = false;
                        while (!EndDialogue)
                        {
                            ConsoleKey push = Console.ReadKey(true).Key;
                            switch (push)
                            {
                                case ConsoleKey.M: { YellowMerch.Use(); EndDialogue = true; break; }
                                case ConsoleKey.D:
                                    {
                                        R.Text = "Работа стоит 5000 монет, выбери предмет:";
                                        R.Options.Clear();
                                        if (Rogue.RAM.Player.Equipment.Armor != null) { R.Options.Add("[A] - Броня"); }
                                        if (Rogue.RAM.Player.Equipment.Boots != null) { R.Options.Add("[B] - Обувь"); }
                                        if (Rogue.RAM.Player.Equipment.Helm != null) { R.Options.Add("[H] - Шлем"); }
                                        if (Rogue.RAM.Player.Equipment.OffHand != null) { R.Options.Add("[O] - Щит"); }
                                        if (Rogue.RAM.Player.Equipment.Weapon != null) { R.Options.Add("[W] - Оружие"); }
                                        R.Options.Add("[Escape] - Отмена.");
                                        DrawEngine.ActiveObjectDraw.Draw(R, n);
                                        int stats = Convert.ToInt32(Rogue.RAM.Player.Level / 2);
                                        if (stats <= 0) { stats = 1; }
                                        ConsoleKey psh = Console.ReadKey(true).Key;
                                        switch (psh)
                                        {
                                            case ConsoleKey.A:
                                                {
                                                    try
                                                    {
                                                        if (checkBuy(5000) && checkEnch(Rogue.RAM.Player.Equipment.Armor))
                                                        {                                                           
                                                            DrawEngine.InfoWindow.cMessage = new List<DrawEngine.ColoredWord>() { new DrawEngine.ColoredWord() { Color = ConsoleColor.Cyan, Word = "Вы убрали зачарование!" } };
                                                        }
                                                    }
                                                    catch { }
                                                    break;
                                                }
                                            case ConsoleKey.B:
                                                {
                                                    try
                                                    {
                                                        if (checkBuy(5000) && checkEnch(Rogue.RAM.Player.Equipment.Boots))
                                                        {
                                                            DrawEngine.InfoWindow.cMessage = new List<DrawEngine.ColoredWord>() { new DrawEngine.ColoredWord() { Color = ConsoleColor.Cyan, Word = "Вы убрали зачарование!" } };
                                                        }
                                                    }
                                                    catch { }
                                                    break;
                                                }
                                            case ConsoleKey.H:
                                                {
                                                    try
                                                    {
                                                        if (checkBuy(5000) && checkEnch(Rogue.RAM.Player.Equipment.Helm))
                                                        {
                                                            DrawEngine.InfoWindow.cMessage = new List<DrawEngine.ColoredWord>() { new DrawEngine.ColoredWord() { Color = ConsoleColor.Cyan, Word = "Вы убрали зачарование!" } };
                                                        }
                                                    }
                                                    catch { }
                                                    break;
                                                }
                                            case ConsoleKey.O:
                                                {
                                                    try
                                                    {
                                                        if (checkBuy(5000) && checkEnch(Rogue.RAM.Player.Equipment.OffHand))
                                                        {
                                                            DrawEngine.InfoWindow.cMessage = new List<DrawEngine.ColoredWord>() { new DrawEngine.ColoredWord() { Color = ConsoleColor.Cyan, Word = "Вы убрали зачарование!" } };
                                                        }
                                                    }
                                                    catch { }
                                                    break;
                                                }
                                            case ConsoleKey.W:
                                                {
                                                    try
                                                    {
                                                        if (checkBuy(5000) && checkEnch(Rogue.RAM.Player.Equipment.Weapon))
                                                        {
                                                            DrawEngine.InfoWindow.cMessage = new List<DrawEngine.ColoredWord>() { new DrawEngine.ColoredWord() { Color = ConsoleColor.Cyan, Word = "Вы убрали зачарование!" } };
                                                        }
                                                    }
                                                    catch { }
                                                    break;
                                                }
                                            case ConsoleKey.Escape:
                                                {
                                                    break;
                                                }
                                        }
                                        R.Text = "Приветствую тебя, я продаю магические предметы и убираю зачарования. Чем тебе помочь?";
                                        R.TextColor = ConsoleColor.DarkMagenta;
                                        R.Options.Clear();
                                        R.Options.Add("[M] - Торговать.");
                                        R.Options.Add("[D] - Зачарование.");
                                        R.Options.Add("[Escape] - Уйти.");
                                        R.OptionsColor = ConsoleColor.DarkCyan;
                                        DrawEngine.ActiveObjectDraw.Draw(R, n);
                                        DrawEngine.GUIDraw.ReDrawCharStat();
                                        break;
                                    }
                                case ConsoleKey.Escape: { EndDialogue = true; break; }
                            }
                        }

                        PlayEngine.Enemy = true;
                    };
                    #endregion
                    return n;
                }
            }
            public static MechEngine.Merchant YellowMerch
            {
                get
                {
                    MechEngine.Merchant m = new MechEngine.Merchant();
                    m.Icon = 'Y';
                    m.MaxGold = 10000;
                    m.Color = ConsoleColor.DarkRed;
                    m.SpeachColor = ConsoleColor.DarkRed;
                    m.SpeachIcon = 'Y';
                    m.CurGold = r.Next(5000);
                    m.Name = "Юллоу";
                    m.Goods = new List<MechEngine.Item>() 
                    { 
                        ItemBase.GetItem,                      
                        ItemBase.GetItem,
                        ItemBase.GetItem,
                        ItemBase.GetItem,
                        ItemBase.GetItem,
                        ItemBase.GetItem,
                        ItemBase.GetItem,
                        ItemBase.GetItem,
                        ItemBase.GetItem,
                        ItemBase.GetItem,
                        ItemBase.GetItem,
                        ItemBase.GetItem,
                    };
                    return m;
                }
            }

            public static MechEngine.NPC Nation
            {
                get
                {
                    MechEngine.NPC n = new MechEngine.NPC();
                    n.Icon = 'N';
                    n.Color = ConsoleColor.Blue;
                    n.Name = "Нэйтан";
                    n.Affix = "Маг трансформации";
                    n.Info = "Говорят что Нэйтан добился таких высот в магии трансформации, что теперь может трансформировать свою смерть в жизнь, а значит, он бессмертен.";
                    n.Script = () =>
                        {
                            #region var
                            bool have = false;
                            bool end = false;
                            #endregion
                            #region quest
                            if (Rogue.RAM.Flags.Scrijal) { have = true; }
                            if (Rogue.RAM.Flags.AllTransform) { end = true; }
                            #endregion
                            #region replica
                            DrawEngine.Replica R = new DrawEngine.Replica();

                            if (!have)
                            {
                                R.Text = "Привет. Я Нэйтан. У меня есть работка. Не хочешь помочь?";
                                R.TextColor = ConsoleColor.Cyan;
                                R.Options.Add("[Y] - С радостью!");
                                R.Options.Add("[Escape] - Уйти.");
                                R.OptionsColor = ConsoleColor.Cyan;
                                DrawEngine.ActiveObjectDraw.Draw(R, n);
                            }
                            else
                            {
                                if (end)
                                {
                                    if (Rogue.RAM.Player.Class == MechEngine.BattleClass.Assassin || Rogue.RAM.Player.Class == MechEngine.BattleClass.Monk)
                                    {
                                        R.Text = "Отлично! Если хочешь. Я могу сделать тебя. Молниеносным. Да?";
                                        R.TextColor = ConsoleColor.Cyan;
                                        R.Options.Add("[J] - Преврашение.");
                                        R.Options.Add("[Escape] - Уйти.");
                                        R.OptionsColor = ConsoleColor.Cyan;
                                        DrawEngine.ActiveObjectDraw.Draw(R, n);
                                    }
                                    else
                                    {
                                        R.Text = "Отлично! Теперь ты можешь использовать алтари трансформации!";
                                        R.TextColor = ConsoleColor.Cyan;
                                        R.Options.Add("[Escape] - Уйти.");
                                        R.OptionsColor = ConsoleColor.Cyan;
                                        DrawEngine.ActiveObjectDraw.Draw(R, n);
                                    }
                                }
                                else
                                {
                                    R.Text = "Привет. Пока трансформируешь 3 предмета. Я не могу ничего сделать.";
                                    R.TextColor = ConsoleColor.Cyan;
                                    R.Options.Add("[Escape] - Уйти.");
                                    R.OptionsColor = ConsoleColor.Cyan;
                                    DrawEngine.ActiveObjectDraw.Draw(R, n);
                                }
                            }
                            #endregion
                            #region main cycle
                            bool EndDialogue = false;
                            while (!EndDialogue)
                            {
                                ConsoleKey push = Console.ReadKey(true).Key;
                                switch (push)
                                {
                                    case ConsoleKey.Y:
                                        {
                                            if (!have)
                                            {
                                                R.Text = "Великолепно. Мне нужно трансформировать три вещи. Но сам я непойду в тот ад.";
                                                DrawEngine.ActiveObjectDraw.Draw(R, n);
                                                DrawEngine.InfoWindow.Warning = "Нажмите любую клавишу для продолжения...";
                                                Console.ReadKey(true);
                                                R.Text = "Поэтому. Прошу тебя это сделать. Так как ты не умеешь этого. Я помогу тебе.";
                                                DrawEngine.ActiveObjectDraw.Draw(R, n);
                                                DrawEngine.InfoWindow.Message = "Нажмите любую клавишу для продолжения...";
                                                Console.ReadKey(true);
                                                R.Text = "Вот. Держи эту скрижаль. Она поможет тебе. Просто найди алтарь трансформации.";
                                                DrawEngine.ActiveObjectDraw.Draw(R, n);
                                                DrawEngine.InfoWindow.Warning = "Нажмите любую клавишу для продолжения...";
                                                Console.ReadKey(true);
                                                R.Text = "Когда трансформируешь. 3 предмета. Отправь их мне. Через алтарь.";
                                                DrawEngine.ActiveObjectDraw.Draw(R, n);
                                                DrawEngine.InfoWindow.Message = "Нажмите любую клавишу для продолжения...";
                                                Console.ReadKey(true);
                                                DrawEngine.InfoWindow.Warning = "Вы готовы принять задание? Y/N?";
                                                ConsoleKey psh = Console.ReadKey(true).Key;
                                                if (psh == ConsoleKey.Y)
                                                {
                                                    var quest = DataBase.QuestBase.NPCQuestFEC.IronToDiamond;
                                                    Rogue.RAM.Player.QuestBook.Add(quest);
                                                    List<DrawEngine.ColoredWord> cq = new List<DrawEngine.ColoredWord>();
                                                    cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = "Вы получаете новое задание - " });
                                                    cq.Add(new DrawEngine.ColoredWord() { Color = quest.Color, Word = quest.Name });
                                                    cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = " !" });
                                                    DrawEngine.InfoWindow.cMessage = cq;

                                                    quest = DataBase.QuestBase.NPCQuestFEC.WoodToGlass;
                                                    Rogue.RAM.Player.QuestBook.Add(quest);
                                                    cq = new List<DrawEngine.ColoredWord>();
                                                    cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = "Вы получаете новое задание - " });
                                                    cq.Add(new DrawEngine.ColoredWord() { Color = quest.Color, Word = quest.Name });
                                                    cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = " !" });
                                                    DrawEngine.InfoWindow.cMessage = cq;

                                                    quest = DataBase.QuestBase.NPCQuestFEC.TransformAllToMage;
                                                    Rogue.RAM.Player.QuestBook.Add(quest);
                                                    cq = new List<DrawEngine.ColoredWord>();
                                                    cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = "Вы получаете новое задание - " });
                                                    cq.Add(new DrawEngine.ColoredWord() { Color = quest.Color, Word = quest.Name });
                                                    cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = " !" });
                                                    DrawEngine.InfoWindow.cMessage = cq;

                                                    quest = DataBase.QuestBase.NPCQuestFEC.DeathToLife;
                                                    Rogue.RAM.Player.QuestBook.Add(quest);
                                                    cq = new List<DrawEngine.ColoredWord>();
                                                    cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = "Вы получаете новое задание - " });
                                                    cq.Add(new DrawEngine.ColoredWord() { Color = quest.Color, Word = quest.Name });
                                                    cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = " !" });
                                                    DrawEngine.InfoWindow.cMessage = cq;
                                                    PlayEngine.GamePlay.DropItem(ResourseBase.ScrijalScroll);
                                                    Rogue.RAM.Flags.Scrijal = true;
                                                    EndDialogue = true;
                                                }
                                                else
                                                {
                                                    R.Text = "Ладно. Помоги мне если сможешь. Потом.";
                                                    DrawEngine.ActiveObjectDraw.Draw(R, n);
                                                    Console.ReadKey(true);
                                                    EndDialogue = true;
                                                }
                                            }
                                            break;
                                        }                                    
                                    case ConsoleKey.J:
                                        {
                                            if (have && end && Rogue.RAM.Player.Class != MechEngine.BattleClass.LightWarrior)
                                            {
                                                R.Text = "Ты готов. Быть пораженным. Молнией?";
                                                DrawEngine.ActiveObjectDraw.Draw(R, n);
                                                DrawEngine.InfoWindow.Warning = "Вы готовы к превращению? Y/N?";
                                                ConsoleKey psh = Console.ReadKey(true).Key;
                                                if (psh == ConsoleKey.Y)
                                                {
                                                    Rogue.RAM.Player.Color = ConsoleColor.DarkRed;
                                                    MechEngine.Perk.AddPerk(new string[] { "AD + 5" }, "Пораженный", "Вас разразил гром.", '¤', ConsoleColor.Cyan);
                                                    Rogue.RAM.Player.AddPerk(new MechEngine.Perk() { AD = 5, Name = "Пораженный", History = "Вас разразил гром.", Icon = '¤', Color = ConsoleColor.Cyan });
                                                    Rogue.RAM.Player.Class = MechEngine.BattleClass.LightWarrior;
                                                    Rogue.RAM.Player.MMP = (Rogue.RAM.Player.Level * 10) + 100;
                                                    Rogue.RAM.Player.CMP = (Rogue.RAM.Player.Level * 10) + 100;
                                                    Rogue.RAM.Player.Ability = EliteAbilityBase.LightWarrior;
                                                    //Rogue.RAM.Player.RecountPerks();
                                                    List<DrawEngine.ColoredWord> cww = new List<DrawEngine.ColoredWord>();
                                                    cww.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = "Ваш класс персонажа изменён на " });
                                                    cww.Add(new DrawEngine.ColoredWord() { Color = ConsoleColor.Cyan, Word = "Молниеносный" });
                                                    cww.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = " !" });
                                                    DrawEngine.InfoWindow.cMessage = cww;
                                                    DrawEngine.GUIDraw.ReDrawCharStat();
                                                    EndDialogue = true;
                                                }
                                                else
                                                {
                                                    R.Text = "Очень жаль. Может быть в другой раз...";
                                                    DrawEngine.ActiveObjectDraw.Draw(R, n);
                                                }
                                            }
                                            break;
                                        }
                                    case ConsoleKey.Escape:
                                        {
                                            EndDialogue = true;
                                            break;
                                        }
                                }
                            }
                            #endregion
                        };
                    return n;
                }
            }

            public static MechEngine.NPC Travor
            {
                get
                {
                    MechEngine.NPC n = new MechEngine.NPC();
                    n.Icon = 'T';
                    n.Color = ConsoleColor.Gray;
                    n.Name = "Тревор";
                    n.Affix = "Школа эссенции";
                    n.Info = "Тревор умеет обращаться с эссенциями и с их помощью может улучшить вашу экипировку.";
                    #region Script
                    n.Script = () =>
                        {
                            PlayEngine.Enemy = false;

                            DrawEngine.Replica R = new DrawEngine.Replica();
                            R.Text = "Я знаю кто ты, "+Rogue.RAM.Player.Name+"! Духи рассказали мне об этом... Моя работа стоит 250 золотых за раз.";
                            R.TextColor = ConsoleColor.DarkMagenta;
                            R.Options.Add("[E] - Зачарование.");                            
                            R.Options.Add("[Escape] - Уйти.");
                            R.OptionsColor = ConsoleColor.DarkCyan;
                            DrawEngine.ActiveObjectDraw.Draw(R, n);

                            CheckGold checkBuy = (int i) =>
                                {
                                    if (Rogue.RAM.Player.Gold >= i) { Rogue.RAM.Player.Gold -= i; DrawEngine.GUIDraw.ReDrawCharStat(); return true; }
                                    else { DrawEngine.InfoWindow.Warning = "У вас недостаточно золота!"; return false; }
                                };
                            Enchanted checkEnch = (MechEngine.Item it) =>
                                {
                                    if (!it.Enchanted) { it.Name += '^'; return true; }
                                    else { DrawEngine.InfoWindow.Warning = "Нельзя зачаровать предмет несколько раз!"; return false; }
                                };

                            bool EndDialogue = false;
                            while (!EndDialogue)
                            {
                                ConsoleKey push = Console.ReadKey(true).Key;
                                switch (push)
                                {
                                    case ConsoleKey.E:
                                        {                                            
                                            R.Text = "Выбери предмет:";                                            
                                            R.Options.Clear();
                                            if (Rogue.RAM.Player.Equipment.Armor != null) { R.Options.Add("[A] - Броня"); }
                                            if (Rogue.RAM.Player.Equipment.Boots != null) { R.Options.Add("[B] - Обувь"); }
                                            if (Rogue.RAM.Player.Equipment.Helm != null) { R.Options.Add("[H] - Шлем"); }
                                            if (Rogue.RAM.Player.Equipment.OffHand != null) { R.Options.Add("[O] - Щит"); }
                                            if (Rogue.RAM.Player.Equipment.Weapon != null) { R.Options.Add("[W] - Оружие"); }
                                            R.Options.Add("[Escape] - Отмена.");
                                            DrawEngine.ActiveObjectDraw.Draw(R, n);
                                            int stats = Convert.ToInt32(Rogue.RAM.Player.Level / 2);
                                            if (stats <= 0) { stats = 1; }
                                            ConsoleKey psh = Console.ReadKey(true).Key;
                                            switch (psh)
                                            {
                                                case ConsoleKey.A:
                                                    {                                                        
                                                        try
                                                        {
                                                            if (checkBuy(250) && checkEnch(Rogue.RAM.Player.Equipment.Armor))
                                                            {
                                                                var link = Rogue.RAM.Player.Equipment.Armor;
                                                                link.Enchanted = true;
                                                                switch (r.Next(4))
                                                                {
                                                                    case 0: { link.ARM += stats; Rogue.RAM.Player.ARM += stats; break; }
                                                                    case 1: { link.HP += stats; Rogue.RAM.Player.CHP += stats; Rogue.RAM.Player.MHP += stats; break; }
                                                                    case 2: { link.MP += stats; Rogue.RAM.Player.CMP += stats; Rogue.RAM.Player.MMP += stats; break; }
                                                                    case 3: { link.MRS += stats; Rogue.RAM.Player.MRS += stats; break; }
                                                                }
                                                                DrawEngine.InfoWindow.cMessage = new List<DrawEngine.ColoredWord>() { new DrawEngine.ColoredWord() { Color = ConsoleColor.Magenta, Word = "Вы зачаровали броню!" } };
                                                            }
                                                        }
                                                        catch { }
                                                        break;
                                                    }
                                                case ConsoleKey.B:
                                                    {
                                                        try
                                                        {
                                                            if (checkBuy(250) && checkEnch(Rogue.RAM.Player.Equipment.Boots))
                                                            {
                                                                var link = Rogue.RAM.Player.Equipment.Boots;
                                                                link.Enchanted = true;
                                                                switch (r.Next(2))
                                                                {
                                                                    case 0: { link.ARM += stats; Rogue.RAM.Player.ARM += stats; break; }
                                                                    case 1: { link.MRS += stats; Rogue.RAM.Player.MRS += stats; break; }
                                                                }
                                                                DrawEngine.InfoWindow.cMessage = new List<DrawEngine.ColoredWord>() { new DrawEngine.ColoredWord() { Color = ConsoleColor.Magenta, Word = "Вы зачаровали обувь!" } };
                                                            }
                                                        }
                                                        catch { }
                                                        break;
                                                    }
                                                case ConsoleKey.H:
                                                    {
                                                        try
                                                        {
                                                            if (checkBuy(250) && checkEnch(Rogue.RAM.Player.Equipment.Helm))
                                                            {
                                                                var link = Rogue.RAM.Player.Equipment.Helm;
                                                                link.Enchanted = true;
                                                                switch (r.Next(4))
                                                                {
                                                                    case 0: { link.AD += stats; Rogue.RAM.Player.AD += stats; break; }
                                                                    case 1: { link.HP += stats; Rogue.RAM.Player.CHP += stats; Rogue.RAM.Player.MHP += stats; break; }
                                                                    case 2: { link.MP += stats; Rogue.RAM.Player.CMP += stats; Rogue.RAM.Player.MMP += stats; break; }
                                                                    case 3: { link.AP += stats; Rogue.RAM.Player.AP += stats; break; }
                                                                }
                                                                DrawEngine.InfoWindow.cMessage = new List<DrawEngine.ColoredWord>() { new DrawEngine.ColoredWord() { Color = ConsoleColor.Magenta, Word = "Вы зачаровали шлем!" } };
                                                            }
                                                        }
                                                        catch { }
                                                        break;
                                                    }
                                                case ConsoleKey.O:
                                                    {
                                                        try
                                                        {
                                                            if (checkBuy(250) && checkEnch(Rogue.RAM.Player.Equipment.OffHand))
                                                            {
                                                                var link = Rogue.RAM.Player.Equipment.OffHand;
                                                                link.Enchanted = true;
                                                                switch (r.Next(6))
                                                                {
                                                                    case 0: { link.ARM += stats; Rogue.RAM.Player.ARM += stats; break; }
                                                                    case 1: { link.AD += stats; Rogue.RAM.Player.AD += stats; break; }
                                                                    case 2: { link.AP += stats; Rogue.RAM.Player.AP += stats; break; }
                                                                    case 3: { link.MRS += stats; Rogue.RAM.Player.MRS += stats; break; }
                                                                    case 4: { link.MADMG += stats; Rogue.RAM.Player.MADMG += stats; break; }
                                                                    case 5: { link.MIDMG += stats; Rogue.RAM.Player.MIDMG += stats; break; }
                                                                }
                                                                DrawEngine.InfoWindow.cMessage = new List<DrawEngine.ColoredWord>() { new DrawEngine.ColoredWord() { Color = ConsoleColor.Magenta, Word = "Вы зачаровали щит!" } };
                                                            }
                                                        }
                                                        catch { }
                                                        break;
                                                    }
                                                case ConsoleKey.W:
                                                    {
                                                        try
                                                        {
                                                            if (checkBuy(250) && checkEnch(Rogue.RAM.Player.Equipment.Weapon))
                                                            {
                                                                var link = Rogue.RAM.Player.Equipment.Weapon;
                                                                link.Enchanted = true;
                                                                switch (r.Next(5))
                                                                {
                                                                    case 0: { link.ARM += stats; Rogue.RAM.Player.ARM += stats; break; }
                                                                    case 1: { link.AD += stats; Rogue.RAM.Player.AD += stats; break; }
                                                                    case 2: { link.AP += stats; Rogue.RAM.Player.AP += stats; break; }
                                                                    case 3: { link.MADMG += stats; Rogue.RAM.Player.MADMG += stats; break; }
                                                                    case 4: { link.MIDMG += stats; Rogue.RAM.Player.MIDMG += stats; break; }
                                                                }
                                                                DrawEngine.InfoWindow.cMessage = new List<DrawEngine.ColoredWord>() { new DrawEngine.ColoredWord() { Color = ConsoleColor.Magenta, Word = "Вы зачаровали оружие!" } };
                                                            }
                                                        }
                                                        catch { }
                                                        break;
                                                    }
                                                case ConsoleKey.Escape:
                                                    {
                                                        break;
                                                    }
                                            }
                                            R.Text = "Я знаю кто ты, " + Rogue.RAM.Player.Name + "! Духи рассказали мне об этом...";
                                            R.TextColor = ConsoleColor.DarkMagenta;
                                            R.Options.Clear();
                                            R.Options.Add("[E] - Зачарование.");
                                            R.Options.Add("[Escape] - Уйти.");
                                            R.OptionsColor = ConsoleColor.DarkCyan;
                                            DrawEngine.ActiveObjectDraw.Draw(R, n);
                                            DrawEngine.GUIDraw.ReDrawCharStat();
                                            break;
                                        }
                                    case ConsoleKey.Escape: { EndDialogue = true; break; }
                                }
                            }

                            PlayEngine.Enemy = true;
                        };
                    #endregion
                    return n;
                }
            }
            private delegate bool Enchanted(MechEngine.Item It);
            private delegate bool CheckGold(int Cost);

            public static MechEngine.NPC Behavor
            {
                get
                {
                    MechEngine.NPC n = new MechEngine.NPC();
                    n.Icon = 'B';
                    n.Color = ConsoleColor.DarkYellow;
                    n.Name = "Бэхор";
                    n.Affix = "Школа материала";
                    n.Info = "Бэхор - материальный маг, он годами обучает своих учеников создавать магические вещи.";
                    #region Script
                    n.Script += delegate()
                    {
                        PlayEngine.Enemy = false;
                        DrawEngine.Replica R = new DrawEngine.Replica();
                        R.Text = "Да. Ты хочешь обучиться созданию магических вещей? С тебя 1000 золотых и 5 эссенций за рецепт.";
                        R.TextColor = ConsoleColor.DarkYellow;

                        R.Options.Add("[W] - Рецепт оружия.");
                        R.Options.Add("[B] - Рецепт обуви.");
                        R.Options.Add("[S] - Рецепт свитка.");
                        R.Options.Add("[Escape] - Уйти.");
                        R.OptionsColor = ConsoleColor.DarkGray;
                        DrawEngine.ActiveObjectDraw.Draw(R, n);

                        CheckGold checkBuy = (int i) =>
                            {
                                if (Rogue.RAM.Player.Gold >= i) { Rogue.RAM.Player.Gold -= i; DrawEngine.GUIDraw.ReDrawCharStat(); return true; }
                                else { DrawEngine.InfoWindow.Warning = "У вас недостаточно золота!"; return false; }
                            };
                        CheckGold checkEssencial = (int i) =>
                            {
                                i = 0;
                                foreach (MechEngine.Item it in Rogue.RAM.Player.Inventory)
                                {
                                    if (it.Name == ResourseBase.Essencial.Name) { i++; } 
                                }
                                if (i >= 5) { return true; }
                                else { DrawEngine.InfoWindow.Warning = "У вас недостаточно эссенций!"; return false; }
                            };
                        Enchanted deleteEssencial = (MechEngine.Item it) =>
                            {
                                List<MechEngine.Item> itms = new List<MechEngine.Item>();
                                foreach (MechEngine.Item i in Rogue.RAM.Player.Inventory)
                                {
                                    if (i.Name == ResourseBase.Essencial.Name) { itms.Add(i); }
                                }
                                try
                                {
                                    for (int i = 0; i < 5; i++)
                                    { Rogue.RAM.Player.Inventory.Remove(itms[i]); }
                                }
                                catch { }
                                DrawEngine.GUIDraw.ReDrawCharInventory();
                                return true;
                            };


                        bool EndDialogue = false;
                        while (!EndDialogue)
                        {
                            ConsoleKey push = Console.ReadKey(true).Key;
                            switch (push)
                            {
                                case ConsoleKey.W:
                                    {
                                        if (checkBuy(1000) & checkEssencial(0))
                                        {
                                            deleteEssencial(ResourseBase.Essencial);
                                            ChangeCraftAbility(OtherAbilityBase.EfirAirWeapon);
                                            DrawEngine.ActiveObjectDraw.Draw(R, n);
                                        }
                                        break;
                                    }
                                case ConsoleKey.B:
                                    {
                                        if (checkBuy(1000) & checkEssencial(0))
                                        {
                                            deleteEssencial(ResourseBase.Essencial);
                                            ChangeCraftAbility(OtherAbilityBase.EfirAirBoots); 
                                            DrawEngine.ActiveObjectDraw.Draw(R, n);
                                        }
                                        break;
                                    }
                                case ConsoleKey.S:
                                    {
                                        if (checkBuy(1000) & checkEssencial(0))
                                        {
                                            deleteEssencial(ResourseBase.Essencial);
                                            ChangeCraftAbility(OtherAbilityBase.EfirAirScroll);
                                            DrawEngine.ActiveObjectDraw.Draw(R, n);
                                        }
                                        break;
                                    }
                                case ConsoleKey.Escape:
                                    {
                                        EndDialogue = true;
                                        break;
                                    }
                            }
                        }

                        PlayEngine.Enemy = true;
                    };
                    #endregion
                    return n;
                }
            }

            public static MechEngine.NPC Folk
            {
                get
                {
                    MechEngine.NPC n = new MechEngine.NPC();
                    n.Icon = 'F';
                    n.Color = ConsoleColor.Red;
                    n.Move = false;
                    n.Name = "Фолк";
                    n.Affix = "Коалиция огня";
                    n.Info = "Представитель стихии огня в Мраумире.";
                    n.UseScript = true;
                    n.Script = () =>
                        {
                            PlayEngine.Enemy = false;
                            DrawEngine.InfoWindow.cMessage = new List<DrawEngine.ColoredWord>()
                            {                                
                                new DrawEngine.ColoredWord(){ Word=n.Affix, Color=ConsoleColor.Yellow},
                                new DrawEngine.ColoredWord(){ Word=n.Name, Color=n.Color},
                                new DrawEngine.ColoredWord(){ Word="Не желает с вами разговаривать!", Color=Rogue.RAM.Map.Biom},
                            };
                            Thread.Sleep(150);
                            PlayEngine.Enemy = true;
                        };
                    return n;
                }
            }

            public static MechEngine.NPC Ward
            {
                get
                {
                    MechEngine.NPC n = new MechEngine.NPC();
                    n.Icon = 'W';
                    n.Color = ConsoleColor.Blue;
                    n.Move = false;
                    n.Name = "Вард";
                    n.Affix = "Коалиция воды";
                    n.Info = "Представитель стихии воды в Мраумире.";
                    n.UseScript = true;
                    n.Script = () =>
                    {
                        PlayEngine.Enemy = false;
                        DrawEngine.InfoWindow.cMessage = new List<DrawEngine.ColoredWord>()
                            {                                
                                new DrawEngine.ColoredWord(){ Word=n.Affix, Color=ConsoleColor.Yellow},
                                new DrawEngine.ColoredWord(){ Word=n.Name, Color=n.Color},
                                new DrawEngine.ColoredWord(){ Word="Не желает с вами разговаривать!", Color=Rogue.RAM.Map.Biom},
                            };
                        Thread.Sleep(150);
                        PlayEngine.Enemy = true;
                    };
                    return n;
                }
            }

            public static MechEngine.NPC Eart
            {
                get
                {
                    MechEngine.NPC n = new MechEngine.NPC();
                    n.Icon = 'E';
                    n.Color = ConsoleColor.DarkYellow;
                    n.Move = false;
                    n.Name = "Иарт";
                    n.Affix = "Коалиция земли";
                    n.Info = "Представитель стихии земли в Мраумире.";
                    n.UseScript = true;
                    n.Script = () =>
                    {
                        PlayEngine.Enemy = false;
                        DrawEngine.InfoWindow.cMessage = new List<DrawEngine.ColoredWord>()
                            {                                
                                new DrawEngine.ColoredWord(){ Word=n.Affix, Color=ConsoleColor.Yellow},
                                new DrawEngine.ColoredWord(){ Word=n.Name, Color=n.Color},
                                new DrawEngine.ColoredWord(){ Word="Не желает с вами разговаривать!", Color=Rogue.RAM.Map.Biom},
                            };
                        Thread.Sleep(150);
                        PlayEngine.Enemy = true;
                    };
                    return n;
                }
            }

            public static MechEngine.NPC Ange
            {
                get
                {
                    MechEngine.NPC n = new MechEngine.NPC();
                    n.Icon = 'A';
                    n.Color = ConsoleColor.Cyan;
                    n.Move = false;
                    n.Name = "Анги";
                    n.Affix = "Коалиция воздуха";
                    n.Info = "Представитель стихии воздуха в Мраумире.";
                    n.UseScript = true;
                    n.Script = () =>
                    {
                        PlayEngine.Enemy = false;
                        DrawEngine.InfoWindow.cMessage = new List<DrawEngine.ColoredWord>()
                            {                                
                                new DrawEngine.ColoredWord(){ Word=n.Affix, Color=ConsoleColor.Yellow},
                                new DrawEngine.ColoredWord(){ Word=n.Name, Color=n.Color},
                                new DrawEngine.ColoredWord(){ Word="Не желает с вами разговаривать!", Color=Rogue.RAM.Map.Biom},
                            };
                        Thread.Sleep(150);
                        PlayEngine.Enemy = true;
                    };
                    return n;
                }
            }

            private static void ChangeCraftAbility(MechEngine.Ability New)
            {
                int currentint = 0;
                DrawEngine.PopUpWindowDraw.SwitchCraftAbilWindow(0, new List<MechEngine.Ability>() { Rogue.RAM.Player.CraftAbility[0], Rogue.RAM.Player.CraftAbility[1], Rogue.RAM.Player.CraftAbility[3] });
                bool endd = false;
                while (!endd)
                {
                    ConsoleKey pushh = Console.ReadKey(true).Key;
                    switch (pushh)
                    {
                        case ConsoleKey.RightArrow: { if (currentint != 2) { currentint++; DrawEngine.PopUpWindowDraw.SwitchCraftAbilWindow(currentint, new List<MechEngine.Ability>() { Rogue.RAM.Player.CraftAbility[0], Rogue.RAM.Player.CraftAbility[1], Rogue.RAM.Player.CraftAbility[3] }); } break; }
                        case ConsoleKey.LeftArrow: { if (currentint != 0) { currentint--; DrawEngine.PopUpWindowDraw.SwitchCraftAbilWindow(currentint, new List<MechEngine.Ability>() { Rogue.RAM.Player.CraftAbility[0], Rogue.RAM.Player.CraftAbility[1], Rogue.RAM.Player.CraftAbility[3] }); } break; }
                        case ConsoleKey.Enter:
                            {
                                if (currentint == 2) { currentint = 3; }
                                string oldabil = Rogue.RAM.Player.CraftAbility[currentint].Name;
                                ConsoleColor oldcolorabil = Rogue.RAM.Player.CraftAbility[currentint].Color;
                                Rogue.RAM.Player.CraftAbility[currentint] = New;

                                DrawEngine.InfoWindow.cMessage = new List<DrawEngine.ColoredWord>()
                                                            {
                                                                new DrawEngine.ColoredWord(){Color=Rogue.RAM.Map.Biom, Word="Вы заменяете "},
                                                                new DrawEngine.ColoredWord() { Color=oldcolorabil, Word=oldabil},
                                                                new DrawEngine.ColoredWord(){Color=Rogue.RAM.Map.Biom, Word=" на "},
                                                                new DrawEngine.ColoredWord() { Color=Rogue.RAM.Player.CraftAbility[currentint].Color, Word=Rogue.RAM.Player.CraftAbility[currentint].Name},
                                                                new DrawEngine.ColoredWord(){Color=Rogue.RAM.Map.Biom, Word=" !"}
                                                            };
                                DrawEngine.GUIDraw.DrawLab();
                                endd = true;
                                break;
                            }
                        case ConsoleKey.Escape: { DrawEngine.GUIDraw.DrawLab(); endd = true; break; }
                        default: { break; }
                    }
                }
            }

            public static MechEngine.NPC Keeper
            {
                get
                {
                    MechEngine.NPC n = new MechEngine.NPC();
                    n.Icon = '@';
                    n.Color = ConsoleColor.White;
                    n.Move = false;
                    n.Name = "Хранитель";
                    n.Affix = "Архангел";                    
                    n.UseScript = true;
                    n.Script = () =>
                    {
                        PlayEngine.Enemy = false;

                        DrawEngine.Replica R = new DrawEngine.Replica();
                        R.Text = "Добро пожаловать в рай, "+Rogue.RAM.Player.Name+" ! Да, именно так, ты в раю! С чего желаешь начать осмотр?";
                        R.TextColor = ConsoleColor.White;
                        R.Options.Add("[Q] - Зал игрищ.");
                        R.Options.Add("[W] - Зал наслаждений.");
                        R.Options.Add("[E] - Зал суда.");
                        R.OptionsColor = ConsoleColor.White;
                        DrawEngine.ActiveObjectDraw.Draw(R, n);

                        #region Cutscene
                        MechEngine.Script Cutscene = () =>
                            {
                                DrawEngine.DigitalArt.Banishment(37, 14);
                                DrawEngine.winAPIDraw.Helpers.ReDrawOneChar(35, 13, 35, 13, '@', '@', 13, 13);
                                SoundEngine.Sound.Teleport();
                                
                            };
                        #endregion


                        bool end = false;
                        while (!end)
                        {
                            ConsoleKey k = Console.ReadKey(true).Key;
                            switch (k)
                            {
                                default: { Cutscene(); Defender.Use(); PlayEngine.Enemy = false; end = true; break; }
                            }
                        }

                        R.Text=Rogue.RAM.Player.Name+", многоуважаемый, не могли бы вы нам помочь с этим?";
                        R.Options.Clear();
                        R.Options.Add("[N] - Нет?");
                        R.Options.Add("[Y] - Я готов!");
                        DrawEngine.ActiveObjectDraw.Draw(R,n);

                        end = false;
                        while (!end)
                        {
                            ConsoleKey k = Console.ReadKey(true).Key;
                            switch (k)
                            {
                                case ConsoleKey.N:
                                    {
                                        R.Text = "Знаете, у вас была не самая однозначная жизнь. В любом случае, мне прийдётся не оставить вам выбора.";
                                        R.Options.Remove("[N] - Нет?");
                                        DrawEngine.ActiveObjectDraw.Draw(R, n);
                                        break;
                                    }
                                case ConsoleKey.Y:
                                    {
                                        R.Text = "Замечательно! Рад что вы согласилсь. Я дам вам новую жизнь, и отправлю на планету Аурегон. Вашей задачей будет найти там Валорана и убить его.";
                                        R.Options.Clear();
                                        DrawEngine.ActiveObjectDraw.Draw(R, n);
                                        List<DrawEngine.ColoredWord> cq = new List<DrawEngine.ColoredWord>();
                                        cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = "Вы получаете новое задание - " });
                                        cq.Add(new DrawEngine.ColoredWord() { Color = QuestBase.__MAIN_QUEST.Color, Word = QuestBase.__MAIN_QUEST.Name });
                                        cq.Add(new DrawEngine.ColoredWord() { Color = Rogue.RAM.Map.Biom, Word = " !" });
                                        DrawEngine.InfoWindow.cMessage = cq;
                                        Thread.Sleep(1000);
                                        DrawEngine.InfoWindow.Message = "Нажмите любую клавишу для продолжения...";
                                        Console.ReadKey(true);
                                        DrawEngine.InfoWindow.Clear();
                                        R.Text = "Вы появитесь в летающем городе Мраумир, оттуда вам будет проще найти Валорана. За детальными инструкциями обращайтесь к ангелу Вирджилу, он пойдёт с вами.";
                                        DrawEngine.ActiveObjectDraw.Draw(R, n);                                        
                                        DrawEngine.InfoWindow.Message = "Нажмите любую клавишу для оживления...";
                                        Console.ReadKey(true);

                                        LabirinthEngine.Create(1, true);
                                        SoundEngine.Music.TownTheme();
                                        PlayEngine.EnemyMoves.Move(true);
                                        PlayEngine.GamePlay.Play();
                                        break;
                                    }
                            }
                        }

                        PlayEngine.Enemy = true;
                    };
                    return n;
                }
            }

            public static MechEngine.NPC Defender
            {
                get
                {
                    MechEngine.NPC n = new MechEngine.NPC();
                    n.Icon = '@';
                    n.Color = ConsoleColor.Magenta;
                    n.Move = false;
                    n.Name = "Тираэль";
                    n.Affix = "Защитник";
                    n.UseScript = true;
                    n.Script = () =>
                    {
                        PlayEngine.Enemy = false;

                        DrawEngine.Replica R = new DrawEngine.Replica();
                        R.Text = "Хранитель! Мерсеры обнаружили активность Валорана, нам срочно нужен доброволец!";
                        R.TextColor = ConsoleColor.DarkYellow;
                        R.Options.Add("[N] - Это не я.");
                        R.Options.Add("[T] - Это точно не я.");
                        R.Options.Add("[K] - Подождать.");
                        R.OptionsColor = ConsoleColor.White;
                        DrawEngine.ActiveObjectDraw.Draw(R, n);

                        bool end = false;
                        while (!end)
                        {
                            ConsoleKey k = Console.ReadKey(true).Key;
                            switch (k)
                            {
                                case ConsoleKey.T:
                                case ConsoleKey.N:
                                    {
                                        R.Options.Clear();
                                        R.Text = "(Тираэль делает вид что не слышит вас) Хранитель, вы должны срочно найти душу-добровольца для того что бы убить Валорана пока не поздно!";
                                        DrawEngine.ActiveObjectDraw.Draw(R, n);
                                        end = true;
                                        break;
                                    }
                                case ConsoleKey.K:
                                    {
                                        R.Options.Clear();
                                        R.Text = "Хранитель, вы должны срочно найти душу-добровольца для того что бы убить Валорана пока не поздно!";
                                        DrawEngine.ActiveObjectDraw.Draw(R, n);
                                        end = true;
                                        break;
                                    }
                              //  default: { Cutscene(); break; }
                            }
                        }
                    };
                    return n;
                }
            }

            public static MechEngine.NPC Vergiliy
            {
                get
                {
                    MechEngine.NPC n = new MechEngine.NPC();
                    n.Icon = '@';
                    n.Color = ConsoleColor.Cyan;
                    n.Move = false;
                    n.Name = "Вирджил";
                    n.Affix = "Лидер 'Ордена'";
                    n.Info = "Вирджил - ваш ангел-хранитель на этой планете. От смерти он вас не спасёт, но зато может дать совет.";
                    n.UseScript = true;
                    n.Script = () =>
                    {
                        PlayEngine.Enemy = false;

                        bool endofgame = false;
                        foreach (MechEngine.Quest q in Rogue.RAM.Player.QuestBook)
                        {
                            if (q.Name == QuestBase.__MAIN_QUEST.Name)
                            {
                                if (q.Progress == q.TargetCount)
                                {
                                    endofgame = true;
                                }
                            }
                        }

                        DrawEngine.Replica R = new DrawEngine.Replica();
                        R.Text = "Брат? Нет, "+Rogue.RAM.Player.Name+", ты не мой брат... Ты хотел поговорить?";
                        R.TextColor = ConsoleColor.DarkCyan;
                        R.Options.Add("[I] - Помоги освоиться.");
                        R.Options.Add("[Q] - Зачем мы здесь?");
                        R.Options.Add("[T] - Торговать.");
                        if (endofgame) { R.Options.Add("[@] - Это конец."); }
                        R.Options.Add("[Escape] - Отмена.");
                        R.OptionsColor = ConsoleColor.White;
                        DrawEngine.ActiveObjectDraw.Draw(R, n);

                        bool end = false;
                        while (!end)
                        {
                            ConsoleKey k = Console.ReadKey(true).Key;
                            switch (k)
                            {
                                case ConsoleKey.NumPad2:
                                case ConsoleKey.D2:
                                    {
                                        if (endofgame)
                                        {
                                            R.Text = "Ты прав. Валорана больше нет. Теперь скажи что ты хочешь делать дальше: ";
                                            R.Options.Clear();
                                            R.Options.Add("[@] - Уйти в рай.");
                                            R.Options.Add("[#] - Остаться тут.");
                                            DrawEngine.ActiveObjectDraw.Draw(R, n);
                                            bool endd = false;
                                            while (!endd)
                                            {
                                                ConsoleKeyInfo kk=Console.ReadKey(true);
                                                switch (kk.Key)
                                                {
                                                    case ConsoleKey.D2:
                                                    case ConsoleKey.NumPad2:
                                                        {
                                                            if (kk.Modifiers == ConsoleModifiers.Shift)
                                                            {
                                                                R.Text = "Разумеется, любой бы на твоём месте поступил так же. Готов к смерти?";
                                                                R.Options.Clear();
                                                                DrawEngine.ActiveObjectDraw.Draw(R, n);
                                                                DrawEngine.InfoWindow.Warning = "Нажмите любую клавишу что бы умереть.";
                                                                Console.ReadKey(true);
                                                                DrawEngine.SplashScreen.AngelEnd();
                                                            }
                                                            break;
                                                        }
                                                    case ConsoleKey.D3:
                                                    case ConsoleKey.NumPad3:
                                                        {
                                                            if (kk.Modifiers == ConsoleModifiers.Shift)
                                                            {
                                                                R.Text = "Да? Ты готов гнить здесь и дальше? Хорошо. Я вернусь в рай без тебя.";
                                                                R.Options.Clear();
                                                                DrawEngine.ActiveObjectDraw.Draw(R, n);
                                                                DrawEngine.InfoWindow.Warning = "Нажмите любую клавишу что бы отпустить вирджила.";
                                                                Console.ReadKey(true);
                                                                DrawEngine.SplashScreen.DeamonEnd();
                                                                SystemEngine.Helper.Information.Point p = SystemEngine.Helper.Information.FindObject(n);
                                                                Rogue.RAM.Map.Map[p.x][p.y].Object = null;
                                                                DrawEngine.winAPIDraw.Helpers.ReDrawOneChar((Int16)p.x, (Int16)p.y, (Int16)p.x, (Int16)p.y);
                                                                Rogue.RAM.Flags.VirgilEnd = true;
                                                                end = true;
                                                                endd = true;
                                                            }
                                                            break;
                                                        }
                                                }
                                            }
                                            DrawEngine.ActiveObjectDraw.Draw(R, n);
                                        }
                                        break;
                                    }
                                case ConsoleKey.I:
                                    {
                                        R.Options.Clear();
                                        DrawEngine.InfoWindow.Clear();
                                        R.Text = "Ладно. Слушай. Это Мраумир - летающий город-крепость над планетой Аурегон. Мы в торговом квартале, рядом со мной есть твой сундук, фонтаны пополнения и дипломат.";
                                        DrawEngine.ActiveObjectDraw.Draw(R, n);
                                        DrawEngine.InfoWindow.Message = "Нажмите любую клавишу для продолжения.";
                                        Console.ReadKey(true);
                                        R.Text = "В сундуке ты можешь хранить личные вещи, фонтаны помогут тебе восстановить силы, а Дипломат расскажет про твои отношения с фракциями.";
                                        DrawEngine.ActiveObjectDraw.Draw(R, n);
                                        DrawEngine.InfoWindow.Warning = "Нажмите любую клавишу для продолжения.";
                                        Console.ReadKey(true);
                                        R.Text = "Выше и левее всего этого добра есть портал на планету. Используй его для того что бы найти Валорана. Помнишь я давал тебе камень города?";
                                        DrawEngine.ActiveObjectDraw.Draw(R, n);
                                        DrawEngine.InfoWindow.Message = "Нажмите любую клавишу для продолжения.";
                                        Console.ReadKey(true);
                                        R.Text = "Так вот, если ты будешь пользоваться им, ниже портала на планету будет твой личный портал. Он будет синим в отличии от главного портала города.";
                                        DrawEngine.ActiveObjectDraw.Draw(R, n);
                                        DrawEngine.InfoWindow.Warning = "Нажмите любую клавишу для продолжения.";
                                        Console.ReadKey(true);
                                        R.Text = "Здесь, в Торговом квартале, в основном живут ремесленники и торговцы, а так же, гораздо ниже портала есть странные пришельцы.";
                                        DrawEngine.ActiveObjectDraw.Draw(R, n);
                                        DrawEngine.InfoWindow.Message = "Нажмите любую клавишу для продолжения.";
                                        Console.ReadKey(true);
                                        R.Text = "Если тебе наскучит общество живых, ты можешь отправиться в квартал мёртвых, он находится на севере. Что там я не знаю.";
                                        DrawEngine.ActiveObjectDraw.Draw(R, n);
                                        DrawEngine.InfoWindow.Warning = "Нажмите любую клавишу для продолжения.";
                                        Console.ReadKey(true);
                                        R.Text = "На западе ты можешь найти квартал магов, там есть зачарователи предметов и представители стихий планеты Аурегон. ";
                                        DrawEngine.ActiveObjectDraw.Draw(R, n);
                                        DrawEngine.InfoWindow.Message = "Нажмите любую клавишу для продолжения.";
                                        Console.ReadKey(true);
                                        R.Text = "У них там особая система перемещений, так что постарайся не запутаться.";
                                        DrawEngine.ActiveObjectDraw.Draw(R, n);
                                        DrawEngine.InfoWindow.Warning = "Нажмите любую клавишу для продолжения.";
                                        Console.ReadKey(true);
                                        R.Text = "На востоке Торгового квартала есть Квартал ярости, на юге - Квартал братства. Но попасть в них нельзя.";
                                        DrawEngine.ActiveObjectDraw.Draw(R, n);
                                        DrawEngine.InfoWindow.Message = "Нажмите любую клавишу для продолжения.";
                                        Console.ReadKey(true);
                                        R.Text = "Говорят, в этом виноват главный архитектор города. На этом мои знания о Мраумире кончаются, попробуй найти что-то ещё сам.";
                                        DrawEngine.ActiveObjectDraw.Draw(R, n);
                                        DrawEngine.InfoWindow.Warning = "Нажмите любую клавишу для продолжения.";
                                        Console.ReadKey(true);
                                        R.Text = "Брат? Нет, " + Rogue.RAM.Player.Name + ", ты не мой брат... Ты хотел поговорить?";
                                        R.TextColor = ConsoleColor.DarkCyan;
                                        R.Options.Add("[I] - Помоги освоиться.");
                                        R.Options.Add("[Q] - Зачем мы здесь?");
                                        R.Options.Add("[T] - Торговать.");
                                        if (endofgame) { R.Options.Add("[@] - Это конец."); }
                                        R.Options.Add("[Escape] - Отмена.");
                                        DrawEngine.ActiveObjectDraw.Draw(R, n);
                                        break;
                                    }
                                case ConsoleKey.Q:
                                    {
                                        R.Options.Clear();
                                        DrawEngine.InfoWindow.Clear();
                                        R.Text = "Хранитель послал тебя сюда что бы остановить Валорана. А меня за компанию как самого бунтующего ангела.";
                                        DrawEngine.ActiveObjectDraw.Draw(R, n);
                                        DrawEngine.InfoWindow.Message = "Нажмите любую клавишу для продолжения.";
                                        Console.ReadKey(true);
                                        R.Text = "Твоя задача - найти его и остановить, иначе Аурегон будет потерян для рая, и силы демонов смогут переместиться в рай через Аурегон.";
                                        DrawEngine.ActiveObjectDraw.Draw(R, n);
                                        DrawEngine.InfoWindow.Warning = "Нажмите любую клавишу для продолжения.";
                                        Console.ReadKey(true);
                                        R.Text = "Если ты спросишь у меня, где и как найти Валорана - я не знаю. Всё что я знаю это то что он находится где-то на этой планете.";
                                        DrawEngine.ActiveObjectDraw.Draw(R, n);
                                        DrawEngine.InfoWindow.Message = "Нажмите любую клавишу для продолжения.";
                                        Console.ReadKey(true);
                                        R.Text = "Всё что я могу тебе посоветовать - ищи его как можно дольше.";
                                        DrawEngine.ActiveObjectDraw.Draw(R, n);
                                        DrawEngine.InfoWindow.Warning = "Нажмите любую клавишу для продолжения.";
                                        Console.ReadKey(true);
                                        double sh = (double)Rogue.RAM.Map.Level;
                                        if (sh < 50) { sh = 0.01; }
                                        else { sh += (Rogue.RAM.Map.Level - 50) * 0.3; }
                                        R.Text = "Ну, и моё чувство ангела подсказывает что шанс найти его сейчас равен <"+sh+"%>...";
                                        DrawEngine.ActiveObjectDraw.Draw(R, n);
                                        DrawEngine.InfoWindow.Message = "Нажмите любую клавишу для продолжения.";
                                        Console.ReadKey(true);
                                        R.Text = "Брат? Нет, " + Rogue.RAM.Player.Name + ", ты не мой брат... Ты хотел поговорить?";
                                        R.TextColor = ConsoleColor.DarkCyan;
                                        R.Options.Add("[I] - Помоги освоиться.");
                                        R.Options.Add("[Q] - Зачем мы здесь?");
                                        R.Options.Add("[T] - Торговать.");
                                        if (endofgame) { R.Options.Add("[@] - Это конец."); }
                                        R.Options.Add("[Escape] - Отмена.");
                                        DrawEngine.ActiveObjectDraw.Draw(R, n);
                                        break;
                                    }
                                case ConsoleKey.T:
                                    {
                                        R.Text = "У меня нет настроения на торговлю...";
                                        DrawEngine.ActiveObjectDraw.Draw(R, n);
                                        break;
                                    }
                                case ConsoleKey.Escape: { end = true; break; }
                            }
                        }
                    };
                    return n;
                }
            }

            public static MechEngine.NPC VergiliyDialogue
            {
                get
                {
                    MechEngine.NPC n = new MechEngine.NPC();
                    n.Icon = '@';
                    n.Color = ConsoleColor.Cyan;
                    n.Move = false;
                    n.Name = "Вирджил";
                    n.Affix = "Лидер 'Ордена'";
                    n.Info = "Вирджил - ваш ангел-хранитель на этой планете. От смерти он вас не спасёт, но зато может дать совет.";
                    n.UseScript = true;
                    n.Script = () =>
                    {
                        PlayEngine.Enemy = false;

                        DrawEngine.Replica R = new DrawEngine.Replica();
                        R.TextColor = ConsoleColor.Cyan;

                        DrawEngine.InfoWindow.Clear();
                        R.Text = "Привет, я твой ангел-хранитель на Аурегоне";
                        DrawEngine.ActiveObjectDraw.Draw(R, n);
                        DrawEngine.InfoWindow.Message = "Нажмите любую клавишу для продолжения.";
                        Console.ReadKey(true);
                        R.Text = "Знаешь, у меня нет магических сил, поэтому умереть ты можешь быстро и внезапно.";
                        DrawEngine.ActiveObjectDraw.Draw(R, n);
                        DrawEngine.InfoWindow.Warning = "Нажмите любую клавишу для продолжения.";
                        Console.ReadKey(true);
                        R.Text = "Но что бы хоть как-то сгладить эту ситуацию, я положил тебе в инвентарь парочку полезных вещей.";
                        DrawEngine.ActiveObjectDraw.Draw(R, n);
                        DrawEngine.InfoWindow.Message = "Нажмите любую клавишу для продолжения.";
                        Console.ReadKey(true);
                        R.Text = "Кроме того, можешь обращаться ко мне за советом, я буду стоять на главной площади.";
                        DrawEngine.ActiveObjectDraw.Draw(R, n);
                        DrawEngine.InfoWindow.Warning = "Нажмите любую клавишу для продолжения.";
                        Console.ReadKey(true);

                        PlayEngine.Enemy = true;

                    };
                    return n;
                }
            }

            public static MechEngine.NPC Angel
            {
                get
                {
                    MechEngine.NPC n = new MechEngine.NPC();
                    n.Icon = '@';
                    n.Color = ConsoleColor.Cyan;
                    n.Name = "Ангел";
                    n.Affix = "Святой";
                    n.Move = true;
                    n.UseScript = true;
                    n.Script = () =>
                    {
                        PlayEngine.Enemy = false;
                        DrawEngine.InfoWindow.cMessage = new List<DrawEngine.ColoredWord>()
                            {                                
                                new DrawEngine.ColoredWord(){ Word=n.Affix, Color=ConsoleColor.Yellow},
                                new DrawEngine.ColoredWord(){ Word=n.Name, Color=n.Color},
                                new DrawEngine.ColoredWord(){ Word="Не желает с вами разговаривать!", Color=Rogue.RAM.Map.Biom},
                            };
                        Thread.Sleep(150);
                        PlayEngine.Enemy = true;
                    };
                    return n;
                }
            }
        }

        public static class MembersBase
        {
            public static MechEngine.Member Undead
            {
                get
                {
                    MechEngine.Member m = new MechEngine.Member();
                    m.BackgroundColor = 128;
                    m.ForegroundColor = 0;
                    m.Icon = '↨';
                    m.Color = ConsoleColor.DarkGray;
                    m.Affix = "Эмиссар";
                    m.Move = false;
                    m.UseScript = true;
                    m.Info = "Эмиссар Мёртвых";
                    m.SpeachColor = ConsoleColor.DarkGray;
                    m.SpeachIcon = '↨';
                    m.CurGold = 10000;
                    m.MaxGold = 100000;
                    if (!Rogue.RAM.Flags.Valery) { m.Name = "Валери"; }
                    if (!Rogue.RAM.Flags.Stephan) { m.Name = "Стефан"; }
                    #region Script
                    m.Script = () =>
                        {
                            PlayEngine.Enemy = false;
                            m.Goods = new List<MechEngine.Item>()
                            {
                                ResourseBase.DeadRose,
                                ResourseBase.DeadWater,
                                ResourseBase.DeadPoison,
                                ItemBase.ReputationItems.AntiUndeadSword,
                                ItemBase.ReputationItems.RatTail,
                                ItemBase.ReputationItems.UndeadHelm,
                                ItemBase.ReputationItems.ElementalHead,
                                ItemBase.ReputationItems.HelmOfDeath
                            };
                            Rogue.RAM.Merch = m;
                            Rogue.RAM.MerchTab.MaxTab = m.Goods.Count;
                            Rogue.RAM.MerchTab.NowTab = 1;
                            DrawEngine.MerchantDraw.DrawGoodsWindow(true);
                            PlayEngine.GamePlay.Merch();

                            PlayEngine.Enemy = true;

                        };
                    #endregion
                    return m;
                }
            }

            public static MechEngine.Member Dwarf
            {
                get
                {
                    MechEngine.Member m = new MechEngine.Member();
                    m.BackgroundColor = 128;
                    m.ForegroundColor = 0;
                    m.Icon = '↨';
                    m.Color = ConsoleColor.DarkGray;
                    m.Affix = "Эмиссар";
                    m.Move = false;
                    m.UseScript = true;
                    m.Info = "Эмиссар дварфов";
                    m.SpeachColor = ConsoleColor.DarkGray;
                    m.SpeachIcon = '↨';
                    m.CurGold = 10000;
                    m.MaxGold = 100000;
                    m.Name = "Хэйкпак";
                    #region Script
                    m.Script = () =>
                    {
                        PlayEngine.Enemy = false;
                        m.Goods = new List<MechEngine.Item>()
                            {
                                ItemBase.ReputationItems.ElementalShield,
                                ItemBase.ReputationItems.HodarsHelm,
                                ItemBase.ReputationItems.BootsOfDeath,
                                ItemBase.ReputationItems.DwarfFortik,
                                ItemBase.ReputationItems.OldRune
                            };
                        Rogue.RAM.Merch = m;
                        Rogue.RAM.MerchTab.MaxTab = m.Goods.Count;
                        Rogue.RAM.MerchTab.NowTab = 1;
                        DrawEngine.MerchantDraw.DrawGoodsWindow(true);
                        PlayEngine.GamePlay.Merch();

                        PlayEngine.Enemy = true;

                    };
                    #endregion
                    return m;
                }
            }
        }

        public static class MerchantBase
        {
            public static MechEngine.Merchant GetMerchant
            {
                get
                {
                    ;
                    int f = r.Next(4);
                    switch (f)
                    {
                        case 0: { return Norman; }
                        case 1: { return CrazyAlchemist; }
                        case 2: { return DrowMiller; }
                        case 3: { return MiniShop; }
                        default: { return MiniShop; }
                    }
                }
            }

            public static MechEngine.Merchant Norman
            {
                get
                {
                    MechEngine.Merchant m = new MechEngine.Merchant();
                    m.Icon = '$';
                    m.MaxGold = 1000;
                    ;
                    m.SpeachColor = ConsoleColor.DarkCyan;
                    m.SpeachIcon='"';
                    m.CurGold = r.Next(500);
                    m.Color = ConsoleColor.Yellow;
                    m.Name = "Норман - Торговец";
                    m.Goods = new List<MechEngine.Item>() 
                    { 
                        ItemBase.GetItem, 
                        ItemBase.GetItem, 
                        ItemBase.GetItem, 
                        ItemBase.GetItem, 
                        ItemBase.GetItem, 
                        ItemBase.GetItem, 
                        ItemBase.GetItem, 
                        ItemBase.GetItem, 
                        ItemBase.GetItem, 
                        ItemBase.GetItem, 
                        ItemBase.GetItem, 
                        ItemBase.GetItem, 
                    };
                    return m;
                }
            }

            public static MechEngine.Merchant CrazyAlchemist
            {
                get
                {
                    MechEngine.Merchant m = new MechEngine.Merchant();
                    m.Icon = '$';
                    m.MaxGold = 1000;
                    m.SpeachColor = ConsoleColor.Magenta;
                    m.SpeachIcon = '*';
                    ;
                    m.CurGold = r.Next(500);
                    m.Color = ConsoleColor.Yellow;
                    m.Name = "Алхимик - Торговец";
                    m.Goods = new List<MechEngine.Item>() 
                    {
                        ItemBase.GetPotion,
                        ItemBase.GetPotion,
                        ItemBase.GetPotion,
                        ItemBase.GetPotion, 
                        ItemBase.GetPotion, 
                        ItemBase.GetPotion, 
                        ItemBase.GetPotion, 
                        ItemBase.GetPotion, 
                        ItemBase.GetPotion, 
                        ItemBase.GetPotion, 
                        ItemBase.GetPotion, 
                        ItemBase.GetPotion, 
                    };
                    return m;
                }
            }

            public static MechEngine.Merchant DrowMiller
            {
                get
                {
                    MechEngine.Merchant m = new MechEngine.Merchant();
                    m.Icon = '$';
                    m.MaxGold = 5000;
                    m.SpeachColor = ConsoleColor.DarkGray;
                    m.SpeachIcon = '╦';
                    ;
                    m.CurGold = r.Next(2500);
                    m.Color = ConsoleColor.Yellow;
                    m.Name = "Миллер - Дроу торговец";
                    m.Goods = new List<MechEngine.Item>() 
                    { 
                        ItemBase.GetScroll, 
                        ItemBase.GetScroll, 
                        ItemBase.GetScroll, 
                        ItemBase.GetScroll, 
                        ItemBase.GetScroll, 
                        ItemBase.GetScroll, 
                        ItemBase.GetScroll, 
                        ItemBase.GetScroll, 
                        ItemBase.GetScroll, 
                        ItemBase.GetScroll, 
                        ItemBase.GetScroll, 
                        ItemBase.GetScroll, 
                    };
                    return m;
                }
            }

            public static MechEngine.Merchant MiniShop
            {
                get
                {
                    MechEngine.Merchant m = new MechEngine.Merchant();
                    m.Icon = '$';
                    m.MaxGold = 100;
                    m.SpeachColor = ConsoleColor.DarkYellow;
                    m.SpeachIcon = '☼';
                    ;
                    m.CurGold = r.Next(25);
                    m.Color = ConsoleColor.Yellow;
                    m.Name = "Механизм - Торговец";
                    m.Goods = new List<MechEngine.Item>() 
                    { 
                        ItemBase.GetItem, 
                        ItemBase.GetItem, 
                        ItemBase.GetItem, 
                        ItemBase.GetItem, 
                        ItemBase.GetItem, 
                        ItemBase.GetItem, 
                        ItemBase.GetItem, 
                        ItemBase.GetItem, 
                        ItemBase.GetItem, 
                        ItemBase.GetItem, 
                        ItemBase.GetItem, 
                        ItemBase.GetItem, 
                    };
                    return m;
                }
            }
        }

        public static class QuestGiversBase
        {
            public static MechEngine.Questgiver GetQuestGiver
            {
                get
                {
                    Random q = new Random();
                    Thread.Sleep(10);
                    int qr = q.Next(3);
                    switch (qr)
                    {
                        case 0: { return Spiritable; }
                        case 1: { return AdventurerLooser; }
                        case 2: { return SacredWarrior; }
                        default: { return Spiritable; }
                    }
                }
            }

            private static MechEngine.Questgiver Spiritable
            {
                get
                {
                    MechEngine.Questgiver qG = new MechEngine.Questgiver();
                    qG.Name = "Спиритал";
                    qG.SpeachColor = ConsoleColor.Cyan;
                    qG.SpeachIcon = 'Y';
                    qG.Icon = '!';
                    qG.Color = ConsoleColor.Red;
                    qG.Quest = QuestBase.GetQuest;              
                    return qG;                    
                }
            }

            private static MechEngine.Questgiver AdventurerLooser
            {
                get
                {
                    MechEngine.Questgiver qg = new MechEngine.Questgiver();
                    qg.Name = "Герой неудачник";
                    qg.SpeachColor= ConsoleColor.DarkGray;
                    qg.SpeachIcon='a';
                    qg.Icon = '!';
                    qg.Color = ConsoleColor.Red;                    
                    qg.Quest = QuestBase.GetQuest;
                    return qg;
                }
            }

            private static MechEngine.Questgiver SacredWarrior
            {
                get
                {
                    MechEngine.Questgiver qg = new MechEngine.Questgiver();
                    qg.Name = "Неизвестный воин";
                    qg.SpeachColor = ConsoleColor.Magenta;
                    qg.SpeachIcon = '╦';
                    qg.Icon = '!';
                    qg.Color = ConsoleColor.Red;
                    qg.Quest = QuestBase.GetQuest;
                    return qg;
                }
            }
        }

        public static class QuestBase
        {
            public static MechEngine.Quest GetQuest
            {
                get
                {
                    ;
                    Thread.Sleep(10);
                    int q=r.Next(6);
                    switch (q)
                    {
                        case 0: { return RatHero; }
                        case 1: { return AzraiGenocide; }
                        case 2: { return DrowGenocide; }
                        case 3: { return AirDestroy; }
                        case 4: { return AirSacrifice; }
                        case 5: { return GoldTaker; }
                        default: { return RatHero; }
                    }
                }
            }

            private static MechEngine.Quest RatHero
            {
                get
                {
                    MechEngine.Quest q = new MechEngine.Quest();
                    q.Name = "Крысиный герой";
                    q.Target = "Уничтожить 5 крыс.";
                    q.Speach = "Привет, " + Rogue.RAM.Player.Name + "! Поможешь мне убить 5 крыс? Для меня это непосильная задача!!!";
                    q.TargetCount = 5;
                    q.M = new List<MechEngine.Monster>() { MobBase.Rat, MobBase.Rat, MobBase.Rat, MobBase.Rat, MobBase.Rat };
                    q.Difficult = ConsoleColor.Green;
                    q.Icon = 'R';
                    q.Color = ConsoleColor.Yellow;
                    MechEngine.Reward r = new MechEngine.Reward()
                    {
                        Exp = new List<int>() { Rogue.RAM.Player.Level*2 },
                        Gold = new List<int>() { 247 },
                        Items = new List<MechEngine.Item>() { ItemBase.RatTail },
                        Abilityes = new List<MechEngine.Ability>() { },
                        Perks = new List<MechEngine.Perk> { }
                    };
                    q.Rewards = r;
                    q.Progress = 0;
                    return q;
                }
            }

            private static MechEngine.Quest AzraiGenocide
            {
                get
                {
                    MechEngine.Quest q = new MechEngine.Quest();
                    q.Name = "Истребить эльфов";
                    q.Target = "Уничтожить " + Rogue.RAM.Map.Level.ToString() + " эльфов.";
                    q.Speach = "Привет, я местный охотник на ушастых. Если поможешь убить мне "+Rogue.RAM.Map.Level.ToString()+" таких, я отправлю тебе по почте подарок.";
                    q.TargetCount = Rogue.RAM.Map.Level;
                    q.M = new List<MechEngine.Monster>();
                    for (int i = 0; i < Rogue.RAM.Map.Level; i++)
                    {
                        q.M.Add(MobBase.AzraiWarrior);
                    }
                    q.Difficult = ConsoleColor.Yellow;
                    q.Icon = 'E';
                    q.Color = ConsoleColor.Green;
                    int val = Convert.ToInt32(Rogue.RAM.Map.Level / 2);
                    if (val <= 0) { val = 1; }
                    MechEngine.Reward r = new MechEngine.Reward()
                    {
                        Exp = new List<int>() { Rogue.RAM.Player.Level },
                        Perks = new List<MechEngine.Perk>() 
                        {
                            new MechEngine.Perk()
                            {
                              Name="Убийца эльфов ("+Rogue.RAM.Map.Level+")",
                              History="Награда за истребление ушастых.",
                              Icon='E',
                              Color= ConsoleColor.Green,
                              Bonus=new List<MechEngine.PerkStat>()
                                {
                                    new MechEngine.PerkStat(){ Stat= MechEngine.AbilityStats.MRS, Value=val},
                                    new MechEngine.PerkStat(){ Stat= MechEngine.AbilityStats.AD, Value=val}
                                }
                            }
                        }
                    };
                    q.Rewards = r;
                    q.Progress = 0;
                    return q;
                }
            }

            private static MechEngine.Quest DrowGenocide
            {
                get
                {
                    MechEngine.Quest q = new MechEngine.Quest();
                    q.Name = "Истребить дроу";
                    q.Target = "Уничтожить " + Rogue.RAM.Map.Level.ToString() + " теней дроу.";
                    q.Speach = "Быстрее! Тени дроу уже приближаются! Я не могу больше их держать в заперти!";
                    q.TargetCount = Rogue.RAM.Map.Level;
                    q.M = new List<MechEngine.Monster>();
                    for (int i = 0; i < Rogue.RAM.Map.Level; i++)
                    {
                        q.M.Add(MobBase.Shadow);
                    }
                    q.Difficult = ConsoleColor.Red;
                    q.Icon = 'D';
                    q.Color = ConsoleColor.Magenta;
                    int val = Convert.ToInt32(Rogue.RAM.Map.Level / 2);
                    if (val <= 0) { val = 1; }
                    MechEngine.Reward r = new MechEngine.Reward()
                    {
                        Exp = new List<int>() { Rogue.RAM.Player.Level },
                        Perks = new List<MechEngine.Perk>() 
                        {
                            new MechEngine.Perk()
                            {
                              Name="Убийца дроу ("+Rogue.RAM.Map.Level+")",
                              History="Награда за истребление тёмных.",
                              Icon='D',
                              Color= ConsoleColor.Magenta,
                              Bonus=new List<MechEngine.PerkStat>()
                                {
                                    new MechEngine.PerkStat(){ Stat= MechEngine.AbilityStats.AP, Value=val},
                                    new MechEngine.PerkStat(){ Stat= MechEngine.AbilityStats.MMP, Value=val}
                                },
                              Penalty=new List<MechEngine.PerkStat>()
                                {
                                    new MechEngine.PerkStat(){ Stat= MechEngine.AbilityStats.MHP, Value=val},
                                    new MechEngine.PerkStat(){ Stat= MechEngine.AbilityStats.AD, Value=val}
                                }
                            }
                        }
                    };
                    q.Rewards = r;
                    q.Progress = 0;
                    return q;
                }
            }

            private static MechEngine.Quest AirDestroy
            {
                get
                {
                    MechEngine.Quest q = new MechEngine.Quest();
                    q.Name = "Уничтожитель воздуха";
                    q.Target = "Уничтожить "+Rogue.RAM.Player.Level*2+" воздушных духов.";
                    q.Speach = "Привет, " + Rogue.RAM.Player.Name + "! Я охочусь на духов воздуха, помоги мне и убей "+Rogue.RAM.Player.Level*2+" штук?";
                    q.TargetCount = Rogue.RAM.Player.Level * 2;
                    for (int i = 0; i < Rogue.RAM.Player.Level * 2; i++)
                    { q.M.Add(MobBase.AirSpirit); }
                    q.Difficult = ConsoleColor.Green;
                    q.Icon = 'A';
                    q.Color = ConsoleColor.White;
                    MechEngine.Reward r = new MechEngine.Reward()
                    {
                        Exp = new List<int>() { Rogue.RAM.Player.Level * 2 },
                        Gold = new List<int>() { Convert.ToInt32(Rogue.RAM.Map.Level / 2) },
                        Items = new List<MechEngine.Item>() { ItemBase.AirHalo },
                        Abilityes = new List<MechEngine.Ability>() { },
                        Perks = new List<MechEngine.Perk> { }
                    };
                    q.Rewards = r;
                    q.Progress = 0;
                    return q;
                }
            }

            private static MechEngine.Quest AirSacrifice
            {
                get
                {
                    MechEngine.Quest q = new MechEngine.Quest();
                    q.Name = "Жертвоприношение воздуху";
                    q.Target = "Собрать 6 Воздушных нимбов";
                    q.Speach = "Я хочу принести жертву духу воздуха, " + Rogue.RAM.Player.Name + "! Если достанешь 6 Воздушных нимбов, получишь награду...";
                    q.TargetCount = 6;
                    for (int i = 0; i < 6; i++)
                    { q.I.Add(ItemBase.AirHalo); }
                    q.Difficult = ConsoleColor.Yellow;
                    q.Icon = 'S';
                    q.Color = ConsoleColor.Red;
                    MechEngine.Reward r = new MechEngine.Reward()
                    {
                        Exp = new List<int>() { 27 },
                        Gold = new List<int>() { 12 },
                        Items = new List<MechEngine.Item>() { },
                        Abilityes = new List<MechEngine.Ability>() { },
                        Perks = new List<MechEngine.Perk>()
                        {
                            new MechEngine.Perk()
                            {
                               AD=1, 
                               AP=1, 
                               Bonus=new List<MechEngine.PerkStat>()
                               {
                                   new MechEngine.PerkStat()
                                   { 
                                       Stat= MechEngine.AbilityStats.AP,
                                       Value=1
                                   }, 
                                   new MechEngine.PerkStat()
                                   {
                                       Stat= MechEngine.AbilityStats.AD,
                                       Value=1
                                   }
                               },
                               Penalty=new List<MechEngine.PerkStat>(),
                               Color= ConsoleColor.Red,
                               Icon='S',
                               History="Плоды жертвы",
                               Name="Жертвоприношение"
                            }
                        }
                    };
                    q.Rewards = r;
                    q.Progress = 0;
                    return q;
                }
            }

            private static MechEngine.Quest GoldTaker
            {
                get
                {
                    MechEngine.Quest q = new MechEngine.Quest();
                    q.Name = "Собиратель золота";
                    q.Target = "Собрать " + Rogue.RAM.Player.Level * 1000 + " золота.";
                    q.Speach = "Смертный! Сейчас же принеси мне " + Rogue.RAM.Player.Level * 1000 + " или вечное проклятье падёт на тебя! Ускоряйся!!!";
                    q.TargetCount = Rogue.RAM.Player.Level * 1000;
                    q.G = Rogue.RAM.Player.Level * 1000;
                    q.Difficult = ConsoleColor.Yellow;
                    q.Icon = '$';
                    q.Color = ConsoleColor.Yellow;
                    MechEngine.Reward r = new MechEngine.Reward()
                    {
                        Exp = new List<int>() { Rogue.RAM.Player.Level * 8 },
                        Gold = new List<int>() { },
                        Items = new List<MechEngine.Item>() { ItemBase.GoldRod },
                        Abilityes = new List<MechEngine.Ability>() { },
                        Perks = new List<MechEngine.Perk> { }
                    };
                    q.Rewards = r;
                    q.Progress = 0;
                    return q;
                }
            }

            public static MechEngine.Quest __MAIN_QUEST
            {
                get
                {
                    MechEngine.Quest q = new MechEngine.Quest();
                    q.Name = "Остановить разрушение";
                    q.Target = "Уничтожить Валорана";
                    q.Speach = "";
                    q.TargetCount = 1;
                    q.M.Add(MobBase.Valoran);
                    q.Difficult = ConsoleColor.DarkMagenta;
                    q.Icon = '♀';
                    q.Color = ConsoleColor.DarkMagenta;
                    MechEngine.Reward r = new MechEngine.Reward();
                    q.Rewards = r;
                    q.Progress = 0;
                    return q;
                }
            }
            /// <summary>
            /// No playable characters quests for elite classes
            /// </summary>
            public static class NPCQuestFEC
            {
                public static MechEngine.Quest ThreeOrb
                {
                    get
                    {
                        MechEngine.Quest q = new MechEngine.Quest();
                        q.Name = "Сферы Валькирий";
                        q.Target = "Собрать: Сферу Силы, Сферу Мудрости, Сферу ловкости.";
                        q.Difficult = ConsoleColor.Yellow;
                        q.TargetCount = 3;
                        q.Icon = 'O';
                        q.Color = ConsoleColor.Red;
                        q.Progress = 0;
                        return q;
                    }
                }

                public static MechEngine.Quest ColdOrb
                {
                    get
                    {
                        MechEngine.Quest q = new MechEngine.Quest();
                        q.Name = "Сфера сопряжения";
                        q.Target = "Найти <Труп Валькирии> и создать <Сферу сопряжения>.";
                        q.Difficult = ConsoleColor.Red;
                        q.TargetCount = 1;
                        q.Icon = '☼';
                        q.Color = ConsoleColor.Cyan;
                        q.Progress = 0;
                        return q;
                    }
                }

                public static MechEngine.Quest KillBoss
                {
                    get
                    {
                        MechEngine.Quest q = new MechEngine.Quest();
                        q.Name = "Предотвратить";
                        q.M.Add(MobBase.GetBoss);                        
                        q.Target = "Найти <"+q.M[0].Name+"> и убить его.";
                        q.Difficult = ConsoleColor.Red;
                        q.TargetCount = 1;
                        q.Icon = q.M[0].Icon;
                        q.Color = q.M[0].Chest;
                        q.Progress = 0;
                        return q;
                    }
                }

                public static MechEngine.Quest StayValkyrie
                {
                    get
                    {
                        MechEngine.Quest q = new MechEngine.Quest();
                        q.Name = "Валькирия";
                        q.Target = "Отнести <Сферу сопряжения> Бас.";
                        q.Difficult = ConsoleColor.Red;
                        q.TargetCount = 1;
                        q.Icon = '@';
                        q.Color = ConsoleColor.Cyan;
                        q.Progress = 0;
                        return q;
                    }
                }

                public static MechEngine.Quest KillNestori
                {
                    get
                    {
                        MechEngine.Quest q = new MechEngine.Quest();
                        q.Name = "Убить Нестори";
                        q.Target = "Найти и убить Нестори на 5 уровне подземелья.";
                        q.Difficult = ConsoleColor.Yellow;
                        q.TargetCount = 1;
                        q.Icon = 'N';
                        q.M.Add(MobBase.Valoran);
                        q.Color = ConsoleColor.Magenta;
                        q.Progress = 0;
                        return q;
                    }
                }

                public static MechEngine.Quest FiveArguments
                {
                    get
                    {
                        MechEngine.Quest q = new MechEngine.Quest();
                        q.Name = "5 Причин";
                        q.Target = "Попробовать 5 способов убить Валери.";
                        q.Difficult = ConsoleColor.Green;
                        q.TargetCount = 5;
                        q.Icon = '5';
                        q.Color = ConsoleColor.Red;
                        q.Progress = 0;
                        return q;
                    }
                }

                public static MechEngine.Quest UndeadRisingOne
                {
                    get
                    {
                        MechEngine.Quest q = new MechEngine.Quest();
                        q.Name = "Опыты - ч.1";
                        q.Target = "Уничтожить 20 подопытных";
                        q.Difficult = ConsoleColor.Gray;
                        q.TargetCount = 20;
                        q.Icon = '†';
                        q.Color = ConsoleColor.Red;
                        q.Progress = 0;
                        return q;
                    }
                }
                public static MechEngine.Quest UndeadRisingTwo
                {
                    get
                    {
                        MechEngine.Quest q = new MechEngine.Quest();
                        q.Name = "Опыты - ч.2";
                        q.Target = "Уничтожить 28 подопытных";
                        q.Difficult = ConsoleColor.Green;
                        q.TargetCount = 28;
                        q.Icon = '╫';
                        q.Color = ConsoleColor.DarkRed;
                        q.Progress = 0;
                        return q;
                    }
                }
                public static MechEngine.Quest UndeadRisingThree
                {
                    get
                    {
                        MechEngine.Quest q = new MechEngine.Quest();
                        q.Name = "Опыты - ч.3";
                        q.Target = "Уничтожить 52 подопытных";
                        q.Difficult = ConsoleColor.Yellow;
                        q.TargetCount = 52;
                        q.Icon = '‡';
                        q.Color = ConsoleColor.DarkYellow;
                        q.Progress = 0;
                        return q;
                    }
                }
                public static MechEngine.Quest UndeadRisingFour
                {
                    get
                    {
                        MechEngine.Quest q = new MechEngine.Quest();
                        q.Name = "Опыты - ч.4";
                        q.Target = "Уничтожить 76 подопытных";
                        q.Difficult = ConsoleColor.Red;
                        q.TargetCount = 76;
                        q.Icon = '⌂';
                        q.Color = ConsoleColor.Green;
                        q.Progress = 0;
                        return q;
                    }
                }

                public static MechEngine.Quest IronToDiamond
                {
                    get
                    {
                        MechEngine.Quest q = new MechEngine.Quest();
                        q.Name = "Железо в Алмаз";
                        q.Target = "Превратить железо в алмаз.";
                        q.Difficult = ConsoleColor.Yellow;
                        q.TargetCount = 1;
                        q.Icon = '∙';
                        q.Color = ConsoleColor.White;
                        q.Progress = 0;
                        return q;
                    }
                }
                public static MechEngine.Quest WoodToGlass
                {
                    get
                    {
                        MechEngine.Quest q = new MechEngine.Quest();
                        q.Name = "Дерево в стекло";
                        q.Target = "Превратить дерево в стекло";
                        q.Difficult = ConsoleColor.Yellow;
                        q.TargetCount = 1;
                        q.Icon = '▌';
                        q.Color = ConsoleColor.Cyan;
                        q.Progress = 0;
                        return q;
                    }
                }
                public static MechEngine.Quest DeathToLife
                {
                    get
                    {
                        MechEngine.Quest q = new MechEngine.Quest();
                        q.Name = "Смерть в жизнь";
                        q.Target = "Превратить мертвую розу в живую";
                        q.Difficult = ConsoleColor.Yellow;
                        q.TargetCount = 1;
                        q.Icon = '☼';
                        q.Color = ConsoleColor.Red;
                        q.Progress = 0;
                        return q;
                    }
                }
                public static MechEngine.Quest TransformAllToMage
                {
                    get
                    {
                        MechEngine.Quest q = new MechEngine.Quest();
                        q.Name = "Отправить";
                        q.Target = "Отправить трансформированные предметы.";
                        q.Difficult = ConsoleColor.Green;
                        q.TargetCount = 1;
                        q.Icon = 'T';
                        q.Color = ConsoleColor.Blue;
                        q.Progress = 0;
                        return q;
                    }
                }

                public static MechEngine.Quest DeadMonogamy
                {
                    get
                    {
                        MechEngine.Quest q = new MechEngine.Quest();
                        q.Name = "Мертвая верность";
                        q.Target = "Забрать обручальное кольцо у Стефана.";
                        q.Difficult = ConsoleColor.Red;
                        q.TargetCount = 1;
                        q.Icon = 'o';
                        q.Color = ConsoleColor.Yellow;
                        q.Progress = 0;
                        return q;
                    }
                }

                public static MechEngine.Quest DeleteTrash
                {
                    get
                    {
                        MechEngine.Quest q = new MechEngine.Quest();
                        q.Name = "Гнилые кости";
                        q.Target = "Убрать 6 куч мусора.";
                        q.Difficult = ConsoleColor.Blue;
                        q.TargetCount = 6;
                        for (int i = 0; i < 6; i++) { q.I.Add(ResourseBase.Trash); }
                        q.Icon = '╕';
                        q.Color = ConsoleColor.Green;
                        q.Progress = 0;
                        return q;
                    }
                }

                public static MechEngine.Quest MouseDoor
                {
                    get
                    {
                        MechEngine.Quest q = new MechEngine.Quest();
                        q.Name = "Дыра";
                        q.Target = "Починить дверь";
                        q.Difficult = ConsoleColor.Green;
                        q.TargetCount = 3;
                        q.Icon = '#';
                        q.Color = ConsoleColor.DarkGray;
                        q.Progress = 0;
                        return q;
                    }
                }
            }
        }

        public static class ItemBase
        {
            public static MechEngine.Item GetPotion
            {
                get
                {
                    Thread.Sleep(30);
                    return Potion;
                }
            }

            public static MechEngine.Item GetItem
            {
                get
                {
                    if (r.Next(2) == 0) { return ResourseBase.Random; }
                    else
                    {
                        MechEngine.Item rtrn = new MechEngine.Item();
                        switch (r.Next(9))
                        {
                            case 0:
                                {
                                    rtrn = GetPotion;
                                    break;
                                }
                            case 1:
                                {
                                    rtrn = SimplySword;
                                    break;
                                }
                            case 2:
                                {
                                    rtrn = SimplyStuff;
                                    break;
                                }
                            case 3:
                                {
                                    rtrn = SimplyPlate;
                                    break;
                                }
                            case 4:
                                {
                                    rtrn = SimplyCloth;
                                    break;
                                }
                            case 5:
                                {
                                    rtrn = SimplyHelm;
                                    break;
                                }
                            case 6:
                                {
                                    rtrn = SimplyBoots;
                                    break;
                                }
                            case 7:
                                {
                                    rtrn = SimplyShield;
                                    break;
                                }
                            case 8:
                                {
                                    rtrn = GetScroll;
                                    break;
                                }
                        }
                        return rtrn;
                    }
                }
            }

            public static MechEngine.Item GetScroll
            {
                get
                {
                    MechEngine.Item rtrn = new MechEngine.Item();
                    Thread.Sleep(30);
                    switch (r.Next(5))
                    {
                        case 0: { rtrn = ScrollummonSceleton; break; }
                        case 1: { rtrn = ScrollPosionNova; break; }
                        case 2: { rtrn = ScrollHolyLight; break; }
                        case 3: { rtrn = ScrollVamp; break; }
                        case 4: { rtrn = ScrollInfernoBlast; break; }
                        default: { rtrn = ScrollHolyLight; break; }
                    }
                    return rtrn;
                }
            }

            public static MechEngine.Item GetItemLoot()
            {
                MechEngine.Item rtrn = new MechEngine.Item();
                ;
                switch (r.Next(10))
                {
                    case 0:
                        {
                            rtrn = HealthPotion;
                            break;
                        }
                    case 1:
                        {
                            rtrn = SimplySword;
                            break;
                        }
                    case 2:
                        {
                            rtrn = SimplyStuff;
                            break;
                        }
                    case 3:
                        {
                            rtrn = SimplyPlate;
                            break;
                        }
                    case 4:
                        {
                            rtrn = SimplyCloth;
                            break;
                        }
                    case 5:
                        {
                            rtrn = IronKey;
                            break;
                        }
                    case 6:
                        {
                            rtrn = MagicKey;
                            break;
                        }
                    case 7:
                        {
                            rtrn = SimplyHelm;
                            break;
                        }
                    case 8:
                        {
                            rtrn = SimplyBoots;
                            break;
                        }
                    case 9:
                        {
                            rtrn = SimplyShield;
                            break;
                        }
                }

                return rtrn;
            }

            public static MechEngine.Item.Potion HealthPotion
            {
                get
                {
                    MechEngine.Item.Potion I = new MechEngine.Item.Potion();
                    ;
                    I.Name = "Зелье здоровья";
                    I.Kind = MechEngine.Kind.Potion;
                    I.Rare = GetRandomRareStats;
                    I.HP = BufferStat * 5;
                    I.ILvl = BufferIlvl;
                    return I;
                }
            }

            public static MechEngine.Item.Potion ManaPotion
            {
                get
                {
                    MechEngine.Item.Potion I = new MechEngine.Item.Potion();
                    ;
                    I.Name = "Зелье желания";
                    I.Kind = MechEngine.Kind.Potion;
                    I.Rare = GetRandomRareStats;
                    I.MP = BufferStat * 5;
                    I.ILvl = BufferIlvl;
                    return I;
                }
            }

            public static MechEngine.Item.Potion Potion
            {
                get
                {                    
                    MechEngine.Item.Potion I = new MechEngine.Item.Potion();
                    ;
                    int q = r.Next(2);
                    if (q == 1) { I.Name = "Зелье здоровья"; } else { I.Name = "Зелье манны"; }
                    I.Kind = MechEngine.Kind.Potion;
                    I.Rare = GetRandomRareStats;
                    if (BufferStat <= 0) { BufferStat = 1; }
                    if (q == 1) { I.HP = BufferStat * 5; } else { I.MP = BufferStat * 5; }
                    I.ILvl = BufferIlvl;
                    return I;
                }
            }            

            public static MechEngine.Item.Helm SimplyHelm
            {
                get
                {
                    MechEngine.Item.Helm I = new MechEngine.Item.Helm();
                    I.Name = "Простой шлем";
                    I.Kind = MechEngine.Kind.Helm;
                    I.Rare = GetRandomRareStats;
                    for (int i = 0; i < HowMuchStat; i++)
                    {
                        switch (r.Next(4))
                        {
                            case 0: { I.AD = BufferStat; break; }
                            case 1: { I.AP = Convert.ToInt32(BufferStat * 1.5); break; }
                            case 2: { I.HP = BufferStat; break; }
                            case 3: { I.MP = BufferStat; break; }
                        }
                    }
                    I.ILvl = BufferIlvl;
                    return I;
                }
            }

            public static MechEngine.Item.Boots SimplyBoots
            {
                get
                {
                    MechEngine.Item.Boots I = new MechEngine.Item.Boots();
                    I.Name = "Простые ботинки";
                    I.Kind = MechEngine.Kind.Boots;
                    I.Rare = GetRandomRareStats;
                    for (int i = 0; i < HowMuchStat; i++)
                    {
                        switch (r.Next(2))
                        {
                            case 0: { I.ARM = BufferStat; break; }
                            case 1: { I.MRS = BufferStat; break; }
                        }
                    }
                    I.ILvl = BufferIlvl;
                    return I;
                }
            }

            public static MechEngine.Item.Helm AirHalo
            {
                get
                {
                    MechEngine.Item.Helm i = new MechEngine.Item.Helm();
                    i.Name = "Воздушный нимб";
                    i.Kind = MechEngine.Kind.Helm;
                    i.Rare = MechEngine.Rarity.Legendary;
                    i.Color = ConsoleColor.Cyan;
                    i.AD = Rogue.RAM.Player.Level * 2;
                    i.AP = Rogue.RAM.Player.Level * 3;
                    i.HP = 1;
                    i.MP = 1;
                    i.ILvl = 0;
                    return i;
                }
            }

            public static MechEngine.Item.OffHand RatTail
            {
                get
                {
                    MechEngine.Item.OffHand i = new MechEngine.Item.OffHand();
                    i.Name="Крысий хвост";
                    i.Kind= MechEngine.Kind.OffHand;
                    i.Rare= MechEngine.Rarity.Set;
                    i.ARM=1;
                    i.MRS=1;
                    i.ILvl=0;
                    return i;
                }
            }

            public static MechEngine.Item.Weapon SimplySword
            {
                get
                {
                    MechEngine.Item.Weapon I = new MechEngine.Item.Weapon();
                    I.Name = "Меч стражи";
                    I.Kind = MechEngine.Kind.Weapon;
                    I.Rare = GetRandomRareStats;
                    for (int i = 0; i < HowMuchStat; i++)
                    {
                        switch (r.Next(5))
                        {
                            case 0: { I.AD = BufferStat; break; }
                            case 1: { I.AP = Convert.ToInt32(BufferStat * 1.5); break; }
                            case 2: { I.ARM = BufferStat; break; }
                            case 3: { I.MADMG = BufferStat; break; }
                            case 4: { I.MIDMG = BufferStat; break; }
                        }
                    }
                    I.ILvl = BufferIlvl;
                    return I;
                }
            }

            public static MechEngine.Item.Weapon SimplyStuff
            {
                get
                {
                    MechEngine.Item.Weapon I = new MechEngine.Item.Weapon();
                    I.Name = "Боевой посох";
                    I.Staff = true;
                    I.Kind = MechEngine.Kind.Weapon;
                    I.Rare = GetRandomRareStats;
                    for (int i = 0; i < HowMuchStat; i++)
                    {
                        switch (r.Next(5))
                        {
                            case 0: { I.AD = BufferStat; break; }
                            case 1: { I.AP = Convert.ToInt32(BufferStat * 1.5); break; }
                            case 2: { I.ARM = BufferStat; break; }
                            case 3: { I.MADMG = BufferStat; break; }
                            case 4: { I.MIDMG = BufferStat; break; }
                        }
                    }
                    I.ILvl = BufferIlvl;
                    return I;
                }
            }

            public static MechEngine.Item.OffHand SimplyShield
            {
                get
                {
                    MechEngine.Item.OffHand I = new MechEngine.Item.OffHand();
                    I.Name = "Старый щит";
                    I.Kind = MechEngine.Kind.OffHand;
                    I.Rare = GetRandomRareStats;
                    for (int i = 0; i < HowMuchStat; i++)
                    {
                        switch (r.Next(6))
                        {
                            case 0: { I.AD = BufferStat; break; }
                            case 1: { I.AP = Convert.ToInt32(BufferStat * 1.5); break; }
                            case 2: { I.ARM = BufferStat; break; }
                            case 3: { I.MADMG = BufferStat; break; }
                            case 4: { I.MIDMG = BufferStat; break; }
                            case 5: { I.MRS = BufferStat; break; }
                        }
                    }
                    I.ILvl = BufferIlvl;
                    return I;
                }
            }

            public static MechEngine.Item.Scroll ScrollummonSceleton
            {
                get
                {
                    MechEngine.Item.Scroll s = new MechEngine.Item.Scroll();
                    s.Name = "Свиток призыва скелета (" + Rogue.RAM.Map.Level + ")";
                    s.ILvl = 0;
                    s.Rare = MechEngine.Rarity.Epic;
                    s.Kind = MechEngine.Kind.Scroll;                    
                    s.Spell = DataBase.BattleAbilityBase.SummonSceleton();
                    s.Spell.Scroll = true;
                    for (int i = 0; i < Rogue.RAM.Map.Level; i++)
                    {
                        s.Spell.UP();
                    }                    
                    return s;
                }
            }

            public static MechEngine.Item.Scroll ScrollPosionNova
            {
                get
                {
                    MechEngine.Item.Scroll s = new MechEngine.Item.Scroll();
                    s.Name = "Свиток 'Нова яда' (" + Rogue.RAM.Map.Level + ")";
                    s.ILvl = 0;
                    s.Rare = MechEngine.Rarity.Set;
                    s.Kind = MechEngine.Kind.Scroll;
                    s.Spell = DataBase.BattleAbilityBase.PoisonNova();
                    s.Spell.Scroll = true;
                    for (int i = 0; i < Rogue.RAM.Map.Level; i++)
                    {
                        s.Spell.UP();
                    }
                    return s;
                }
            }

            public static MechEngine.Item.Scroll ScrollHolyLight
            {
                get
                {
                    MechEngine.Item.Scroll s = new MechEngine.Item.Scroll();
                    s.Name = "Свиток лечения (" + Rogue.RAM.Map.Level + ")";
                    s.ILvl = 0;
                    s.Rare = MechEngine.Rarity.Rare;
                    s.Kind = MechEngine.Kind.Scroll;
                    s.Spell = DataBase.BattleAbilityBase.HolyLight();
                    s.Spell.Scroll = true;
                    for (int i = 0; i < Rogue.RAM.Map.Level; i++)
                    {
                        s.Spell.UP();
                    }
                    return s;
                }
            }

            public static MechEngine.Item.Scroll ScrollVamp
            {
                get
                {
                    MechEngine.Item.Scroll s = new MechEngine.Item.Scroll();
                    s.Name = "Свиток крови (" + Rogue.RAM.Map.Level + ")";
                    s.ILvl = 0;
                    s.Rare = MechEngine.Rarity.Watered;
                    s.Kind = MechEngine.Kind.Scroll;
                    s.Spell = DataBase.BattleAbilityBase.Vampirism();
                    s.Spell.Scroll = true;
                    for (int i = 0; i < Rogue.RAM.Map.Level; i++)
                    {
                        s.Spell.UP();
                    }
                    return s;
                }
            }

            public static MechEngine.Item.TownPortal AmuletOfMraumir
            {
                get
                {
                    MechEngine.Item.TownPortal tp = new MechEngine.Item.TownPortal();
                    tp.Name = "Камень Мраумира";
                    return tp;
                }
            }

            public static MechEngine.Item.TownPortal.TownScroll ScrollOfMraumir
            {
                get
                {
                    MechEngine.Item.TownPortal.TownScroll tp = new MechEngine.Item.TownPortal.TownScroll();
                    tp.Name = "Свиток Мраумира";
                    return tp;
                }
            }

            public static MechEngine.Item.Scroll ScrollInfernoBlast
            {
                get
                {
                    MechEngine.Item.Scroll s = new MechEngine.Item.Scroll();
                    s.Name = "Свиток 'Ад и Погибель' (" + Rogue.RAM.Map.Level + ")";
                    s.ILvl = 0;
                    s.Rare = MechEngine.Rarity.Fired;
                    s.Kind = MechEngine.Kind.Scroll;
                    s.Spell = DataBase.BattleAbilityBase.InfernoBlast();
                    s.Spell.Scroll = true;
                    for (int i = 0; i < Rogue.RAM.Map.Level; i++)
                    {
                        s.Spell.UP();
                    }
                    return s;
                }
            }

            public static MechEngine.Item.Armor SimplyPlate
            {
                get
                {
                    MechEngine.Item.Armor I = new MechEngine.Item.Armor();
                    I.Name = "Простые латы";
                    I.Kind = MechEngine.Kind.Armor;
                    I.Rare = GetRandomRareStats;
                    for (int i = 0; i < HowMuchStat; i++)
                    {
                        switch (r.Next(4))
                        {
                            case 0: { I.ARM = BufferStat; break; }
                            case 1: { I.HP = BufferStat; break; }
                            case 2: { I.MP = BufferStat; break; }
                            case 3: { I.MRS = BufferStat; break; }
                        }
                    }
                    I.ILvl = BufferIlvl;
                    return I;
                }
            }

            public static MechEngine.Item.Armor SimplyCloth
            {
                get
                {
                    MechEngine.Item.Armor I = new MechEngine.Item.Armor();
                    I.Name = "Простые одежды";
                    I.Kind = MechEngine.Kind.Armor;
                    I.Rare = GetRandomRareStats;
                    for (int i = 0; i < HowMuchStat; i++)
                    {
                        switch (r.Next(4))
                        {
                            case 0: { I.ARM = BufferStat; break; }
                            case 1: { I.HP = BufferStat; break; }
                            case 2: { I.MP = BufferStat; break; }
                            case 3: { I.MRS = BufferStat; break; }
                        }
                    }
                    I.ILvl = BufferIlvl;
                    return I;
                }
            }

            public static MechEngine.Item.Key IronKey
            {
                get
                {
                    MechEngine.Item.Key I = new MechEngine.Item.Key();
                    I.Name = "Железный ключ";
                    I.Kind = MechEngine.Kind.Key;
                    I.Rare = MechEngine.Rarity.Common;
                    I.ILvl = 0;
                    return I;
                }
            }

            public static MechEngine.Item.Key MagicKey
            {
                get
                {
                    MechEngine.Item.Key I = new MechEngine.Item.Key();
                    I.Name = "Магический ключ";
                    I.Kind = MechEngine.Kind.Key;
                    I.Rare = MechEngine.Rarity.Epic;
                    I.ILvl = 0;
                    return I;
                }
            }

            public static MechEngine.Item.Weapon GoldRod
            {
                get
                {
                    MechEngine.Item.Weapon i = new MechEngine.Item.Weapon();
                    i.Name = "Золотой жезл";
                    i.Staff = true;
                    i.Kind = MechEngine.Kind.Weapon;
                    i.Rare = MechEngine.Rarity.Legendary;
                    i.MIDMG = 20;
                    i.MADMG = 40;
                    i.ARM = 30;
                    i.ILvl = Rogue.RAM.Player.Level;
                    return i;
            }
            }

            public static MechEngine.Item.Weapon FireStaff
            {
                get
                {
                    MechEngine.Item.Weapon I = new MechEngine.Item.Weapon();
                    I.Name = "Посох из огня";
                    I.Kind = MechEngine.Kind.Weapon;
                    I.Rare = MechEngine.Rarity.Fired;
                    I.AP = Convert.ToInt32(Rogue.RAM.Player.AP * 0.25);
                    I.ILvl = 0;
                    return I;
                }
            }

            public static class CraftItemsFromNPC
            {
                public static MechEngine.Item.Weapon LostStaff
                {
                    get
                    {
                        MechEngine.Item.Weapon I = new MechEngine.Item.Weapon();
                        I.Name = "Дряхлый посох";
                        I.Kind = MechEngine.Kind.Weapon;
                        I.Color = ConsoleColor.Gray;
                        I.MIDMG = Convert.ToInt32(Rogue.RAM.Player.Level / 4);
                        I.Staff = true;
                        I.ILvl = 0;
                        return I;
                    }
                }

                public static MechEngine.Item.Weapon JustStaff
                {
                    get
                    {
                        MechEngine.Item.Weapon I = new MechEngine.Item.Weapon();
                        I.Name = "Обычный посох";
                        I.Kind = MechEngine.Kind.Weapon;
                        I.Color = ConsoleColor.Gray;
                        I.AP = Convert.ToInt32(Rogue.RAM.Player.Level / 2) + 2;
                        I.Staff = true;
                        I.ILvl = Rogue.RAM.Player.Level + (Rogue.RAM.Map.Level * 2) + 1 * 5;
                        return I;
                    }
                }

                public static MechEngine.Item.Weapon JustStaffD
                {
                    get
                    {
                        MechEngine.Item.Weapon I = new MechEngine.Item.Weapon();
                        I.Name = "Посох из дуба";
                        I.Kind = MechEngine.Kind.Weapon;
                        I.Color = ConsoleColor.DarkYellow;
                        I.AP = Convert.ToInt32(Rogue.RAM.Player.Level / 2) + 2;
                        I.Staff = true;
                        I.ILvl = Rogue.RAM.Player.Level + (Rogue.RAM.Map.Level * 2) + 1 * 5;
                        return I;
                    }
                }

                public static MechEngine.Item.Weapon RedWoodStaff
                {
                    get
                    {
                        MechEngine.Item.Weapon I = new MechEngine.Item.Weapon();
                        I.Name = "Кровавый посох";
                        I.Kind = MechEngine.Kind.Weapon;
                        I.Color = ConsoleColor.DarkRed;
                        I.AP = Rogue.RAM.Player.Level * 2;
                        I.Staff = true;
                        I.ILvl = Rogue.RAM.Player.Level + (Rogue.RAM.Map.Level * 2) + 1 * 5;
                        return I;
                    }
                }

                public static MechEngine.Item.Weapon DeadStaff
                {
                    get
                    {
                        MechEngine.Item.Weapon I = new MechEngine.Item.Weapon();
                        I.Name = "Посох смерти";
                        I.Kind = MechEngine.Kind.Weapon;
                        I.Color = ConsoleColor.DarkGray;
                        I.AP = Rogue.RAM.Player.Level * 3;
                        I.Staff = true;
                        I.ILvl = Rogue.RAM.Player.Level + (Rogue.RAM.Map.Level * 2) + 1 * 5;
                        return I;
                    }
                }

                public static class Rutger
                {
                    public static MechEngine.Item.Armor RutgerArmor
                    { get { if (r.Next(99) < 25) { return HodarsArmor; } else { return JustArmor; } } }
                    private static MechEngine.Item.Armor JustArmor
                    {
                        get
                        {
                            MechEngine.Item.Armor I = new MechEngine.Item.Armor();
                            I.Name = "Латный доспех";
                            I.Kind = MechEngine.Kind.Armor;
                            I.Rare = GetRandomRareStats;
                            switch (r.Next(4))
                            {
                                case 0: { I.ARM = BufferStat; break; }
                                case 1: { I.HP = BufferStat; break; }
                                case 2: { I.MP = BufferStat; break; }
                                case 3: { I.MRS = BufferStat; break; }
                            }
                            I.ILvl = BufferIlvl;
                            return I;
                        }
                    }
                    private static MechEngine.Item.Armor HodarsArmor
                    {
                        get
                        {
                            MechEngine.Item.Armor I = new MechEngine.Item.Armor();
                            I.BufStat = Convert.ToInt32(Rogue.RAM.Player.Level / 2) + 4;
                            I.BufLvl = Rogue.RAM.Player.Level + (Rogue.RAM.Map.Level * 2) + 1 * 7;
                            I.Name = "Броня Ходара";
                            I.ArmorSet = "Hodar";
                            I.Kind = MechEngine.Kind.Armor;
                            I.Info = "Броня великого война Ходара: 1: AD+" + I.BufStat + ". 2: HP+" + I.BufStat + ". 3: ARM+" + I.BufStat + ", MRS+" + I.BufStat + ". 4: HP+" + (I.BufStat * 2).ToString() + ". 5: Прирост ярости +10 ед.";
                            I.Rare = MechEngine.Rarity.Set;
                            switch (r.Next(4))
                            {
                                case 0: { I.ARM = I.BufStat; break; }
                                case 1: { I.HP = I.BufStat; break; }
                                case 2: { I.MP = I.BufStat; break; }
                                case 3: { I.MRS = I.BufStat; break; }
                            }
                            I.ILvl = I.BufLvl;
                            I.Script = (bool Dress) =>
                                {
                                    int SetLevel = 0;
                                    if (Rogue.RAM.Player.Equipment.Boots.ArmorSet == "Hodar") { SetLevel++; }
                                    if (Rogue.RAM.Player.Equipment.Helm.ArmorSet == "Hodar") { SetLevel++; }
                                    if (Rogue.RAM.Player.Equipment.OffHand.ArmorSet == "Hodar") { SetLevel++; }
                                    if (Rogue.RAM.Player.Equipment.Weapon.ArmorSet == "Hodar") { SetLevel++; }
                                    switch (SetLevel)
                                    {
                                        case 0:
                                            { if (Dress) { Rogue.RAM.Player.AD += I.BufStat; } else { Rogue.RAM.Player.AD -= I.BufStat; } break; }
                                        case 1:
                                            { if (Dress) { Rogue.RAM.Player.CHP += I.BufStat; Rogue.RAM.Player.MHP += I.BufStat; } else { Rogue.RAM.Player.MHP -= I.BufStat; Rogue.RAM.Player.CHP -= I.BufStat; } break; }
                                        case 2:
                                            { if (Dress) { Rogue.RAM.Player.ARM += I.BufStat; Rogue.RAM.Player.MRS += I.BufStat; } else { Rogue.RAM.Player.ARM -= I.BufStat; Rogue.RAM.Player.MRS -= I.BufStat; } break; }
                                        case 3:
                                            { if (Dress) { Rogue.RAM.Player.CHP += I.BufStat * 2; Rogue.RAM.Player.MHP += I.BufStat * 2; } else { Rogue.RAM.Player.MHP -= I.BufStat * 2; Rogue.RAM.Player.CHP -= I.BufStat * 2; } break; }
                                        case 4:
                                            {
                                                if (Dress) { Rogue.RAM.ArmSet.Add(new SystemEngine.ArmorSet() { Name = "Hodar", Active = true }); }
                                                else
                                                {
                                                    SystemEngine.ArmorSet aset = new SystemEngine.ArmorSet();
                                                    foreach (SystemEngine.ArmorSet asset in Rogue.RAM.ArmSet)
                                                    { if (asset.Name == "Hodar") { aset = asset; } }
                                                    Rogue.RAM.ArmSet.Remove(aset);
                                                }
                                                break;
                                            }
                                    }
                                };
                            return I;
                        }
                    }

                    public static MechEngine.Item.Weapon RutgerWeapon
                    { get { if (r.Next(99) < 25) { return ElementalSword; } else { return JustWeapon; } } }
                    private static MechEngine.Item.Weapon JustWeapon
                    {
                        get
                        {
                            MechEngine.Item.Weapon I = new MechEngine.Item.Weapon();
                            I.Name = "Хороший меч";
                            I.Kind = MechEngine.Kind.Weapon;
                            I.Rare = GetRandomRareStats;
                            switch (r.Next(5))
                            {
                                case 0: { I.AD = BufferStat; break; }
                                case 1: { I.AP = Convert.ToInt32(BufferStat * 1.5); break; }
                                case 2: { I.ARM = BufferStat; break; }
                                case 3: { I.MADMG = BufferStat; break; }
                                case 4: { I.MIDMG = BufferStat; break; }
                            }
                            I.ILvl = BufferIlvl;
                            return I;
                        }
                    }
                    private static MechEngine.Item.Weapon ElementalSword
                    {
                        get
                        {
                            MechEngine.Item.Weapon I = new MechEngine.Item.Weapon();
                            I.BufStat = Convert.ToInt32(Rogue.RAM.Player.Level / 2) + 4;
                            I.BufLvl = Rogue.RAM.Player.Level + (Rogue.RAM.Map.Level * 2) + 1 * 7;
                            I.Name = "Меч Элементов";
                            I.Kind = MechEngine.Kind.Weapon;
                            I.ArmorSet = "ElArm";
                            I.Info = "Оружие созданное элементальным путём: 1: DMG↑+" + I.BufStat + ". 2: HP+" + I.BufStat + ". 3: MP+" + I.BufStat + ". 4: MRS+" + (I.BufStat * 2).ToString() + ". 5: 10% Шанс не потратить Элементы.";
                            I.Rare = MechEngine.Rarity.Set;
                            switch (r.Next(5))
                            {
                                case 0: { I.AD = I.BufStat; break; }
                                case 1: { I.AP = Convert.ToInt32(I.BufStat * 1.5); break; }
                                case 2: { I.ARM = I.BufStat; break; }
                                case 3: { I.MADMG = I.BufStat; break; }
                                case 4: { I.MIDMG = I.BufStat; break; }
                            }
                            I.ILvl = I.BufLvl;
                            I.Script = (bool Dress) =>
                            {
                                int SetLevel = 0;
                                if (Rogue.RAM.Player.Equipment.Boots.ArmorSet == "ElArm") { SetLevel++; }
                                if (Rogue.RAM.Player.Equipment.Helm.ArmorSet == "ElArm") { SetLevel++; }
                                if (Rogue.RAM.Player.Equipment.OffHand.ArmorSet == "ElArm") { SetLevel++; }
                                if (Rogue.RAM.Player.Equipment.Weapon.ArmorSet == "ElArm") { SetLevel++; }
                                switch (SetLevel)
                                {
                                    case 0:
                                        { if (Dress) { Rogue.RAM.Player.MADMG += I.BufStat; } else { Rogue.RAM.Player.MADMG -= I.BufStat; } break; }
                                    case 1:
                                        { if (Dress) { Rogue.RAM.Player.CHP += I.BufStat; Rogue.RAM.Player.MHP += I.BufStat; } else { Rogue.RAM.Player.MHP -= I.BufStat; Rogue.RAM.Player.CHP -= I.BufStat; } break; }
                                    case 2:
                                        { if (Dress) { Rogue.RAM.Player.CMP += I.BufStat; Rogue.RAM.Player.MMP += I.BufStat; } else { Rogue.RAM.Player.MMP -= I.BufStat; Rogue.RAM.Player.CMP -= I.BufStat; } break; }
                                    case 3:
                                        { if (Dress) { Rogue.RAM.Player.MRS += I.BufStat; } else { Rogue.RAM.Player.MRS -= I.BufStat; } break; }
                                    case 4:
                                        {
                                            if (Dress) { Rogue.RAM.ArmSet.Add(new SystemEngine.ArmorSet() { Name = "ElArm", Active = true }); }
                                            else
                                            {
                                                SystemEngine.ArmorSet aset = new SystemEngine.ArmorSet();
                                                foreach (SystemEngine.ArmorSet asset in Rogue.RAM.ArmSet)
                                                { if (asset.Name == "ElArm") { aset = asset; } }
                                                Rogue.RAM.ArmSet.Remove(aset);
                                            }
                                            break;
                                        }
                                }
                            };
                            return I;
                        }
                    }

                    public static MechEngine.Item.Helm RutgerHelm
                    { get { if (r.Next(99) < 25) { return HolyHelm; } else { return JustHelm; } } }
                    private static MechEngine.Item.Helm JustHelm
                    {
                        get
                        {
                            MechEngine.Item.Helm I = new MechEngine.Item.Helm();
                            I.Name = "Железный Шлем";
                            I.Kind = MechEngine.Kind.Helm;
                            I.Rare = GetRandomRareStats;
                            switch (r.Next(4))
                            {
                                case 0: { I.AD = BufferStat; break; }
                                case 1: { I.AP = Convert.ToInt32(BufferStat * 1.5); break; }
                                case 2: { I.HP = BufferStat; break; }
                                case 3: { I.MP = BufferStat; break; }
                            }
                            I.ILvl = BufferIlvl;
                            return I;
                        }
                    }
                    private static MechEngine.Item.Helm HolyHelm
                    {
                        get
                        {
                            MechEngine.Item.Helm I = new MechEngine.Item.Helm();
                            I.BufStat = Convert.ToInt32(Rogue.RAM.Player.Level / 2) + 4;
                            I.BufLvl = Rogue.RAM.Player.Level + (Rogue.RAM.Map.Level * 2) + 1 * 7;
                            I.Name = "Шлем Света";
                            I.ArmorSet = "Harm";
                            I.Kind = MechEngine.Kind.Helm;
                            I.Info = "Священный шлем: 1: MP+" + I.BufStat + ". 2: MP+" + (I.BufStat * 2) + ". 3: MP+" + (I.BufStat * 3) + ". 4: Столп Света стоит меньше на 5%. 5: Свет Небес стоит меньше на 10%.";
                            I.Rare = MechEngine.Rarity.Set;
                            switch (r.Next(4))
                            {
                                case 0: { I.AD = I.BufStat; break; }
                                case 1: { I.AP = Convert.ToInt32(I.BufStat * 1.5); break; }
                                case 2: { I.HP = I.BufStat; break; }
                                case 3: { I.MP = I.BufStat; break; }
                            }
                            I.ILvl = I.BufLvl;
                            I.Script = (bool Dress) =>
                            {
                                int SetLevel = 0;
                                if (Rogue.RAM.Player.Equipment.Boots.ArmorSet == "Harm") { SetLevel++; }
                                if (Rogue.RAM.Player.Equipment.Helm.ArmorSet == "Harm") { SetLevel++; }
                                if (Rogue.RAM.Player.Equipment.OffHand.ArmorSet == "Harm") { SetLevel++; }
                                if (Rogue.RAM.Player.Equipment.Weapon.ArmorSet == "Harm") { SetLevel++; }
                                switch (SetLevel)
                                {
                                    case 0:
                                        { if (Dress) { Rogue.RAM.Player.CMP += I.BufStat; Rogue.RAM.Player.MMP += I.BufStat; } else { Rogue.RAM.Player.MMP -= I.BufStat; Rogue.RAM.Player.CMP -= I.BufStat; } break; }
                                    case 1:
                                        { if (Dress) { Rogue.RAM.Player.CMP += I.BufStat*2; Rogue.RAM.Player.MMP += I.BufStat*2; } else { Rogue.RAM.Player.MMP -= I.BufStat*2; Rogue.RAM.Player.CMP -= I.BufStat*2; } break; }
                                    case 2:
                                        { if (Dress) { Rogue.RAM.Player.CMP += I.BufStat * 3; Rogue.RAM.Player.MMP += I.BufStat * 3; } else { Rogue.RAM.Player.MMP -= I.BufStat * 3; Rogue.RAM.Player.CMP -= I.BufStat * 3; } break; }
                                    case 3:
                                        {
                                            if (Dress) { Rogue.RAM.ArmSet.Add(new SystemEngine.ArmorSet() { Name = "HLight1", Active = true }); }
                                            else
                                            {
                                                SystemEngine.ArmorSet aset = new SystemEngine.ArmorSet();
                                                foreach (SystemEngine.ArmorSet asset in Rogue.RAM.ArmSet)
                                                { if (asset.Name == "HLight1") { aset = asset; } }
                                                Rogue.RAM.ArmSet.Remove(aset);
                                            }
                                            break;
                                        }
                                    case 4:
                                        {
                                            if (Dress) { Rogue.RAM.ArmSet.Add(new SystemEngine.ArmorSet() { Name = "HLight2", Active = true }); }
                                            else
                                            {
                                                SystemEngine.ArmorSet aset = new SystemEngine.ArmorSet();
                                                foreach (SystemEngine.ArmorSet asset in Rogue.RAM.ArmSet)
                                                { if (asset.Name == "HLight2") { aset = asset; } }
                                                Rogue.RAM.ArmSet.Remove(aset);
                                            }
                                            break;
                                        }
                                }
                            };
                            return I;
                        }
                    }

                    public static MechEngine.Item.OffHand RutgerOffhand
                    { get { if (r.Next(99) < 25) { return WandOfDeath; } else { return JustOffhand; } } }
                    private static MechEngine.Item.OffHand JustOffhand
                    {
                        get
                        {
                            MechEngine.Item.OffHand I = new MechEngine.Item.OffHand();
                            I.Name = "Закалённый щит";
                            I.Kind = MechEngine.Kind.OffHand;
                            I.Rare = GetRandomRareStats;
                            switch (r.Next(6))
                            {
                                case 0: { I.AD = BufferStat; break; }
                                case 1: { I.AP = Convert.ToInt32(BufferStat * 1.5); break; }
                                case 2: { I.ARM = BufferStat; break; }
                                case 3: { I.MADMG = BufferStat; break; }
                                case 4: { I.MIDMG = BufferStat; break; }
                                case 5: { I.MRS = BufferStat; break; }
                            }
                            I.ILvl = BufferIlvl;
                            return I;
                        }
                    }
                    private static MechEngine.Item.OffHand WandOfDeath
                    {
                        get
                        {
                            MechEngine.Item.OffHand I = new MechEngine.Item.OffHand();
                            I.BufStat = Convert.ToInt32(Rogue.RAM.Player.Level / 2) + 4;
                            I.BufLvl = Rogue.RAM.Player.Level + (Rogue.RAM.Map.Level * 2) + 1 * 7;
                            I.ArmorSet = "DeWarLock";
                            I.Name = "Палочка смерти";
                            I.Kind = MechEngine.Kind.OffHand;
                            I.Info = "Палочка великого некроманта Журха: 1: AP+" + I.BufStat + ". 2: MP+" + I.BufStat + ". 3: MRS+" + (I.BufStat * 3) + ". 4: Все заклинания Варлока стоят на 10% больше. 5: Все заклинания Варлока наносят на 20% больше урона.";
                            I.Rare = MechEngine.Rarity.Set;
                            switch (r.Next(6))
                            {
                                case 0: { I.AD = I.BufStat; break; }
                                case 1: { I.AP = Convert.ToInt32(I.BufStat * 1.5); break; }
                                case 2: { I.ARM = I.BufStat; break; }
                                case 3: { I.MADMG = I.BufStat; break; }
                                case 4: { I.MIDMG = I.BufStat; break; }
                                case 5: { I.MRS = I.BufStat; break; }
                            }
                            I.ILvl = I.BufLvl;
                            I.Script = (bool Dress) =>
                            {
                                int SetLevel = 0;
                                if (Rogue.RAM.Player.Equipment.Boots.ArmorSet == "DeWarLock") { SetLevel++; }
                                if (Rogue.RAM.Player.Equipment.Helm.ArmorSet == "DeWarLock") { SetLevel++; }
                                if (Rogue.RAM.Player.Equipment.OffHand.ArmorSet == "DeWarLock") { SetLevel++; }
                                if (Rogue.RAM.Player.Equipment.Weapon.ArmorSet == "DeWarLock") { SetLevel++; }
                                switch (SetLevel)
                                {
                                    case 0:
                                        { if (Dress) { Rogue.RAM.Player.AP += I.BufStat; } else { Rogue.RAM.Player.AP -= I.BufStat; } break; }
                                    case 1:
                                        { if (Dress) { Rogue.RAM.Player.CMP += I.BufStat; Rogue.RAM.Player.MMP += I.BufStat ; } else { Rogue.RAM.Player.MMP -= I.BufStat ; Rogue.RAM.Player.CMP -= I.BufStat ; } break; }
                                    case 2:
                                        { if (Dress) { Rogue.RAM.Player.MRS += I.BufStat; } else { Rogue.RAM.Player.MRS -= I.BufStat; } break; }
                                    case 3:
                                        {
                                            if (Dress) { Rogue.RAM.ArmSet.Add(new SystemEngine.ArmorSet() { Name = "DW1", Active = true }); }
                                            else
                                            {
                                                SystemEngine.ArmorSet aset = new SystemEngine.ArmorSet();
                                                foreach (SystemEngine.ArmorSet asset in Rogue.RAM.ArmSet)
                                                { if (asset.Name == "DW1") { aset = asset; } }
                                                Rogue.RAM.ArmSet.Remove(aset);
                                            }
                                            break;
                                        }
                                    case 4:
                                        {
                                            if (Dress) { Rogue.RAM.ArmSet.Add(new SystemEngine.ArmorSet() { Name = "DW2", Active = true }); }
                                            else
                                            {
                                                SystemEngine.ArmorSet aset = new SystemEngine.ArmorSet();
                                                foreach (SystemEngine.ArmorSet asset in Rogue.RAM.ArmSet)
                                                { if (asset.Name == "DW2") { aset = asset; } }
                                                Rogue.RAM.ArmSet.Remove(aset);
                                            }
                                            break;
                                        }
                                }
                            };
                            return I;
                        }
                    }

                    public static MechEngine.Item.Boots RutgerBoots
                    { get { if (r.Next(99) < 25) { return MayBoots; } else { return JustBoots; } } }
                    private static MechEngine.Item.Boots JustBoots
                    {
                        get
                        {
                            MechEngine.Item.Boots I = new MechEngine.Item.Boots();
                            I.Name = "Укрепленные сапоги";
                            I.Kind = MechEngine.Kind.Boots;
                            I.Rare = GetRandomRareStats;
                            switch (r.Next(2))
                            {
                                case 0: { I.ARM = BufferStat; break; }
                                case 1: { I.MRS = BufferStat; break; }
                            }
                            I.ILvl = BufferIlvl;
                            return I;
                        }
                    }
                    private static MechEngine.Item.Boots MayBoots
                    {
                        get
                        {
                            MechEngine.Item.Boots I = new MechEngine.Item.Boots();
                            I.BufStat = Convert.ToInt32(Rogue.RAM.Player.Level / 2) + 4;
                            I.BufLvl = Rogue.RAM.Player.Level + (Rogue.RAM.Map.Level * 2) + 1 * 7;
                            I.Name = "Ботинки Мэя";
                            I.ArmorSet = "May";
                            I.Kind = MechEngine.Kind.Boots;
                            I.Info = "Одеяние Мэя: 1: Молитва смерти +1 ход. 2: Молитва смерти +3 хода. 3: Молитва смерти +5 ходов. 4: Каменный кулак бонус от AP. 5: Стиль посоха работает на всё оружие.";
                            I.Rare = MechEngine.Rarity.Set;
                            switch (r.Next(2))
                            {
                                case 0: { I.ARM = I.BufStat; break; }
                                case 1: { I.MRS = I.BufStat; break; }
                            }
                            I.ILvl = I.BufLvl;
                            I.Script = (bool Dress) =>
                            {
                                int SetLevel = 0;
                                if (Rogue.RAM.Player.Equipment.Boots.ArmorSet == "May") { SetLevel++; }
                                if (Rogue.RAM.Player.Equipment.Helm.ArmorSet == "May") { SetLevel++; }
                                if (Rogue.RAM.Player.Equipment.OffHand.ArmorSet == "May") { SetLevel++; }
                                if (Rogue.RAM.Player.Equipment.Weapon.ArmorSet == "May") { SetLevel++; }
                                switch (SetLevel)
                                {
                                    case 0:
                                        {
                                            if (Dress) { Rogue.RAM.ArmSet.Add(new SystemEngine.ArmorSet() { Name = "M1", Active = true }); }
                                            else
                                            {
                                                SystemEngine.ArmorSet aset = new SystemEngine.ArmorSet();
                                                foreach (SystemEngine.ArmorSet asset in Rogue.RAM.ArmSet)
                                                { if (asset.Name == "M1") { aset = asset; } }
                                                Rogue.RAM.ArmSet.Remove(aset);
                                            }
                                            break;
                                        }
                                    case 1:
                                        {
                                            if (Dress) { Rogue.RAM.ArmSet.Add(new SystemEngine.ArmorSet() { Name = "M2", Active = true }); }
                                            else
                                            {
                                                SystemEngine.ArmorSet aset = new SystemEngine.ArmorSet();
                                                foreach (SystemEngine.ArmorSet asset in Rogue.RAM.ArmSet)
                                                { if (asset.Name == "M2") { aset = asset; } }
                                                Rogue.RAM.ArmSet.Remove(aset);
                                            }
                                            break;
                                        }
                                    case 2:
                                        {
                                            if (Dress) { Rogue.RAM.ArmSet.Add(new SystemEngine.ArmorSet() { Name = "M3", Active = true }); }
                                            else
                                            {
                                                SystemEngine.ArmorSet aset = new SystemEngine.ArmorSet();
                                                foreach (SystemEngine.ArmorSet asset in Rogue.RAM.ArmSet)
                                                { if (asset.Name == "M3") { aset = asset; } }
                                                Rogue.RAM.ArmSet.Remove(aset);
                                            }
                                            break;
                                        }
                                    case 3:
                                        {
                                            {
                                                if (Dress) { Rogue.RAM.ArmSet.Add(new SystemEngine.ArmorSet() { Name = "M4", Active = true }); }
                                                else
                                                {
                                                    SystemEngine.ArmorSet aset = new SystemEngine.ArmorSet();
                                                    foreach (SystemEngine.ArmorSet asset in Rogue.RAM.ArmSet)
                                                    { if (asset.Name == "M4") { aset = asset; } }
                                                    Rogue.RAM.ArmSet.Remove(aset);
                                                }
                                                break;
                                            }
                                        }
                                    case 4:
                                        {
                                            {
                                                if (Dress) { Rogue.RAM.ArmSet.Add(new SystemEngine.ArmorSet() { Name = "M5", Active = true }); }
                                                else
                                                {
                                                    SystemEngine.ArmorSet aset = new SystemEngine.ArmorSet();
                                                    foreach (SystemEngine.ArmorSet asset in Rogue.RAM.ArmSet)
                                                    { if (asset.Name == "M5") { aset = asset; } }
                                                    Rogue.RAM.ArmSet.Remove(aset);
                                                }
                                                break;
                                            }
                                        }
                                }
                            };
                            return I;
                        }
                    }
                }
            }

            public static class CraftItemsFromAbility
            {
                public static MechEngine.Item.Armor TailorMantle
                {
                    get
                    {
                        MechEngine.Item.Armor I = new MechEngine.Item.Armor();
                        I.Name = "Мантия" + Rogue.RAM.Player.Name;
                        I.Kind = MechEngine.Kind.Armor;
                        I.Color = ConsoleColor.Magenta;
                        I.ARM = Convert.ToInt32(Rogue.RAM.Player.Level / 4);
                        I.HP = Convert.ToInt32(Rogue.RAM.Player.Level / 2);
                        I.MP = Convert.ToInt32(Rogue.RAM.Player.Level);
                        I.MRS = Convert.ToInt32(Rogue.RAM.Player.Level / 4);
                        I.ILvl = 0;
                        return I;
                    }
                }

                public static MechEngine.Item.Weapon EfirWeapon
                {
                    get
                    {
                        MechEngine.Item.Weapon I = new MechEngine.Item.Weapon();
                        switch (r.Next(3))
                        {
                            case 0: { I.Name = "Праща эфира"; break; }
                            case 1: { I.Name = "Эфирный меч"; break; }
                            case 2: { I.Name = "Посох эфира"; break; }
                        }                        
                        I.Kind = MechEngine.Kind.Weapon;
                        I.Color = ConsoleColor.Cyan;
                        I.AD = EfirStat;
                        I.AP = EfirStat;
                        I.ARM = EfirStat;                        
                        I.ILvl = 0;
                        return I;
                    }
                }

                public static MechEngine.Item.Boots EfirBoots
                {
                    get
                    {
                        MechEngine.Item.Boots I = new MechEngine.Item.Boots();
                        switch (r.Next(3))
                        {
                            case 0: { I.Name = "Эфирные сапоги"; break; }
                            case 1: { I.Name = "Эфирные тапочки"; break; }
                            case 2: { I.Name = "Эфирные туфли"; break; }
                        }
                        I.Kind = MechEngine.Kind.Boots;
                        I.Color = ConsoleColor.Cyan;
                        I.MRS = EfirStat;                        
                        I.ARM = EfirStat;
                        I.ILvl = 0;
                        return I;
                    }
                }

                public static MechEngine.Item.Scroll EfirScroll
                { get { return (ItemBase.GetScroll as MechEngine.Item.Scroll); } }

                private static int EfirStat { get { double get = 0; for (int i = 0; i < Rogue.RAM.Map.Level; i++) { get += 0.05; } return Convert.ToInt32(get); } }
            }

            public static class ReputationItems
            {
                public static MechEngine.Item.Helm ElementalHead
                {
                    get
                    {
                        MechEngine.Item.Helm I = new MechEngine.Item.Helm();
                        I.BufStat = Convert.ToInt32(Rogue.RAM.Player.Level / 2) + 4;
                        I.BufLvl = Rogue.RAM.Player.Level + (Rogue.RAM.Map.Level * 2) + 1 * 7;
                        I.Name = "Шлем Элементов";
                        I.Kind = MechEngine.Kind.Helm;
                        I.ReputationSell = 5000;
                        I.ReputationName = "Мертвые";
                        I.ArmorSet = "ElArm";
                        I.Info = "Шлем созданное элементальным путём: 1: DMG↑+" + I.BufStat + ". 2: HP+" + I.BufStat + ". 3: MP+" + I.BufStat + ". 4: MRS+" + (I.BufStat * 2).ToString() + ". 5: 10% Шанс не потратить Элементы.";
                        I.Rare = MechEngine.Rarity.Set;
                        switch (r.Next(4))
                        {
                            case 0: { I.AD = I.BufStat; break; }
                            case 1: { I.AP = Convert.ToInt32(I.BufStat * 1.5); break; }
                            case 2: { I.HP = I.BufStat; break; }
                            case 3: { I.MP= I.BufStat; break; }
                        }
                        I.ILvl = I.BufLvl;
                        I.Script = (bool Dress) =>
                        {
                            int SetLevel = 0;
                            if (Rogue.RAM.Player.Equipment.Boots.ArmorSet == "ElArm") { SetLevel++; }
                            if (Rogue.RAM.Player.Equipment.Helm.ArmorSet == "ElArm") { SetLevel++; }
                            if (Rogue.RAM.Player.Equipment.OffHand.ArmorSet == "ElArm") { SetLevel++; }
                            if (Rogue.RAM.Player.Equipment.Weapon.ArmorSet == "ElArm") { SetLevel++; }
                            switch (SetLevel)
                            {
                                case 0:
                                    { if (Dress) { Rogue.RAM.Player.MADMG += I.BufStat; } else { Rogue.RAM.Player.MADMG -= I.BufStat; } break; }
                                case 1:
                                    { if (Dress) { Rogue.RAM.Player.CHP += I.BufStat; Rogue.RAM.Player.MHP += I.BufStat; } else { Rogue.RAM.Player.MHP -= I.BufStat; Rogue.RAM.Player.CHP -= I.BufStat; } break; }
                                case 2:
                                    { if (Dress) { Rogue.RAM.Player.CMP += I.BufStat; Rogue.RAM.Player.MMP += I.BufStat; } else { Rogue.RAM.Player.MMP -= I.BufStat; Rogue.RAM.Player.CMP -= I.BufStat; } break; }
                                case 3:
                                    { if (Dress) { Rogue.RAM.Player.MRS += I.BufStat; } else { Rogue.RAM.Player.MRS -= I.BufStat; } break; }
                                case 4:
                                    {
                                        if (Dress) { Rogue.RAM.ArmSet.Add(new SystemEngine.ArmorSet() { Name = "ElArm", Active = true }); }
                                        else
                                        {
                                            SystemEngine.ArmorSet aset = new SystemEngine.ArmorSet();
                                            foreach (SystemEngine.ArmorSet asset in Rogue.RAM.ArmSet)
                                            { if (asset.Name == "ElArm") { aset = asset; } }
                                            Rogue.RAM.ArmSet.Remove(aset);
                                        }
                                        break;
                                    }
                            }
                        };
                        return I;
                    }
                }

                public static MechEngine.Item.Helm HelmOfDeath
                {
                    get
                    {
                        MechEngine.Item.Helm I = new MechEngine.Item.Helm();
                        I.BufStat = Convert.ToInt32(Rogue.RAM.Player.Level / 2) + 4;
                        I.BufLvl = Rogue.RAM.Player.Level + (Rogue.RAM.Map.Level * 2) + 1 * 7;
                        I.ArmorSet = "DeWarLock";
                        I.ReputationName = "Мертвые";
                        I.ReputationSell = 5000;
                        I.Name = "Капюшон смерти";
                        I.Kind = MechEngine.Kind.Helm;
                        I.Info = "Капюшон великого некроманта Журха: 1: AP+" + I.BufStat + ". 2: MP+" + I.BufStat + ". 3: MRS+" + (I.BufStat * 3) + ". 4: Все заклинания Варлока стоят на 10% больше. 5: Все заклинания Варлока наносят на 20% больше урона.";
                        I.Rare = MechEngine.Rarity.Set;
                        switch (r.Next(4))
                        {
                            case 0: { I.AD = I.BufStat; break; }
                            case 1: { I.AP = Convert.ToInt32(I.BufStat * 1.5); break; }
                            case 2: { I.HP = I.BufStat; break; }
                            case 3: { I.MP = I.BufStat; break; }
                        }
                        I.ILvl = I.BufLvl;
                        I.Script = (bool Dress) =>
                        {
                            int SetLevel = 0;
                            if (Rogue.RAM.Player.Equipment.Boots.ArmorSet == "DeWarLock") { SetLevel++; }
                            if (Rogue.RAM.Player.Equipment.Helm.ArmorSet == "DeWarLock") { SetLevel++; }
                            if (Rogue.RAM.Player.Equipment.OffHand.ArmorSet == "DeWarLock") { SetLevel++; }
                            if (Rogue.RAM.Player.Equipment.Weapon.ArmorSet == "DeWarLock") { SetLevel++; }
                            switch (SetLevel)
                            {
                                case 0:
                                    { if (Dress) { Rogue.RAM.Player.AP += I.BufStat; } else { Rogue.RAM.Player.AP -= I.BufStat; } break; }
                                case 1:
                                    { if (Dress) { Rogue.RAM.Player.CMP += I.BufStat; Rogue.RAM.Player.MMP += I.BufStat; } else { Rogue.RAM.Player.MMP -= I.BufStat; Rogue.RAM.Player.CMP -= I.BufStat; } break; }
                                case 2:
                                    { if (Dress) { Rogue.RAM.Player.MRS += I.BufStat; } else { Rogue.RAM.Player.MRS -= I.BufStat; } break; }
                                case 3:
                                    {
                                        if (Dress) { Rogue.RAM.ArmSet.Add(new SystemEngine.ArmorSet() { Name = "DW1", Active = true }); }
                                        else
                                        {
                                            SystemEngine.ArmorSet aset = new SystemEngine.ArmorSet();
                                            foreach (SystemEngine.ArmorSet asset in Rogue.RAM.ArmSet)
                                            { if (asset.Name == "DW1") { aset = asset; } }
                                            Rogue.RAM.ArmSet.Remove(aset);
                                        }
                                        break;
                                    }
                                case 4:
                                    {
                                        if (Dress) { Rogue.RAM.ArmSet.Add(new SystemEngine.ArmorSet() { Name = "DW2", Active = true }); }
                                        else
                                        {
                                            SystemEngine.ArmorSet aset = new SystemEngine.ArmorSet();
                                            foreach (SystemEngine.ArmorSet asset in Rogue.RAM.ArmSet)
                                            { if (asset.Name == "DW2") { aset = asset; } }
                                            Rogue.RAM.ArmSet.Remove(aset);
                                        }
                                        break;
                                    }
                            }
                        };
                        return I;
                    }
                }

                public static MechEngine.Item.Helm UndeadHelm
                {
                    get
                    {
                        MechEngine.Item.Helm I = new MechEngine.Item.Helm();
                        I.Name = "Шлем нежити";
                        I.Kind = MechEngine.Kind.Helm;
                        I.ReputationSell = 1000;
                        I.ReputationName = "Мертвые";
                        I.Rare = GetRandomRareStats;
                        for (int i = 0; i < HowMuchStat+1; i++)
                        {
                            switch (r.Next(4))
                            {
                                case 0: { I.AD = BufferStat; break; }
                                case 1: { I.AP = Convert.ToInt32(BufferStat * 1.5); break; }
                                case 2: { I.HP = BufferStat; break; }
                                case 3: { I.MP = BufferStat; break; }
                            }
                        }
                        I.ILvl = BufferIlvl;
                        return I;
                    }
                }

                public static MechEngine.Item.OffHand RatTail
                {
                    get
                    {
                        MechEngine.Item.OffHand i = new MechEngine.Item.OffHand();
                        i.Name = "Крысий хвост";
                        i.Kind = MechEngine.Kind.OffHand;
                        i.Rare = MechEngine.Rarity.Rare;
                        i.ReputationSell = 50;
                        i.ReputationName = "Мертвые";
                        i.ARM = 1;
                        i.MRS = 1;
                        i.ILvl = 0;
                        return i;
                    }
                }

                public static MechEngine.Item.Weapon AntiUndeadSword
                {
                    get
                    {
                        MechEngine.Item.Weapon I = new MechEngine.Item.Weapon();
                        I.Name = "Меч против нежити";
                        I.Kind = MechEngine.Kind.Weapon;
                        I.ReputationSell = 500;
                        I.ReputationName = "Мертвые";
                        I.Rare = GetRandomRareStats;
                        for (int i = 0; i < HowMuchStat+1; i++)
                        {
                            switch (r.Next(5))
                            {
                                case 0: { I.AD = BufferStat; break; }
                                case 1: { I.AP = Convert.ToInt32(BufferStat * 1.5); break; }
                                case 2: { I.ARM = BufferStat; break; }
                                case 3: { I.MADMG = BufferStat; break; }
                                case 4: { I.MIDMG = BufferStat; break; }
                            }
                        }
                        I.ILvl = BufferIlvl;
                        return I;
                    }
                }

                public static MechEngine.Item.OffHand ElementalShield
                {
                    get
                    {
                        MechEngine.Item.OffHand I = new MechEngine.Item.OffHand();
                        I.BufStat = Convert.ToInt32(Rogue.RAM.Player.Level / 2) + 4;
                        I.BufLvl = Rogue.RAM.Player.Level + (Rogue.RAM.Map.Level * 2) + 1 * 7;
                        I.Name = "Щит Элементов";
                        I.Kind = MechEngine.Kind.OffHand;
                        I.ReputationSell = 250;
                        I.ReputationName = "Дварфы";
                        I.ArmorSet = "ElArm";
                        I.Info = "Щит созданный элементальным путём: 1: DMG↑+" + I.BufStat + ". 2: HP+" + I.BufStat + ". 3: MP+" + I.BufStat + ". 4: MRS+" + (I.BufStat * 2).ToString() + ". 5: 10% Шанс не потратить Элементы.";
                        I.Rare = MechEngine.Rarity.Set;
                        switch (r.Next(6))
                        {
                            case 0: { I.AD = I.BufStat; break; }
                            case 1: { I.AP = Convert.ToInt32(I.BufStat * 1.5); break; }
                            case 2: { I.ARM = I.BufStat; break; }
                            case 3: { I.MRS = I.BufStat; break; }
                            case 4: { I.MIDMG = I.BufStat; break; }
                            case 5: { I.MADMG = I.BufStat; break; }
                        }
                        I.ILvl = I.BufLvl;
                        I.Script = (bool Dress) =>
                        {
                            int SetLevel = 0;
                            if (Rogue.RAM.Player.Equipment.Boots.ArmorSet == "ElArm") { SetLevel++; }
                            if (Rogue.RAM.Player.Equipment.Helm.ArmorSet == "ElArm") { SetLevel++; }
                            if (Rogue.RAM.Player.Equipment.OffHand.ArmorSet == "ElArm") { SetLevel++; }
                            if (Rogue.RAM.Player.Equipment.Weapon.ArmorSet == "ElArm") { SetLevel++; }
                            switch (SetLevel)
                            {
                                case 0:
                                    { if (Dress) { Rogue.RAM.Player.MADMG += I.BufStat; } else { Rogue.RAM.Player.MADMG -= I.BufStat; } break; }
                                case 1:
                                    { if (Dress) { Rogue.RAM.Player.CHP += I.BufStat; Rogue.RAM.Player.MHP += I.BufStat; } else { Rogue.RAM.Player.MHP -= I.BufStat; Rogue.RAM.Player.CHP -= I.BufStat; } break; }
                                case 2:
                                    { if (Dress) { Rogue.RAM.Player.CMP += I.BufStat; Rogue.RAM.Player.MMP += I.BufStat; } else { Rogue.RAM.Player.MMP -= I.BufStat; Rogue.RAM.Player.CMP -= I.BufStat; } break; }
                                case 3:
                                    { if (Dress) { Rogue.RAM.Player.MRS += I.BufStat; } else { Rogue.RAM.Player.MRS -= I.BufStat; } break; }
                                case 4:
                                    {
                                        if (Dress) { Rogue.RAM.ArmSet.Add(new SystemEngine.ArmorSet() { Name = "ElArm", Active = true }); }
                                        else
                                        {
                                            SystemEngine.ArmorSet aset = new SystemEngine.ArmorSet();
                                            foreach (SystemEngine.ArmorSet asset in Rogue.RAM.ArmSet)
                                            { if (asset.Name == "ElArm") { aset = asset; } }
                                            Rogue.RAM.ArmSet.Remove(aset);
                                        }
                                        break;
                                    }
                            }
                        };
                        return I;
                    }
                }

                public static MechEngine.Item.Boots BootsOfDeath
                {
                    get
                    {
                        MechEngine.Item.Boots I = new MechEngine.Item.Boots();
                        I.BufStat = Convert.ToInt32(Rogue.RAM.Player.Level / 2) + 4;
                        I.BufLvl = Rogue.RAM.Player.Level + (Rogue.RAM.Map.Level * 2) + 1 * 7;
                        I.ArmorSet = "DeWarLock";
                        I.ReputationName = "Дварфы";
                        I.ReputationSell = 327;
                        I.Name = "Тапочки смерти";
                        I.Kind = MechEngine.Kind.Boots;
                        I.Info = "Тапочки великого некроманта Журха: 1: AP+" + I.BufStat + ". 2: MP+" + I.BufStat + ". 3: MRS+" + (I.BufStat * 3) + ". 4: Все заклинания Варлока стоят на 10% больше. 5: Все заклинания Варлока наносят на 20% больше урона.";
                        I.Rare = MechEngine.Rarity.Set;
                        switch (r.Next(2))
                        {
                            case 0: { I.ARM = I.BufStat; break; }
                            case 1: { I.MRS = I.BufStat; break; }
                        }
                        I.ILvl = I.BufLvl;
                        I.Script = (bool Dress) =>
                        {
                            int SetLevel = 0;
                            if (Rogue.RAM.Player.Equipment.Boots.ArmorSet == "DeWarLock") { SetLevel++; }
                            if (Rogue.RAM.Player.Equipment.Helm.ArmorSet == "DeWarLock") { SetLevel++; }
                            if (Rogue.RAM.Player.Equipment.OffHand.ArmorSet == "DeWarLock") { SetLevel++; }
                            if (Rogue.RAM.Player.Equipment.Weapon.ArmorSet == "DeWarLock") { SetLevel++; }
                            switch (SetLevel)
                            {
                                case 0:
                                    { if (Dress) { Rogue.RAM.Player.AP += I.BufStat; } else { Rogue.RAM.Player.AP -= I.BufStat; } break; }
                                case 1:
                                    { if (Dress) { Rogue.RAM.Player.CMP += I.BufStat; Rogue.RAM.Player.MMP += I.BufStat; } else { Rogue.RAM.Player.MMP -= I.BufStat; Rogue.RAM.Player.CMP -= I.BufStat; } break; }
                                case 2:
                                    { if (Dress) { Rogue.RAM.Player.MRS += I.BufStat; } else { Rogue.RAM.Player.MRS -= I.BufStat; } break; }
                                case 3:
                                    {
                                        if (Dress) { Rogue.RAM.ArmSet.Add(new SystemEngine.ArmorSet() { Name = "DW1", Active = true }); }
                                        else
                                        {
                                            SystemEngine.ArmorSet aset = new SystemEngine.ArmorSet();
                                            foreach (SystemEngine.ArmorSet asset in Rogue.RAM.ArmSet)
                                            { if (asset.Name == "DW1") { aset = asset; } }
                                            Rogue.RAM.ArmSet.Remove(aset);
                                        }
                                        break;
                                    }
                                case 4:
                                    {
                                        if (Dress) { Rogue.RAM.ArmSet.Add(new SystemEngine.ArmorSet() { Name = "DW2", Active = true }); }
                                        else
                                        {
                                            SystemEngine.ArmorSet aset = new SystemEngine.ArmorSet();
                                            foreach (SystemEngine.ArmorSet asset in Rogue.RAM.ArmSet)
                                            { if (asset.Name == "DW2") { aset = asset; } }
                                            Rogue.RAM.ArmSet.Remove(aset);
                                        }
                                        break;
                                    }
                            }
                        };
                        return I;
                    }
                }

                public static MechEngine.Item.Boots DwarfFortik
                {
                    get
                    {
                        MechEngine.Item.Boots I = new MechEngine.Item.Boots();
                        I.Name = "Каменный сапог";
                        I.Kind = MechEngine.Kind.Boots;
                        I.ReputationSell = 50;
                        I.ReputationName = "Дварфы";
                        I.Rare = GetRandomRareStats;
                        for (int i = 0; i < HowMuchStat + 1; i++)
                        {
                            switch (r.Next(2))
                            {
                                case 0: { I.MRS = BufferStat; break; }
                                case 1: { I.ARM = BufferStat; break; }
                            }
                        }
                        I.ILvl = BufferIlvl;
                        return I;
                    }
                }

                public static MechEngine.Item.Helm HodarsHelm
                {
                    get
                    {
                        MechEngine.Item.Helm I = new MechEngine.Item.Helm();
                        I.BufStat = Convert.ToInt32(Rogue.RAM.Player.Level / 2) + 4;
                        I.BufLvl = Rogue.RAM.Player.Level + (Rogue.RAM.Map.Level * 2) + 1 * 7;
                        I.Name = "Шлем Ходара";
                        I.ArmorSet = "Hodar";
                        I.ReputationSell = 670;
                        I.ReputationName = "Дварфы";
                        I.Kind = MechEngine.Kind.Helm;
                        I.Info = "Шлем великого война Ходара: 1: AD+" + I.BufStat + ". 2: HP+" + I.BufStat + ". 3: ARM+" + I.BufStat + ", MRS+" + I.BufStat + ". 4: HP+" + (I.BufStat * 2).ToString() + ". 5: Прирост ярости +10 ед.";
                        I.Rare = MechEngine.Rarity.Set;
                        switch (r.Next(4))
                        {
                            case 0: { I.AD = I.BufStat; break; }
                            case 1: { I.HP = I.BufStat; break; }
                            case 2: { I.MP = I.BufStat; break; }
                            case 3: { I.AP = I.BufStat; break; }
                        }
                        I.ILvl = I.BufLvl;
                        I.Script = (bool Dress) =>
                        {
                            int SetLevel = 0;
                            if (Rogue.RAM.Player.Equipment.Boots.ArmorSet == "Hodar") { SetLevel++; }
                            if (Rogue.RAM.Player.Equipment.Helm.ArmorSet == "Hodar") { SetLevel++; }
                            if (Rogue.RAM.Player.Equipment.OffHand.ArmorSet == "Hodar") { SetLevel++; }
                            if (Rogue.RAM.Player.Equipment.Weapon.ArmorSet == "Hodar") { SetLevel++; }
                            switch (SetLevel)
                            {
                                case 0:
                                    { if (Dress) { Rogue.RAM.Player.AD += I.BufStat; } else { Rogue.RAM.Player.AD -= I.BufStat; } break; }
                                case 1:
                                    { if (Dress) { Rogue.RAM.Player.CHP += I.BufStat; Rogue.RAM.Player.MHP += I.BufStat; } else { Rogue.RAM.Player.MHP -= I.BufStat; Rogue.RAM.Player.CHP -= I.BufStat; } break; }
                                case 2:
                                    { if (Dress) { Rogue.RAM.Player.ARM += I.BufStat; Rogue.RAM.Player.MRS += I.BufStat; } else { Rogue.RAM.Player.ARM -= I.BufStat; Rogue.RAM.Player.MRS -= I.BufStat; } break; }
                                case 3:
                                    { if (Dress) { Rogue.RAM.Player.CHP += I.BufStat * 2; Rogue.RAM.Player.MHP += I.BufStat * 2; } else { Rogue.RAM.Player.MHP -= I.BufStat * 2; Rogue.RAM.Player.CHP -= I.BufStat * 2; } break; }
                                case 4:
                                    {
                                        if (Dress) { Rogue.RAM.ArmSet.Add(new SystemEngine.ArmorSet() { Name = "Hodar", Active = true }); }
                                        else
                                        {
                                            SystemEngine.ArmorSet aset = new SystemEngine.ArmorSet();
                                            foreach (SystemEngine.ArmorSet asset in Rogue.RAM.ArmSet)
                                            { if (asset.Name == "Hodar") { aset = asset; } }
                                            Rogue.RAM.ArmSet.Remove(aset);
                                        }
                                        break;
                                    }
                            }
                        };
                        return I;
                    }
                }

                public static MechEngine.Item.Scroll OldRune
                {
                    get
                    {
                        MechEngine.Item.Scroll rn = new MechEngine.Item.Scroll();
                        rn.ReputationSell = 2900;
                        rn.ReputationName = "Дварфы";
                        rn.Name = "Старая руна";
                        rn.Kind = MechEngine.Kind.Rune;
                        rn.Color = ConsoleColor.DarkGray;
                        rn.Spell = OtherAbilityBase.OldRune;
                        return rn;
                    }
                }
            }

            public static MechEngine.Item.Potion AlchemistPotion
            {
                get
                {
                    MechEngine.Item.Potion I = new MechEngine.Item.Potion();
                    I.Name = "Зелье алхимика";
                    I.Kind = MechEngine.Kind.Potion;
                    I.Rare = MechEngine.Rarity.Fired;
                    I.HP = Convert.ToInt32(Rogue.RAM.Player.Level * (Rogue.RAM.Player.AP * 1.21));
                    I.MP = Convert.ToInt32(Rogue.RAM.Player.Level * (Rogue.RAM.Player.AP * 1.21));
                    I.ILvl = 0;
                    return I;
                }
            }

            private static int BufferStat;

            private static int HowMuchStat;

            private static int BufferIlvl;

            private static MechEngine.Rarity GetRandomRareStats
            {
                get
                {
                    MechEngine.Rarity rar = new MechEngine.Rarity();
                    ;
                    int rare = r.Next(101);
                    rare += Convert.ToInt32(MechEngine.Item.GetGearScore() * 0.001);
                    rare += Convert.ToInt32(Rogue.RAM.Player.AP * 0.1);
                    if (rare < 10)
                    {
                        BufferStat = 1;
                        BufferIlvl = Rogue.RAM.Player.Level + (Rogue.RAM.Map.Level * 2) + 1 * 3;
                        HowMuchStat = 1;
                        rar = MechEngine.Rarity.Poor;
                    }
                    if (rare > 10 && rare < 30)
                    {
                        BufferStat = Convert.ToInt32(Rogue.RAM.Player.Level / 3) + 1;
                        BufferIlvl = Rogue.RAM.Player.Level + (Rogue.RAM.Map.Level * 2) + 1 * 4;
                        HowMuchStat = 1;
                        rar = MechEngine.Rarity.Common;
                    }
                    if (rare > 30 && rare < 60)
                    {
                        BufferStat = Convert.ToInt32(Rogue.RAM.Player.Level / 2) + 2;
                        BufferIlvl = Rogue.RAM.Player.Level + (Rogue.RAM.Map.Level * 2) + 1 * 5;
                        HowMuchStat = 2;
                        rar = MechEngine.Rarity.Uncommon;
                    }
                    if (rare > 60 && rare < 70)
                    {
                        BufferStat = Convert.ToInt32(Rogue.RAM.Player.Level / 2) + 3;
                        BufferIlvl = Rogue.RAM.Player.Level + (Rogue.RAM.Map.Level * 2) + 1 * 6;
                        HowMuchStat = 2;
                        rar = MechEngine.Rarity.Rare;
                    }
                    if (rare > 70 && rare < 80)
                    {
                        BufferStat = Convert.ToInt32(Rogue.RAM.Player.Level / 2) + 4;
                        BufferIlvl = Rogue.RAM.Player.Level + (Rogue.RAM.Map.Level * 2) + 1 * 7;
                        HowMuchStat = 3;
                        rar = MechEngine.Rarity.Set;
                    }
                    if (rare > 80 && rare < 90)
                    {
                        BufferStat = Convert.ToInt32(Rogue.RAM.Player.Level / 2) + 5;
                        BufferIlvl = Rogue.RAM.Player.Level + (Rogue.RAM.Map.Level * 2) + 1 * 9;
                        HowMuchStat = 3;
                        rar = MechEngine.Rarity.Epic;
                    }
                    if (rare > 90 && rare < 95)
                    {
                        BufferStat = Rogue.RAM.Player.Level + 1;
                        BufferIlvl = Rogue.RAM.Player.Level + (Rogue.RAM.Map.Level * 2) + 1 * 10;
                        rar = MechEngine.Rarity.Legendary;
                        HowMuchStat = 4;
                    }
                    if (rare > 95)
                    {
                        BufferStat = Rogue.RAM.Player.Level + 2;
                        BufferIlvl = Rogue.RAM.Player.Level + (Rogue.RAM.Map.Level * 2) + 1 * 12;
                        rar = MechEngine.Rarity.Artefact;
                        HowMuchStat = 5;
                    }
                    return rar;
                }
            }
        }

        public static class ResourseBase
        {
            public static MechEngine.Item.Resource Random
            {
                get
                {
                    switch (r.Next(11))
                    {
                        case 0: { return CatFood; }
                        case 1: { return Needle; }
                        case 2: { return Thread; }
                        case 3: { return TailorKit; }
                        case 4: { return Wood; }
                        case 5: { return WoodD; }
                        case 6: { return DeadWood; }
                        case 7: { return Iron; }
                        case 8: { return DeadWater; }
                        case 9: { return DeadRose; }
                        case 10: { return Essencial; }
                        //case 11: { return RetronslateV; }
                        default: return Trash;
                    }
                }
            }

            public static MechEngine.Item.Resource CatFood
            {
                get
                {
                    MechEngine.Item.Resource I = new MechEngine.Item.Resource();
                    I.Name = "Корм для кошек";
                    I._Icon = '▬';
                    I.Kind = MechEngine.Kind.Resource;
                    I.Color = ConsoleColor.DarkYellow;
                    I.ILvl = 0;
                    return I;
                }
            }

            public static MechEngine.Item.Resource Needle
            {
                get
                {
                    MechEngine.Item.Resource I = new MechEngine.Item.Resource();
                    I.Name = "Иголка";
                    I._Icon = '│';
                    I.Kind = MechEngine.Kind.Resource;
                    I.Color = ConsoleColor.Green;
                    I.ILvl = 0;
                    return I;
                }
            }

            public static MechEngine.Item.Resource Thread
            {
                get
                {
                    MechEngine.Item.Resource I = new MechEngine.Item.Resource();
                    I.Name = "Нитки";
                    I._Icon = '=';
                    I.Kind = MechEngine.Kind.Resource;
                    I.Color = ConsoleColor.Green;
                    I.ILvl = 0;
                    return I;
                }
            }

            public static MechEngine.Item.Resource TailorKit
            {
                get
                {
                    MechEngine.Item.Resource I = new MechEngine.Item.Resource();
                    I.Name = "Набор портного";
                    I._Icon = '┬';
                    I.Kind = MechEngine.Kind.Resource;
                    I.Color = ConsoleColor.Green;
                    I.ILvl = 0;
                    return I;
                }
            }

            public static MechEngine.Item.Resource Wood
            {
                get
                {
                    MechEngine.Item.Resource I = new MechEngine.Item.Resource();
                    I.Name = "Дерево";
                    I._Icon = '▌';
                    I.Kind = MechEngine.Kind.Resource;
                    I.Color = ConsoleColor.DarkYellow;
                    I.ILvl = 0;
                    return I;
                }
            }

            public static MechEngine.Item.Resource WoodD
            {
                get
                {
                    MechEngine.Item.Resource I = new MechEngine.Item.Resource();
                    I.Name = "Дерево(Дуб)";
                    I._Icon = '▌';
                    I.Kind = MechEngine.Kind.Resource;
                    I.Color = ConsoleColor.DarkYellow;
                    I.ILvl = 0;
                    return I;
                }
            }

            public static MechEngine.Item.Resource BloodWood
            {
                get
                {
                    MechEngine.Item.Resource I = new MechEngine.Item.Resource();
                    I.Name = "Красное дерево";
                    I._Icon = '▌';
                    I.Kind = MechEngine.Kind.Resource;
                    I.Color = ConsoleColor.DarkRed;
                    I.ILvl = 0;
                    return I;
                }
            }

            public static MechEngine.Item.Resource DeadWood
            {
                get
                {
                    MechEngine.Item.Resource I = new MechEngine.Item.Resource();
                    I.Name = "Мертвое дерево";
                    I._Icon = '▌';
                    I.Kind = MechEngine.Kind.Resource;
                    I.Color = ConsoleColor.DarkGray;
                    I.ILvl = 0;
                    return I;
                }
            }

            public static MechEngine.Item.Resource Iron
            {
                get
                {
                    MechEngine.Item.Resource I = new MechEngine.Item.Resource();
                    I.Name = "Железо";
                    I._Icon = '∙';
                    I.Kind = MechEngine.Kind.Resource;
                    I.Color = ConsoleColor.Gray;
                    I.ILvl = 0;
                    return I;
                }
            }

            public static MechEngine.Item.Resource IStone
            {
                get
                {
                    MechEngine.Item.Resource I = new MechEngine.Item.Resource();
                    I.Name = "Камень мудрости";
                    I._Icon = '☼';
                    I.Kind = MechEngine.Kind.Resource;
                    I.Color = ConsoleColor.Magenta;
                    I.ILvl = 0;
                    return I;
                }
            }

            public static MechEngine.Item.Resource SStone
            {
                get
                {
                    MechEngine.Item.Resource I = new MechEngine.Item.Resource();
                    I.Name = "Камень силы";
                    I._Icon = '☼';
                    I.Kind = MechEngine.Kind.Resource;
                    I.Color = ConsoleColor.Red;
                    I.ILvl = 0;
                    return I;
                }
            }

            public static MechEngine.Item.Resource CStone
            {
                get
                {
                    MechEngine.Item.Resource I = new MechEngine.Item.Resource();
                    I.Name = "Камень ловкости";
                    I._Icon = '☼';
                    I.Kind = MechEngine.Kind.Resource;
                    I.Color = ConsoleColor.Green;
                    I.ILvl = 0;
                    return I;
                }
            }

            public static MechEngine.Item.Resource ColdStone
            {
                get
                {
                    MechEngine.Item.Resource I = new MechEngine.Item.Resource();
                    I.Name = "Камень сопряжения";
                    I._Icon = '☼';
                    I.Kind = MechEngine.Kind.Resource;
                    I.Color = ConsoleColor.Cyan;
                    I.ILvl = 0;
                    return I;
                }
            }

            public static MechEngine.Item.Resource RetronslateV
            {
                get
                {
                    MechEngine.Item.Resource I = new MechEngine.Item.Resource();
                    I.Name = "Ретронслятор V";
                    I._Icon = 'z';
                    I.Kind = MechEngine.Kind.Resource;
                    I.Color = ConsoleColor.DarkRed;
                    I.ILvl = 0;
                    return I;
                }
            }

            public static MechEngine.Item.Resource UndeadRing
            {
                get
                {
                    MechEngine.Item.Resource I = new MechEngine.Item.Resource();
                    I.Name = "Обручальное кольцо";
                    I._Icon = 'o';
                    I.Kind = MechEngine.Kind.Resource;
                    I.Color = ConsoleColor.Yellow;
                    I.ILvl = 0;
                    return I;
                }
            }

            public static MechEngine.Item.Resource DeadRose
            {
                get
                {
                    MechEngine.Item.Resource I = new MechEngine.Item.Resource();
                    I.Name = "Мертвая роза";
                    I._Icon = '☼';
                    I.Kind = MechEngine.Kind.Resource;
                    I.Color = ConsoleColor.DarkGray;
                    I.ReputationSell = 50;
                    I.ReputationName = "Мертвые";
                    I.ILvl = 0;
                    return I;
                }
            }

            public static MechEngine.Item.Resource DeadWater
            {
                get
                {
                    MechEngine.Item.Resource I = new MechEngine.Item.Resource();
                    I.Name = "Мертвая вода";
                    I._Icon = '~';
                    I.Kind = MechEngine.Kind.Resource;
                    I.Color = ConsoleColor.DarkGray;
                    I.ReputationSell = 100;
                    I.ReputationName = "Мертвые";
                    I.ILvl = 0;
                    return I;
                }
            }

            public static MechEngine.Item.Resource DeadPoison
            {
                get
                {
                    MechEngine.Item.Resource I = new MechEngine.Item.Resource();
                    I.Name = "Трупный яд";
                    I._Icon = '"';
                    I.Kind = MechEngine.Kind.Resource;
                    I.Color = ConsoleColor.Green;
                    I.ReputationSell = 50;
                    I.ReputationName = "Мертвые";
                    I.ILvl = 0;
                    return I;
                }
            }

            public static MechEngine.Item.Resource Essencial
            {
                get
                {
                    MechEngine.Item.Resource I = new MechEngine.Item.Resource();
                    I.Name = "Эссенция";
                    I._Icon = '*';
                    I.Kind = MechEngine.Kind.Resource;
                    I.Color = ConsoleColor.Magenta;
                    I.ILvl = 0;
                    return I;
                }
            }

            public static MechEngine.Item.Resource Trash
            {
                get
                {
                    MechEngine.Item.Resource I = new MechEngine.Item.Resource();
                    I.Name = "Мусор";
                    I._Icon = '╕';
                    I.Kind = MechEngine.Kind.Resource;
                    I.Color = ConsoleColor.DarkBlue;
                    I.ILvl = 0;
                    return I;
                }
            }

            public static MechEngine.Item.Resource Diamond
            {
                get
                {
                    MechEngine.Item.Resource I = new MechEngine.Item.Resource();
                    I.Name = "Алмаз";
                    I._Icon = '∙';
                    I.Kind = MechEngine.Kind.Resource;
                    I.Color = ConsoleColor.White;
                    I.SetSell = 1000;
                    I.ILvl = 0;
                    return I;
                }
            }

            public static MechEngine.Item.Resource Glass
            {
                get
                {
                    MechEngine.Item.Resource I = new MechEngine.Item.Resource();
                    I.Name = "Стекло";
                    I._Icon = '▌';
                    I.Kind = MechEngine.Kind.Resource;
                    I.Color = ConsoleColor.Cyan;
                    I.SetSell = 570;
                    I.ILvl = 0;
                    return I;
                }
            }

            public static MechEngine.Item.Resource LifeRose
            {
                get
                {
                    MechEngine.Item.Resource I = new MechEngine.Item.Resource();
                    I.Name = "Живая роза";
                    I._Icon = '☼';
                    I.Kind = MechEngine.Kind.Resource;
                    I.Color = ConsoleColor.Red;
                    I.SetSell = 750;
                    I.ILvl = 0;
                    return I;
                }
            }

            public static MechEngine.Item.Resource ScrijalScroll
            {
                get
                {
                    MechEngine.Item.Resource I = new MechEngine.Item.Resource();
                    I.Name = "Скрижаль";
                    I._Icon = '░';
                    I.Kind = MechEngine.Kind.Resource;
                    I.Color = ConsoleColor.Cyan;
                    I.ILvl = 0;
                    return I;
                }
            }
        }

        public static class DoorBase
        {
            public static MechEngine.ActiveObject GetDoor(int Door)
            {                
                if (Door == 0)
                {
                    return IronDoor();
                }
                else
                {
                    return MagicDoor();
                }
            }

            public static MechEngine.ActiveObject Exit
            {
                get
                {
                    MechEngine.ActiveObject e = new MechEngine.ActiveObject();
                    e.Name = "Exit";
                    e.Move = false;
                    return e;
                }
            }

            public static MechEngine.ActiveObject IronDoor()
            {
                MechEngine.ActiveObject rtrn = new MechEngine.ActiveObject();

                rtrn.Icon = '▒';
                rtrn.Name = "Железная дверь";
                rtrn.Color = ConsoleColor.White;
                MechEngine.Item.Key k = new MechEngine.Item.Key();
                k.Kind = MechEngine.Kind.Key;
                k.Rare = MechEngine.Rarity.Common;
                k.Name = "Железный ключ";
                rtrn.Key = k;

                return rtrn;
            }

            public static MechEngine.CapitalDoor UndeadDoor
            {
                get
                {
                    MechEngine.CapitalDoor r = new MechEngine.CapitalDoor();
                    r.Icon = '║';
                    r.Quarter = 1;
                    r.Color = ConsoleColor.DarkCyan;
                    return r;
                }
            }

            public static MechEngine.CapitalDoor MagicalDoor
            {
                get
                {
                    MechEngine.CapitalDoor r = new MechEngine.CapitalDoor();
                    r.Icon = '╡';
                    r.Quarter = 2;
                    r.Color = ConsoleColor.DarkMagenta;
                    return r;
                }
            }

            public static MechEngine.CapitalDoor HumanDoor
            {
                get
                {
                    MechEngine.CapitalDoor r = new MechEngine.CapitalDoor();
                    r.Icon = '☼';
                    r.Quarter = 3;
                    r.Color = ConsoleColor.DarkYellow;
                    return r;
                }
            }

            public static MechEngine.CapitalDoor OrcDoor
            {
                get
                {
                    MechEngine.CapitalDoor r = new MechEngine.CapitalDoor();
                    r.Icon = '☻';
                    r.Quarter = 4;
                    r.Color = ConsoleColor.DarkGreen;
                    return r;
                }
            }

            public static MechEngine.CapitalDoor TradeDoor
            {
                get
                {
                    MechEngine.CapitalDoor r = new MechEngine.CapitalDoor();
                    r.Icon = '$';
                    r.Quarter = 0;
                    r.Color = ConsoleColor.Yellow;
                    return r;
                }
            }

            public static MechEngine.CapitalDoor.TownPortal TownPortal
            {
                get
                {
                    MechEngine.CapitalDoor.TownPortal r = new MechEngine.CapitalDoor.TownPortal();
                    r.Icon = '░';
                    r.Color = ConsoleColor.Blue;
                    return r;
                }
            }
            
            public static MechEngine.ActiveObject MagicDoor()
            {
                MechEngine.ActiveObject rtrn = new MechEngine.ActiveObject();

                rtrn.Icon = '░';
                rtrn.Name = "Магическая дверь";
                rtrn.Color = ConsoleColor.Magenta;
                MechEngine.Item.Key k = new MechEngine.Item.Key();
                k.Kind = MechEngine.Kind.Key;
                k.Rare = MechEngine.Rarity.Epic;
                k.Name = "Магический ключ";
                rtrn.Key = k;

                return rtrn;
            }

            public static MechEngine.Altar Altar
            {
                get
                {
                    MechEngine.Altar a = new MechEngine.Altar();
                    a.Icon='&';
                    a.Color = ConsoleColor.Blue;
                    a.Color = ConsoleColor.Blue;
                    return a;
                }
            }
        }

        public static class SummonedBase
        {
            public static MechEngine.Summoned Sceleton()
            {
                MechEngine.Summoned z = new MechEngine.Summoned();
                z.Name = "Скелет";
                z.Actions = new MechEngine.AbilityActionType[] { MechEngine.AbilityActionType.Damage };
                z.AttackSpeed = 5000;
                z.MagicOrPhysic = MechEngine.AbilityRate.AttackDamage;
                z.PAD = Convert.ToInt32(Rogue.RAM.Player.AP * 1.005);
                return z;
            }

            public static MechEngine.Summoned Cat(int Power)
            {
                MechEngine.Summoned z = new MechEngine.Summoned();
                z.Name = "Кот";
                z.Actions = new MechEngine.AbilityActionType[] { MechEngine.AbilityActionType.Damage };
                z.AttackSpeed = 5000;
                z.MagicOrPhysic = MechEngine.AbilityRate.AttackDamage;
                z.PAD = Power / 2;
                if (z.PAD <= 0) { z.PAD = 1; }
                return z;
            }

            public static MechEngine.Summoned MHeal(int Power)
            {
                MechEngine.Summoned z = new MechEngine.Summoned();
                z.Name = "Лекарь";
                z.Actions = new MechEngine.AbilityActionType[] { MechEngine.AbilityActionType.Heal };
                z.AttackSpeed = 5000;
                z.MagicOrPhysic = MechEngine.AbilityRate.AbilityPower;
                z.PAH = Power;
                return z;
            }
            public static MechEngine.Summoned MWarrior(int Power)
            {
                MechEngine.Summoned z = new MechEngine.Summoned();
                z.Name = "Воин";
                z.Actions = new MechEngine.AbilityActionType[] { MechEngine.AbilityActionType.Damage };
                z.AttackSpeed = 5000;
                z.MagicOrPhysic = MechEngine.AbilityRate.AttackDamage;
                z.PAD = Power;
                if (z.PAD <= 0) { z.PAD = 1; }
                return z;
            }
            public static MechEngine.Summoned MMage(int Power)
            {
                MechEngine.Summoned z = new MechEngine.Summoned();
                z.Name = "Маг";
                z.Actions = new MechEngine.AbilityActionType[] { MechEngine.AbilityActionType.Damage };
                z.AttackSpeed = 5000;
                z.MagicOrPhysic = MechEngine.AbilityRate.AbilityPower;
                z.PAD = Power;
                if (z.PAD <= 0) { z.PAD = 1; }
                return z;
            }

            public static MechEngine.Summoned Spirit()
            {
                MechEngine.Summoned z = new MechEngine.Summoned();
                z.Name = "Призрак";
                z.Actions = new MechEngine.AbilityActionType[] { MechEngine.AbilityActionType.Heal };
                z.AttackSpeed = 8000;
                z.MagicOrPhysic = MechEngine.AbilityRate.AbilityPower;
                z.PAH = Convert.ToInt32(Rogue.RAM.Player.AP * 1.005);
                return z;
            }

            public static MechEngine.Summoned FireElemental()
            {
                MechEngine.Summoned z = new MechEngine.Summoned();
                z.Name = "Дух огня";
                z.Actions = new MechEngine.AbilityActionType[] { MechEngine.AbilityActionType.Damage, MechEngine.AbilityActionType.Debuff };
                z.AttackSpeed = 5000;
                z.MagicOrPhysic = MechEngine.AbilityRate.AbilityPower;
                z.PAD = Convert.ToInt32(Rogue.RAM.Player.AP * 1.24);
                z.PAM = Convert.ToInt32(Rogue.RAM.Player.AP * 1.24);
                z.PAMa = MechEngine.AbilityStats.ARM;
                return z;
            }

            public static MechEngine.Summoned InquisitorUnit()
            {
                MechEngine.Summoned z = new MechEngine.Summoned();
                z.Name = "Unit";
                z.Actions = new MechEngine.AbilityActionType[] { MechEngine.AbilityActionType.Damage, MechEngine.AbilityActionType.Heal };
                z.AttackSpeed = 12874;
                z.MagicOrPhysic = MechEngine.AbilityRate.AbilityPower;
                z.PAD = Convert.ToInt32(Rogue.RAM.Player.AP * 1.1);
                z.PAH = Convert.ToInt32(Rogue.RAM.Player.AP * 1.1);
                return z;
            }
        }

        public static class BattleAbilityBase
        {
            /// <summary>
            /// Blood mage / vampire abilityes
            /// </summary>
            /// <returns>List of blood mage ability</returns>
            public static List<MechEngine.Ability> Vampire()
            { return new List<MechEngine.Ability> { Vampirism(), BloodSpear(), BloodShield(), Ghoul() }; }
            /// <summary>
            /// Paladin abilityes
            /// </summary>
            /// <returns>List of paladin ability</returns>
            public static List<MechEngine.Ability> Paladin()
            { return new List<MechEngine.Ability> { HolyLight(), HolyNova(), TurnEvil(), LightStar() }; }
            /// <summary>
            /// Inquisitor abilityes
            /// </summary>
            /// <returns>List of inquisitor ability</returns>
            public static List<MechEngine.Ability> Inquisitor()
            { return new List<MechEngine.Ability> { Banishment(), SummonAlly(), AngelAttack(), DemonAttack() }; }
            /// <summary>
            /// Fire mage abilityes
            /// </summary>
            /// <returns>List of fire mage ability</returns>
            public static List<MechEngine.Ability> FireMage()
            { return new List<MechEngine.Ability> { InfernoBlast(), FireShield(), FireWeapon(), SummonElemental() }; }
            /// <summary>
            /// Assassin abilityes
            /// </summary>
            /// <returns>List of assassin ability</returns>
            public static List<MechEngine.Ability> Assassin()
            { return new List<MechEngine.Ability> { JustPoison(), DeadlyPoison(), RemissivePoison(), PutTrap() }; }
            /// <summary>
            /// Shaman abilityes
            /// </summary>
            /// <returns>List of shaman ability</returns>
            public static List<MechEngine.Ability> Shaman()
            { return new List<MechEngine.Ability> { PoisonNova(), WolfPower(), TigerPower(), ShamanPin() }; }
            /// <summary>
            /// Necromant abilityes
            /// </summary>
            /// <returns>List of necromant ability</returns>
            public static List<MechEngine.Ability> Necromant()
            { return new List<MechEngine.Ability> { SummonSceleton(), BoneSpear(), SummonSpirit(), TakeSoul() }; }
            /// <summary>
            /// Monk abilityes
            /// </summary>
            /// <returns>List of monk ability</returns>
            public static List<MechEngine.Ability> Monk()
            { return new List<MechEngine.Ability> { ChangeSpecialization(), PrayerOfDead(), StoneFist(), StaffPower() }; }
            /// <summary>
            /// Alchemist abilityes
            /// </summary>
            /// <returns>List of alchemist ability</returns>
            public static List<MechEngine.Ability> Alchemist()
            { return new List<MechEngine.Ability> { RainbowBolt(), BottleOfElements(), BrewPotion(), Alchemism() }; }
            /// <summary>
            /// Warrior abilityes
            /// </summary>
            /// <returns>List of warrior ability</returns>
            public static List<MechEngine.Ability> Warrior
            { get { return new List<MechEngine.Ability> { Suppress, Regeneration, Finish, Rage }; } }
            /// <summary>
            /// Deal damage if enemy full hp
            /// </summary>
            /// <returns></returns>
            public static MechEngine.Ability Suppress
            {
                get
                {
                    MechEngine.Ability fk = new MechEngine.Ability();
                    fk.Level = 0;
                    fk.LVRate = 0.39;
                    fk.Mode = MechEngine.AbilityType.Active;
                    fk.Power = 1;
                    fk.Class = MechEngine.BattleClass.Warrior;
                    fk.Info = "Опыт война.\n\nОсобый прием.\n\nЕсли у врага полное здоровье\n-\nнаносит & урона.\nИначе 0.";
                    fk.Rate = MechEngine.AbilityRate.AttackDamage;
                    fk.ADRate = 2.34;
                    fk.CostRate = 10;
                    fk.Elem = MechEngine.AbilityElement.Physical;
                    fk.Duration = 0;
                    fk.Name = "Усмирить";
                    fk.Icon = '╥';
                    fk.Color = ConsoleColor.Blue;
                    fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Damage, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.DmgHealInstant } } };
                    fk.Location = MechEngine.AbilityLocation.Combat;
                    return fk;
                }
            }
            /// <summary>
            /// Regenerate hp in battle
            /// </summary>
            /// <returns></returns>
            private static MechEngine.Ability Regeneration
            {
                get
                {
                    MechEngine.Ability fk = new MechEngine.Ability();
                    fk.Level = 0;
                    fk.LVRate = 0.21;
                    fk.Mode = MechEngine.AbilityType.Active;
                    fk.Power = 1;
                    fk.Class = MechEngine.BattleClass.Warrior;
                    fk.Rate = MechEngine.AbilityRate.AbilityPower;
                    fk.Duration = 0;
                    fk.APRate = 1.93;
                    fk.CostRate = 30;
                    fk.Elem = MechEngine.AbilityElement.Physical;
                    fk.Name = "Регенерация";
                    fk.Info = "Жажда битвы.\n\nУмение.\n\nВосстанавливает ^ здоровья.";
                    fk.Icon = '=';
                    fk.Color = ConsoleColor.Red;
                    fk.Location = MechEngine.AbilityLocation.Combat;
                    fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Heal, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.DmgHealInstant } } };
                    return fk;
                }
            }
            /// <summary>
            /// If enemy hp less 25% deal damage
            /// </summary>
            /// <returns></returns>
            private static MechEngine.Ability Finish
            {
                get
                {
                    MechEngine.Ability fk = new MechEngine.Ability();
                    fk.Level = 0;
                    fk.LVRate = 0.8;
                    fk.Mode = MechEngine.AbilityType.Active;
                    fk.Power = 1;
                    fk.Class = MechEngine.BattleClass.Warrior;
                    fk.Rate = MechEngine.AbilityRate.AttackDamage;
                    fk.ADRate = 1.36;
                    fk.CostRate = 50;
                    fk.Elem = MechEngine.AbilityElement.Physical;
                    fk.Name = "Добить";
                    fk.Info = "Опыт война.\n\nОсобый прием.\n\nЕсли здоровье врага меньше 25%\n-\nнаносит & урона.\nИначе 0.";
                    fk.Icon = '%';
                    fk.Duration = 0;
                    fk.Color = ConsoleColor.Blue;
                    fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Damage, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.DmgHealInstant } } };
                    fk.Location = MechEngine.AbilityLocation.Combat;
                    return fk;
                }
            }
            /// <summary>
            /// Passive rage bonus
            /// </summary>
            /// <returns></returns>
            private static MechEngine.Ability Rage
            {
                get
                {
                    MechEngine.Ability fk = new MechEngine.Ability();
                    fk.Class = MechEngine.BattleClass.Warrior;
                    fk.Level = 0;
                    fk.LVRate = 1;
                    fk.Mode = MechEngine.AbilityType.Passive;
                    fk.Power = 0;
                    fk.Rate = MechEngine.AbilityRate.AttackDamage;
                    fk.ADRate = 0;
                    fk.CostRate = 0;
                    fk.Elem = MechEngine.AbilityElement.Physical;
                    fk.Name = "Ярость";
                    fk.Info = "Опыт война.\n\nТранс.\n\nУвеличивает прирост ярости от удара на &.";
                    fk.Icon = '┼';
                    fk.Color = ConsoleColor.Red;
                    fk.Duration = 0;
                    fk.Location = MechEngine.AbilityLocation.Alltime;
                    fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Improve, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.DmgHealInstant } } };
                    return fk;
                }
            }
            /// <summary>
            /// Active ability, deal damage and heal blood mage
            /// </summary>
            /// <returns></returns>
            public static MechEngine.Ability Vampirism()
            {
                MechEngine.Ability fk = new MechEngine.Ability();
                fk.Level = 0;
                fk.LVRate = 0.6;
                fk.Mode = MechEngine.AbilityType.Active;
                fk.Power = 1;
                fk.Class = MechEngine.BattleClass.BloodMage;
                fk.Info = "Забирает кровь у врага.\n\nНаносит & урона\nИсцеляет ^ жизни магу.";
                fk.Rate = MechEngine.AbilityRate.AbilityPower;
                fk.APRate = 1.34;
                fk.CostRate = 5;
                fk.Elem = MechEngine.AbilityElement.BloodMagic;
                fk.Duration = 0;
                fk.Name = "Вампиризм";
                fk.Icon = '╥';
                fk.Color = ConsoleColor.DarkRed;
                fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Damage, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.DmgHealInstant } },
                new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Heal, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.DmgHealInstant } }};
                fk.Location = MechEngine.AbilityLocation.Combat;
                return fk;
            }
            /// <summary>
            /// Active, damage enemy and cost blood
            /// </summary>
            /// <returns></returns>
            private static MechEngine.Ability BloodSpear()
            {
                MechEngine.Ability fk = new MechEngine.Ability();
                fk.Level = 0;
                fk.LVRate = 0.21;
                fk.Mode = MechEngine.AbilityType.Active;
                fk.Power = 1;
                fk.Class = MechEngine.BattleClass.BloodMage;
                fk.Rate = MechEngine.AbilityRate.AbilityPower;
                fk.Duration = 0;
                fk.APRate = 1.73;
                fk.CostRate = 10;
                fk.Elem = MechEngine.AbilityElement.BloodMagic;
                fk.Name = "Копьё крови";
                fk.Info = "Трансформация крови.\n\nПревращает кровь в твердое копьё.\nНаносит & урона врагу.";
                fk.Icon = '>';
                fk.Color = ConsoleColor.Red;
                fk.Location = MechEngine.AbilityLocation.Combat;
                fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Damage, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.DmgHealInstant } } };
                return fk;
            }
            /// <summary>
            /// Cost health, give some ARM and MRS
            /// </summary>
            /// <returns></returns>
            private static MechEngine.Ability BloodShield()
            {
                MechEngine.Ability fk = new MechEngine.Ability();
                fk.Level = 0;
                fk.LVRate = 0.2;
                fk.Mode = MechEngine.AbilityType.Active;
                fk.Power = 1;
                fk.Class = MechEngine.BattleClass.BloodMage;
                fk.Rate = MechEngine.AbilityRate.AbilityPower;
                fk.APRate = 1.36;
                fk.CostRate = 20;
                fk.Elem = MechEngine.AbilityElement.BloodMagic;
                fk.Name = "Щит крови";
                fk.Info = "Трансформация крови.\n\nПревращает кровь в щит.\nУвеличивает\nФиз. защиту, Маг. защиту и Силу магии\nна #";
                fk.Icon = '#';
                fk.Color = ConsoleColor.DarkRed;
                fk.Stats = new List<MechEngine.AbilityStats> { MechEngine.AbilityStats.ARM, MechEngine.AbilityStats.MRS, MechEngine.AbilityStats.AP };
                fk.Duration = 60;
                fk.Location = MechEngine.AbilityLocation.Alltime;
                fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Improve, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.EffectOfTime } } };
                return fk;
            }
            /// <summary>
            /// Blood mage stand powerful Ghoul who have more DMG, ARM and MRS
            /// </summary>
            /// <returns></returns>
            private static MechEngine.Ability Ghoul()
            {
                MechEngine.Ability fk = new MechEngine.Ability();
                fk.Level = 0;
                fk.LVRate = 0.97;
                fk.Mode = MechEngine.AbilityType.Active;
                fk.Power = 1;
                fk.Class = MechEngine.BattleClass.BloodMage;
                fk.Rate = MechEngine.AbilityRate.AttackDamage;
                fk.ADRate = 1.21;
                fk.CostRate = 35;
                fk.Elem = MechEngine.AbilityElement.BloodMagic;
                fk.Name = "Вурдалак";
                fk.Info = "Превращение.\n\nВы изменяете свою форму.\nФорма вурдалака позволяет царапать врагов.\nСила атаки, Урон, Здоровье +#";
                fk.Icon = '☼';
                fk.Form = new MechEngine.Morphling('☼', ConsoleColor.Green);
                fk.Color = ConsoleColor.Green;
                fk.Duration = 30;
                fk.Stats = new List<MechEngine.AbilityStats> { MechEngine.AbilityStats.AD, MechEngine.AbilityStats.DMG, MechEngine.AbilityStats.MHP };
                fk.Location = MechEngine.AbilityLocation.Alltime;
                fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Improve, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.EffectOfTime } } };
                return fk;
            }
            /// <summary>
            /// Active, heal paladin
            /// </summary>
            /// <returns></returns>
            public static MechEngine.Ability HolyLight()
            {
                MechEngine.Ability fk = new MechEngine.Ability();
                fk.Class = MechEngine.BattleClass.Paladin;
                fk.Level = 0;
                fk.LVRate = 0.3;
                fk.Mode = MechEngine.AbilityType.Active;
                fk.Power = 4;
                fk.Rate = MechEngine.AbilityRate.AbilityPower;
                fk.APRate = 1.2;
                fk.CostRate = 15;
                fk.Elem = MechEngine.AbilityElement.HolyMagic;
                fk.Name = "Свет небес";
                fk.Info = "Светлая магия.\n\nЛуч света с небес исцеляет паладина.\nИсцеляет ^ урона.";
                fk.Icon = '▼';
                fk.Color = ConsoleColor.Yellow;
                fk.Duration = 0;
                fk.Location = MechEngine.AbilityLocation.Alltime;
                fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Heal, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.DmgHealInstant } } };
                return fk;
            }
            /// <summary>
            /// Active, damage all enemy and heal all ally
            /// </summary>
            /// <returns></returns>
            public static MechEngine.Ability HolyNova()
            {
                MechEngine.Ability fk = new MechEngine.Ability();
                fk.Class = MechEngine.BattleClass.Paladin;
                fk.Level = 0;
                fk.LVRate = 0.4;
                fk.Mode = MechEngine.AbilityType.Active;
                fk.Power = 1;
                fk.Rate = MechEngine.AbilityRate.AbilityPower;
                fk.APRate = 1.21;
                fk.CostRate = 40;
                fk.Elem = MechEngine.AbilityElement.HolyMagic;
                fk.Name = "Столп света";
                fk.Info = "▲\nСветлая магия.\n\nРазрушающая магия.\n\nВ радиусе действия [8]\n исцеляет (^ * Кол.Врагов).\nИ наносит & урона каждому врагу.";
                fk.Icon = '■';
                fk.AOE = 9;
                fk.Color = ConsoleColor.Yellow;
                fk.Duration = 0;
                fk.Location = MechEngine.AbilityLocation.WorldMap;
                fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Destruction, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.DmgHealInstant } } };
                return fk;
            }
            /// <summary>
            /// Deal damage, for evil double damage
            /// </summary>
            /// <returns></returns>
            private static MechEngine.Ability TurnEvil()
            {
                MechEngine.Ability fk = new MechEngine.Ability();
                fk.Class = MechEngine.BattleClass.Paladin;
                fk.Level = 0;
                fk.LVRate = 1.0;
                fk.Mode = MechEngine.AbilityType.Active;
                fk.Power = 1;
                fk.Rate = MechEngine.AbilityRate.AttackDamage;
                fk.ADRate = 2.0;
                fk.CostRate = 10;
                fk.Elem = MechEngine.AbilityElement.HolyMagic;
                fk.Name = "Изгнать зло";
                fk.Info = "Светлая магия.\n\nНаносит врагу ^ урона.\n Урон будет нанесёт только злым.";
                fk.Icon = '\\';
                fk.Color = ConsoleColor.DarkYellow;
                fk.Duration = 0;
                fk.Location = MechEngine.AbilityLocation.Combat;
                fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Damage, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.DmgHealInstant } } };
                return fk;
            }
            /// <summary>
            /// Deal holy damage for one enemy
            /// </summary>
            /// <returns></returns>
            private static MechEngine.Ability LightStar()
            {
                MechEngine.Ability fk = new MechEngine.Ability();
                fk.Class = MechEngine.BattleClass.Paladin;
                fk.Level = 0;
                fk.LVRate = 0.3;
                fk.Mode = MechEngine.AbilityType.Active;
                fk.Power = 3;
                fk.Rate = MechEngine.AbilityRate.AttackDamage;
                fk.ADRate = 1.41;
                fk.CostRate = 45;
                fk.Elem = MechEngine.AbilityElement.HolyMagic;
                fk.Name = "Удар светом";
                fk.Info = "Светлая магия.\n\nМогучий удар.\n Наносит ^ урона врагу\nУдар основан на божественной силе.";
                fk.Icon = '√';
                fk.Color = ConsoleColor.DarkYellow;
                fk.Duration = 0;
                fk.Location = MechEngine.AbilityLocation.Combat;
                fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Damage, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.DmgHealInstant } } };
                return fk;
            }
            /// <summary>
            /// Turn evil or good enemy from map
            /// </summary>
            /// <returns></returns>
            private static MechEngine.Ability Banishment()
            {
                MechEngine.Ability fk = new MechEngine.Ability();
                fk.Class = MechEngine.BattleClass.Inquisitor;
                fk.Level = 0;
                fk.LVRate = 1.79;
                fk.Mode = MechEngine.AbilityType.Active;
                fk.Power = 1;
                fk.Rate = MechEngine.AbilityRate.AbilityPower;
                fk.APRate = 1.3;
                fk.CostRate = 1;
                fk.Elem = MechEngine.AbilityElement.DemonMagic;
                fk.Name = "Изгнание";
                fk.Info = "Тайны Экзорцизма.\n\nУничтожение.\nИзгоняет выбранное существо.\nВыберите мировоззрение и направление.\nБудет сделанна попытка изгнания.";
                fk.Icon = '¤';
                fk.Color = ConsoleColor.DarkGray;
                fk.Duration = 0;
                fk.Location = MechEngine.AbilityLocation.WorldMap;
                fk.Dest = MechEngine.AbilityDestructionType.Objects;
                fk.AOE = 1;
                fk.Menu = "Banish";
                fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Destruction, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.DmgHealInstant } } };
                return fk;
            }
            /// <summary>
            /// Summon ally, good or evil
            /// </summary>
            /// <returns></returns>
            private static MechEngine.Ability SummonAlly()
            {
                MechEngine.Ability fk = new MechEngine.Ability();
                fk.Class = MechEngine.BattleClass.Inquisitor;
                fk.Level = 0;
                fk.LVRate = 0.1;
                fk.Mode = MechEngine.AbilityType.Active;
                fk.Power = 1;
                fk.Rate = MechEngine.AbilityRate.AbilityPower;
                fk.APRate = 1.1;
                fk.CostRate = 1;
                fk.Elem = MechEngine.AbilityElement.DemonMagic;
                fk.Name = "Вызов";
                fk.Info = "Пленники равновесия.\n\nПомощник.\nЗависит от последнего изгнания.\nПризывает ангела или демона.\nПомощник атакует врага и лечит вас.";
                fk.Icon = '@';
                fk.Color = ConsoleColor.Cyan;
                fk.Duration = 60;
                fk.SummonMonster = SummonedBase.InquisitorUnit();
                fk.SummonBlock = true;
                fk.Location = MechEngine.AbilityLocation.WorldMap;
                fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Summon, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.EffectOfTime } } };
                return fk;
            }
            /// <summary>
            /// Deal holy damage with AP rate
            /// </summary>
            /// <returns></returns>
            private static MechEngine.Ability AngelAttack()
            {
                MechEngine.Ability fk = new MechEngine.Ability();
                fk.Class = MechEngine.BattleClass.Inquisitor;
                fk.Level = 0;
                fk.LVRate = 0.82;
                fk.Mode = MechEngine.AbilityType.Active;
                fk.Power = 1;
                fk.Rate = MechEngine.AbilityRate.AbilityPower;
                fk.APRate = 1.5;
                fk.CostRate = 1;
                fk.Elem = MechEngine.AbilityElement.HolyMagic;
                fk.Name = "Удар ангела";
                fk.Info = "Правый кулак власти.\n\nБоевое исскуство.\nНаносит удар заряженный ангельским дыханием.\nНаносит & урона.";
                fk.Icon = '↑';
                fk.Color = ConsoleColor.Cyan;
                fk.Duration = 0;
                fk.Location = MechEngine.AbilityLocation.Combat;
                fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Damage, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.DmgHealInstant } } };
                return fk;
            }
            /// <summary>
            /// Deal demon damage with AD rate
            /// </summary>
            /// <returns></returns>
            private static MechEngine.Ability DemonAttack()
            {
                MechEngine.Ability fk = new MechEngine.Ability();
                fk.Class = MechEngine.BattleClass.Inquisitor;
                fk.Level = 0;
                fk.LVRate = 0.66;
                fk.Mode = MechEngine.AbilityType.Active;
                fk.Power = 1;
                fk.Rate = MechEngine.AbilityRate.AttackDamage;
                fk.ADRate = 1.75;
                fk.CostRate = 1;
                fk.Elem = MechEngine.AbilityElement.HolyMagic;
                fk.Name = "Удар демона";
                fk.Info = "Левый кулак ненависти.\n\nБоевое исскуство.\nНаносит удар проклятый пленными демонами.\nНаносит & урона.";
                fk.Icon = '↓';
                fk.Color = ConsoleColor.DarkRed;
                fk.Duration = 0;
                fk.Location = MechEngine.AbilityLocation.Combat;
                fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Damage, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.DmgHealInstant } } };
                return fk;
            }
            /// <summary>
            /// Deal solo target damage from fire
            /// </summary>
            /// <returns></returns>
            public static MechEngine.Ability InfernoBlast()
            {
                MechEngine.Ability fk = new MechEngine.Ability();
                fk.Class = MechEngine.BattleClass.FireMage;
                fk.Level = 0;
                fk.LVRate = 0.57;
                fk.Mode = MechEngine.AbilityType.Active;
                fk.Power = 1;
                fk.Rate = MechEngine.AbilityRate.AbilityPower;
                fk.APRate = 1.24;
                fk.CostRate = 5;
                fk.Elem = MechEngine.AbilityElement.FireMagic;
                fk.Info = "Ад.\n\nВзрыв.\n\nОгненная магия.\n\nНаносит удар стрелой пламени на & урона.";
                fk.Name = "Стрела огня";
                fk.Icon = '→';
                fk.Color = ConsoleColor.Red;
                fk.Duration = 0;
                fk.Location = MechEngine.AbilityLocation.Combat;
                fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Damage, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.DmgHealInstant } } };
                return fk;
            }
            /// <summary>
            /// Deal fire damage from each strike //NOPE.
            /// </summary>
            /// <returns></returns>
            private static MechEngine.Ability FireShield()
            {
                MechEngine.Ability fk = new MechEngine.Ability();
                fk.Class = MechEngine.BattleClass.FireMage;
                fk.Level = 0;
                fk.LVRate = 0.81;
                fk.Mode = MechEngine.AbilityType.Active;
                fk.Power = 1;
                fk.Rate = MechEngine.AbilityRate.AbilityPower;
                fk.APRate = 1.47;
                fk.CostRate = 35;
                fk.Elem = MechEngine.AbilityElement.FireMagic;
                fk.Name = "Щит огня";
                fk.Info = "Огненная магия.\n\nОбволакивает мага щитом из пламени.\nПри использовании:\nНаносит & урона одному врагу или вокруг.\nУвеличивает Силу магии и Маг. Защиту\nна #.";
                fk.Icon = '▒';
                fk.Color = ConsoleColor.Red;
                fk.Stats = new List<MechEngine.AbilityStats> { MechEngine.AbilityStats.AP, MechEngine.AbilityStats.MRS };
                fk.Duration = 15;
                fk.Location = MechEngine.AbilityLocation.Alltime;
                fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Damage, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.DmgHealInstant } },
                new MechEngine.AbilityAction(){Act= MechEngine.AbilityActionType.Improve, Atr= new List<MechEngine.AbilityActionAttribute>(){MechEngine.AbilityActionAttribute.EffectOfTime}}};
                return fk;
            }
            /// <summary>
            /// Summon fire weapon equal mage level
            /// </summary>
            /// <returns></returns>
            private static MechEngine.Ability FireWeapon()
            {
                MechEngine.Ability fk = new MechEngine.Ability();
                fk.Class = MechEngine.BattleClass.FireMage;
                fk.Level = 0;
                fk.LVRate = 1.1;
                fk.Mode = MechEngine.AbilityType.Active;
                fk.Power = 1;
                fk.Rate = MechEngine.AbilityRate.AbilityPower;
                fk.APRate = 1.15;
                fk.CostRate = 70;
                fk.Elem = MechEngine.AbilityElement.FireMagic;
                fk.Name = "Оружие огня";
                fk.Info = "Огнепоклонничество.\n\nРемесло.\nСоздаёт посох заряженный огнём.\nПри ношении Посох увеличивает\nМагическую силу на #.";
                fk.Icon = '┤';
                fk.Color = ConsoleColor.Red;
                fk.Duration = 0;
                fk.Location = MechEngine.AbilityLocation.WorldMap;
                fk.CraftItem = ItemBase.FireStaff;
                fk.CraftGetName = "FireStaff";
                fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Craft, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.DmgHealInstant } } };
                return fk;
            }
            /// <summary>
            /// Summon fire elemental and damage all enemyes
            /// </summary>
            /// <returns></returns>
            private static MechEngine.Ability SummonElemental()
            {
                MechEngine.Ability fk = new MechEngine.Ability();
                fk.Class = MechEngine.BattleClass.FireMage;
                fk.Level = 0;
                fk.LVRate = 0.67;
                fk.Mode = MechEngine.AbilityType.Active;
                fk.Power = 0;//1;
                fk.Rate = MechEngine.AbilityRate.AbilityPower;
                fk.APRate = 1.24;
                fk.CostRate = 25;
                fk.Elem = MechEngine.AbilityElement.FireMagic;
                fk.Name = "Дух огня";
                fk.Info = "Огнепоклонничество.\n\nГнев.\n\nПризывает на помощь духа огня.\nКогда Дух огня появляется\nнаносит & урона врагу.\nДух огня атакует врага на ‡\n и\n снижает его защиту на ;/2";
                fk.Icon = '╬';
                fk.Color = ConsoleColor.Red;
                fk.SummonMonster = SummonedBase.FireElemental();
                fk.SummonBlock = true;
                fk.Duration = 10;
                fk.DHoTtiks = 3;
                fk.Location = MechEngine.AbilityLocation.Alltime;
                fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Summon, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.EffectOfTime } } };
                return fk;
            }
            /// <summary>
            /// Add poison damage to weapon
            /// </summary>
            /// <returns></returns>
            private static MechEngine.Ability JustPoison()
            {
                MechEngine.Ability fk = new MechEngine.Ability();
                fk.Class = MechEngine.BattleClass.Assassin;
                fk.Level = 0;
                fk.LVRate = 0.1;
                fk.Mode = MechEngine.AbilityType.Active;
                fk.Power = 1;
                fk.Rate = MechEngine.AbilityRate.AttackDamage;
                fk.ADRate = 1.34;
                fk.CostRate = 1;
                fk.Elem = MechEngine.AbilityElement.NatureMagic;
                fk.Name = "Простой яд";
                fk.Info = "Мастерство убийцы.\n\nЯды.\n\nНаносит простой яд на оружие.\nУвеличивает урон от каждого удара на ^.";
                fk.Icon = '~';                
                fk.Color = ConsoleColor.Green;
                fk.Duration = 120;
                fk.Stats = new List<MechEngine.AbilityStats> { MechEngine.AbilityStats.DMG };
                fk.Location = MechEngine.AbilityLocation.Alltime;
                fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Improve, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.EffectOfTime } } };
                return fk;
            }
            /// <summary>
            /// Deadly poison, have shance equal power kill enemy
            /// </summary>
            /// <returns></returns>
            private static MechEngine.Ability DeadlyPoison()
            {
                MechEngine.Ability fk = new MechEngine.Ability();
                fk.Class = MechEngine.BattleClass.Assassin;
                fk.Level = 0;
                fk.LVRate = 0.37;
                fk.Mode = MechEngine.AbilityType.Active;
                fk.Power = 1;
                fk.Rate = MechEngine.AbilityRate.AttackDamage;
                fk.ADRate = 1.24;
                fk.CostRate = 1;
                fk.Elem = MechEngine.AbilityElement.NatureMagic;
                fk.Name = "Смертельный яд";
                fk.Info = "Мастерство убийцы.\n\nЯды.\n\nОтравляет противника смертельным ядом.\nКаждые < секунд есть шанс равный @%\n что враг мгновенно умрёт.";
                fk.Icon = '╤';
                fk.Color = ConsoleColor.Green;
                fk.Duration = 60;
                fk.DHoTtiks = 2;
                fk.Location = MechEngine.AbilityLocation.Combat;
                fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Damage, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.DmgHealOnTime } } };
                return fk;
            }
            /// <summary>
            /// Remissive, enemy will have low attack damage and ability power
            /// </summary>
            /// <returns></returns>
            private static MechEngine.Ability RemissivePoison()
            {
                MechEngine.Ability fk = new MechEngine.Ability();
                fk.Class = MechEngine.BattleClass.Assassin;
                fk.Level = 0;
                fk.LVRate = 0.78;
                fk.Mode = MechEngine.AbilityType.Active;
                fk.Power = 1;
                fk.Rate = MechEngine.AbilityRate.AttackDamage;
                fk.ADRate = 1.24;
                fk.CostRate = 1;
                fk.Elem = MechEngine.AbilityElement.NatureMagic;
                fk.Name = "Ослабляющий яд";
                fk.Info = "Мастерство убийцы.\n\nЯды.\n\nОтравляет противника ослабляющим ядом.\nУменьшает Силу атаки и Силу магии врага\nна ;.";
                fk.Icon = ';';
                fk.Color = ConsoleColor.Green;
                fk.Duration = 180;
                fk.Location = MechEngine.AbilityLocation.Combat;
                fk.Stats = new List<MechEngine.AbilityStats> { MechEngine.AbilityStats.AD, MechEngine.AbilityStats.AP };
                fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Debuff, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.EffectOfTime} } };
                return fk;
            }
            /// <summary>
            /// Put trap, damage equal power
            /// </summary>
            /// <returns></returns>
            private static MechEngine.Ability PutTrap()
            {
                MechEngine.Ability fk = new MechEngine.Ability();
                fk.Class = MechEngine.BattleClass.Assassin;
                fk.Level = 0;
                fk.LVRate = 0.97;
                fk.Mode = MechEngine.AbilityType.Active;
                fk.Power = 1;
                fk.Rate = MechEngine.AbilityRate.AttackDamage;
                fk.ADRate = 1.21;
                fk.CostRate = 1;
                fk.Elem = MechEngine.AbilityElement.ElementalMagic;
                fk.Name = "Ловушка";
                fk.Info = "Мастерство вора.\n\nЛовкость.\n\nСтавит ловушку на земле.\nВыберите элемент урона и направление.\nЕсли вы наступите на ловушку она останется.\nЕсли враг наступит на ловушку она\nнанесёт & урона.";
                fk.Icon = '¤';
                fk.Color = ConsoleColor.Green;
                fk.Duration = 0;
                fk.Menu = "Trap";
                fk.Location = MechEngine.AbilityLocation.WorldMap;
                fk.Dest = MechEngine.AbilityDestructionType.Objects;
                fk.AOE = 1;
                fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Destruction, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.DmgHealInstant } } };
                return fk;
            }
            /// <summary>
            /// Deal damage all enemy with poison
            /// </summary>
            /// <returns></returns>
            public static MechEngine.Ability PoisonNova()
            {
                MechEngine.Ability fk = new MechEngine.Ability();
                fk.Class = MechEngine.BattleClass.Shaman;
                fk.Level = 0;
                fk.LVRate = 0.5;
                fk.Mode = MechEngine.AbilityType.Active;
                fk.Power = 1;
                fk.Rate = MechEngine.AbilityRate.AbilityPower;
                fk.APRate = 5.24;
                fk.CostRate = 10;
                fk.Elem = MechEngine.AbilityElement.NatureMagic;
                fk.Name = "Кольцо яда";
                fk.Info = "Древний вудуизм.\n\nМогущество гнили.\n\nВ радиусе действия [8]\n наносит & урона каждому врагу.";
                fk.Icon = '@';
                fk.Color = ConsoleColor.Green;
                fk.Duration = 60;
                fk.Location = MechEngine.AbilityLocation.WorldMap;
                fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Damage, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.DmgHealInstant } } };
                return fk;
            }
            /// <summary>
            /// Upgrade character attack damage from ability power
            /// </summary>
            /// <returns></returns>
            public static MechEngine.Ability WolfPower()
            {
                MechEngine.Ability fk = new MechEngine.Ability();
                fk.Class = MechEngine.BattleClass.Shaman;
                fk.Level = 0;
                fk.LVRate = 0.37;
                fk.Mode = MechEngine.AbilityType.Active;
                fk.Power = 1;
                fk.Rate = MechEngine.AbilityRate.AbilityPower;
                fk.APRate = 2.47;
                fk.CostRate = 5;
                fk.Elem = MechEngine.AbilityElement.NatureMagic;
                fk.Name = "Сила волка";
                fk.Info = "Настоящий вудуизм.\n\nОборотничество.\n\nУвеличивает Силу атаки за счет Силы магии\nна #.";
                fk.Icon = '(';
                fk.Color = ConsoleColor.Red;
                fk.Duration = 10;
                fk.Location = MechEngine.AbilityLocation.Alltime;
                fk.Stats = new List<MechEngine.AbilityStats> { MechEngine.AbilityStats.AD };
                fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Improve, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.EffectOfTime } } };
                return fk;
            }
            /// <summary>
            /// Upgrade character ability power from attack damage
            /// </summary>
            /// <returns></returns>
            private static MechEngine.Ability TigerPower()
            {
                MechEngine.Ability fk = new MechEngine.Ability();
                fk.Class = MechEngine.BattleClass.Shaman;
                fk.Level = 0;
                fk.LVRate = 2.37;
                fk.Mode = MechEngine.AbilityType.Active;
                fk.Power = 1;
                fk.Rate = MechEngine.AbilityRate.AttackDamage;
                fk.ADRate = 1.76;
                fk.CostRate = 5;
                fk.Elem = MechEngine.AbilityElement.NatureMagic;
                fk.Name = "Сила тигра";
                fk.Info = "Настоящий вудуизм.\n\nОборотничество.\n\nУвеличивает Силу магии за счет Силы атаки\nна #.";
                fk.Icon = ')';
                fk.Color = ConsoleColor.Blue;
                fk.Duration = 10;
                fk.Stats = new List<MechEngine.AbilityStats> { MechEngine.AbilityStats.AP};
                fk.Location = MechEngine.AbilityLocation.Alltime;
                fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Improve, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.EffectOfTime } } };
                return fk;
            }
            /// <summary>
            /// Deal damage from percent of mana
            /// </summary>
            /// <returns></returns>
            private static MechEngine.Ability ShamanPin()
            {
                MechEngine.Ability fk = new MechEngine.Ability();
                fk.Class = MechEngine.BattleClass.Shaman;
                fk.Level = 1;
                fk.LVRate = 0;
                fk.Mode = MechEngine.AbilityType.Active;
                fk.Power = 0;
                fk.Rate = MechEngine.AbilityRate.AttackDamage;
                fk.ADRate = 1.21;
                fk.CostRate = 1;
                fk.Elem = MechEngine.AbilityElement.NatureMagic;
                fk.Name = "Булавка %";
                fk.Info = "Новейший вудуизм.\n\nДревняя магия.\n\nНаносит урон врагу равный:\n@% от текущей манны.";
                fk.Icon = '0';
                fk.Color = ConsoleColor.Yellow;
                fk.Duration = 0;
                fk.Location = MechEngine.AbilityLocation.Combat;
                fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Damage, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.DmgHealInstant } } };
                return fk;
            }
            /// <summary>
            /// Summon sceleton for help
            /// </summary>
            /// <returns></returns>
            public static MechEngine.Ability SummonSceleton()
            {
                MechEngine.Ability fk = new MechEngine.Ability();
                fk.Class = MechEngine.BattleClass.Necromant;
                fk.Level = 0;
                fk.LVRate = 0.05;
                fk.Mode = MechEngine.AbilityType.Active;
                fk.Power = 1;
                fk.Rate = MechEngine.AbilityRate.AbilityPower;
                fk.APRate = 1.005;
                fk.CostRate = 1;
                fk.Elem = MechEngine.AbilityElement.DeadMagic;
                fk.Name = "Призвать скелета";
                fk.Info = "Некромантия.\n\nМатериальный мир.\n\nПризывает скелета с атакой равной\n‡.\nСкелет атакует врага каждую атаку.";
                fk.Icon = 'S';
                fk.Color = ConsoleColor.White;
                fk.SummonMonster = SummonedBase.Sceleton();
                fk.SummonBlock = true;
                fk.Duration = 40;
                fk.Location = MechEngine.AbilityLocation.Alltime;
                fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Summon, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.EffectOfTime } } };
                return fk;
            }
            /// <summary>
            /// Deal dead damage single target
            /// </summary>
            /// <returns></returns>
            private static MechEngine.Ability BoneSpear()
            {
                MechEngine.Ability fk = new MechEngine.Ability();
                fk.Class = MechEngine.BattleClass.Necromant;
                fk.Level = 0;
                fk.LVRate = 0.67;
                fk.Mode = MechEngine.AbilityType.Active;
                fk.Power = 1;
                fk.Rate = MechEngine.AbilityRate.AbilityPower;
                fk.APRate = 1.61;
                fk.CostRate = 1;
                fk.Elem = MechEngine.AbilityElement.DeadMagic;
                fk.Name = "Копьё кости";
                fk.Info = "Некромантия.\n\nМатериальный мир.\n\nПризывает скелет и создаёт из него копьё.\nКопьё наносит & урона.";
                fk.Icon = '¶';
                fk.Color = ConsoleColor.White;
                fk.Duration = 0;
                fk.Location = MechEngine.AbilityLocation.Combat;
                fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Damage, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.DmgHealInstant } } };
                return fk;
            }
            /// <summary>
            /// Summon Spirit for help
            /// </summary>
            /// <returns></returns>
            private static MechEngine.Ability SummonSpirit()
            {
                MechEngine.Ability fk = new MechEngine.Ability();
                fk.Class = MechEngine.BattleClass.Necromant;
                fk.Level = 0;
                fk.LVRate = 0.05;
                fk.Mode = MechEngine.AbilityType.Active;
                fk.Power = 1;
                fk.Rate = MechEngine.AbilityRate.AbilityPower;
                fk.APRate = 1.005;
                fk.CostRate = 1;
                fk.Elem = MechEngine.AbilityElement.DeadMagic;
                fk.Name = "Призвать духа";
                fk.Info = "Некромантия.\n\nДуховный мир.\n\nПризывает духа с атакой равной\n‡\nДух лечит некроманта каждую атаку.";
                fk.Icon = 'Y';
                fk.Color = ConsoleColor.White;
                fk.SummonMonster = SummonedBase.Spirit();
                fk.SummonBlock = true;
                fk.Duration = 20;
                fk.Location = MechEngine.AbilityLocation.Alltime;
                fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Summon, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.EffectOfTime } } };
                return fk;
            }
            /// <summary>
            /// Take piece of enemy soul and heal character
            /// </summary>
            /// <returns></returns>
            private static MechEngine.Ability TakeSoul()
            {
                MechEngine.Ability fk = new MechEngine.Ability();
                fk.Class = MechEngine.BattleClass.Necromant;
                fk.Level = 0;
                fk.LVRate = 0.94;
                fk.Mode = MechEngine.AbilityType.Active;
                fk.Power = 1;
                fk.Rate = MechEngine.AbilityRate.AbilityPower;
                fk.APRate = 1.78;
                fk.CostRate = 2;
                fk.Elem = MechEngine.AbilityElement.DeadMagic;
                fk.Name = "Взять душу";
                fk.Info = "Некромантия.\n\nДуховный мир.\n\nНекромант вытягивает душу из врага.\nНаносит & урона.";
                fk.Icon = '%';
                fk.Color = ConsoleColor.White;
                fk.Duration = 0;
                fk.Location = MechEngine.AbilityLocation.Combat;
                fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Damage, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.DmgHealInstant } },
                new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Heal, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.DmgHealInstant } }};
                return fk;
            }
            /// <summary>
            /// Change AD to AP and AD to AP
            /// </summary>
            /// <returns></returns>
            private static MechEngine.Ability ChangeSpecialization()
            {
                MechEngine.Ability fk = new MechEngine.Ability();
                fk.Class = MechEngine.BattleClass.Monk;
                fk.Level = 0;
                fk.LVRate = 0;
                fk.Mode = MechEngine.AbilityType.Active;
                fk.Power = 1;
                fk.Rate = MechEngine.AbilityRate.AttackDamage;
                fk.ADRate = 1;
                fk.CostRate = 0;
                fk.Elem = MechEngine.AbilityElement.HolyMagic;
                fk.Name = "Специализация";
                fk.Info = "Баланс.\n\nМонах изменяет баланс в своём теле.\nТекущая характеристика\nСила магии     или     Сила атаки\nМеняется местами.";
                fk.Icon = '±';
                fk.Color = ConsoleColor.Gray;
                fk.Duration = 0;
                fk.Location = MechEngine.AbilityLocation.WorldMap;
                fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Neutral, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.DmgHealInstant } } };
                return fk;
            }
            /// <summary>
            /// Get immune of death
            /// </summary>
            /// <returns></returns>
            private static MechEngine.Ability PrayerOfDead()
            {
                MechEngine.Ability fk = new MechEngine.Ability();
                fk.Class = MechEngine.BattleClass.Monk;
                fk.Level = 1;
                fk.LVRate = 0;
                fk.Mode = MechEngine.AbilityType.Active;
                fk.Power = 1000;
                fk.Rate = MechEngine.AbilityRate.AttackDamage;
                fk.ADRate = 1;
                fk.CostRate = 20;
                fk.Elem = MechEngine.AbilityElement.HolyMagic;
                fk.Name = "Молитва смерти";
                fk.Info = "Баланс.\n\nМонах молится об избавлении от смерти.\nНа короткий промежуток монах будет\nневосприимчив к смерти.";
                fk.Icon = 'x';
                fk.Color = ConsoleColor.Gray;
                fk.Duration = 5;
                //fk.Stats = new List<MechEngine.AbilityStats> { MechEngine.AbilityStats.MHP };
                fk.Location = MechEngine.AbilityLocation.Alltime;
                fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Improve, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.EffectOfTime } } };
                return fk;
            }
            /// <summary>
            /// Deal damage
            /// </summary>
            /// <returns></returns>
            private static MechEngine.Ability StoneFist()
            {
                MechEngine.Ability fk = new MechEngine.Ability();
                fk.Class = MechEngine.BattleClass.Monk;
                fk.Level = 0;
                fk.LVRate = 1.02;
                fk.Mode = MechEngine.AbilityType.Active;
                fk.Power = 1;
                fk.Rate = MechEngine.AbilityRate.AttackDamage;
                fk.ADRate = 1.27;
                fk.CostRate = 5;
                fk.Elem = MechEngine.AbilityElement.Physical;
                fk.Name = "Каменный кулак";
                fk.Info = "Боевое исскуство.\n\nМонашеский орден.\n\nМонах сосредотачивает всю силу в одном ударе.\nНаносит & урона.";
                fk.Icon = '[';
                fk.Color = ConsoleColor.Gray;
                fk.Duration = 0;
                fk.Location = MechEngine.AbilityLocation.Combat;
                fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Damage, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.DmgHealInstant } } };
                return fk;
            }
            /// <summary>
            /// Each strike deal additional damage if character's weapon - staff
            /// </summary>
            /// <returns></returns>
            private static MechEngine.Ability StaffPower()
            {
                MechEngine.Ability fk = new MechEngine.Ability();
                fk.Class = MechEngine.BattleClass.Monk;
                fk.Level = 0;
                fk.LVRate = 0.75;
                fk.Mode = MechEngine.AbilityType.Passive;
                fk.Power = 1;
                fk.Rate = MechEngine.AbilityRate.AttackDamage;
                fk.ADRate = 1.12;
                fk.CostRate = 0;
                fk.Elem = MechEngine.AbilityElement.Physical;
                fk.Name = "Стиль посоха";
                fk.Info = "Боевое исскуство.\n\nМонашеский орден.\n\nКаждый монах обучается бою с посохом.\nЕсли монах носит посох\nего атака увеличивается на &";
                fk.Icon = '|';
                fk.Color = ConsoleColor.Gray;
                fk.Duration = 999;
                fk.Location = MechEngine.AbilityLocation.Alltime;
                fk.Stats = new List<MechEngine.AbilityStats> { MechEngine.AbilityStats.DMG };
                fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Improve, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.DmgHealInstant } } };
                return fk;
            }
            /// <summary>
            /// Deal damage from 4 elements 
            /// </summary>
            /// <returns></returns>
            private static MechEngine.Ability RainbowBolt()
            {
                MechEngine.Ability fk = new MechEngine.Ability();
                fk.Class = MechEngine.BattleClass.Alchemist;
                fk.Level = 0;
                fk.LVRate = 0.24;
                fk.Mode = MechEngine.AbilityType.Active;
                fk.Power = 1;
                fk.Rate = MechEngine.AbilityRate.AbilityPower;
                fk.APRate = 1.37;
                fk.CostRate = 5;
                fk.Elem = MechEngine.AbilityElement.ElementalMagic;
                fk.Info = "Стихийная алхимия.\n\nТрансмогрификация.\n\nАлхимик выпускает бутыль с ? брызгами.\nКаждая капля брызг наносит & урона.";
                fk.Name = "Радужные брызги";
                fk.Icon = '«';
                fk.Color = ConsoleColor.Cyan;
                fk.Duration = 0;
                fk.Location = MechEngine.AbilityLocation.Combat;
                fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Damage, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.DmgHealInstant } }, 
                new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Damage, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.DmgHealInstant } }};
                return fk;
            }
            /// <summary>
            /// Damage enemy, heal character, improve character hp and descrease enemy MRS
            /// </summary>
            /// <returns></returns>
            private static MechEngine.Ability BottleOfElements()
            {
                MechEngine.Ability fk = new MechEngine.Ability();
                fk.Class = MechEngine.BattleClass.Alchemist;
                fk.Level = 0;
                fk.LVRate = 0.15;
                fk.Mode = MechEngine.AbilityType.Active;
                fk.Power = 1;
                fk.Rate = MechEngine.AbilityRate.AbilityPower;
                fk.APRate = 1.21;
                fk.CostRate = 15;
                fk.Elem = MechEngine.AbilityElement.ElementalMagic;
                fk.Name = "Бутыль стихий";
                fk.Info = "Стихийная алхимия.\n\nУничтожение.\n\nАлхимик разбивает бутыль с № элементами.\nКаждый элемент:\nУменьшает врагу характеристику на # \nПервый элемент наносит & урона.\nПоследний элемент исцеляет на ^ урона.";
                fk.Icon = 'U';
                fk.Color = ConsoleColor.Magenta;
                fk.Duration = 0;
                fk.DHoTtiks = 3;
                fk.Stats = new List<MechEngine.AbilityStats> { MechEngine.AbilityStats.MRS, MechEngine.AbilityStats.AP };
                fk.Location = MechEngine.AbilityLocation.Combat;
                fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Damage, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.DmgHealInstant } },
                new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Heal, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.DmgHealInstant } },
                new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Debuff, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.EffectInstant } }};
                return fk;
            }
            /// <summary>
            /// Craft one of 4 potions with power equal power or power/2
            /// </summary>
            /// <returns></returns>
            private static MechEngine.Ability BrewPotion()
            {
                MechEngine.Ability fk = new MechEngine.Ability();
                fk.Class = MechEngine.BattleClass.Alchemist;
                fk.Level = 0;
                fk.LVRate = 0.79;
                fk.Mode = MechEngine.AbilityType.Active;
                fk.Power = 4;
                fk.Rate = MechEngine.AbilityRate.AbilityPower;
                fk.APRate = 1.21;
                fk.CostRate = 10;
                fk.Elem = MechEngine.AbilityElement.ElementalMagic;
                fk.Name = "Сварить зелье";
                fk.Info = "Обычная алхимия.\n\nРемесло.\n\nАлхимик создаёт лечебное зелье.\nЗелье восстанавливает ^ жизни и ~ элементов.";
                fk.Icon = '⌂';
                fk.Color = ConsoleColor.Blue;
                fk.Duration = 0;
                fk.CraftGetName = "Potion";
                fk.CraftItem = DataBase.ItemBase.AlchemistPotion;
                fk.Location = MechEngine.AbilityLocation.WorldMap;
                fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Craft, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.DmgHealInstant } } };
                return fk;
            }
            /// <summary>
            /// Approve all alchemist abilityes
            /// </summary>
            /// <returns></returns>
            private static MechEngine.Ability Alchemism()
            {
                MechEngine.Ability fk = new MechEngine.Ability();
                fk.Class = MechEngine.BattleClass.Alchemist;
                fk.Level = 0;
                fk.LVRate = 0;
                fk.Mode = MechEngine.AbilityType.Passive;
                fk.Power = 0;
                fk.Rate = MechEngine.AbilityRate.AbilityPower;
                fk.ADRate = 1;
                fk.CostRate = 0;
                fk.Elem = MechEngine.AbilityElement.ElementalMagic;
                fk.Name = "Алхимия";
                fk.Info = "Тайная алхимия.\n\nУлучшает все навыки алхимика:\nРадужные брызги: +$ брызги\nБутыль стихий: +$ элемент\nСварить зелье: +$ хар-ка.\nАлхимия: При использовании любой способности\nесть шанс получить бонус от элемента стихии.\nОгонь - +$ урон  Вода - +$ лечение\nВоздух - +$ Элемент  Земля - +$ здоровье";
                fk.Icon = '╩';
                fk.Color = ConsoleColor.Red;
                fk.Duration = 0;
                fk.Location = MechEngine.AbilityLocation.Alltime;
                fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Improve, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.DmgHealInstant } } };
                return fk;
            }
            /// <summary>
            /// Random effect from this ability
            /// </summary>
            /// <returns></returns>
            public static MechEngine.Ability Elementalism
            {
                get
                {
                    MechEngine.Ability fk = new MechEngine.Ability();
                    fk.Class = MechEngine.BattleClass.Alchemist;
                    fk.Level = 0;
                    fk.LVRate = 0.25;
                    fk.Mode = MechEngine.AbilityType.Active;
                    fk.Power = 1;
                    fk.Rate = MechEngine.AbilityRate.AbilityPower;
                    fk.APRate = 1.25;
                    fk.CostRate = 2;
                    fk.Elem = MechEngine.AbilityElement.ElementalMagic;
                    fk.Name = "Элементализм";
                    fk.Info = "Один из эффектов алхимии.";
                    fk.Icon = '♀';
                    fk.Color = ConsoleColor.DarkBlue;
                    fk.Duration = 10;
                    fk.Stats = new List<MechEngine.AbilityStats>();
                    fk.Location = MechEngine.AbilityLocation.Alltime;
                    fk.Action = fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Improve, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.EffectOfTime } } };
                    return fk;
                }
            }
        }

        public static class EliteAbilityBase
        {
            /// <summary>
            /// Warlock abilityes
            /// </summary>
            /// <returns>List of warlock ability</returns>
            public static List<MechEngine.Ability> Warlock
            { get { return new List<MechEngine.Ability>() { Flush, Curse, BloodHappy, Sacrifice }; } }
            /// <summary>
            /// Warlock
            /// </summary>
            /// <returns>List of warlock elite ability</returns>
            public static List<MechEngine.Ability> WarlockElite
            { get { return new List<MechEngine.Ability>() { BigFlush, BigCurse, BigBloodHappy, Ultimate }; } }

            public static MechEngine.Ability Flush
            {
                get
                {
                    MechEngine.Ability fk = new MechEngine.Ability();
                    fk.Class = MechEngine.BattleClass.Warlock;
                    fk.Level = 0;
                    fk.LVRate = 0.21;
                    fk.Mode = MechEngine.AbilityType.Active;
                    fk.Power = 1;
                    fk.Rate = MechEngine.AbilityRate.AbilityPower;
                    fk.APRate = 1.51;
                    fk.CostRate = 10;
                    fk.Elem = MechEngine.AbilityElement.FireMagic;
                    fk.Info = "Огонь.\n\nКолдунство.\n\nОбычная форма.\n\nНаносит & урона.";
                    fk.Name = "Огненный пшик";
                    fk.Icon = '!';
                    fk.Color = ConsoleColor.Red;
                    fk.Duration = 0;
                    fk.Location = MechEngine.AbilityLocation.Combat;
                    fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Damage, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.DmgHealInstant } } };
                    return fk;
                }
            }

            public static MechEngine.Ability Curse
            {
                get
                {
                    MechEngine.Ability fk = new MechEngine.Ability();
                    fk.Class = MechEngine.BattleClass.Warlock;
                    fk.Level = 0;
                    fk.LVRate = 0.10;
                    fk.Mode = MechEngine.AbilityType.Active;
                    fk.Power = 1;
                    fk.Rate = MechEngine.AbilityRate.AbilityPower;
                    fk.APRate = 1.21;
                    fk.CostRate = 20;
                    fk.Elem = MechEngine.AbilityElement.FireMagic;
                    fk.Info = "Смерть.\n\nКолдунство.\n\nОбычная форма.\n\nПроклинает врага и наносит\n & урона каждый удар.";
                    fk.Name = "Проклятье";
                    fk.Icon = '│';
                    fk.Color = ConsoleColor.Green;
                    fk.Duration = 0;
                    fk.Location = MechEngine.AbilityLocation.Combat;
                    fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Damage, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.DmgHealOnTime } } };
                    return fk;
                }
            }

            public static MechEngine.Ability BloodHappy
            {
                get
                {
                    MechEngine.Ability fk = new MechEngine.Ability();
                    fk.Level = 0;
                    fk.LVRate = 0.12;
                    fk.Mode = MechEngine.AbilityType.Active;
                    fk.Power = 1;
                    fk.Class = MechEngine.BattleClass.Warlock;
                    fk.Info = "Кровь.\n\nКолдунство.\n\nОбычная форма.\n\nВытягивает жизнь:\n Наносит & урона\nи\nпередаёт ^*2 жизни варлоку.";
                    fk.Rate = MechEngine.AbilityRate.AbilityPower;
                    fk.APRate = 1.24;
                    fk.CostRate = 15;
                    fk.Elem = MechEngine.AbilityElement.BloodMagic;
                    fk.Duration = 0;
                    fk.Name = "Вытягивание";
                    fk.Icon = '┼';
                    fk.Color = ConsoleColor.DarkRed;
                    fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Damage, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.DmgHealInstant } },
                    new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Heal, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.DmgHealInstant } }};
                    fk.Location = MechEngine.AbilityLocation.Combat;
                    return fk;
                }
            }

            private static MechEngine.Ability Sacrifice
            {
                get
                {
                    MechEngine.Ability fk = new MechEngine.Ability();
                    fk.Level = 0;
                    fk.LVRate = 5;
                    fk.Mode = MechEngine.AbilityType.Active;
                    fk.Power = 1;
                    fk.Class = MechEngine.BattleClass.Warlock;
                    fk.Rate = MechEngine.AbilityRate.AbilityPower;
                    fk.APRate = 1;
                    fk.CostRate = 0;
                    fk.Elem = MechEngine.AbilityElement.BloodMagic;
                    fk.Name = "Жертва";
                    fk.Info = "Смерть.\n\nКолдунство\n\nВы изменяете свою форму.\nЭта форма изменяет все ваши навыки.\nКаждое убийство продлевает действие формы\nна 10 раундов.";
                    fk.Icon = '☼';
                    fk.Form = new MechEngine.Morphling('@', ConsoleColor.Magenta);
                    fk.Color = ConsoleColor.Magenta;
                    fk.Duration = 30;
                    fk.Stats = new List<MechEngine.AbilityStats> { MechEngine.AbilityStats.AD, MechEngine.AbilityStats.DMG, MechEngine.AbilityStats.MHP };
                    fk.Location = MechEngine.AbilityLocation.Alltime;
                    fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Improve, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.EffectOfTime } } };
                    return fk;
                }
            }

            public static MechEngine.Ability BigFlush
            {
                get
                {
                    MechEngine.Ability fk = new MechEngine.Ability();
                    fk.Class = MechEngine.BattleClass.Warlock;
                    fk.Level = 0;
                    fk.LVRate = 0.21;
                    fk.Mode = MechEngine.AbilityType.Active;
                    fk.Power = 1;
                    fk.Rate = MechEngine.AbilityRate.AbilityPower;
                    fk.APRate = 2.51;
                    fk.CostRate = 10;
                    fk.Elem = MechEngine.AbilityElement.FireMagic;
                    fk.Info = "Пламя.\n\nКолдунство.\n\nФорма смерти.\n\nНаносит & урона.";
                    fk.Name = "Ужасное пламя";
                    fk.Icon = '‼';
                    fk.Color = ConsoleColor.Red;
                    fk.Duration = 0;
                    fk.Location = MechEngine.AbilityLocation.Combat;
                    fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Damage, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.DmgHealInstant } } };
                    return fk;
                }
            }

            public static MechEngine.Ability BigCurse
            {
                get
                {
                    MechEngine.Ability fk = new MechEngine.Ability();
                    fk.Class = MechEngine.BattleClass.Warlock;
                    fk.Level = 0;
                    fk.LVRate = 0.10;
                    fk.Mode = MechEngine.AbilityType.Active;
                    fk.Power = 1;
                    fk.Rate = MechEngine.AbilityRate.AbilityPower;
                    fk.APRate = 2.21;
                    fk.CostRate = 30;
                    fk.Elem = MechEngine.AbilityElement.FireMagic;
                    fk.Info = "Смерть.\n\nКолдунство.\n\nФорма смерти.\n\nНаводит на врага страшную чуму,\nнаносит & урона каждый удар.";
                    fk.Name = "Чума";
                    fk.Icon = '║';
                    fk.Color = ConsoleColor.Green;
                    fk.Duration = 0;
                    fk.Location = MechEngine.AbilityLocation.Combat;
                    fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Damage, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.DmgHealOnTime } } };
                    return fk;
                }
            }

            public static MechEngine.Ability BigBloodHappy
            {
                get
                {
                    MechEngine.Ability fk = new MechEngine.Ability();
                    fk.Level = 0;
                    fk.LVRate = 0.12;
                    fk.Mode = MechEngine.AbilityType.Active;
                    fk.Power = 1;
                    fk.Class = MechEngine.BattleClass.Warlock;
                    fk.Info = "Кровище.\n\nКолдунство.\n\nФорма смерти.\n\nВытягивает жизнь:\n Наносит & урона\nи\nпередаёт ^*2 жизни варлоку.";
                    fk.Rate = MechEngine.AbilityRate.AbilityPower;
                    fk.APRate = 2.24;
                    fk.CostRate = 25;
                    fk.Elem = MechEngine.AbilityElement.BloodMagic;
                    fk.Duration = 0;
                    fk.Name = "Вытягивание";
                    fk.Icon = '╪';
                    fk.Color = ConsoleColor.DarkRed;
                    fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Damage, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.DmgHealInstant } },
                    new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Heal, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.DmgHealInstant } }};
                    fk.Location = MechEngine.AbilityLocation.Combat;
                    return fk;
                }
            }

            public static MechEngine.Ability Ultimate
            {
                get
                {
                    MechEngine.Ability fk = new MechEngine.Ability();
                    fk.Class = MechEngine.BattleClass.Warlock;
                    fk.Level = 0;
                    fk.LVRate = 0.21;
                    fk.Mode = MechEngine.AbilityType.Active;
                    fk.Power = 1;
                    fk.Rate = MechEngine.AbilityRate.AbilityPower;
                    fk.APRate = 5.51;
                    fk.CostRate = 95;
                    fk.Elem = MechEngine.AbilityElement.FireMagic;
                    fk.Info = "Сопряжение.\n\nКолдунство.\n\nФорма смерти.\n\nУбивает врага со спецэффектами.";
                    fk.Name = "Последний";
                    fk.Icon = '♀';
                    fk.Color = ConsoleColor.Yellow;
                    fk.Duration = 0;
                    fk.Location = MechEngine.AbilityLocation.Combat;
                    fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Damage, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.DmgHealInstant } } };
                    return fk;
                }
            }
            /// <summary>
            /// Valkyrie
            /// </summary>
            /// <returns>List of Valkyrie ability</returns>
            public static List<MechEngine.Ability> Valkyrie
            { get { return new List<MechEngine.Ability>() { FrostOrb,WaterOrb,IceOrb,ValkyrieStrike }; } }

            public static MechEngine.Ability FrostOrb
            {
                get
                {
                    MechEngine.Ability fk = new MechEngine.Ability();
                    fk.Class = MechEngine.BattleClass.Valkyrie;
                    fk.Level = 0;
                    fk.LVRate = 0;
                    fk.Mode = MechEngine.AbilityType.Active;
                    fk.Power = 0;
                    fk.Rate = MechEngine.AbilityRate.AbilityPower;
                    fk.APRate = 1;
                    fk.CostRate = 5;
                    fk.Elem = MechEngine.AbilityElement.NatureMagic;
                    fk.Info = "Сфера.\n\nТайное знание.\n\nСфера инея изменяет Удар Валькирии\nи с каждым ударом уменьшает врагу\nурон на @.";
                    fk.Name = "Сфера инея";
                    fk.Icon = '☼';
                    fk.Color = ConsoleColor.Cyan;
                    fk.Duration = 60;
                    fk.Location = MechEngine.AbilityLocation.Alltime;
                    fk.Stats = new List<MechEngine.AbilityStats>();
                    fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Improve, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.EffectOfTime } } };
                    return fk;
                }
            }

            public static MechEngine.Ability WaterOrb
            {
                get
                {
                    MechEngine.Ability fk = new MechEngine.Ability();
                    fk.Class = MechEngine.BattleClass.Valkyrie;
                    fk.Level = 0;
                    fk.LVRate = 0;
                    fk.Mode = MechEngine.AbilityType.Active;
                    fk.Power = 0;
                    fk.Rate = MechEngine.AbilityRate.AbilityPower;
                    fk.APRate = 1;
                    fk.CostRate = 5;
                    fk.Elem = MechEngine.AbilityElement.NatureMagic;
                    fk.Info = "Сфера.\n\nТайное знание.\n\nСфера воды изменяет Удар Валькирии\nи с каждым ударом лечит её\nна @.";
                    fk.Name = "Сфера воды";
                    fk.Icon = '☼';
                    fk.Color = ConsoleColor.Blue;
                    fk.Duration = 60;
                    fk.Location = MechEngine.AbilityLocation.Alltime;
                    fk.Stats = new List<MechEngine.AbilityStats>();
                    fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Improve, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.EffectOfTime } } };
                    return fk;
                }
            }

            public static MechEngine.Ability IceOrb
            {
                get
                {
                    MechEngine.Ability fk = new MechEngine.Ability();
                    fk.Class = MechEngine.BattleClass.Valkyrie;
                    fk.Level = 0;
                    fk.LVRate = 0;
                    fk.Mode = MechEngine.AbilityType.Active;
                    fk.Power = 0;
                    fk.Rate = MechEngine.AbilityRate.AbilityPower;
                    fk.APRate = 1;
                    fk.CostRate = 5;
                    fk.Elem = MechEngine.AbilityElement.NatureMagic;
                    fk.Info = "Сфера.\n\nТайное знание.\n\nСфера льда изменяет Удар Валькирии\nи с каждым ударом наносит\nдополнительные @ урона.";
                    fk.Name = "Сфера льда";
                    fk.Icon = '☼';
                    fk.Color = ConsoleColor.White;
                    fk.Duration = 60;
                    fk.Location = MechEngine.AbilityLocation.Alltime;
                    fk.Stats = new List<MechEngine.AbilityStats>();
                    fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Improve, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.EffectOfTime } } };
                    return fk;
                }
            }

            public static MechEngine.Ability ValkyrieStrike
            {
                get
                {
                    MechEngine.Ability fk = new MechEngine.Ability();
                    fk.Class = MechEngine.BattleClass.Warlock;
                    fk.Level = 0;
                    fk.LVRate = 0.21;
                    fk.Mode = MechEngine.AbilityType.Active;
                    fk.Power = 1;
                    fk.Rate = MechEngine.AbilityRate.AbilityPower;
                    fk.APRate = 1.51;
                    fk.CostRate = 10;
                    fk.Elem = MechEngine.AbilityElement.FireMagic;
                    fk.Info = "Удар.\n\nТайное знание.\n\nНаносит & урона.\nЕсли у Валькирии есть сфера\nдополнительно срабатывает эффект сферы.";
                    fk.Name = "Удар Валькирии";
                    fk.Icon = '√';
                    fk.Color = ConsoleColor.Magenta;
                    fk.Duration = 0;
                    fk.Location = MechEngine.AbilityLocation.Combat;
                    fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Damage, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.DmgHealInstant } } };
                    return fk;
                }
            }
            /// <summary>
            /// LightWarrior
            /// </summary>
            /// <returns>List of LigthWarrior ability</returns>
            public static List<MechEngine.Ability> LightWarrior
            { get { return new List<MechEngine.Ability>() { LightMark, MagicBlast, PhysicBlast, Speed }; } }
            /// <summary>
            /// Main skill
            /// </summary>
            /// <returns></returns>
            public static MechEngine.Ability LightMark
            {
                get
                {
                    MechEngine.Ability fk = new MechEngine.Ability();
                    fk.Class = MechEngine.BattleClass.LightWarrior;
                    fk.Level = 0;
                    fk.LVRate = 0.78;
                    fk.Mode = MechEngine.AbilityType.Active;
                    fk.Power = 1;
                    fk.Rate = MechEngine.AbilityRate.AttackDamage;
                    fk.ADRate = 1.24;
                    fk.CostRate = 5;
                    fk.Elem = MechEngine.AbilityElement.NatureMagic;
                    fk.Name = "Метка";
                    fk.Info = "Мастерство убийцы.\n\nМолния.\n\nСтавит метку на врага.\nКаждый удар по метке наносит\n ^ урона.";
                    fk.Icon = '¤';
                    fk.Color = ConsoleColor.Cyan;
                    fk.Duration = 180;
                    fk.Location = MechEngine.AbilityLocation.Combat;
                    fk.Stats = new List<MechEngine.AbilityStats> { };
                    fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Debuff, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.EffectOfTime } } };
                    return fk;
                }
            }
            /// <summary>
            /// Explore mark AP
            /// </summary>
            /// <returns></returns>
            public static MechEngine.Ability MagicBlast
            {
                get
                {
                    MechEngine.Ability fk = new MechEngine.Ability();
                    fk.Class = MechEngine.BattleClass.LightWarrior;
                    fk.Level = 0;
                    fk.LVRate = 0.82;
                    fk.Mode = MechEngine.AbilityType.Active;
                    fk.Power = 1;
                    fk.Rate = MechEngine.AbilityRate.AttackDamage;
                    fk.ADRate = 1.5;
                    fk.CostRate = 10;
                    fk.Elem = MechEngine.AbilityElement.HolyMagic;
                    fk.Name = "Взрыв магии";
                    fk.Info = "Тайны молнии.\n\nБоевое исскуство.\nЕсли у врага есть метка, взрывает её.\nНаносит & магического урона.";
                    fk.Icon = '¤';
                    fk.Color = ConsoleColor.DarkMagenta;
                    fk.Duration = 0;
                    fk.Location = MechEngine.AbilityLocation.Combat;
                    fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Damage, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.DmgHealInstant } } };
                    return fk;
                }
            }
            /// <summary>
            /// Explore mark AD
            /// </summary>
            /// <returns></returns>
            public static MechEngine.Ability PhysicBlast
            {
                get
                {
                    MechEngine.Ability fk = new MechEngine.Ability();
                    fk.Class = MechEngine.BattleClass.LightWarrior;
                    fk.Level = 0;
                    fk.LVRate = 0.82;
                    fk.Mode = MechEngine.AbilityType.Active;
                    fk.Power = 1;
                    fk.Rate = MechEngine.AbilityRate.AttackDamage;
                    fk.ADRate = 1.1;
                    fk.CostRate = 10;
                    fk.Elem = MechEngine.AbilityElement.HolyMagic;
                    fk.Name = "Взрыв силы";
                    fk.Info = "Тайны молнии.\n\nБоевое исскуство.\nЕсли у врага есть метка, взрывает её.\nНаносит & физического урона.";
                    fk.Icon = '¤';
                    fk.Color = ConsoleColor.DarkCyan;
                    fk.Duration = 0;
                    fk.Location = MechEngine.AbilityLocation.Combat;
                    fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Damage, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.DmgHealInstant } } };
                    return fk;
                }
            }
            /// <summary>
            /// Light warrior special two attacks
            /// </summary>
            /// <returns></returns>
            public static MechEngine.Ability Speed
            {
                get
                {
                    MechEngine.Ability fk = new MechEngine.Ability();
                    fk.Class = MechEngine.BattleClass.LightWarrior;
                    fk.Level = 0;
                    fk.LVRate = 0.1;
                    fk.Mode = MechEngine.AbilityType.Passive;
                    fk.Power = 0;
                    fk.Rate = MechEngine.AbilityRate.AttackDamage;
                    fk.ADRate = 0.01;
                    fk.CostRate = 0;
                    fk.Elem = MechEngine.AbilityElement.ElementalMagic;
                    fk.Name = "Скорость";
                    fk.Info = "Болезнь молнии.\n\nКаждая еденица силы навыка увеличивает\nколичество атак.\nУвеличивает кол-во атак на ^.\n\n(Для двух атак сила должна быть равна 1)";
                    fk.Icon = '¤';
                    fk.Color = ConsoleColor.DarkGreen;
                    fk.Duration = 0;
                    fk.Location = MechEngine.AbilityLocation.Alltime;
                    fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Improve, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.DmgHealInstant } } };
                    return fk;
                }
            }
        }

        public static class OtherAbilityBase
        {
            public static List<MechEngine.Ability> BaseSet()
            { return new List<MechEngine.Ability>() { Key, MagicKey, DestroyWall, TownPortalCraft }; }
            /// <summary>
            /// Character find key
            /// </summary>
            /// <returns></returns>
            private static MechEngine.Ability Key
            {
                get
                {
                    MechEngine.Ability fk = new MechEngine.Ability();
                    fk.Level = 1;
                    fk.LVRate = 0.1;
                    fk.Mode = MechEngine.AbilityType.Active;
                    fk.Power = 1;
                    fk.Rate = MechEngine.AbilityRate.AbilityPower;
                    fk.APRate = 1.1;
                    fk.CostRate = 1;
                    fk.Name = "Жел. Ключ*";
                    fk.Info = "Ремесло.\n\nСоздание железного ключа.";
                    fk.Icon = '&';
                    fk.Color = ConsoleColor.White;
                    fk.Duration = 0;
                    fk.Location = MechEngine.AbilityLocation.WorldMap;
                    fk.CraftItem = ItemBase.IronKey;
                    fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Craft, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.DmgHealInstant } } };
                    return fk;
                }
            }
            /// <summary>
            /// Character find magic key
            /// </summary>
            /// <returns></returns>
            private static MechEngine.Ability MagicKey
            {
                get
                {
                    MechEngine.Ability fk = new MechEngine.Ability();
                    fk.Level = 1;
                    fk.LVRate = 0.1;
                    fk.Mode = MechEngine.AbilityType.Active;
                    fk.Power = 1;
                    fk.Rate = MechEngine.AbilityRate.AbilityPower;
                    fk.APRate = 1.1;
                    fk.CostRate = 1;
                    fk.Name = "Маг. Ключ*";
                    fk.Info = "Ремесло.\n\nСоздание магического ключа.";
                    fk.Icon = '&';
                    fk.Color = ConsoleColor.DarkMagenta;
                    fk.Duration = 0;
                    fk.Location = MechEngine.AbilityLocation.WorldMap;
                    fk.CraftItem = ItemBase.MagicKey;
                    fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Craft, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.DmgHealInstant } } };
                    return fk;
                }
            }

            private static MechEngine.Ability DestroyWall
            {
                get
                {
                    MechEngine.Ability fk = new MechEngine.Ability();
                    fk.Level = 0;
                    fk.LVRate = 0.01;
                    fk.Mode = MechEngine.AbilityType.Active;
                    fk.Power = 1;
                    fk.Rate = MechEngine.AbilityRate.AttackDamage;
                    fk.ADRate = 1.01;
                    fk.CostRate = 0;
                    fk.Elem = MechEngine.AbilityElement.Physical;
                    fk.Name = "Молитва богам";
                    fk.Info = "Молтива.\n\nОтчаяние.\n\nМолитва богам для благословения.";
                    fk.Icon = '░';
                    fk.Color = ConsoleColor.DarkGray;
                    fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Craft, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.DmgHealInstant } } };
                    return fk;
                }
            }

            private static MechEngine.Ability TownPortalCraft
            {
                get
                {
                    MechEngine.Ability fk = new MechEngine.Ability();
                    fk.Level = 1;
                    fk.LVRate = 0.1;
                    fk.Mode = MechEngine.AbilityType.Active;
                    fk.Power = 1;
                    fk.Rate = MechEngine.AbilityRate.AbilityPower;
                    fk.APRate = 1.1;
                    fk.CostRate = 1;
                    fk.Name = "Свит. Дома*";
                    fk.Info = "Ремесло.\n\nСоздание свитка телепортации.";
                    fk.Icon = '⌂';
                    fk.Color = ConsoleColor.Blue;
                    fk.Duration = 0;
                    fk.Location = MechEngine.AbilityLocation.WorldMap;
                    fk.CraftItem = ItemBase.ScrollOfMraumir;
                    fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Craft, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.DmgHealInstant } } };
                    return fk;
                }
            }

            public static MechEngine.Ability Human
            {
                get
                {
                    MechEngine.Ability fk = new MechEngine.Ability();
                    fk.Level = 0;
                    fk.LVRate = 0.17;
                    fk.Mode = MechEngine.AbilityType.Active;
                    fk.Power = 1;
                    fk.Rate = MechEngine.AbilityRate.AttackDamage;
                    fk.ADRate = 1.12;
                    fk.CostRate = 2;
                    fk.Elem = MechEngine.AbilityElement.NatureMagic;
                    fk.Name = "Выживание";
                    fk.Info = "Расовая способность.\n\nЛюди.\n\nУвеличивает Здоровье на #.";
                    fk.Icon = '♥';
                    fk.Color = ConsoleColor.Red;
                    fk.Duration = 30;
                    fk.Stats = new List<MechEngine.AbilityStats> { MechEngine.AbilityStats.MHP };
                    fk.Location = MechEngine.AbilityLocation.WorldMap;
                    fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Improve, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.EffectOfTime } } };
                    return fk;
                }
            }

            public static MechEngine.Ability Elf
            {
                get
                {
                    MechEngine.Ability fk = new MechEngine.Ability();
                    fk.Level = 0;
                    fk.LVRate = 0.17;
                    fk.Mode = MechEngine.AbilityType.Active;
                    fk.Power = 1;
                    fk.Rate = MechEngine.AbilityRate.AbilityPower;
                    fk.APRate = 1.12;
                    fk.CostRate = 2;
                    fk.Elem = MechEngine.AbilityElement.NatureMagic;
                    fk.Name = "Магическая нить";
                    fk.Info = "Расовая способность.\n\nЭльфы.\n\nУвеличивает Ресурсы на #.";
                    fk.Icon = '─';
                    fk.Color = ConsoleColor.Blue;
                    fk.Duration = 30;
                    fk.Stats = new List<MechEngine.AbilityStats> { MechEngine.AbilityStats.MMP };
                    fk.Location = MechEngine.AbilityLocation.WorldMap;
                    fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Improve, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.EffectOfTime } } };
                    return fk;
                }
            }

            public static MechEngine.Ability DarkElf
            {
                get
                {
                    MechEngine.Ability fk = new MechEngine.Ability();
                    fk.Level = 0;
                    fk.LVRate = 0.17;
                    fk.Mode = MechEngine.AbilityType.Active;
                    fk.Power = 1;
                    fk.Rate = MechEngine.AbilityRate.AbilityPower;
                    fk.APRate = 1.12;
                    fk.CostRate = 2;
                    fk.Elem = MechEngine.AbilityElement.NatureMagic;
                    fk.Name = "Магическая нить";
                    fk.Info = "Расовая способность.\n\nЭльфы.\n\nУвеличивает Силу магии на #.";
                    fk.Icon = '─';
                    fk.Color = ConsoleColor.Magenta;
                    fk.Duration = 30;
                    fk.Stats = new List<MechEngine.AbilityStats> { MechEngine.AbilityStats.AP };
                    fk.Location = MechEngine.AbilityLocation.WorldMap;
                    fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Improve, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.EffectOfTime } } };
                    return fk;
                }
            }

            public static MechEngine.Ability Dwarf
            {
                get
                {
                    MechEngine.Ability fk = new MechEngine.Ability();
                    fk.Level = 0;
                    fk.LVRate = 0.17;
                    fk.Mode = MechEngine.AbilityType.Active;
                    fk.Power = 1;
                    fk.Rate = MechEngine.AbilityRate.AttackDamage;
                    fk.ADRate = 1.12;
                    fk.CostRate = 2;
                    fk.Elem = MechEngine.AbilityElement.NatureMagic;
                    fk.Name = "Стальная кожа";
                    fk.Info = "Расовая способность.\n\nДварфы.\n\nУвеличивает Физ. Защиту на #.";
                    fk.Icon = '♦';
                    fk.Color = ConsoleColor.DarkCyan;
                    fk.Duration = 30;
                    fk.Stats = new List<MechEngine.AbilityStats> { MechEngine.AbilityStats.ARM };
                    fk.Location = MechEngine.AbilityLocation.WorldMap;
                    fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Improve, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.EffectOfTime } } };
                    return fk;
                }
            }

            public static MechEngine.Ability Gnome
            {
                get
                {
                    MechEngine.Ability fk = new MechEngine.Ability();
                    fk.Level = 0;
                    fk.LVRate = 0.17;
                    fk.Mode = MechEngine.AbilityType.Active;
                    fk.Power = 1;
                    fk.Rate = MechEngine.AbilityRate.AttackDamage;
                    fk.ADRate = 1.12;
                    fk.CostRate = 2;
                    fk.Elem = MechEngine.AbilityElement.NatureMagic;
                    fk.Name = "Скорость";
                    fk.Info = "Расовая способность.\n\nГномы.\n\nУвеличивает Урон на #.";
                    fk.Icon = '╪';
                    fk.Color = ConsoleColor.DarkGray;
                    fk.Duration = 30;
                    fk.Stats = new List<MechEngine.AbilityStats> { MechEngine.AbilityStats.DMG };
                    fk.Location = MechEngine.AbilityLocation.WorldMap;
                    fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Improve, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.EffectOfTime } } };
                    return fk;
                }
            }

            public static MechEngine.Ability MoonElf
            {
                get
                {
                    MechEngine.Ability fk = new MechEngine.Ability();
                    fk.Level = 0;
                    fk.LVRate = 0.17;
                    fk.Mode = MechEngine.AbilityType.Active;
                    fk.Power = 1;
                    fk.Rate = MechEngine.AbilityRate.AbilityPower;
                    fk.APRate = 1.12;
                    fk.CostRate = 2;
                    fk.Elem = MechEngine.AbilityElement.NatureMagic;
                    fk.Name = "Сила луны";
                    fk.Info = "Расовая способность.\n\nЭльфы.\n\nУвеличивает Силу магии и Ресурсы\nна #.";
                    fk.Icon = '╩';
                    fk.Color = ConsoleColor.DarkBlue;
                    fk.Duration = 30;
                    fk.Stats = new List<MechEngine.AbilityStats> { MechEngine.AbilityStats.AP, MechEngine.AbilityStats.MMP };
                    fk.Location = MechEngine.AbilityLocation.WorldMap;
                    fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Improve, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.EffectOfTime } } };
                    return fk;
                }
            }

            public static MechEngine.Ability Orc
            {
                get
                {
                    MechEngine.Ability fk = new MechEngine.Ability();
                    fk.Level = 0;
                    fk.LVRate = 0.17;
                    fk.Mode = MechEngine.AbilityType.Active;
                    fk.Power = 1;
                    fk.Rate = MechEngine.AbilityRate.AttackDamage;
                    fk.ADRate = 1.12;
                    fk.CostRate = 2;
                    fk.Elem = MechEngine.AbilityElement.NatureMagic;
                    fk.Name = "Жажда жизни";
                    fk.Info = "Расовая способность.\n\nОрки.\n\nУвеличивает Здоровье на #.";
                    fk.Icon = '♥';
                    fk.Color = ConsoleColor.DarkRed;
                    fk.Duration = 30;
                    fk.Stats = new List<MechEngine.AbilityStats> { MechEngine.AbilityStats.MHP };
                    fk.Location = MechEngine.AbilityLocation.WorldMap;
                    fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Improve, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.EffectOfTime } } };
                    return fk;
                }
            }

            public static MechEngine.Ability Troll
            {
                get
                {
                    MechEngine.Ability fk = new MechEngine.Ability();
                    fk.Level = 0;
                    fk.LVRate = 0.17;
                    fk.Mode = MechEngine.AbilityType.Active;
                    fk.Power = 1;
                    fk.Rate = MechEngine.AbilityRate.AttackDamage;
                    fk.ADRate = 1.12;
                    fk.CostRate = 2;
                    fk.Elem = MechEngine.AbilityElement.NatureMagic;
                    fk.Name = "Силки";
                    fk.Info = "Расовая способность.\n\nТролли.\n\nУвеличивает Силу атаки на #.";
                    fk.Icon = '╘';
                    fk.Color = ConsoleColor.DarkRed;
                    fk.Duration = 30;
                    fk.Stats = new List<MechEngine.AbilityStats> { MechEngine.AbilityStats.AD };
                    fk.Location = MechEngine.AbilityLocation.WorldMap;
                    fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Improve, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.EffectOfTime } } };
                    return fk;
                }
            }

            public static MechEngine.Ability Undead
            {
                get
                {
                    MechEngine.Ability fk = new MechEngine.Ability();
                    fk.Level = 0;
                    fk.LVRate = 0.17;
                    fk.Mode = MechEngine.AbilityType.Active;
                    fk.Power = 1;
                    fk.Rate = MechEngine.AbilityRate.AttackDamage;
                    fk.ADRate = 1.12;
                    fk.CostRate = 2;
                    fk.Elem = MechEngine.AbilityElement.NatureMagic;
                    fk.Name = "Преданность";
                    fk.Info = "Расовая способность.\n\nНежить.\n\nУвеличивает Маг. Защиту\n на #.";
                    fk.Icon = '╘';
                    fk.Color = ConsoleColor.DarkMagenta;
                    fk.Duration = 30;
                    fk.Stats = new List<MechEngine.AbilityStats> { MechEngine.AbilityStats.MRS };
                    fk.Location = MechEngine.AbilityLocation.WorldMap;
                    fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Improve, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.EffectOfTime } } };
                    return fk;
                }
            }

            public static MechEngine.Ability FallenAngel
            {
                get
                {
                    MechEngine.Ability fk = new MechEngine.Ability();
                    fk.Level = 0;
                    fk.LVRate = 0.17;
                    fk.Mode = MechEngine.AbilityType.Active;
                    fk.Power = 1;
                    fk.Rate = MechEngine.AbilityRate.AbilityPower;
                    fk.APRate = 1.12;
                    fk.CostRate = 2;
                    fk.Elem = MechEngine.AbilityElement.NatureMagic;
                    fk.Name = "Одиночество";
                    fk.Info = "Расовая способность.\n\nПадший.\n\nУвеличивает Физ. Защиту\n на #.";
                    fk.Icon = '♂';
                    fk.Color = ConsoleColor.Yellow;
                    fk.Duration = 30;
                    fk.Stats = new List<MechEngine.AbilityStats> { MechEngine.AbilityStats.ARM };
                    fk.Location = MechEngine.AbilityLocation.WorldMap;
                    fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Improve, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.EffectOfTime } } };
                    return fk;
                }
            }
            /// <summary>
            /// Summon cat for help :D
            /// </summary>
            public static MechEngine.Ability CatJoin
            {
                get
                {
                    MechEngine.Ability fk = new MechEngine.Ability();
                    fk.Class = MechEngine.BattleClass.Illusionist;
                    fk.Level = 0;
                    fk.LVRate = 0.25;
                    fk.Mode = MechEngine.AbilityType.Active;
                    fk.Power = 1;
                    fk.Rate = MechEngine.AbilityRate.AbilityPower;
                    fk.APRate = 1.25;
                    fk.CostRate = 0;
                    fk.Elem = MechEngine.AbilityElement.DemonMagic;
                    fk.Name = "Кот";
                    fk.Info = "Помогает вам.";
                    fk.Icon = 'C';
                    fk.Color = ConsoleColor.DarkYellow;
                    fk.Duration = 180;
                    fk.SummonMonster = SummonedBase.Cat(Rogue.RAM.Player.Level);
                    fk.SummonBlock = true;
                    fk.Location = MechEngine.AbilityLocation.WorldMap;
                    fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Summon, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.EffectOfTime } } };
                    for (int i = 0; i < Rogue.RAM.Player.Level; i++)
                    {
                        fk.UP();
                    }
                    return fk;
                }
            }
            /// <summary>
            /// Mercenary
            /// </summary>
            public static MechEngine.Ability MercenaryHeal
            {
                get
                {
                    MechEngine.Ability fk = new MechEngine.Ability();
                    fk.Class = MechEngine.BattleClass.Illusionist;
                    fk.Level = 0;
                    fk.LVRate = 0.25;
                    fk.Mode = MechEngine.AbilityType.Active;
                    fk.Power = 1;
                    fk.Rate = MechEngine.AbilityRate.AbilityPower;
                    fk.APRate = 1.25;
                    fk.CostRate = 0;
                    fk.Elem = MechEngine.AbilityElement.DemonMagic;
                    fk.Name = "Наёмник";
                    fk.Info = "Лечит.";
                    fk.Icon = 'H';
                    fk.Color = ConsoleColor.Red;
                    fk.Duration = 360;
                    fk.SummonMonster = SummonedBase.MHeal(Rogue.RAM.Map.Level);
                    fk.SummonBlock = true;
                    fk.Location = MechEngine.AbilityLocation.WorldMap;
                    fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Summon, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.EffectOfTime } } };
                    for (int i = 0; i < Rogue.RAM.Player.Level; i++)
                    {
                        fk.UP();
                    }
                    return fk;
                }
            }
            /// <summary>
            /// Mercenary
            /// </summary>
            public static MechEngine.Ability MercenaryWarrior
            {
                get
                {
                    MechEngine.Ability fk = new MechEngine.Ability();
                    fk.Class = MechEngine.BattleClass.Illusionist;
                    fk.Level = 0;
                    fk.LVRate = 0.25;
                    fk.Mode = MechEngine.AbilityType.Active;
                    fk.Power = 1;
                    fk.Rate = MechEngine.AbilityRate.AbilityPower;
                    fk.APRate = 1.25;
                    fk.CostRate = 0;
                    fk.Elem = MechEngine.AbilityElement.DemonMagic;
                    fk.Name = "Наёмник";
                    fk.Info = "Атакует.";
                    fk.Icon = 'W';
                    fk.Color = ConsoleColor.Blue;
                    fk.Duration = 360;
                    fk.SummonMonster = SummonedBase.MWarrior(Rogue.RAM.Map.Level);
                    fk.SummonBlock = true;
                    fk.Location = MechEngine.AbilityLocation.WorldMap;
                    fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Summon, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.EffectOfTime } } };
                    for (int i = 0; i < Rogue.RAM.Player.Level; i++)
                    {
                        fk.UP();
                    }
                    return fk;
                }
            }
            /// <summary>
            /// Mercenary
            /// </summary>
            public static MechEngine.Ability MercenaryMage
            {
                get
                {
                    MechEngine.Ability fk = new MechEngine.Ability();
                    fk.Class = MechEngine.BattleClass.Illusionist;
                    fk.Level = 0;
                    fk.LVRate = 0.25;
                    fk.Mode = MechEngine.AbilityType.Active;
                    fk.Power = 1;
                    fk.Rate = MechEngine.AbilityRate.AbilityPower;
                    fk.APRate = 1.25;
                    fk.CostRate = 0;
                    fk.Elem = MechEngine.AbilityElement.DemonMagic;
                    fk.Name = "Наёмник";
                    fk.Info = "Кастует.";
                    fk.Icon = 'M';
                    fk.Color = ConsoleColor.Magenta;
                    fk.Duration = 360;
                    fk.SummonMonster = SummonedBase.MMage(Rogue.RAM.Map.Level);
                    fk.SummonBlock = true;
                    fk.Location = MechEngine.AbilityLocation.WorldMap;
                    fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Summon, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.EffectOfTime } } };
                    for (int i = 0; i < Rogue.RAM.Player.Level; i++)
                    {
                        fk.UP();
                    }
                    return fk;
                }
            }
            /// <summary>
            /// Create armor
            /// </summary>
            /// <returns></returns>
            public static MechEngine.Ability TailorAmor
            {
                get
                {
                    MechEngine.Ability fk = new MechEngine.Ability();
                    fk.Level = 1;
                    fk.LVRate = 0.1;
                    fk.Mode = MechEngine.AbilityType.Active;
                    fk.Power = 1;
                    fk.Rate = MechEngine.AbilityRate.AbilityPower;
                    fk.APRate = 1.1;
                    fk.CostRate = 1;
                    fk.Name = "Мантия*";
                    fk.Info = "Ремесло.\n\nСоздание особенной мантии.";
                    fk.Icon = '♦';
                    fk.CraftGetName = "Tailor";
                    fk.Color = ConsoleColor.Magenta;
                    fk.Duration = 0;
                    fk.Location = MechEngine.AbilityLocation.WorldMap;
                    fk.CraftItem = ItemBase.CraftItemsFromAbility.TailorMantle;
                    fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Craft, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.DmgHealInstant } } };
                    return fk;
                }
            }
            /// <summary>
            /// Gregory prayer of dead
            /// </summary>
            /// <returns></returns>
            public static MechEngine.Ability Gregory
            {
                get
                {
                    MechEngine.Ability fk = new MechEngine.Ability();
                    fk.Class = MechEngine.BattleClass.Illusionist;
                    fk.Level = 1;
                    fk.LVRate = 0;
                    fk.Mode = MechEngine.AbilityType.Active;
                    fk.Power = 1000;
                    fk.Rate = MechEngine.AbilityRate.AttackDamage;
                    fk.ADRate = 1;
                    fk.CostRate = 0;
                    fk.Elem = MechEngine.AbilityElement.HolyMagic;
                    fk.Name = "Забвение Грегори";
                    fk.Info = "Баланс.\n\nМонах молится об избавлении от смерти.\nНа короткий промежуток монах будет\nневосприимчив к смерти.";
                    fk.Icon = 'G';
                    fk.Color = ConsoleColor.DarkGray;
                    fk.Duration = 500;
                    //fk.Stats = new List<MechEngine.AbilityStats> { MechEngine.AbilityStats.MHP };
                    fk.Location = MechEngine.AbilityLocation.Alltime;
                    fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Improve, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.EffectOfTime } } };
                    return fk;
                }
            }
            /// <summary>
            /// Dwarf's Runes
            /// </summary>
            public static MechEngine.Ability OldRune
            {
                get
                {
                    MechEngine.Ability fk = new MechEngine.Ability();
                    fk.Level = 0;
                    fk.LVRate = 0.17;
                    fk.Mode = MechEngine.AbilityType.Active;
                    fk.Power = Rogue.RAM.Player.Level;
                    fk.Rate = MechEngine.AbilityRate.AttackDamage;
                    fk.ADRate = 1.12;
                    fk.CostRate = 2;
                    fk.Elem = MechEngine.AbilityElement.NatureMagic;
                    fk.Name = "Старая руна";
                    fk.Info = "Древняя реликвия.";
                    fk.Icon = '⌂';
                    fk.Color = ConsoleColor.DarkGray;
                    fk.Duration = 500;
                    fk.Stats = new List<MechEngine.AbilityStats> { MechEngine.AbilityStats.ARM, MechEngine.AbilityStats.MRS };
                    fk.Location = MechEngine.AbilityLocation.WorldMap;
                    fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Improve, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.EffectOfTime } } };
                    return fk;
                }
            }
            /// <summary>
            /// Craft magic weapon
            /// </summary>
            /// <returns></returns>
            public static MechEngine.Ability EfirAirWeapon
            {
                get
                {
                    MechEngine.Ability fk = new MechEngine.Ability();
                    fk.Level = 1;
                    fk.LVRate = 0.1;
                    fk.Mode = MechEngine.AbilityType.Active;
                    fk.Power = 1;
                    fk.Rate = MechEngine.AbilityRate.AbilityPower;
                    fk.APRate = 1.1;
                    fk.CostRate = 1;
                    fk.Name = "Оружие*";
                    fk.Info = "Ремесло.\n\nСоздание оружия из эфира.";
                    fk.Icon = '{';
                    fk.Color = ConsoleColor.Cyan;
                    fk.Duration = 0;
                    fk.Location = MechEngine.AbilityLocation.WorldMap;
                    fk.CraftItem = ItemBase.CraftItemsFromAbility.EfirWeapon;
                    fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Craft, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.DmgHealInstant } } };
                    return fk;
                }
            }
            /// <summary>
            /// Craft magic boots
            /// </summary>
            /// <returns></returns>
            public static MechEngine.Ability EfirAirBoots
            {
                get
                {
                    MechEngine.Ability fk = new MechEngine.Ability();
                    fk.Level = 1;
                    fk.LVRate = 0.1;
                    fk.Mode = MechEngine.AbilityType.Active;
                    fk.Power = 1;
                    fk.Rate = MechEngine.AbilityRate.AbilityPower;
                    fk.APRate = 1.1;
                    fk.CostRate = 1;
                    fk.Name = "Обувь*";
                    fk.Info = "Ремесло.\n\nСоздание обуви из эфира.";
                    fk.Icon = '▼';
                    fk.Color = ConsoleColor.Cyan;
                    fk.Duration = 0;
                    fk.Location = MechEngine.AbilityLocation.WorldMap;
                    fk.CraftItem = ItemBase.CraftItemsFromAbility.EfirBoots;
                    fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Craft, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.DmgHealInstant } } };
                    return fk;
                }
            }
            /// <summary>
            /// Craft random magic scroll
            /// </summary>
            /// <returns></returns>
            public static MechEngine.Ability EfirAirScroll
            {
                get
                {
                    MechEngine.Ability fk = new MechEngine.Ability();
                    fk.Level = 1;
                    fk.LVRate = 0.1;
                    fk.Mode = MechEngine.AbilityType.Active;
                    fk.Power = 1;
                    fk.Rate = MechEngine.AbilityRate.AbilityPower;
                    fk.APRate = 1.1;
                    fk.CostRate = 1;
                    fk.Name = "Свиток*";
                    fk.Info = "Ремесло.\n\nСоздание свитка из эфира.";
                    fk.Icon = 'S';
                    fk.Color = ConsoleColor.Cyan;
                    fk.Duration = 0;
                    fk.Location = MechEngine.AbilityLocation.WorldMap;
                    fk.CraftItem = ItemBase.CraftItemsFromAbility.EfirScroll;
                    fk.Action = new List<MechEngine.AbilityAction>() { new MechEngine.AbilityAction() { Act = MechEngine.AbilityActionType.Craft, Atr = new List<MechEngine.AbilityActionAttribute>() { MechEngine.AbilityActionAttribute.DmgHealInstant } } };
                    return fk;
                }
            }
        }

        public static class BiomBase
        {
            public static MechEngine.Biom Random
            {
                get
                {                   
                    switch (r.Next(6))
                    {
                        case 0: { return Drow; }
                        case 1: { return Elves; }
                        case 2: { return Mount; }
                        case 3: { return Demons; }
                        case 4: { return Dwarfs; }
                        case 5: { return Undeads; }
                        default: { return new MechEngine.Biom(); }
                    }
                }
            }

            public static MechEngine.Biom Drow
            {
                get
                {
                    switch (r.Next(4))
                    {
                        case 0: { return new MechEngine.Biom() { Affix = "Святилище", Map = DrowMaps.ElvesHoly, Color = ConsoleColor.DarkMagenta, Name = "Подземелья дроу" }; }
                        case 1: { return new MechEngine.Biom() { Affix = "Лабиринт испытаний", Map = DrowMaps.Labirinth, Color = ConsoleColor.DarkMagenta, Name = "Подземелья дроу" }; }
                        case 2: { return new MechEngine.Biom() { Affix = "Лабиринт старейшин", Map = DrowMaps.UnderOlder, Color = ConsoleColor.DarkMagenta, Name = "Подземелья дроу" }; }
                        case 3: { return new MechEngine.Biom() { Affix = "Казармы", Map = DrowMaps.Barraks, Color = ConsoleColor.DarkMagenta, Name = "Подземелья дроу" }; }
                        default: { return new MechEngine.Biom(); }
                    }
                }
            }

            public static MechEngine.Biom Elves
            {
                get
                {
                    switch (r.Next(4))
                    {
                        case 0: { return new MechEngine.Biom() { Affix = "Круги силы", Map = ElvesMaps.PowerCircles, Color = ConsoleColor.DarkGreen, Name = "Древо мира" }; }
                        case 1: { return new MechEngine.Biom() { Affix = "Дорога друидов", Map = ElvesMaps.DruidsWay, Color = ConsoleColor.DarkGreen, Name = "Древо мира" }; }
                        case 2: { return new MechEngine.Biom() { Affix = "Мировое дерево", Map = ElvesMaps.WorldTree, Color = ConsoleColor.DarkGreen, Name = "Древо мира" }; }
                        case 3: { return new MechEngine.Biom() { Affix = "Корни дерева", Map = ElvesMaps.TreeCore, Color = ConsoleColor.DarkGreen, Name = "Древо мира" }; }
                        default: { return new MechEngine.Biom(); }
                    }
                }
            }

            public static MechEngine.Biom Mount
            {
                get
                {                    
                    switch (r.Next(4))
                    {
                        case 0: { return new MechEngine.Biom() { Affix = "Главный хребет", Map = MountMaps.MainMount, Color = ConsoleColor.DarkCyan, Name = "Инеистые горы" }; }
                        case 1: { return new MechEngine.Biom() { Affix = "Разлом", Map = MountMaps.Rift, Color = ConsoleColor.DarkCyan, Name = "Инеистые горы" }; }
                        case 2: { return new MechEngine.Biom() { Affix = "Пропасть", Map = MountMaps.Yama, Color = ConsoleColor.DarkCyan, Name = "Инеистые горы" }; }
                        case 3: { return new MechEngine.Biom() { Affix = "Северные дороги", Map = MountMaps.NorthWays, Color = ConsoleColor.DarkCyan, Name = "Инеистые горы" }; }
                        default: { return new MechEngine.Biom(); }
                    }
                }
            }

            public static MechEngine.Biom Demons
            {
                get
                {
                    switch (r.Next(4))
                    {
                        case 0: { return new MechEngine.Biom() { Affix = "Окаменевшие реки", Map = DemonMaps.FireRivers, Color = ConsoleColor.DarkYellow, Name = "Материк демонов" }; }
                        case 1: { return new MechEngine.Biom() { Affix = "Кратер возгорания", Map = DemonMaps.FirePlace, Color = ConsoleColor.DarkYellow, Name = "Материк демонов" }; }
                        case 2: { return new MechEngine.Biom() { Affix = "Залежи серы", Map = DemonMaps.Sulphur, Color = ConsoleColor.DarkYellow, Name = "Материк демонов" }; }
                        case 3: { return new MechEngine.Biom() { Affix = "Место углей", Map = DemonMaps.Coal, Color = ConsoleColor.DarkYellow, Name = "Материк демонов" }; }
                        default: { return new MechEngine.Biom(); }
                    }
                }
            }

            public static MechEngine.Biom Dwarfs
            {
                get
                {
                    switch (r.Next(4))
                    {
                        case 0: { return new MechEngine.Biom() { Affix = "Рабочие траншеи", Map = DwarfMaps.Trench, Color = ConsoleColor.Gray, Name = "Пещеры дварфов" }; }
                        case 1: { return new MechEngine.Biom() { Affix = "Паучьи пещеры", Map = DwarfMaps.SpiderCave, Color = ConsoleColor.Gray, Name = "Пещеры дварфов" }; }
                        case 2: { return new MechEngine.Biom() { Affix = "Основные траншеи", Map = DwarfMaps.MaxTrench, Color = ConsoleColor.Gray, Name = "Пещеры дварфов" }; }
                        case 3: { return new MechEngine.Biom() { Affix = "Рухнувшие траншеи", Map = DwarfMaps.DestroyTrench, Color = ConsoleColor.Gray, Name = "Пещеры дварфов" }; }
                        default: { return new MechEngine.Biom(); }
                    }
                }
            }

            public static MechEngine.Biom Undeads
            {
                get
                {
                    switch (r.Next(4))
                    {
                        case 0: { return new MechEngine.Biom() { Affix = "Погост", Map = UndeadMaps.Chyrchyard, Color = ConsoleColor.DarkGray, Name = "Мертвые земли" }; }
                        case 1: { return new MechEngine.Biom() { Affix = "Залежи костей", Map = UndeadMaps.Bones, Color = ConsoleColor.DarkGray, Name = "Мертвые земли" }; }
                        case 2: { return new MechEngine.Biom() { Affix = "Болото", Map = UndeadMaps.Swamp, Color = ConsoleColor.DarkGray, Name = "Мертвые земли" }; }
                        case 3: { return new MechEngine.Biom() { Affix = "Руины", Map = UndeadMaps.Ruins, Color = ConsoleColor.DarkGray, Name = "Мертвые земли" }; }
                        default: { return new MechEngine.Biom(); }
                    }
                }
            }

            private static class DrowMaps
            {
                /// <summary>
                /// Святилище эльфов
                /// </summary>
                public static string ElvesHoly
                {
                    get
                    {
                        return
@"72
23
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222000000000000000000000000000000000000000000000000000000000000022222
22222000000000000000000000000000000000000000000000000000000000000022222
22222000000222222222222222222222222222222222222222222222222200000022222
22222000000222222222222222222222222222222222222222222222222200000022222
22222200000222222222222222222222222222222222222222222222222200000222222
22222200000222222222222220000000000000000000000022222222222200000222222
22222220000222222222222220000000000000000000000022222222222200002222222
22222220000222222222222220000000000000000000000022222222222200002222222
22222222000222222222222220000000000000000000000022222222222200022222222
22222222000222222222222220000000000000000000000022222222222200022222222
22222222200222222222222220000000000000000000000022222222222200222222222
22222222200222222222222222222220000000000022222222222222222200222222222
22222222220222222222222222222220000000000022222222222222222202222222222
22222222220222222222222222222220000000000022222222222222222202222222222
22222222222222222222222222222220000000000022222222222222222222222222222
22222222222222222222222222222220000000000022222222222222222222222222222
22222222222222222222222222222220000000000022222222222222222222222222222
22222222222222222222222222222220000000000022222222222222222222222222222
22222222222222222222222222222220000000000022222222222222222222222222222
22222222222222222222222222222220000000000022222222222222222222222322222
22222222222222222222222222222222222222222222222222222222222222222222222
";
                    }
                }
                /// <summary>
                /// Лабиринт испытаний
                /// </summary>
                public static string Labirinth
                {
                    get
                    {
                        return
@"72
23
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222000000000000000000000000000000000000000000000000000000022222222
22222222222000000000000000000000000000000000000000000000000022222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222200000000000000000000000000000000000000222222222222222
22222222222222222222000000000000000000000000000000002222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222200000000000000000022222222222222222222222222
22222222222222222222222222222220000000222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22220000000000000000222222222222220022222222222000000000000000000000222
22220000000000000000222222222222200002222222222000000000000000000000222
22220000000000000000222222222220000000222222222000000000000000000000222
22222222222222222222222222222000000000022222222222222222222222222222222
22222222222222222222222222220000000000002222222222222222222222222322222
22222222222222222222222222222222222222222222222222222222222222222222222
";
                    }
                }
                /// <summary>
                /// Лабиринт старейшин
                /// </summary>
                public static string UnderOlder
                {
                    get
                    {
                        return
@"72
23
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222200002222222222222222222222222222222222222222222222222222222
22222222220000000022222222222222222222222222222222222222220000222222222
22222222222200002222222222222222222222222222222222222222000000002222222
22222222222222222222222222222222222222222222222222222222220000222222222
22222222200222222222222222222222200002222222222222222222222222222222222
22222222200222222222222222222222000000222222222222222222222222222222222
22222222200222222222222222222000000000000222222222222222222222222222222
22222222200222222222222220000000000000000000222222222222220000222222222
22222222200222222222220000000000000000000000000222222222000000002222222
22222222200222222222222220000000000000000000222222222222220000222222222
22222222200222222222222222222000000000000222222222222222222222222222222
22222222200222222222222222222222000000222222222222222222222222222222222
22222222200222222222222222222222200002222222222222222222222222222222222
22222222200222222222222222222222220022222222222222222222222222222222222
22222222200222222222222222222222222222222222222222222222200002222222222
22222222222222222222222222222222222222222222222222222220000000022222222
22222222222200002222222222222222222222222222222222222222200002222222222
22222222220000000022222222222222222222222222222222222222222222222222222
22222222222200002222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222322222
22222222222222222222222222222222222222222222222222222222222222222222222
";
                    }
                }
                /// <summary>
                /// Казармы
                /// </summary>
                public static string Barraks
                {
                    get
                    {
                        return
@"72
23
22222222222222222222222222222222222222222222222222222222222222222222222
22000000000022222222222222222220000000000222222222222222220000000000222
22000000000022222222222222222220000000000222222222222222220000000000222
22000000000022222222222222222220000000000222222222222222220000000000222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222200000000000000000000000000000222222222222222222222
22000000000022222222200000000000000000000000000000222222222222222222222
22000000000022222222200000000000000000000000000000222222222222222222222
22000000000022222222200000000000000000000000000000222222222222222222222
22222222222222222222200000000000000000000000000000222222222000000000022
22222222222222222222200000000000000000000000000000222222222000000000022
22222222222222222222200000000000000000000000000000222222222000000000022
22222222222222222222200000000000000000000000000000222222222222222222222
22222222222222222222200000000000000000000000000000222222222222222222222
22222222222222222222200000000000000000000000000000222222222222222222222
22222222222222222222200000000000000000000000000000222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22000000000022222222222222222222222222222222222222222222000000000022222
22000030000022222222222222222222222222222222222222222222000000000022222
22000000000022222222222222222222222222222222222222222222000000000022222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
";
                    }
                }
            }
            private static class ElvesMaps
            {
                /// <summary>
                /// Круги силы
                /// </summary>
                public static string PowerCircles
                {
                    get
                    {
                        return
@"72
23
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222000000000000000000000000000000000000000022222222222222222222222
22222222000000000000000000000000000000000000000022222222222222222222222
22222222000000000000000000000000000000000000000022222222222222222222222
22222222000000222222222222222222222222222200000022222222222222222222222
22222222000000222222222222222222222222222200000022222222222222222222222
22222222000000222222222222222222222222222200000022222222222222222222222
22222222000000222222222222222222222222222200000022222222222222222222222
22222222000000222222222222222222222222222200000022222222222222222222222
22222222000000222222222222222222222222222200000022222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222200000000000000000000000000000000000000002222
22222222222222222222222222200000000000000000000000000000000000000002222
22222222222222222222222222200000022222222222222223222222222220000002222
22222222222222222222222222200000022222222222222222222222222220000002222
22222222222222222222222222200000022222222222222222222222222220000002222
22222222222222222222222222200000022222222222222222222222222220000002222
22222222222222222222222222200000022222222222222222222222222220000002222
22222222222222222222222222200000022222222222222222222222222220000002222
22222222222222222222222222222222222222222222222222222222222222222222222
";
                    }
                }
                /// <summary>
                /// Дорога друидов
                /// </summary>
                public static string DruidsWay
                {
                    get
                    {
                        return
@"72
23
22222222222222222222222222222222222222222222222222222222222222222222222
22220000000000000000000000000222222222222222222222222222222222222222222
22220000000000000000000000000222222222222222222222222222222222222222222
22220000000002222200000000000222222222222222222222222222222222222222222
22220000000002222200000000000222222222222222220000002222222222222222222
22220000000000000000000000000222222222222222220000002222222222222222222
22220000000000000000000000000222222222222222220000002222222222222222222
22222222222222222222222222222222222222222222220000002222222222222222222
22222222222222222222222222222222222222222222220000002222222222222222222
22222222222222222222222222222222222222222222220000002222222222222222222
20000000000000000000000000000000000000000000000000000000000000000000002
20000000000000000000000000000000000000000000000000000000000000000000002
20000000000000000000000000000000000000000000000000000000000000000000002
22222222222222000000222222222222222222222222222222222222222222222222222
22222222222222000000222222222222222222222222222222222222222222222222222
22222222222222000000222222222222220000000000000000000000000222222222222
22222222222222000000222222222222220000000000000000000000000222222222222
22222222222222000000222222222222220000000002222200000000000222222222222
22222222222222000000222222222222220000000002222200000000000222222222222
22222222222222000000222222222222220000000000000000000000000222222222222
22222222222222222222222222222222220000000000000000000000000222222222222
22222222222222222222222222222222222222222222222222222222222222222322222
22222222222222222222222222222222222222222222222222222222222222222222222
";
                    }
                }
                /// <summary>
                /// Мировое дерево
                /// </summary>
                public static string WorldTree
                {
                    get
                    {
                        return
@"72
23
22222222222222222222222222222222222222222222222222222222222222222222222
22000000000000000000000000000000000000000000000000000000000000000000022
22000000000000000000000000000000000000000000000000000000000000000000022
22000222222222222222222222222222222222222222222222222222222222222200022
22000222222222222222222222222222222222222222222222222222222222222200022
22000222220000000000000000000000000000000000000000000000000002222200022
22000222220000000000000000000000000000000000000000000000000002222200022
22000222220002222222222222222222222222222222222222222222220002222200022
22000222220002222222222222222222222222222222222222222222220002222200022
22000222220002222222222222222222222222222222222222222222220002222200022
22000222220002222222222222222222222222222222222222222222220002222200022
22002222220002222222222222222222223222222222222222222222220002222200022
22000222220002222222222222222222222222222222222222222222220002222200022
22000222220002222222222222222222222222222222222222222222220002222200022
22000222220002222222222222222222222222222222222222222222220002222200022
22000222220002222222222222222222222222222222222222222222220002222200022
22000222220002222222222222222222222222222222222222222222220002222200022
22000222220000000000000000000000000000000000000000000000000002222200022
22000222220000000000000000000000000000000000000000000000000002222200022
22000222222222222222222222222222222222222222222222222222222222222200022
22000222222222222222222222222222222222222222222222222222222222222200022
22000000000000000000000000000000000000000000000000000000000000000000022
22222222222222222222222222222222222222222222222222222222222222222222222
";
                    }
                }
                /// <summary>
                /// Корни Мирового Дерева
                /// </summary>
                public static string TreeCore
                {
                    get
                    {
                        return
@"72
23
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222200000000000000000000000000000000000000000000000000000000000222222
22222200000000000000000000000000000000000000000000000000000000000222222
22220000000002222222222222222222222222222222000000000000000000000222222
22220000000222222222222222222222222222222222220000000000000000000222222
22220000000000000222222222222222222222222222222222000000000000000222222
22222222200000000000000002222222222222222222222222222222222222222222222
22222222222222200000000000000002222220000000000000222222222222222222222
22222222222222222222000000000000000000000000000002222222222222222222222
22222222222222222222222222222000000000000002222222222222222222222222222
22222222222222222222222222000000000000002222222222222222200000000222222
22222222222222222222000000000000000032222222222222222000000000000222222
22222222222222000000000000000000002222222222000000000000000222222222222
22222222000000000000000022222222222222222222222222222222222222222222222
22222000000002222222222222222222222222222222220000000000222222222222222
22220002222222222222222222222222222222220000000000000002222222222222222
22222000002222222222222222222222222222222222222222222002222222222222222
22222000000000022222222222222222222222222222222222222002222222222222222
22222000000000000000000000000000000000000000000000000002222222222222222
22222222222222222222222222222222222222222222222222222222222222222322222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
";
                    }
                }
            }
            private static class MountMaps
            {
                /// <summary>
                /// главный хребет
                /// </summary>
                public static string MainMount
                {
                    get
                    {
                        return
@"72
23
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222200000222222222222
22222222222222222222222222222222222222222222222222220000000000222222222
22222222222222222222222222222222222222222222222220000022222000002222222
22222222222222222222222222222222222222222222222000002222222220000022222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222002222222222222222222222222222222222222222222222222222222222
22222222200000222222222222222222222222222222222222222222222222222222222
22222220000000000222222222222222222222222222222222222222222222222222222
22220000022222000002222222222222222222222222222222222222222222222222222
22000002222222220000022222222222222222222222222222222222222222222222222
22222222222222222222222222222222220000000000000000000002222222222222222
22222222222222222222222222222220000000000000000000000000022222222222222
22222222222222222222222222220000000000000000000000000000000000222222222
22222222222222222222222220000000000000000000000000000000000000000222222
22222222222222222222222000000000000000000000000000000000000000000002222
22222222222222222222200000000000000000000000000000000000000000000000022
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222322222
22222222222222222222222222222222222222222222222222222222222222222222222
";
                    }
                }
                /// <summary>
                /// Разлом
                /// </summary>
                public static string Rift
                {
                    get
                    {
                        return
@"72
23
22222222222222222222222222222222222222222222222222222222222222222222222
22200000000000022222222222222222222222222220000000000000222222222222222
22220000000000000222222222222222222220000000000000000000000222222222222
22222222000000000000222222222222222000000000000002222222222222222222222
22222222222222000000000000000000000000002222222222222222222222222222222
22222222222222222200000000000000000222222222222222222222222222222222222
22222222222222222222222200000000222222222222222222222222222222222222222
22222222222222222222220000000022222222222222222222222222222222222222222
22222222222222222222222000000002222222222222222222222222222222222222222
22222222222222222222222220000000000002222222222222222222222222222222222
22222222222222222222222222222200000000000000000222222222222222222222222
22222222222222222222222222222222200000000000000000000222222222222222222
22222222222222222222222222222222220000000000000000022222222222222222222
22222222222222222222222222222222222000000000000222222222222222222222222
22222222222222222222222222222000000000000022222222222222222222222222222
22222222222222222222222220000000000000022222222222222222222222222222222
22222222222222222000000000000000000222222222222222222222222222222222222
22222222222000000000000000000000222222222222222222222222222222222222222
22222222222222222000000000000000000000000000002222222222222222222222222
22222222222222222220000000000000000000000000000022222222222222222222222
22222222222222222222200000000000000000000000000000222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222322222
22222222222222222222222222222222222222222222222222222222222222222222222
";
                    }
                }
                /// <summary>
                /// Обрыв
                /// </summary>
                public static string Yama
                {
                    get
                    {
                        return
@"72
23
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222200000000000000000000000000000000000002222222222222222
22222222222222222000000000222000000000000000000000000002222222222222222
22222222222222220000000000002220000000000000000000000000022222222222222
22222222222222200000000000222000000000000000000000000000002222222222222
22222222222220000000000000022200000000000000000000000000002222222222222
22222222222200000000000002220000000000000000000000000000000022222222222
22222222222000000000000000222000000000000000000000000000000222222222222
22222222220000000000000000022220000000000000000000000000000000222222222
22222222200000000000000022220000000000000000000000000000000000222222222
22222222000000000000000000222200000000000000000000000000000000000222222
22222222222222222200000000000222000000022200000000000022222222222222222
22222222222222222000000000000222222222200000000000000002222222222222222
22222222222222220000000000000000000002220000000000000002222222222222222
22222222222222200000000000000000000000022220000000000000222222222222222
22222222222220000000000000000000000000002222000000000000000222222222222
22222222222200000000000000000000000000000002220000000000000002222222222
22222222222000000000000000000000000000000000022200000000000022222222222
22222222220000000000000000000000000000000000000000000000000000222222222
22222222200000000000000000000000000000000000000000000000000000022222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222322222
22222222222222222222222222222222222222222222222222222222222222222222222
";
                    }
                }
                /// <summary>
                /// Северные дороги
                /// </summary>
                public static string NorthWays
                {
                    get
                    {
                        return
@"72
23
22222222222222222222222222222222222222222222222222222222222222222222222
22220000022222222222222222222222222222222222200000222222222222000022222
22222000000222222222200000000000220000022220000200000222222000022222222
22222222000000002200000020000000000000000000022222000002000022222222222
22222222222200000000222222200000002200000222222222222000002222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22220000022222222222222222222222222222222222200000222222222222000022222
22222000000222222222200000222220000022220000200000222222000022222222222
22222222000000000000000020000000000000000000022222000002000022222222222
22222222000000000000022222220000000220000022222222222200000222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222000022222
22222222222222222222222222222222222222222220000000000222222000022222222
22222222000000002200000000002222222222200000022222000002000000000000022
22222222222200000000222222200000002200000222222222222000002222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222322222
22222222222222222222222222222222222222222222222222222222222222222222222
";
                    }
                }
            }
            private static class DemonMaps
            {
                /// <summary>
                /// Огненные реки
                /// </summary>
                public static string FireRivers
                {
                    get
                    {
                        return
@"72
23
22222222222222222222222222222222222222222222222222222222222222222222222
22222222000000000000222222222222222222222222222222220000000000002222222
22222222000000000000222222222222222222222222222222220002222220002222222
22222222000000000000222222222222222222222222222222220002222000002222222
22222222000000000000222222222222222222222222222222220000022220002222222
22222222000000000000222222222222222222222222222222220002220000002222222
22222222002222222000222222222222222222222222222222220000000000002222222
22222222000022000000222222222222222222222222222222220000000000002222222
22222222000022200000222222222222222222222222222222220000000000002222222
22222222000002222200222222222222222222222222222222220000000000002222222
22222222000000000000222222222222222222222222222222220000000000002222222
22222222000000000000222222222222222222222222222222220000000000002222222
22222222000000000000222222222222222222222222222222220002222222002222222
22222222000000000000222222222222222222222222222222220022222200002222222
22222222000000000000222222222222222222222222222222220000222220002222222
22222222000000000000222222222222222222222222222222220000000000002222222
22222222000000000000222222222222222222222222222222220000000000002222222
22222222000000222000222222222222222222222222222222220000000000002222222
22222222000002222200222222222222222222222222222222220000000000002222222
22222222000222200000222222222222222222222222222222220000000000002222222
22222222000002222000222222222222222222222222222222220000000000002222222
22222222000000000000222222222222222222222222222222220000000000002322222
22222222222222222222222222222222222222222222222222222222222222222222222
";
                    }
                }
                /// <summary>
                /// Кратер возгорания
                /// </summary>
                public static string FirePlace
                {
                    get
                    {
                        return
@"72
23
22222222222222222222222222222222222222222222222222222222222222222222222
22222222220000002222222222002222222222222222000000200000022222222222222
22222200000022222222222222202202222222222002222222222222200000022222222
22000000222222222222222220022200222222202002222222202222220000002222222
22000000222222222222200222002222220022220022002222220002222200000022222
22200000022222222222202022002000000200200222002200222202002222000000222
22222000000222222222200220022220000222002222002222200220220022220000002
22220000002222222222022222220000022220000000002220022220200222220000002
22220000002222222000222222000000000000000000000000000222002222200000022
22200000022222222002222000000000000000000000000000000220022222200000022
22000000222222222200220000000000000000000000000000022200222222200000022
22200000022222222222200222200000000000000000000000022220022222000000222
22222000000222222220022222220000000000000022222222200222222000000222222
22222000000222222222220000000222222222222222222200000002222222200000022
22222000000222222222222222000000002222222220000000022222222200000022222
22222200000022222222222222222000000000000000022222222220000000000000222
22222200000022222222222222222222222222222222222220000000002222222222222
22220000002222222222222222222222222222222222000000000000222222222222222
22222222000000000000000000000000000000000000000000000000000000000222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222322222
22222222222222222222222222222222222222222222222222222222222222222222222
";
                    }
                }
                /// <summary>
                /// Залежи серы
                /// </summary>
                public static string Sulphur
                {
                    get
                    {
                        return
@"72
23
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222000000000002222222222222222222222222222222222
22222222222222222222222222000000000002222222222222222222222222222222222
22222222222222222222222000000000000000022222222222222222222222222222222
22222222222222222222200002222222222200022222222222222222222222222222222
22222222222222000000000002222222222200000000000222222222222222222222222
22222222222222000000000002222222222200000000000222222222222222222222222
22222220000000000000000002222222222200000000000222222222222222222222222
22222220000002222222222222222222222222222220000222222222222222222222222
22000000000002222222222222222222222222222220000000000000000000000002222
22000000000002222222222222222222222222222222222222222222000000000002222
22000000000002222222222222222222222222222222222222222222000000000002222
22222222222002222222222222222222222222222222222222222222002222222222222
22222222222002222222222222222222222222222222222222222222002222222222222
22222222222002222222222222222222222222222222200000000000002222222222222
22222220000000000000000000000000000000000000000000000000222222222222222
22222220000000000022222222222222222222222222200000000000222222222222222
22222220000000000022222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222322222
22222222222222222222222222222222222222222222222222222222222222222222222
";
                    }
                }
                /// <summary>
                /// Место углей
                /// </summary>
                public static string Coal
                {
                    get
                    {
                        return
@"72
23
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222220000022222222222222222222000002222222222222222222222222
22222222222222222222000000022222222000000022222222222222222222222222222
22222222222222222222222222002222220022222222222222222222222222222222222
22222222222222222222222222200222200222222222222222222222222222222222222
22222222222222222222222222220000002222222222222222222222222222232222222
22222222222222222222222222222200222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
";
                    }
                }
            }
            private static class DwarfMaps
            {
                /// <summary>
                /// Рабочие траншеи
                /// </summary>
                public static string Trench
                {
                    get
                    {
                        return
@"72
23
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22220000000000000000000000000000000000000000000000000000000000000000222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22220000000000000000000000000000000000000000000000000000000000000000222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22220000000000000000000000000000000000000000000000000000000000000000222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22220000000000000000000000000000000000000000000000000000000000000000222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22220000000000000000000000000000000000000000000000000000000000000000222
22222222222222222222222222222222222222222222222222222222222222222322222
22222222222222222222222222222222222222222222222222222222222222222222222
";
                    }
                }
                /// <summary>
                /// Паучьи пещеры
                /// </summary>
                public static string SpiderCave
                {
                    get
                    {
                        return
@"72
23
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22220000000000000000000000000000000000000000000000000000000000000000222
22220222222220222222222222022222222202222222222202222222202222222220222
22220222222202222222222222022222222202222222222022222222202222222220222
22220222222220222222222220222222222022222222220222222222022222222202222
22202222222220222222222202222222222202222222220222222222202222222202222
22202222222222022222222220222222222202222222202222222222202222222220222
22022222222222022222222222022222222202222222202222222222220222222202222
22022222222220222222222222022222222220222222022222222222220222222220222
22202222222220222222222222022222222220222222202222222222202222222220222
22220222222202222222222222022222222220222222220222222222202222222220222
22222022222220222222222220222222222222022222222022222222220222222202222
22222202222222022222222202222222222222202222222202222222220222222220222
22222220222222202222222220222222222222220222222202222222220222222220222
22222202222222220222222222022222222222220222222202222222202222222202222
22222022222222220222222222202222222222202222222220222222220222222220222
22222022222222220222222222202222222222022222222220222222222022222220222
22222022222222202222222222220222222220222222222222022222222022222202222
22220222222222022222222222222022222202222222222222202222222022222220222
22220222222220222222222222222022222220222222222222220222222202222222222
22202222222222022222222222222202222222022222222222202222222220222322222
22222222222222222222222222222222222222222222222222222222222222222222222
";
                    }
                }
                /// <summary>
                /// Основные траншеи
                /// </summary>
                public static string MaxTrench
                {
                    get
                    {
                        return
@"72
23
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22220000000000000000000000000000000000000000000000000000000000000000222
22220000000000000000000000000000000000000000000000000000000000000000222
22220000000000000000000000000000000000000000000000000000000000000000222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22220000000000000000000000000000000000000000000000000000000000000000222
22220000000000000000000000000000000000000000000000000000000000000000222
22220000000000000000000000000000000000000000000000000000000000000000222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22220000000000000000000000000000000000000000000000000000000000000000222
22220000000000000000000000000000000000000000000000000000000000000000222
22220000000000000000000000000000000000000000000000000000000000000000222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22220000000000000000000000000000000000000000000000000000000000000000222
22220000000000000000000000000000000000000000000000000000000000000000222
22220000000000000000000000000000000000000000000000000000000000000300222
22222222222222222222222222222222222222222222222222222222222222222222222
";
                    }
                }
                /// <summary>
                /// Рухнувшие траншеи
                /// </summary>
                public static string DestroyTrench
                {
                    get
                    {
                        return
@"72
23
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22220000000000000000000000000000000000000022222222222222222000000000222
22220000002222222222222222200000000000000000000000000000000000000000222
22220000000000000000000000000000000000222222222222222222200000000000222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222200000000000000000000000000000000000000000000000002222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22220000000000000000000000000000222222222222222222222222222000000000222
22220000222222222222222222220000000000000000000000000000000000000000222
22220000000000000000022222222222222222222222222222200000000000000000222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22220000022222222222222222222222222222222222222222222222222222222000222
22220000000000000000000002222222222222222222222000000000000000000000222
22220000022222222222222222222222222222222222222222222222222222222000222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22220000000000000000000000000000000000000000000000000000000000000000222
22220000000000000000000000000000000000000000000000000000000000000000222
22220000000000000000000000000000000000000000000000000000000000000300222
22222222222222222222222222222222222222222222222222222222222222222222222
";
                    }
                }
            }
            private static class UndeadMaps
            {
                /// <summary>
                /// Погост
                /// </summary>
                public static string Chyrchyard
                {
                    get
                    {
                        return
@"72
23
22222222222222222222222222222222222222222222222222222222222222222222222
22220000000000000000000000000000000000000000000000000000000000002222222
22222222000000000000000000000000000000000000000000000000000022222222222
22222222222200000000000000000000000000000000000000000000022222222222222
22222222222222222222222222220000000000002222222222222222222222222222222
22222222222222222222222222220000000000002222222222222222222222222222222
22222222222222222222222222220000000000002222222222222222222222222222222
22222222222222222222222222220000000000002222222222222222222222222222222
22222222222222222222222222222222000002222222222222222222222222222222222
22222222222222222222222222222222000002222222222222222222222222222222222
22222222222222222222222222222222000002222222222222222222222222222222222
22222222222222222222222222222222000002222222222222222222222222222222222
22222222222222222222222222222222000002222222222222222222222222222222222
22222222222222222222222222222222000002222222222222222222222222222222222
22222222222222222222222222222222000002222222222222222000000000000002222
22220000000022222222222222222222000000000000000000000000000000000002222
22220000000000000000000000000000000002222222222222222000000000000002222
22220000000022222222222222222222000002222222222222222222222222222222222
22222222222222222222222222222222000002222222222222222222222222222222222
22222222222200000000000000000000000000000000000000000000000000222222222
22222222222200000000222222222222222222222222222200000000000000222222222
22222222222200000000222222222222222222222222222200000000000000222222322
22222222222222222222222222222222222222222222222222222222222222222222222
";
                    }
                }
                /// <summary>
                /// Залежи костей
                /// </summary>
                public static string Bones
                {
                    get
                    {
                        return
@"72
23
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22220000000022222222222222222222222222222222222222222220000000022222222
22220000000022222222222222222222222222222222222222222220000000022222222
22220000000022222222222222222222222222222222222222222220000000022222222
22222200000000000000000000000000000000000000000000000000000000222222222
22222200000000000000000000000000000000000000000000000000000000222222222
22220000000022222222222222222222222222222222222222222220000000022222222
22220000000022222222222222222222222222222222222222222220000000022222222
22220000000022222222222222222222222222222222222222222220000000022222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222220000000000000000000222222222222222222222222222
22222222222220000000000000000222222222220000000000000022222222222222222
22222222220000000000002222222222222222222222222200000000000000000222222
22222222222220000000000000000002222222200000000000000000222222222222222
22222222222222222222222000000000000000000000000002222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222223222
22222222222222222222222222222222222222222222222222222222222222222222222
";
                    }
                }
                /// <summary>
                /// Болото
                /// </summary>
                public static string Swamp
                {
                    get
                    {
                        return
@"72
23
22222222222222222222222222222222222222222222222222222222222222222222222
22222222000000000222222222222222222222222222222220000000000222222222222
22220000022222000000222222222222222222222222000000222222200022222222222
22000002222222222222200000022222222222220000002222222220000000222222222
22222000000222222222222222220000200000002222222222200000002222222222222
22222222000002222222222222222000000022222222222220000000222222222222222
22222222220000002222222222222222222222222200000002222222222222222222222
22222222222222000002222222222222222222200000002222222222222222222222222
22222222222000000022222222222222200000002222200000000000022222222222222
22222222000000022222222222222222222000000022222222200000000022222222222
22222222222000000222222222222220000000222222222222222222220000002222222
22222222222222200000002222222222222222222222222222222222222220000222222
22222222222222222220000000002222222222222222222222222222222222200002222
22222222222222222222222200000000222222222222222222222222222200000222222
22222222222222222200000000022222222222222222222222222222000000222222222
22222222222222000000000222222222222222222200000002222222222000222222222
22222222200000000022222222222222222000000002222000000022200002222222222
22220000000022222222222222222000000002222222222222200000000222222222222
22222220000000000222222000000002222222222222222222222222222222222222222
22222222220000000022000000222222222222222222222222222222222222222222222
22222222222200000000022222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222223222
22222222222222222222222222222222222222222222222222222222222222222222222
";
                    }
                }
                /// <summary>
                /// Руины
                /// </summary>
                public static string Ruins
                {
                    get
                    {
                        return
@"72
23
22222222222222222222222222222222222222222222222222222222222222222222222
22222222000000000222222222222222222222222222222220000000000222222222222
22220000022222000000222222222222222222222222000000220222200022222222222
22000002222222022222200000022222222222220000002222220220000000222222222
22222000000000000000000000000000000000000000000000000000002222222222222
22222222000002222222022222222000000022222202222220000000222222222222222
22222222220000002222022222222222222222222200000002222222222222222222222
22222222222222000000000000000000000000000000002222222222222222222222222
22222222222000000022222222022222200000002222200000000000022222222222222
22222222000000022222222222022222222000000022202222200000000022222222222
22222222222000000222222222022220000000222222202222222222220000002222222
22222222222222200000002222022222222222222222202222222222222220000222222
22222222222222222220000000000000000000000000000000000000000000000222222
22222222222222222222222200000000222222022222222222222222222200000222222
22222222222222222200000000022222222222022222222222222222000000222222222
22222222222222000000000220022222222222022200000002222222222000222222222
22222222200000000022222220222222222000000002222000000022200000022222222
22220000000022222222222220222000000002222022222022200000000220002222222
22222220000000000222222000000002222222222022222022222222222222022222222
22222222220000000022000000000000000000000000000000000000000000022222222
22222222222200000000022222222222222222222222222222222222222222222222222
22222222222222222222222222222222222222222222222222222222222222222223222
22222222222222222222222222222222222222222222222222222222222222222222222
";
                    }
                }
            }
        }
    }
}