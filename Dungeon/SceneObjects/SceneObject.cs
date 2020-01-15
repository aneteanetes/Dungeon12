namespace Dungeon.SceneObjects
{
    using Dungeon;
    using Dungeon.Drawing;
    using Dungeon.Drawing.SceneObjects;
    using Dungeon.GameObjects;
    using Dungeon.Proxy;
    using Dungeon.SceneObjects.Mixins;
    using Dungeon.Scenes.Manager;
    using Dungeon.Types;
    using Dungeon.View.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class SceneObject<TComponent> : GameComponent, ISceneObject, IFlowable, IMixinContainer
        where TComponent : class, IGameComponent
    {
        private readonly Scenes.GameScene owner;
        
        public virtual TComponent Component { get; private set; }

        /// <summary>
        /// В КОНСТРУКТОРЕ ЕСТЬ КОСТЫЛЬ
        /// </summary>
        public SceneObject(TComponent component, bool bindView = true)
        {
            if (bindView && component != default)
            {
                BindComponent(component);
            }
            else if (component != default)
            {
                this.Destroy += () =>
                {
                    Component = default;
                };
            }

            Component = component;

            // ЭТО ПИЗДЕЦ КОСТЫЛЬ
            owner = SceneManager.Preapering;

            if (owner != null)
            {
                // ВСЕ ЭТИ МЕТОДЫ ПУБЛИЧНЫЕ ТОЛЬКО ПОТОМУ ЧТО НУЖНЫ ЗДЕСЬ
                ControlBinding += owner.AddControl;
                DestroyBinding += owner.RemoveObject;
                Destroy += () =>
                {
                    owner.RemoveObject(this);
                    this.UnsubscribeEvents();
                };
                ShowInScene += owner.ShowEffectsBinding;
            }

            ProcessSingleton();
        }

        public void BindComponent(TComponent component)
        {
            component.SetView(this);

            this.Destroy += () =>
            {
                Component = default;
                component.SetView(default);
            };
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
            var measure = DungeonGlobal.DrawClient.MeasureText(textControl.Text,this);

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

        protected virtual T AddChildCenter<T>(T control, bool horizontal = true, bool vertical = true)
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
        protected Point MeasureText(IDrawText text, ISceneObject parent = default) => DungeonGlobal.DrawClient.MeasureText(text, parent);

        protected DrawText CutText(DrawText text, double height)
        {
            var y = this.MeasureText(text, this).Y / 32;
            if (y > height)
            {
                text.SetText(text.StringData.Remove(text.StringData.LastIndexOf(" ")));
                return CutText(text, height);
            }
            else
            {
                return text;
            }
        }

        /// <summary>
        /// измеряет изображение и возвращает уже в формате координат
        /// </summary>
        /// <param name="text"></param>
        /// <returns>relative X/Y</returns>
        protected Point MeasureImage(string img)
        {
            var m = DungeonGlobal.DrawClient.MeasureImage(img);

            return new Point(m.X / 32, m.Y / 32);
        }

        public virtual double Opacity { get; set; } = 1;

        /// <summary>
        /// Установить размер объекта в соответствии с изображением (если оно есть)
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void AutoSizeImage(double width=1,double height=1)
        {
            if (this.Image != default)
            {
                var m = MeasureImage(this.Image);
                width = m.X;
                height = m.Y;
            }

            this.Width = width;
            this.Height = height;
        }

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

        public virtual Rectangle ImageRegion { get; set; }

        public virtual IDrawText Text { get; protected set; }

        public virtual IDrawablePath Path { get; }
        
        public ICollection<ISceneObject> Children { get; } = new List<ISceneObject>();

        /// <summary>
        /// Здесь обрабатываются всевозможные события например пересчёт уровней
        /// </summary>
        protected virtual void AfterAddChild()
        {

        }

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

        public IEnumerable<T> GetChildren<T>() => Children.Where(x => x is T).Select(x => (T)x);

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

        public Action<List<ISceneObject>> ShowInScene { get; set; }

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

        public virtual double Scale { get; set; }


        public SceneObject<TComponent> ScaleTo(double value)
        {
            foreach (var child in Children)
            {
                child.Top *= value;
                child.Left *= value;
                child.Scale = value;
            }
            Scale = value;
            return this;
        }    

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

        public Action<SceneObject<TComponent>> OnUpdate { get; set; }

        /// <summary>
        /// Хинт для фронта
        /// </summary>
        public bool ScaleAndResize { get; set; }

        public virtual void Update()
        {
            OnUpdate?.Invoke(this);
        }

        public virtual bool Updatable=>false;

        public bool Drawed { get; set; }

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

        public void AddEffects(params ISceneObject[] effects)
        {
            effects.ForEach(this.AddChild);
        }
    }
}