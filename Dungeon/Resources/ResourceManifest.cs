using System;
using System.Collections.Generic;
using System.Text;
using Dungeon.Data;

namespace Dungeon.Resources
{
    public class ResourceManifest : Persist
    {
        public List<Resource> Resources { get; set; } = new List<Resource>();
    }
}