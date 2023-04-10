using Dungeon;
using Dungeon.Monogame.Runner;
using Dungeon.Monogame.Settings;
using Dungeon.Resources;
using Dungeon12;

var cfg = DungeonGlobal.Init<Global>(true, true);

var monocfg = cfg.Get<MonogameSettings>("Monogame");
#if DEBUG
monocfg.IsDebug = true;
#endif

var client = new MonogameRunner(monocfg);

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