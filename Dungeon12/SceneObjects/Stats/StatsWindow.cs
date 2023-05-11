using Dungeon;
using Dungeon.Control;
using Dungeon.Control.Keys;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using Dungeon12.Entities;
using Dungeon12.Entities.Enums;
using Dungeon12.SceneObjects.Base;

namespace Dungeon12.SceneObjects.Stats
{
    internal class StatsWindow : SceneControl<Hero>, IAutoFreeze
    {
        Title CharacterName;
        ImageObject Avatar;
        ClassBadge _class;
        InventorySceneObject _inventory;

        public StatsWindow(ISceneLayer layer) : base(Global.Game.Party.Hero1)
        {
            this.Layer = layer;
            this.Width=1141;
            this.Height =554;

            this.Image="UI/Windows/Stats/back.png";

            this.AddChild(new Title(Global.Strings["Инвентарь отряда"], 325, 42)
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
            this.AddChild(new ImageObject("UI/start/icon.png") { Left=610, Top=307 });
            _class = this.AddChild(new ClassBadge(Component.Archetype) { Left=613, Top=310 });

            this.AddTextPos(TextSeg(Global.Strings["Основное"], 14), 250, 85, 190, 18);
            this.AddTextPos(TextSeg(Global.Strings["Защита"], 14), 250, 263, 192, 16);
            this.AddTextPos(TextSeg(Global.Strings["Дополнительно"], 14), 38, 316, 190, 18);
            this.AddTextPos(Global.Strings["Свободные очки"].SegoeUIBold().InSize(16).InColor(Global.CommonColorLight), 32, 30, 208, 26);

            this.AddTextPos(TextGab(Global.Strings["Навыки"], 20), 35, 80, 200, 26);

            this.AddTextPos(TextGab(Global.Strings["Характеристики"], 20), 32, 197, 200, 26);

            Fill(Component);

            this.AddChild(new SlideButton("Назад", SlideBack) { Left=243, Top=25 });
            this.AddChild(new SlideButton("Далее", SlideForward, false) { Left=723, Top=25 });

            this.AddChild(new Close29x29(this)
            {
                Left = 1104,
                Top = 8
            });
        }

        private int index = 0;

        private void SlideBack()
        {
            index--;
            if (index<0)
                index= 3;
            Fill(Global.Game.Party.Heroes[index]);
        }

        private void SlideForward()
        {
            index++;
            if (index>3)
                index= 0;
            Fill(Global.Game.Party.Heroes[index]);
        }

        private ParamBinder binderCommon;
        private ParamBinder binderDef;
        private ParamBinder binderAdd;
        private TextObject freePoints;
        private List<SceneObject<IDrawText>> skillAndstats = new();

        private void AddTexts(Hero hero)
        {
            freePoints?.Destroy();
            binderCommon?.Destroy();
            binderDef?.Destroy();
            binderAdd?.Destroy();
            skillAndstats.ForEach(x => x.Destroy());

            var leftSkill = 33;
            var topSkill = 106;

            hero.Archetype.Skills().ForEach(s =>
            {
                var sEnum = s.ToString();
                var skillName = Global.Strings[s];
                var text = TextGab(skillName);
                var hint = new TextObjectHint(text, skillName, skillName, Global.Strings.Description[sEnum]) { Left= leftSkill, Top= topSkill };
                var sk = this.AddChild(hint);
                var skv = this.AddTextPos(TextSeg(hero.SkillValue(s).ToString(), 18), 193, topSkill, 24, 20);
                skillAndstats.Add(sk);
                skillAndstats.Add(skv);
                topSkill+=22;
            });

            skillAndstats.Add(this.AddChild(new TextObjectHint(TextGab(Global.Strings[nameof(Hero.Strength)]), Global.Strings[nameof(Hero.Strength)], Global.Strings[nameof(Hero.Strength)], Global.Strings.Description[nameof(Hero.Strength)]) { Left= 33, Top= 223 }));
            skillAndstats.Add(this.AddTextPos(TextSeg(hero.Strength.ToString(), 18), 193, 222, 24, 20));
            
            skillAndstats.Add(this.AddChild(new TextObjectHint(TextGab(Global.Strings[nameof(Hero.Agility)]), Global.Strings[nameof(Hero.Agility)], Global.Strings[nameof(Hero.Agility)], Global.Strings.Description[nameof(Hero.Agility)]) { Left= 33, Top= 245 })); 
            skillAndstats.Add(this.AddTextPos(TextSeg(hero.Agility.ToString(), 18), 193, 245, 24, 20));

            skillAndstats.Add(this.AddChild(new TextObjectHint(TextGab(Global.Strings[nameof(Hero.Intellegence)]), Global.Strings[nameof(Hero.Intellegence)], Global.Strings[nameof(Hero.Intellegence)], Global.Strings.Description[nameof(Hero.Intellegence)]) { Left= 33, Top= 266 }));
            skillAndstats.Add(this.AddTextPos(TextSeg(hero.Intellegence.ToString(), 18), 193, 266, 24, 20));

            skillAndstats.Add(this.AddChild(new TextObjectHint(TextGab(Global.Strings[nameof(Hero.Stamina)]), Global.Strings[nameof(Hero.Stamina)], Global.Strings[nameof(Hero.Stamina)], Global.Strings.Description[nameof(Hero.Stamina)]) { Left= 33, Top= 289 })); 
            skillAndstats.Add(this.AddTextPos(TextSeg(hero.Stamina.ToString(), 18), 193, 289, 24, 20));

            freePoints = this.AddTextPos((Global.Strings[$"навыков и характеристик"]+$" {hero.FreePoints}").SegoeUIBold().InSize(16).InColor(Global.CommonColorLight), 32, 44, 208, 26);

            binderCommon = new ParamBinder(this, 255, 183, 104);
            binderCommon.AddParam("Уровень", hero.Level);
            binderCommon.AddParam("Опыт", $"{hero.Exp}/{hero.ExpTable[hero.Level]}");
            binderCommon.AddParam("Здоровье", hero.Hp.ToString());
            binderCommon.AddParam("Урон", hero.Damage.ToString("-"));
            binderCommon.AddParam("Сила атаки", hero.AD);
            binderCommon.AddParam("Сила магии", hero.AP);
            binderCommon.AddParam("Скорость", hero.Speed);
            binderCommon.AddParam("Меткость", $"{hero.Accuracy}%");
            binderCommon.AddParam("Шанс крит.", $"{hero.CritChance}%");

            binderDef=new ParamBinder(this, 254, 183, 277);
            binderDef.AddParam("Броня", hero.Armor);
            binderDef.AddParam("Класс брони", hero.ArmorClass);
            binderDef.AddEmpty();
            binderDef.AddParam("Отражение магии", $"{hero.SpellReflect}%");
            binderDef.AddParam("Защита от огня", hero.FireProtection);
            binderDef.AddParam("Защита от холода", hero.FrostProtection);
            binderDef.AddParam("Защита от магии", hero.MagicProtection);
            binderDef.AddParam("Защита от света", hero.LightProtection);
            binderDef.AddParam("Защита от тьмы", hero.DarkProtection);
            binderDef.AddParam("Уклонение", $"{hero.DodgeChance}%");
            binderDef.AddParam("Шанс физ. блока", $"{hero.BlockChance}%");

            binderAdd=new ParamBinder(this, 41, 189, 334);
            binderAdd.AddParam("Инициатива", hero.Initiative);
            binderAdd.AddParam("Проницание магии", hero.MagicPenetration);
            binderAdd.AddParam("Сокрушительный удар", $"{hero.CrushingBlowChance}%");
            binderAdd.AddParam("Скользящий удар", $"{hero.GlancingBlowChance}%");
            binderAdd.AddParam("Урон скольз. ударов", $"x{hero.GlancingBlowMultiplier}");
            binderAdd.AddParam("Парирование", $"{hero.ParryChance}%");
            binderAdd.AddEmpty();
            binderAdd.AddParam("Шанс физ. эффектов", $"{hero.EffectPhysicalChance}%");
            binderAdd.AddParam("Урон физ. эффектов", $"{hero.EffectPhysicalDamage}%");
            binderAdd.AddParam("Шанс маг. эффектов", $"{hero.EffectMagicChance}%");
            binderAdd.AddParam("Урон маг. эффектов", $"{hero.EffectMagicDamage}%");

        }

        private IDrawText TextSeg(string text, int size = 16)
            => text.AsDrawText().SegoeUIBold().InSize(size).InColor(Global.CommonColorLight);
        private IDrawText TextGab(string text, int size = 16)
            => text.AsDrawText().Gabriela().InSize(size).InColor(Global.CommonColorLight);

        private void Fill(Hero hero)
        {
            CharacterName?.Destroy();
            CharacterName = this.AddChild(new Title(hero.Name, 425, 38)
            {
                Left=297,
                Top=32,
            });

            _inventory?.Destroy();
            _inventory= this.AddChild(new InventorySceneObject(hero.Inventory)
            {
                Left =468,
                Top=101
            });

            Avatar.Image=hero.Avatar;
            _class.Set(hero.Archetype);
            AddTexts(hero);
        }

        protected override ControlEventType[] Handles => new ControlEventType[] { ControlEventType.Key };

        protected override Key[] KeyHandles => new Key[] { Key.I, Key.Escape };

        public override void KeyDown(Key key, KeyModifiers modifier, bool hold)
        {
            if (this.Drawed && (key== Key.I || key== Key.Escape) && !hold)
            {
                Global.Windows.Activate<StatsWindow>(this.Layer);
            }
            base.KeyDown(key, modifier, hold);
        }
    }
}