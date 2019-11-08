namespace Dungeon.SceneObjects
{
    using Dungeon;
    using Dungeon.Drawing.Impl;
    using Dungeon.Drawing.SceneObjects;
    using Dungeon.Drawing.SceneObjects.UI;
    using Dungeon.GameObjects;
    using Dungeon.Proxy;
    using Dungeon.SceneObjects.Mixins;
    using Dungeon.Scenes.Manager;
    using Dungeon.Settings;
    using Dungeon.Types;
    using Dungeon.View.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    
    public abstract class SceneObject<TComponent> : GameComponent, ISceneObject, IFlowable, IMixinContainer
        where TComponent : IGameComponent
    {
        private readonly Scenes.GameScene owner;

        public TComponent Component { get; private set; }

        /// <summary>
        /// В КОНСТРУКТОРЕ ЕСТЬ КОСТЫЛЬ
        /// </summary>
        public SceneObject(TComponent component, bool bindView=true)
        {
            if (bindView && component != default)
            {
                component.SetView(this);
                Component = component;

                this.Destroy += () =>
                {
                    Component = default;
                    component.SetView(default);
                };
            }

            // ЭТО ПИЗДЕЦ КОСТЫЛЬ
            owner = SceneManager.Preapering;

            if (owner != null)
            {
                // ВСЕ ЭТИ МЕТОДЫ ПУБЛИЧНЫЕ ТОЛЬКО ПОТОМУ ЧТО НУЖНЫ ЗДЕСЬ
                ControlBinding += owner.AddControl;
                DestroyBinding += owner.RemoveObject;
                Destroy += () => owner.RemoveObject(this);
                ShowEffects += owner.ShowEffectsBinding;

                //ПИЗДЕЦ. Это надо лечить
                if (this is DraggableControl draggableControl)
                { }
                else
                {
                    ZIndex = DragAndDropSceneControls.DraggableLayers;
                }
            }

            Global.Events.Subscribe(@event =>
            {
                this.Dispatch((so, arg) => so.OnEvent(arg), @event);
            });

            //ХВАТИТ ОРАТЬ В КОММЕНТАРИЯХ
            // пожалуйста :)

            ProcessSingleton();
        }

        private static Dictionary<string, ISceneObject> singletonInstances = new Dictionary<string, ISceneObject>();

        /// <summary>
        /// омфг, это замена компонента T на новый компонент T с удалением предыдущего
        /// </summary>
        private void ProcessSingleton()
        {
            if (Singleton)
            {
                var key = GetType().FullName;
                if (singletonInstances.TryGetValue(key, out var instance))
                {
                    instance.Destroy?.Invoke();
                    singletonInstances.Remove(key);
                }

                singletonInstances.Add(key, this);
                Destroy += () => singletonInstances.Remove(key);
            }
        }

        protected TextControl AddTextCenter(IDrawText drawText, bool horizontal = true, bool vertical = true)
        {
            var textControl = new TextControl(drawText);

            var measure = Global.DrawClient.MeasureText(textControl.Text);

            var width = Width * 32;
            var height = Height * 32;

            if (horizontal)
            {
                var left = width / 2 - measure.X / 2;
                textControl.Left = left / 32;
            }

            if (vertical)
            {
                var top = height / 2 - measure.Y / 2;
                textControl.Top = top / 32;
            }

            Children.Add(textControl);

            return textControl;
        }

        protected T AddChildImageCenter<T>(T control, bool horizontal = true, bool vertical = true)
            where T : ISceneObject
        {
            var measure = MeasureImage(control.Image);
            measure.X = measure.X * 32;
            measure.Y = measure.Y * 32;

            var width = Width * 32;
            var height = Height * 32;

            if (horizontal)
            {
                var left = width / 2 - measure.X / 2;
                control.Left = left / 32;
            }

            if (vertical)
            {
                var top = height / 2 - measure.Y / 2;
                control.Top = top / 32;
            }
            AddChild(control);

            return control;
        }

        protected T AddChildCenter<T>(T control, bool horizontal = true, bool vertical = true)
            where T : ISceneObject
        {
            if (horizontal)
            {
                var left = Width / 2d - control.Width / 2d;
                control.Left = left;
            }

            if (vertical)
            {
                var top = Height / 2d - control.Height / 2d;
                control.Top = top;
            }
            AddChild(control);

            return control;
        }

        public SceneObject<TComponent> WithText(IDrawText drawText, bool center = false)
        {
            if (center)
            {
                AddTextCenter(drawText);
            }
            else
            {
                var textControl = new TextControl(drawText);
                Children.Add(textControl);
            }

            return this;
        }

        /// <summary>
        /// Возвращает в абсолютных координатах
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        protected Point MeasureText(IDrawText text, ISceneObject parent = default) => Global.DrawClient.MeasureText(text, parent);

        /// <summary>
        /// измеряет изображение и возвращает уже в формате координат
        /// </summary>
        /// <param name="text"></param>
        /// <returns>relative X/Y</returns>
        protected Point MeasureImage(string img)
        {
            var m = Global.DrawClient.MeasureImage(img);

            return new Point(m.X / 32, m.Y / 32);
        }

        public virtual double Opacity { get; set; } = 1;

        /// <summary>
        /// Relative
        /// </summary>
        public virtual double Left { get; set; }

        /// <summary>
        /// Relative
        /// </summary>
        public virtual double Top { get; set; }

        /// <summary>
        /// Relative
        /// </summary>
        public virtual double Width { get; set; }

        /// <summary>
        /// Relative
        /// </summary>
        public virtual double Height { get; set; }

        /// <summary>
        /// Угол на который надо повернуть объект при отображении
        /// </summary>
        public virtual double Angle { get; set; }

        private Rectangle pos = null;

        /// <summary>
        /// Relative position
        /// </summary>
        public virtual Rectangle Position
        {
            get
            {
                if (pos == null || !CacheAvailable)
                {
                    pos = new Rectangle()
                    {
                        X = (float)Left,
                        Y = (float)Top,
                        Width = (float)Width,
                        Height = (float)Height
                    };
                }

                return pos;
            }
        }

        public string Uid { get; } = Guid.NewGuid().ToString();

        public virtual string Image { get; set; }

        public virtual Rectangle ImageRegion { get; set; }

        public virtual IDrawText Text { get; protected set; }

        public virtual IDrawablePath Path { get; }

        public ICollection<ISceneObject> Children { get; } = new List<ISceneObject>();

        public void AddChild(ISceneObject sceneObject)
        {
            sceneObject.Destroy += () =>
            {
                RemoveChild(sceneObject);
                DestroyBinding(sceneObject);
            };

            Destroy += () => sceneObject.Destroy?.Invoke();

            if (sceneObject is ISceneObject sceneControlObject)
            {
                sceneControlObject.Parent = this;
            }

            Children.Add(sceneObject);
        }

        public void ClearChildrens()
        {
            var forRemove = new List<ISceneObject>(Children);

            foreach (var removing in forRemove)
            {
                removing.Destroy?.Invoke();
                RemoveChild(removing);
            }
        }

        public void RemoveChild(ISceneObject sceneObject)
        {
            Children.Remove(sceneObject);
        }

        public void RemoveChild<T>()
        {
            var forRemove = new List<ISceneObject>();

            Children.Where(x => x.GetType().IsAssignableFrom(typeof(T)) || x.GetType() == typeof(T))
                .ForEach(x =>
                {
                    forRemove.Add(x);
                });

            foreach (var removing in forRemove)
            {
                removing.Destroy?.Invoke();
                RemoveChild(removing);
            }
        }

        private Rectangle _computedPosition;
        public Rectangle ComputedPosition
        {
            get
            {
                if (_computedPosition == null || !CacheAvailable)
                {
                    var parentX = Parent?.ComputedPosition?.X ?? 0f;
                    var parentY = Parent?.ComputedPosition?.Y ?? 0f;

                    _computedPosition = new Rectangle
                    {
                        X = parentX + (float)Left,
                        Y = parentY + (float)Top
                    };
                }

                return _computedPosition;
            }
        }

        public ISceneObject Parent { get; set; }

        public virtual bool CacheAvailable { get; set; } = true;

        public virtual bool IsBatch => false;

        public virtual bool Expired => false;

        /// <summary>
        /// Абсолютная позиция НЕ НАСЛЕДУЕТСЯ в целях ПРОИЗВОДИТЕЛЬНОСТИ
        /// </summary>
        public virtual bool AbsolutePosition { get; set; } = false;

        public Action Destroy { get; set; }

        public Action<List<ISceneObject>> ShowEffects { get; set; }

        public Action<ISceneObject> DestroyBinding { get; set; }

        public Action<ISceneObjectControl> ControlBinding { get; set; }

        public virtual Rectangle CropPosition => new Rectangle
        {
            X = Position.X,
            Y = Position.Y,
            Height = Children.Max(c => c.Position.Y + c.Position.Height),
            Width = Children.Max(c => c.Position.X + c.Position.Width)
        };

        public virtual int Layer { get; set; }

        public bool ForceInvisible { get; set; }

        private bool visible = true;

        public virtual bool Visible
        {
            get => visible && (Parent?.Visible ?? true);
            set => visible = value;
        }

        public virtual int ZIndex { get; set; } = 0;

        /// <summary>
        /// Значит что инстанс такого класса должен быть один, если создаётся новый то автоматом удаляется старый
        /// </summary>
        public virtual bool Singleton { get; set; } = false;

        public virtual bool DrawOutOfSight { get; set; } = true;

        public virtual bool Shadow { get; set; }

        public virtual ILight Light { get; set; }

        public List<IEffect> Effects { get; set; } = new List<IEffect>();

        public virtual bool Interface { get; set; }

        public virtual IImageMask ImageMask => Mask;

        public virtual ImageMask Mask { get; set; }

        public virtual bool Blur { get; set; }

        public virtual bool Filtered { get; set; } = true;

        public override string ToString()
        {
            return $"{owner.GetType().Name}#{Uid} : {base.ToString()}";
        }

        public bool IntersectsWith(ISceneObject another)
        {
            var xsum1 = Math.Max(ComputedPosition.X, another.ComputedPosition.X);
            var xsum2 = Math.Min(ComputedPosition.X + Width, another.ComputedPosition.X + another.Position.Width);
            var ysum1 = Math.Max(ComputedPosition.Y, another.ComputedPosition.Y);
            var ysum2 = Math.Min(ComputedPosition.Y + Height, another.ComputedPosition.Y + another.Position.Height);

            if (xsum2 >= xsum1 && ysum2 >= ysum1)
            {
                return true;
            }

            return false;
        }

        public virtual void Update() { }

        public virtual void OnEvent(object @object)
        {
            CallOnEvent(@object as dynamic);
        }

        /// <summary>
        /// Method must call this.Discard(obj); for runtime dynamic binding 
        /// </summary>
        /// <param name="obj"></param>
        protected virtual void CallOnEvent(dynamic obj) => OnEvent(obj);

        private object flowContext = null;

        public T GetFlowProperty<T>(string property, T @default = default) => flowContext.GetProperty<T>(property);

        public bool SetFlowProperty<T>(string property, T value)
        {
            try
            {
                flowContext.SetProperty(property, value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void SetFlowContext(object context) => flowContext = context;

        public object GetFlowContext() => flowContext;


        private IFlowable flowparent = null;

        public void SetParentFlow(IFlowable parent) => flowparent = parent;

        public IFlowable GetParentFlow() => flowparent;

        private Dictionary<string, object> MixinContainer = new Dictionary<string, object>();

        public void SetMixinValue<T>(string property, T value)
        {
            if (!MixinContainer.ContainsKey(property))
            {
                MixinContainer.Add(property, null);
            }

            MixinContainer[property] = value;
        }

        public T GetMixinValue<T>(string property)
        {
            if (!MixinContainer.ContainsKey(property))
            {
                MixinContainer.Add(property, null);
            }

            return MixinContainer[property].As<T>();
        }

        protected readonly List<object> Mixins = new List<object>();

        /// <summary>
        /// Добавляет компонент как миксин
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mixin"></param>
        public virtual void AddMixin<T>(T mixin) where T : IMixin
        {
            mixin.InitAsMixin(this);
            Mixins.Add(mixin);
            this.AddChild(mixin);
        }

        public T Mixin<T>() where T : IMixin
        {
            var mix = Mixins.FirstOrDefault(m => m is T);
            if (mix != default)
            {
                return (T)mix;
            }

            return default;
        }
    }
}