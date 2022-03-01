using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Dungeon12.Entities.Talks
{
    public class Goal
    {
        public string Name { get; set; }

        public string Icon { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ReplicaType Type { get; set; }

        public bool IsAchived { get; set; }

        public string Function { get; set; }
    }
}