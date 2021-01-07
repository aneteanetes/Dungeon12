using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Dungeon.Drawing;
using Dungeon.Engine.Projects;
using Dungeon.Monogame;
using Dungeon.SceneObjects;
using Dungeon.Scenes;
using Dungeon.Settings;
using Dungeon.Types;
using Dungeon.View.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ProjectMercury.Renderers;
using Color = Microsoft.Xna.Framework.Color;
using Matrix = Microsoft.Xna.Framework.Matrix;

namespace Dungeon.Engine.Host
{
    public partial class D3D11Host : IDrawClient
    {
        private IScene _scene { get; set; }
        public IScene scene
        {
            get => _scene;
            set
            {
                SceneLayers = new Dictionary<ISceneLayer, RenderTarget2D>();
                if(value.Is<Scenes.Sys_Clear_Screen>())
                {
                    GraphicsDevice.Clear(Color.Black);
                }
                _scene = value;
            }
        }

        public DungeonEngineCamera Camera { get; set; } = new DungeonEngineCamera();

        public double CameraOffsetX => Camera.CameraOffsetX;

        public double CameraOffsetY => Camera.CameraOffsetY;

        public double CameraOffsetZ => Camera.CameraOffsetZ;

        public double CameraOffsetLimitX => Camera.CameraOffsetLimitX;

        public double CameraOffsetLimitY => Camera.CameraOffsetLimitY;

        public double CameraOffsetLimitZ => Camera.CameraOffsetLimitZ;

        SpriteBatchKnowed spriteBatch;
        XNADrawClientImplementation XNADrawClientImplementation;

        public void InitImpl()
        {
            var _services = new GameServiceContainer();
            var graphicsService = new DefaultGraphicsDeviceManager(GraphicsDevice);
            _services.AddService(typeof(IGraphicsDeviceService), graphicsService);
            var _content = new ContentManager(_services);
            spriteBatch = new SpriteBatchKnowed(GraphicsDevice);
            DungeonGlobal.TransportVariable = GraphicsDevice;

            var cellSize = App.Container.Resolve<EngineProject>()?.CompileSettings.CellSize ?? 32;

            XNADrawClientImplementation = new XNADrawClientImplementation(GraphicsDevice, default, spriteBatch, cellSize, default, _content, Camera,new SpriteBatchRenderer
            {
                GraphicsDeviceService = graphicsService
            });
        }

        public void ChangeCell(int newCellSize)
        {
            DrawingSize.Cell = newCellSize;
            XNADrawClientImplementation.cell = newCellSize;
        }

        public Microsoft.Xna.Framework.Color ClearColor { get; set; } = Color.CornflowerBlue;


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

        public void Draw(IEnumerable<IDrawSession> drawSessions)
        {
            throw new NotImplementedException();
        }

        private Callback callback;
        private bool drawed = false;

        private bool skipCallback = false;

        public string Uid { get; } = Guid.NewGuid().ToString();

        public Callback SetScene(IScene scene)
        {
            drawed = false;
            XNADrawClientImplementation.scene = scene;
            this.scene = scene;
            callback = new Callback(() =>
            {
                scene.Destroy();
            });

            return callback;
        }

        public Types.Point MeasureText(IDrawText drawText, ISceneObject parent = null)
            => XNADrawClientImplementation.MeasureText(drawText, parent);

        public Types.Point MeasureImage(string image)
            => XNADrawClientImplementation.MeasureImage(image);

        public void SaveObject(ISceneObject sceneObject, string path, Types.Point offset, string runtimeCacheName = null)
            => XNADrawClientImplementation.SaveObject(sceneObject,path,offset,runtimeCacheName);

        public void Animate(IAnimationSession animationSession)
        {
            throw new NotImplementedException();
        }

        public void Drag(ISceneObject @object, ISceneObject area = null)
        {
            
        }

        void IDrawClient.Drop()
        {
        }

        public void Clear(IDrawColor drawColor = null)
            => XNADrawClientImplementation.Clear(drawColor);

        public void SetCursor(string texture)
        {
            
        }

        public void CacheObject(ISceneObject @object)
            => XNADrawClientImplementation.CacheObject(@object);

        public void CacheImage(string image)
            => XNADrawClientImplementation.CacheImage(image);

        public void MoveCamera(Direction direction, bool stop = false, bool once = false)
            => Camera.MoveCamera(direction, stop);

        public void StopMoveCamera() 
            => Camera.StopMoveCamera();

        public void SetCamera(double x, double y)
            => Camera.SetCamera(x,y);

        public void ResetCamera()
            => Camera.ResetCamera();

        public void SetCameraSpeed(double speed)
            => Camera.SetCameraSpeed(speed);

        public bool InCamera(ISceneObject sceneObject)
            => Camera.InCamera(sceneObject);
    }
}