using Dungeon.Resources;
using Microsoft.Xna.Framework.Content;
using System.IO;

namespace Dungeon.Monogame.Resolvers
{
    public class DataBaseContentResolver : ContentResolver
    {
        public override Stream Resolve(string contentPath)
        {
            return Resources.ResourceLoader.Load(DungeonGlobal.Resources, contentPath).Stream;
        }
    }
}
