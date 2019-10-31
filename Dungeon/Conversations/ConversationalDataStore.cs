namespace Dungeon.Conversations
{
    using Dungeon.Data.Region;
    using System.Collections.Generic;

    public class ConversationalDataStore : RegionPart
    {
        public List<string> Conversations { get; set; }
    }
}