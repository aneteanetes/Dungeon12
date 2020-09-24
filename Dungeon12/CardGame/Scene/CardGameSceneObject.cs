using Dungeon;
using Dungeon12.Drawing.SceneObjects.UI;
using Dungeon12.SceneObjects; using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using Dungeon12.CardGame.Engine;
using Dungeon12.CardGame.SceneObjects;
using System;

namespace Dungeon12.CardGame.Scene
{
    public class CardGameSceneObject : Dungeon12.SceneObjects.HandleSceneControl<Engine.CardGame>
    {
        public CardGameSceneObject(Engine.CardGame component, Deck enemyDeck, Deck playerDeck, CardDropMask cardDropMask) : base(component, true)
        {
            this.Width = 40;
            this.Height = 22.5;

            this.Image = "ui/horizontal(40x225).png".AsmImgRes();

            var (Player1, Player2) = component.Start(playerDeck, "Персонаж", enemyDeck, "Трактирщик");

            var pLeft = new CardPlayerSceneObject(Player1)
            {
                Left = 1,
                Top = 1
            };
            var pRight = new CardPlayerSceneObject(Player2, true);
            pRight.Top = 1;
            pRight.Left = 40 - pRight.Width;

            this.AddChild(pLeft);
            this.AddChild(pRight);

            this.AddChild(new LandCardSceneObject(component)
            {
                Left = 16,
                Top = 1
            });

            component.Turn();
            component.PlayerTurn(Player1);

            var gameDesk = this.AddChildCenter(new GameDeskSceneObject(component));
            gameDesk.Left += 1.7;
            gameDesk.Top -= 1;
            gameDesk.AfterHandPlayed = EnemyTurn;

            var cardInHands = new CardInHands(Player1, () => EnemyTurn(gameDesk))
            {
                Left = 7.5,
                Top = 14,
                AbsolutePosition = true,
                CacheAvailable = false
            };
            this.AddChild(cardInHands);

            cardDropMask.OnDropInMask += cardInHands.Redraw;

            this.Destroy += () => enemyCardPlaying?.Destroy?.Invoke();
        }

        private CardSceneObject enemyCardPlaying = default;

        private void EnemyTurn(GameDeskSceneObject gameDeskSceneObject)
        {
            Component.PlayerTurn(Component.Player2);

            var card = Component.Player2.Auto(Component);

            if (card != default)
            {
                var c = new CardSceneObject(card, Component.Player2)
                .Maximize();

                var left = Width / 2d - (4.65625 * 1.5) / 2d;
                c.Left = left-1;

                var top = Height / 2d - (7 * 1.5) / 2d;
                c.Top = top;

                var @lock = new LockSceneObject();
                this.ShowInScene(@lock.InList<ISceneObject>());
                Global.Freezer.World = @lock;

                enemyCardPlaying = c;
                c.OnFlush = () => AfterCardFlushed(gameDeskSceneObject, card, c);

                Global.Time.Timer(Guid.NewGuid().ToString())
                    .After(500)
                    .Do(() =>
                    {
                        this.ShowInScene(c.InList<ISceneObject>());
                        Global.Freezer.World = c;
                        @lock.Destroy?.Invoke();
                        Global.Time.Timer(Guid.NewGuid().ToString())
                            .After(2000)
                            .Do(() =>
                            {
                                AfterCardFlushed(gameDeskSceneObject, card, c);
                            })
                            .Trigger();
                    }).Auto();
            }
            else
            {
                Toast.Show($"{Component.Player2.Name} пропускает ход", this.ShowInScene);
            }
        }

        private class LockSceneObject : EmptyControlSceneObject
        { }

        private void AfterCardFlushed(GameDeskSceneObject gameDeskSceneObject, Entities.Card card, CardSceneObject c)
        {
            if (c.Flushed)
                return;

            try
            {
                var wasGuards = Component.Player2.Guards.Count;
                Component.PlayCard(card, Component.Player2);
                var nowGuards = Component.Player2.Guards.Count;

                switch (card.CardType)
                {
                    case Interfaces.CardType.Guardian:
                        if (wasGuards < nowGuards)
                        {
                            gameDeskSceneObject.AddCardEnemy(card, Component.Player2);
                        }
                        break;
                    default:
                        break;
                }

                Component.Turn();
                Component.PlayerTurn(Component.Player1);
                c.Destroy?.Invoke();

                Global.Freezer.World = null;
            }
            catch { }

            gameDeskSceneObject.RefreshDeck();
        }
    }
}