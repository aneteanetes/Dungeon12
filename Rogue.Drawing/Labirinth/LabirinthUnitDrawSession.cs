﻿using Rogue.Drawing.Impl;
using Rogue.Map;
using Rogue.Types;
using Rogue.View.Interfaces;

namespace Rogue.Drawing.Labirinth
{
    /// <summary>
    /// draw object in labirinth bounds, by setting rectangle and drawable object
    /// </summary>
    public class LabirinthUnitDrawSession : DrawSession
    {
        public GameMap Location { get; set; }

        public Point Position { get; set; }

        public override IDrawSession Run()
        {
            this.Drawables = Location.MapOld[(int)Position.Y][(int)Position.X];

            return base.Run();
        }
    }
}
