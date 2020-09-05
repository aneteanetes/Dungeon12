using System.Windows;

namespace Dungeon.Engine.Forms
{
    /// <summary>
    /// Interaction logic for ProjectForm.xaml
    /// </summary>
    public partial class Message : Window
    {
        public Message(string message)
        {
            InitializeComponent();
            MsgText.Text = message;
        }

        public static void Show(string msg)
        {
            new Message(msg).ShowDialog();
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}