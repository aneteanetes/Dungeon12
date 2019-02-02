namespace Rogue.View.Interfaces
{
    using Rogue.Types;
    using System.Collections.Generic;

    public interface ISceneObject
    {
        /// <summary>
        /// Must exists
        /// </summary>
        Rectangle Position { get; }

        string Image { get; }

        Rectangle ImageRegion { get; }

        IDrawText Text { get; }

        IDrawablePath Path { get; }

        ICollection<ISceneObject> Children { get; }

        string Uid { get; }
    }
}
