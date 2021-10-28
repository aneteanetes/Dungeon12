using Dungeon.Types;
using Dungeon.View.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Dungeon.Monogame.Effects
{
    public class Light2D : IMonogameEffect, IEffect
    {
        public Light2D()
        {
            Assembly = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
        }

        public bool Loaded { get; set; }

        public string Name => "2DLight";

        public double Scale => 1;

        public string Assembly { get; private set; }

        public EffectTime When => EffectTime.PreProcess;

        public Point Position { get; set; }

        public string Image { get; set; }

        public Point Size { get; set; }

        public bool UseGlobalImageFilter => false;

        public bool NotDrawOriginal => false;

        public Texture2D Draw(RenderTarget2D input)
        {
            throw new NotImplementedException();
        }

#if !Engine
        public void Load(XNADrawClient client)
        {
            throw new NotImplementedException();
        }
#endif
    }
}