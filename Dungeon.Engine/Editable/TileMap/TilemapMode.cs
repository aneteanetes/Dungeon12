using Dungeon.Utils;
using LiteDB;
using System.Collections.ObjectModel;

namespace Dungeon.Engine.Editable.TileMap
{
    public class TilemapMode
    {
        public TilemapMode()
        {

        }

        public TilemapMode(string name, TilemapModeEnum @enum)
        {
            Name = name;
            Mode = @enum;
        }

        public string Name { get; set; }

        [Equality]
        public TilemapModeEnum Mode { get; set; }

        public readonly static TilemapMode Usuall = new TilemapMode("Обычный", TilemapModeEnum.Usuall);
        public readonly static TilemapMode Isometric = new TilemapMode("Изометрический", TilemapModeEnum.Isometric);

        [BsonIgnore]
        public static ObservableCollection<TilemapMode> Enum => new ObservableCollection<TilemapMode>()
        {
            Usuall,
            Isometric
        };
    }

    public enum TilemapModeEnum
    {
        Usuall = 0,
        Isometric = 1
    }
}
