namespace Dungeon.Scenes
{
    using Dungeon.Audio;
    using Dungeon.Control;
    using Dungeon.Control.Gamepad;
    using Dungeon.Control.Keys;
    using Dungeon.Control.Pointer;
    using Dungeon.ECS;
    using Dungeon.Localization;
    using Dungeon.Resources;
    using Dungeon.Scenes.Manager;
    using Dungeon.Types;
    using Dungeon.View.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public abstract class Scene : IScene
    {
        public SceneManager sceneManager;

        public abstract bool Destroyable { get; }

        public IAudioPlayer AudioPlayer=>DungeonGlobal.AudioPlayer;

        public LocalizationStringDictionary Strings => DungeonGlobal.GetBindedGlobal().GetStringsClass();

        public string Uid { get; } = Guid.NewGuid().ToString();

        public override string ToString()
        {
            return base.ToString() + $" [{Uid}]";
        }

        SceneLayerGraph SceneLayerGraph;

        public Scene(SceneManager sceneManager)
        {
            this.sceneManager = sceneManager;
            SceneLayerGraph = new SceneLayerGraph()
            {
                Name = "<== SCENE LAYER ==>",
                Size = new Physics.PhysicalSize()
                {
                    Width = DungeonGlobal.Sizes.Width,
                    Height = DungeonGlobal.Sizes.Height,
                },
                Position = new Physics.PhysicalPosition(0, 0)
            };
        }

        public virtual bool AbsolutePositionScene => true;

        private SceneLayer _activeLayer;
        private bool loggedallinactivedlayers = false;
#warning ActiveLayer - ошибка (может и нет, но блядь, работа с ним - ошибка)
        public SceneLayer ActiveLayer
        {
            get
            {
                if (_activeLayer == default && !loggedallinactivedlayers)
                {
                    loggedallinactivedlayers = true;
                    DungeonGlobal.Logger.Log($"All layers on {this.GetType()}-scene is inactive!");
                }
                return _activeLayer;
            }
            set
            {
                if (_activeLayer == default)
                    DungeonGlobal.Logger.Log($"All layers on {this.GetType()}-scene now setted as inactive!");
                _activeLayer = value;
            }
        }

        public SceneLayer CreateLayer(string name) => CreateLayer<SceneLayer>(name);

        public SceneLayer AddLayer(string name) => CreateLayer(name);

        public TSceneLayer CreateLayer<TSceneLayer>(string name)
            where TSceneLayer : SceneLayer
        {
            var newLayer = typeof(TSceneLayer).New(this).As<TSceneLayer>();
            newLayer.Name = name;
            newLayer.Width = DungeonGlobal.Resolution.Width;
            newLayer.Height = DungeonGlobal.Resolution.Height;
            LayerList.Add(newLayer);
            LayerMap.Add(name, () => LayerList.IndexOf(newLayer));

            SceneLayerGraph.Add(new SceneLayerGraph(newLayer));
            ActiveLayer = newLayer;

            return newLayer;
        }

        public SceneLayer RemoveLayer(string name)
        {
            var layer = GetLayer(name).As<SceneLayer>();
            layer.Destroyed = true;
            LayerMap.Remove(name);
            LayerList.Remove(layer);
            layer.Destroy();
            return layer;
        }

        public ISceneLayer GetLayer(string name) => LayerList[LayerMap[name]()];

        public void ResetActiveLayer() => ActiveLayer = null;

        protected void CallLayer(Action<SceneLayer> layerAction, PointerArgs mouse = default)
        {
            if (mouse != default && ActiveLayer != default)
            {
                layerAction?.Invoke(ActiveLayer);
            }
            else if (ActiveLayer == default && mouse != default)
            {
                var layers = SceneLayerGraph.QueryPhysical(new SceneLayerGraph(mouse));
                foreach (var layer in layers)
                {
                    var sceneLayer = layer.SceneLayer;
                    if (!sceneLayer.Destroyed)
                        layerAction?.Invoke(sceneLayer);
                }
            }
            else if (mouse == default)
            {
                foreach (var layer in LayerList)
                {
                    if (!layer.Destroyed)
                        layerAction?.Invoke(layer);
                }
            }
        }

        public void OnText(string text)
        {
            if (Destroyed)
                return;

            CallLayer(l => l.OnText(text));
        }

        public void OnKeyDown(KeyArgs keyEventArgs)
        {
            var key = keyEventArgs.Key;
            var modifier = keyEventArgs.Modifiers;

            if (DungeonGlobal.Freezer.World == null && !DungeonGlobal.BlockSceneControls)
                try
                {
                    KeyPress(key, modifier, keyEventArgs.Hold);
                }
                catch (Exception ex)
                {
                    DungeonGlobal.Exception(ex);
                    return;
                }

            CallLayer(l => l.OnKeyDown(keyEventArgs));
        }

        public void OnStickMoveOnce(Direction direction, GamePadStick stick)
        {
            if (Destroyed)
                return;

            if (DungeonGlobal.Freezer.World == null && !DungeonGlobal.BlockSceneControls)
                try
                {
                    StickMoveOnce(direction, stick);
                }
                catch (Exception ex)
                {
                    DungeonGlobal.Exception(ex);
                    return;
                }

            CallLayer(l => l.OnStickMoveOnce(direction, stick));
        }

        public void OnGamePadButtons(GamePadButton[] btns, bool pressed)
        {
            if (Destroyed)
                return;

            if (DungeonGlobal.Freezer.World == null && !DungeonGlobal.BlockSceneControls)
                try
                {
                    if (pressed)
                    {
                        GamePadButtonPress(btns);
                        CallLayer(l => l.OnGamePadButtonsPress(btns));
                    }
                    else
                    {
                        GamePadButtonRelease(btns);
                        CallLayer(l => l.OnGamePadButtonsRelease(btns));
                    }
                }
                catch (Exception ex)
                {
                    DungeonGlobal.Exception(ex);
                    return;
                }
        }

        public void OnStickMove(Direction direction, GamePadStick stick)
        {
            if (Destroyed)
                return;

            if (DungeonGlobal.Freezer.World == null && !DungeonGlobal.BlockSceneControls)
                try
                {
                    StickMove(direction, stick);
                }
                catch (Exception ex)
                {
                    DungeonGlobal.Exception(ex);
                    return;
                }

            CallLayer(l => l.OnStickMove(direction, stick));
        }

        public void OnKeyUp(KeyArgs keyEventArgs)
        {
            if (Destroyed)
                return;

            var key = keyEventArgs.Key;
            var modifier = keyEventArgs.Modifiers;

            if (DungeonGlobal.Freezer.World == null && !DungeonGlobal.BlockSceneControls)
                try
                {
                    KeyUp(key, modifier);
                }
                catch (Exception ex)
                {
                    DungeonGlobal.Exception(ex);
                    return;
                }

            CallLayer(l => l.OnKeyUp(keyEventArgs));
        }

        public void OnMousePress(PointerArgs pointerPressedEventArgs, Dot offset)
        {
            if (Destroyed)
                return;

            if (DungeonGlobal.Freezer.World == null && !DungeonGlobal.BlockSceneControls)
                try
                {
                    MousePress(pointerPressedEventArgs);
                }
                catch (Exception ex)
                {
                    DungeonGlobal.Exception(ex);
                    return;
                }

            CallLayer(l => l.OnMousePress(pointerPressedEventArgs, offset), pointerPressedEventArgs);
        }

        public void OnMouseRelease(PointerArgs pointerPressedEventArgs, Dot offset)
        {
            if (Destroyed)
                return;

            if (DungeonGlobal.Freezer.World == null && !DungeonGlobal.BlockSceneControls)
                try
                {
                    MouseRelease(pointerPressedEventArgs);
                }
                catch (Exception ex)
                {
                    DungeonGlobal.Exception(ex);
                    return;
                }

            CallLayer(l => l.OnMouseRelease(pointerPressedEventArgs, offset), pointerPressedEventArgs);
        }

        public void OnMouseWheel(MouseWheelEnum wheelEnum)
        {
            if (Destroyed)
                return;

            if (DungeonGlobal.Freezer.World == null && !DungeonGlobal.BlockSceneControls)
                try
                {
                    MouseWheel(wheelEnum);
                }
                catch (Exception ex)
                {
                    DungeonGlobal.Exception(ex);
                    return;
                }

            CallLayer(l => l.OnMouseWheel(wheelEnum));
        }

        public void OnMouseMove(PointerArgs pointerPressedEventArgs, Dot offset)
        {
            if (sceneManager.Current != this)
                return;

            if (DungeonGlobal.Freezer.World == null && !DungeonGlobal.BlockSceneControls)
                try
                {
                    MouseMove(pointerPressedEventArgs);
                }
                catch (Exception ex)
                {
                    DungeonGlobal.Exception(ex);
                    return;
                }

            CallLayer(l => l.OnMouseMove(pointerPressedEventArgs, offset), pointerPressedEventArgs);
        }

        protected virtual void KeyPress(Key keyPressed, KeyModifiers keyModifiers, bool hold) { }

        protected virtual void MousePress(PointerArgs pointerArgs) { }

        protected virtual void MouseRelease(PointerArgs pointerArgs) { }

        protected virtual void StickMoveOnce(Direction direction, GamePadStick stick) { }

        protected virtual void GamePadButtonPress(GamePadButton[] btns) { }

        protected virtual void GamePadButtonRelease(GamePadButton[] btns) { }

        protected virtual void StickMove(Direction direction, GamePadStick stick) { }

        protected virtual void MouseWheel(MouseWheelEnum mouseWheel) { }

        protected virtual void MouseMove(PointerArgs pointerArgs) { }

        protected virtual void KeyUp(Key keyPressed, KeyModifiers keyModifiers) { }

        public abstract void Initialize();

        private ResourceTable _resources=new ResourceTable();
        public ResourceTable Resources => _resources;

        /// <summary>
        /// Активирует сцену на следующем цикле отрисовки
        /// </summary>
        /// <returns></returns>
        public virtual Task Activate()
        {
            return this.sceneManager.GameClient.ChangeScene(this);
        }

        public virtual void Switch<T>(params string[] args) where T : GameScene
        {
            this.sceneManager.Switch<T>(args);
        }

        public bool Destroyed { get; private set; } = false;

        public Dictionary<string, Func<int>> LayerMap = new Dictionary<string, Func<int>>();
        private readonly List<SceneLayer> LayerList = new List<SceneLayer>();

        public ISceneLayer[] Layers => LayerList.ToArray();

        public List<ISystem> Systems { get; set; } = new List<ISystem>();

        public bool IsLoaded { get; set; }

        public bool IsInitialized { get; set; }

        public IEnumerable<ISystem> GetSystems() => Systems;

        public void AddSystem(ISystem system)
        {
            if (!Systems.Contains(system))
            {
                Systems.Add(system);
            }
        }

        public TSystem GetSystem<TSystem>() where TSystem : ISystem
            => Systems.FirstOrDefault(s => s.Is<TSystem>()).As<TSystem>();

        public virtual void Destroy()
        {
            foreach (var l in LayerList)
            {
                l.Destroy();
            }

            LayerList.Clear();
            LayerMap = null;
            SceneLayerGraph = null;

            if (!Dungeon.Resources.ResourceLoader.Settings.IsEmbeddedMode && !Dungeon.Resources.ResourceLoader.Settings.NotDisposingResources)
            {
                Resources?.Dispose();
                _resources = null;
            }
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);

            Destroyed = true;
        }

        public virtual void Loaded() { }

        public virtual void Load() { }

        public virtual void Unload() { }

        public Resource GetResource(string name)
        {
            if (Resources.TryGetValue(name, out var res))
                return res;

            if (DungeonGlobal.Resources.TryGetValue(name, out res))
                return res;

            throw new KeyNotFoundException($"Ресурс {name} не загружен на сцену!");
        }

        public virtual void Update(GameTimeLoop gameTimeLoop)
        {
            this.ActiveLayer.Update(gameTimeLoop);
        }
    }
}