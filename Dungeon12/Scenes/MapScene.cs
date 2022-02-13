using Dungeon;
using Dungeon.Control.Keys;
using Dungeon.Drawing.SceneObjects;
using Dungeon.Scenes;
using Dungeon.Scenes.Manager;
using Dungeon12.Drawing.SceneObjects.Map;
using Dungeon12.ECS.Systems;
using Dungeon12.Entities.Map;
using Dungeon12.Functions.ObjectFunctions;
using Dungeon12.SceneObjects;
using Dungeon12.SceneObjects.Map;
using Dungeon12.SceneObjects.UI;
using Dungeon12.SceneObjects.UserInterface;

namespace Dungeon12.Scenes
{
    public class MapScene : GameScene<StartScene>
    {
        public MapScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => true;

        public override bool AbsolutePositionScene => false;
        public override void Initialize()
        {
            Global.Helps = new HelpingSceneObject();

            Global.RegisterFunction<NameEnterWindowFunction>();
            Global.RegisterFunction<SelectOriginFunction>();
            Global.RegisterFunction<SelectCraftFunction>();
            Global.RegisterFunction<SelectFractionFunction>();
            Global.RegisterFunction<SelectSpecFunction>();
            Global.RegisterFunction<HeroConfirmFunction>();
            Global.RegisterFunction<DialogueFunction>();

            Global.Game = new Game()
            {
                Party = new Entities.Party()
                {
                    Hero1 = new Entities.Hero()
                },
                Calendar = new Entities.Calendar()
            };

            var background = this.CreateLayer("background");
            background.AddObject(new ImageObject("Backgrounds/ship_fxd.png".AsmImg())
            {
                Width = Global.Resolution.Width,
                Height = Global.Resolution.Height
            });

            var mapLayer = this.CreateLayer("map");

            var region = Region.Load("Ship");
            var regionSceneObj = new StaticRegionSceneObject(region);
            //regionSceneObj.Scale = .7;
            //regionSceneObj.Left = (Global.Resolution.Width / 2) - (regionSceneObj.Width * regionSceneObj.Scale / 2);
            //regionSceneObj.Top = (Global.Resolution.Height / 2) - (regionSceneObj.Height * regionSceneObj.Scale / 2);
            mapLayer.AddObjectCenter(regionSceneObj);

            region.PositionVisual = new Dungeon.Types.Point(regionSceneObj.Left, regionSceneObj.Top);

            Global.Game.Region = region;

            mapLayer.AddObject(regionSceneObj);
            mapLayer.AddSystem(new TooltipSystem());
            mapLayer.AddObject(new PartySceneObject(Global.Game.Party));

            var ui = this.CreateLayer("ui");
            ui.AbsoluteLayer = true;
            ui.AddSystem(new TooltipSystem());
            ui.AddSystem(new TooltipCustomSystem());

            Global.Game.HeroPlate1 = new HeroPlate(null);
            Global.Game.HeroPlate2 = new HeroPlate(null);
            Global.Game.HeroPlate3 = new HeroPlate(null);
            Global.Game.HeroPlate4 = new HeroPlate(null);

            ui.AddObject(Global.Game.HeroPlate1);
            ui.AddObject(Global.Game.HeroPlate2);
            ui.AddObject(Global.Game.HeroPlate3);
            ui.AddObject(Global.Game.HeroPlate4);
            var clock = new GlobalClock(Global.Game.Calendar);
            ui.AddObjectCenter(clock);
            clock.Top = 0;
            ui.AddObject(new FoodCounter(Global.Game.Party.Food)
            {
                Left = Global.Resolution.Width - 255
            });
            var panel = new Panel();
            ui.AddObjectCenter(panel);
            panel.Top = Global.Resolution.Height - panel.Height;


            var overlay = this.CreateLayer("overlay");
            overlay.AddObject(Global.Helps);

            //ui.AddObject(new ImageObject("listtemplate.png"));

            //Global.AudioPlayer.Effect("Sounds/Ship.wav".AsmRes(), new Dungeon.Audio.AudioOptions()
            //{
            //    Repeat = true
            //});
            //Global.AudioPlayer.Music("Sounds/RainStorm.ogg".AsmRes());
        }

        protected override void KeyPress(Key keyPressed, KeyModifiers keyModifiers, bool hold)
        {
            if (keyPressed == Key.Escape)
                this.Switch<StartScene>();

            base.KeyPress(keyPressed, keyModifiers, hold);
        }
    }
}