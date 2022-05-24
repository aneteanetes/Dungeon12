using Dungeon;

namespace Dungeon12.Entities.Talks
{
    internal class Dialogue
    {
        public string Id { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Должность
        /// </summary>
        public string Position => Name.Split('*')[0];

        /// <summary>
        /// Имя
        /// </summary>
        public string Personaly => Name.Split('*')[1];

        public string Avatar { get; set; }

        public Subject[] Subjects { get; set; }

        public Goal[] Goals { get; set; }

        public void BindLinks()
        {
            foreach (var subj in Subjects)
            {
                subj.Dialogue = this;

                BindLineLinks(subj.Replica, subj);                
            }
        }

        private void BindLineLinks(Replica repl, Subject subj)
        {
            if (repl != default)
            {
                repl.Subject = subj;

                if (repl.Lines.IsNotEmpty())
                {
                    foreach (var line in repl.Lines)
                    {
                        BindLineLinks(line.Replica, subj);
                    }
                }
            }
        }
    }
}