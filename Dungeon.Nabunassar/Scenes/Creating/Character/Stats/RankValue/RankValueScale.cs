using Dungeon.SceneObjects;
using Nabunassar.Entities.Stats.PrimaryStats;

namespace Nabunassar.Scenes.Creating.Character.Stats.RankValue
{
    internal class RankValueScale : EmptySceneObject
    {
        Func<Rank> _rankSource;

        private RankImage d4, d6, d8, d10, d12;

        public RankValueScale(Func<Rank> rankSource, double w, double h)
        {
            _rankSource = rankSource;


            double rankWidth = w;
            double rankHeight = h;

            this.Height = 75;

            double widthOffset = 10;

            d4 =this.AddChild(new RankImage(Rank.d4, rankSource() == Rank.d4, rankWidth, rankHeight));
            d6 = this.AddChild(new RankImage(Rank.d6, rankSource() == Rank.d6, rankWidth, rankHeight));
            d6.Left += rankWidth + widthOffset;
            d8 = this.AddChild(new RankImage(Rank.d8, rankSource() == Rank.d8, rankWidth, rankHeight));
            d8.Left = d6.LeftMax + widthOffset;
            d10 = this.AddChild(new RankImage(Rank.d10, rankSource() == Rank.d10, rankWidth, rankHeight));
            d10.Left = d8.LeftMax + widthOffset;
            d12 = this.AddChild(new RankImage(Rank.d12, rankSource() == Rank.d12, rankWidth, rankHeight));
            d12.Left = d10.LeftMax + widthOffset;

            this.Width = d12.LeftMax;
        }

        public override bool Updatable => true;

        public override void Update(GameTimeLoop gameTime)
        {
            var newRank = _rankSource();

            d4.UpdateVisuals(newRank);
            d6.UpdateVisuals(newRank);
            d8.UpdateVisuals(newRank);
            d10.UpdateVisuals(newRank);
            d12.UpdateVisuals(newRank);

            base.Update(gameTime);
        }
    }
}
