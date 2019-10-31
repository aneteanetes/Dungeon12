using Microsoft.Build.Locator;
using System;

namespace Dungeon.PropsGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            MSBuildLocator.RegisterDefaults();
            new PropGenerator(@"C:\Users\Anton\Source\Repos\Rogue\Rouge.sln", new ConditionalSymbolProps("Core"))
                .Generate();
            Console.WriteLine("Press any key to continue...");
            Console.Read();
        }
    }
}
