using Dungeon;
using Dungeon.Data;
using Dungeon.Localization;
using Dungeon.SceneObjects;
using Dungeon.Types;
using Dungeon.View.Interfaces;
using Dungeon12.Classes;
using Dungeon12.Game;
using Dungeon12.Items;
using Dungeon12.Localization;
using Dungeon12.Map;
using LiteDB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon12
{
    public class Global : DungeonGlobal
    {
        public Global()
        {
            DefaultFontName = "Triforce(RUS BY LYAJKA)";
            DefaultFontSize = 30;
            BuildLocation = @"C:\Users\anete\source\repos\Dungeon12\Dungeon12\bin\Debug\netcoreapp3.1\";
        }

        public static GameState GameState { get; set; } = new GameState();

        protected override void OnException(Exception ex, Action ok = null)
        {
            try
            {
                Save(0, "Автосохранение",true);
                MessageBox.Show($"Ошибка!{Environment.NewLine}Игра сохранена, выйдите и загрузите игру снова!", ok);
            }
            catch (Exception ex1)
            {
                Logger.Log(ex1.ToString());
                MessageBox.Show($"Ошибка!{Environment.NewLine} Игра НЕ СОХРАНЕНА, выйдите и загрузите игру снова!", ok);
            }
        }

        /// <summary>
        /// Сохраняет текущий регион в память
        /// </summary>
        public static void SaveInMemmory() => Save(0, "@!#$memory$#!@", true, true);

        /// <summary>
        /// Удаляет текущую карту из памяти
        /// </summary>
        public static void RemoveSaveInMemmory()
        {
            var temp = Load("@!#$memory$#!@");
            if (temp != default)
                RemoveSavedGame(Load("@!#$memory$#!@").Id);
        }

        public static SaveModel Load(string id) => LoadSaveModel(id);
        
        public static string Save(int liteStoreId = 0, string saveGameName = null, bool overrideExistedName = false, bool hiddenSave = false)
        {
            using (var lStore = new LiteDatabase($@"{Store.MainPath}\Saves.db"))
            {
                var map = Global.GameState.Map;
                var region = Global.GameState.Region;
                var avatar = Global.GameState.Player.Avatar;
                var id = saveGameName ?? $"{DateTime.Now.ToString()}";

                if (liteStoreId != 0)
                {
                    id = lStore.GetCollection<SaveModel>().FindById(liteStoreId).IdentifyName;
                }

                var save = new SavedGame()
                {
                    Time = Global.Time,
                    Character = new CharSaveModel()
                    {
                        Character = avatar.Entity,
                        Location = avatar.Location
                    },
                    IdentifyName = id,
                    Region = new MapSaveModel()
                    {
                        Name = region?.MapIdentifyId,
                        Objects = region?.SaveableObjects ?? new HashSet<MapObject>()
                    },
                    Map = new MapSaveModel()
                    {
                        Name = map.MapIdentifyId,
                        Objects = map.SaveableObjects
                    },
                    Underlevels = Global.GameState.Underlevels.Select(lvl => new MapSaveModel()
                    {
                        Name = lvl.MapIdentifyId,
                        Objects = lvl.SaveableObjects
                    }).ToList(),
                    EquipmentState = Global.GameState.Equipment,
                    MapDeferredOptions = GameMap.DeferredMapObjects
                };

                var camera = Global.DrawClient as ICamera;

                var saveModel = new SaveModel()
                {
                    GameTime = $"{save.Time.Hours}:{save.Time.Minutes} [{save.Time.Years} год месяца Зимы]",
                    ScreenPosition = avatar.SceenPosition,
                    RegionName = Global.GameState.Map.Name,
                    CharacterName = avatar.Entity.Name,
                    IdentifyName = id,
                    ClassName = avatar.Entity.ClassName,
                    Level = avatar.Entity.Level,
                    Name = id,
                    CameraOffset = new Point(camera.CameraOffsetX, camera.CameraOffsetY),
                    Data = JsonConvert.SerializeObject(save, GetSaveSerializeSettings()),
                    Hidden = hiddenSave,
                    RestorableRespawns = Global.GameState.RestorableRespawns
                };

                if (liteStoreId != 0)
                {
                    lStore.GetCollection<SaveModel>().Update(liteStoreId, saveModel);

                }
                else if (saveGameName != default && overrideExistedName)
                {
                    var getCurrent = Store.Entity<SaveModel>(x => x.IdentifyName == saveGameName).FirstOrDefault();
                    if (getCurrent != default)
                    {
                        lStore.GetCollection<SaveModel>().Update(getCurrent.Id, saveModel);
                    }
                    else
                    {
                        lStore.GetCollection<SaveModel>().Insert(saveModel);
                    }
                }
                else
                {
                    lStore.GetCollection<SaveModel>().Insert(saveModel);
                }

                return id;
            }
        }

        public static JsonSerializerSettings GetSaveSerializeSettings()
        {
            return new JsonSerializerSettings()
            {
                Converters = new IgnoreConverter().InList<JsonConverter>(),
                ContractResolver = new WritablePropertiesOnlyResolver(),
                TypeNameHandling = TypeNameHandling.Auto
            };
        }

        public static SaveModel LoadSaveModel(string id)
        {
            return Store.Entity<SaveModel>(x => x.IdentifyName == id, db: "Saves").FirstOrDefault();
        }

        public static IEnumerable<SaveModel> SavedGames() => Store.Entity<SaveModel>(db: "Saves").Where(x => !x.Hidden);

        public static bool RemoveSavedGame(int id)
        {
            using (var lStore = new LiteDatabase($@"{Store.MainPath}\Saves.db"))
            {
                return lStore.GetCollection<SaveModel>().Delete(id);
            }
        }

        public static GameStrings Strings { get; set; } = new GameStrings();

        public override LocalizationStringDictionary GetStringsClass() => Strings;

        public override void LoadStrings(object localizationStringDictionary) => Strings = localizationStringDictionary.As<GameStrings>();

        public class WritablePropertiesOnlyResolver : Newtonsoft.Json.Serialization.DefaultContractResolver
        {
            protected override IList<Newtonsoft.Json.Serialization.JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
            {
                IList<Newtonsoft.Json.Serialization.JsonProperty> props = base.CreateProperties(type, memberSerialization);
                return props.Where(p => p.Writable).ToList();
            }
        }

        public class IgnoreConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return typeof(Delegate).IsAssignableFrom(objectType)
                    || typeof(GameMap).IsAssignableFrom(objectType)
                    || typeof(ISceneObject).IsAssignableFrom(objectType);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
            {
                return null;
            }

            public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
            {
                writer.WriteNull();
            }
        }

        public class SaveModel : Persist
        {
            public string GameTime { get; set; }

            public Point ScreenPosition { get; set; }

            public string Name { get; set; }

            public string RegionName { get; set; }

            public string CharacterName { get; set; }

            public string ClassName { get; set; }

            public Point CameraOffset { get; set; }

            public int Level { get; set; }

            public string Data { get; set; }

            public bool Hidden { get; set; }

            public List<string> RestorableRespawns { get; set; }
        }

        public class SavedGame : Persist
        {
            public GameTime Time { get; set; }

            public CharSaveModel Character { get; set; }

            public MapSaveModel Map { get; set; }

            public MapSaveModel Region { get; set; }

            public List<MapSaveModel> Underlevels { get; set; }

            public EquipmentState EquipmentState { get; set; }

            public List<MapDeferredOptions> MapDeferredOptions { get; set; }
        }

        public class MapSaveModel
        {
            public string Name { get; set; }

            public HashSet<MapObject> Objects { get; set; }
        }

        public class CharSaveModel
        {
            public Character Character { get; set; }

            public Point Location { get; set; }
        }
    }
}
