using Dungeon;
using Dungeon12.Entities.Turning;

namespace Dungeon12.Entities.Objects
{
    internal class GameObject
    {
        public GameObject()
        {
            if (Global.Game!=default)
                globalId=Global.Game.State.GlobalId++;
        }

        private ulong globalId;

        /// <summary>
        /// possible отъебёт
        /// </summary>
        public virtual ulong GlobalId
        {
            get
            {
                if (globalId == default)
                    globalId=Global.Game.State.GlobalId++;

                return globalId;
            }
            set
            {
                globalId = value;
            }
        }

        public virtual string Name { get; set; }

        /// <summary>
        /// Absolute path
        /// </summary>
        public virtual string Image { get; set; }

        public virtual string Chip { get; set; }

        public Value Initiative { get; set; } = new Value(1);

        public virtual GameObjectType GameType => GameObjectType.Object;

        public virtual IEnumerable<Turn> GetTurns() => new Turn(this) { Initiative=(int)this.Initiative.FlatValue }.InEnumerable();

        public override string ToString()=> GlobalId.ToString();

        /// <summary>
        /// skip available
        /// </summary>
        public virtual TurnType DoTurn() => TurnType.Next;
    }
}