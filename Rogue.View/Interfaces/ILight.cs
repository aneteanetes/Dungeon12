using System;
using System.Collections.Generic;
using System.Text;

namespace Rogue.View.Interfaces
{
    public interface ILight
    {
        IDrawColor Color { get; set; }

        float Range { get; set; }
    }
}
