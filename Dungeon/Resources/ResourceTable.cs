using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
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

        public bool TryGetValue(string path, ISceneObject sceneObject, out Resource value)
        {
            if (!resources.TryGetValue(path, out value))
            {
                if (this != DungeonGlobal.GlobalResources)
                {
                    if (!DungeonGlobal.GlobalResources.TryGetValue(path, sceneObject, out value))
                        throw ResourceNotFound(sceneObject);
                }
            }

            return value != null;
        }

        public Resource Get(string path, ISceneObject sceneObject)
        {
            if (!TryGetValue(path, sceneObject, out Resource value))
                throw ResourceNotFound(sceneObject);
            return value;
        }

        private Exception ResourceNotFound(ISceneObject sceneObject) 
            => new KeyNotFoundException($"SceneObject {sceneObject} requested not registered resource!");

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
                res.OnDispose += () => resources.Remove(res.Path);
                this.Add(res.Path,res);
            }
        }

        public Resource Load(string path,ISceneObject sceneObject)
        {
            var res = ResourceLoader.Load(this, sceneObject, path);
            return res;
        }

        public Resource LoadGlobal(string path, ISceneObject sceneObject)
        {
            var res = ResourceLoader.Load(DungeonGlobal.GlobalResources, sceneObject, path);
            return res;
        }

        public IEnumerable<Resource> LoadFolder(string path)
        {
            return ResourceLoader.LoadResourceFolder(path,this);
        }

        public IEnumerable<Resource> LoadFolderGlobal(string path)
        {
            return ResourceLoader.LoadResourceFolder(path, DungeonGlobal.GlobalResources);
        }

        public void UnloadFolderGlobal(string path)
        {
            var table = DungeonGlobal.GlobalResources;
            var folder = table.folderResources[path];
            foreach (var resource in folder)
            {
                resource.Dispose();
            }

            table.folderResources.Remove(path);
        }

        public void Load(IEnumerable<string> paths, ISceneObject sceneObject)
        {
            foreach (var path in paths)
            {
                Load(path, sceneObject);
            }
        }

        public void Dispose()
        {
            foreach (var kv in this.resources)
            {
                kv.Value?.Dispose();
            }
            this.resources.Clear();
            this.folderResources.Clear();
        }

        /// <summary>
        /// Все шрифты загружаются глобально
        /// </summary>
        /// <param name="fontName"></param>
        public void LoadFont(string fontName, ISceneObject sceneObject)
        {
            this.LoadGlobal($"{DungeonGlobal.GameAssemblyName}.Resources.Assets.Fonts.ttf/{fontName}.ttf".Embedded(), sceneObject);
        }
    }
}
