using Rogue.Abilities.Talants.NotAPI;
using Rogue.Abilities.Talants.TalantTrees;
using Rogue.Drawing.SceneObjects.Base;
using Rogue.Drawing.SceneObjects.Map;
using Rogue.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rogue.Drawing.SceneObjects.Main.CharacterInfo.Talants
{
    public class TalantTreeTabContent : HandleSceneControl
    {
        public override bool CacheAvailable => false;

        public override bool AbsolutePosition => true;

        public TalantTreeTabContent(TalantTree talantTree, double left)
        {
            this.Width = 12;
            this.Height = 15;

            var talants = talantTree.Talants;

            List<(Point point, TalantBase tal)> drawed = new List<(Point, TalantBase)>();

            foreach (var tier in talants)
            {
                var inTier = tier.Count();

                tier.OrderBy(t=>t.Order).ForEach((talant, i) =>
                {
                    var img = this.AddControlCenter(new TalantInfoSceneControl(talant, this.ShowEffects), inTier == 1, false);
                    img.Left -= left;

                    if (inTier == 2)
                    {
                        img.Left += i == 0 ? 2 : 8;
                    }

                    if (inTier == 3)
                    {
                        switch (i)
                        {
                            case 1: img.Left += 4; break;
                            case 2: img.Left += 6; break;
                            default: img.Left += 2; break;
                        }
                    }

                    img.Top += tier.Key * 3;
                    img.Top += 3;


                    var dependsFrom = drawed.Where(d => talant.DependentTalants.Contains(d.tal)).ToArray();
                    if (dependsFrom.Length > 0)
                    {
                        foreach (var (point, tal) in dependsFrom)
                        {
                            ConsoleColor color = tal.Opened && talant.Opened
                                ? ConsoleColor.White
                                : ConsoleColor.Black;

                            var from = new Point(point.X + 1, point.Y + 2);
                            var to = new Point(img.Left + 1, img.Top);
                            this.AddChild(new LineSceneControl(from, to,color: color)
                            {
                                Depth=2
                            });
                        }
                    }

                    drawed.Add((new Point(img.Left, img.Top), talant));
                });
            }
        }
    }
}
