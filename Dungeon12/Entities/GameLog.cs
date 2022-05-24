using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Entities
{
    internal class GameLog
    {
        public List<string> Records { get; set; } = new List<string>();

        public void Push(string text)
        {
            var record = $"{Global.Game.Calendar.TimeText()}: {text}";
            Records.Add(record);
            OnPush?.Invoke(record);
        }

        public Action<string> OnPush { get; set; }
    }
}
