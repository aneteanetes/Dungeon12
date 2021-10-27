using Microsoft.CodeAnalysis.MSBuild;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon.AndroidProjectConverter
{
    public class SolutionConverter
    {
        private string _slnPath;

        public SolutionConverter(string slnPath)
        {
            _slnPath = slnPath;
        }

        public async Task Convert()
        {
            using (var workspace = MSBuildWorkspace.Create())
            {
                var sln = await workspace.OpenSolutionAsync(_slnPath);

                var pool = new ProjectGuidPool(sln.Projects.Select(x => (x.Name+".Android",x.FilePath)));

                foreach (var project in sln.Projects)
                {
                    Console.WriteLine(project.Name);
                    var writer = new AndroidProjectWriter(project.Name + ".Android", project.FilePath,pool);

                    var assetsPath = Path.Combine(Path.GetDirectoryName(project.FilePath), @"obj\project.assets.json");
                    var json = File.ReadAllText(assetsPath);

                    var obj = JObject.Parse(json);

                    foreach (JProperty lib in obj["libraries"])
                    {
                        var isProj = lib.Children().First()["type"].ToString() == "project" ? true : false;

                        if (isProj)
                        {
                            var projName = lib.Name.Split("/", StringSplitOptions.RemoveEmptyEntries)[0];
                            var path = lib.Children().First()["path"].ToString()
                                .Replace(projName+".csproj", projName + ".Android.csproj");

                            writer.WriteProjectReference(projName + ".Android", path);
                        }
                        else
                        {
                            if (lib.Name.Contains("NETCore") || lib.Name.Contains("NETStand"))
                            {
                                continue;
                            }

                            var complexName = lib.Name.Split("/", StringSplitOptions.RemoveEmptyEntries);

                            writer.WritePackageReference(complexName[0], complexName[1]);
                        }

                    }

                    writer.WriteFile();
                }

                var slnWriter = new AndroidSolutionWriter(Path.GetFileNameWithoutExtension(_slnPath), _slnPath, pool);
                slnWriter.WriteFile();

            }
        }
    }
}
