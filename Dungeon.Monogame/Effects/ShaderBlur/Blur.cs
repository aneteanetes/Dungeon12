using Dungeon.Types;
using Dungeon.View.Interfaces;
using Microsoft.Xna.Framework.Graphics;

namespace Dungeon.Monogame.Effects.ShaderBlur
{
    public class Blur : IMonogameEffect, IEffect
    {
        public string Name => "MonogameShaderBlur";

        public double Scale => 1;

        private GaussianBlur gaussianBlur;
        private GameClient _gameClient;
        public Blur()
        {
            _gameClient=GameClient.Instance;
            Assembly = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            gaussianBlur=new GaussianBlur();
            gaussianBlur.ComputeKernel(7, 2f);
        }

        public string Assembly { get; private set; }

        public EffectTime When => EffectTime.PostProcess;

        public Dot Position { get; set; }

        public string Image { get; set; }

        public Dot Size { get; set; }

        public bool Loaded { get; set; }

        public bool UseGlobalImageFilter => false;

        public bool NotDrawOriginal => false;

        public Texture2D Draw(RenderTarget2D input)
        {
            var w = input.Width / 2;
            var h = input.Height/2;

            var r1 = new RenderTarget2D(_gameClient.GraphicsDevice,
                w, h, false,
                _gameClient.GraphicsDevice.PresentationParameters.BackBufferFormat,
                DepthFormat.None);

            var r2 = new RenderTarget2D(_gameClient.GraphicsDevice,
                 w, h, false,
               _gameClient.GraphicsDevice.PresentationParameters.BackBufferFormat,
                DepthFormat.None);

            gaussianBlur.ComputeOffsets(w, h);

            _gameClient.DrawClient.SpriteBatchManager.Begin(_gameClient.ResolutionMatrix);

            var result =  gaussianBlur.PerformGaussianBlur(input, r1, r2, _gameClient.DrawClient.SpriteBatchManager.GetSpriteBatch());

            _gameClient.DrawClient.SpriteBatchManager.End();

            return result;
        }

        public void Load(GameClient client)
        {
            //throw new NotImplementedException();
        }
    }
}
