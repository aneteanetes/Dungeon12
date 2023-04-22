using Dungeon;
using Dungeon.Types;
using Dungeon12.Entities.Map;
using Dungeon12.SceneObjects.MUD.Locations;
using Dungeon12.SceneObjects.RegionScreen;
using Microsoft.Xna.Framework.Graphics;

namespace Dungeon12.SceneObjects.MUD
{
    internal class FieldLocationPanel : SceneControl<Location>
    {
        public override void Throw(Exception ex)
        {
            throw ex;
        }

        public FieldLocationPanel(Location location) : base(location)
        {
            this.Width=1120;
            this.Height = 570;

            this.Image = $"Backgrounds/Regions/"+location.BackgroundImage;

            this.AddBorderBack(.8);

            //PolygonPositionSettings();
            //return;

            location.Polygons.ForEach(p =>
            {
                var pos = PolygonPositions[$"{p.X},{p.Y}"];

                this.AddChild(new PolygonView(p,pos.X,pos.Y,polygonSize,polygonSize));
            });
        }

        private double polygonSize = 145;

        private void PolygonPositionSettings()
        {
            var icons = new string[]
            {
                "hk_new-blank_005",
                "hk_new-blank_006"
            };

            var top = 10d;
            var size = polygonSize;
            var topAdd = size/1.5;
            var leftadd = size/2.6;
            var simpleLeft = size - (size/4.6);

            var l = 8;
            var t = 5;

            var offset = 5;

            //component.Polygons;

            for (int y = 0; y < t; y++)
            {
                double left = 60;
                if (y % 2 != 0) // 1,3
                {
                    left+=leftadd;
                    left+=offset/2d;
                    l--;
                }

                for (int x = 0; x < l; x++)
                {
                    this.AddChild(new PolygonView(new Polygon()
                    {
                        Icon = icons[RandomGlobal.Next(0, 1)],
                        X=x,
                        Y=y,
                    }, left, top, size, size));

                    Console.WriteLine($@"{{""{x},{y}"", new Dot({left.ToString().Replace(",", ".")}, {top.ToString().Replace(",", ".")})}},");

                    left+=simpleLeft;
                    left+=offset;
                }

                if (y % 2 != 0)
                {
                    top+=topAdd;
                    l++;
                }
                else
                {
                    top+=topAdd;
                }
                top+=offset;
            }
        }

        private static Dictionary<string, Dot> PolygonPositions = new Dictionary<string, Dot>()
        {
            {"0,0", new Dot(60, 10)},
            {"1,0", new Dot(178.47826086956522, 10)},
            {"2,0", new Dot(296.95652173913044, 10)},
            {"3,0", new Dot(415.4347826086956, 10)},
            {"4,0", new Dot(533.9130434782609, 10)},
            {"5,0", new Dot(652.3913043478261, 10)},
            {"6,0", new Dot(770.8695652173914, 10)},
            {"7,0", new Dot(889.3478260869566, 10)},
            {"0,1", new Dot(118.26923076923077, 111.66666666666667)},
            {"1,1", new Dot(236.747491638796, 111.66666666666667)},
            {"2,1", new Dot(355.2257525083612, 111.66666666666667)},
            {"3,1", new Dot(473.70401337792646, 111.66666666666667)},
            {"4,1", new Dot(592.1822742474917, 111.66666666666667)},
            {"5,1", new Dot(710.660535117057, 111.66666666666667)},
            {"6,1", new Dot(829.1387959866222, 111.66666666666667)},
            {"0,2", new Dot(60, 213.33333333333334)},
            {"1,2", new Dot(178.47826086956522, 213.33333333333334)},
            {"2,2", new Dot(296.95652173913044, 213.33333333333334)},
            {"3,2", new Dot(415.4347826086956, 213.33333333333334)},
            {"4,2", new Dot(533.9130434782609, 213.33333333333334)},
            {"5,2", new Dot(652.3913043478261, 213.33333333333334)},
            {"6,2", new Dot(770.8695652173914, 213.33333333333334)},
            {"7,2", new Dot(889.3478260869566, 213.33333333333334)},
            {"0,3", new Dot(118.26923076923077, 315)},
            {"1,3", new Dot(236.747491638796, 315)},
            {"2,3", new Dot(355.2257525083612, 315)},
            {"3,3", new Dot(473.70401337792646, 315)},
            {"4,3", new Dot(592.1822742474917, 315)},
            {"5,3", new Dot(710.660535117057, 315)},
            {"6,3", new Dot(829.1387959866222, 315)},
            {"0,4", new Dot(60, 416.6666666666667)},
            {"1,4", new Dot(178.47826086956522, 416.6666666666667)},
            {"2,4", new Dot(296.95652173913044, 416.6666666666667)},
            {"3,4", new Dot(415.4347826086956, 416.6666666666667)},
            {"4,4", new Dot(533.9130434782609, 416.6666666666667)},
            {"5,4", new Dot(652.3913043478261, 416.6666666666667)},
            {"6,4", new Dot(770.8695652173914, 416.6666666666667)},
            {"7,4", new Dot(889.3478260869566, 416.6666666666667)},
        };
    }
}