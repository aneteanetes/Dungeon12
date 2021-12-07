using Dungeon;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using Dungeon12.SceneObjects.Base;

namespace Dungeon12.SceneObjects.UserInterface
{
    public class GameTooltip : Tooltip
    {
        public GameTooltip(string title, string text, int width=0, bool nodesc=false) : base(null, null)
        {
            Opacity = 1;

            this.Image = "Backgrounds/plate250300.png".AsmImg();

            Width = width;

            var titletext = this.AddTextCenter(title.AsDrawText().Gabriela().InSize(12), vertical: false);
            if (Width == 0)
                titletext.Left = 7;
            titletext.Top = 5;

            var titlesize = MeasureText(titletext.Text);

            if (Width == 0)
                Width = titlesize.X + 15;

            var line = this.AddChild(new ImageObject("Backgrounds/line230.png".AsmImg())
            {
                Width = this.Width - 6,
                Height=5,
                Left = 3,
                Top = titletext.Top + titlesize.Y + 10
            });

            if (!nodesc)
            {

                var desctext = text.AsDrawText().Gabriela().InSize(12).WithWordWrap();
                var desctextsize = MeasureText(desctext, new EmptySceneObject() { Width = this.Width });

                Height = desctextsize.Y + 55;

                var desc = this.AddTextCenter(desctext, true, false);
                desc.Top = line.Top + 10;
                if (width == 0)
                    desc.Left = 5;
            }
            else
            {
                Height = 55;
            }

            TooltipText = titletext.Text;
        }

        public override IDrawablePath Path => null;
    }
}
