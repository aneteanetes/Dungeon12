namespace Rogue.Drawing.SceneObjects
{
    using Rogue.Types;
    using Rogue.View.Interfaces;
    using System;
    using System.Collections.Generic;

    public abstract class SceneControl : ISceneObject
    {
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
        public Rectangle Position
        {
            get
            {
                if (pos == null || !CacheAvailable)
                {
                    pos =new Rectangle()
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
            if(sceneObject is SceneControl sceneControlObject)
            {
                sceneControlObject.Parent = this;
            }

            this.Children.Add(sceneObject);
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

        public virtual bool CacheAvailable => true;

        public virtual bool IsBatch => false;

        public virtual bool Expired => false;
    }
}
