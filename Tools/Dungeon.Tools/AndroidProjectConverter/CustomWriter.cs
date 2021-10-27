using Dungeon.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Dungeon.AndroidProjectConverter
{
    public class CustomWriter
    {
        protected string GetTemplate(string templatName)
        {
            var tplPath = "Dungeon.AndroidProjectConverter.Templates." + templatName;
            var res = ResourceLoader.Load(tplPath);
            StreamReader reader = new StreamReader(res.Stream);
            return reader.ReadToEnd();
        }
    }
}
