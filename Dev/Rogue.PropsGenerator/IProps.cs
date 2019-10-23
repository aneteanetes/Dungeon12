using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Rogue.PropsGenerator
{
    public interface IProps
    {
        string Name { get; }

        string Content();
    }
}
