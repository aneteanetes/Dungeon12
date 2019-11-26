using Dungeon.Classes;
using Dungeon.Drawing.SceneObjects.Map;
using Dungeon.Map;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon.Game
{
    public class GameState
    {
        public GameMap Map { get; set; }

        private PlayerSceneObject _player { get; set; }
        public PlayerSceneObject Player
        {
            get => _player;
            set
            {
                _player = value;
                Character = _player.Component.Entity;
            }
        }

        /// <summary>
        /// Потому что при загрузке например персонаж быть может, а его представление - нет
        /// </summary>
        public Character Character { get; set; }
    }
}
