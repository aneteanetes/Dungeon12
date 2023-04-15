namespace Dungeon12.Entities.Journal
{
    internal class GameLogMessage
    {
        public string Time { get; set; }

        public string Message { get; set; }

        public override string ToString() => $"{Time}: {Message}";
    }
}
