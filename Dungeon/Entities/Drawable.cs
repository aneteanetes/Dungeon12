using Dungeon.Types;
using Dungeon.View.Interfaces;
using System;

namespace Dungeon.Entities
{
    /// <summary>
    /// Отрисовываемый
    /// </summary>
    public class Drawable : IDrawable
    {
        public string Icon { get; set; }
        private string _image;
        public string Image { get => _image ?? Icon; set => _image = value; }
        public string Name { get; set; }
        public IDrawColor BackgroundColor { get; set; }
        public IDrawColor ForegroundColor { get; set; }

        public virtual string Tileset { get; set; }

        public virtual Rectangle TileSetRegion { get; set; }

        public virtual Rectangle Region { get; set; }

        public virtual bool Container => false;

        public string Uid { get; } = Guid.NewGuid().ToString();
        
        /// <summary>
        /// Возвращает свойство типа T - реализация: case of types
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual T PropertyOfType<T>() where T : class => default;


        /// <summary>
        /// Возвращает свойства типов T - реализация: case of types
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual T[] PropertiesOfType<T>() where T : class => default;

        public ISceneObject SceneObject { get; set; }

        public void SetView(ISceneObject sceneObject)
        {
            SceneObject = sceneObject;
        }

        public virtual void Destroy()
        {

        }

        public void Init()
        {
        }
    }
}