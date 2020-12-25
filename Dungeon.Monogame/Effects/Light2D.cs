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

        public Texture2D Draw(Texture2D input)
        {
            throw new NotImplementedException();
        }

        public void Load(XNADrawClient client)
        {
            throw new NotImplementedException();
        }
    }
}