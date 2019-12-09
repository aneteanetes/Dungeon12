using Dungeon12.Classes;
using Dungeon12.Map.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Map.Objects
{
    [Template("†")]
    public class Ally : Avatar
    {
        public Ally(Character character) : base(character)
        {
        }
    }
}
