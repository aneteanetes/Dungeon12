#if !Engine
namespace Dungeon.Monogame
#elif Engine
namespace Dungeon.Engine.Host
#endif
{
    using Dungeon;
    using Dungeon.Drawing;
    using Dungeon.Monogame.Effects;
    using Dungeon.Scenes;
    using Dungeon.View.Interfaces;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;

#if !Engine

    public partial class XNADrawClient : Game, IDrawClient
#elif Engine
    public partial class D3D11Host
#endif
    {
        public DrawColor ClearColor { get; set; }

        private Dictionary<ISceneLayer, List<Texture2D>> PostProcessed = new Dictionary<ISceneLayer, List<Texture2D>>();

        private Dictionary<ISceneLayer, List<Texture2D>> PreProcessed = new Dictionary<ISceneLayer, List<Texture2D>>();

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
                PostProcessed.Add(layer, new List<Texture2D>());
            }
            else
            {
                PostProcessed[layer].Clear();
            }
            PostProcessed[layer].Add(processed);
        }

        private Dictionary<ISceneLayer, RenderTarget2D> SceneLayers = new Dictionary<ISceneLayer, RenderTarget2D>();

        private void UpdateLayers(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (this.scene != default)
            {
                foreach (var layer in this.scene.Layers)
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

        protected
#if !Engine
        override
#endif
        void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(
                //#if Engine
                //                _renderTarget
                //#else
                null
                //#endif
                );
            GraphicsDevice.Clear(Color.Transparent);

#if !Engine
            drawCicled = true;
            CalculateCamera();
#endif

            if (this.scene != default)
            {
                foreach (var layer in this.scene.Layers)
                {
                    var buffer = SceneLayers[layer];

                    bool light = false;

                    PreProcessed.Clear();
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

                    XNADrawClientImplementation.Draw(layer.Objects, gameTime, buffer, light, clear: this.scene.Is<@Sys_Clear_Screen>(), layer: layer, resolutionMatrix: ResolutionScale);

                    PostProcessed.Clear();
                    foreach (var postEffect in layer.SceneGlobalEffects.Where(e => e.When == EffectTime.PostProcess))
                    {
                        if (postEffect.Is<IMonogameEffect>())
                        {
                            ProcessMonogameEffect(postEffect.As<IMonogameEffect>(), layer, buffer);
                        }
                    }
                }
            }

            if (spriteBatch.IsOpened)
                spriteBatch.End();

            GraphicsDevice.SetRenderTarget(
#if Engine
                _renderTarget
#else
                null
#endif
                );
            GraphicsDevice.Clear(Color.Transparent);

            spriteBatch.Begin(
#if !Engine
                //transformMatrix: ResolutionScale
#endif
                );

            if (this.scene != default)
            {
                foreach (var layerInfo in SceneLayers)
                {
                    spriteBatch.Draw(layerInfo.Value, new Vector2((float)layerInfo.Key.Left, (float)layerInfo.Key.Top), Color.White);
                    if (PostProcessed.ContainsKey(layerInfo.Key))
                        foreach (var processed in PostProcessed[layerInfo.Key])
                        {
                            spriteBatch.Draw(processed, Vector2.Zero, Color.White);
                        }
                }
            }
            spriteBatch.End();

            DrawDebugInfo();
#if !Engine
            OnPointerMoved();
#endif

            try
            {
                spriteBatch.Begin();
            }
            catch { }
            finally
            {
                spriteBatch.End();
            }

#if !Engine
            Draw3D();
            base.Draw(gameTime);
#endif
        }

#if Engine

    }
#elif !Engine
#region frameSettings

        private bool frameEnd;
        private int _frame;
        private TimeSpan _lastFps;
        private int _lastFpsFrame;
        private double _fps;
        Stopwatch _st = Stopwatch.StartNew();

#endregion

        private void DrawDebugInfo()
        {
            try
            {
                bool neeedClose = false;
                if (!spriteBatch.IsOpened)
                {
                    neeedClose = true;
                    spriteBatch.Begin();
                }
                var nowTs = _st.Elapsed;
                var now = DateTime.Now;
                var fpsTimeDiff = (nowTs - _lastFps).TotalSeconds;
                if (fpsTimeDiff > 1)
                {
                    _fps = (_frame - _lastFpsFrame) / fpsTimeDiff;
                    DungeonGlobal.FPS = _fps;
                    _lastFpsFrame = _frame;
                    _lastFps = nowTs;
                }

                frameEnd = DungeonGlobal.FPS >= 55;

                //var text = $"Версия: {DungeonGlobal.Version}";


                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(DefaultFontXnbExistedFile))
                {
                    if (stream.CanSeek)
                    {
                        stream.Seek(0, SeekOrigin.Begin);
                    }

                    var font = Content.Load<SpriteFont>(DefaultFontXnbExistedFile, stream);

                    var m = (float)this.MeasureText(DungeonGlobal.FPS.ToString().AsDrawText().InSize(10)).X;

                    //spriteBatch.DrawString(font, text, new Vector2(1050, 16), Color.White);

                    spriteBatch.DrawString(font, DungeonGlobal.FPS.ToString(), new Vector2((this.Window.ClientBounds.Width - m)-2, 2), Color.Yellow);
                }

                if (neeedClose)
                {
                    spriteBatch.End();
                }

                _frame++;
            }
            catch { spriteBatch.End(); }
        }

        private const string DefaultFontXnbExistedFile = "Dungeon.Monogame.Resources.Fonts.xnb.Montserrat.Montserrat10.xnb";

        public Dungeon.Types.Point MeasureText(IDrawText drawText, ISceneObject parent = default)
            => XNADrawClientImplementation.MeasureText(drawText, parent);

        public Dungeon.Types.Point MeasureImage(string image) => XNADrawClientImplementation.MeasureImage(image);

        public void SaveObject(ISceneObject sceneObject, string path, Dungeon.Types.Point offset, string runtimeCacheName = null)
        {
            XNADrawClientImplementation.SaveObject(sceneObject, path, offset, runtimeCacheName);
        }

        public void Animate(IAnimationSession animationSession)
        {
            throw new System.NotImplementedException();
        }

        public void Clear(IDrawColor drawColor = null)
        {
            XNADrawClientImplementation.Clear(drawColor);
        }

        public void CacheObject(ISceneObject @object)
        {
            XNADrawClientImplementation.CacheObject(@object);
        }

        public void CacheImage(string image)
        {
            XNADrawClientImplementation.CacheImage(image);
        }
    }
#endif
    }