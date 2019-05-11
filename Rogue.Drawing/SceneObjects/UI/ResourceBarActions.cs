namespace Rogue.Drawing.SceneObjects.UI
{
    using Rogue.Entites.Alive.Character;
    using System.Linq;

    public class ResourceBarActions : ResourceBar
    {
        private ActionBarSB[] actions;

        public ResourceBarActions(Player avatar) : base(avatar)
        {
            actions = Enumerable.Range(0, 5)
                .Select((x, i) =>
                {
                    var img = new ActionBarSB("Rogue.Resources.Images.ui.player.action.png")
                    {
                        Height = 0.5,
                        Width = 0.8,
                        Left = i + (i == 0 ? 0 : 0.05)
                    };
                    return img;
                })
                .ToArray();

            actions.ForEach(this.AddChild);
        }

        protected override string BarTile => string.Empty;

        public override double Width
        {
            get
            {
                Enumerable.Range(1, 5).ForEach(Check);

                return base.Width;
            }
            set => base.Width = value;
        }

        private void Check(int num)
        {
            if (Player.Actions < num)
            {
                actions[num - 1].Width = 0;
            }
            else
            {
                actions[num - 1].Width = 0.8;
            }
        }

        public override bool CacheAvailable => false;

        private class ActionBarSB : ImageControl
        {
            public ActionBarSB(string imagePath) : base(imagePath)
            {
            }

            public override bool CacheAvailable => false;
        }
    }
}