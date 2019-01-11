using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Rogue.Windows
{
    public class MainView : Window
    {
        public MainView()
        {
            this.CanResize = false;
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();            
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}