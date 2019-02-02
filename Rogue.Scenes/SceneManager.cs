﻿namespace Rogue.Scenes
{
    using Rogue.Scenes.Scenes;
    using Rogue.View.Interfaces;
    using System;
    using System.Collections.Generic;

    public class SceneManager
    {
        public IDrawClient DrawClient { get; set; }
        
        private static readonly Dictionary<Type, GameScene> SceneCache = new Dictionary<Type, GameScene>();

        public GameScene Current = null;

        public void Change<TScene>() where TScene : GameScene
        {
            var sceneType = typeof(TScene);
            if (!SceneCache.TryGetValue(sceneType, out GameScene next))
            {
                next = sceneType.New<TScene>(this);
                SceneCache.Add(typeof(TScene), next);

                Populate(Current, next);
                next.Init();
            }
            
            if (Current?.Destroyable ?? false)
            {
                SceneCache.Remove(Current.GetType());
            }

            Current = next;
            
            Current.Activate();
        }

        private void Populate(GameScene previous, GameScene next)
        {
            if (previous == null)
                return;

            next.Player = previous.Player;
            next.Log = previous.Log;
            next.Location = previous.Location;
        }
    }
}