using Dungeon;
using Dungeon.Drawing.SceneObjects;
using Dungeon12.Drawing.SceneObjects.Map;
using Dungeon12.SceneObjects; using Dungeon.SceneObjects;
using Dungeon12.Entities.Quests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.SceneObjects.UI
{
    public class RewardSceneObject : Dungeon12.SceneObjects.HandleSceneControl<Reward>
    {
        public override bool AbsolutePosition => true;
        public override bool CacheAvailable => false;

        public RewardSceneObject(Reward component) : base(component, false)
        {
            this.Width = 12;
            this.Height = 2;

            this.AddChild(new DarkRectangle()
            {
                Width=this.Width,
                Height=this.Height
            });

            double left = 3.5;            

            for (int i = 0; i < 4; i++)
            {
                string image = "";
                string tooltip = "";
                switch (i)
                {
                    case 0:
                        {
                            if (component.Exp > 0)
                            {
                                image = "Icons/exp.png".AsmImgRes();
                                tooltip = $"Опыт: {component.Exp}";
                            }
                            break;
                        }
                    case 1:
                        {
                            if (component.Gold > 0)
                            {
                                image = "Items/gold_b.png".AsmImgRes();
                                tooltip = $"Золото: {component.Gold}";
                            }
                            break;
                        }
                    case 2:
                        {
                            if (component.Perks.Count > 0)
                            {
                                image = "Items/gold_b.png".AsmImgRes();
                                tooltip = $"Способности: Хождение по воде, Материк";
                            }
                            break;
                        }
                    case 3:
                        {
                            if (component.ItemGenerators.Count > 0)
                            {
                                image = "Items/gold_b.png".AsmImgRes();
                                tooltip = $"Предметы: Оружие, Кольцо";
                            }
                            break;
                        }
                    default:
                        break;
                }

                var reward = new RewardIconObject(image, tooltip);
                this.AddControlCenter(reward, false, false);
                reward.Left = left;
                left += 1.25;
            }
        }

        private class RewardIconObject : EmptyTooltipedSceneObject
        {
            public override bool AbsolutePosition => true;

            public override bool CacheAvailable => false;

            public RewardIconObject(string img, string tooltip) : base(tooltip)
            {
                this.Image = img;
                this.AddChild(new DarkRectangle()
                {
                    Width = 1,
                    Height = 1
                });

                this.Width = 1;
                this.Height = 1;
            }
        }
    }
}
