using Dungeon.Game;
using Dungeon.Network;
using Dungeon.View.Interfaces;

namespace Dungeon
{
    public class Visual : NetObject, IVisual
    {
        public virtual ISceneObject Visual(GameState gameState) => default;
    }
}