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
    public class ReplicaViewModel : INotifyPropertyChanged
    {
        public Replica Value { get; set; }

        private Func<List<Replica>> replicsSource;

        public ReplicaViewModel(Replica repl, Func<List<Replica>> replicsSource)
        {
            this.replicsSource = replicsSource;
            Value = repl;
            ReplicsTagSwitch = new ObservableCollection<ReplicaLinViewModel>(repl.ReplicsTags?.Select(x => new ReplicaLinViewModel(x, replicsSource)) ?? new ReplicaLinViewModel[0]);

            if (Value.TriggerArguments == default)
            {
                Value.TriggerArguments = new List<string>();
            }

            TriggerArguments = new ObservableCollection<ReplicaTriggerArgumentViewModel>(Value.TriggerArguments.Select(x => new ReplicaTriggerArgumentViewModel(x)));

            VariablesVM = new ObservableCollection<VariableViewModel>(repl.Variables.Select(x => new VariableViewModel(x, replicsSource)));
        }

        public void Save()
        {
            this.Value.ReplicsTags = this.ReplicsTagSwitch.Select(x => x.Current).ToList();

            if (TriggerArguments.Count == 0)
            {
                Value.TriggerArguments = default;
            }
            else
            {
                Value.TriggerArguments = TriggerArguments.Select(x => x.Name).ToList();
            }
        }

        public ObservableCollection<VariableViewModel> VariablesVM { get; set; }

        public ObservableCollection<ReplicaLinViewModel> ReplicsTagSwitch { get; set; }

        public ObservableCollection<ReplicaTriggerArgumentViewModel> TriggerArguments { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public void AddReplicaSwitchTag()
        {
            ReplicsTagSwitch.Add(new ReplicaLinViewModel(-1, replicsSource));
        }

        public void AddVariable()
        {
            var v = new Variable();
            this.Value.Variables.Add(v);
            this.VariablesVM.Add(new VariableViewModel(v, replicsSource));
        }

        public void RemoveVariable(VariableViewModel variableViewModel)
        {
            this.VariablesVM.Remove(variableViewModel);
            this.Value.Variables.Remove(variableViewModel.Value);
        }

        public void RemoveLink(ReplicaLinViewModel replicaLinViewModel)
        {
            ReplicsTagSwitch.Remove(replicaLinViewModel);
        }

        public void AddTriggerArguments()
        {
            this.TriggerArguments.Add(new ReplicaTriggerArgumentViewModel(""));
        }

        public void RemoveTriggerArguments(ReplicaTriggerArgumentViewModel replicaTriggerArgumentViewModel)
        {
            this.TriggerArguments.Remove(replicaTriggerArgumentViewModel);
        }
    }

    public class ReplicaTriggerArgumentViewModel : INotifyPropertyChanged
    {
        public string Name { get; set; }

        public ReplicaTriggerArgumentViewModel(string value)
        {
            Name = value;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }


    public class VariableViewModel : INotifyPropertyChanged
    {
        private int _tag;
        public int Tag
        {
            get => _tag;
            set
            {
                _tag = value;
                Value.Value = _tag;
            }
        }

        public Variable Value { get; set; }

        private Func<List<Replica>> replicsSource;

        private int _current;
        public int Current
        {
            get => _current;
            set
            {
                _current = value;
                Value.Value = _current;
            }
        }

        public List<ReplicaLinViewModel> ReplicsLinks => replicsSource?.Invoke().Select(x => new ReplicaLinViewModel(x.Tag, replicsSource)).ToList();

        public VariableViewModel(Variable variable, Func<List<Replica>> replicsSource)
        {
            Value = variable;
            this.replicsSource = replicsSource;
            Current = variable.Value;
        }

        public string Name { get => replicsSource?.Invoke().FirstOrDefault(x => x.Tag == this.Tag).Answer ?? "Реплика не выбрана"; set { } }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }

    public class ReplicaLinViewModel : INotifyPropertyChanged
    {
        public int Tag { get; set; }
        private Func<List<Replica>> replicsSource;
        
        public List<ReplicaLinViewModel> ReplicsLinks => replicsSource?.Invoke().Select(x => new ReplicaLinViewModel(x.Tag, replicsSource)).ToList();

        public ReplicaLinViewModel(int tag, Func<List<Replica>> replicsSource)
        {
            this.Tag = tag;
            this.replicsSource = replicsSource;
            Current = tag;
        }

        public int Current { get; set; }

        public string Name { get => replicsSource?.Invoke().FirstOrDefault(x => x.Tag == this.Tag).Answer ?? "Реплика не выбрана"; set { } }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}