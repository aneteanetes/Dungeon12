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
        public IActionResult Get(string platform, string version)
        {
            var pathToFile = PlatformServices.Default.Application.ApplicationBasePath
               + Path.DirectorySeparatorChar.ToString()
               + "files"
               + Path.DirectorySeparatorChar.ToString()
               + platform
               + Path.DirectorySeparatorChar.ToString()
               + $"{version}.zip";

            return PhysicalFile(pathToFile, "application/octet-stream", $"{version}.zip");
        }
    }
}