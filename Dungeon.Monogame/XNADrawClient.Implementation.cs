using Dungeon;
using Dungeon.Resources;
using Dungeon.Scenes;
using Dungeon.Types;
using Dungeon.View.Enums;
using Dungeon.View.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;
using Penumbra;
using ProjectMercury;
using ProjectMercury.Renderers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using Rect = Dungeon.Types.Rectangle;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Dungeon.Monogame
{
    public class XNADrawClientImplementation
    {
        private int _cell;
        public int cell
        {
            get => cellMode ? _cell : 1;
            set => _cell = value;
        }
        private bool cellMode = false;
        private GraphicsDevice GraphicsDevice;
        private PenumbraComponent penumbra;
        private SpriteBatch spriteBatch;
        private Effect GlobalImageFilter;
        ContentManager Content; 
        Renderer myRenderer;
        private Microsoft.Xna.Framework.GameTime gameTime;

        public ISceneLayer layer { get; set; }

        public IScene scene { get; set; }

        public ICamera Camera;

        public XNADrawClientImplementation(GraphicsDevice graphicsDevice, PenumbraComponent penumbra, SpriteBatch spriteBatch,int cell,Effect globalImageFilter,ContentManager content, ICamera camera, Renderer myRenderer, bool cellMode=false)
        {
            GraphicsDevice = graphicsDevice;
            this.cellMode = cellMode;
            this.penumbra = penumbra;
            this.spriteBatch = spriteBatch;
            this.cell = cell;
            this.GlobalImageFilter = globalImageFilter;
            this.Content = content;
            this.Camera = camera;
            this.myRenderer = myRenderer;
        }

        public RenderTarget2D target;

        private bool useLight;

        public void Draw(ISceneObject[] sceneObjects, Microsoft.Xna.Framework.GameTime gameTime, RenderTarget2D target = default, bool useLight = false,bool clear=false,Matrix? resolutionMatrix=default, ISceneLayer layer=default)
        {
            this.useLight = useLight;
            this.target = target;

            this.gameTime = gameTime;

            InterfaceObjects.Clear();

            var all = sceneObjects
                .Where(x => x.Visible && (x.DrawOutOfSight || (!x.DrawOutOfSight && Camera.InCamera(x))))
                .ToArray();

            var isAbsoluteScene = layer != default
                ? layer.AbsoluteLayer
                : (scene?.AbsolutePositionScene ?? false);

            var absolute = all
                .Where(x => x.AbsolutePosition || isAbsoluteScene)
                .OrderBy(x => x.LayerLevel).ToArray();

            var offsetted = all
                .Where(x => !isAbsoluteScene && !x.AbsolutePosition)
                .OrderBy(x => x.LayerLevel).ToArray();

            GraphicsDevice.SetRenderTarget(target);
            GraphicsDevice.Clear(Color.Transparent);

#if Core
            if (useLight)
                penumbra?.BeginDraw();
#endif
            SetSpriteBatch();

            foreach (var offsetSceneObject in offsetted)
            {
                DrawSceneObject(offsetSceneObject);
            }
            spriteBatch.End();

#if Core
            if (useLight)
                penumbra?.Draw(gameTime);
#endif
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

            //draw lights
            List<string> lightsfordelete = new List<string>();
            foreach (var light in Lights)
            {
                var sceneObj = this.layer?.Objects?.FirstOrDefault(o => light.Key == o.Uid);
                if (sceneObj != default)
                    if (!Camera.InCamera(sceneObj) || !sceneObj.Visible)
                    {
                        lightsfordelete.Add(light.Key);
                    }
            }
            foreach (var lightDelete in lightsfordelete)
            {
                Lights.Remove(lightDelete);
            }
        }

        private void SetSpriteBatch(bool absolute = false, bool @interface = false, double scale=0)
        {
            Matrix scaleMatrix = Matrix.Identity;
            if (Camera.CameraOffsetZ != 0)
            {
                var scaleVal = 1 + (Camera.CameraOffsetZ * 0.1);

                scaleMatrix = Matrix.CreateScale((float)scaleVal);
            }

            if (scale != 0)
            {
                scaleMatrix = Matrix.CreateScale((float)scale);
            }

            if (!absolute)
            {
                SpriteBatchRestore = (smooth, filter, alphaBlend) => spriteBatch.Begin(
                    transformMatrix: Matrix.CreateTranslation((float)Camera.CameraOffsetX, (float)Camera.CameraOffsetY,0) * scaleMatrix,
                    samplerState: !smooth ? SamplerState.PointWrap : SamplerState.LinearClamp,
                    blendState: useLight || alphaBlend ? BlendState.AlphaBlend : BlendState.NonPremultiplied, effect: filter ? GlobalImageFilter : null
                    );
            }
            else
            {
                SpriteBatchRestore = (smooth, filter, alphaBlend) => spriteBatch.Begin(
                    transformMatrix: scaleMatrix,
                    samplerState: !smooth ? SamplerState.PointWrap : SamplerState.LinearClamp,
                    blendState: useLight || alphaBlend ? BlendState.AlphaBlend : BlendState.NonPremultiplied/*, effect: @interface ? null : GlobalImageFilter*/);
            }
            currentAbsolute = absolute;
            currentAbsolute=@interface;
            SpriteBatchRestore.Invoke(false, true);
        }

        private bool currentAbsolute = false;
        private bool currentInterface = false;
        private SpriteBatchRestoring SpriteBatchRestore = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="LinearClamp"></param>
        /// <param name="globalImageFilter">[Obsolete] use pre+post filters for layer</param>
        private delegate void SpriteBatchRestoring(bool LinearClamp, bool globalImageFilter,bool alphaBlend=false);

        private static readonly string DefaultFontXnbExistedFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.Resources.Fonts.xnb.Montserrat.Montserrat10.xnb";

        public Types.Point MeasureText(IDrawText drawText, ISceneObject parent = default)
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
                var parentWidth = parent.BoundPosition.Width;
                if (parentWidth > 0)
                {
                    data = WrapText(font, data, parentWidth * cell);
                }
            }

            var m = font.MeasureString(data);

            return new Dungeon.Types.Point(m.X, m.Y);
        }

        public Dungeon.Types.Point MeasureImage(string image)
        {
            var img = TileSetByName(image);
            if (img == default)
                return new Dungeon.Types.Point();

            return new Dungeon.Types.Point()
            {
                X = img.Width,
                Y = img.Height
            };
        }

        public void SaveObject(ISceneObject sceneObject, string path=default, Dungeon.Types.Point offset=default, string runtimeCacheName = null)
        {
            if (path == default && runtimeCacheName == default)
            {
                throw new ArgumentException($"At least one parameter must be setted: {nameof(path)} or {nameof(runtimeCacheName)}");
            }

            if (offset == default)
            {
                offset = Types.Point.Zero;
            }

            int w = (int)sceneObject.Width * cell;
            int h = (int)sceneObject.Height * cell;

            var bitmap = new RenderTarget2D(GraphicsDevice, w, h, false, GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);

            try
            {
                spriteBatch.End();
            }
            catch { }

            GraphicsDevice.SetRenderTargets(bitmap);
            GraphicsDevice.Clear(Color.Transparent);
            SpriteBatchRestore.Invoke(false, sceneObject.Filtered);

            DrawSceneObject(sceneObject, offset.X, offset.Y, true, true, true);

            spriteBatch.End();
            GraphicsDevice.SetRenderTarget(target ?? null);

            using (var f = File.Create(path))
            {
                bitmap.SaveAsPng(f, w, h);
            }

            if (runtimeCacheName != default)
            {
                tilesetsCache.Add(runtimeCacheName, bitmap);
            }
        }

        public void Animate(IAnimationSession animationSession)
        {
            throw new System.NotImplementedException();
        }

        private readonly Dictionary<string, RenderTarget2D> BatchCache = new Dictionary<string, RenderTarget2D>();
        private Dictionary<string, Rect> TileSetCache = new Dictionary<string, Rect>();
        private Dictionary<string, Rect> PosCache = new Dictionary<string, Rect>();
        private static readonly Dictionary<string, Texture2D> tilesetsCache = new Dictionary<string, Texture2D>();

        /// <summary>
        /// TODO: нужно логирование что бы игра не падала но можно было понять причину сбоя
        /// </summary>
        /// <param name="tilesetName"></param>
        /// <returns></returns>
        public Texture2D TileSetByName(string tilesetName)
        {
            if (!tilesetsCache.TryGetValue(tilesetName, out var bitmap))
            {
#if Android
                tilesetName = tilesetName.Replace("Dungeon.Resources.","Dungeon.Resources.Android.");
#endif
                var res = ResourceLoader.Load(tilesetName);
                if (res == default)
                    return default;

                bitmap = Texture2D.FromStream(GraphicsDevice, res.Stream);

                tilesetsCache.TryAdd(tilesetName, bitmap);

                res.OnDispose += () =>
                {
                    tilesetsCache.Remove(tilesetName);
                    bitmap.Dispose();
                };
            }

            return bitmap;
        }

        private List<(ISceneObject sceneObject, double x, double y)> InterfaceObjects = new List<(ISceneObject sceneObject, double x, double y)>();

        private void DrawSceneObject(ISceneObject sceneObject, double xParent = 0, double yParent = 0, bool batching = false, bool force = false, bool lightIgnoring = false)
        {
            if (!batching && sceneObject.Interface && !lightIgnoring && !sceneObject.AbsolutePosition)
            {
                InterfaceObjects.Add((sceneObject, xParent, yParent));
                return;
            }

            if (!sceneObject.Visible)
                return;

            if (force && sceneObject.ForceInvisible)
                return;

            var localSpriteBatchRestore = SpriteBatchRestore;
            //if (sceneObject.Scale != 0)
            //{
            //    spriteBatch.End();
            //    SetSpriteBatch(currentAbsolute, currentInterface, sceneObject.Scale);
            //}

            sceneObject.Drawed = true;

            var y = sceneObject.ComputedPosition.Y * cell;// + yParent;
            var x = sceneObject.ComputedPosition.X * cell;// + xParent;

            DrawLight(sceneObject, x, y);
            DrawEffects(sceneObject, x, y);

            if (sceneObject.IsBatch && !batching)
            {
                int width = (int)Math.Round(sceneObject.BoundPosition.Width * cell);
                int height = (int)Math.Round(sceneObject.BoundPosition.Height * cell);

                if (sceneObject.Expired || !BatchCache.TryGetValue(sceneObject.Uid, out var bitmap))
                {
                    bitmap = new RenderTarget2D(GraphicsDevice, width, height, false, GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);

                    spriteBatch.End();

                    GraphicsDevice.SetRenderTargets(bitmap);
                    GraphicsDevice.Clear(Color.Transparent);
                    SpriteBatchRestore.Invoke(false, sceneObject.Filtered);

                    DrawSceneObject(sceneObject, 0, 0, true);

                    TileSetCache[sceneObject.Uid] = new Rect(0, 0, width, height);
                    PosCache[sceneObject.Uid] = new Rect(x, y, width, height);

                    BatchCache[sceneObject.Uid] = bitmap;


                    spriteBatch.End();
                    GraphicsDevice.SetRenderTarget(target);

                    SpriteBatchRestore.Invoke(false, sceneObject.Filtered);
                }

                TileSetCache.TryGetValue(sceneObject.Uid, out var tilesetPos);
                PosCache.TryGetValue(sceneObject.Uid, out var sceneObjPos);

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

#warning potencial performance harm!
                if (sceneObject.TileMap != default)
                {
                    sceneObject.TileMap.Tiles.Where(t => Camera.InCamera(t))
                        .ForEach(t => DrawSceneTile(t, sceneObject, y, x, force));
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
                        DrawSceneObject(child, x, y, batching, force);
                    }
                }
            }

            SpriteBatchRestore = localSpriteBatchRestore;
        }

        private void DrawSceneTile(ITile tile, ISceneObject sceneObject, double y, double x, bool force)
        {
            var image = TileSetByName(tile.Source);
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
                spriteBatch.End();
                SpriteBatchRestore?.Invoke(true, sceneObject.Filtered);

                if (sceneObject.Scale > 0)
                {
                    spriteBatch.Draw(image, new Vector2(dest.X, dest.Y), source, drawColor, 0, origin, (float)sceneObject.Scale, spriteEffects, 0f);
                }
                else
                {
                    spriteBatch.Draw(image, dest, source, drawColor, angle, origin, spriteEffects, 0f);
                }

                spriteBatch.End();
                SpriteBatchRestore?.Invoke(false, sceneObject.Filtered);
            }
            else
            {
                if (sceneObject.Scale > 0)
                {
                    if (sceneObject.ScaleAndResize)
                    {
                        //spriteBatch.Draw(image, destinationRectangle: dest, sourceRectangle: source, origin: origin, rotation: angle, scale: new Vector2((float)sceneObject.Scale), color: color, effects: spriteEffects, layerDepth: 0);
                        spriteBatch.Draw(image, new Vector2((float)tile.Left, (float)tile.Top), source, color, angle, origin, new Vector2((float)sceneObject.Scale), spriteEffects, 0);
                    }
                    else
                    {
                        spriteBatch.Draw(image, new Vector2(dest.X, dest.Y), source, drawColor, angle, origin, (float)sceneObject.Scale, spriteEffects, 0f);
                    }
                }
                else if (sceneObject.AlphaBlend)
                {
                    spriteBatch.End();
                    SpriteBatchRestore?.Invoke(true, sceneObject.Filtered,true);

                    spriteBatch.Draw(image, dest, source, drawColor, angle, origin, spriteEffects, 0f);

                    spriteBatch.End();
                    SpriteBatchRestore?.Invoke(false, sceneObject.Filtered);
                }
                else
                {
                    spriteBatch.Draw(image, dest, source, drawColor, angle, origin, spriteEffects, 0f);
                }
            }
        }

        private void DrawSceneImage(ISceneObject sceneObject, double y, double x, bool force)
        {
            var image = TileSetByName(sceneObject.Image);
            if (image == default)
            {
                DungeonGlobal.Logger.Log("Медленный рендер из-за отсутствия картинки!");
                //Debugger.Break();
                return;
            }

            Rect tileRegion = default;

            if (sceneObject.DrawPartInSight)
            {
                image = sceneObject.GetPropertyExpr<Texture2D>("CroppedImage");

                var r = DungeonGlobal.Resolution;
                tileRegion = new Rect(0, 0, r.Width, r.Height);
            }
            else
            if (force || !TileSetCache.TryGetValue(sceneObject.Uid, out tileRegion))
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
            

            if (force || !PosCache.TryGetValue(sceneObject.Uid, out Rect pos) || sceneObject.Expired)
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
                }
                else
                {
                    width *= cell;
                    height *= cell;
                }

                pos = new Rect(x, y, width, height);

                if (!force && sceneObject.CacheAvailable)
                {
                    PosCache.Add(sceneObject.Uid, pos);
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

            Rectangle source = new Rectangle(tileRegion.Xi, tileRegion.Yi, tileRegion.Widthi, tileRegion.Heighti);

            var color = Color.White;

            var alpha = sceneObject.Opacity == 0
                   ? color.A
                   : sceneObject.Opacity;

            var drawColor = new Color(color.R, color.G, color.B, (float)alpha);

            if (sceneObject.Blur)
            {
                spriteBatch.End();
                SpriteBatchRestore?.Invoke(true, sceneObject.Filtered);

                if (sceneObject.Scale > 0)
                {
                    spriteBatch.Draw(image, new Vector2(dest.X, dest.Y), source, drawColor, angle, origin, (float)sceneObject.Scale, spriteEffects, 0f);
                }
                else
                {
                    spriteBatch.Draw(image, dest, source, drawColor, angle, origin, spriteEffects, 0f);
                }

                spriteBatch.End();
                SpriteBatchRestore?.Invoke(false, sceneObject.Filtered);
            }
            else
            {
                if (sceneObject.Scale > 0)
                {
                    if (sceneObject.ScaleAndResize)
                    {
                        //spriteBatch.Draw(image, destinationRectangle: dest, sourceRectangle: source, origin: origin, rotation: angle, scale: new Vector2((float)sceneObject.Scale), color: color, effects: spriteEffects, layerDepth: 0);
                        spriteBatch.Draw(image, new Vector2((float)pos.Xi, (float)pos.Yi), source, color, angle, origin, new Vector2((float)sceneObject.Scale), spriteEffects, 0);
                    }
                    else
                    {
                        spriteBatch.Draw(image, new Vector2(dest.X, dest.Y), source, drawColor, angle, origin, (float)sceneObject.Scale, spriteEffects, 0f);
                    }
                }
                else
                {
                    spriteBatch.Draw(image, dest, source, drawColor, angle, origin, spriteEffects, 0f);
                }
            }
        }

        readonly Dictionary<string, Dictionary<float, Texture2D>> MaskCache = new Dictionary<string, Dictionary<float, Texture2D>>();

        private (Texture2D image, SpriteEffects effects) ApplyImageMask(Texture2D image, ISceneObject sceneObject)
        {
            var mask = sceneObject.ImageMask;
            var progress = (float)Math.Round(mask.AmountPercentage * 0.01f, 2);
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

        private void DrawSceneText(float fontSize, double y, double x, IDrawText range, ISceneObject sceneObject)
        {
            bool fontWeight = range.Bold;

            SpriteFont spriteFont;

            if (string.IsNullOrEmpty(range.FontName))
            {
                if (range.CompiledFontName == default)
                {
                    range.CompiledFontName = $"Dungeon12.Resources.Fonts.xnb/{DungeonGlobal.DefaultFontName}/{DungeonGlobal.DefaultFontName}{DungeonGlobal.DefaultFontSize}.xnb".Embedded();
                }
                var font = range.CompiledFontName;

                var resFont = ResourceLoader.Load(font);
                if (resFont == default)
                    return;

                spriteFont = Content.Load<SpriteFont>(font, resFont.Stream);
            }
            else
            {
                if (range.CompiledFontName == default)
                {
                    range.CompiledFontName = $"Dungeon12.Resources.Fonts.xnb/{range.FontName}/{range.FontName}{range.Size}.xnb".Embedded();
                }
                var font = range.CompiledFontName;

                var resFont = ResourceLoader.Load(font);
                if (resFont == default)
                    return;

                if (string.IsNullOrEmpty(range.FontPath))
                {
                    spriteFont = Content.Load<SpriteFont>(font, resFont.Stream);
                }
                else
                {
                    spriteFont = Content.Load<SpriteFont>(font, resFont.Stream);
                }
            }

            var txt = range.StringData;

            var componentWidth = sceneObject.BoundPosition.Width;
            if (range.WordWrap && componentWidth > 0)
            {
                txt = WrapText(spriteFont, txt, componentWidth * cell);
            }

            var color = new Color(range.ForegroundColor.R, range.ForegroundColor.G, range.ForegroundColor.B, range.ForegroundColor.A);

            //spriteBatch.End();
            //SpriteBatchRestore?.Invoke(true, sceneObject.Filtered);

            if (sceneObject.Scale > 0)
            {
                spriteBatch.DrawString(spriteFont, txt, new Vector2((int)x, (int)y), color, 0, Vector2.Zero, (float)sceneObject.Scale, SpriteEffects.None, 1);
            }
            else
            {
                spriteBatch.DrawString(spriteFont, txt, new Vector2((int)x, (int)y), color);
            }

            //spriteBatch.End();
            //SpriteBatchRestore?.Invoke(false, sceneObject.Filtered);
        }

        private static string WrapText(SpriteFont font, string text, double maxLineWidth, int counter = 0, string original = default)
        {
            if (original == default)
            {
                original = text;
            }
            string[] words = text.Split(' ');
            StringBuilder sb = new StringBuilder();
            float lineWidth = 0f;
            float spaceWidth = font.MeasureString(" ").X;

            if (counter > 20)
                return text;

            if (maxLineWidth < spaceWidth)
                return text; //попытка избежать stackoverflowexception

            foreach (string word in words)
            {
                Vector2 size = font.MeasureString(word);

                if (lineWidth + size.X < maxLineWidth)
                {
                    sb.Append(word + " ");
                    lineWidth += size.X + spaceWidth;
                }
                else
                {
                    if (size.X > maxLineWidth)
                    {
                        if (sb.ToString() == "")
                        {
                            sb.Append(WrapText(font, word.Insert(word.Length / 2, " ") + " ", maxLineWidth, ++counter, original));
                        }
                        else
                        {
                            sb.Append(Environment.NewLine + WrapText(font, word.Insert(word.Length / 2, " ") + " ", maxLineWidth, ++counter, original));
                        }
                    }
                    else
                    {
                        sb.Append(Environment.NewLine + word + " ");
                        lineWidth = size.X + spaceWidth;
                    }
                }
            }

            return sb.ToString();
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
                    var depth = (int)Math.Round(drawablePath.Depth);
                    if(depth==0)
                    {
                        depth = 1;
                    }
                    DrawBorder(rect, depth, drawColor);
                }
            }

            if (drawablePath.PathPredefined == PathPredefined.Line)
            {
                Texture2D texture = default;

                if (drawablePath.Texture != string.Empty)
                {
                    texture = TileSetByName(drawablePath.Texture);
                }

                var from = new Dungeon.Types.Point(drawablePath.Path.First());
                from.X *= cell;
                from.Y *= cell;
                from.X += x;
                from.Y += y;

                var to = new Dungeon.Types.Point(drawablePath.Path.Last());
                to.X *= cell;
                to.Y *= cell;
                to.X += x;
                to.Y += y;

                var fromVector = new Vector2(from.Xf, from.Yf);
                var toVector = new Vector2(to.Xf, to.Yf);

                var color = drawablePath.BackgroundColor;

                var alpha = color.Opacity == 0
                    ? color.A
                    : color.Opacity;

                var drawColor = new Color(color.R, color.G, color.B, (float)alpha);

                DrawLineTo(spriteBatch, texture, fromVector, toVector, drawColor, (int)drawablePath.Depth);
            }
        }

        public void DrawLineColor(SpriteBatch spriteBatch, Vector2 begin, Vector2 end, Color color, int width = 1)
        {
            width = 1;
            Rectangle r = new Rectangle((int)begin.X, (int)begin.Y, (int)(end - begin).Length() + width, width);
            Vector2 v = Vector2.Normalize(begin - end);
            float angle = (float)Math.Acos(Vector2.Dot(v, -Vector2.UnitX));
            if (begin.Y > end.Y) angle = MathHelper.TwoPi - angle;
            spriteBatch.Draw(PixelColorTexture(), r, default, color, angle, Vector2.Zero, SpriteEffects.None, 0);
        }

        public void DrawLineTo(SpriteBatch sb, Texture2D texture, Vector2 src, Vector2 dst, Color color, int depth)
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

        private static readonly Dictionary<string, Light> Lights = new Dictionary<string, Light>();

        private void DrawLight(ISceneObject sceneObject, double x, double y)
        {
#if Core
            if (sceneObject.Light == null)
                return;

            var objLight = sceneObject.Light;

            var xf = x + Camera.CameraOffsetX;
            var yf = y + Camera.CameraOffsetY;

            xf += sceneObject.BoundPosition.Width / 2 * cell;
            yf += sceneObject.BoundPosition.Height / 2 * cell;

            var pos = new Vector2((float)xf, (float)yf);

            var objLightColor = objLight.Color;
            var color = new Color(objLightColor.R, objLightColor.G, objLightColor.B, objLightColor.A);

            if (penumbra != default)
            {
                if (objLight.Updated && Lights.ContainsKey(sceneObject.Uid))
                {
                    var oldLight = Lights[sceneObject.Uid];
                    penumbra.Lights.Remove(oldLight);
                    Lights.Remove(sceneObject.Uid);
                }

                if (!Lights.TryGetValue(sceneObject.Uid, out var light))
                {
                    switch (objLight.Type)
                    {
                        case LightType.Point:
                            light = new PointLight()
                            {
                                Scale = new Vector2(objLight.Range * cell),
                                ShadowType = ShadowType.Illuminated,
                                Radius = sceneObject.Light.Range * cell,
                                Position = pos,
                                Color = color
                            };
                            break;
                        case LightType.Spot:

                            light = new Spotlight()
                            {
                                Scale = new Vector2(objLight.Range * cell),
                                ShadowType = ShadowType.Illuminated,
                                Radius = sceneObject.Light.Range * cell,
                                Position = pos,
                                Color = color
                            };
                            break;
                        case LightType.Texture:

                            light = new TexturedLight(TileSetByName(objLight.Image))
                            {
                                Scale = new Vector2(objLight.Range * cell),
                                ShadowType = ShadowType.Illuminated,
                                Radius = sceneObject.Light.Range * cell,
                                Position = pos,
                                Color = color
                            };
                            break;
                        default:
                            break;
                    }

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
#endif
        }

        private static readonly Dictionary<string, ParticleEffect> ParticleEffects = new Dictionary<string, ParticleEffect>();

        private void DrawEffects(ISceneObject sceneObject, double x, double y)
        {
            if (sceneObject.Effects==default || sceneObject.Effects.Count == 0)
                return;

            x = (int)x;
            y = (int)y;

            foreach (var effect in sceneObject.Effects)
            {
                var path = $"{effect.Assembly}.Resources.Particles.{effect.Name}.xml";

                void DrawEffect()
                {
                    if (myRenderer == default)
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
                        * Matrix.CreateTranslation((float)Camera.CameraOffsetX, (float)Camera.CameraOffsetY, 0);

                    myRenderer.RenderEffect(particleEffect, ref v);

                    SpriteBatchRestore.Invoke(false, sceneObject.Filtered);
                }

                Once.Call(DrawEffect, "XNACLIENT" + nameof(Draw) + nameof(DrawEffect) + path);
            }
        }

        public void CacheObject(ISceneObject sceneObject)
        {
            var image = TileSetByName(sceneObject.Image);

            if (ResourceLoader.CacheImagesAndMasks)
                if (sceneObject.ImageMask != default && sceneObject.ImageMask.CacheAvailable)
                {
                    CacheImageMask(image, sceneObject);
                }
        }

        public void CacheImage(string image)
        {
            TileSetByName(image);
        }

        private void CacheImageMask(Texture2D image, ISceneObject sceneObject)
        {
            //var uid = sceneObject.Image;
            //var mask = sceneObject.ImageMask;

            //if (!MaskCache.ContainsKey(uid))
            //{
            //    MaskCache.Add(uid, new Dictionary<float, Texture2D>());
            //}
            //else
            //{
            //    return;
            //}

            //var cache = MaskCache[uid];
            //for (float i = 0f; i < 1; i += 0.01f)
            //{
            //    var v = (float)Math.Round(i,2);
            //    if (!cache.ContainsKey(v))
            //    {
            //        cache.Add(v, MakeMask(image, v, mask.Color.Convert(), mask.Opacity));
            //    }

            //}
        }

        public void Clear(IDrawColor drawColor = null)
        {
            var xnacolor = Color.Transparent;
            if (drawColor != default)
            {
                xnacolor = new Color(drawColor.R, drawColor.G, drawColor.B, drawColor.A);
            }
            GraphicsDevice.Clear(xnacolor);
        }
    }
}
