using System.Windows;
using System.Windows.Forms;

namespace Dungeon.Engine.Forms
{
    /// <summary>
    /// Interaction logic for ProjectForm.xaml
    /// </summary>
    public partial class AddNamedForm : Window
    {
        public AddNamedForm(string title, bool file=false)
        {
            InitializeComponent();
            this.Title.Text = title;
            DataContext = this;

            if(file)
            {
                FileField.Visibility = Visibility.Visible;
            }
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

        private void SelectPathButton(object sender, RoutedEventArgs e)
        {
            using var dialog = new OpenFileDialog();
            var result = dialog.ShowDialog();
            nametxt.Text = dialog.FileName;
        }

        public string Text { get; private set; }
    }
}
