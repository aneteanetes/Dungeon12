using Dungeon.Engine.Utils;
using Dungeon.Resources;
using System.Windows;

namespace Dungeon.Engine
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Container Container { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var resCompiler = new ResourceCompiler();
            resCompiler.Compile();

            Store.LoadAllAssemblies();
            Container = new Container();
        }
    }
}
