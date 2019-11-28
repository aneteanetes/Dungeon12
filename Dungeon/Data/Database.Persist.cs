using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Reflection;
using LiteDB;
using Newtonsoft.Json;
using System.Linq;
using System.Runtime.Loader;
using Dungeon.Resources;
using Dungeon.Classes;
using Dungeon.Map.Objects;
using Dungeon.Types;
using Dungeon.Map;
using Dungeon.View.Interfaces;

namespace Dungeon.Data
{
    public static partial class Database
    {
        public static string Save(int liteDbId = 0, string saveGameName=null)
        {
            using (var db = new LiteDatabase($@"{MainPath}\Saves.db"))
            {
                var map = Global.GameState.Map;
                var avatar = Global.GameState.Player.Component;
                var id = saveGameName ?? $"{DateTime.Now.ToString()}";

                if (liteDbId != 0)
                {
                    id = db.GetCollection<SaveModel>().FindById(liteDbId).IdentifyName;
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
                    Map = new MapSaveModel()
                    {
                        Name = map.MapIdentifyId,
                        Objects = map.SaveableObjects
                    },
                    Underlevels = Global.GameState.Underlevels.Select(lvl => new MapSaveModel()
                    {
                        Name = lvl.MapIdentifyId,
                        Objects = lvl.SaveableObjects
                    }).ToList()
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

                };

                if (liteDbId != 0)
                {
                    db.GetCollection<SaveModel>().Update(liteDbId, saveModel);

                }
                else
                {
                    db.GetCollection<SaveModel>().Insert(saveModel);
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

        public static SaveModel Load(string id)
        {
            return Entity<SaveModel>(x => x.IdentifyName == id,db:"Saves").FirstOrDefault();
        }

        public static IEnumerable<SaveModel> SavedGames()=> Entity<SaveModel>(db: "Saves");

        public static bool RemoveSavedGame(int id)
        {
            using (var db = new LiteDatabase($@"{MainPath}\Saves.db"))
            {
                return db.GetCollection<SaveModel>().Delete(id);
            }
        }
    }

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
    }

    public class SavedGame : Persist
    {
        public GameTime Time { get; set; }

        public CharSaveModel Character { get; set; }

        public MapSaveModel Map { get; set; }

        public List<MapSaveModel> Underlevels { get; set; }
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