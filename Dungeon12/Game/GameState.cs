using Dungeon12.Classes;
using Dungeon12.Drawing.SceneObjects.Map;
using Dungeon12.Items;
using Dungeon12.Map;
using Dungeon12.Map.Objects;
using System.Collections.Generic;

namespace Dungeon12.Game
{
    public class GameState
    {
        public Avatar PlayerAvatar { get; set; }

        public GameMap Map { get; set; }

        public GameMap Region { get; set; }

        public List<GameMap> Underlevels { get; set; } = new List<GameMap>();

        private PlayerSceneObject _player { get; set; }
        public PlayerSceneObject Player
        {
            get => _player;
            set
            {
                _player = value;
                Character = _player?.Component?.Entity;
            }
        }

        /// <summary>
        /// Потому что при загрузке например персонаж быть может, а его представление - нет
        /// </summary>
        public Character Character { get; set; }

        public void Reset()
        {
            this.Map = default;
            this.Player = default;
        }

        public EquipmentState Equipment { get; set; } = new EquipmentState();
    }
}
