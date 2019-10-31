using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dungeon.AndroidProjectConverter
{
    public class ProjectGuidPool
    {
        private HashSet<Guid> Guids = new HashSet<Guid>();
        private Dictionary<string, (Guid uid, string path)> Pool = new Dictionary<string, (Guid uid, string path)>();

        public ProjectGuidPool(IEnumerable<(string name, string path)> values)
        {
            Guids = Enumerable.Range(0, values.Count()).Select(x => Guid.NewGuid()).ToHashSet();
            values.ToList().ForEach(x =>
            {
                GetUid(x.name, x.path.Replace(x.name.Replace("Android",""), x.name + "."));
            });
        }

        public string GetUid(string project, string path)
        {
            if (!Pool.TryGetValue(project, out var uid))
            {
                uid = (GetFreeGuid(), path);
                Pool.Add(project, uid);
            }
            return uid.ToString();
        }

        public string GetUid(string project)
        {
            return Pool[project].uid.ToString();
        }

        public IEnumerable<(string, string, Guid)> GetPool() => Pool.Select(x => (x.Key, x.Value.path, x.Value.uid));

        private Guid GetFreeGuid()
        {
            return Guids.FirstOrDefault(x => !Pool.Select(p => p.Value.uid).Contains(x));
        }
    }
}