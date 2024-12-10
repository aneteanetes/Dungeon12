using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon.Resources.Resolvers
{
    public abstract class ResourceResolver
    {
        public abstract Resource Resolve(string res);
    }
}
