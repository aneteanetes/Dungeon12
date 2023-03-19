namespace Dungeon.Scenes.Manager
{
    using Dungeon.Settings;
    using Dungeon.Types;
    using Dungeon.View.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading;

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

        private readonly Dictionary<Type, GameScene> SceneCache = new Dictionary<Type, GameScene>();

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
        public GameScene Preapering = null;

        public GameScene CurrentScene { get; set; }

        public Type LoadingScreenType { get; set; }

        public bool IsSwitching { get; set; }

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
            if (_current?.Loadable ?? false)
                LoadingScreenCustom(_current.LoadArguments).Then(cb =>
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
                loading.Initialize();
                return GameClient.SetScene(loading);
            }
        }

        public Callback LoadingScreenCustom(params object[] args)
        {
            var loading = LoadingScreenType.NewAs<LoadingScene>(2, args);
            loading.Initialize();
            return GameClient.SetScene(loading);
        }

        private void SwitchImplementation<TScene>(string[] args) where TScene : GameScene
        {
            IsSwitching= true;

            // вначале уничтожаем сцену, потому что если мы
            // хотим переключить на ту же самую сцену,
            // она будет в кэше и не будет создана т.к.
            // будет в состоянии "не удаляемой"
            if (_current?.Destroyable ?? false)
            {
                SceneCache.Remove(_current.GetType());
                _current.Destroy();
            }

            var sceneType = typeof(TScene);

            if (!SceneCache.TryGetValue(sceneType, out GameScene next))
            {
                next = sceneType.New<TScene>(this);
                SceneCache.Add(typeof(TScene), next);

                Populate(_current, next, args);
                Preapering = next;
                CurrentScene = Preapering;
                next.Initialize();
            }

            //Если мы переключаем сцену, а она в это время фризит мир - надо освободить мир
            if (_current?.Freezer != null)
            {
                DungeonGlobal.Freezer.World = null;
            }

            // удаляем ссылки
            if (_current.Destroyable)
                _current.sceneManager=null;


            Preapering = next;
            CurrentScene = Preapering;

            _current=next;
            //Если мы переключаем сцену, а в следующей есть физер - значит надо восстановить её состояние
            if (next.Freezer != null)
            {
                DungeonGlobal.Freezer.World = next.Freezer;
            }

            _current.Activate();
            CurrentScene = _current;

            IsSwitching = false;
        }

        public void Change<TScene>(params string[] args) where TScene : GameScene => Switch<TScene>(args);

        public void Change(Type sceneType, params string[] args)
        {
            if (_current?.Loadable ?? false)
                LoadingScreenCustom(_current.LoadArguments).Then(cb =>
                {
                    ChangeImplementation(sceneType, args);
                    cb.Dispose();
                });
            else
                ChangeImplementation(sceneType, args);
        }

        private void ChangeImplementation(Type sceneType, string[] args)
        {
            IsSwitching=true;

            if (_current?.Destroyable ?? false)
            {
                SceneCache.Remove(_current.GetType());
            }

            if (!SceneCache.TryGetValue(sceneType, out GameScene next))
            {
                next = sceneType.NewAs<GameScene>(this);
                SceneCache.Add(sceneType, next);

                Populate(_current, next);
                Preapering = next;
                CurrentScene = Preapering;
                next.Args = args;
                ProcessArgs(next);
                next.Initialize();
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
            _current = next;

            _current.Args = args;
            _current.Activate();
            CurrentScene = _current;

            IsSwitching = false;
        }

        private void ProcessArgs(GameScene scene)
        {
            var arg = scene.Args?.ElementAtOrDefault(0);
            if (arg != default)
            {
                if (bool.TryParse(arg, out var isInGame))
                {
                    scene.InGame = isInGame;
                }
            }
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