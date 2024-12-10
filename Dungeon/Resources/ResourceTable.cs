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
            if (value != null)
                return true;

            return DungeonGlobal.Resources.TryGetValue(path, out value);
        }

        public Resource Get(string path)
        {
            TryGetValue(path, out Resource value);
            return value;
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
            var res = ResourceLoader.Load(this, path);
            return res;
        }

        public Resource LoadGlobal(string path)
        {
            var res = ResourceLoader.Load(DungeonGlobal.Resources, path);
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

        /// <summary>
        /// Все шрифты загружаются глобально
        /// </summary>
        /// <param name="fontName"></param>
        public void LoadFont(string fontName)
        {
            this.LoadGlobal($"{DungeonGlobal.GameAssemblyName}.Resources.Fonts.ttf/{fontName}.ttf".Embedded());
        }
    }
}
