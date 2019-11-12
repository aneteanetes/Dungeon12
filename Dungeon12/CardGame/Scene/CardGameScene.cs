namespace Dungeon12.CardGame.Scene
{
    using Dungeon;
    using Dungeon.Control.Keys;
    using Dungeon.Scenes;
    using Dungeon.Scenes.Manager;
    using Dungeon12.CardGame.Engine;
    using Dungeon12.Scenes.Game;
    using Dungeon12.Scenes.Menus;
    using System.Linq;

    public class CardGameScene : GameScene<Main,Start>
    {
        public CardGameScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => true;

        public override void Init()
        {
            var enemyDeckName = this.Args.FirstOrDefault();
            var enemyDeck = Deck.Load("Guardian");

            var game = new CardGame(new CardGameSettings()
            {
                Hits = 100,
                Influence = 100,
                Resources = 0
            });

            Global.Freezer.World = game;

            this.AddObject(new CardGameSceneObject(game, enemyDeck,Deck.Load("Guardian")));
        }

        protected override void KeyPress(Key keyPressed, KeyModifiers keyModifiers, bool hold)
        {
            if (keyPressed == Key.Escape)
                this.Switch<Start>();
        }
    }
}