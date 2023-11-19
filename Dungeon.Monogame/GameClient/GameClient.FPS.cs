using Dungeon.Varying;
using Dungeon.View.Interfaces;
using FontStashSharp;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
        private DynamicSpriteFont fontFPS;

        private void DrawDebugInfo()
        {
            //try
            //{
            //bool neeedClose = false;
            //if (!LayerSpriteBatch.IsOpened)
            //{
            //    neeedClose = true;
            //    LayerSpriteBatch.Begin();
            //}
            //var nowTs = _st.Elapsed;
            //var now = DateTime.Now;
            //var fpsTimeDiff = (nowTs - _lastFps).TotalSeconds;
            //if (fpsTimeDiff > 1)
            //{
            //    _fps = (_frame - _lastFpsFrame) / fpsTimeDiff;
            //    DungeonGlobal.FPS = _fps;
            //    _lastFpsFrame = _frame;
            //    _lastFps = nowTs;
            //}
            ////var text = $"Версия: {DungeonGlobal.Version}";

            if (fontFPS == null)
            {
                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(DefaultFontXnbExistedFile))
                {
                    if (stream.CanSeek)
                    {
                        stream.Seek(0, SeekOrigin.Begin);
                    }


                    var fontSys = DrawClient.GetTrueTypeFontSystemByNameAndStream("Montserrat Bold", stream);
                    fontFPS = fontSys.GetFont(20);
                }
            }

            var msg = " Fps: " + (FPS.frames / FPS.elapsed).ToString() 
                + "\n Elapsed time: " 
                + FPS.elapsed.ToString() 
                + "\n Updates: " 
                + FPS.updates.ToString() 
                + "\n Frames: " + FPS.frames.ToString()
                + "\n Objects: " + SceneLayers.Sum(x=>x.Key.ActiveObjects.Count)
                + "\n Controls: " + SceneLayers.Sum(x => x.Key.ActiveObjectControls.Count);

            fontFPS.DrawText(LayerSpriteBatch, msg, new Vector2(Variables.Get<int>("FPSLEFT"), 25), Color.Yellow);
            FPS.frames++;

            //if (neeedClose)
            //{
            //    LayerSpriteBatch.End();
            //}

            //_frame++;
            //}
            //catch { DefaultSpriteBatch.End(); }
        }

        private const string DefaultFontXnbExistedFile = "Dungeon.Monogame.Resources.Fonts.ttf.Montserrat-Bold.ttf";
    }
}