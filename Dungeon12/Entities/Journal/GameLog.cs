namespace Dungeon12.Entities.Journal
{
    internal class GameLog
    {
        public List<GameLogMessage> Records { get; set; } = new();

        public void Push(string text)
        {
            var record = new GameLogMessage()
            {
                Time=Global.Game.Calendar.TimeText(),
                Message = text
            };
            Records.Add(record);
            OnPush?.Invoke(record);
        }

        public Action<GameLogMessage> OnPush { get; set; } = x => { };
    }
}
