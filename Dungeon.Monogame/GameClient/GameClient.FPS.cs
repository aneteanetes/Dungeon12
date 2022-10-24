using Dungeon.View.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Dungeon.Monogame
{
    public partial class GameClient : Game, IGameClient
    {
        private int _frame;
        private TimeSpan _lastFps;
        private int _lastFpsFrame;
        private double _fps;
        readonly Stopwatch _st = Stopwatch.StartNew();

        private void DrawDebugInfo()
        {
            try
            {
                bool neeedClose = false;
                if (!DefaultSpriteBatch.IsOpened)
                {
                    neeedClose = true;
                    DefaultSpriteBatch.Begin();
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
                //var text = $"Версия: {DungeonGlobal.Version}";

                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(DefaultFontXnbExistedFile))
                {
                    if (stream.CanSeek)
                    {
                        stream.Seek(0, SeekOrigin.Begin);
                    }

                    var font = Content.Load<SpriteFont>(DefaultFontXnbExistedFile, stream);

                    //spriteBatch.DrawString(font, text, new Vector2(1050, 16), Color.White);

                    DefaultSpriteBatch.DrawString(font, DungeonGlobal.FPS.ToString("F0"), new Vector2((this.Window.ClientBounds.Width - 50) - 2, 2), Color.Yellow);
                }

                if (neeedClose)
                {
                    DefaultSpriteBatch.End();
                }

                _frame++;
            }
            catch { DefaultSpriteBatch.End(); }
        }

        private const string DefaultFontXnbExistedFile = "Dungeon.Monogame.Resources.Fonts.xnb.Montserrat.Montserrat10.xnb";
    }
}