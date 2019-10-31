using Dungeon.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Dungeon.AndroidProjectConverter
{
    public class AndroidSolutionWriter : CustomWriter
    {
        private const string ProjectGuid = "[PROJGUID]";
        private const string PackageReferences = "[PKGREF]";
        private const string ProjectReferences = "[PRJREF]";
        private const string Name = "[NME]";
        private const string Version = "[VRS]";
        private const string Uid = "[UID]";
        private const string ProjectPath = "[PTH]";
        private const string ProjectsInSolution = "[PRJS]";

        private string _name;
        private string _filePath;
        private ProjectGuidPool _pool;

        public AndroidSolutionWriter(string sln, string slnPath, ProjectGuidPool pool)
        {
            _name = sln;
            _filePath = slnPath;
            _pool = pool;
        }

        private string WriteProjInSln(string proj,string path, Guid guid)
        {
            var tpl = GetTemplate("ProjectInSolutionTemplate.slnt");
            return tpl.Replace(ProjectPath, path)
                .Replace(Uid, guid.ToString())
                .Replace(Name, proj);
        }

        public void WriteFile()
        {
            var tpl = GetTemplate("SolutionTemplate.slnt");
            var prjs = _pool.GetPool().Select(x => WriteProjInSln(x.Item1, x.Item2, x.Item3));
            var sln = tpl.Replace(ProjectsInSolution, string.Join(Environment.NewLine, prjs));

            File.WriteAllText(Path.Combine(Path.GetDirectoryName(_filePath), $"{_name}.Android.sln"), sln);
        }
    }
}
