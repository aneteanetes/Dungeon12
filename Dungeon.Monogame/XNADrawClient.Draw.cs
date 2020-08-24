namespace Dungeon.Monogame
{
    using Dungeon;
    using Dungeon.View.Interfaces;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;

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

        protected override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            drawCicled = true;
            GraphicsDevice.Clear(Color.CornflowerBlue);

            CalculateCamera();

            if (this.scene != default)
            {
                XNADrawClientImplementation.Draw(this.scene.Objects, gameTime);
            }

            DrawDebugInfo();

            OnPointerMoved();

            try
            {
                spriteBatch.Begin();
            }
            catch { }
            finally
            {
                spriteBatch.End();
            }

            if (!clientSettings.Add2DLighting)
                base.Draw(gameTime);
        }

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
                if (!spriteBatch.Opened)
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

                    spriteBatch.DrawString(font, DungeonGlobal.FPS.ToString(), new Vector2(this.Window.ClientBounds.Width - m, 15), Color.Yellow);
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
}