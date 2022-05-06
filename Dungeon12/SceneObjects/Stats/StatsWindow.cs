using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon12.Entities;
using Dungeon12.SceneObjects.Base;

namespace Dungeon12.SceneObjects.Stats
{
    public class StatsWindow : SceneControl<Hero>
    {
        Title CharacterName;
        ImageObject Avatar;

        public StatsWindow() : base(Global.Game.Party.Hero1)
        {
            this.Width=1141;
            this.Height =554;

            this.Image="UI/Windows/Stats/back.png";

            this.AddChild(new Title(Global.Strings.PartyInventory, 325, 42)
            {
                Left=783,
                Top=28,
            });

            Avatar = this.AddChild(new ImageObject(Component.Avatar)
            {
                Width=115,
                Height=180,
                Left=554,
                Top=186,
            });
        }

        private void Fill(Hero hero)
        {
            CharacterName?.Destroy();
            Avatar?.Destroy();

            CharacterName = this.AddChild(new Title(Component.Name, 425, 38)
            {
                Left=297,
                Top=32,
            });
        }
    }
}