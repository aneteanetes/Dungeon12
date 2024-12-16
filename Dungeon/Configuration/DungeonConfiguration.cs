using Dungeon.Resources;
using Microsoft.Extensions.Configuration;

namespace Dungeon.Configuration
{
    public class DungeonConfiguration
    {
        public string TwoLetterISOLanguageName { get; set; } = "ru";

        public string DataDirectory { get; set; } = "Data";

        public string LocaleDirectory { get; set; } = "Locales";

        public string DbDataFileName { get; set; } = "Data.dtr";

        public string DbAssetsFileName { get; set; } = "Assets.dtr";

        public bool EnableSound { get; set; } = true;

        public bool DrawDebugInfo { get; set; } = false;

        public bool VariableEditor { get; set; } = false;

        public bool ExceptionRethrow { get; set; }

        public IConfigurationRoot ConfigurationRoot { get; set; }

        public T Get<T>(string propertyName)
        {
            return ConfigurationRoot.GetSection(propertyName).Get<T>();
        }

        public ResourceLoaderSettings ResourceLoader { get; set; } = new();
    }
}