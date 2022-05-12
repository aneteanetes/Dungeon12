using Dungeon;
using Dungeon.SceneObjects;

namespace Dungeon12.SceneObjects.Create
{
    public class CreateTitle : EmptySceneObject
    {
        public CreateTitle()
        {
            this.Width = 564;
            this.Height = 124;
            this.Image = "UI/start/title.png".AsmImg();

            this.AddTextCenter(Global.Strings["CreateParty"].AsDrawText().InColor(Global.CommonColor).Gabriela().InSize(35));
        }
    }
}