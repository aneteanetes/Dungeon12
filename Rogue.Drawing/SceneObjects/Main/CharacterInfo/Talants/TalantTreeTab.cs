using Rogue.Abilities.Talants.TalantTrees;
using Rogue.Drawing.SceneObjects.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rogue.Drawing.SceneObjects.Main.CharacterInfo.Talants
{
    public class TalantTreeTab : TabControlFlex<TalantTreeTabContent, TalantTree, TalantTreeTab>
    {
        public TalantTreeTab(SceneObject parent, TalantTree talantTree, bool active = false)
            : base(parent, active, talantTree, talantTree.Name) { }
        
        protected override TalantTreeTab Self => this;

        protected override Func<TalantTree, double, TalantTreeTabContent> CreateContent => OpenTalantTreeTab;

        private TalantTreeTabContent OpenTalantTreeTab(TalantTree talantTree, double left)
        {
            return new TalantTreeTabContent(talantTree, left);
        }
    }
}
