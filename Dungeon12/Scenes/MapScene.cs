using Dungeon;
using Dungeon.Control.Keys;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon.Scenes;
using Dungeon.Scenes.Manager;
using Dungeon.Tiled;
using Dungeon12.Entities.Map;
using Dungeon12.SceneObjects.Map;
using System.Linq;

namespace Dungeon12.Scenes
{
    public class MapScene : GameScene<StartScene>
    {
        public MapScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => true;

        public override bool AbsolutePositionScene => false;

        private HintScenarioSceneObject hints = new HintScenarioSceneObject();

        public override void Initialize()
        {
            var background = this.CreateLayer("background");
            background.AddObject(new ImageObject("Backgrounds/ship.png".AsmImg())
            {
                Width = Global.Resolution.Width,
                Height = Global.Resolution.Height
            });

            var mapLayer = this.CreateLayer("map");

            var region = Region.Load("ShipFaithIsland");
            var regionSceneObj = new RegionSceneObject(region);
            regionSceneObj.Scale = .7;
            regionSceneObj.Left = (Global.Resolution.Width / 2) - (regionSceneObj.Width * regionSceneObj.Scale / 2);
            regionSceneObj.Top = (Global.Resolution.Height / 2) - (regionSceneObj.Height * regionSceneObj.Scale / 2);

            mapLayer.AddObject(regionSceneObj);

            var ui = this.CreateLayer("ui");
            ui.AbsoluteLayer = true;
            ui.AddObject(hints);
            

            //Global.AudioPlayer.Effect("Sounds/Ship.wav".AsmRes(), new Dungeon.Audio.AudioOptions()
            //{
            //    Repeat=true
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