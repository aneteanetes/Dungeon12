namespace Rogue.Control.Commands
{
    using System;
    using Rogue.Control.Keys;

    public class Command
    {
        public Key Key { get; set; }

        public string Name { get; set; }

        public Action Run { get; set; }
    }
}