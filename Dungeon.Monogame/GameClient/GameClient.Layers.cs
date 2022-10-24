using Dungeon.Types;
using Dungeon.View.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Dungeon.Monogame
{
    public partial class GameClient : Game, IGameClient
    {
        private Dictionary<ISceneLayer, RenderTarget2D> SceneLayers = new Dictionary<ISceneLayer, RenderTarget2D>();

        private void UpdateLayersExistance(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (this.Scene != default)
            {
                foreach (var layer in this.Scene.Layers)
                {
                    if (SceneLayers.ContainsKey(layer))
                    {
                        if (layer.Destroyed)
                        {
                            SceneLayers.Remove(layer);
                        }
                    }
                    else
                    {
                        var pp = GraphicsDevice.PresentationParameters;
                        SceneLayers.Add(layer, new RenderTarget2D(GraphicsDevice, (int)layer.Width, (int)layer.Height, false,
                            pp.BackBufferFormat, pp.DepthStencilFormat, pp.MultiSampleCount, pp.RenderTargetUsage));
                    }
                }
            }
        }
    }
}