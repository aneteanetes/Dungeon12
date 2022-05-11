using Dungeon;
using Dungeon.Control;
using Dungeon.Control.Keys;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using Dungeon12.Entities;
using Dungeon12.Entities.Enums;
using Dungeon12.SceneObjects.Base;
using System.Collections.Generic;

namespace Dungeon12.SceneObjects.Stats
{
    public class StatsWindow : SceneControl<Hero>, IAutoFreeze
    {
        Title CharacterName;
        ImageObject Avatar;

        public StatsWindow() : base(Global.Game.Party.Hero1)
        {
            this.Width=1141;
            this.Height =554;

            this.Image="UI/Windows/Stats/back.png";

            this.AddChild(new Title(Global.Strings.PartyInventory, 325, 42)
            {
                Left=783,
                Top=28,
            });

            Avatar = this.AddChild(new ImageObject(Component.Avatar)
            {
                Width=115,
                Height=180,
                Left=554,
                Top=186,
            });

            this.AddTextPos(TextSeg("Основное",9), 250, 85, 190, 18);
            this.AddTextPos(TextSeg("Защита",9), 250, 263, 192, 16);
            this.AddTextPos(TextSeg("Дополнительно",9), 38, 316, 190, 18);
            this.AddTextPos("Свободные очки".SegoeUIBold().InSize(10).InColor(Global.CommonColorLight), 32, 30, 208, 26);

            this.AddTextPos(TextGab("Навыки", 14), 35, 80, 200, 26);

            this.AddTextPos(TextGab("Характеристики", 14), 32, 197, 200, 26);


            Fill(Component);

        }

        private ParamBinder binderCommon;
        private ParamBinder binderDef;
        private ParamBinder binderAdd;
        private TextObject freePoints;
        private List<TextObject> skillAndstats = new List<TextObject>();

        private void AddTexts(Hero hero)
        {
            freePoints?.Destroy();
            binderCommon?.Destroy();
            binderDef?.Destroy();
            binderAdd?.Destroy();
            skillAndstats.ForEach(x =>x.Destroy());

            var leftSkill = 33;
            var topSkill = 106;

            hero.Class.Skills().ForEach(s =>
            {
                var sk = this.AddText(TextGab(s.Display()), leftSkill, topSkill);
                var skv = this.AddTextPos(TextSeg(hero.SkillValue(s).ToString(),12), 193, topSkill,24,20);
                skillAndstats.Add(sk);
                skillAndstats.Add(skv);
                topSkill+=22;
            });

            skillAndstats.Add(this.AddText(TextGab("Сила"), 33, 223)); skillAndstats.Add(this.AddTextPos(TextSeg(hero.Strength.ToString(),12), 193, 222, 24, 20));
            skillAndstats.Add(this.AddText(TextGab("Ловкость"), 33, 245)); skillAndstats.Add(this.AddTextPos(TextSeg(hero.Agility.ToString(),12), 193, 245, 24, 20));
            skillAndstats.Add(this.AddText(TextGab("Интеллект"), 33, 266)); skillAndstats.Add(this.AddTextPos(TextSeg(hero.Intellegence.ToString(),12), 193, 266, 24, 20));
            skillAndstats.Add(this.AddText(TextGab("Выносливость"), 33, 289)); skillAndstats.Add(this.AddTextPos(TextSeg(hero.Stamina.ToString(),12), 193, 289, 24, 20));

            freePoints = this.AddTextPos($"навыков и характеристик: {hero.FreePoints}".SegoeUIBold().InSize(10).InColor(Global.CommonColorLight), 32, 44, 208, 26);

            binderCommon = new ParamBinder(this, 255, 183, 104);
            binderCommon.AddParam("Уровень", hero.Level);
            binderCommon.AddParam("Здоровье", hero.Hp.ToString());
            binderCommon.AddEmpty();
            binderCommon.AddParam("Урон", hero.Damage.ToString("-"));
            binderCommon.AddParam("Сила атаки", hero.AD);
            binderCommon.AddParam("Сила магии", hero.AP);
            binderCommon.AddParam("Скорость", hero.Speed);
            binderCommon.AddParam("Меткость", $"{hero.HitChance}%");
            binderCommon.AddParam("Шанс крит.", $"{hero.CritChance}%");

            binderDef=new ParamBinder(this, 254, 183, 277);
            binderDef.AddParam("Броня", hero.Armor);
            binderDef.AddParam("Класс брони", hero.ArmorClass);
            binderDef.AddEmpty();
            binderDef.AddParam("Отражение магии", $"{hero.MagicReflectChance}%");
            binderDef.AddParam("Защита от огня", hero.FireProtection);
            binderDef.AddParam("Защита от холода", hero.FrostProtection);
            binderDef.AddParam("Защита от магии", hero.MagicProtection);
            binderDef.AddParam("Защита от света", hero.LightProtection);
            binderDef.AddParam("Защита от тьмы", hero.DarkProtection);
            binderDef.AddParam("Уклонение", $"{hero.DodgeChance}%");
            binderDef.AddParam("Шанс физ. блока", $"{hero.BlockChance}%");

            binderAdd=new ParamBinder(this, 41, 189, 334);
            binderAdd.AddParam("Проницание магии", hero.MagicPenetration);
            binderAdd.AddParam("Сокрушительный удар", $"{hero.CrushingBlowChance}%");
            binderAdd.AddParam("Скользящий удар", $"{hero.GlancingBlowChance}%");
            binderAdd.AddEmpty();
            binderAdd.AddParam("Урон скольз. ударов", $"x{hero.GlancingBlowMultiplier}");
            binderAdd.AddParam("Парирование", $"{hero.ParryChance}%");
            binderAdd.AddEmpty();
            binderAdd.AddParam("Шанс физ. эффектов", $"{hero.EffectPhysicalChance}%");
            binderAdd.AddParam("Урон физ. эффектов", $"{hero.EffectPhysicalDamage}%");
            binderAdd.AddParam("Шанс маг. эффектов", $"{hero.EffectMagicChance}%");
            binderAdd.AddParam("Урон маг. эффектов", $"{hero.EffectMagicDamage}%");

        }

        private IDrawText TextSeg(string text, int size=16)
            => text.AsDrawText().SegoeUIBold().InSize(size).InColor(Global.CommonColorLight);
        private IDrawText TextGab(string text, int size = 12)
            => text.AsDrawText().Gabriela().InSize(size).InColor(Global.CommonColorLight);

        private void Fill(Hero hero)
        {
            CharacterName?.Destroy();
            CharacterName = this.AddChild(new Title(Component.Name, 425, 38)
            {
                Left=297,
                Top=32,
            });

            Avatar.Image=hero.Avatar;
            AddTexts(Component);
        }

        protected override ControlEventType[] Handles => new ControlEventType[] { ControlEventType.Key };

        protected override Key[] KeyHandles => new Key[] { Key.I };

        public override void KeyDown(Key key, KeyModifiers modifier, bool hold)
        {
            if (this.Drawed && key== Key.I && !hold)
            {
                Global.Windows.Activate<StatsWindow>(this.Layer);
            }
            base.KeyDown(key, modifier, hold);
        }
    }
}