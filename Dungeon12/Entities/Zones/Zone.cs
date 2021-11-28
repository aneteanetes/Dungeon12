using Dungeon12.Entities.Perks;

namespace Dungeon12.Entities.Zones
{
    public class Zone
    {
        public string ObjectId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool Selectable { get; set; }

        public Perk Perk { get; set; }
    }

    public enum Zones
    {
        Braveedge,
        Darkwood,
        Deadplains,
        Eternalgrove,
        Faithisland,
        Falconwatch,
        Overland,
        Silverdale,
        Snowland,
        Ushal,
        Voidbay
    }
}