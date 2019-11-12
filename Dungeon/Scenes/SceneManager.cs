namespace Dungeon.Scenes.Manager
{
    using Dungeon.Settings;
    using Dungeon.View.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class SceneManager
    {
        public static IDrawClient StaticDrawClient { get; set; }

        public IDrawClient DrawClient
        {
            get => StaticDrawClient;
            set
            {
                Global.DrawClient = value;
                StaticDrawClient = value;
            }
        }

        private static readonly Dictionary<Type, GameScene> SceneCache = new Dictionary<Type, GameScene>();

        public static GameScene Current = null;
        public static GameScene Preapering = null;

        public void Start()
        {
            Type startSceneType = null;

            Global.Assemblies.FirstOrDefault(asm =>
            {
                var type = asm.GetTypes().FirstOrDefault(t => t?.BaseType?.Name?.Contains("StartScene") ?? false);
                if (type == null)
                {
                    return false;
                }

                if (!type.IsGenericType)
                {
                    startSceneType = type;
                    return true;
                }

                return false;
            });

            Global.GameAssemblyName = startSceneType.Assembly.GetName().Name;
            Global.GameAssembly = startSceneType.Assembly;

            if (startSceneType!=null)
            {
                Change(startSceneType);
            }
        }

        public void Change<TScene>(params string[] args) where TScene : GameScene
        {
            var sceneType = typeof(TScene);
            if (!SceneCache.TryGetValue(sceneType, out GameScene next))
            {
                next = sceneType.New<TScene>(this);
                SceneCache.Add(typeof(TScene), next);

                Populate(Current, next, args);
                Preapering = next;
                next.Init();
            }

            if (Current?.Destroyable ?? false)
            {
                SceneCache.Remove(Current.GetType());
                Current.Destroy();
            }

            //Если мы переключаем сцену, а она в это время фризит мир - надо освободить мир
            if (Current.Freezer != null)
            {
                Global.Freezer.World = null;
            }

            Preapering = next;
            Current = next;
            //Если мы переключаем сцену, а в следующей есть физер - значит надо восстановить её состояние
            if (next.Freezer != null)
            {
                Global.Freezer.World = next.Freezer;
            }

            Current.Activate();
        }

        public void Change(Type sceneType)
        {
            if (!SceneCache.TryGetValue(sceneType, out GameScene next))
            {
                next = sceneType.NewAs<GameScene>(this);
                SceneCache.Add(sceneType, next);

                Populate(Current, next);
                Preapering = next;
                next.Init();
            }
            
            if (Current?.Destroyable ?? false)
            {
                SceneCache.Remove(Current.GetType());
            }

            Preapering = next;
            Current = next;
            
            Current.Activate();
        }

        private void Populate(GameScene previous, GameScene next, string[] args = default)
        {
            if (previous == null)
                return;

            next.PlayerAvatar = previous.PlayerAvatar;
            next.Log = previous.Log;
            next.Gamemap = previous.Gamemap;
            next.Args = args;
        }
    }
}