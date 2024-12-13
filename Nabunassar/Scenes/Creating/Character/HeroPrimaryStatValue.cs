using Dungeon.SceneObjects;
using Nabunassar.Entities;
using Nabunassar.Scenes.Creating.Character.Stats.RankValue;

namespace Nabunassar.Scenes.Creating.Character
{
    internal class HeroPrimaryStatValue : SceneObject<Hero>
    {
        private RankImage _rankImage;
        private int _idx;

        public HeroPrimaryStatValue(Hero component, int idx) : base(component)
        {
            _idx = idx;
            this.Width = 40;
            this.Height = 100;

            if (component == null)
                return;

            string txt = Global.Strings[component.PrimaryStats.GetName(idx)[0].ToString()];

            this.AddTextCenter(txt.DefaultTxt(25).FrizQuad(), vertical: false);

            _rankImage = this.AddChild(new RankImage(component.PrimaryStats[idx], false, 40, 40));
            _rankImage.Top = 25;
        }

        public override bool Updatable => true;

        public override void Update(GameTimeLoop gameTime)
        {
            if (_rankImage != null)
                _rankImage.SetVisuals(Component.PrimaryStats[_idx]);
            base.Update(gameTime);
        }
    }
}
