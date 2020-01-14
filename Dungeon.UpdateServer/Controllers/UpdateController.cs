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

            var pathToFile = PlatformServices.Default.Application.ApplicationBasePath
               + Path.DirectorySeparatorChar.ToString()
               + "files"
               + Path.DirectorySeparatorChar.ToString()
               + platform
               + Path.DirectorySeparatorChar.ToString()
               + name;

            return PhysicalFile(pathToFile, "application/octet-stream", $"{version}.zip");
        }
    }
}