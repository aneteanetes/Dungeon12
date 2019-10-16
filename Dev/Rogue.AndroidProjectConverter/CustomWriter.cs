using Rogue.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Rogue.AndroidProjectConverter
{
    public class CustomWriter
    {
        protected string GetTemplate(string templatName)
        {
            var tplPath = "Rogue.AndroidProjectConverter.Templates." + templatName;
            var stream = ResourceLoader.Load(tplPath, tplPath);
            StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}
