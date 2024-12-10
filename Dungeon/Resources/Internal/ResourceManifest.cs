using Dungeon.Data;
using System.Collections.Generic;

namespace Dungeon.Resources.Internal
{
    public class ResourceManifest : Persist
    {
        public List<Resource> Resources { get; set; } = new List<Resource>();
    }
}