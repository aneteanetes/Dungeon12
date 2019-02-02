namespace Rogue.Scenes.Controls
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
        public double Left { get; set; }

        /// <summary>
        /// Relative
        /// </summary>
        public double Top { get; set; }

        /// <summary>
        /// Relative
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// Relative
        /// </summary>
        public double Height { get; set; }

        private Rectangle pos = null;

        /// <summary>
        /// Relative position
        /// </summary>
        public Rectangle Position
        {
            get
            {
                if (pos == null)
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
    }
}
