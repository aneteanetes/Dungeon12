namespace Rogue.Drawing.SceneObjects.Main
{
    using Rogue.Control.Keys;
    using Rogue.Drawing.SceneObjects.Common;
    using Rogue.Entites.Alive.Character;
    using System.Collections.Generic;

    public class SkillBar : SceneObject
    {
        public SkillBar(Rogue.Map.Objects.Avatar player)
        {
            //this.Children.Add(new ImageControl("Rogue.Resources.Images.ui.sphere.png"));
            //this.Children.Add(new ImageControl("Rogue.Resources.Images.ui.sphere.png")
            //{
            //    Left = 15.5f
            //});

            var x = 4.9;

            for (int i = 0; i < 6; i++)
            {
                this.AddChild(new SkillControl(KeyMapping[i])
                {
                    Left = x,
                    Top = 2
                });

                x += 2;
            }
        }

        private static Dictionary<int, Key> KeyMapping => new Dictionary<int, Key>()
        {
            {0, Key.D1 },
            {1, Key.D2 },
            {2, Key.D3 },
            {3, Key.D4},
            {4, Key.D5},
            {5, Key.D6}
        };
    }
}
