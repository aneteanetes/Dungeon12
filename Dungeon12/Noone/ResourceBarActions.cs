namespace Dungeon12
{
    using Dungeon;
    using Dungeon.Drawing.SceneObjects;
    using Dungeon12.Drawing.SceneObjects.UI;
    using Dungeon12.Noone;
    using System.Linq;

    public class ResourceBarActions : ResourceBar<Noone.Noone>
    {
        private ActionBarSB[] actions;

        public ResourceBarActions(Noone.Noone avatar) : base(avatar)
        {
            actions = Enumerable.Range(0, 5)
                .Select((x, i) =>
                {
                    var img = new ActionBarSB("Images/action.png".NoonePath())
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

        private class ActionBarSB : Dungeon.Drawing.SceneObjects.ImageObject
        {
            public ActionBarSB(string imagePath) : base(imagePath)
            {
            }

            public override bool CacheAvailable => false;
        }
    }
}