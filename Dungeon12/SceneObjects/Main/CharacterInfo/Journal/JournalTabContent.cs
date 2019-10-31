using Dungeon.Classes;
using Dungeon.Drawing.SceneObjects;
using Dungeon.Entites.Journal;

namespace Dungeon12.Drawing.SceneObjects.Main.CharacterInfo.Talants
{
    public class JournalTabContent : HandleSceneControl
    {
        public override bool CacheAvailable => false;

        public override bool AbsolutePosition => true;

        public JournalTabContent(JournalCategory jcategory,Character character, double left)
        {
            this.Width = 12;
            this.Height = 15;            
        }
    }
}
