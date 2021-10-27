using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.PlatformAbstractions;
using System.IO;

namespace Dungeon.UpdateServer.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class VersionController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            var pathToFile = PlatformServices.Default.Application.ApplicationBasePath
               + Path.DirectorySeparatorChar.ToString()
               + "files"
               + Path.DirectorySeparatorChar.ToString()
               + "version.txt";

            return System.IO.File.ReadAllText(pathToFile);
        }
    }
}