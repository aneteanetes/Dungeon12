namespace Rogue.Data
{
    using System.Collections.Generic;

    public class ConversationalDataStore : Persist
    {
        public List<string> Conversations { get; set; }
    }
}