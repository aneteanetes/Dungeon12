using Dungeon;
using Dungeon.Types;
using Dungeon12.Entities.Map;
using Dungeon12.SceneObjects.MUD.Locations;
using Dungeon12.SceneObjects.RegionScreen;
using Microsoft.Xna.Framework.Graphics;

namespace Dungeon12.SceneObjects.MUD
{
    internal class LocationPanel : SceneControl<Location>
    {
        public override void Throw(Exception ex)
        {
            throw ex;
        }

        public LocationPanel(Location location) : base(location)
        {
            this.Width=1120;
            this.Height=600;

            this.Image = $"Backgrounds/Regions/"+location.BackgroundImage;

            this.AddBorderBack(.8);

            var icons = new string[]
            {
                "hk_new-blank_005",
                "hk_new-blank_006"
            };

            for (int y = 0; y < 5; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    if (y % 2 != 0 && x==7)
                        continue;

                    location.Polygons.Add(new Polygon()
                    {
                        Icon = icons[RandomGlobal.Next(0, 1)],
                        X=x,
                        Y=y,
                    });
                }
            }

            location.Polygons.ForEach(p =>
            {
                var pos = PolygonPositions[$"{p.X},{p.Y}"];

                this.AddChild(new PolygonView(p)
                {
                    Left = pos.X,
                    Top = pos.Y,
                    Width=polygonSize,
                    Height=polygonSize,
                });
            });
        }

        private double polygonSize = 160;

        private void PolygonPositionSettings(string[] icons)
        {
            var top = -5d;
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
                double left = 25;
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
                    })
                    {
                        Left = left,
                        Top = top,
                        Width=size,
                        Height=size,
                    });

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
            {"0,0", new Dot(25, -5)},
            {"1,0", new Dot(155.2173913043478, -5)},
            {"2,0", new Dot(285.4347826086956, -5)},
            {"3,0", new Dot(415.65217391304344, -5)},
            {"4,0", new Dot(545.8695652173913, -5)},
            {"5,0", new Dot(676.086956521739, -5)},
            {"6,0", new Dot(806.3043478260868, -5)},
            {"7,0", new Dot(936.5217391304345, -5)},
            {"0,1", new Dot(89.03846153846153, 106.66666666666667)},
            {"1,1", new Dot(219.25585284280936, 106.66666666666667)},
            {"2,1", new Dot(349.4732441471572, 106.66666666666667)},
            {"3,1", new Dot(479.690635451505, 106.66666666666667)},
            {"4,1", new Dot(609.9080267558528, 106.66666666666667)},
            {"5,1", new Dot(740.1254180602007, 106.66666666666667)},
            {"6,1", new Dot(870.3428093645484, 106.66666666666667)},
            {"0,2", new Dot(25, 218.33333333333334)},
            {"1,2", new Dot(155.2173913043478, 218.33333333333334)},
            {"2,2", new Dot(285.4347826086956, 218.33333333333334)},
            {"3,2", new Dot(415.65217391304344, 218.33333333333334)},
            {"4,2", new Dot(545.8695652173913, 218.33333333333334)},
            {"5,2", new Dot(676.086956521739, 218.33333333333334)},
            {"6,2", new Dot(806.3043478260868, 218.33333333333334)},
            {"7,2", new Dot(936.5217391304345, 218.33333333333334)},
            {"0,3", new Dot(89.03846153846153, 330)},
            {"1,3", new Dot(219.25585284280936, 330)},
            {"2,3", new Dot(349.4732441471572, 330)},
            {"3,3", new Dot(479.690635451505, 330)},
            {"4,3", new Dot(609.9080267558528, 330)},
            {"5,3", new Dot(740.1254180602007, 330)},
            {"6,3", new Dot(870.3428093645484, 330)},
            {"0,4", new Dot(25, 441.6666666666667)},
            {"1,4", new Dot(155.2173913043478, 441.6666666666667)},
            {"2,4", new Dot(285.4347826086956, 441.6666666666667)},
            {"3,4", new Dot(415.65217391304344, 441.6666666666667)},
            {"4,4", new Dot(545.8695652173913, 441.6666666666667)},
            {"5,4", new Dot(676.086956521739, 441.6666666666667)},
            {"6,4", new Dot(806.3043478260868, 441.6666666666667)},
            {"7,4", new Dot(936.5217391304345, 441.6666666666667)},
        };
    }
}