using Dungeon.Engine.Projects;
using Dungeon.Monogame;
using Dungeon.Settings;
using Dungeon.Types;
using Dungeon.View.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ProjectMercury.Renderers;
using System;
using System.Diagnostics;

namespace Dungeon.Engine.Host
{
    public partial class D3D11Host
    {
        SpriteBatchKnowed spriteBatch;

        public void InitImpl()
        {
            var _services = new GameServiceContainer();
            var graphicsService = new DefaultGraphicsDeviceManager(GraphicsDevice);
            _services.AddService(typeof(IGraphicsDeviceService), graphicsService);
            var _content = new ContentManager(_services);
            spriteBatch = new SpriteBatchKnowed(GraphicsDevice);
            DungeonGlobal.TransportVariable = GraphicsDevice;

            var cellSize = App.Container.Resolve<EngineProject>()?.CompileSettings.CellSize ?? 32;
        }

        public void ChangeCell(int newCellSize)
        {
            DrawingSize.Cell = newCellSize;
        }


        #region frameSettings

        private bool frameEnd;
        private int _frame;
        private TimeSpan _lastFps;
        private int _lastFpsFrame;
        private double _fps;
        Stopwatch _st = Stopwatch.StartNew();

        public double FPS => _fps;

        #endregion

        private void DrawDebugInfo()
        {
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

            _frame++;
        }

        private Callback callback;
        private bool drawed = false;

        private bool skipCallback = false;


        public void Drag(ISceneObject @object, ISceneObject area = null)
        {
            
        }

        public void SetCursor(string texture)
        {
            
        }
    }
}