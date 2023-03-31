using Dungeon;
using Dungeon.SceneObjects;
using Dungeon12.Entities.Map;
using System;

namespace Dungeon12.SceneObjects.RegionScreen
{
    internal class LocationTitle : SceneObject<Location>
    {
        public LocationTitle(Location component) : base(component)
        {
            Image = "UI/layout/titleback.png".AsmImg();
            this.Width = 348;
            this.Height = 100;
            //this.Left = 768;
            this.Top = 5;

            this.AddTextCenter(component.Name.AsDrawText().Gabriela().InColor(ConsoleColor.Black).InSize(20));
        }
    }
}