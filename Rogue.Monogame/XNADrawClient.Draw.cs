namespace Rogue
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using MonoGame.Extended.Particles;
    using Penumbra;
    using Rogue.Resources;
    using Rogue.View.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
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
            GraphicsDevice.Clear(Color.Black);
            CalculateCamera();


            Draw(this.scene.Objects, gameTime);

            DrawDebugInfo();

            //if (visibleEmitters)
            //{
            //    spriteBatch.Begin(blendState: BlendState.AlphaBlend);
            //    spriteBatch.Draw(_particleEffect);
            //    spriteBatch.End();
            //}

            OnPointerMoved();



        }

        private void Draw(ISceneObject[] sceneObjects, GameTime gameTime)
        {
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

            spriteBatch.Begin(transformMatrix: Matrix.CreateTranslation((float)CameraOffsetX, (float)CameraOffsetY, 0));
            foreach (var offsetSceneObject in offsetted)
            {
                DrawSceneObject(offsetSceneObject);
            }
            spriteBatch.End();

            penumbra.Draw(gameTime);

            spriteBatch.Begin();
            foreach (var absoluteSceneObject in absolute)
            {
                DrawSceneObject(absoluteSceneObject);
            }
            spriteBatch.End();
        }

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
            var font = Content.Load<SpriteFont>(drawText.FontName ?? "Triforce/Triforce30");

            var m = font.MeasureString(drawText.StringData);

            return new Types.Point(m.X, m.Y);
        }

        public Types.Point MeasureImage(string image)
        {
            var img = TileSetByName(image);
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

        private Texture2D TileSetByName(string tilesetName)
        {
            if (!tilesetsCache.TryGetValue(tilesetName, out var bitmap))
            {
                var stream = ResourceLoader.Load(tilesetName, tilesetName);
                bitmap = Texture2D.FromStream(GraphicsDevice, stream);

                tilesetsCache.TryAdd(tilesetName, bitmap);
            }

            return bitmap;
        }

        private void DrawSceneObject(ISceneObject sceneObject, double xParent = 0, double yParent = 0, bool batching = false, bool force = false)
        {
            if (!sceneObject.Visible)
                return;

            if (force && sceneObject.ForceInvisible)
                return;

            var y = sceneObject.Position.Y * cell + yParent;
            var x = sceneObject.Position.X * cell + xParent;

            DrawShadow(sceneObject, x, y);

            if (sceneObject.IsBatch && !batching)
            {
                if (sceneObject.Expired || !BatchCache.TryGetValue(sceneObject.Uid, out var bitmap))
                {
                    int width = (int)Math.Round(sceneObject.Position.Width * cell);
                    int height = (int)Math.Round(sceneObject.Position.Height * cell);

                    bitmap = new RenderTarget2D(GraphicsDevice, width, height, false, GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);

                    GraphicsDevice.SetRenderTarget(bitmap);

                    DrawSceneObject(sceneObject, xParent, yParent, true);

                    TileSetCache[sceneObject.Uid] = new Rect(0, 0, width, height);
                    PosCahce[sceneObject.Uid] = new Rect(sceneObject.Position.X, sceneObject.Position.Y, width, height);

                    BatchCache[sceneObject.Uid] = bitmap;

                    GraphicsDevice.SetRenderTarget(null);
                }

                TileSetCache.TryGetValue(sceneObject.Uid, out var tilesetPos);
                PosCahce.TryGetValue(sceneObject.Uid, out var sceneObjPos);

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

                    foreach (var range in text.Data)
                    {
                        DrawSceneText(text.Size, y, textX, range);
                        textX += range.Length * range.LetterSpacing;
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

            var dest = new Microsoft.Xna.Framework.Rectangle(pos.Xi, pos.Yi, pos.Widthi, pos.Heighti);

            spriteBatch.Draw(image, dest,
                new Microsoft.Xna.Framework.Rectangle(tileRegion.Xi, tileRegion.Yi,
                    tileRegion.Widthi, tileRegion.Heighti),
                Color.White);
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
                    spriteFont = Content.Load<SpriteFont>(range.FontName);
                    //typeface = new Typeface(Font.GetFontFamily(range.FontName, range.FontPath, range.FontAssembly), fontSize: fontSize);
                }
            }

            var txt = range.StringData;

            var color = new Color(range.ForegroundColor.R, range.ForegroundColor.G, range.ForegroundColor.B, range.ForegroundColor.A);

            spriteBatch.DrawString(spriteFont, txt, new Vector2((int)x, (int)y), color);
        }

        private void DrawScenePath(IDrawablePath drawablePath, double x, double y)
        {
            if (drawablePath.PathPredefined == View.Enums.PathPredefined.Rectangle)
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
            Texture2D pixel;

            // Somewhere in your LoadContent() method:
            pixel = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            pixel.SetData(new[] { Color.White }); // so that we can draw whatever color we want on top of it

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

        private static Dictionary<string, PointLight> Shadows = new Dictionary<string, PointLight>();

        private void DrawShadow(ISceneObject sceneObject, double x, double y)
        {
            if (!sceneObject.Shadow)
                return;

            var xf = x + CameraOffsetX;
            var yf = y + CameraOffsetY;

            xf += sceneObject.Position.Width / 2 * cell;
            yf += sceneObject.Position.Height * cell;

            var pos = new Vector2((float)xf, (float)yf);

            if (!Shadows.TryGetValue(sceneObject.Uid, out var hullShadow))
            {
                hullShadow = new PointLight()
                {
                    Scale = new Vector2(32),
                    ShadowType = ShadowType.Occluded,
                    Radius = 32,
                    Position = pos,
                    Color= Color.DarkGray
                };

                penumbra.Lights.Add(hullShadow);
                Shadows.Add(sceneObject.Uid, hullShadow);

                sceneObject.Destroy += () =>
                {
                    Shadows.Remove(sceneObject.Uid);
                    penumbra.Lights.Remove(hullShadow);
                };
            }

            hullShadow.Position = pos;
        }
    }
}