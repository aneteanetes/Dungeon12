namespace Rogue.Drawing.SceneObjects
{
    using Rogue.Drawing.Impl;
    using Rogue.Scenes.Manager;
    using Rogue.Settings;
    using Rogue.Types;
    using Rogue.View.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class SceneObject : ISceneObject
    {
        private Rogue.Scenes.GameScene owner;

        /// <summary>
        /// В КОНСТРУКТОРЕ ЕСТЬ КОСТЫЛЬ
        /// </summary>
        public SceneObject()
        {
            // ЭТО ПИЗДЕЦ КОСТЫЛЬ
            owner = SceneManager.Preapering;

            if (owner != null)
            {
                // ВСЕ ЭТИ МЕТОДЫ ПУБЛИЧНЫЕ ТОЛЬКО ПОТОМУ ЧТО НУЖНЫ ЗДЕСЬ
                ControlBinding += owner.AddControl;
                DestroyBinding += owner.RemoveObject;
                Destroy = () => owner.RemoveObject(this);
                ShowEffects += owner.ShowEffectsBinding;
            }

            //ХВАТИТ ОРАТЬ В КОММЕНТАРИЯХ

            ProcessSingleton();
        }

        private static Dictionary<string, ISceneObject> singletonInstances = new Dictionary<string, ISceneObject>();

        private void ProcessSingleton()
        {
            if (this.Singleton)
            {
                var key = this.GetType().FullName;
                if (singletonInstances.TryGetValue(key, out var instance))
                {
                    instance.Destroy?.Invoke();
                    singletonInstances.Remove(key);
                }

                singletonInstances.Add(key, this);
                this.Destroy += () => singletonInstances.Remove(key);
            }
        }

        protected TextControl AddTextCenter(IDrawText drawText, bool horizontal = true, bool vertical = true)
        {
            var textControl = new TextControl(drawText);

            var measure = Global.DrawClient.MeasureText(textControl.Text);

            var width = this.Width * 32;
            var height = this.Height * 32;

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

            this.Children.Add(textControl);

            return textControl;
        }

        protected Point MeasureText(IDrawText text) => Global.DrawClient.MeasureText(text);

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

        protected void AddChild(ISceneObject sceneObject)
        {
            sceneObject.Destroy += () =>
            {
                this.RemoveChild(sceneObject);
                DestroyBinding(sceneObject);
            };

            this.Destroy += () => sceneObject.Destroy?.Invoke();

            if (sceneObject is SceneObject sceneControlObject)
            {
                sceneControlObject.Parent = this;
            }

            this.Children.Add(sceneObject);
        }

        protected void RemoveChild(ISceneObject sceneObject)
        {
            this.Children.Remove(sceneObject);
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
            X = this.Position.X,
            Y = this.Position.Y,
            Height = this.Children.Max(c => c.Position.Y + c.Position.Height),
            Width = this.Children.Max(c => c.Position.X + c.Position.Width)
        };

        public virtual int Layer { get; set; }

        public bool ForceInvisible { get; set; }

        public virtual bool Visible { get; set; } = true;

        public virtual int ZIndex { get; set; } = 0;

        public virtual bool Singleton { get; set; } = false;

        public virtual bool DrawOutOfSight { get; set; } = true;

        public override string ToString()
        {
            return $"{owner.GetType().Name}#{Uid} : {base.ToString()}";
        }

        public bool IntersectsWith(ISceneObject another)
        {
            var xsum1 = Math.Max(this.ComputedPosition.X, another.ComputedPosition.X);
            var xsum2 = Math.Min(this.ComputedPosition.X + this.Width, another.ComputedPosition.X + another.Position.Width);
            var ysum1 = Math.Max(this.ComputedPosition.Y, another.ComputedPosition.Y);
            var ysum2 = Math.Min(this.ComputedPosition.Y + this.Height, another.ComputedPosition.Y + another.Position.Height);

            if (xsum2 >= xsum1 && ysum2 >= ysum1)
            {
                return true;
            }

            return false;
        }
    }
}