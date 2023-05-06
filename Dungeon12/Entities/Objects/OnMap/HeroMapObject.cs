using Dungeon.SceneObjects.Grouping;
using Dungeon12.Entities.Turning;

namespace Dungeon12.Entities.Objects.OnMap
{
    internal class HeroMapObject : MapObject
    {
        private readonly Hero _hero;
        
        public HeroMapObject(Hero hero)
        {
            _hero=hero;
            _hero.OnSelect(() => Selected.True());
            GameObject=hero;
        }

        public ObjectGroupProperty Selected { get; set; } = new();

        public override bool IsSelected { get => Selected; set => Selected.Set(value); }

        public override void Select()
        {
            if (this.Selected)
                return;

            var turn = Global.Game.Turns.TurnHero(_hero);
            if (turn.IsSuccess())
            {
                this.Selected.True();
                _hero.IsActive.True();
            }
        }
    }
}
