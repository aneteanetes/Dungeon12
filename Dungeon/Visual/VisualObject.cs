using Dungeon.Network;
using Dungeon.View.Interfaces;

namespace Dungeon
{
    public class VisualObject : GameNetObject, IVisual
    {
        public virtual ISceneObject Visual() => default;
    }
}