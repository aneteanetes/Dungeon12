namespace Dungeon.SceneObjects
{
    using Dungeon.Control;
    using Dungeon.Control.Gamepad;
    using Dungeon.Control.Keys;
    using Dungeon.Control.Pointer;
    using Dungeon.Proxy;
    using Dungeon.Scenes.Manager;
    using Dungeon.Types;
    using Dungeon.Utils;
    using Dungeon.View.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Tracing;
    using System.Linq;

    public interface IHandleSceneControl : IMixinContainer
    {
        void AddDynamicEvent(string eventName, Delegate method);

        void AddHandle(ControlEventType controlEventType);

        void RemoveHandle(ControlEventType controlEventType);
    }

    [Hidden]
    public abstract class SceneControl<T> : SceneObject<T>, ISceneControl, IHandleSceneControl
        where T : class
    {
        public SceneControl(T component, bool bindView = true) : base(component, bindView)
        {
            //dynamic binding
            new string[] {
                nameof(KeyDown),
                nameof(KeyUp),
                nameof(Focus),
                nameof(Unfocus),
                nameof(Click),
                nameof(ClickRelease),
                nameof(MouseMove),
                nameof(GlobalMouseMove),
                nameof(MouseWheel),
                nameof(TextInput),
                nameof(GlobalClick),
                nameof(GlobalClickRelease),
            }.ForEach(s => dynamicEvents.Add(s, null));
        }

        public void AddDynamicEvent(string eventName, Delegate method)
        {
            if (dynamicEvents.ContainsKey(eventName))
            {
                dynamicEvents[eventName] = Delegate.Combine(dynamicEvents[eventName], method);
            }
        }

        public void RemoveDynamicEvent(string eventName, Delegate method)
        {
            if (dynamicEvents.ContainsKey(eventName))
            {
                dynamicEvents[eventName] = Delegate.Remove(dynamicEvents[eventName], method);
            }
        }

        private Dictionary<string, Delegate> dynamicEvents = new Dictionary<string, Delegate>();

        private readonly ControlEventType[] allHandling = new ControlEventType[]
        {
             ControlEventType.Click,
             ControlEventType.Focus,
             ControlEventType.Key
        };

        public bool MousePerPixel { get; set; }

        public virtual string CursorOld { get; set; } = null;

        protected virtual ControlEventType[] Handles { get; } = null;

        protected virtual Key[] KeyHandles { get; set; } = null;

        private Lazy<ControlEventType[]> ControlHandlers => new Lazy<ControlEventType[]>(() => Handles);

        private HashSet<ControlEventType> dynamicHandles = new HashSet<ControlEventType>();

        public void AddHandle(ControlEventType controlEventType)
        {
            dynamicHandles.Add(controlEventType);
        }

        public void RemoveHandle(ControlEventType controlEventType)
        {
            dynamicHandles.Remove(controlEventType);
        }

        private Lazy<Key[]> KeyHandlers => new Lazy<Key[]>(() => KeyHandles);

        public ControlEventType[] CanHandle => (ControlHandlers.Value ?? allHandling).Concat(dynamicHandles).ToArray();

        public Key[] KeysHandle => KeyHandlers.Value ?? new Key[0];

        public virtual bool AllKeysHandle => false;

        public virtual void Click(PointerArgs args) => dynamicEvents[nameof(Click)]?.DynamicInvoke(args);

        public virtual void GlobalClick(PointerArgs args) => dynamicEvents[nameof(GlobalClick)]?.DynamicInvoke(args);

        /// <summary>
        /// Если есть изображение то мерит по нему, иначе по размеру контрола
        /// </summary>
        /// <typeparam name="TSceneObject"></typeparam>
        /// <param name="control"></param>
        /// <param name="horizontal"></param>
        /// <param name="vertical"></param>
        /// <param name="forceNotImage">принудительно мерять по контролу а не изображению</param>
        /// <returns></returns>
        public TSceneObject AddControlCenter<TSceneObject>(TSceneObject control, bool horizontal = true, bool vertical = true, bool forceNotImage=false)
            where TSceneObject : ISceneControl
        {
            ControlBinding?.Invoke(control);

            Dot measure = string.IsNullOrWhiteSpace(control.Image) && !forceNotImage
                ? new Dot(control.Width, control.Height)
                : new Dot(MeasureImage(control.Image).X * Settings.DrawingSize.CellF, MeasureImage(control.Image).Y * Settings.DrawingSize.CellF);

            double width = this.Width;
            double height = this.Height;

            if (!string.IsNullOrWhiteSpace(control.Image) && !forceNotImage)
            {
                width = Width * Settings.DrawingSize.CellF;
                height = Height * Settings.DrawingSize.CellF;
            }

            if (horizontal)
            {
                var left = width / 2 - measure.X / 2;
                control.Left = left / Settings.DrawingSize.CellF;
            }

            if (vertical)
            {
                var top = height / 2 - measure.Y / 2;
                control.Top = top / Settings.DrawingSize.CellF;
            }

            AddChild(control);

            return control;
        }

        public bool IsInFocus { get; private set; }

        public virtual bool HideCursor { get; set; }

        public bool Controlable { get; set; }

        public virtual void Focus()
        {
            IsInFocus = true;
            if (CursorOld != null)
            {
                DungeonGlobal.GameClient.SetCursor(("Cursors." + CursorOld + ".png").PathImage());
            }
            if (HideCursor)
            {
                DungeonGlobal.GameClient.SetCursor("1px.png".AsmImg());
            }
            dynamicEvents[nameof(Focus)]?.DynamicInvoke();
        }

        public virtual void Unfocus()
        {
            IsInFocus = false;
            if (CursorOld != null || HideCursor)
            {
                DungeonGlobal.GameClient.SetCursor("Cursors.common.png".PathImage());
            }
            dynamicEvents[nameof(Unfocus)]?.DynamicInvoke();
        }

        public virtual void KeyDown(Key key, KeyModifiers modifier, bool hold) => dynamicEvents[nameof(KeyDown)]?.DynamicInvoke(key,modifier,hold);

        public virtual void KeyUp(Key key, KeyModifiers modifier) => dynamicEvents[nameof(KeyUp)]?.DynamicInvoke(key, modifier);

        public virtual void ClickRelease(PointerArgs args) => dynamicEvents[nameof(ClickRelease)]?.DynamicInvoke(args);

        public virtual void GlobalClickRelease(PointerArgs args) => dynamicEvents[nameof(GlobalClickRelease)]?.DynamicInvoke(args);
        
        public virtual void MouseWheel(MouseWheelEnum mouseWheelEnum) => dynamicEvents[nameof(MouseWheel)]?.DynamicInvoke(mouseWheelEnum);

        public virtual void MouseMove(PointerArgs args) => dynamicEvents[nameof(MouseMove)]?.DynamicInvoke(args);

        public virtual void GlobalMouseMove(PointerArgs args) => dynamicEvents[nameof(GlobalMouseMove)]?.DynamicInvoke(args);

        public virtual void TextInput(string text) => dynamicEvents[nameof(TextInput)]?.DynamicInvoke(text);

        //public virtual void AddChild(ISceneControl sceneObject)
        //{
        //    ControlBinding?.Invoke(sceneObject);
        //    base.AddChild(sceneObject);
        //}

        /// <summary>
        /// Добавляет компонент как миксин
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mixin"></param>
        public override void AddMixin<TMixin>(TMixin mixin)
        {
            mixin.InitAsMixin(this);
            Mixins.Add(mixin);
            if (mixin is ISceneControl mixinControl)
            {
                this.AddChild(mixinControl);
            }
        }

        public virtual void StickMoveOnce(Direction direction, GamePadStick stick) { }

        public virtual void StickMove(Direction direction, GamePadStick stick) { }

        public virtual void GamePadButtonsPress(GamePadButton[] btns) { }

        public virtual void GamePadButtonsRelease(GamePadButton[] btns) { }

        public override TSceneObject AddChild<TSceneObject>(TSceneObject sceneObject)
        {
            if (sceneObject is ISceneControl sceneControl)
                AddControl(sceneControl);

            return base.AddChild(sceneObject);
        }

        public override T1 AddChildCenter<T1>(T1 control, bool horizontal = true, bool vertical = true)
        {
            if(control is ISceneControl sceneControl)
                AddControl(sceneControl);

            return base.AddChildCenter(control, horizontal, vertical);
        }

        public virtual void AddControl<T1>(T1 control) where T1 : ISceneControl
        {
            if (this.Layer != default)
            {
                this.Layer.AddExistedControl(control);
            }
            else
            {
                var activeLayer = DungeonGlobal.SceneManager?.Current?.ActiveLayer;
                if(activeLayer!=default)
                    activeLayer.AddExistedControl(control);
            }
        }
    }
}