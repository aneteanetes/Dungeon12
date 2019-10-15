namespace Rogue
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Penumbra;
    using ProjectMercury;
    using Rogue.Monogame;
    using Rogue.Resources;
    using Rogue.View.Enums;
    using Rogue.View.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using Rect = Rogue.Types.Rectangle;

    public partial class XNADrawClient : Game, IDrawClient
    {
        private static float cell = 32;

        /// <summary>
        /// DEAD END
        /// </summary>
        /// <param name="drawSessions"></param>
        public void Draw(IEnumerable<IDrawSession> drawSessions)
        {
            throw new System.NotImplementedException();
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            CalculateCamera();

            Draw(this.scene.Objects, gameTime);

            DrawDebugInfo();

            OnPointerMoved();
        }

        private void Draw(ISceneObject[] sceneObjects, GameTime gameTime)
        {
            InterfaceObjects.Clear();

            var all = sceneObjects
                .Where(x => x.Visible && (x.DrawOutOfSight || (!x.DrawOutOfSight && InCamera(x))))
                .ToArray();

            var absolute = all
                .Where(x => x.AbsolutePosition || scene.AbsolutePositionScene)
                .OrderBy(x => x.Layer).ToArray();

            var offsetted = all
                .Where(x => !scene.AbsolutePositionScene && !x.AbsolutePosition)
                .OrderBy(x => x.Layer).ToArray();

            penumbra.BeginDraw();

            SetSpriteBatch();

            foreach (var offsetSceneObject in offsetted)
            {
                DrawSceneObject(offsetSceneObject);
            }
            spriteBatch.End();

            penumbra.Draw(gameTime);

            SetSpriteBatch();

            for (int i = 0; i < InterfaceObjects.Count; i++)
            {
                var (sceneObject, x, y) = InterfaceObjects[i];
                DrawSceneObject(sceneObject, x, y, lightIgnoring: true);
            }

            spriteBatch.End();

            SetSpriteBatch(true);

            foreach (var absoluteSceneObject in absolute)
            {
                DrawSceneObject(absoluteSceneObject);
            }
            spriteBatch.End();
        }

        private void SetSpriteBatch(bool absolute=false)
        {
            if (!absolute)
            {
                SpriteBatchRestore = smooth => spriteBatch.Begin(
                    transformMatrix: Matrix.CreateTranslation((float)CameraOffsetX, (float)CameraOffsetY, 0), 
                    samplerState: !smooth ? SamplerState.PointWrap : SamplerState.LinearClamp,
                    blendState: BlendState.NonPremultiplied);
            }
            else
            {
                SpriteBatchRestore = smooth => spriteBatch.Begin(
                    samplerState: !smooth ? SamplerState.PointWrap : SamplerState.LinearClamp,
                    blendState: BlendState.NonPremultiplied);
            }
            SpriteBatchRestore.Invoke(false);
        }

        private Action<bool> SpriteBatchRestore = null;

        #region frameSettings

        private int _frame;
        private TimeSpan _lastFps;
        private int _lastFpsFrame;
        private double _fps;
        Stopwatch _st = Stopwatch.StartNew();

        #endregion
        
        private void DrawDebugInfo()
        {
            spriteBatch.Begin();
            var nowTs = _st.Elapsed;
            var now = DateTime.Now;
            var fpsTimeDiff = (nowTs - _lastFps).TotalSeconds;
            if (fpsTimeDiff > 1)
            {
                _fps = (_frame - _lastFpsFrame) / fpsTimeDiff;
                Global.FPS = _fps;
                _lastFpsFrame = _frame;
                _lastFps = nowTs;
            }

            var text = $"FPS: {_fps}";

            var font = Content.Load<SpriteFont>("Montserrat");

            spriteBatch.DrawString(font, text, new Vector2(555, 16), Color.White);

            spriteBatch.DrawString(font, Global.Time, new Vector2(625, 30), Color.Yellow);

            spriteBatch.End();

            _frame++;
        }

        public Types.Point MeasureText(IDrawText drawText)
        {
            string customFont = null;
            if (drawText.FontName != null)
            {
                customFont = $"{drawText.FontName}/{drawText.FontName}{drawText.Size}";
            }

            var font = Content.Load<SpriteFont>(customFont ?? "Triforce/Triforce30");

            var m = font.MeasureString(drawText.StringData);

            return new Types.Point(m.X, m.Y);
        }

        public Types.Point MeasureImage(string image)
        {
            var img = TileSetByName(image);
            if (img == default)
                return new Types.Point();

            return new Types.Point()
            {
                X = img.Width,
                Y = img.Height
            };
        }

        public void SaveObject(ISceneObject sceneObject, string path, Types.Point offset, string runtimeCacheName = null)
        {
            throw new System.NotImplementedException();
        }

        public void Animate(IAnimationSession animationSession)
        {
            throw new System.NotImplementedException();
        }

        private readonly Dictionary<string, RenderTarget2D> BatchCache = new Dictionary<string, RenderTarget2D>();
        private Dictionary<string, Rect> TileSetCache = new Dictionary<string, Rect>();
        private Dictionary<string, Rect> PosCahce = new Dictionary<string, Rect>();
        private static readonly Dictionary<string, Texture2D> tilesetsCache = new Dictionary<string, Texture2D>();

        /// <summary>
        /// TODO: нужно логирование что бы игра не падала но можно было понять причину сбоя
        /// </summary>
        /// <param name="tilesetName"></param>
        /// <returns></returns>
        private Texture2D TileSetByName(string tilesetName)
        {
            if (!tilesetsCache.TryGetValue(tilesetName, out var bitmap))
            {
                var stream = ResourceLoader.Load(tilesetName, tilesetName);
                if (stream == default)
                    return default;

                bitmap = Texture2D.FromStream(GraphicsDevice, stream);

                tilesetsCache.TryAdd(tilesetName, bitmap);
            }

            return bitmap;
        }

        private List<(ISceneObject sceneObject, double x, double y)> InterfaceObjects = new List<(ISceneObject sceneObject, double x, double y)>();

        private void DrawSceneObject(ISceneObject sceneObject, double xParent = 0, double yParent = 0, bool batching = false, bool force = false, bool lightIgnoring=false)
        {
            if (sceneObject.Interface && !lightIgnoring && !sceneObject.AbsolutePosition)
            {
                InterfaceObjects.Add((sceneObject, xParent, yParent));
                return;
            }

            if (!sceneObject.Visible)
                return;

            if (force && sceneObject.ForceInvisible)
                return;

            sceneObject.Update();

            var y = sceneObject.Position.Y * cell + yParent;
            var x = sceneObject.Position.X * cell + xParent;

            DrawLight(sceneObject, x, y);
            DrawEffects(sceneObject, x, y);

            if (sceneObject.IsBatch && !batching)
            {
                int width = (int)Math.Round(sceneObject.Position.Width * cell);
                int height = (int)Math.Round(sceneObject.Position.Height * cell);

                if (sceneObject.Expired || !BatchCache.TryGetValue(sceneObject.Uid, out var bitmap))
                {
                    bitmap = new RenderTarget2D(GraphicsDevice, width, height, false, GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);

                    spriteBatch.End();

                    GraphicsDevice.SetRenderTargets(bitmap);
                    GraphicsDevice.Clear(Color.Transparent);
                    SpriteBatchRestore.Invoke(false);

                    DrawSceneObject(sceneObject, 0, 0, true);

                    TileSetCache[sceneObject.Uid] = new Rect(0, 0, width, height);
                    PosCahce[sceneObject.Uid] = new Rect(x, y, width, height);

                    BatchCache[sceneObject.Uid] = bitmap;


                    spriteBatch.End();
                    GraphicsDevice.SetRenderTarget(null);

                    SpriteBatchRestore.Invoke(false);
                }

                TileSetCache.TryGetValue(sceneObject.Uid, out var tilesetPos);
                PosCahce.TryGetValue(sceneObject.Uid, out var sceneObjPos);

                if (!sceneObject.CacheAvailable)
                {
                    sceneObjPos = new Rect(x, y, width, height);
                }

                spriteBatch.Draw(bitmap, new Vector2(sceneObjPos.Xf, sceneObjPos.Yf), new Microsoft.Xna.Framework.Rectangle(tilesetPos.Xi, tilesetPos.Yi, tilesetPos.Widthi, tilesetPos.Heighti), Color.White);
            }
            else
            {
                if (!string.IsNullOrEmpty(sceneObject.Image))
                {
                    DrawSceneImage(sceneObject, y, x, force);
                }

                if (sceneObject.Path != null)
                {
                    DrawScenePath(sceneObject.Path, x, y);
                }

                if (sceneObject.Text != null)
                {
                    var text = sceneObject.Text;
                    var textX = x;
                    var textY = y;

                    IDrawText prev = null;

                    foreach (var range in text.Data)
                    {
                        DrawSceneText(text.Size, textY, textX, range);

                        if (range.StringData == Environment.NewLine)
                        {
                            textX = x;
                            textY += MeasureText(prev).Y;
                        }
                        else
                        {
                            textX += range.Length * range.LetterSpacing;
                        }

                        prev = range;
                    }
                }

                var childrens = sceneObject.Children.OrderBy(c => c.Layer).ToArray();

                for (int i = 0; i < childrens.Length; i++)
                {
                    var child = childrens.ElementAtOrDefault(i);
                    if (child != null)
                    {
                        DrawSceneObject(child, x, y, batching, force);
                    }
                }
            }
        }

        private void DrawSceneImage(ISceneObject sceneObject, double y, double x, bool force)
        {
            var image = TileSetByName(sceneObject.Image);
            if (image == default)
                return;

            if (force || !TileSetCache.TryGetValue(sceneObject.Uid, out Rect tileRegion))
            {
                if (sceneObject.ImageRegion == null)
                {
                    tileRegion = new Rect(0, 0, image.Width, image.Height);
                }
                else
                {
                    var tilePos = sceneObject.ImageRegion;
                    tileRegion = new Rect(tilePos.X, tilePos.Y, tilePos.Width, tilePos.Height);
                }

                if (!force && sceneObject.CacheAvailable)
                {
                    TileSetCache.Add(sceneObject.Uid, tileRegion);
                }
            }

            if (force || !PosCahce.TryGetValue(sceneObject.Uid, out Rect pos))
            {
                double width = sceneObject.Position.Width;
                double height = sceneObject.Position.Height;

                if (width == 0 && height == 0)
                {
                    width = image.Width;
                    height = image.Height;
                }
                else
                {
                    width *= cell;
                    height *= cell;
                }

                pos = new Rect(x, y, width, height);

                if (!force && sceneObject.CacheAvailable)
                {
                    PosCahce.Add(sceneObject.Uid, pos);
                }
            }

#warning Вот здесь теперь всё хорошо,но это место можно использовать для того что бы заоптимизировать преобразование размеров, т.к. масштабирование текстур происходит тут

            var dest = new Rectangle(pos.Xi, pos.Yi, pos.Widthi, pos.Heighti);

            if (sceneObject.Effects.Count > 0) {
                //Console.WriteLine($"texture: {dest.X+CameraOffsetX} {dest.Y+CameraOffsetY}");
            }

            var angle = (float)sceneObject.Angle;
            var origin = angle != 0
                ? new Vector2(tileRegion.Widthi / 2f, tileRegion.Heighti / 2f)
                : Vector2.Zero;

            SpriteEffects spriteEffects = SpriteEffects.None;

            if (sceneObject.ImageMask != default && sceneObject.ImageMask.Visible)
            {
                var maskResult = ApplyImageMask(image, sceneObject);
                image = maskResult.image;
                spriteEffects = maskResult.effects;
            }

            var source = new Rectangle(tileRegion.Xi, tileRegion.Yi, tileRegion.Widthi, tileRegion.Heighti);
            spriteBatch.Draw(image, dest, source, Color.White, angle, origin, spriteEffects, 0f);
        }

        readonly Dictionary<string, Dictionary<float, Texture2D>> MaskCache = new Dictionary<string, Dictionary<float, Texture2D>>();

        private (Texture2D image, SpriteEffects effects) ApplyImageMask(Texture2D image, ISceneObject sceneObject)
        {
            var mask = sceneObject.ImageMask;
            var progress = (float)Math.Round(mask.AmountPercentage * 0.01f,2);
            SpriteEffects effects = mask.Pattern == MaskPattern.RadialClockwise
                ? SpriteEffects.None
                : SpriteEffects.FlipVertically;

            var uid = sceneObject.Image;

            Texture2D texture;
            void MaskTexture()
            {
                texture = MakeMask(image, progress, mask.Color.Convert(), mask.Opacity);
            }

            if (!mask.CacheAvailable)
            {
                MaskTexture();
            }
            else
            {
                if (MaskCache.TryGetValue(uid, out var progressCache))
                {
                    if (progressCache.TryGetValue(progress, out texture))
                    {
                        return (texture, effects);
                    }
                    else
                    {
                        MaskTexture();
                        progressCache.Add(progress, texture);
                    }
                }
                else
                {
                    MaskTexture();
                    MaskCache.Add(uid, new Dictionary<float, Texture2D>());
                    MaskCache[uid].Add(progress, texture);
                }
            }

            return (texture, effects);
        }

        private Texture2D MakeMask(Texture2D texture2D, float progress, Color overlayColor, float opacity)
        {
            var thisTex = new Texture2D(GraphicsDevice, texture2D.Width, texture2D.Height);
            //find the centre pixel
            var centre = new Vector2(MathF.Ceiling(thisTex.Width / 2), MathF.Ceiling(thisTex.Height / 2));
            for (int y = 0; y < thisTex.Height; y++)
            {
                for (int x = 0; x < thisTex.Width; x++)
                { 
                    //find the angle between the centre and this pixel (between -180 and 180)
                    var angle = MathHelper.ToDegrees(MathF.Atan2(x - centre.X, y - centre.Y));
                    if (angle < 0)
                    {
                        angle += 360; //change angles to go from 0 to 360
                    }
                    var pixColor = texture2D.GetPixel(x, y);
                    if (angle >= progress * 360.0)
                    {
                        //if the angle is less than the progress angle blend the overlay colour
                        pixColor = Color.Lerp(pixColor, overlayColor, 0.5f);
                        thisTex.SetPixel(x, y, pixColor);
                    }
                    else
                    {
                        thisTex.SetPixel(x, y, pixColor);
                    }
                }
            }
            return thisTex;
        }

        private void DrawSceneText(float fontSize, double y, double x, IDrawText range)
        {
            bool fontWeight = range.Bold;

            SpriteFont spriteFont;
            if (string.IsNullOrEmpty(range.FontName))
            {
                spriteFont = Content.Load<SpriteFont>("Triforce/Triforce30");
            }
            else
            {
                if (string.IsNullOrEmpty(range.FontPath))
                {
                    spriteFont = Content.Load<SpriteFont>($"{range.FontName}/{range.FontName}{range.Size}");
                    //typeface = new Typeface(range.FontName, weight: fontWeight);
                }
                else
                {
                    spriteFont = Content.Load<SpriteFont>($"{range.FontName}/{range.FontName}{range.Size}");
                    //typeface = new Typeface(Font.GetFontFamily(range.FontName, range.FontPath, range.FontAssembly), fontSize: fontSize);
                }
            }

            var txt = range.StringData;            
            var color = new Color(range.ForegroundColor.R, range.ForegroundColor.G, range.ForegroundColor.B, range.ForegroundColor.A);

            spriteBatch.End();
            SpriteBatchRestore?.Invoke(true);

            spriteBatch.DrawString(spriteFont, txt, new Vector2((int)x, (int)y), color);

            spriteBatch.End();
            SpriteBatchRestore?.Invoke(false);
        }

        private void DrawScenePath(IDrawablePath drawablePath, double x, double y)
        {
            if (drawablePath.PathPredefined == PathPredefined.Rectangle)
            {
                var color = drawablePath.BackgroundColor;

                var drawColor = new Color(color.R, color.G, color.B, (float)color.Opacity);
                var pathReg = drawablePath.Region;

                var rect = new Microsoft.Xna.Framework.Rectangle((int)x, (int)y, (int)(pathReg.Width * cell), (int)(pathReg.Height * cell));
                var cornerRadius = drawablePath.Radius;

                if (drawablePath.Fill)
                {
                    var dummyTexture = new Texture2D(GraphicsDevice, 1, 1);
                    dummyTexture.SetData(new Color[] { drawColor });

                    spriteBatch.Draw(dummyTexture, rect, drawColor);
                }
                else
                {
                    DrawBorder(rect, 1, drawColor);
                }
            }

            if (drawablePath.PathPredefined == PathPredefined.Line)
            {
                Texture2D texture = default;

                if (drawablePath.Texture != string.Empty)
                {
                    texture = TileSetByName(drawablePath.Texture);
                }

                var from = new Types.Point(drawablePath.Path.First());
                from.X *= 32;
                from.Y *= 32;
                from.X += x;
                from.Y += y;

                var to = new Types.Point(drawablePath.Path.Last());
                to.X *= 32;
                to.Y *= 32;
                to.X += x;
                to.Y += y;

                var fromVector = new Vector2(from.Xf, from.Yf);
                var toVector = new Vector2(to.Xf, to.Yf);

                var color = drawablePath.BackgroundColor;

                var alpha = color.Opacity == 0
                    ? color.A
                    : color.Opacity;

                var drawColor = new Color(color.R, color.G, color.B, (float)alpha);

                DrawLineTo(spriteBatch, texture, fromVector, toVector, drawColor,(int)drawablePath.Depth);
            }
        }

        public void DrawLineColor(SpriteBatch spriteBatch, Vector2 begin, Vector2 end, Color color, int width = 1)
        {
            width = 1;
            Rectangle r = new Rectangle((int)begin.X, (int)begin.Y, (int)(end - begin).Length() + width, width);
            Vector2 v = Vector2.Normalize(begin - end);
            float angle = (float)Math.Acos(Vector2.Dot(v, -Vector2.UnitX));
            if (begin.Y > end.Y) angle = MathHelper.TwoPi - angle;
            spriteBatch.Draw(PixelColorTexture(), r,default, color, angle, Vector2.Zero, SpriteEffects.None, 0);
        }

        public void DrawLineTo(SpriteBatch sb, Texture2D texture, Vector2 src, Vector2 dst, Color color,int depth)
        {
            if (texture == default)
            {
                texture = PixelColorTexture();
            }
            else
            {
                color = Color.White;
            }

            //direction is destination - source vectors
            Vector2 direction = dst - src;
            //get the angle from 2 specified numbers (our point)
            var angle = (float)Math.Atan2(direction.Y, direction.X);
            //calculate the distance between our two vectors
            float distance;
            Vector2.Distance(ref src, ref dst, out distance);

            //draw the sprite with rotation
            sb.Draw(texture, src, new Rectangle((int)src.X, (int)src.Y, (int)distance, depth), color, angle, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
        }
        
        private void DrawLine(SpriteBatch spriteBatch, Texture2D texture, Vector2 start, Vector2 end, float height)
        {
            spriteBatch.Draw(texture, start, null, Color.White,
                             (float)Math.Atan2(end.Y - start.Y, end.X - start.X),
                             new Vector2(0f, height),
                             new Vector2(Vector2.Distance(start, end), 1f),
                             SpriteEffects.None, 0f);
        }

        /// <summary>
        /// СПИЗЖЕНО
        /// <para>
        /// Will draw a border (hollow rectangle) of the given 'thicknessOfBorder' (in pixels)
        /// of the specified color.
        ///
        /// By Sean Colombo, from http://bluelinegamestudios.com/blog
        /// </para>
        /// </summary>
        /// <param name="rectangleToDraw"></param>
        /// <param name="thicknessOfBorder"></param>
        private void DrawBorder(Microsoft.Xna.Framework.Rectangle rectangleToDraw, int thicknessOfBorder, Color borderColor)
        {
            Texture2D pixel = PixelColorTexture();

            // Draw top line
            spriteBatch.Draw(pixel, new Microsoft.Xna.Framework.Rectangle(rectangleToDraw.X, rectangleToDraw.Y, rectangleToDraw.Width, thicknessOfBorder), borderColor);

            // Draw left line
            spriteBatch.Draw(pixel, new Microsoft.Xna.Framework.Rectangle(rectangleToDraw.X, rectangleToDraw.Y, thicknessOfBorder, rectangleToDraw.Height), borderColor);

            // Draw right line
            spriteBatch.Draw(pixel, new Microsoft.Xna.Framework.Rectangle((rectangleToDraw.X + rectangleToDraw.Width - thicknessOfBorder),
                                            rectangleToDraw.Y,
                                            thicknessOfBorder,
                                            rectangleToDraw.Height), borderColor);
            // Draw bottom line
            spriteBatch.Draw(pixel, new Microsoft.Xna.Framework.Rectangle(rectangleToDraw.X,
                                            rectangleToDraw.Y + rectangleToDraw.Height - thicknessOfBorder,
                                            rectangleToDraw.Width,
                                            thicknessOfBorder), borderColor);
        }

        private Texture2D PixelColorTexture()
        {
            Texture2D pixel;

            // Somewhere in your LoadContent() method:
            pixel = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            pixel.SetData(new[] { Color.White }); // so that we can draw whatever color we want on top of it
            return pixel;
        }

        private static readonly Dictionary<string, PointLight> Lights = new Dictionary<string, PointLight>();

        private void DrawLight(ISceneObject sceneObject, double x, double y)
        {
            if (sceneObject.Light == null)
                return;

            var objLight = sceneObject.Light;

            var xf = x + CameraOffsetX;
            var yf = y + CameraOffsetY;

            xf += sceneObject.Position.Width / 2 * cell;
            yf += sceneObject.Position.Height * cell;

            var pos = new Vector2((float)xf, (float)yf);

            var objLightColor = objLight.Color;
            var color = new Color(objLightColor.R, objLightColor.G, objLightColor.B, objLightColor.A);

            if (objLight.Updated && Lights.ContainsKey(sceneObject.Uid))
            {
                var oldLight = Lights[sceneObject.Uid];
                penumbra.Lights.Remove(oldLight);
                Lights.Remove(sceneObject.Uid);
            }

            if (!Lights.TryGetValue(sceneObject.Uid, out var light))
            {
                light = new PointLight()
                {
                    Scale = new Vector2(objLight.Range*32),
                    ShadowType = ShadowType.Occluded,
                    Radius = sceneObject.Light.Range * 32,
                    Position = pos,
                    Color = color
                };

                penumbra.Lights.Add(light);
                Lights[sceneObject.Uid] = light;

                sceneObject.Destroy += () =>
                {
                    Lights.Remove(sceneObject.Uid);
                    penumbra.Lights.Remove(light);
                };
            }

            light.Position = pos;
        }

        private static readonly Dictionary<string, ParticleEffect> ParticleEffects = new Dictionary<string, ParticleEffect>();

        private void DrawEffects(ISceneObject sceneObject, double x, double y)
        {
            if (sceneObject.Effects.Count == 0)
                return;

            x = (int)x;
            y = (int)y;

            foreach (var effect in sceneObject.Effects)
            {
                if (!ParticleEffects.TryGetValue(sceneObject.Uid, out var particleEffect))
                {
                    var path = $"Rogue.Resources.Particles.{effect.Name}.xml";
                    if(effect.Assembly!=default)
                    {
                        path = $"{effect.Assembly}.Particles.{effect.Name}.xml";
                    }

                    var particleStream = ResourceLoader.Load(path, path);
                    var loader = new ParticleEffectLoader(particleStream,effect.Assembly);
                    particleEffect = loader.Load();
                    particleEffect.Scale = (float)effect.Scale;
                    particleEffect.LoadContent(this.Content);
                    particleEffect.Initialise();

                    ParticleEffects.Add(sceneObject.Uid, particleEffect);

                    sceneObject.Destroy += () => ParticleEffects.Remove(sceneObject.Uid);

                    myRenderer.LoadContent(Content);
                }

                var pos = new Vector2((float)x, (float)y);
                particleEffect.Trigger(pos);
                var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                particleEffect.Update(deltaTime);

                spriteBatch.End();
                
                var v = Matrix.CreateTranslation(-(float)x, -(float)y, 0) 
                    * Matrix.CreateScale((float)effect.Scale) 
                    * Matrix.CreateTranslation((float)x, (float)y, 0)
                    * Matrix.CreateTranslation((float)CameraOffsetX, (float)CameraOffsetY, 0);

                myRenderer.RenderEffect(particleEffect, ref v);

                SpriteBatchRestore.Invoke(false);
            }
        }

        public void CacheObject(ISceneObject sceneObject)
        {
            var image = TileSetByName(sceneObject.Image);

            if (sceneObject.ImageMask != default && sceneObject.ImageMask.CacheAvailable)
            {
                CacheImageMask(image, sceneObject);
            }
        }

        private void CacheImageMask(Texture2D image, ISceneObject sceneObject)
        {
            var uid = sceneObject.Image;
            var mask = sceneObject.ImageMask;

            if (!MaskCache.ContainsKey(uid))
            {
                MaskCache.Add(uid, new Dictionary<float, Texture2D>());
            }
            else
            {
                return;
            }

            var cache = MaskCache[uid];
            for (float i = 0f; i < 1; i += 0.01f)
            {
                var v = (float)Math.Round(i);
                if (!cache.ContainsKey(v))
                {
                    cache.Add(v, MakeMask(image, v, mask.Color.Convert(), mask.Opacity));
                }

            }
        }
    }
}