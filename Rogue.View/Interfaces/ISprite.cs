namespace Rogue.View.Interfaces
{
    using System;
    using Rogue.Types;

    public interface ISprite
    {
        string Tileset { get; }

        Rectangle Position { get; }

        Rectangle Source { get; }

        Action Click { get; }

        Action Focus { get; }

        Action Unfocus { get; }
    }
}