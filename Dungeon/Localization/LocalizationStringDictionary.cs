using Dungeon.Control.Keys;
using Dungeon.Drawing;
using Dungeon.View.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Dungeon.Localization
{
    public class LocalizationStringDictionary
    {
        public LocalizationStringDictionary(string defaultLanguageCode = "ru")
        {
            this.GetType().GetProperties().ForEach(p =>
            {
                if (p.PropertyType == typeof(LocalizationStringSubDictionary))
                    this.SetProperty(p.Name, new LocalizationStringSubDictionary(this, p.Name));
            });
        }

        public virtual string ___DefaultLanguageCode { get; }

        public Dictionary<string, string> Values { get; set; } = new Dictionary<string, string>();

        public LocalizationStringDictionaryChain this[string @const]
        {
            get
            {
                return new LocalizationStringDictionaryChain(@const, this);
            }
        }

        public LocalizationStringDictionaryChain this[object @const]
        {
            get
            {
                string key = @const.ToString();
                var type = @const.GetType();

                if (type.IsEnum)
                {
                    key = $"{type.Name}.{key}";
                }

                return new LocalizationStringDictionaryChain(key, this);
            }
        }

        public string GetValueEnumPrefix(string prefix, object enumValue)
        {
            string key = enumValue.ToString();
            var type = enumValue.GetType();

            if (type.IsEnum)
            {
                key = $"{type.Name}.{key}";
            }

            return GetValueInternal(prefix + key);
        }

        internal string GetValueInternal(string key)
        {
            if (!Values.TryGetValue(key.ToLowerInvariant(), out var value))
                return $"LOCALE-STRING-NOT-FOUND: {key}";

            if (value.StartsWith("{") && value.EndsWith("}"))
            {
                if (!Values.TryGetValue(value.Replace("{", "").Replace("}", ""), out value))
                    return $"WRONG-LINK: [{key}:{value}]";
            }

            return value;
        }

        public T ___Load<T>(string lang)
        {
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(DoPath(lang)));
        }

        protected string DoPath(string lang)
        {
            var root = DungeonGlobal.BuildLocation;
            var path = Path.Combine(root, DungeonGlobal.Configuration.LocaleDirectory, lang + ".json");

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
                Values = Resources.ResourceLoader.LoadLocale(lang);
                return Values;
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

    public class LocalizationStringDictionaryChain
    {
        LocalizationStringDictionary _dict;

        internal string KeyChain { get; set; }

        public LocalizationStringDictionaryChain(string key, LocalizationStringDictionary dict)
        {
            _dict = dict;
            ExpandChain(key);
        }

        private void ExpandChain(string key)
        {
            if (KeyChain.IsNotEmpty())
            {
                KeyChain += "." + key;
            }
            else
                KeyChain = key;
        }

        public LocalizationStringDictionaryChain this[string @const]
        {
            get
            {
                ExpandChain(@const);
                return this;
            }
        }

        public LocalizationStringDictionaryChain this[object @const]
        {
            get
            {
                string key = @const.ToString();
                var type = @const.GetType();

                if (type.IsEnum)
                {
                    key = $"{type.Name}.{key}";
                }

                ExpandChain(key);
                return this;
            }
        }

        public static implicit operator string(LocalizationStringDictionaryChain chain)
        {
            return chain.ToString();
        }

        public static implicit operator DrawText(LocalizationStringDictionaryChain chain)
        {
            return chain.AsDrawText();
        }

        public DrawText AsDrawText() => this.ToString().AsDrawText();

        public override string ToString()
        {
            return this._dict.GetValueInternal(KeyChain);
        }
    }
}