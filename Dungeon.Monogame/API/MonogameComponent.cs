using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Dungeon.Monogame.API
{
    public abstract class MonogameComponent : IDisposable
    {
        public abstract void Dispose();

        public abstract void Load(GraphicsDevice graphicsDevice, ContentManager content);
    }
}