using Rogue.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Rogue.AndroidProjectConverter
{
    public class AndroidProjectWriter : CustomWriter
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

        private List<string> ProjRefs = new List<string>();
        private List<string> Pkgs = new List<string>();

        public AndroidProjectWriter(string project, string projectPath, ProjectGuidPool pool)
        {
            _name = project;
            _filePath = projectPath;
            _pool = pool;
        }

        public void WriteProjectReference(string projRef, string pth)
        {
            var tpl = GetTemplate("ProjectRefTemplate.xml");
            var @ref = tpl.Replace(ProjectPath, pth)
                .Replace(Uid, _pool.GetUid(projRef))
                .Replace(Name, projRef);

            ProjRefs.Add(@ref);
        }

        public void WritePackageReference(string pkgRef,string vers)
        {
            var tpl = GetTemplate("PackageRefTemplate.xml");
            var @ref = tpl.Replace(Name, pkgRef)
                .Replace(Version, vers);
            Pkgs.Add(@ref);
        }

        public void WriteFile()
        {
            var tpl = GetTemplate("ProjectTemplate.xml");
            var proj = tpl.Replace("[PROJGUID]", _pool.GetUid(_name))
                .Replace(PackageReferences, string.Join(Environment.NewLine, Pkgs))
                .Replace(ProjectReferences, string.Join(Environment.NewLine, ProjRefs))
                .Replace(Name, _name);

            File.WriteAllText(Path.Combine(Path.GetDirectoryName(_filePath), $"{_name}.csproj"), proj);
        }
    }
}
