using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Dungeon.Monogame;
using Dungeon.Scenes;
using Dungeon.Types;
using Dungeon.View.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;
using Matrix = Microsoft.Xna.Framework.Matrix;

namespace Dungeon.Engine.Host
{
    public partial class D3D11Host : IDrawClient
    {
        public IScene scene { get; set; }

        public ICamera Camera { get; set; } = new DungeonEngineCamera();

        public double CameraOffsetX => Camera.CameraOffsetX;

        public double CameraOffsetY => Camera.CameraOffsetY;

        public double CameraOffsetLimitX => Camera.CameraOffsetLimitX;

        public double CameraOffsetLimitY => Camera.CameraOffsetLimitY;

        SpriteBatch spriteBatch;
        XNADrawClientImplementation XNADrawClientImplementation;

        public void InitImpl()
        {
            var _services = new GameServiceContainer();
            var _content = new ContentManager(_services);
            spriteBatch = new SpriteBatch(GraphicsDevice);
            XNADrawClientImplementation = new XNADrawClientImplementation(GraphicsDevice, default, spriteBatch, 32, default, _content, Camera, default);
        }

        public void Draw()
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.RasterizerState = RasterizerState.CullNone;

            if (scene != default)
            {
                XNADrawClientImplementation.Draw(scene.Objects, new Microsoft.Xna.Framework.GameTime());
            }
        }

        public void Draw(IEnumerable<IDrawSession> drawSessions)
        {
            throw new NotImplementedException();
        }

        private Callback сallback;

        private bool skipCallback = false;
        public Callback SetScene(IScene scene)
        {
            XNADrawClientImplementation.scene = scene;
            this.scene = scene;
            сallback = new Callback(() =>
            {
                scene.Destroy();
            });

            return сallback;
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

        public void MoveCamera(Direction direction, bool stop = false)
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