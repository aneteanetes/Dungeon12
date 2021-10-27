using Dungeon;
using Dungeon.GameObjects;
using Dungeon12.GameObjects.Character;
using Dungeon12.GameObjects.Party;
using Dungeon12.World.Map;

namespace Dungeon12.World
{
    public class GameWorld : GameComponent
    {
        public GameWorld()
        {

        }

        public WorldMap WorldMap { get; set; }

        public Party Party { get; set; }

        public WorldMap CreateWorldMap(string regionName)
        {
            return WorldMap = new WorldMap(regionName);            
        }

        public Party CreateParty()
        {
            return Party = new Party()
            {
                CharacterSlot1 = new CharacterEntity(
                    new Dungeon.Types.Point(70, 70),
                    "Dolls.Childs.Girl1.normal.png".AsmImg(),
                    "Dolls.Childs.Girl1.axis.png".AsmImg())
            };
        }
    }
}