using Microsoft.CodeAnalysis.MSBuild;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Rogue.PropsGenerator
{
    public class PropGenerator
    {
        List<IProps> _props;
        string _solutionFilePath;

        public PropGenerator(string solutionFilePath, params IProps[] props)
        {
            _solutionFilePath = solutionFilePath;
            _props = props.ToList();
        }

        public void Generate()
        {
            LoadProjects();
            _props.ForEach(GenerateProp);
            _props.ForEach(ImportProp);
        }

        List<string> projectFiles = new List<string>();

        private void LoadProjects()
        {
            Task.Run(async () =>
            {
                using (var workspace = MSBuildWorkspace.Create())
                {
                    var sln = await workspace.OpenSolutionAsync(_solutionFilePath);

                    projectFiles = sln.Projects.Select(p => p.FilePath).ToList();
                }
            }).GetAwaiter().GetResult();
        }

        private void ImportProp(IProps props)
        {
            var propName = $"{props.Name}.props";
            var propValue = $@"$(SolutionDir)\\{propName}";

            projectFiles.ForEach(proj =>
            {
                var xml = XDocument.Parse(File.ReadAllText(proj));

                var propImported = xml.Nodes().Any(n =>
                {
                    if (!(n is XElement xElement))
                        return false;

                    if (xElement.Name != "Import")
                        return false;

                    return xElement.Attribute("Project")?.Value == propValue;
                });

                if (!propImported)
                {
                    var import = new XElement("Import");
                    import.Add(new XAttribute("Project", "$(SolutionDir)\\" + props.Name + ".props"));
                    import.Add(new XAttribute("PropsGenerator", propName));
                    xml.Root.Add(import);

                    xml.Save(proj);
                }
            });
        }

        string SolutionPath => Path.GetDirectoryName(_solutionFilePath);

        string InSlnFolder(string file) => Path.Combine(SolutionPath, file);

        private void GenerateProp(IProps props)
        {
            var propFileName = props.Name + ".props";
            var propPath = InSlnFolder(propFileName);

            File.WriteAllText(propPath, props.Content());
        }
    }
}
