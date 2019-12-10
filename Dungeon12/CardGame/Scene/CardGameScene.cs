namespace Dungeon12.CardGame.Scene
{
    using Dungeon.Control.Keys;
    using Dungeon.Drawing;
    using Dungeon12.Drawing.SceneObjects.UI;
    using Dungeon12.SceneObjects;
    using Dungeon.SceneObjects;
    using Dungeon.Scenes;
    using Dungeon.Scenes.Manager;
    using Dungeon12.CardGame.Engine;
    using Dungeon12.CardGame.SceneObjects;
    using Dungeon12.Drawing.SceneObjects;
    using Dungeon12.Scenes.Game;
    using Dungeon12.Scenes.Menus;
    using System;
    using System.Linq;
    using Dungeon12;

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
                Hits = 20,
                Influence = 25,
                Resources = 1
            });

            Global.Freezer.World = game;

            var dropMask = new CardDropMask();
            var cardgameSceneObject = new CardGameSceneObject(game, enemyDeck, Deck.Load("Guardian"), dropMask);
            this.AddObject(cardgameSceneObject);
            this.AddObject(dropMask);
            
            game.OnWin += winner =>
            {
                dropMask.Destroy?.Invoke();
                cardgameSceneObject.Destroy?.Invoke();

                this.AddObject(new Background());

                var endText = new TextControl(new DrawText($"Выйграл {winner.Name}", ConsoleColor.Blue).Triforce());
                endText.Text.Size = 72;
                endText.Left = 8;
                endText.Top = 9;
                this.AddObject(endText);

                this.AddObject(new MetallButtonControl("Ок")
                {
                    Left = 15.5f,
                    Top = 17,
                    OnClick = () =>
                    {
                        Environment.Exit(0);
                    }
                });

                Global.Time.Timer(Guid.NewGuid().ToString())
                    .After(1000)
                    .Do(() =>
                    {
                        this.Switch<Main>();
                    }).Auto();
            };
        }

        protected override void KeyPress(Key keyPressed, KeyModifiers keyModifiers, bool hold)
        {
            if (keyPressed == Key.Escape)
                this.Switch<Main>();
        }
    }

    public class CardDropMask : DropableControl<CardSceneObject>
    {
        public CardDropMask()
        {
            this.Width = 40;
            this.Height = 22.5;
        }


        public Action OnDropInMask { get; set; }

        protected override void OnDrop(CardSceneObject source)
        {
            source.Destroy?.Invoke();
            OnDropInMask?.Invoke();
        }
    }
}