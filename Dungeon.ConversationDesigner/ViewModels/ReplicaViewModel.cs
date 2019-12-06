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

        public ReplicaViewModel(Replica repl)
        {
            Value = repl;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
