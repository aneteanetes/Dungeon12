using Dungeon.ConversationDesigner.ViewModels;
using Dungeon12.Conversations;
using Dungeon12.Data.Conversations;
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
        public ApplicationViewModel() { }

        public ApplicationViewModel(string path) => Load(path);

        public void Load(string path)
        {
            Conversation = JsonConvert.DeserializeObject<ConversationData>(File.ReadAllText(path));
            Subjects = new ObservableCollection<SubjectViewModel>(Conversation.Subjects.Select(s=>new SubjectViewModel(s)));
        }

        public ConversationData Conversation { get; set; }

        private SubjectViewModel selected;

        public ObservableCollection<SubjectViewModel> Subjects { get; set; }

        public void AddNewSubject()
        {
            var sbj = new Subject();
            sbj.Replics = new System.Collections.Generic.List<Replica>();
            Conversation.Subjects.Add(sbj);
            Subjects.Add(new SubjectViewModel(sbj));
        }      

        public void AddSubjectReplica()
        {
            var r = new Replica();
            Selected.Value.Replics.Add(r);            
            Selected.ReplicsView.Add(new ReplicaViewModel(r, Selected.GetReplics));
        }

        public void RemoveSubjectReplica(ReplicaViewModel replicaViewModel)
        {
            Selected.Value.Replics.Remove(replicaViewModel.Value);
            Selected.ReplicsView.Remove(replicaViewModel);
        }

        public void RemoveSubject(SubjectViewModel subjectViewModel)
        {
            Subjects.Remove(subjectViewModel);
            Conversation.Subjects.Remove(subjectViewModel.Value);
        }

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
        
        public string QuestIdentify { get; set; } = "{QUESTNAME}";

        public void AddQuest()
        {
            string completedVar = $"QuestCompleted_{QuestIdentify}";
            string getVar = $"QuestGet{QuestIdentify}";

            var subj = new Subject()
            {
                Name = "Задание",
                Visible = new Variable() { Name = completedVar },
                Variables = new System.Collections.Generic.List<Variable>()
                {
                    new Variable()
                    {
                        Name = getVar,
                        Value = 44,
                        Global = true
                    }
                },
                Text="Описание задания",
                Replics = new System.Collections.Generic.List<Replica>()
            };

            //взять задание
            subj.Replics.Add(new Replica()
            {
                Shown=true,
                Tag=55,
                Answer="Взять задание",
                Text="Описание выполнения задания",
                ReplicsTags = new System.Collections.Generic.List<int> { 77 },
                Variables=new System.Collections.Generic.List<Variable>()
                {
                    new Variable()
                    {
                        Name=getVar,
                        Value=44,
                        Global=true
                    }
                },
                TriggerClass= "QuestConversationTrigger",
                TriggerArguments=new System.Collections.Generic.List<string>()
                {
                    QuestIdentify
                }
            });

            //отказаться
            subj.Replics.Add(new Replica()
            {
                Shown=true,
                Tag = 66,
                Answer = "Отказаться",
                Text = "Текст отказа от задания",               
            });

            //если согласились, при нажатии на задание:
            subj.Replics.Add(new Replica()
            {
                Tag = 44,
                Answer = QuestIdentify,
                Text = "Текст проверки выполнения задания",
                ReplicsTags=new System.Collections.Generic.List<int>()
                {
                    45,46
                }
            });
            
            //Попытка сдать задание
            subj.Replics.Add(new Replica()
            {
                Tag = 45,
                Answer = "Текст утверждения выполнения задания",
                Text = "",
                ReplicsTags = new System.Collections.Generic.List<int> { 77 },
                TriggerClass = "QuestRewardTryTrigger",
                TriggerArguments = new System.Collections.Generic.List<string>()
                {
                    QuestIdentify,
                    "QuestCompleted",
                    "Текст не выполненного задания",
                    "Текст выполненного задания"
                }
            });

            //Сообщение о том что квест ещё не выполнен
            subj.Replics.Add(new Replica()
            {
                Tag = 46,
                Answer = "Текст утверждения НЕ выполнено",
                Text = "Возвращайся когда всё будет готово!",
                ReplicsTags = new System.Collections.Generic.List<int> { 77 },
            });

            //Назад до тем
            subj.Replics.Add(new Replica()
            {
                Tag = 77,
                Answer = "Назад",
                Escape=true
            });

            Conversation.Subjects.Add(subj);
            Subjects.Add(new SubjectViewModel(subj));
        }
    }
}
