using Dungeon.Engine.Utils;
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
            Store.LoadAllAssemblies();
            Container = new Container();
        }
    }
}
