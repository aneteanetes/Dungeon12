﻿using Dungeon.Types;
using Dungeon.View.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dungeon.Monogame.Effects.Fogofwar
{
    public class FogOfWarEffect : IEffect, IMonogameEffect
    {
        public FogOfWarEffect()
        {
            Assembly = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
        }

        public string Name => "FogOfWar";

        public double Scale { get; set; }

        public string Assembly { get; set; }

        public EffectTime When => EffectTime.PostProcess;

        public Dot Position { get; set; } = new Dot();

        public string Image { get; set; }

        public bool Loaded { get; set; }

        public Dot Size { get; set; }

#warning need FogOfWarEffect refactoring
        public Texture2D Draw(RenderTarget2D input)
        {
            spriteBatch.DepthStencilState = null;
            spriteBatch.spriteSortMode = null;

            // здесь мы чистим fogMaskCursor, отчего у нас не сохраняются пустые места
            GraphicsDevice.SetRenderTargets(fogMaskCursor);
            GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin();
            spriteBatch.Draw(areaTexture, new Vector2(Position.Xf, Position.Yf), Color.White);
            spriteBatch.End();

            // что бы не было бага purple pixel проворачиваем эту тупую хуйню:
            // не чистим уже этот таргет, из-за чего получаем эффект постепенно открывающегося тумана
            GraphicsDevice.SetRenderTargets(fogMask);
            spriteBatch.Begin();
            spriteBatch.Draw(fogMaskCursor, Vector2.Zero, Color.White);
            spriteBatch.End();

            // теперь наша готовая текстура лежит в fogMask
            // таргетом ставим буфер - на который рисуем вначале маску (которая будет инвертированна)
            GraphicsDevice.SetRenderTargets(backBuffer);
            GraphicsDevice.Clear(Color.Transparent);

            //рисуем альфа слой
            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, state1, null, alphaTestEffect);
            spriteBatch.Draw(fogMask, Vector2.Zero, Color.White); //The mask 
            spriteBatch.End();

            //рисуем чё там у нас высралось
            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, state2, null);
            spriteBatch.Draw(input, Vector2.Zero, Color.White); //The mask 
            spriteBatch.End();

            //возвращаем залупу которая получилась
            return backBuffer;
        }

        SpriteBatchKnowed spriteBatch => client.DefaultSpriteBatch;

        GraphicsDeviceManager graphics => client.graphics;

        GraphicsDevice GraphicsDevice => client.GraphicsDevice;

        public bool UseGlobalImageFilter => true;

        public bool NotDrawOriginal => true;

        GameClient client;

        RenderTarget2D fogMaskCursor;

        RenderTarget2D fogMask;

        RenderTarget2D backBuffer;

        Texture2D areaTexture;
        Texture2D background;

        Matrix projectionMatrix;

        AlphaTestEffect alphaTestEffect;

        DepthStencilState state1;

        DepthStencilState state2;

        public void Load(GameClient client)
        {
            this.client = client;
            var pp = GraphicsDevice.PresentationParameters;

            fogMaskCursor = new RenderTarget2D(GraphicsDevice,
                this.Size.Xi,
                this.Size.Yi,
                false,
                SurfaceFormat.Color,
                DepthFormat.None);

            fogMask = new RenderTarget2D(GraphicsDevice,
                this.Size.Xi,
                this.Size.Yi,
                false,
                SurfaceFormat.Color,
                DepthFormat.Depth24Stencil8,
                0, 
                RenderTargetUsage.PreserveContents);

            backBuffer = new RenderTarget2D(GraphicsDevice,
                this.Size.Xi,
                this.Size.Yi,
                false,
                pp.BackBufferFormat,
                pp.DepthStencilFormat,
                pp.MultiSampleCount,
                pp.RenderTargetUsage);

#warning fog of war loading not from scene resources!
            areaTexture = client.ImageLoader.LoadTexture2D(DungeonGlobal.Resources, Image);

            projectionMatrix = Matrix.CreateOrthographicOffCenter(0,
             graphics.GraphicsDevice.PresentationParameters.BackBufferWidth,
             graphics.GraphicsDevice.PresentationParameters.BackBufferHeight,
             0, 0, 1
            );

            alphaTestEffect = new AlphaTestEffect(graphics.GraphicsDevice) { Projection = projectionMatrix };

            state1 = new DepthStencilState
            {
                StencilEnable = true,
                StencilFunction = CompareFunction.Always,
                StencilPass = StencilOperation.Replace,
                ReferenceStencil = 1,
                DepthBufferEnable = false,
            };

            state2 = new DepthStencilState
            {
                StencilEnable = true,
                StencilFunction = CompareFunction.LessEqual,
                StencilPass = StencilOperation.Keep,
                ReferenceStencil = 1,
                DepthBufferEnable = false,
            };
        }
    }
}
