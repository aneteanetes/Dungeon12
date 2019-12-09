using Dungeon12.Conversations;
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
            ReplicsView = new ObservableCollection<ReplicaViewModel>(subject.Replics?.Select(x => new ReplicaViewModel(x, GetReplics)) ?? new ReplicaViewModel[0]);
            VariablesVM = new ObservableCollection<VariableViewModel>(subject.Variables.Select(x => new VariableViewModel(x, GetReplics)));

            this.VisibleName = subject.Visible?.Name;
            this.InvisibleName = subject.Invisible?.Name;
        }

        public void Save()
        {
            foreach (var replicView in this.ReplicsView)
            {
                replicView.Save();
            }
        }

        private string _visibleName;
        public string VisibleName
        {
            get => _visibleName;
            set
            {
                _visibleName = value;
                if (Value.Visible == default)
                {
                    Value.Visible = new Variable();
                }
                Value.Visible.Name = value;
            }
        }

        private string _invisibleName;
        public string InvisibleName
        {
            get => _invisibleName;
            set
            {
                _invisibleName = value;
                if (Value.Invisible == default)
                {
                    Value.Invisible = new Variable();
                }
                Value.Invisible.Name = value;
            }
        }


        public void AddVariable()
        {
            var v = new Variable();
            this.Value.Variables.Add(v);
            this.VariablesVM.Add(new VariableViewModel(v, GetReplics));
        }

        public void RemoveVariable(VariableViewModel variableViewModel)
        {
            this.VariablesVM.Remove(variableViewModel);
            this.Value.Variables.Remove(variableViewModel.Value);
        }

        public ObservableCollection<VariableViewModel> VariablesVM { get; set; }

        public ObservableCollection<ReplicaViewModel> ReplicsView { get; set; }

        public List<Replica> GetReplics() => ReplicsView.Select(x => x.Value).ToList();

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
