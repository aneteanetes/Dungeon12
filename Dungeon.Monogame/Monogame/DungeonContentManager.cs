using Dungeon.Resources;
using Dungeon.Scenes.Manager;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.IO;

namespace Dungeon.Monogame.Monogame
{
    internal class DungeonContentManager : ContentManager
    {
        public DungeonContentManager(IServiceProvider serviceProvider, SceneManager sceneManager) : base(serviceProvider)
        {
            this.sceneManager = sceneManager;
        }

        private SceneManager sceneManager;

        public override T Load<T>(string assetName)
        {
            return base.Load<T>(assetName);
        }

        public override T LoadLocalized<T>(string assetName)
        {
            return base.LoadLocalized<T>(assetName);
        }

        public override void UnloadAsset(string assetName)
        {
            base.UnloadAsset(assetName);
        }

        public override void Unload()
        {
            base.Unload();
        }

        public override void UnloadAssets(IList<string> assetNames)
        {
            base.UnloadAssets(assetNames);
        }

        protected override void ReloadAsset<T>(string originalAssetName, T currentAsset)
        {
            base.ReloadAsset(originalAssetName, currentAsset);
        }

        protected override Stream OpenStream(string assetName)
        {
            var res = ResourceLoader.Load(sceneManager.Current?.Resources,sceneManager.Current?.ResourceSceneObject, assetName);
            return res.Stream;
        }

        protected override void ReloadGraphicsAssets()
        {
            base.ReloadGraphicsAssets();
        }
    }
}