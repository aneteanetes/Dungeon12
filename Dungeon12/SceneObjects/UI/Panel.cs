using Dungeon;
using Dungeon.SceneObjects;
using System;
using System.Collections.Generic;

namespace Dungeon12.SceneObjects.UI
{
    internal class Panel : EmptySceneControl
    {
        public Panel()
        {
            this.Width = 570;
            this.Height = 120;
            Image = "UI/panel.png".AsmImg();

            var x = 45;

            btns.ForEach(btn =>
            {
                var btnobj = this.AddChildCenter(new PanelButton($"UI/{btn.img}".AsmImg(), btn.tooltip), false);
                btnobj.OnClick = btn.click;
                btnobj.Left = x;

                x += 80;
            });
        }

        static List<(string img, string tooltip, Action click)> btns => new List<(string img, string tooltip, Action click)>()
        {
            ("char.png","Персонаж",()=>{ }),
            ("skills.png","Влияние",()=>{ }),
            ("quests.png","Дневник",()=>{ }),
            ("map.png","Карта",()=>{ }),
            ("batlepos.png","Строй",()=>{ }),
            ("esc.png","Меню",()=>{ })
        };
    }
}