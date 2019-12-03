namespace Dungeon12.Map.Objects
{
    using Dungeon.Data;
    using Dungeon.Data.Attributes;
    using Dungeon.Data.Region;
    using Dungeon.Drawing.SceneObjects.Map;
    using Dungeon.Entities.Fractions;
    using Dungeon.Game;
    using Dungeon.Map;
    using Dungeon.Map.Infrastructure;
    using Dungeon.Map.Objects;
    using Dungeon.Physics;
    using Dungeon.View.Interfaces;
    using Dungeon12.Data.Homes;
    using System.Linq;

    [Template("Home")]
    [DataClass(typeof(HomeData))]
    public class Home : Сonversational
    {
        public override string Icon { get => "N"; set { } }

        protected override MapObject Self => this;

        public override bool Obstruction => true;

        public override ISceneObject Visual(GameState gameState)
        {
            return new HomeSceneObject(gameState.Player, this, this.Name, gameState.Map);
        }

        public Fraction Fraction { get; set; }

        protected override void Load(RegionPart homeData)
        {
            var data = Database.Entity<HomeData>(x => x.IdentifyName == homeData.IdentifyName).FirstOrDefault();

            this.ScreenImage = data.ScreenImage;
            this.Frames = data.Frames;
            this.Name = data.Name;
            this.Size = new PhysicalSize()
            {
                Width = 32,
                Height = 32
            };
            this.Location = homeData.Position;

            if (data.FractionIdentity != default)
            {
                Fraction = FractionView.Load(data.FractionIdentity).ToFraction();
            }

            if (data.Merchant)
            {
                this.Merchant = new Dungeon.Merchants.Merchant();
                this.Merchant.FillBackpacks();
            }
            this.BuildConversations(data);
        }
    }
}