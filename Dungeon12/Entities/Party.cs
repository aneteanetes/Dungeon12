using Dungeon12.Entities.Map;

namespace Dungeon12.Entities
{
    public class Party
    {
        public Hero Hero1 { get; set; }

        public Hero Hero2 { get; set; }

        public Hero Hero3 { get; set; }

        public Hero Hero4 { get; set; }

        public int Food { get; set; }

        public void Move(Location location)
        {
            Global.Game.Location = location;
        }
    }
}
