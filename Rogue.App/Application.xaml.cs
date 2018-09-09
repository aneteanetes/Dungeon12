using System.IO;
using System.Reflection;
using Avalonia.Markup.Xaml;

namespace Rogue.App
{
    public class Application : Avalonia.Application
    {
        public override void Initialize()
        {
            //var assembly = Assembly.GetExecutingAssembly();
            //var resourceName = "Rogue..MyProduct.MyFile.txt";

            //using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            //using (StreamReader reader = new StreamReader(stream))
            //{
            //    string result = reader.ReadToEnd();
            //}

            AvaloniaXamlLoader.Load(this);
        }
    }
}
