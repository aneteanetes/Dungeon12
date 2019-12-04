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

        public GameScene CurrentScene { get; set; }

        public void Start(params string[] args)
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

            if (startSceneType != null)
            {
                Change(startSceneType, args);
            }
        }

        /// <summary>
        /// Возможность явно уничтожать сцены, полезно для сцен которые не <see cref="Scene.Destroyable"/>
        /// </summary>
        /// <typeparam name="TScene"></typeparam>
        public static void Destroy<TScene>() where TScene : GameScene
        {
            var sceneType = typeof(TScene);

            if (SceneCache.TryGetValue(sceneType, out var existedScene))
            {
                SceneCache.Remove(sceneType);
                existedScene.Destroy();
            }
        }

        public static void Switch<TScene>(params string[] args) where TScene : GameScene
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
                next = sceneType.New<TScene>(Global.SceneManager);
                SceneCache.Add(typeof(TScene), next);

                Populate(Current, next, args);
                Preapering = next;
                Global.SceneManager.CurrentScene = Preapering;
                next.Init();
            }

            //Если мы переключаем сцену, а она в это время фризит мир - надо освободить мир
            if (Current.Freezer != null)
            {
                Global.Freezer.World = null;
            }

            Preapering = next;
            Global.SceneManager.CurrentScene = Preapering;
            Current = next;
            //Если мы переключаем сцену, а в следующей есть физер - значит надо восстановить её состояние
            if (next.Freezer != null)
            {
                Global.Freezer.World = next.Freezer;
            }

            Current.Activate();
            Global.SceneManager.CurrentScene = Current;
        }

        public void Change<TScene>(params string[] args) where TScene : GameScene => Switch<TScene>(args);

        public void Change(Type sceneType, params string[] args)
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

            next.PlayerAvatar = previous.PlayerAvatar;
            next.Log = previous.Log;
            next.Gamemap = previous.Gamemap;
            next.Args = args;
        }
    }
}