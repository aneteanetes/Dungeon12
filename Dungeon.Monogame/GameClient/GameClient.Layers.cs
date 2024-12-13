using Dungeon.View.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Dungeon.Monogame
{
    public partial class GameClient : Game, IGameClient
    {
        private Dictionary<ISceneLayer, RenderTarget2D> SceneLayers = new Dictionary<ISceneLayer, RenderTarget2D>();

        private void SelectSceneObjects(ISceneLayer layer)
        {
            layer.ActiveObjects.Clear();
            layer.ActiveObjectControls.Clear();

            var objs = layer.Objects;
            for (int i = 0; i < objs.Count; i++)
            {
                var obj = objs[i];
                if (obj.Visible)
                    SelectSceneObject(layer, obj);
            }
        }

        private void SelectSceneObject(ISceneLayer layer, ISceneObject sceneObject,double xParent = 0, double yParent = 0, bool batching = false, double parentScale = 0)
        {
            bool needScalePosition = false;

            var scale_ = sceneObject.GetScaleValue();
            if (parentScale != 0)
            {
                scale_ = parentScale;
                needScalePosition = true;
            }

            if (scale_ == 0)
                scale_ = 1;

            var x = 0d;
            var y = 0d;

            if (!batching)
            {
                if (scale_ == 1 || !needScalePosition)
                {
                    x = xParent + (float)sceneObject.Left;
                    y = yParent + (float)sceneObject.Top;
                }
                else
                {
                    x = xParent + (float)sceneObject.Left * (needScalePosition ? scale_ : 1);
                    y = yParent + (float)sceneObject.Top * (needScalePosition ? scale_ : 1);
                }
            }

            int width;
            int height;
            if (scale_ == 1)
            {
                width = (int)sceneObject.Width;
                height = (int)sceneObject.Height;
            }
            else
            {
                width = (int)Math.Round((sceneObject.Width * scale_));
                height = (int)Math.Round((sceneObject.Height * scale_));
            }

            sceneObject.DrawClientWidth=width; 
            sceneObject.DrawClientHeight=height;
            sceneObject.DrawClientX = x;
            sceneObject.DrawClientY = y;

            if (sceneObject.DrawOutOfSight || sceneObject.DrawPartInSight || this.InCamera(width, height, x, y))
            {
                layer.ActiveObjects.Add(sceneObject);
                if (sceneObject is ISceneControl sceneControl)
                    layer.ActiveObjectControls.Add(sceneControl);

                for (int i = 0; i < sceneObject.Children.Count; i++)
                {
                    SelectSceneObject(layer, sceneObject.Children[i], x, y, sceneObject.IsBatch || batching, sceneObject.Scale);
                }

                return;
            }
        }

        private void UpdateLayers(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (this.Scene != default)
            {
                for (int i = 0; i < this.Scene.Layers.Length; i++)
                {
                    var layer = this.Scene.Layers[i];
                    if(SceneLayers.TryGetValue(layer, out var target))
                    {
                        if (layer.Destroyed)
                        {
                            SceneLayers.Remove(layer);
                        }
                    }
                    else
                    {
                        var pp = GraphicsDevice.PresentationParameters;
                        target = new RenderTarget2D(GraphicsDevice, (int)layer.Width, (int)layer.Height, false,
                            pp.BackBufferFormat, pp.DepthStencilFormat, pp.MultiSampleCount, pp.RenderTargetUsage);
                        SceneLayers.Add(layer, target);
                    }

                    SelectSceneObjects(layer);
                }
            }
        }
    }
}