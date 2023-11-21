using System;
using System.Collections.Generic;

namespace Dungeon.Resources
{
    public class ResourceTable : IDisposable
    {
        private Dictionary<string, Resource> resources = new();

        public bool TryGetValue(string path, out Resource value)
        {
            value = this[path];
            return value != null;
        }

        public bool ContainsKey(string path) => resources.ContainsKey(path);

        public void Add(string path, Resource res)
        {
            resources[path] = res;
        }

        public Resource this[string path]
        {
            get
            {
                if(!resources.TryGetValue(path, out var res))
                {
                    res = Load(path);
                }

                return res;
            }
        }

        public Resource Load(string path)
        {
            var res = ResourceLoader.Load(path);
            resources.Add(path, res);
            return res;
        }

        public void Load(IEnumerable<string> paths)
        {
            foreach (var path in paths)
            {
                Load(path);
            }
        }

        public void Dispose()
        {
            foreach (var kv in this.resources)
            {
                kv.Value.Dispose();
            }
            this.resources.Clear();
        }
    }
}
