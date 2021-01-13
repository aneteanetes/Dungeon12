namespace Dungeon.Scenes.Manager
{
    using Dungeon.Settings;
    using Dungeon.Types;
    using Dungeon.View.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class SceneManager
    {
        public string Uid { get; } = Guid.NewGuid().ToString();

        public override string ToString()
        {
            return this.GetType() + $" [{Uid}]";
        }

        private IDrawClient drawClient;
        public IDrawClient DrawClient
        {
            get => drawClient;
            set
            {
                DungeonGlobal.DrawClient = value;
                drawClient = value;
            }
        }

        private readonly Dictionary<Type, GameScene> SceneCache = new Dictionary<Type, GameScene>();

        public GameScene Current { get; set; }
        public GameScene Preapering = null;

        public GameScene CurrentScene { get; set; }

        public Type LoadingScreenType { get; set; }

        public void Start(params string[] args)
        {
            Type startSceneType = null;

            DungeonGlobal.Assemblies.FirstOrDefault(asm =>
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

            DungeonGlobal.GameAssemblyName = startSceneType.Assembly.GetName().Name;
            DungeonGlobal.GameAssembly = startSceneType.Assembly;

            DungeonGlobal.Assemblies.FirstOrDefault(asm =>
            {
                try
                {
                    var type = asm.GetTypes().FirstOrDefault(t => t?.BaseType?.Name?.Contains("LoadingScene") ?? false);
                    if (type == null)
                    {
                        return false;
                    }

                    if (!type.IsGenericType)
                    {
                        LoadingScreenType = type;
                        type.New();
                        return true;
                    }

                }
                catch { }
                return false;
            });

            if (startSceneType != null)
            {
                Change(startSceneType, args);
            }
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
                SceneCache.Remove(sceneType);
                existedScene.Destroy();
                if(existedScene.Loadable)
                {
                    LoadingScreen.Then(c => c.Dispose());
                }
            }
        }

        public void Switch<TScene>(params string[] args) where TScene : GameScene
        {
            if (Current?.Loadable ?? false)
                LoadingScreenCustom(Current.LoadArguments).Then(cb =>
                {
                    SwitchImplementation<TScene>(args);
                    cb.Dispose();
                });
            else
                SwitchImplementation<TScene>(args);
        }

        public Callback Loading => LoadingScreen;

        public Callback LoadingScreen
        {
            get
            {
                var loading = LoadingScreenType.NewAs<LoadingScene>();
                loading.Init();
                return DrawClient.SetScene(loading);
            }
        }

        public Callback LoadingScreenCustom(params object[] args)
        {
            var loading = LoadingScreenType.NewAs<LoadingScene>(2, args);
            loading.Init();
            return DrawClient.SetScene(loading);
        }

        private void SwitchImplementation<TScene>(string[] args) where TScene : GameScene
        {
            // вначале уничтожаем сцену, потому что если мы
            // хотим переключить на ту же самую сцену,
            // она будет в кэше и не будет создана т.к.
            // будет в состоянии "не удаляемой"
            if (Current?.Destroyable ?? false)
            {
                SceneCache.Remove(Current.GetType());
                Current.Destroy();
            }

            var sceneType = typeof(TScene);

            if (!SceneCache.TryGetValue(sceneType, out GameScene next))
            {
                next = sceneType.New<TScene>(this);
                SceneCache.Add(typeof(TScene), next);

                Populate(Current, next, args);
                Preapering = next;
                CurrentScene = Preapering;
                next.Init();
            }

            //Если мы переключаем сцену, а она в это время фризит мир - надо освободить мир
            if (Current?.Freezer != null)
            {
                DungeonGlobal.Freezer.World = null;
            }

            Preapering = next;
            CurrentScene = Preapering;
            Current = next;
            //Если мы переключаем сцену, а в следующей есть физер - значит надо восстановить её состояние
            if (next.Freezer != null)
            {
                DungeonGlobal.Freezer.World = next.Freezer;
            }

            Current.Activate();
            CurrentScene = Current;
        }

        public void Change<TScene>(params string[] args) where TScene : GameScene => Switch<TScene>(args);

        public void Change(Type sceneType, params string[] args)
        {
            if (Current?.Loadable ?? false)
                LoadingScreenCustom(Current.LoadArguments).Then(cb =>
                {
                    ChangeImplementation(sceneType, args);
                    cb.Dispose();
                });
            else
                ChangeImplementation(sceneType, args);
        }

        private void ChangeImplementation(Type sceneType, string[] args)
        {
            if (Current?.Destroyable ?? false)
            {
                SceneCache.Remove(Current.GetType());
            }

            if (!SceneCache.TryGetValue(sceneType, out GameScene next))
            {
                next = sceneType.NewAs<GameScene>(this);
                SceneCache.Add(sceneType, next);

                Populate(Current, next);
                Preapering = next;
                CurrentScene = Preapering;
                next.Args = args;
                next.Init();
                if (next is StartScene nextStartScene)
                {
                    if (nextStartScene.IsFatalException)
                    {
                        nextStartScene.FatalException();
                    }
                }
            }

            Preapering = next;
            CurrentScene = Preapering;
            Current = next;

            Current.Args = args;
            Current.Activate();
            CurrentScene = Current;
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