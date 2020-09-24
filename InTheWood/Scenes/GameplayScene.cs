using Dungeon;
using Dungeon.Control.Keys;
using Dungeon.Drawing.SceneObjects;
using Dungeon.Scenes;
using Dungeon.Scenes.Manager;
using Force.DeepCloner;
using InTheWood.Entities.MapScreen;
using InTheWood.SceneObjects.MapObjects;

namespace InTheWood.Scenes
{
    public class GameplayScene : StartScene
    {        
        public override bool Destroyable => true;

        public GameplayScene(SceneManager sceneManager) : base(sceneManager) { }

        public override void Init()
        {
            var map = new Map();
            var centerSector = new Sector();
            map.AddSector(centerSector);

            var leftSector = new Sector();
            leftSector.Status = MapStatus.Friendly;
            map.AddSector(leftSector, new SectorConnection(centerSector, leftSector)
            {
                ConnectDirection = Dungeon.Types.SimpleDirection.Left,
                Position = 2,
                Offset = 1
            });

            var rightSector = new Sector();
            rightSector.Status = MapStatus.Hostile;
            map.AddSector(rightSector, new SectorConnection(centerSector, rightSector)
            {
                ConnectDirection = Dungeon.Types.SimpleDirection.Right,
                Position = 2,
                Offset = 1
            });

            var mapObj = new MapSceneObject(map)
            {
                Left = 450,
                Top = 100,
            };
            mapObj.Scale = .5;

            this.AddObject(mapObj);
        }

        protected override void KeyPress(Key keyPressed, KeyModifiers keyModifiers, bool hold)
        {
            if (keyPressed == Key.Escape)
            {
                Global.Exit();
            }

            base.KeyPress(keyPressed, keyModifiers, hold);
        }
    }
}