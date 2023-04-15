using Dungeon.Resources;
using Microsoft.Extensions.Configuration;

namespace Dungeon.Configuration
{
    public class DungeonConfiguration
    {
        public bool ExceptionRethrow { get; set; }

        public IConfigurationRoot ConfigurationRoot { get; set; }

        public T Get<T>(string propertyName)
        {
            return ConfigurationRoot.GetSection(propertyName).Get<T>();
        }

        public ResourceLoaderSettings ResourceLoader { get; set; } = new();
    }
}