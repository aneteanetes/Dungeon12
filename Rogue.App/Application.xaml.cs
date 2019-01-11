﻿using System.IO;
using System.Reflection;
using Avalonia.Markup.Xaml;

namespace Rogue.App
{
    public class Application : Avalonia.Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
