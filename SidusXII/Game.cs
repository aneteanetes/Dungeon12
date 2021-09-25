using SidusXII.Characters;
using SidusXII.Objects.Map;

namespace SidusXII
{
    public class Game
    {
        public Character Character { get; set; } = new Character();

        public Character PartyMember1 { get; set; }

        public Character PartyMember2 { get; set; }

        public LocationMap Location { get; set; }
    }
}
