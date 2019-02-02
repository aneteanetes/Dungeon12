namespace Rogue.View.Interfaces
{
    using Rogue.Types;
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
        /// Must exists
        /// </summary>
        Rectangle Position { get; }

        /// <summary>
        /// Position with parent
        /// </summary>
        Rectangle ComputedPosition { get; }

        string Image { get; }

        Rectangle ImageRegion { get; }

        IDrawText Text { get; }

        IDrawablePath Path { get; }

        ISceneObject Parent { get; }

        ICollection<ISceneObject> Children { get; }

        string Uid { get; }
    }
}
