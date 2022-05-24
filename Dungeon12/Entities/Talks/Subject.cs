using System;

namespace Dungeon12.Entities.Talks
{
    internal class Subject
    {
        public string SubjectId => Dialogue.Id + Name;

        public Dialogue Dialogue { get; set; }

        public string Name { get; set; }

        public Replica Replica { get; set; }

        public Action End { get; set; }
    }
}