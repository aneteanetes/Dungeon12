namespace Rogue.App
{
    using Avalonia;

    public class Program
    {
        /// <summary>
        /// Компилирует БД из json файлов, будет замедлять запуск
        /// </summary>
        private static bool CompileDatabase => false;

        static void Main(string[] args)
        {
            //if (CompileDatabase)
            //{
            //    Rogue.DataAccess.Program.Main(new string[0]);
            //}

            BuildAvaloniaApp().Start<MainView>();
        }

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<Application>()
                .UsePlatformDetect();
    }
}