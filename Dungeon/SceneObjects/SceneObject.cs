namespace Dungeon.SceneObjects
{
    using Dungeon;
    using Dungeon.Drawing;
    using Dungeon.Drawing.SceneObjects;
    using Dungeon.ECS;
    using Dungeon.GameObjects;
    using Dungeon.Proxy;
    using Dungeon.SceneObjects.Mixins;
    using Dungeon.Types;
    using Dungeon.Utils;
    using Dungeon.View;
    using Dungeon.View.Enums;
    using Dungeon.View.Interfaces;
    using MathNet.Numerics.Distributions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    [Hidden]
    public abstract class SceneObject<TComponent> : ISceneObject, IFlowable, IMixinContainer
        where TComponent : class
    {
        private readonly Scenes.GameScene owner;

        public virtual TComponent Component { get; private set; }

        /// <summary>
        /// В КОНСТРУКТОРЕ ЕСТЬ КОСТЫЛЬ
        /// </summary>
        public SceneObject(TComponent component, bool bindView = true)
        {
            if (this is IAutoFreeze)
            {
                if (this is IAutoUnfreeze unfreeze)
                    DungeonGlobal.Freezer.Freeze(unfreeze);
                else //not possible now
                    DungeonGlobal.Freezer.Freeze(this);
            }

            if (bindView && component != default)
            {
                BindGameComponent(component);
            }

            Component = component;

            ProcessSingleton();
        }

        public void BindGameComponent(TComponent component)
        {
            if (component is IGameComponent gameComponent)
            {
                gameComponent.SetView(this);
            }
        }

        public void BindComponent(TComponent component)
        {
            Component = component;
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
                    instance.Destroy();
                    singletonInstances.Remove(key);
                }

                singletonInstances.Add(key, this);
            }
        }

        public TextObject AddTextCenter(IDrawText drawText, bool horizontal = true, bool vertical = true, double parentWidth = 0)
            => AddTextCenter<TextObject>(drawText,horizontal,vertical,parentWidth);

        private class EmptyObj : SceneObject<GameComponentEmpty>
        {
            public EmptyObj() : base(new GameComponentEmpty())
            {
            }

            public override void Throw(Exception ex)
            {
                throw ex;
            }
        }

        public T AddTextCenter<T>(IDrawText drawText, bool horizontal = true, bool vertical = true, double parentWidth=0)
            where T : SceneObject<IDrawText>
        {
            var boundObj = parentWidth == 0
                ? this as ISceneObject
                : new EmptyObj() { Width = parentWidth, Height = this.Height };

            var textControl = typeof(T).New(drawText).As<T>();
            var measure = DungeonGlobal.GameClient.MeasureText(textControl.Text, parentWidth==0
                ? this
                : new EmptyObj() { Width = parentWidth });

            if (drawText.WordWrap)
            {
                textControl.Width = boundObj.Width;
                textControl.Height = boundObj.Height;
            }

            var width = Width;
            var height = Height;

            if (horizontal)
            {
                textControl.Left = width / 2 - measure.X / 2;
            }

            if (vertical)
            {
                textControl.Top = Math.Abs(height / 2 -  measure.Y / 2);
            }

            this.AddChild(textControl);

            return textControl;
        }

        public T CenterChild<T>(T child, bool x = true, bool y = true, Dot measure =default)
            where T : ISceneObject
        {
            var width = Width;
            var height = Height;

            if (measure == default)
                measure = new Dot(child.Width, child.Height);

            if (x)
            {
                child.Left = width / 2 - measure.X / 2;
            }

            if (y)
            {
                child.Top = Math.Abs(height / 2 -  measure.Y / 2);
            }

            return child;
        }

        public T CenterChildText<T>(T text, bool x = true, bool y = true)
            where T : SceneObject<IDrawText>
        {
            var measure = DungeonGlobal.GameClient.MeasureText(text.Text, this);
            return CenterChild(text, x, y, measure);
        }

        public TextObject AddText(IDrawText text, double left, double top)
        {
            var txt = new TextObject(text)
            {
                Left=left,
                Top=top
            };

            return this.AddChild(txt);
        }

        public TextObject AddTextPos(IDrawText text, double left, double top, double width, double height, bool alignCenter = true)
        {
            var textControl = new TextObject(text);

            textControl.Left = left;
            textControl.Top=top;
            textControl.Width = width;
            textControl.Height = height;

            var m = this.MeasureText(text);

            textControl.Left -= m.X / 2 - width / 2;
            textControl.Top -= m.Y / 2 - height / 2;

            return this.AddChild(textControl);
        }

        public T AddTextPos<T>(T textControl, double left, double top, double width, double height, bool alignCenter = true)
            where T : SceneObject<IDrawText>
        {
            textControl.Left = left;
            textControl.Top=top;
            textControl.Width = width;
            textControl.Height = height;

            var m = this.MeasureText(textControl.Text);

            textControl.Left -= m.X / 2 - width / 2;
            textControl.Top -= m.Y / 2 - height / 2;

            return this.AddChild(textControl);
        }

        public void CenterText(TextObject textControl, bool horizontal = true, bool vertical = true, double parentWidth = 0)
        {
            var measure = DungeonGlobal.GameClient.MeasureText(textControl.Text, parentWidth == 0
                ? this
                : new EmptyObj() { Width = parentWidth });

            var width = Width;
            var height = Height;

            if (horizontal)
            {
                textControl.Left = width / 2 - measure.X / 2;
            }

            if (vertical)
            {
                textControl.Top = height / 2 - measure.Y / 2;
            }
        }

        protected T AddChildImageCenter<T>(T control, bool horizontal = true, bool vertical = true)
            where T : ISceneObject
        {
            var measure = MeasureImage(control.Image);
            measure.X = measure.X * Settings.DrawingSize.CellF;
            measure.Y = measure.Y * Settings.DrawingSize.CellF;

            var width = Width * Settings.DrawingSize.CellF;
            var height = Height * Settings.DrawingSize.CellF;

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

        public virtual T AddChildCenter<T>(T control, bool horizontal = true, bool vertical = true)
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
                var textControl = new TextObject(drawText);
                Children.Add(textControl);
            }

            return this;
        }

        /// <summary>
        /// Возвращает в абсолютных координатах
        /// </summary>
        /// <param name="text"></param>
        /// <param name="parent">Только если wordWrap</param>
        /// <returns></returns>
        protected Dot MeasureText(IDrawText text, ISceneObject parent = default) => DungeonGlobal.GameClient.MeasureText(text, parent);

        protected DrawText CutText(DrawText text, double height)
        {
            var y = this.MeasureText(text, this).Y / Settings.DrawingSize.CellF;
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
        protected Dot MeasureImage(string img)
        {
            var m = DungeonGlobal.GameClient.MeasureImage(img);

            return new Dot(m.X / Settings.DrawingSize.CellF, m.Y / Settings.DrawingSize.CellF);
        }

        [Default(1)]
        public virtual double Opacity { get; set; } = 1;

        /// <summary>
        /// Установить размер объекта в соответствии с изображением (если оно есть)
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void AutoSizeImage(double width = 1, double height = 1)
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

        public double LeftMax => Width+Left;

        public double TopMax => Height+Top;

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

        private Square pos;

        /// <summary>
        /// Relative position
        /// </summary>
        public virtual Square BoundPosition
        {
            get
            {
                if (pos == null || !CachePosition)
                {
                    if (pos == null)
                    {
                        pos = new Square();
                    }
                    pos.X = (float)Left;
                    pos.Y = (float)Top;
                    pos.Width = (float)Width;
                    pos.Height = (float)Height;

                    if (Scale != default)
                    {
                        pos.X *= Scale;
                        pos.Y *= Scale;
                        pos.Width *= Scale;
                        pos.Height *= Scale;
                    }
                }

                return pos;
            }
        }

        public string Uid { get; } = Guid.NewGuid().ToString();

        public virtual IDrawText Text { get; protected set; }

        public virtual IDrawablePath Path { get; set; }

        public ICollection<ISceneObject> Children { get; } = new List<ISceneObject>();

        /// <summary>
        /// Здесь обрабатываются всевозможные события например пересчёт уровней
        /// </summary>
        protected virtual void AfterAddChild()
        {

        }

        public ISceneObject AddChild(ISceneObject sceneObject)
        {
            sceneObject.OnDestroy += () =>
            {
                RemoveChild(sceneObject);
                DestroyBinding?.Invoke(sceneObject);
            };

            OnDestroy += () => sceneObject.Destroy();

            sceneObject.Parent = this;

            Children.Add(sceneObject);
            return sceneObject;
        }

        public virtual TSceneObject AddChild<TSceneObject>(TSceneObject sceneObject)
            where TSceneObject : ISceneObject
        {
            sceneObject.OnDestroy += () =>
            {
                RemoveChild(sceneObject);
                DestroyBinding?.Invoke(sceneObject);
            };

            OnDestroy += () => sceneObject.Destroy();

            sceneObject.Parent = this;

            Children.Add(sceneObject);
            sceneObject.Parent = this;

            return sceneObject;
        }

        public void ClearChildrens()
        {
            var forRemove = new List<ISceneObject>(Children);

            foreach (var removing in forRemove)
            {
                removing.Destroy();
                RemoveChild(removing);
            }
        }

        public ISceneObject RemoveChild(ISceneObject sceneObject)
        {
            Children.Remove(sceneObject);
            return sceneObject;
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
                removing.Destroy();
                RemoveChild(removing);
            }
        }

        /// <summary>
        /// Обнуляет внутренние значения <see cref="ComputedPosition"/> и <see cref="BoundPosition"/>
        /// </summary>
        public void RecalculateComputedAndBounds()
        {
            this.Expired = true;

            foreach (var child in this.Children)
            {
                child.Call(nameof(RecalculateComputedAndBounds));
            }
        }

        private Square _computedPosition;
        public Square ComputedPosition
        {
            get
            {
                if (_computedPosition == default || !CachePosition)
                {
                    var parentX = Parent?.ComputedPosition.X ?? 0;
                    var parentY = Parent?.ComputedPosition.Y ?? 0;

                    var scale_ = this.GetScaleValue();

                    bool needScalePosition = false;

                    var parentScale = Parent?.Scale ?? 0;
                    if (parentScale != 0)
                    {
                        scale_ = parentScale;
                        needScalePosition = true;
                    }

                    if (scale_ == 0)
                        scale_ = 1;

                    if (_computedPosition != default)
                    {
                        _computedPosition.X = parentX + (float)Left * (needScalePosition ? scale_ : 1);
                        _computedPosition.Y = parentY + (float)Top * (needScalePosition ? scale_ : 1);
                    }
                    else
                        _computedPosition = new Square
                        {
                            X = parentX + (float)Left * (needScalePosition ? scale_ : 1),
                            Y = parentY + (float)Top * (needScalePosition ? scale_ : 1)
                        };
                }

                return _computedPosition;
            }
        }

        [Hidden]
        public ISceneObject Parent { get; set; }

        /// <summary>
        /// Default: False since 18.11.12
        /// </summary>
        public virtual bool CacheAvailable { get; set; } = false;

        public virtual bool CachePosition { get; set; } = false;

        public virtual bool IsBatch { get; set; } = false;

        public virtual bool Expired { get; set; }

        /// <summary>
        /// Абсолютная позиция НЕ НАСЛЕДУЕТСЯ в целях ПРОИЗВОДИТЕЛЬНОСТИ
        /// </summary>
        public virtual bool AbsolutePosition { get; set; } = false;

        [Hidden]
        public Action<List<ISceneObject>> ShowInScene { get; set; }

        [Hidden]
        public Action<ISceneObject> DestroyBinding { get; set; }

        [Hidden]
        public Action<ISceneControl> ControlBinding { get; set; }

        public virtual Square CropPosition => new Square
        {
            X = BoundPosition.X,
            Y = BoundPosition.Y,
            Height = Children.Max(c => c.BoundPosition.Y + c.BoundPosition.Height),
            Width = Children.Max(c => c.BoundPosition.X + c.BoundPosition.Width)
        };

        public virtual int LayerLevel { get; set; }

        [Hidden]        
        public bool ForceInvisible { get; set; }

        private bool visible = true;

        [Default(true)]
        public virtual bool Visible
        {
            get => (VisibleFunction == null ? visible : VisibleFunction()) && (Parent?.Visible ?? true);
            set => visible = value;
        }

        public Func<bool> VisibleFunction { get; set; }

        public virtual int ZIndex { get; set; } = 0;

        /// <summary>
        /// Значит что инстанс такого класса должен быть один, если создаётся новый то автоматом удаляется старый
        /// </summary>
        public virtual bool Singleton { get; set; } = false;

        public virtual bool DrawOutOfSight { get; set; } = true;

        public virtual bool Shadow { get; set; }

        public virtual ILight Light { get; set; }

        public List<IEffectParticle> ParticleEffects { get; set; } = new List<IEffectParticle>();

        public virtual bool Interface { get; set; }

        public virtual IImageMask ImageMask => Mask;

        public virtual ImageMask Mask { get; set; }

        public virtual bool Blur { get; set; }

        public virtual bool Filtered { get; set; } = true;

        private double _scale;

        public double GetScaleValue() => _scale;

        public virtual double Scale
        {
            get
            {
                if (_scale != default)
                    return _scale;

                if (Parent != default)
                {
                    return Parent.Scale;
                }

                return default;
            }
            set
            {
                _scale = value;
            }
        }

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
            return $"{this.GetType().Name}.{Parent?.ToString()} :base {base.ToString()}";
        }

        public bool IntersectsWith(ISceneObject another)
        {
            var xsum1 = Math.Max(ComputedPosition.X, another.ComputedPosition.X);
            var xsum2 = Math.Min(ComputedPosition.X + Width, another.ComputedPosition.X + another.BoundPosition.Width);
            var ysum1 = Math.Max(ComputedPosition.Y, another.ComputedPosition.Y);
            var ysum2 = Math.Min(ComputedPosition.Y + Height, another.ComputedPosition.Y + another.BoundPosition.Height);

            if (xsum2 >= xsum1 && ysum2 >= ysum1)
            {
                return true;
            }

            return false;
        }

        [Hidden]
        public Action<SceneObject<TComponent>> AfterUpdate { get; set; }

        /// <summary>
        /// Хинт для фронта
        /// </summary>
        public bool ScaleAndResize { get; set; }

        public virtual bool Updatable => true;

        public bool Drawed { get; set; }

        public string Tag { get; set; }

        public ISceneLayer Layer { get; set; }

        [Title("Рисовать видимую часть")]
        public bool DrawPartInSight { get; set; }

        public ITileMap TileMap { get; set; }

        public bool AlphaBlend { get; set; }

        public IDrawColor Color { get; set; }

        public FlipStrategy Flip { get; set; } = FlipStrategy.None;

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

        public void AddParticleEffects(params ISceneObject[] effects)
        {
            effects.ForEach(x => this.AddChild(x));
        }

        public ITexture Texture { get; set; }

        public virtual bool PerPixelCollision { get; set; }

        public bool HighLevelComponent { get; set; }

        protected Animation animation;

        protected bool InAnimation { get; set; } = false;
        TimeSpan animationTime;
        TimeSpan frameTime;
        TimeSpan elapsed;
        int frameCount = 0;

        private string _image;
        /// <summary>
        /// Теперь AsmImg() проставляется автоматом!
        /// </summary>
        public virtual string Image
        {
            get => _image;
            set
            {
                _image = value;
                if (!_image.Contains(".Resources.Images."))
                {
                    _image = Assembly.GetCallingAssembly().GetName().Name + ".Resources.Images." + _image.Embedded();
                }
            }

        }
        private Square _originalImageRegion;
        private Square _imageRegion;

        public virtual Square ImageRegion { get; set; } = new Square();

        public bool AutoBindSceneObjectSizeByContainedImage { get; set; } = true;

        public virtual bool IsMonochrome { get; set; }

        public virtual DrawMode Mode { get; set; } = DrawMode.Normal;

        public string Name { get; set; }

        public void PlayAnimation(Animation animation)
        {
            if (InAnimation && animation.Name == this.animation.Name)
                return;

            InAnimation = true;
            this.animation = animation;
            this.animationTime = animation.Time == default ? TimeSpan.FromSeconds(1) : animation.Time;
            this.frameTime = this.animationTime / animation.Frames.Count;
            this.frameCount = 0;
        }

        public void StopAnimation()
        {
            if (!InAnimation)
                return;

            InAnimation = false;

            this.animationTime = TimeSpan.Zero;
            this.frameTime = TimeSpan.Zero;

            if (this.animation.Loop)
            {
                PlayAnimation(this.animation);
            }
            else
                this.animation = null;
        }

        protected virtual void AnimationFrameChange() { }

        public void ComponentUpdateChainCall(GameTimeLoop gameTime)
        {
            Update(gameTime);

            elapsed += gameTime.ElapsedGameTime;

            if (InAnimation)
            {
                if (elapsed >= frameTime)
                {
                    elapsed = TimeSpan.Zero;
                    var frame = animation.Frames[frameCount];
                    this.ImageRegion=ImageRegion.SetCoords(frame.X, frame.Y);

                    frameCount++;
                    AnimationFrameChange();

                    if (frameCount == animation.Frames.Count)
                    {
                        StopAnimation();
                    }
                }
            }
            AfterUpdate?.Invoke(this);
        }

        public virtual void Update(GameTimeLoop gameTime) { }

        public Action OnDestroy { get; set; }

        public bool IsDestoryed { get; private set; }
        public double AngleDegree
        {
            get
            {
                return (180d / Math.PI) * Angle;
            }
            set
            {
                Angle = (Math.PI / 180d) * value;
            }
        }

        public List<IECSComponent> Components { get; set; } = new List<IECSComponent>();

        public virtual void Destroy()
        {
            Component = default;
            if (Component is GameComponent gameComponent)
                gameComponent.SetView(default);

            singletonInstances.Remove(GetType().FullName);
            OnDestroy?.Invoke();
            OnDestroy = null;

            ClearDelegates();
            IsDestoryed=true;
        }

        private void ClearDelegates()
        {
            this.GetType()
                .GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.PropertyType.BaseType!=null && p.PropertyType.BaseType.Name=="MulticastDelegate")
                .ForEach(p => this.SetPropertyExprType(p.Name, null, p.PropertyType));
        }

        public virtual void Init() { }

        public virtual void Drawing() { }

        public virtual void Throw(Exception ex) { }
    }
}