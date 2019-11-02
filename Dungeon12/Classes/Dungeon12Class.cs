using Dungeon.Classes;
using Dungeon;

namespace Dungeon12
{
    public abstract class Dungeon12Class : BaseCharacterTileset
    {
        public override string Avatar => GetType().Name.AsmImgRes();
    }
}
