using System.Windows;

namespace Dungeon.Engine.Forms
{
    /// <summary>
    /// Interaction logic for ProjectForm.xaml
    /// </summary>
    public partial class AddNamedForm : Window
    {
        public AddNamedForm(string title)
        {
            InitializeComponent();
            this.Title.Text = title;
            DataContext = this;
        }
        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Cancel(object sender, RoutedEventArgs e) => OnCloseButtonClick(sender, e);

        private void AddResProcess(object sender, RoutedEventArgs e)
        {
            Text = nametxt.Text;
            this.Close();
        }

        public string Text { get; private set; }
    }
}
