namespace Rogue.Drawing.SceneObjects.Main
{
    using Rogue.Drawing.SceneObjects.Common;
    using Rogue.Entites.Alive.Character;

    public class SkillBar : SceneControl
    {
        public SkillBar(Player player)
        {
            this.Children.Add(new ImageControl("Rogue.Resources.Images.ui.sphere.png"));
            this.Children.Add(new ImageControl("Rogue.Resources.Images.ui.sphere.png")
            {
                Left = 15.5f
            });

            var x = 4.9;

            for (int i = 0; i < 6; i++)
            {
                this.AddChild(new SkillControl()
                {
                    Left = x,
                    Top = 2
                });

                x += 2;
            }
        }
    }
}
