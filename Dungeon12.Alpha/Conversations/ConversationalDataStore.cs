namespace Dungeon12.Conversations
{
    using Dungeon12.Data.Region;
    using System.Collections.Generic;

    public class ConversationalDataStore : RegionPart
    {
        public List<string> Conversations { get; set; }
    }
}