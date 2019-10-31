using Dungeon.Classes;
using Dungeon.Map.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon.Map.Objects
{
    [Template("†")]
    public class Ally : Avatar
    {
        public Ally(Character character) : base(character)
        {
        }
    }
}
