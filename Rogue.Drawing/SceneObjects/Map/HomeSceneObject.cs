namespace Rogue.Drawing.SceneObjects.Map
{
    using Rogue.Map.Objects;
    using Rogue.View.Interfaces;
    using System;
    using System.Collections.Generic;

    public class HomeSceneObject : TooltipedSceneObject
    {
        public HomeSceneObject(Home home, string tooltip, Action<List<ISceneObject>> showEffects) : base(tooltip, showEffects)
        {
            Left = home.Location.X;
            Top = home.Location.Y;
            Width = 1;
            Height = 1;
        }
    }
}