namespace Dungeon.Questing
{
    using Dungeon.Classes;
    using Dungeon.Entites.Journal;
    using Dungeon.Events.Events.Gameplay;
    using Dungeon.Network;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Quest : JournalEntry
    {
        private Character _character;

        public Quest(Character character)
        {
            _character = character;
            Global.Events.Subscribe<QuestDoneEvent, Quest>(DoneQuest, this.ProxyId);
        }

        public override string ProxyId => _character.ProxyId + this.Name;

        public void Check()
        {

        }

        private void DoneQuest(object quest, string[] args)
        {
        }
    }
}