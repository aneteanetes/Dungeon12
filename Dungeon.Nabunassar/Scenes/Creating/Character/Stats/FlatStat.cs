using Dungeon.Control;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using Nabunassar.Entities;

namespace Nabunassar.Scenes.Creating.Character.Stats
{
    internal class FlatStat : SceneControl<Hero>
    {
        Func<IDrawText> _statUpd;

        private TextObject _text;

        private string _hint;

        protected override ControlEventType[] Handles => [ControlEventType.Focus];

        public FlatStat(Hero component, string img, Func<IDrawText> statUpd, string hint) : base(component)
        {
            _hint = hint;
            this.Width = 50;
            this.Height = 35;
            _statUpd=statUpd;

            this.AddChild(new ImageObject(img) { Width = 30, Height = 30, Color = Global.CommonColorLight });

            _text = this.AddText("_".DefaultTxt(20), 37,-10);
        }

        public override bool Updatable => true;

        public override void Update(GameTimeLoop gameTime)
        {
            _text.SetText(_statUpd());
        }

        public override void Focus()
        {
            Global.Game.Creation.Hint = _hint;
            base.Focus();
        }

        public override void Unfocus()
        {
            Global.Game.Creation.Hint = Global.Strings["guide"]["stats"];
            base.Unfocus();
        }
    }
}
