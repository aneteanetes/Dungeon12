using Rogue.Classes;
using Rogue.Map.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rogue.Map.Objects
{
    [Template("†")]
    public class Ally : Avatar
    {
        public Ally(Character character) : base(character)
        {
        }
    }
}
