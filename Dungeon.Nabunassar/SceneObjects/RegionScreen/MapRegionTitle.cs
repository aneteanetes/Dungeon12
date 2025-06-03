using Dungeon;
using Dungeon.SceneObjects;
using Nabunassar.Entities.Map;
using System;

namespace Nabunassar.SceneObjects.RegionScreen
{
    internal class MapRegionTitle : SceneObject<MapRegion>
    {

        public MapRegionTitle(MapRegion component) : base(component)
        {
            Image = "UI/layout/titleback.png".AsmImg();
            this.Width = 348;
            this.Height = 100;
            this.Left = 768;
            this.Top = 19;

            this.AddTextCenter(component.Name.AsDrawText().Gabriela().InColor(ConsoleColor.Black).InSize(20));
        }
    }
}