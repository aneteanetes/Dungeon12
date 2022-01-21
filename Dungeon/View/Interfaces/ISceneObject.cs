namespace Dungeon.View.Interfaces
{
    using Dungeon.Types;
    using Dungeon.Utils;
    using Dungeon.View.Enums;
    using System;
    using System.Collections.Generic;

    [Hidden]
    public interface ISceneObject : IGameComponent
    {
        public bool AlphaBlend { get; set; }

        public ITileMap TileMap { get; set; }

        /// <summary>
        /// Host layer
        /// </summary>
        ISceneLayer Layer { get; set; }

        bool HighLevelComponent { get; set; }

        bool Shadow { get; set; }

        /// <summary>
        /// Can be cached or have animation
        /// </summary>
        bool CacheAvailable { get; }

        /// <summary>
        /// Кэш позиции, используется когда родительский объект двигается, и вместе с ним должен двигаться дочерний
        /// </summary>
        bool CachePosition { get; }
        
        /// <summary>
        /// Is this object can be batched
        /// </summary>
        bool IsBatch { get; }

        /// <summary>
        /// is this object need to re cached
        /// </summary>
        bool Expired { get; set; }

        /// <summary>
        /// Флаг указывающий что на объект действуют глобальные фильтры
        /// </summary>
        bool Filtered { get; }

        /// <summary>
        /// Must exists
        /// </summary>
        Rectangle BoundPosition { get; }

        /// <summary>
        /// Must exists
        /// </summary>
        Rectangle CropPosition { get; }

        ITexture Texture { get; set; }

        /// <summary>
        /// 0..1
        /// </summary>
        double Opacity { get; set; }

        /// <summary>
        /// Масштаб
        /// </summary>
        double Scale { get; set; }

        double GetScaleValue();

        /// <summary>
        /// Тэг для поиска в visual tree
        /// </summary>
        public string Tag { get; set; }

        bool ScaleAndResize { get; set; }

        /// <summary>
        /// Position with parent and scale
        /// </summary>
        Rectangle ComputedPosition { get; }

        string Image { get; set; }

        Rectangle ImageRegion { get; set; }

        bool ImageInvertColor { get; set; }

        int LayerLevel { get; set; }

        IDrawText Text { get; }

        IDrawablePath Path { get; }

        /// <summary>
        /// <para>
        /// <para>Этот флаг говорит о том что если движок рисования умеет по разному рисовать изображение, </para>
        /// <para>и один из режимов в разных случаях работает по разному, то для этого объекта</para>
        /// следует выбрать режим сглаживания, такая логика работает по умолчанию со шрифтами
        /// </para>
        /// </summary>
        bool Blur { get; }

        void AddParticleEffects(params ISceneObject[] effects);

        IImageMask ImageMask { get; }

        [Hidden]
        ISceneObject Parent { get; set; }

        [Hidden]
        Action<ISceneObject> DestroyBinding { get; set; }

        [Hidden]
        Action<ISceneControl> ControlBinding { get; set; }

        /// <summary>
        /// Невидимый в режиме отрисовки `Force`
        /// </summary>
        [Hidden]
        bool ForceInvisible { get; }

        bool Visible { get; set; }
        
        /// <summary>
        /// Relative
        /// </summary>
        double Left { get; set; }

        /// <summary>
        /// Relative
        /// </summary>
        double Top { get; set; }

        /// <summary>
        /// Relative
        /// </summary>
        double Width { get; set; }

        /// <summary>
        /// Relative
        /// </summary>
        double Height { get; set; }

        /// <summary>
        /// If object dont have width/height but have image with size it size will be bind to the sceneobject
        /// </summary>
        bool AutoBindSceneObjectSizeByContainedImage { get; set; }

        /// <summary>
        /// Угол на который надо повернуть объект при отображении
        /// </summary>

        double Angle { get;}

        public FlipStrategy Flip { get; set; }

        ICollection<ISceneObject> Children { get; }

        IDrawColor Color { get; set; }

        bool AbsolutePosition { get; }

        string Uid { get; }

        /// <summary>
        /// Вызвать уничтожение объекта. КОМУ НАДО ТОТ УНИЧТОЖИТ ЁПТА
        /// </summary>
        [Hidden]
        new Action Destroy { get; set; }

        /// <summary>
        /// Посылание эффектов в сцену
        /// </summary>
        [Hidden]
        Action<List<ISceneObject>> ShowInScene { get; set; }

        int ZIndex { get; set; }

        bool DrawOutOfSight { get; set; }

        [Title("Рисовать видимую часть")]
        bool DrawPartInSight { get; set; }

        bool IntersectsWith(ISceneObject another);

        ILight Light { get; set; }

        List<IEffect> ParticleEffects { get; set; }

        bool Interface { get; set; }

        /// <summary>
        /// Метод вызывается перед отрисовкой, а то заебало уже хаки юзать
        /// </summary>
        void Update(GameTimeLoop gameTime);

        /// <summary>
        /// Метод вызывается перед отрисовкой, а то заебало уже хаки юзать
        /// </summary>
        void Update();

        bool Drawed { get; set; }

        bool Updatable { get; }

        ISceneObject AddChild(ISceneObject sceneObject);

        ISceneObject RemoveChild(ISceneObject sceneObject);

        bool PerPixelCollision { get; }
    }
}
