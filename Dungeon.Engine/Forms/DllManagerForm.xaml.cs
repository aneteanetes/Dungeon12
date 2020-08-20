using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Dungeon.Engine.Forms
{
    /// <summary>
    /// Interaction logic for DllManagerForm.xaml
    /// </summary>
    public partial class DllManagerForm : Window
    {
        public DllManagerForm()
        {
            InitializeComponent();
        }

        private void RemoveDllReference(object sender, RoutedEventArgs e)
        {

        }
        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
