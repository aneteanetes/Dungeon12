namespace Dungeon12.Data
{
    using Dungeon.Data;
    using System.Collections.Generic;

    public class ConversationalDataStore : Dungeon.Data.Persist
    {
        public List<string> Conversations { get; set; }
    }
}