namespace Dungeon.Data.Homes
{
    using System.Collections.Generic;

    public class HomeData : ConversationalDataStore
    {
        public string IdentifyName { get; set; }

        public string Name { get; set; }

        public string ScreenImage { get; set; }

        public int Frames { get; set; }

        public bool Merchant { get; set; }
    }
}