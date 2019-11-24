using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Dungeon.Resources
{
    public class Resource
    {
        public string Name { get; set; }

        public Stream Stream { get; set; }

        public Action Dispose { get; set; }
    }
}
