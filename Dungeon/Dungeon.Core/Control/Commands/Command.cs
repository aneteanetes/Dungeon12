namespace Dungeon.Control.Commands
{
    using System;
    using System.Collections.Generic;
    using Dungeon.Control.Keys;

    public abstract class Command
    {
        public abstract IEnumerable<Key> Keys { get; }

        public abstract string Name { get; }

        public abstract void Run(Key keyPressed);

        public abstract bool UI { get; }
    }
}