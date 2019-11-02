using Dungeon.Classes;
using Dungeon;
using Dungeon12.Entites.Journal;

namespace Dungeon12
{
    public abstract class Dungeon12Class : BaseCharacterTileset
    {
        public override string Avatar => GetType().Name.AsmImgRes();
               
        public Journal Journal { get; set; } = new Journal();
    }
}
