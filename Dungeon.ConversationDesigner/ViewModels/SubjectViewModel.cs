using Dungeon.Conversations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon.ConversationDesigner.ViewModels
{
    public class SubjectViewModel : INotifyPropertyChanged
    {
        public Subject Value { get; set; }

        public SubjectViewModel(Subject subject)
        {
            Value = subject;
            ReplicsView = new ObservableCollection<ReplicaViewModel>(subject.Replics.Select(x => new ReplicaViewModel(x)));
        }

        public ObservableCollection<ReplicaViewModel> ReplicsView { get; set; }


        private ReplicaViewModel selectedReplica;
        public ReplicaViewModel SelectedReplica
        {
            get { return selectedReplica; }
            set
            {
                selectedReplica = value;
                OnPropertyChanged("SelectedReplica");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
