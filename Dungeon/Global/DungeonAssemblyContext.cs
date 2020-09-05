using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace Dungeon.Global
{
    public class DungeonAssemblyContext : AssemblyLoadContext
    {
        public DungeonAssemblyContext() : base(isCollectible: true) { }
    }  
}
