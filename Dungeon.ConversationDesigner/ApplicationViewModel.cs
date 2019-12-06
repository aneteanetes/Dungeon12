using Dungeon.Conversations;
using Dungeon.Data.Conversations;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace Dungeon.ConversationDesigner
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        public ApplicationViewModel(string path) => Load(path);

        public void Load(string path)
        {
            Conversation = JsonConvert.DeserializeObject<ConversationData>(File.ReadAllText(path));
            Subjects = new ObservableCollection<Subject>(Conversation.Subjects);
        }

        public ConversationData Conversation { get; set; }

        private Subject selected;

        public ObservableCollection<Subject> Subjects { get; set; }

        public Subject Selected
        {
            get { return selected; }
            set
            {
                selected = value;
                OnPropertyChanged("Selected");
            }
        }
        
        public ObservableCollection<Replica> Replics { get; set; }
        
        private Replica selectedReplica;
        public Replica SelectedReplica
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
