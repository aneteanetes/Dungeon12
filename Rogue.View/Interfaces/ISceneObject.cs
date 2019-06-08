namespace Rogue.View.Interfaces
{
    using Rogue.Types;
    using System;
    using System.Collections.Generic;

    public interface ISceneObject
    {
        /// <summary>
        /// Can be cached or have animation
        /// </summary>
        bool CacheAvailable { get; }

        /// <summary>
        /// Is this object can be batched
        /// </summary>
        bool IsBatch { get; }

        /// <summary>
        /// is this object need to re cached
        /// </summary>
        bool Expired { get; }

        /// <summary>
        /// Must exists
        /// </summary>
        Rectangle Position { get; }

        /// <summary>
        /// Must exists
        /// </summary>
        Rectangle CropPosition { get; }

        /// <summary>
        /// Position with parent
        /// </summary>
        Rectangle ComputedPosition { get; }

        string Image { get; }

        Rectangle ImageRegion { get; set; }

        int Layer { get; set; }

        IDrawText Text { get; }

        IDrawablePath Path { get; }

        ISceneObject Parent { get; }

        Action<ISceneObject> DestroyBinding { get; set; }

        Action<ISceneObjectControl> ControlBinding { get; set; }

        bool ForceInvisible { get; }

        bool Visible { get; }

        ICollection<ISceneObject> Children { get; }

        bool AbsolutePosition { get; }

        string Uid { get; }

        /// <summary>
        /// Вызвать уничтожение объекта. КОМУ НАДО ТОТ УНИЧТОЖИТ ЁПТА
        /// </summary>
        Action Destroy { get; set; }

        /// <summary>
        /// Посылание эффектов в сцену
        /// </summary>
        Action<List<ISceneObject>> ShowEffects { get; set; }
    }
}
