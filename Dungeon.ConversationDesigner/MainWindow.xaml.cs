using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Dungeon.ConversationDesigner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = vm=new ApplicationViewModel(@"C:\Users\a.tretyakov\source\repos\Dungeon-12\Dungeon12\Database\Conversations\Data\FaithIslandTavernResident.json");
        }

        ApplicationViewModel vm;

        private void OpenClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                DataContext = vm= new ApplicationViewModel(openFileDialog.FileName);
            }
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
        }
    }
}
