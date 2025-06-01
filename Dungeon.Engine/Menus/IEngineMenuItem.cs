using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon.Engine.Menus
{
    public interface IEngineMenuItem
    {
        public string Text { get; }

        public string Tag { get; }

        public int Weight { get; }

        public Action Click { get;  }
    }
}