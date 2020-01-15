using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.PlatformAbstractions;
using Semver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Dungeon.UpdateServer.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class NotesController : ControllerBase
    {
        [HttpGet]
        public string Get(string platform, string fromVersion, string toVersion)
        {
            string notesPath(string v)
            {
                return $"{root}clients{sep}{platform}{sep}{v}{sep}notes.txt";
            };

            List<SemVersion> versions = new List<SemVersion>();

            foreach (var dir in Directory.GetDirectories($"{root}clients{sep}{platform}"))
            {
                versions.Add(SemVersion.Parse(new DirectoryInfo(dir).Name));
            }

            versions = versions.OrderBy(x => x).ToList();

            var indexOfCurrent = versions.IndexOf(versions.FirstOrDefault(x => x.ToString() == fromVersion));

            StringBuilder patchNotes = new StringBuilder();

            for (int i = indexOfCurrent; i < versions.Count; i++)
            {
                var notes = notesPath(versions[i].ToString());
                patchNotes.Append(System.IO.File.ReadAllText(notes));
            }

            return patchNotes.ToString();
        }

        private static string sep => Path.DirectorySeparatorChar.ToString();

        private static string root => PlatformServices.Default.Application.ApplicationBasePath;
    }
}