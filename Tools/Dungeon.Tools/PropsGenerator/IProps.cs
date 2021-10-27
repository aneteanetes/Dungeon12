using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Dungeon.PropsGenerator
{
    public interface IProps
    {
        string Name { get; }

        string Content();
    }
}
