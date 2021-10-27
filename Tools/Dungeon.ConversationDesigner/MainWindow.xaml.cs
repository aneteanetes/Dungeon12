using Dungeon.ConversationDesigner.ViewModels;
using Dungeon12.Conversations;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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

            DataContext = vm=new ApplicationViewModel();

            this.KeyDown += MainWindow_KeyDown;
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.S && Keyboard.Modifiers == ModifierKeys.Control)
            {
                SaveClick(default, default);
            }
        }

        ApplicationViewModel vm;

        private string sourcePath = "";

        private void OpenClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                sourcePath = openFileDialog.FileName;
                DataContext = vm= new ApplicationViewModel(sourcePath);
            }
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            foreach (var subj in vm.Subjects)
            {
                subj.Save();
            }
            File.WriteAllText(sourcePath, JsonConvert.SerializeObject(vm.Conversation,new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting= Formatting.Indented,
                DefaultValueHandling = DefaultValueHandling.Ignore
            }));
        }

        private void AddSubject(object sender, RoutedEventArgs e)
        {
            vm.AddNewSubject();
        }

        private void RemoveSubject(object sender, RoutedEventArgs e)
        {
            if (!(e.Source is Button btn)) return;
            if (!(btn.Parent is StackPanel stackPanel)) return;
            if (!(stackPanel.DataContext is SubjectViewModel subjectViewModel)) return;
            vm.RemoveSubject(subjectViewModel);
        }

        private void AddSubjectVariable(object sender, RoutedEventArgs e)
        {
            vm.Selected.AddVariable();
            subjvariables.Items.Refresh();
        }
        private void RemoveSubjectVariable(object sender, RoutedEventArgs e)
        {
            if (!(e.Source is Button btn)) return;
            if (!(btn.Parent is StackPanel stackPanel)) return;
            if (!(stackPanel.DataContext is VariableViewModel variableViewModel)) return;
            
            vm.Selected.RemoveVariable(variableViewModel);
            subjvariables.Items.Refresh();
        }

        private void AddSubjectReplica(object sender, RoutedEventArgs e)
        {
            vm.AddSubjectReplica();
        }
        private void RemoveSubjectReplica(object sender, RoutedEventArgs e)
        {
            if (!(e.Source is Button btn)) return;
            if (!(btn.Parent is StackPanel stackPanel)) return;
            if (!(stackPanel.DataContext is ReplicaViewModel replicaViewModel)) return;
            vm.RemoveSubjectReplica(replicaViewModel);
        }

        private void AddReplicaVariable(object sender, RoutedEventArgs e)
        {
            vm.Selected.SelectedReplica.AddVariable();
            replicavariables.Items.Refresh();
        }
        private void RemoveReplicaVariable(object sender, RoutedEventArgs e)
        {
            if (!(e.Source is Button btn)) return;
            if (!(btn.Parent is StackPanel stackPanel)) return;
            if (!(stackPanel.DataContext is VariableViewModel variableViewModel)) return;

            vm.Selected.SelectedReplica.RemoveVariable(variableViewModel);
            replicavariables.Items.Refresh();
        }

        private void AddReplicaSwitchTag(object sender, RoutedEventArgs e)
        {
            vm.Selected.SelectedReplica.AddReplicaSwitchTag();
        }

        private void RemoveReplicaSwitchTag(object sender, RoutedEventArgs e)
        {
            if (!(e.Source is Button btn)) return;
            if (!(btn.Parent is StackPanel stackPanel)) return;
            if (!(stackPanel.DataContext is ReplicaLinViewModel replicaLinViewModel)) return;

            vm.Selected.SelectedReplica.RemoveLink(replicaLinViewModel);
            replicaswitcheslist.Items.Refresh();
        }

        private void RemoveTriggerArguments(object sender, RoutedEventArgs e)
        {
            if (!(e.Source is Button btn)) return;
            if (!(btn.Parent is StackPanel stackPanel)) return;
            if (!(stackPanel.DataContext is ReplicaTriggerArgumentViewModel  replicaTriggerArgumentViewModel)) return;

            vm.Selected.SelectedReplica.RemoveTriggerArguments(replicaTriggerArgumentViewModel);
        }

        private void AddTriggerArguments(object sender, RoutedEventArgs e)
        {
            vm.Selected.SelectedReplica.AddTriggerArguments();
        }

        private void AddQuest(object sender, RoutedEventArgs e)
        {
            vm.AddQuest();
        }
    }
}
