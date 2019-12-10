namespace Dungeon12.Data.Homes
{
    using Dungeon.Physics;
    using Dungeon.Types;
    using Dungeon12.Conversations;

    public class HomeData : ConversationalDataStore
    {
        public string Name { get; set; }

        public string ScreenImage { get; set; }

        public int Frames { get; set; }

        public bool Merchant { get; set; }

        public string FractionIdentity { get; set; }
    }
}