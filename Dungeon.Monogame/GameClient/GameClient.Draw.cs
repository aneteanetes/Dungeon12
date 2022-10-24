using Dungeon.Drawing;
using Dungeon.Monogame.Effects;
using Dungeon.Scenes;
using Dungeon.View.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Dungeon.Monogame
{
    public partial class GameClient : Game, IGameClient
    {

        private Dictionary<ISceneLayer, List<(Texture2D texture, IMonogameEffect effect)>> PostProcessed = new Dictionary<ISceneLayer, List<(Texture2D, IMonogameEffect)>>();

        private Dictionary<ISceneLayer, List<(Texture2D texture, IMonogameEffect effect)>> PreProcessed = new Dictionary<ISceneLayer, List<(Texture2D, IMonogameEffect)>>();

        private void ProcessMonogameEffect(IMonogameEffect monogameEffect, ISceneLayer layer, RenderTarget2D buffer)
        {
            if (!monogameEffect.Loaded)
            {
#if !Engine
                monogameEffect.Load(this);
                monogameEffect.Loaded = true;
#endif
            }
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.Transparent);
            var processed = monogameEffect.Draw(buffer);
            if (!PostProcessed.ContainsKey(layer))
            {
                PostProcessed.Add(layer, new List<(Texture2D, IMonogameEffect)>());
            }
            else
            {
                PostProcessed[layer].Clear();
            }
            PostProcessed[layer].Add((processed, monogameEffect));
        }

        private string prevSceneUid;

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.Transparent);

            drawCicled = true;

            if (_settings.NeedCalculateCamera)
                CalculateCamera();

            PreProcessed.Clear();
            PostProcessed.Clear();

            if (this.Scene != default)
            {
                foreach (var layer in this.Scene.Layers)
                {
                    var buffer = SceneLayers[layer];

                    bool light = false;

                    foreach (var preEffect in layer.SceneGlobalEffects.Where(e => e.When == EffectTime.PreProcess))
                    {
                        if (preEffect.Is<Light2D>())
                        {
                            light = true;
                            continue;
                        }

                        if (preEffect.Is<IMonogameEffect>())
                        {
                            ProcessMonogameEffect(preEffect.As<IMonogameEffect>(), layer, buffer);
                        }
                    }

                    DrawClient.Draw(layer, buffer, gameTime, light);

                    foreach (var postEffect in layer.SceneGlobalEffects.Where(e => e.When == EffectTime.PostProcess))
                    {
                        if (postEffect.Is<IMonogameEffect>())
                        {
                            ProcessMonogameEffect(postEffect.As<IMonogameEffect>(), layer, buffer);
                        }
                    }
                }
            }

            RenderTarget2D screenshottarget = null;

            if (makingscreenshot)
            {
                var pp = GraphicsDevice.PresentationParameters;
                screenshottarget = new RenderTarget2D(GraphicsDevice, DungeonGlobal.Resolution.Width, DungeonGlobal.Resolution.Height, false,
                            pp.BackBufferFormat, pp.DepthStencilFormat, pp.MultiSampleCount, pp.RenderTargetUsage);

                GraphicsDevice.SetRenderTarget(screenshottarget);
            }
            else
            {
                GraphicsDevice.SetRenderTarget(null);
            }

            GraphicsDevice.Clear(Color.Transparent);

            LayerSpriteBatch.Begin();
            if (this.Scene != default)
            {
                foreach (var layerInfo in SceneLayers)
                {
                    bool skipForPostProcess = false;

                    var havePostProcess = PostProcessed.ContainsKey(layerInfo.Key);
                    if (havePostProcess)
                    {
                        skipForPostProcess = PostProcessed[layerInfo.Key].Any(x => x.effect.NotDrawOriginal);
                    }

                    if (!skipForPostProcess)
                        LayerSpriteBatch.Draw(layerInfo.Value, new Vector2((float)layerInfo.Key.Left, (float)layerInfo.Key.Top), Color.White);

                    if (PostProcessed.ContainsKey(layerInfo.Key))
                        foreach (var processed in PostProcessed[layerInfo.Key])
                        {
                            LayerSpriteBatch.Draw(processed.texture, Vector2.Zero, Color.White);
                        }
                }
            }
            LayerSpriteBatch.End();

            DrawDebugInfo();

#warning code changed and commented cuz idk for what
            //if (spriteBatch.IsOpened)
            //    spriteBatch.End();

            Draw3D();
            base.Draw(gameTime);

            if (makingscreenshot)
            {
                GraphicsDevice.SetRenderTarget(null);
                var screenpath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Screenshots");
                if (!Directory.Exists(screenpath))
                {
                    Directory.CreateDirectory(screenpath);
                }
                using (var f = File.Create(Path.Combine(screenpath, $"Screenshot {DateTime.Now:dd.MM.yyyy HH mm}.png")))
                {
                    screenshottarget.SaveAsPng(f, screenshottarget.Width, screenshottarget.Height);
                }
                makingscreenshot = false;
            }

            if (prevSceneUid != Scene.Uid)
            {
                Scene.Loaded();
                prevSceneUid = Scene.Uid;
            }
        }
    }
}