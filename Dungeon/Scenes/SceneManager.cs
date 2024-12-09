namespace Dungeon.Scenes.Manager
{
    using Dungeon.Settings;
    using Dungeon.Types;
    using Dungeon.View.Interfaces;
    using Geranium.Reflection;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using static System.Net.Mime.MediaTypeNames;

    public class SceneManager
    {
        public SceneManager(IGameClient gameClient)
        {
            DungeonGlobal.GameClient = this.GameClient = gameClient;
        }

        public string Uid { get; } = Guid.NewGuid().ToString();

        public override string ToString()
        {
            return this.GetType() + $" [{Uid}]";
        }

        public IGameClient GameClient { get; }

        private readonly ConcurrentDictionary<Type, GameScene> SceneCache = new ConcurrentDictionary<Type, GameScene>();

        private GameScene _current;

        public GameScene Current
        {
            get
            {
                return IsSwitching 
                    ? null 
                    : _current;
            }
        }

        public bool IsSwitching { get; set; }

        public void Start(params string[] args)
        {
            var startScene = DungeonGlobal.GameAssembly.GetTypes().FirstOrDefault(x =>  x.Is<GameScene>() && x.IsAttributeExists<EntrySceneAttribute>());

            if (startScene == null)
                throw new InvalidOperationException("Невозможно запустить игру без стартовой сцены!");

            Switch(startScene, args);
        }

        /// <summary>
        /// Возможность явно уничтожать сцены, полезно для сцен которые не <see cref="Scene.Destroyable"/>
        /// </summary>
        /// <typeparam name="TScene"></typeparam>
        public void Destroy<TScene>() where TScene : GameScene
        {
            var sceneType = typeof(TScene);

            if (SceneCache.TryGetValue(sceneType, out var existedScene))
            {
                SceneCache.TryRemove(sceneType, out var scene);
                scene.Destroy();
            }
        }

        public void Switch<TScene>(params string[] args) where TScene : GameScene
            => Switch(typeof(TScene), args);

        public void Switch(Type sceneType, params string[] args)
        {
            IsSwitching = true;

            var loadingScreenType = GetLoadingGameScene(sceneType);
            if (loadingScreenType != null)
            {
                LoadingScreen loadingScreen = null;

                // если есть загрузочный экран, запускаем его
                if (loadingScreenType.Is<LoadingScreen>())
                {
                    loadingScreen = InstantiateLoadingScreen(loadingScreenType, () =>
                    {
                        Loading(sceneType);
                        SceneCache.TryRemove(loadingScreenType, out var loadscreen);
                        loadscreen.Destroy();
                    }).As<LoadingScreen>();
                }
            }
            else
            {
                Loading(sceneType);
            }
        }

        /// <summary>
        /// 
        /// <para>
        /// [Кэшируемый]
        /// </para>
        /// </summary>
        public Type GetLoadingGameScene(Type type)
        {
            if (!___IsLoadingGameSceneCache.TryGetValue(type, out var value))
            {
                value = GetLoadingGameSceneImplimentation(type);
                ___IsLoadingGameSceneCache.AddOrUpdate(type, value, (x, y) => value);
            }

            return value;
        }
        private static ConcurrentDictionary<Type, Type> ___IsLoadingGameSceneCache = new();

        private Type GetLoadingGameSceneImplimentation(Type sceneType)
        {
            if (sceneType == null)
                return null;

            if (sceneType.IsGenericType)
            {
                var loadingScreenType = sceneType.GetGenericArguments()[0];
                if (loadingScreenType.Is<LoadingScreen>())
                    return loadingScreenType;

                return GetLoadingGameSceneImplimentation(sceneType.BaseType);
            }
            else
            {
                return GetLoadingGameSceneImplimentation(sceneType.BaseType);
            }
        }

        private void Loading(Type sceneType)
        {
            var loadingTask = new Task(() => { });

            // уничтожаем сцену, потому что если переключаем на ту же самую сцену,
            // она будет в кэше и не будет пересоздана (станет не удаляемая)
            if (_current?.Destroyable ?? false)
            {
                SceneCache.TryRemove(_current.GetType(), out var scene);

                loadingTask = new Task(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    scene.Destroy();
                });
            }

            loadingTask.ContinueWith(t =>
            {
                var next = InstantiateScene(sceneType);

                //Если мы переключаем сцену, а что-то на ней в это время фризит мир - надо освободить мир
                if (_current?.Freezer != null)
                {
                    DungeonGlobal.Freezer.World = null;
                }

                // удаляем ссылки
                if (_current?.Destroyable ?? false)
                    _current.sceneManager = null;

                _current = next;
                //Если мы переключаем сцену, а в следующей есть фризер - значит надо восстановить её состояние
                if (next.Freezer != null)
                {
                    DungeonGlobal.Freezer.World = next.Freezer;
                }

                IsSwitching = false;
                _current.Loaded();

                _current.Activate();
            });

            loadingTask.Start();
        }

        private GameScene InstantiateScene(Type sceneType, params string[] args)
        {
            if (!SceneCache.TryGetValue(sceneType, out GameScene scene))
            {
                scene = sceneType.New(this).As<GameScene>();
                SceneCache.TryAdd(sceneType, scene);

                Populate(_current, scene, args);

                scene.Load();
                scene.IsLoaded = true;
                scene.Loaded();

                scene.Initialize();
                scene.IsInitialized = true;
            }

            return scene;
        }

        private GameScene InstantiateLoadingScreen(Type sceneType, Action onLoadComplete)
        {
            if (!SceneCache.TryGetValue(sceneType, out GameScene scene))
            {
                scene = sceneType.New(this, onLoadComplete).As<GameScene>();
                SceneCache.TryAdd(sceneType, scene);

                scene.Load();
                scene.IsLoaded = true;
                scene.Loaded();

                scene.Initialize();
                scene.IsInitialized = true;
            }

            scene.Activate();

            return scene;
        }

        private static void Populate(GameScene previous, GameScene next, string[] args = default)
        {
            if (previous == null)
                return;

            next.Log = previous.Log;
            next.Args = args;
        }
    }
}