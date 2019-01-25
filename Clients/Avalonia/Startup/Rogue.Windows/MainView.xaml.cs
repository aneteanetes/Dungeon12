using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Rogue.Scenes;

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

        protected override void OnKeyDown(KeyEventArgs e)
        {
            SceneManager.KeyDown(e);
            base.OnKeyDown(e);
        }
    }
}