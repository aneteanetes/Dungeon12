using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Dungeon.Localization
{
    public abstract class LocalizationStringDictionary
    {
        public LocalizationStringDictionary()
        {
            this.GetType().GetProperties().ForEach(p =>
            {
                if (p.PropertyType == typeof(LocalizationStringSubDictionary))
                    this.SetProperty(p.Name, new LocalizationStringSubDictionary(this, p.Name));
            });
        }

        public abstract string ___RelativeLocalizationFilesPath { get; }

        public abstract string ___DefaultLanguageCode { get; }

        public Dictionary<string, string> Values { get; set; } = new Dictionary<string, string>();

        public string this[string @const]
        {
            get
            {
                if (!Values.TryGetValue(@const, out var value))
                    return $"LOCALE-STRING-NOT-FOUND: {@const}";
                return value;
            }
        }

        public string this[object @const]
        {
            get
            {
                string key = @const.ToString();
                var type = @const.GetType();

                if (type.IsEnum)
                {
                    key=$"{type.Name}.{key}";
                }

                if (!Values.TryGetValue(key, out var value))
                    return $"LOCALE-ENUM-STRING-NOT-FOUND: {key}";
                return value;
            }
        }

        public T ___Load<T>(string lang)
        {
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(DoPath(lang)));
        }

        protected string DoPath(string lang)
        {
            var root = DungeonGlobal.BuildLocation;
            var path = Path.Combine(root, ___RelativeLocalizationFilesPath, lang + ".json");

            var dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return path;
        }

        public object ___AutoLoad(string lang = default, LocalizationStringDictionary strings = default)
        {
            if (lang == default)
                return default;

            try
            {
                Values = Resources.ResourceLoader.LoadJson<Dictionary<string, string>>($"Locales/{lang}.json".AsmRes(this.GetType().Assembly), @throw: false);
                if (DungeonGlobal.IsDevelopment)
                    return Values;


                if (Values==default)
                    Values=new Dictionary<string, string>();

                var path = DoPath(lang);
                if (!File.Exists(path) && strings != default)
                {
                    File.WriteAllText(path, JsonConvert.SerializeObject(Values, Formatting.Indented));
                    return strings;
                }

                var json = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(DoPath(lang)));
                Values=json;
                return this;
            }
            catch (FileNotFoundException)
            {
                return default;
            }
        }

        public void ___Save(string lang)
        {
            File.WriteAllText(DoPath(lang), JsonConvert.SerializeObject(this));
        }

        public List<LocalizedString> DynamicStrings { get; set; }
    }
}