using Dungeon;
using Dungeon.Drawing.SceneObjects;
using Dungeon.Scenes;
using Dungeon.Scenes.Manager;
using SidusXII.Enums;
using SidusXII.SceneObjects.ItemSelector;
using SidusXII.Scenes.Game;
using SidusXII.Scenes.MainMenu;
using System.Collections.Generic;
using System.Linq;

namespace SidusXII.Scenes.Creation
{
    public class CreationItemScene<T, TNext, TBack> : GameScene<TNext, TBack>
        where T : GameEnum
        where TNext : GameScene
        where TBack : GameScene
    {
        public CreationItemScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => true;

        public override void Initialize()
        {
            var back = CreateLayer("Background");
            back.AddObject(new ImageObject("Common/back.jpg".AsmImgRes()));

            var gui = CreateLayer("gui");

            var left = DungeonGlobal.Resolution.Width / 2d - 811 / 2d;
            gui.AddObject(new ItemSelector<T>(Back, Items)
            {
                Left = left,
                OnSelect = Next
            });
        }

        public virtual IEnumerable<GameEnum> Items { get; }

        private void Back() => Switch<TBack>();

        protected virtual void Next(GameEnum value)
        {
            if (value == default)
                return;

            Global.Game.Character.SetPropertyExprConverted(value.GetType().Name, value);
            this.Switch<TNext>();
        }
    }

    public class RaceScene : CreationItemScene<Race, ClassScene, MainMenuScene>
    {
        public RaceScene(SceneManager sceneManager) : base(sceneManager)
        {
            this.AvailableScenes.Add(typeof(SubRaceScene));
        }

        protected override void Next(GameEnum value)
        {
            if (value.As<Race>().HasSubRaces)
            {
                this.Switch<SubRaceScene>(value.PropertyName);
                return;
            }

            base.Next(value);
        }

        public override IEnumerable<GameEnum> Items => GameEnum.AllValues<Race>().Where(x => x.MainRace == default);
    }

    public class SubRaceScene : CreationItemScene<Race, ClassScene, RaceScene>
    {
        public SubRaceScene(SceneManager sceneManager) : base(sceneManager)
        {
            this.AvailableScenes.Add(typeof(RaceScene));
        }

        public override IEnumerable<GameEnum> Items => GameEnum.AllValues<Race>().Where(x => x.MainRace != default && x.MainRace.PropertyName == Args[0]);
    }

    public class ClassScene : CreationItemScene<Class, SpecScene, RaceScene>
    {
        public ClassScene(SceneManager sceneManager) : base(sceneManager) { }
    }

    public class SpecScene : CreationItemScene<Spec, ProfScene, ClassScene>
    {
        public SpecScene(SceneManager sceneManager) : base(sceneManager) { }

        public override IEnumerable<GameEnum> Items => Spec.ByClass(Global.Game.Character.Class).Select(s => s.As<GameEnum>());
    }

    public class ProfScene : CreationItemScene<Profession, MainScene, ClassScene>
    {
        public ProfScene(SceneManager sceneManager) : base(sceneManager) { }
    }
}