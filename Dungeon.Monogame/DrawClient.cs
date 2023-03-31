using Dungeon.Resources;
using Dungeon.SceneObjects;
using Dungeon.Types;
using Dungeon.View.Enums;
using Dungeon.View.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Penumbra;
using ProjectMercury;
using ProjectMercury.Renderers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using static System.Net.WebRequestMethods;
using Matrix = Microsoft.Xna.Framework.Matrix;
using Rect = Dungeon.Types.Square;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Dungeon.Monogame
{
    public class DrawClient : IDisposable
    {
        /// settings

        private static readonly string DefaultFontXnbExistedFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.Resources.Fonts.xnb.Montserrat.Montserrat10.xnb";
        public SpriteBatchManager SpriteBatchManager;
        private ImageLoader ImageLoader;
        private PixelTexture PixelTexture;
        private GraphicsDevice GraphicsDevice;
        private PenumbraComponent penumbra;
        private ContentManager Content;
        private ParticleRenderer ParticleRenderer;
        private ICamera camera;
        private Matrix? _resolutionMatrix;

        /// debugging

        public static bool BoundMode = false;

        /// cache

        private static readonly Dictionary<string, ParticleEffect> ParticleEffects = new();
        private static readonly Dictionary<string, Light> LightsInstances = new();
        private readonly Dictionary<string, RenderTarget2D> BatchCache = new();
        private readonly Dictionary<string, Rect> TileSetCache = new();
        private readonly Dictionary<string, Rect> PosCache = new();
        private readonly Dictionary<string, Texture2D> DrawablePathCache = new();
        private readonly Dictionary<string, SpriteFont> SpriteFontCache = new();
        private readonly Dictionary<Color, Texture2D> dummyTextureCache = new();

        public DrawClient(GraphicsDevice graphicsDevice, ContentManager content, ImageLoader imageLoader, ParticleRenderer particleRenderer, PenumbraComponent penumbra=null, ICamera camera = null)
        {
            this.GraphicsDevice = graphicsDevice;
            this.penumbra = penumbra;
            this.Content = content;
            this.camera = camera;
            this.ParticleRenderer = particleRenderer;
            this.ImageLoader = imageLoader;
            this.PixelTexture=new PixelTexture(graphicsDevice);
        }

        public void ChangeResolution(Matrix? resolutionMatrix)
        {
            this._resolutionMatrix=resolutionMatrix;
        }

        public void Draw(ISceneLayer layer, RenderTarget2D target, GameTime gameTime, bool lightning)
        {
            if (lightning)
                lightning=penumbra!=null;

            var sceneObjects = layer.Objects
                .Where(x => x.Visible);

            if (camera!=null)
                sceneObjects = sceneObjects.Where(x => camera.InCamera(x) || x.DrawOutOfSight);

            var isAbsolute = layer.AbsoluteLayer;

            GraphicsDevice.SetRenderTarget(target);

            if (lightning)
                penumbra?.BeginDraw();

            GraphicsDevice.Clear(Color.Transparent);
            
            SpriteBatchManager.Begin(_resolutionMatrix);

            var spriteBatch = SpriteBatchManager.GetSpriteBatch();

            foreach (var sceneObject in sceneObjects)
            {
                DrawSceneObject(sceneObject, target, spriteBatch, gameTime);
            }

            SpriteBatchManager.End();

            if (lightning)
                penumbra?.Draw(gameTime);

            DrawLights(layer);
        }

        private void DrawSceneObject(ISceneObject sceneObject, RenderTarget2D renderTarget, SpriteBatchKnowed spriteBatch, GameTime gameTime, double xParent = 0, double yParent = 0, bool batching = false, double parentScale = 0)
        {
            if (!sceneObject.Visible)
                return;

            sceneObject.Drawed = true;

            bool needScalePosition = false;

            var scale_ = sceneObject.GetScaleValue();
            if (parentScale != 0) {
                scale_ = parentScale;
                needScalePosition = true;
            }

            if (scale_ == 0)
                scale_ = 1;

            var x = 0d;
            var y = 0d;

            if (!batching)
            {
                x = xParent + (float)sceneObject.Left * (needScalePosition ? scale_ : 1);
                y = yParent + (float)sceneObject.Top * (needScalePosition ? scale_ : 1);
            }

            DrawLight(sceneObject, x, y);
            DrawEffects(sceneObject, x, y,gameTime);

            int width = (int)Math.Round((sceneObject.Width * scale_));
            int height = (int)Math.Round((sceneObject.Height * scale_) );

            if (sceneObject.IsBatch && !batching)
            {
                if (sceneObject.Expired || !BatchCache.TryGetValue(sceneObject.Uid, out var bitmap))
                {
                    bitmap = new RenderTarget2D(GraphicsDevice, width, height, false, GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);

                    GraphicsDevice.SetRenderTargets(bitmap);
                    GraphicsDevice.Clear(Color.Transparent);

                    var localSpriteBatch = SpriteBatchManager.GetSpriteBatch(isTransformMatrix: false);

                    DrawSceneObject(sceneObject, renderTarget, localSpriteBatch, gameTime, 0, 0, true);

                    TileSetCache[sceneObject.Uid] = new Rect(0, 0, width, height);
                    PosCache[sceneObject.Uid] = new Rect(x, y, width, height);

                    BatchCache[sceneObject.Uid] = bitmap;

                    localSpriteBatch.End();

                    GraphicsDevice.SetRenderTarget(renderTarget);
                }

                TileSetCache.TryGetValue(sceneObject.Uid, out var tilesetPos);
                PosCache.TryGetValue(sceneObject.Uid, out var sceneObjPos);

                if (!sceneObject.CachePosition)
                {
                    sceneObjPos = new Rect(x, y, width, height);
                }

                if (sceneObject.PerPixelCollision && sceneObject.Texture == null)
                {
                    sceneObject.Texture = new Texture2DAdapter(bitmap, sceneObject);
                }

                spriteBatch.Draw(bitmap, new Vector2(sceneObjPos.Xf, sceneObjPos.Yf), new Microsoft.Xna.Framework.Rectangle(tilesetPos.Xi, tilesetPos.Yi, tilesetPos.Widthi, tilesetPos.Heighti), Color.White);
            }
            else
            {
                if (!string.IsNullOrEmpty(sceneObject.Image))
                {
                    DrawSceneImage(sceneObject, y, x);
                }

#warning potencial performance harm!
                if (sceneObject.TileMap != default)
                {
                    var tls = sceneObject.TileMap.Tiles;
                    if (camera!=null)
                        tls=tls.Where(t => camera.InCamera(t)).ToList();
                    tls.ForEach(t => DrawSceneTile(t, sceneObject, y, x));
                }

                if (sceneObject.Path != null || BoundMode)
                {
                    if (BoundMode)
                    {
                        DrawScenePath(new Dungeon.Drawing.Impl.DrawablePath()
                        {
                            Fill=true,
                            ForegroundColor = Drawing.DrawColor.Red,
                            BackgroundColor = Drawing.DrawColor.Red,
                            Depth = 5,
                            PathPredefined = PathPredefined.Rectangle,
                            Region = sceneObject.BoundPosition
                        },x,y);
                    }
                    else
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
                        DrawSceneText(text.Size, textY, textX, range, sceneObject);

                        if (range.StringData == Environment.NewLine)
                        {
                            textX = x;
                            textY += MeasureText(prev).Y;
                        }
                        else
                        {
                            textX += this.MeasureText(range, sceneObject).X;// range.Length * range.LetterSpacing;
                        }

                        prev = range;
                    }
                }

                var childrens = sceneObject.Children.OrderBy(c => c.LayerLevel).ToArray();

                for (int i = 0; i < childrens.Length; i++)
                {
                    var child = childrens.ElementAtOrDefault(i);
                    if (child != null)
                    {
                        DrawSceneObject(child, renderTarget, spriteBatch, gameTime, x, y, batching, sceneObject.Scale);
                    }
                }
            }
        }

        private void DrawSceneTile(ITile tile, ISceneObject sceneObject, double y, double x)
        {
            var image = ImageLoader.LoadTexture2D(tile.Source);
            if (image == default)
            {
                DungeonGlobal.Logger.Log("Медленный рендер тайла из-за отсутствия картинки!");
                //Debugger.Break();
                return;
            }

            var source = new Rectangle(tile.X, tile.Y, tile.Width, tile.Height);
            var dest = new Rectangle(tile.Left+(int)x, tile.Top+ (int)y, tile.Width, tile.Height);

            SpriteEffects spriteEffects = SpriteEffects.None;
                       
            var color = Color.White;
            var drawColor = new Color(color.R, color.G, color.B, color.A);

            var angle = (float)sceneObject.Angle;
            var origin = angle != 0
                ? new Vector2(tile.TileRegion.Widthi / 2f, tile.TileRegion.Heighti / 2f)
                : Vector2.Zero;

            if (sceneObject.Blur)
            {
                var sb = SpriteBatchManager.GetSpriteBatch(SamplerState.LinearWrap);

                if (sceneObject.Scale > 0)
                {
                    sb.Draw(image, new Vector2(dest.X, dest.Y), source, drawColor, 0, origin, (float)sceneObject.Scale, spriteEffects, 0f);
                }
                else
                {
                    sb.Draw(image, dest, source, drawColor, angle, origin, spriteEffects, 0f);
                }
            }
            else
            {
                var sb = SpriteBatchManager.GetSpriteBatch();
                if (sceneObject.Scale > 0)
                {

                    if (sceneObject.ScaleAndResize)
                    {
                        //spriteBatch.Draw(image, destinationRectangle: dest, sourceRectangle: source, origin: origin, rotation: angle, scale: new Vector2((float)sceneObject.Scale), color: color, effects: spriteEffects, layerDepth: 0);
                        sb.Draw(image, new Vector2((float)tile.Left, (float)tile.Top), source, color, angle, origin, new Vector2((float)sceneObject.Scale), spriteEffects, 0);
                    }
                    else
                    {
                        sb.Draw(image, new Vector2(dest.X, dest.Y), source, drawColor, angle, origin, (float)sceneObject.Scale, spriteEffects, 0f);
                    }
                }
                else if (sceneObject.AlphaBlend)
                {
                    var linearSb = SpriteBatchManager.GetSpriteBatch(SamplerState.LinearWrap);
                    linearSb.Draw(image, dest, source, drawColor, angle, origin, spriteEffects, 0f);
                }
                else
                {
                    sb.Draw(image, dest, source, drawColor, angle, origin, spriteEffects, 0f);
                }
            }
        }

        private void DrawSceneImage(ISceneObject sceneObject, double y, double x)
        {
            if (sceneObject.Opacity == 0)
                return;

            sceneObject.Drawing();

            Texture2D image = ImageLoader.LoadTexture2D(sceneObject.Image, sceneObject);

            if (image == default)
            {
                DungeonGlobal.Logger.Log("Медленный рендер из-за отсутствия картинки!");
                //Debugger.Break();
                return;
            }
            else if (sceneObject.PerPixelCollision && sceneObject.Texture == null)
            {
                sceneObject.Texture = new Texture2DAdapter(image, sceneObject);
            }

            Rect tileRegion = default;

            if (sceneObject.DrawPartInSight)
            {
                image = sceneObject.GetPropertyExpr<Texture2D>("CroppedImage");

                var r = DungeonGlobal.Resolution;
                tileRegion = new Rect(0, 0, r.Width, r.Height);
            }
            else
            if (!TileSetCache.TryGetValue(sceneObject.Uid, out tileRegion))
            {
                if (sceneObject.ImageRegion == default(Square))
                {
                    tileRegion = new Rect(0, 0, image.Width, image.Height);
                }
                else
                {
                    var tilePos = sceneObject.ImageRegion;
                    tileRegion = new Rect(tilePos.X, tilePos.Y, tilePos.Width, tilePos.Height);
                }

                if (sceneObject.CacheAvailable)
                {
                    TileSetCache.Add(sceneObject.Uid, tileRegion);
                }
            }
            

            if (!PosCache.TryGetValue(sceneObject.Uid, out Rect pos) || sceneObject.Expired)
            {
                if (PosCache.ContainsKey(sceneObject.Uid))
                {
                    PosCache.Remove(sceneObject.Uid);
                }

                double width = sceneObject.BoundPosition.Width;
                double height = sceneObject.BoundPosition.Height;

                if (width == 0 && height == 0)
                {
                    width = image.Width;
                    height = image.Height;

#warning sceneobject bind size by image [AutoBindSceneObjectSizeByContainedImage]
                    if (sceneObject.AutoBindSceneObjectSizeByContainedImage) {
                        sceneObject.Width = width;
                        sceneObject.Height = height;
                    }
                }

                pos = new Rect(x, y, width, height);

                if (sceneObject.CacheAvailable)
                {
                    PosCache[sceneObject.Uid]= pos;
                    sceneObject.Expired = false;
                }
            }

            if (sceneObject.DrawPartInSight)
            {
                pos = tileRegion;
            }

#warning [INFO] Вот здесь теперь всё хорошо,но это место можно использовать для того что бы заоптимизировать преобразование размеров, т.к. масштабирование текстур происходит тут

            var dest = new Rectangle(pos.Xi, pos.Yi, pos.Widthi, pos.Heighti);

            var angle = (float)sceneObject.Angle;
            var origin = angle != 0
                ? new Vector2(tileRegion.Widthi / 2f, tileRegion.Heighti / 2f)
                : Vector2.Zero;

            SpriteEffects spriteEffects = SpriteEffects.None;

            //if (sceneObject.ImageMask != default && sceneObject.ImageMask.Visible)
            //{
            //    var maskResult = ApplyImageMask(image, sceneObject);
            //    image = maskResult.image;
            //    spriteEffects = maskResult.effects;
            //}

            if (sceneObject.Flip != FlipStrategy.None)
            {
                if (sceneObject.Flip == FlipStrategy.Both)
                    spriteEffects = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
                else if (sceneObject.Flip == FlipStrategy.Horizontally)
                    spriteEffects = SpriteEffects.FlipHorizontally;
                else if (sceneObject.Flip == FlipStrategy.Vertically)
                    spriteEffects = SpriteEffects.FlipVertically;
            }

            Rectangle source = new Rectangle(tileRegion.Xi, tileRegion.Yi, tileRegion.Widthi, tileRegion.Heighti);

            var color =  Color.White;
            if (sceneObject.Color != default)
            {
                var dcol = sceneObject.Color;
                color = new Color(dcol.R, dcol.G, dcol.B, dcol.A);
            }

            var alpha = sceneObject.Opacity == 0
                   ? color.A
                   : sceneObject.Opacity;

            var drawColor = new Color(color.R, color.G, color.B);
            drawColor.A=color.A;
            //(float)alpha);

            IEffect effect = null;
            if(sceneObject.IsMonochrome)
            {
                effect = new NamedEffect("Greyscale");
            }

            var samplerstate = SamplerState.PointWrap;

            if (sceneObject.Blur)
            {
                samplerstate =sceneObject.Mode == DrawMode.Normal
                    ? SamplerState.LinearClamp
                    : SamplerState.LinearWrap;

                var sb = SpriteBatchManager.GetSpriteBatch(samplerstate);

                if (sceneObject.Scale > 0)
                {
                    sb.Draw(image, new Vector2(dest.X, dest.Y), source, drawColor, angle, origin, (float)sceneObject.Scale, spriteEffects, 0f);
                }
                else
                {
                    sb.Draw(image, dest, source, drawColor, angle, origin, spriteEffects, 0f);
                }
            }
            else
            {
                samplerstate =/*sceneObject.Mode == DrawMode.Normal
                    ? SamplerState.PointWrap
                    : */SamplerState.LinearWrap;

                var sb = SpriteBatchManager.GetSpriteBatch(samplerstate);

#warning как бы здесь не потерять Mode.Tiled
                //if (sceneObject.Mode!= DrawMode.Normal)
                //{
                //    //BeginDraw(samplerstate, filter: sceneObject.Filtered);
                //}

                if (sceneObject.Scale > 0)
                {
                    if (sceneObject.ScaleAndResize)
                    {
                        //spriteBatch.Draw(image, destinationRectangle: dest, sourceRectangle: source, origin: origin, rotation: angle, scale: new Vector2((float)sceneObject.Scale), color: color, effects: spriteEffects, layerDepth: 0);
                        sb.Draw(image, new Vector2((float)pos.Xi, (float)pos.Yi), source, color, angle, origin, new Vector2((float)sceneObject.Scale), spriteEffects, 0);
                    }
                    else
                    {
                        sb.Draw(image, new Vector2(dest.X, dest.Y), source, drawColor, angle, origin, (float)sceneObject.Scale, spriteEffects, 0f);
                    }
                }
                else
                {
                    sb.Draw(image, dest, source, drawColor, angle, origin, spriteEffects, 0f);
                }

                //BeginDraw(filter: sceneObject.Filtered);
            }
        }

        private void DrawSceneText(float fontSize, double y, double x, IDrawText range, ISceneObject sceneObject)
        {
            bool fontWeight = range.Bold;

            SpriteFont spriteFont;

            if (string.IsNullOrEmpty(range.FontName))
            {
                if (range.CompiledFontName == default)
                {
                    range.CompiledFontName = $"{DungeonGlobal.GameAssemblyName}.Resources.Fonts.xnb/{DungeonGlobal.DefaultFontName}/{DungeonGlobal.DefaultFontName}{DungeonGlobal.DefaultFontSize}.xnb".Embedded();
                }
                var font = range.CompiledFontName;

                var resFont = ResourceLoader.Load(font);
                if (resFont == default)
                    return;

                if (!SpriteFontCache.TryGetValue(font, out spriteFont))
                {
                    SpriteFontCache[font] =spriteFont = Content.Load<SpriteFont>(font, resFont.Stream);
                }
            }
            else
            {
                if (range.CompiledFontName == default)
                {
                    range.CompiledFontName = $"{DungeonGlobal.GameAssemblyName}.Resources.Fonts.xnb/{range.FontName}/{range.FontName}{range.Size}.xnb".Embedded();
                }
                var font = range.CompiledFontName;

                var resFont = ResourceLoader.Load(font);
                if (resFont == default)
                    return;

                if (!SpriteFontCache.TryGetValue(font, out spriteFont))
                {
                    SpriteFontCache[font] =spriteFont = Content.Load<SpriteFont>(font, resFont.Stream);
                }
            }

            var baseLineSpacing = spriteFont.LineSpacing;

            if (range.LineSpacing != 0)
            {
                spriteFont.LineSpacing = range.LineSpacing;
            }

            var txt = range.StringData;

            var componentWidth = sceneObject.BoundPosition.Width;
            if (range.WordWrap && componentWidth > 0)
            {
                txt = TextWrapper.WrapText(spriteFont, txt, componentWidth);
            }

            var alpha = /*sceneObject.Opacity == 0
                   ?*/ range.ForegroundColor.A/*
                   : sceneObject.Opacity*/;

            if (sceneObject.Opacity > 0 && sceneObject.Opacity < 1)
            {
                var new_value = sceneObject.Opacity * 255;
                alpha = Convert.ToByte((int)Math.Round(new_value));
            }

            var color = new Color(range.ForegroundColor.R, range.ForegroundColor.G, range.ForegroundColor.B, (byte)alpha);

            var sb = SpriteBatchManager.GetSpriteBatch(SamplerState.LinearWrap);

            if (sceneObject.Scale > 0)
            {
                sb.DrawString(spriteFont, txt, new Vector2((int)x, (int)y), color, 0, Vector2.Zero, (float)sceneObject.Scale, SpriteEffects.None, 1);
            }
            else
            {
                sb.DrawString(spriteFont, txt, new Vector2((int)x, (int)y), color);
            }

            spriteFont.LineSpacing = baseLineSpacing;
        }

        private void DrawScenePath(IDrawablePath drawablePath, double x, double y)
        {
            if (drawablePath.PathPredefined == PathPredefined.Rectangle)
            {
                var drawColor = drawablePath.BackgroundColor.ToColor();
                var pathReg = drawablePath.Region;

                var rect = new Rectangle((int)x, (int)y, (int)(pathReg.Width), (int)(pathReg.Height));
                var cornerRadius = drawablePath.Radius;

                if (drawablePath.Fill)
                {
                    if (!dummyTextureCache.TryGetValue(drawColor, out var dummyTexture))
                    {
                        dummyTextureCache.Add(drawColor, dummyTexture= new Texture2D(GraphicsDevice, 1, 1));
                        dummyTexture.SetData(new Color[] { drawColor });
                    }
                    var sb = SpriteBatchManager.GetSpriteBatch(SamplerState.LinearWrap);
                    sb.Draw(dummyTexture, rect, drawColor);
                }
                else
                {
                    var depth = (int)Math.Round(drawablePath.Depth);
                    if (depth == 0)
                    {
                        depth = 1;
                    }
                    DrawBorder(rect, depth, drawColor, drawablePath);
                }
            }

            if (drawablePath.PathPredefined == PathPredefined.Line)
            {
                Texture2D texture = default;

                if (drawablePath.Texture != string.Empty)
                {
                    texture = ImageLoader.LoadTexture2D(drawablePath.Texture);
                }

                var from = new Dot(drawablePath.Path.First());
                from.X += x;
                from.Y += y;

                var to = new Dot(drawablePath.Path.Last());
                to.X += x;
                to.Y += y;

                var fromVector = new Vector2(from.Xf, from.Yf);
                var toVector = new Vector2(to.Xf, to.Yf);

                var drawColor = drawablePath.BackgroundColor.ToColor();

                DrawLineTo(texture, fromVector, toVector, drawColor, (int)drawablePath.Depth);
            }
        }

        public void DrawLineTo(Texture2D texture, Vector2 src, Vector2 dst, Color color, int depth)
        {
            if (texture == default)
            {
                texture = PixelTexture.Get();
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
            var sb = SpriteBatchManager.GetSpriteBatch(SamplerState.LinearWrap);
            sb.Draw(texture, src, new Rectangle((int)src.X, (int)src.Y, (int)distance, depth), color, angle, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
        }

        /// <summary>
        /// Will draw a border (hollow rectangle) of the given 'thicknessOfBorder' (in pixels)
        /// of the specified color.
        /// By Sean Colombo, from http://bluelinegamestudios.com/blog
        /// </para>
        /// </summary>
        /// <param name="rectangleToDraw"></param>
        /// <param name="thicknessOfBorder"></param>
        private void DrawBorder(Rectangle rectangleToDraw, int thicknessOfBorder, Color borderColor, IDrawablePath drawablePath)
        {
            var sb = SpriteBatchManager.GetSpriteBatch(SamplerState.LinearWrap);
            var pixel = PixelTexture.Get();

            // Draw top line
            sb.Draw(pixel, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, rectangleToDraw.Width, thicknessOfBorder), borderColor);

            // Draw left line
            sb.Draw(pixel, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, thicknessOfBorder, rectangleToDraw.Height), borderColor);

            // Draw right line
            sb.Draw(pixel, new Rectangle((rectangleToDraw.X + rectangleToDraw.Width - thicknessOfBorder),
                                            rectangleToDraw.Y,
                                            thicknessOfBorder,
                                            rectangleToDraw.Height), borderColor);
            // Draw bottom line
            sb.Draw(pixel, new Rectangle(rectangleToDraw.X,
                                            rectangleToDraw.Y + rectangleToDraw.Height - thicknessOfBorder,
                                            rectangleToDraw.Width,
                                            thicknessOfBorder), borderColor);
        }

        private void DrawLight(ISceneObject sceneObject, double x, double y)
        {
            if (sceneObject.Light == null)
                return;

            var objLight = sceneObject.Light;

            var xf = x + (camera?.CameraOffsetX ?? 0);
            var yf = y + (camera?.CameraOffsetY ?? 0);

            xf += sceneObject.BoundPosition.Width / 2;
            yf += sceneObject.BoundPosition.Height / 2;

            var pos = new Vector2((float)xf, (float)yf);

            var objLightColor = objLight.Color;
            var color = new Color(objLightColor.R, objLightColor.G, objLightColor.B, objLightColor.A);

            if (penumbra != default)
            {
                if (objLight.Updated && LightsInstances.ContainsKey(sceneObject.Uid))
                {
                    var oldLight = LightsInstances[sceneObject.Uid];
                    penumbra.Lights.Remove(oldLight);
                    LightsInstances.Remove(sceneObject.Uid);
                    objLight.Updated=false;
                }

                if (!LightsInstances.TryGetValue(sceneObject.Uid, out var light))
                {
                    switch (objLight.Type)
                    {
                        case LightType.Point:
                            light = new PointLight()
                            {
                                Scale = new Vector2(objLight.Range*100),
                                ShadowType = ShadowType.Illuminated,
                                Radius = sceneObject.Light.Range,
                                Position = pos,
                                Color = color
                            };
                            break;
                        case LightType.Spot:

                            light = new Spotlight()
                            {
                                Scale = new Vector2(objLight.Range*100),
                                ShadowType = ShadowType.Illuminated,
                                Radius = sceneObject.Light.Range,
                                Position = pos,
                                Color = color
                            };
                            break;
                        case LightType.Texture:

                            light = new TexturedLight(ImageLoader.LoadTexture2D(objLight.Image))
                            {
                                Scale = new Vector2(objLight.Range*100),
                                ShadowType = ShadowType.Illuminated,
                                Radius = sceneObject.Light.Range,
                                Position = pos,
                                Color = color
                            };
                            break;
                        default:
                            break;
                    }

                    penumbra.Lights.Add(light);
                    LightsInstances[sceneObject.Uid] = light;

                    sceneObject.OnDestroy += () =>
                    {
                        LightsInstances.Remove(sceneObject.Uid);
                        penumbra.Lights.Remove(light);
                    };
                }

                light.Position = pos;
            }
        }

        private void DrawEffects(ISceneObject sceneObject, double x, double y, GameTime gameTime)
        {
            if (sceneObject.ParticleEffects==default || sceneObject.ParticleEffects.Count == 0)
                return;

            x = (int)x;
            y = (int)y;

            foreach (var effect in sceneObject.ParticleEffects)
            {

                var path = $"{effect.Assembly}.Resources.Particles.{effect.Name}.xml";

                void DrawEffect()
                {
                    if (ParticleRenderer == default)
                        return;

                    if (!ParticleEffects.TryGetValue(sceneObject.Uid, out var particleEffect))
                    {
                        var particleRes = ResourceLoader.Load(path);
                        if (particleRes==default)
                            return;

                        var loader = new ParticleEffectLoader(particleRes.Stream, effect.Assembly);

                        particleEffect = loader.Load();
                        particleEffect.Scale = (float)effect.Scale;
                        particleEffect.LoadContent(this.Content);
                        particleEffect.Initialise();

                        ParticleEffects.Add(sceneObject.Uid, particleEffect);

                        sceneObject.OnDestroy += () => ParticleEffects.Remove(sceneObject.Uid);

                        ParticleRenderer.LoadContent(Content);
                    }


                    if (!effect.Once || effect.TriggerCount>0)
                    {
                        var pos = new Vector2((float)x, (float)y);
                        particleEffect.Trigger(pos);

                        effect.TriggerCount--;
                    }

                    var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                    particleEffect.Update(deltaTime);

#warning Здесь вызывался spriteBatch.End(); перед тем как работает ParticleRenderer.RenderEffect, видимо на частицах придётся здесь переинициализировать

                    var v = Matrix.CreateTranslation(-(float)x, -(float)y, 0)
                        * Matrix.CreateScale((float)effect.Scale)
                        * Matrix.CreateTranslation((float)x, (float)y, 0)
                        * Matrix.CreateTranslation((float)(camera?.CameraOffsetX ?? 0), (float)(camera?.CameraOffsetY?? 0), 0);

                    ParticleRenderer.RenderEffect(particleEffect, ref v);

#warning Здесь вызывался BeginDraw(); после того как отработает ParticleRenderer.RenderEffect
                }

                DrawEffect();

                //Once.Call(DrawEffect, "XNACLIENT" + nameof(Draw) + nameof(DrawEffect) + path);
            }
        }

        public void CacheImage(string image) => ImageLoader.LoadTexture2D(image);

        public void Clear(IDrawColor drawColor = null)
        {
            var xnacolor = Color.Transparent;
            if (drawColor != default)
            {
                xnacolor = new Color(drawColor.R, drawColor.G, drawColor.B, drawColor.A);
            }
            GraphicsDevice.Clear(xnacolor);
        }

        public Dot MeasureText(IDrawText drawText, ISceneObject parent = default)
        {
            string customFontName = null;
            if (drawText.FontName != null)
            {
                customFontName = $"{drawText.FontAssembly}.Resources.Fonts.xnb.{drawText.FontName}/{drawText.FontName}{drawText.Size}.xnb".Embedded();
            }

            if (customFontName == default)
            {
                customFontName = $"{drawText.FontAssembly}.Resources.Fonts/xnb/{DungeonGlobal.DefaultFontName}/{DungeonGlobal.DefaultFontName}{DungeonGlobal.DefaultFontSize}.xnb".Embedded();
            }

            SpriteFont font = default;
            var resFont = ResourceLoader.Load(customFontName, false, false);

            if (resFont != default)
            {
                font = Content.Load<SpriteFont>(customFontName, resFont.Stream);
            }
            else
            {
                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(DefaultFontXnbExistedFile))
                {
                    if (stream.CanSeek)
                    {
                        stream.Seek(0, SeekOrigin.Begin);
                    }

                    font = Content.Load<SpriteFont>(DefaultFontXnbExistedFile, stream);
                }
            }

            var data = drawText.StringData;

            if (drawText.WordWrap && parent != default)
            {
                var parentWidth = parent.Width;
                if (parentWidth > 0)
                {
                    data = TextWrapper.WrapText(font, data, parentWidth, dtext: drawText);
                }
            }

            var m = font.MeasureString(data);

            return new Dungeon.Types.Dot(m.X, m.Y);
        }

        public Dot MeasureImage(string image)
        {
            var img = ImageLoader.LoadTexture2D(image);
            if (img == default)
                return new Dungeon.Types.Dot();

            return new Dungeon.Types.Dot()
            {
                X = img.Width,
                Y = img.Height
            };
        }

        /// <summary>
        /// Что-то надо с этим делать, жуйня какая-то, и не понятно почему вызывается тут
        /// </summary>
        /// <param name="layer"></param>
        private void DrawLights(ISceneLayer layer)
        {
            //draw lights
            List<string> lightsfordelete = new List<string>();
            foreach (var light in LightsInstances)
            {
                var sceneObj = layer?.Objects?.FirstOrDefault(o => light.Key == o.Uid);
                if (sceneObj != default)
                    if (!sceneObj.Visible || (camera!=null && !camera.InCamera(sceneObj)))
                    {
                        lightsfordelete.Add(light.Key);
                    }
            }
            foreach (var lightDelete in lightsfordelete)
            {
                LightsInstances.Remove(lightDelete);
            }
        }

        public void Dispose()
        {
            SpriteBatchManager.Dispose();
        }
    }
}
