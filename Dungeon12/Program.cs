using Dungeon;
using Dungeon.Monogame.Runner;
using Dungeon.Monogame.Settings;
using Dungeon.Resources;
using Dungeon12;
using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

DungeonGlobal.BindGlobal<Global>(true, true);

ResourceLoader.Settings.EmbeddedMode = false;


//DungeonGlobal.ExceptionRethrow = true;
//DungeonGlobal.GlobalExceptionHandling();
//ResourceLoader.NotDisposingResources = true;
//ResourceLoader.CacheImagesAndMasks = false;

ResourceLoader.ResourceResolvers.Add(new EmbeddedResourceResolver(Assembly.GetExecutingAssembly()));

var width = 1920;
var height = 1080;
var monitor = 1;

if (args!=null && args.Length > 0)
{
    int.TryParse(args.ElementAtOrDefault(0) ?? "1920", out width);
    int.TryParse(args.ElementAtOrDefault(1) ?? "1080", out height);
    int.TryParse(args.ElementAtOrDefault(2) ?? "1", out monitor);
}

var client = new GameRunner(new GameSettings()
{
    OriginWidthPixel = width,
    OriginHeightPixel = height,
    WindowMode = WindowMode.FullScreenHardware,
    Add2DLighting = true,
    IsWindowedFullScreen = false,
    MonitorIndex = monitor,
    NeedCalculateCamera=false,
#if DEBUG
    IsDebug = true,
#endif
});

DungeonGlobal.OnRun+=() =>
{
    Task.Run(() =>
    {
        while (true)
        {
            var value = Console.ReadLine();
            if (value.IsNotEmpty() && value.Contains(' '))
            {                
                var keyvalue = value.Split(' ');
                Debugger.Set(keyvalue[0], keyvalue[1]);
            }
            else if (!value.Contains(' '))
            {
                Debugger.Get(value);
            }
        }
    });
};

DungeonGlobal.Run(client);