namespace Dungeon.View.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface ILight
    {
        IDrawColor Color { get; set; }

        float Range { get; set; }

        bool Updated { get; set; }

        LightType Type { get; set; }

        string Image { get; set; }
    }
}
