namespace Rogue.App
{
    using Avalonia;

    public class Program
    {
        static void Main(string[] args)
        {
            BuildAvaloniaApp().Start<MainView>();
        }

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<Application>()
                .UsePlatformDetect();
    }
}