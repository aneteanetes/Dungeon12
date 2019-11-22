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
            var stream = ResourceLoader.Load(tplPath);
            StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}
