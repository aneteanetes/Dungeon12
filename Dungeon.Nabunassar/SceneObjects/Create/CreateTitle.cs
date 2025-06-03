using Dungeon;
using Dungeon.SceneObjects;

namespace Nabunassar.SceneObjects.Create
{
    internal class CreateTitle : EmptySceneObject
    {
        public CreateTitle()
        {
            this.Width = 564;
            this.Height = 124;
            this.Image = "UI/start/title.png".AsmImg();

            this.AddTextCenter(Global.Strings["CreateParty"].ToString().AsDrawText().InColor(Global.CommonColor).Gabriela().InSize(50));
        }
    }
}