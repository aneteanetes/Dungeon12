using Dungeon;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon.Scenes;
using Dungeon.Scenes.Manager;
using Dungeon.Tiled;
using Dungeon12.Entities.MapRelated;
using SidusXII.Models.Map;
using SidusXII.SceneObjects.Main.Map;
using System.Linq;

namespace Dungeon12.Scenes
{
    public class MapScene : GameScene<StartScene>
    {
        public MapScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => false;

        public override bool AbsolutePositionScene => false;

        public override void Initialize()
        {
            var background = this.CreateLayer("background");
            background.AddObject(new ImageObject("Backgrounds/ship.png".AsmImg())
            {
                Width = Global.Resolution.Width,
                Height = Global.Resolution.Height
            });

            var map = this.CreateLayer("map");

            //var c = MapComponent.Load("Maps/ShipFromMainLandToFaithIsland.tmx");
            //c.Init();
            //var m = new MapSceneObject(c);
            //map.AddObject(m);

            var m = TiledMap.Load("Maps/ShipFromMainLandToFaithIsland.tmx".AsmRes());

            var area = m.Objects.FirstOrDefault(x => x.Properties.FirstOrDefault(p => p.name == "Area") != default);
            var obj = new EmptySceneObject()
            {
                Width = area.width,
                Height = area.height
            };

            m.Objects.ForEach(o =>
            {
                if (o.file == null)
                    return;

                obj.AddChild(new ImageObject(o.file.AsmRes())
                {
                    Width = o.width,
                    Height = o.height,
                    Left = o.x,
                    Top = o.y - o.height,
                });
            });

            obj.Scale = .5;

            obj.Left = (Global.Resolution.Width / 2) - ((obj.Width * .1) / 2);
            obj.Top = (Global.Resolution.Height / 2) - ((obj.Height * .1) / 2);
            map.AddObject(obj);


            //Global.AudioPlayer.Effect("Sounds/Ship.wav".AsmRes(), new Dungeon.Audio.AudioOptions()
            //{
            //    Repeat=true
            //});
            //Global.AudioPlayer.Music("Sounds/RainStorm.ogg".AsmRes());
        }
    }
}
