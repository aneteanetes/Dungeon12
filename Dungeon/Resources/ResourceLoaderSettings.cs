using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon.Resources
{
    public sealed class ResourceLoaderSettings
    {
        public bool ThrowIfNotFound { get; set; } = true;

        public Action<string> NotFoundAction { get; set; }
    }
}
