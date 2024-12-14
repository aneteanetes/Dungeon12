using Dungeon.Drawing.SceneObjects;
using Nabunassar.Entities.Stats.PrimaryStats;

namespace Nabunassar.Scenes.Creating.Character.Stats.RankValue
{
    internal class RankImage : ImageObject
    {
        private Rank _rank;

        public RankImage(Rank rank, bool isEmpty, double w, double h) : base("Dices/Transparent/d5.png")
        {
            this.Width = w;
            this.Height = h;

            _rank=rank;
            UpdateVisuals(isEmpty ? Rank.None : _rank);
        }

        public void UpdateVisuals(Rank rank)
        {
            Image = $"Dices/{(rank!=_rank ? "Transparent" : "Common")}/{_rank.ToString()}.png";
        }

        public void SetVisuals(Rank rank)
        {
            Image = $"Dices/Common/{rank.ToString()}.png";
        }
    }
}
