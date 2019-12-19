using Dungeon;
using Dungeon.Data.Attributes;
using Dungeon12.Data.Region;
using Dungeon12.Game;
using Dungeon12.Map;
using Dungeon12.Map.Infrastructure;
using Dungeon12.Map.Objects;
using Dungeon.Scenes.Manager;
using Dungeon.View.Interfaces;
using Dungeon12.Data;
using Dungeon12.SceneObjects.Map;
using Dungeon12.Scenes.Game;
using System.Linq;

namespace Dungeon12.Map
{
    [Template("Teleport")]
    [DataClass(typeof(TransporterData))]
    public class Transporter : MapObject
    {
        public override bool Saveable => true;

        public TransporterData Data { get; set; }

        public override string Name => Data.Name;

        protected override void Load(RegionPart regionPart)
        {
            base.Load(regionPart);            
            this.Data = regionPart.As<TransporterData>();
        }

        public override ISceneObject Visual()
        {
            return new TransporterSceneObject(Global.GameState.Player, this);
        }

        public override bool Interactable => true;

        public void Interact(Avatar player)
        {
            Global.GameState.Player.StopMovings();

            SceneManager.LoadingScreenCustom(Data.LoadingScreenName ?? "FaithIsland").Then(cb => {
                
                // уничтожаем что у нас там есть сейчас в игре
                SceneManager.Destroy<Scenes.Game.Main>();

                // замораживаем рисование пресонажа
                Global.GameState.Player.FreezeDrawLoop = true;

                //удаляем нахой персонажа с карты
                Global.GameState.Map.MapObject.Remove(player);

                // разъёбываем нахуй всё что бы сцена точно удалилась
                Global.DrawClient.SetScene(default);

                // объекты не на текущей карте должны перестать "двигаться"
                // замораживаем движение на текущей карте
                Global.GameState.Map.Disabled = true;

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

                // восстанавливаем движение на именно текущей карте
                Global.GameState.Map.Disabled = false;

                // указываем что карта загружена что бы она не инициализировалась "по-умолчанию"
                Global.GameState.Map.Loaded = true;

                // устанавливаем персонажа
                // внутри установится и камера
                Global.GameState.Map.SetPlayerLocation(Data.Destination);

                Global.DrawClient.SetCursor("Cursors.common.png".PathImage());

                // переключаемся на главную сцену
                SceneManager.Switch<Main>();

                cb.Dispose();
            });
        }

        protected override void CallInteract(dynamic obj) => this.Interact(obj);
    }
}