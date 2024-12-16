using Dungeon.SceneObjects;
using Nabunassar.SceneObjects.Base;

namespace Nabunassar.Scenes.Creating.Character.Names
{
    internal class NameDescriptionBlock : EmptySceneObject
    {
        TextObject _text;

        public NameDescriptionBlock()
        {
            Width = 250;
            Height = 75;

            this.AddBorderMapBack(new BorderConfiguration()
            {
                ImagesPath = "UI/bordermin/panel-border-022.png",
                Size = 16,
                Padding = 2
            });

            var txt = " ".DefaultTxt(18).WithWordWrap();

            _text = this.AddChild(new TextObject(txt)
            {
                Width = this.Width,
                Height = this.Height,
                Left = 5,
                Top = 10
            });
        }

        public void SetHint(string hint)
        {
            _text.SetText(hint);
        }
    }
}
