namespace Dungeon12.Entities.Talks
{
    public class Replica
    {
        public Subject Subject { get; set; }

        public string Text { get; set; }

        public ReplicaLine[] Lines { get; set; }

        public bool End { get; set; }

        public string Goal { get; set; }
    }
}