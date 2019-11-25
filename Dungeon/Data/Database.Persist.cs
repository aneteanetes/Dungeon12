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
        public static string Save()
        {
            var avatar = Global.GameState.Player.Component;
            var id = $"{avatar.Entity.Name}`{DateTime.Now.ToString()}";

            var save = new SavedGame() { Time = Global.Time, Character = new CharSaveModel()
            {
                Character=avatar.Entity,
                Location=avatar.Location
            }, IdentifyName = id };

            var saveModel = new SaveModel()
            {
                RegionName = Global.GameState.Map.Name,
                CharacterName = avatar.Entity.Name,
                IdentifyName = id,
                ClassName = avatar.Entity.ClassName,
                Level = avatar.Entity.Level,
                Name = id,
                Data = JsonConvert.SerializeObject(save, new JsonSerializerSettings()
                {
                    Converters = new IgnoreConverter().InList<JsonConverter>(),
                    ContractResolver = new WritablePropertiesOnlyResolver()
                })
            };

            using (var db = new LiteDatabase($@"{MainPath}\Data.db"))
            {
                db.GetCollection<SaveModel>().Insert(saveModel);
            }

            return id;
        }

        public static SavedGame Load(string id)
        {
            return Entity<SavedGame>(x => x.IdentifyName == id).FirstOrDefault();
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
                || typeof(MapObject).IsAssignableFrom(objectType) 
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
        public string Name { get; set; }

        public string RegionName { get; set; }

        public string CharacterName { get; set; }

        public string ClassName { get; set; }

        public int Level { get; set; }

        public string Data { get; set; }
    }

    public class SavedGame : Persist
    {
        public GameTime Time { get; set; }

        public CharSaveModel Character { get; set; }
    }

    public class CharSaveModel
    {
        public Character Character { get; set; }

        public Point Location { get; set; }
    }
}