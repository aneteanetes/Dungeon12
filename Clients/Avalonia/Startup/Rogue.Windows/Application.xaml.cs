namespace Rogue.Windows
{
    using Avalonia.Markup.Xaml;

    public class Application : Avalonia.Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
