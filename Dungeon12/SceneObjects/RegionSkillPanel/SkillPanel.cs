using Dungeon.SceneObjects;
using Dungeon12.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon12.SceneObjects.RegionSkillPanel
{
    public class SkillPanel : SceneControl<Party>
    {
        public SkillPanel(Party component) : base(component)
        {
            this.Width = 75;
            this.Height = 425;

            this.Left = 45;
            this.Top = 40; //+15

            var skillbtns = new List<SkillButton>();

            if (component.Heroes.Any(h => h.Trade > 0))
            {
                skillbtns.Add(new SkillButton(Entities.Enums.Skill.Trade, TradeWindow));
            }

            if (component.Heroes.Any(h => h.Spiritism > 0))
            {
                skillbtns.Add(new SkillButton(Entities.Enums.Skill.Spiritism, SpiritMode));
            }

            if (component.Heroes.Any(h => h.Landscape > 0))
            {
                skillbtns.Add(new SkillButton(Entities.Enums.Skill.Landscape, LandscapeEdit));
            }

            if (component.Heroes.Any(h => h.Traps > 0))
            {
                skillbtns.Add(new SkillButton(Entities.Enums.Skill.Traps, TrapsWindow));
            }

            if (component.Heroes.Any(h => h.Portals > 0))
            {
                skillbtns.Add(new SkillButton(Entities.Enums.Skill.Portals, PortalWindow));
            }

            var top = 0;

            foreach (var skillbtn in skillbtns)
            {
                skillbtn.Top = top;
                this.AddChild(skillbtn);
                top += 90;
            }
        }

        private void TradeWindow() { }
        
        private void SpiritMode() { }

        private void LandscapeEdit() { }

        private void TrapsWindow() { }

        private void PortalWindow() { }
    }
}