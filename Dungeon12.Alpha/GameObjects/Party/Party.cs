using Dungeon.GameObjects;
using Dungeon12.GameObjects.Character;
using System.Collections.Generic;

namespace Dungeon12.GameObjects.Party
{
    public class Party : GameComponent
    {
        public CharacterEntity CharacterSlot1 { get; set; }

        public CharacterEntity CharacterSlot2 { get; set; }

        public CharacterEntity CharacterSlot3 { get; set; }

        public CharacterEntity CharacterSlot4 { get; set; }

        public List<CharacterEntity> Characters { get; set; }
    }
}
