using Dungeon;

namespace Nabunassar.Entities.Objects
{
    internal class GameObject
    {
        public GameObject()
        {
            if (Global.Game!=default)
                globalId=Global.Game.Vars.GlobalId++;
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
                    globalId=Global.Game.Vars.GlobalId++;

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

        /// <summary>
        /// resource path
        /// </summary>
        public virtual string Chip { get; set; }

        public virtual GameObjectType GameType => GameObjectType.Object;


        public override string ToString()=> GlobalId.ToString();
    }
}