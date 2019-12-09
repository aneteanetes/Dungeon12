using Dungeon;
using Dungeon12.Abilities.Talants.NotAPI;
using Dungeon12.Abilities.Talants.TalantTrees;
using Dungeon12.Classes;
using Dungeon.Drawing.SceneObjects;
using Dungeon12.SceneObjects; using Dungeon.SceneObjects;
using Dungeon.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon12.Drawing.SceneObjects.Main.CharacterInfo.Talants
{
    public class TalantTreeTabContent : HandleSceneControl<TalantTree>
    {
        public override bool CacheAvailable => false;

        public override bool AbsolutePosition => true;

        public TalantTreeTabContent(TalantTree talantTree,Character character, double left):base(talantTree)
        {
            this.Width = 12;
            this.Height = 15;

            var talants = talantTree.Talants;

            List<(Point point, TalantBase tal)> drawed = new List<(Point, TalantBase)>();

            foreach (var tier in talants)
            {
                var index = talants.IndexOf(tier);
                var inTier = tier.Count();

                tier.OrderBy(t=>t.Order).ForEach((talant, i) =>
                {
                    var blocked = index > 1;
                    if (character.GetType().Name.Contains("Servant"))
                    {
                        blocked = index >= 1;
                    }

                    var img = this.AddControlCenter(new TalantInfoSceneControl(talant, character, blocked), inTier == 1, false);
                    img.Left -= left;

                    if (inTier == 2)
                    {
                        img.Left += i == 0 ? 2 : 8;
                    }

                    if (inTier == 3)
                    {
                        switch (i)
                        {
                            case 1: img.Left += 5; break;
                            case 2: img.Left += 8; break;
                            default: img.Left += 2; break;
                        }
                    }

                    if (inTier == 4)
                    {
                        switch (i)
                        {
                            case 1: img.Left += 4; break;
                            case 2: img.Left += 7; break;
                            case 3: img.Left += 10; break;
                            default: img.Left += 1; break;
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
                            this.AddChild(new LineSceneControl(new LineSceneModel(){
                                From=from,
                                To=to,
                                Color=color
                            })
                            {
                                Depth = 2
                            });
                        }
                    }

                    drawed.Add((new Point(img.Left, img.Top), talant));
                });
            }
        }
    }
}
