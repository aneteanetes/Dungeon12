using Dungeon12.Abilities.Talants.TalantTrees;
using Dungeon12.Classes;
using Dungeon12.Drawing.SceneObjects.UI;
using Dungeon.Events;
using Dungeon12.SceneObjects; using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using System;
namespace Dungeon12.Drawing.SceneObjects.Main.CharacterInfo.Talants
{
    public class TalantTreeTab : TabControlFlex<TalantTreeTabContent, TalantTree, TalantTreeTab>
    {
        private readonly Character character;

        public TalantTreeTab(ISceneObject parent, TalantTree talantTree, Character character, bool active = false)
            : base(parent, active, talantTree, talantTree.Name)
        {
            this.character = character;
        }
        
        protected override TalantTreeTab Self => this;

        protected override Func<TalantTree, double, TalantTreeTabContent> CreateContent => OpenTalantTreeTab;
        
        private TalantTreeTabContent OpenTalantTreeTab(TalantTree talantTree, double left)
        {
            return new TalantTreeTabContent(talantTree,this.character, left);
        }
    }
}
