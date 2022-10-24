using Dungeon.Types;
using Dungeon.View.Interfaces;

namespace Dungeon.SceneObjects
{
    public class NamedEffect : IEffect
    {
        public NamedEffect(string name)
        {
            Name = name;
        }

        public string Name { get; set;  }

        public double Scale { get; set; }

        public string Assembly { get; set; }

        public EffectTime When { get; }

        public Dot Position { get; set; }

        public string Image { get; set; }

        public Dot Size { get; set; }
    }
}