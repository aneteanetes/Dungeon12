namespace Dungeon12.Map.Objects
{
    using Dungeon.Data;
    using Dungeon.Data.Attributes;
    using Dungeon12.Data.Region;
    using Dungeon12.Drawing.SceneObjects.Map;
    using Dungeon12.Entities.Fractions;
    using Dungeon12.Game;
    using Dungeon12.Map;
    using Dungeon12.Map.Infrastructure;
    using Dungeon12.Map.Objects;
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

        public override ISceneObject Visual()
        {
            return new HomeSceneObject(Global.GameState.Player, this, this.Name, Global.GameState.Map);
        }

        public Fraction Fraction { get; set; }

        protected override void Load(RegionPart homeData)
        {
            var data = Dungeon.Store.Entity<HomeData>(x => x.IdentifyName == homeData.IdentifyName).FirstOrDefault();

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
                this.Merchant = new Dungeon12.Merchants.Merchant();
                this.Merchant.FillBackpacks();
            }
            this.BuildConversations(data);
        }
    }
}