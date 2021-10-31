using Dungeon.Data;

namespace Dungeon.GameObjects
{
    public abstract class GameComponentStored<T> : GameComponent
        where T : Persist
    {
        public T Data { get; protected set; }

        public GameComponentStored(int id)
        {
            Data = Persist.LoadOne<T>(x => x.ObjectId == id);
        }

        public GameComponentStored(string name)
        {
            Data = Persist.LoadOne<T>(x => x.IdentifyName == name);
        }
    }
}
