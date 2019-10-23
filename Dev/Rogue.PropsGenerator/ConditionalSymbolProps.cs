using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Rogue.PropsGenerator
{
    public class ConditionalSymbolProps : IProps
    {
        string[] _symbols;

        public ConditionalSymbolProps(params string[] symbols) => _symbols = symbols;

        public string Name => "Symbols";

        public string Content() => @"<Project DefaultTargets=""Build"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
<PropertyGroup>
<DefineConstants>$(DefineConstants);" + string.Join(';', _symbols) + @"</DefineConstants>
</PropertyGroup>
</Project>";
    }
}