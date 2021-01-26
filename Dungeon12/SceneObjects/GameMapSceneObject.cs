using Dungeon.Control;
using Dungeon.Drawing.SceneObjects;
using Dungeon12.Drawing.SceneObjects.Map;
using Dungeon12.Map;
using Dungeon12.Map.Objects;
using Dungeon12.SceneObjects; using Dungeon.SceneObjects;
using Dungeon.Types;
using Dungeon.View.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Dungeon;

namespace Dungeon12.Drawing.Labirinth
{
    public class GameMapSceneObject : Dungeon12.SceneObjects.SceneControl<GameMap>
    {
        public override bool IsBatch => true;

        private GameMap gamemap;

        public Action<List<ISceneObject>> drawpath;

        public override bool Expired => gamemap.ReloadCache;

        public GameMapSceneObject(GameMap location, PlayerSceneObject avatar):base(location)
        {
            Height = 0.1;
            Width = 0.1; 
            location.PublishObject = PublishMapObject;
            gamemap = location;
        }

        public Action<List<ISceneObject>, List<ISceneObject>> OnReload;
        
        public void Init()
        {
            var newSceneObjects = new List<ISceneObject>();
            newSceneObjects.AddRange(AddAsSceneObjects(gamemap.Objects));

            OnReload(this.currentAdditionalObjects, newSceneObjects);
            currentAdditionalObjects = newSceneObjects;
        }

        private List<ISceneObject> currentAdditionalObjects = new List<ISceneObject>();
        
        private IEnumerable<ISceneObject> AddAsSceneObjects(HashSet<MapObject> mapObjects)
        {
            List<ISceneObject> sceneObjects = new List<ISceneObject>();

            foreach (var obj in mapObjects.Where(x=>!(x is Obstruct)))
            {
                var view = obj.Visual();
                if (view != null)
                {
                    sceneObjects.Add(view);
                }
            }

            return sceneObjects;
        }

        private void PublishMapObject(MapObject mapObject)
        {
            ISceneObject sceneObject = mapObject.Visual();

            this.ShowInScene?.Invoke(sceneObject.InList());
        }
        
        protected override ControlEventType[] Handles => new ControlEventType[0];        
    }    
}