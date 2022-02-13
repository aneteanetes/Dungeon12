namespace Dungeon12.Entities.Talks
{
    public class Dialogue
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Avatar { get; set; }

        public Subject[] Subjects { get; set; }

        public Goal[] Goals { get; set; }
    }
}