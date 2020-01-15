using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.PlatformAbstractions;
using System.IO;

namespace Dungeon.UpdateServer.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class UpdateController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get(string platform, string fromVersion, string toVersion)
        {
            var name = $"{fromVersion + "_" + toVersion}.zip";

            var pathToFile = $"{root}files{sep}{platform}{sep}{name}";

            if (!System.IO.File.Exists(pathToFile))
            {
                Pack.Run(
                    $"{root}clients{sep}{platform}{sep}{fromVersion}",
                    $"{root}clients{sep}{platform}{sep}{toVersion}",
                    name,
                    pathToFile,
                    $"{root}temp{sep}{name}".Replace(".zip",""));
            }

            return PhysicalFile(pathToFile, "application/octet-stream", $"{toVersion}.patch");
        }

        private static string sep => Path.DirectorySeparatorChar.ToString();

        private static string root => PlatformServices.Default.Application.ApplicationBasePath;
    }
}