using System;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon.Resources
{
    public class ResourceTable : IDisposable
    {
        private Dictionary<string, Resource> resources = new();
        private Dictionary<string,IEnumerable<Resource>> folderResources = new();

        public bool IsFolderLoaded(string path) => folderResources.ContainsKey(path);

        public IEnumerable<Resource> GetFolder(string path) => folderResources[path];

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

        public void AddFolder(string folder, IEnumerable<Resource> folderRes)
        {
            folderResources[folder] = folderRes;
            foreach (var res in folderRes)
            {
                this.Add(res.Path,res);
            }
        }

        public Resource this[string path]
        {
            get
            {
                if(!resources.TryGetValue(path, out var res))
                {
                    res = Load(path);
                }

                if (res == null)
                    throw new KeyNotFoundException(path);

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

        public IEnumerable<Resource> LoadFolder(string path)
        {
            return ResourceLoader.LoadResourceFolder(path,this);
        }

        public IEnumerable<Resource> LoadFolderGlobal(string path)
        {
            return ResourceLoader.LoadResourceFolder(path, DungeonGlobal.Resources);
        }

        public void UnloadFolderGlobal(string path)
        {
            DungeonGlobal.Resources.folderResources[path].ForEach(r => r.Dispose());
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
            this.folderResources.Clear();
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
