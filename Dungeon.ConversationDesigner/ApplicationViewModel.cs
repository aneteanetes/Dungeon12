using Dungeon.ConversationDesigner.ViewModels;
using Dungeon.Conversations;
using Dungeon.Data.Conversations;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Dungeon.ConversationDesigner
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        public ApplicationViewModel(string path) => Load(path);

        public void Load(string path)
        {
            Conversation = JsonConvert.DeserializeObject<ConversationData>(File.ReadAllText(path));
            Subjects = new ObservableCollection<SubjectViewModel>(Conversation.Subjects.Select(s=>new SubjectViewModel(s)));
        }

        public ConversationData Conversation { get; set; }

        private SubjectViewModel selected;

        public ObservableCollection<SubjectViewModel> Subjects { get; set; }

        public SubjectViewModel Selected
        {
            get { return selected; }
            set
            {
                selected = value;
                OnPropertyChanged("Selected");
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
