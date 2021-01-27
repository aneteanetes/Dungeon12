using LiteDB;
using Newtonsoft.Json;
using System.Linq;

namespace Dungeon.Localization
{
    public class LocalizedString
    {
        public string Code { get; set; }

        public string Lang { get; set; }

        public string Value { get; set; }

        private string value;

        [JsonIgnore]
        [BsonIgnore]
        public string LocalizedValue
        {
            get
            {
                if (DungeonGlobal.CultureInfo == default || Lang == DungeonGlobal.CultureInfo.TwoLetterISOLanguageName)
                {
                    value = Value;
                }
                else if (Lang != DungeonGlobal.CultureInfo?.TwoLetterISOLanguageName)
                {
                    var glob = DungeonGlobal.GetBindedGlobal();
                    if (glob != default)
                    {
                        var localizedString = glob.GetStringsClass().DynamicStrings.FirstOrDefault(s => s.Code == this.Code);
                        if (localizedString != null)
                        {
                            this.value = localizedString.Value;
                        }
                    }
                }

                return value;
            }
            set => this.value = value;
        }

        public static implicit operator string(LocalizedString localizedString) => localizedString.LocalizedValue;

        public static implicit operator LocalizedString(string @string)=>new LocalizedString()
        {
            Value=@string
        };
    }
}