using Dungeon.Resources;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon.Monogame.Effects
{
    public class Grayscale : IMonogameEffect
    {
        public bool Loaded { get; set; }

        public bool UseGlobalImageFilter => false;

        public bool NotDrawOriginal => false;

        public Texture2D Draw(RenderTarget2D input)
        {
            throw new NotImplementedException();
        }

        public Effect Shader { get; set; }

        public void Load(XNADrawClient client)
        {            
            var res = ResourceLoader.Load("Dungeon.Monogame.Resources.Shaders.Greyscale.xnb",caching:true);

            var embcontres = new EmbeddedContentResolver();

            Shader = client.Content.Load<Effect>("Greyscale",res.Stream, contentResolver: embcontres);
            Shader.CurrentTechnique = Shader.Techniques["BasicColorDrawing"];

            Loaded = true;
        }
    }
}