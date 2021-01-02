using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Dungeon.Monogame.Effects
{
    public interface IMonogameEffect
    {
        public bool Loaded { get; set; }

#if !Engine
        void Load(XNADrawClient client);
#endif

        Texture2D Draw(Texture2D input);
    }
}
