using Microsoft.Build.Locator;
using System;

namespace Dungeon.AndroidProjectConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            MSBuildLocator.RegisterDefaults();

            try
            {
                var converter = new SolutionConverter(@"C:\Users\a.tretyakov\source\repos\Rogue2\Rouge.sln");
                converter.Convert().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.ReadLine();
        }
    }
}
