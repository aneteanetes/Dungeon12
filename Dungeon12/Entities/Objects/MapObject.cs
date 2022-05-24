using Dungeon.GameObjects;
using Dungeon12.Entities.Enums;

namespace Dungeon12.Entities.Objects
{
    internal class MapObject : GameComponent
    {
        public string Id { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public ObjectType Type { get; set; }

        private string _icon;
        public string Icon
        {
            get
            {
                if (_icon==null)
                {
                    _icon=BindIcon();
                }

                return _icon;
            }
            set => _icon=value;
        }

        private string BindIcon() => Type switch
        {
            ObjectType.Barrel => "Objects/barrel",
            ObjectType.Note => "Objects/note",
            ObjectType.NPC => "NPCs/f_01",
            ObjectType.Chest => "Objects/chest",
            _ => "Objects/cube",
        };
    }
}