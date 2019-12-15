using Dungeon;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon.Scenes;

namespace Dungeon12.Scenes.Game
{
    public class Loading : LoadingScene
    {
        public Loading()
        {
            for (int i = 1; i < 6; i++)
            {
                Global.DrawClient.CacheImage($"Loading/{i}.png".AsmImgRes());
            }
        }

        public override void Init()
        {
            var r = RandomDungeon.Range(1, 5);

            this.AddObject(new ImageControl($"Loading/{r}.png".AsmImgRes()));

            var endText = new TextControl("ЗАГРУЗКА".AsDrawText().InSize(70).Triforce());
            endText.Left = 12;
            endText.Top = 9;
            this.AddObject(endText);
        }
    }
}