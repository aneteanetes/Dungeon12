using Dungeon;
using Dungeon.SceneObjects;

namespace SidusXII.SceneObjects.Main
{
    public class CharBar : EmptySceneControl
    {
        public CharBar()
        {
            this.Image = "GUI/Planes/char_back.png".AsmImg();
            this.Width = 410;
            this.Height = 157;

            this.AddChild(new HpBar())
                .Build(x => x.Left = 10)
                .Build(x => x.Top = 15);

            this.AddChild(new ExpBar())
                .Build(x => x.Left = 4)
                .Build(x => x.Top = 138);

            this.AddChild(new Skill())
                .Build(x => x.Left = 10)
                .Build(x => x.Top = 42);

            this.AddChild(new Skill())
                .Build(x => x.Left = 109)
                .Build(x => x.Top = 42);

            this.AddChild(new Skill())
                .Build(x => x.Left = 208)
                .Build(x => x.Top = 42);

            this.AddChild(new Skill())
                .Build(x => x.Left = 307)
                .Build(x => x.Top = 42);
        }
    }
}