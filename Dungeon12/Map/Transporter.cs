using Dungeon;
using Dungeon.Data.Attributes;
using Dungeon.Data.Region;
using Dungeon.Game;
using Dungeon.Map;
using Dungeon.Map.Infrastructure;
using Dungeon.Map.Objects;
using Dungeon.Scenes.Manager;
using Dungeon.View.Interfaces;
using Dungeon12.Data;
using Dungeon12.Scenes.Game;
using System.Linq;

namespace Dungeon12.Map
{
    [Template("Respawn")]
    [DataClass(typeof(TransporterData))]
    public class Transporter : MapObject
    {
        public override bool Saveable => true;

        public TransporterData Data { get; set; }

        protected override void Load(RegionPart regionPart)
        {
            base.Load(regionPart);

            this.Data = regionPart.As<TransporterData>();
        }

        public override ISceneObject Visual(GameState gameState)
        {
            return base.Visual(gameState);
        }

        public override bool Interactable => true;

        public void Interact(Avatar player)
        {
            // уничтожаем что у нас там есть сейчас в игре
            SceneManager.Destroy<Scenes.Game.Main>();

            var isUnderLevel = Data.UnderlevelIdentify != default;
            if (isUnderLevel)
            {
                if (!Global.GameState.Map.IsUnderLevel)
                {
                    // если мы сейчас не в под-уровне сохраняем карту региона в память
                    Global.GameState.Region = Global.GameState.Map;
                }

                // сохраняем текущее состояние в памяти
                Global.SaveInMemmory();

                // объекты не на текущей карте должны перестать "двигаться"
                Global.GameState.Map.OnMoving = default;

                var inMemmoryUnderlevel = Global.GameState.Underlevels.FirstOrDefault(x => x.MapIdentifyId == Data.UnderlevelIdentify);
                if (inMemmoryUnderlevel != default)
                {
                    // загружаем под уровень из памяти если он там есть
                    Global.GameState.Map = inMemmoryUnderlevel;
                }
                else // если под уровня в памяти нет
                {
                    Global.GameState.Map = new GameMap();
                    Global.GameState.Map.InitRegion(Data.UnderlevelIdentify);

                    // добавляем под уровень в текущую память
                    Global.GameState.Underlevels.Add(Global.GameState.Map);
                }
            }
            else
            {
                var region = Data.RegionIdentify;
                if (Global.GameState.Region.MapIdentifyId == region)
                {
                    // если мы входим в текущий регион, значит мы выходим из под-уровня
                    // соответственно можем просто восстановить карту
                    Global.GameState.Map = Global.GameState.Region;

                    // под-уровень не сохраняем, т.к. он есть в коллекции под-уровней
                }
                else
                {
#warning TODO: реализовать когда появятся другие регионы
                }
            }

            // устанавливаем персонажа
            // внутри установится и камера
            Global.GameState.Map.SetPlayerLocation(Data.Destination);

            // переключаемся на главную сцену
            SceneManager.Switch<Main>();
        }

        protected override void CallInteract(dynamic obj) => this.Interact(obj);
    }
}