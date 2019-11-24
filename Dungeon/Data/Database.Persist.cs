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

namespace Dungeon.Data
{
    public static partial class Database
    {
        public static string Save()
        {
            var character = Global.GameState.Player.Component;
            var id = $"{character.Entity.Name}`{DateTime.Now.ToString()}";
            var save = new SavedGame() { Time = Global.Time, Character = character, IdentifyName = id };

            using (var db = new LiteDatabase($@"{MainPath}\Data.db"))
            {
                db.GetCollection<SavedGame>().Insert(save);
            }

            return id;
        }

        public static SavedGame Load(string id)
        {
            return Entity<SavedGame>(x => x.IdentifyName == id).FirstOrDefault();
        }
    }

    public class SavedGame : Persist
    {
        public GameTime Time { get; set; }

        public Avatar Character { get; set; }
    }
}