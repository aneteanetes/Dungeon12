using Dungeon;
using Dungeon.Control;
using Dungeon.Drawing.SceneObjects;
using Dungeon12.Drawing.SceneObjects.UI;
using Dungeon12.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon12.CardGame.Entities;
using Dungeon12.CardGame.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon12.CardGame.SceneObjects
{
    public class CardSceneObject : DraggableControl<CardSceneObject>
    {
        public Card Card { get; private set; }
        
        public override bool DestroyOnEscape => false;

        public override bool BlockSceneControls => false;

        public CardGamePlayer Player { get; private set; }

        public CardSceneObject(Card card, CardGamePlayer player)
        {
            this.TooltipText = "Карта: " + card.Name;
            Player = player;
            this.Card = card;
            this.Width = 4.65625;
            this.Height = 7;

            this.Image = $"Cards/Guardian/{ImageMap[card.CardType]}0.png".AsmImgRes();

            switch (card.CardType)
            {
                case CardType.Guardian:
                    AsGuard(card.As<GuardCard>());
                    break;
                case CardType.Ability:
                    AsAbility(card.As<AbilityCard>());
                    break;
                case CardType.Region:
                    AsRegion(card.As<AreaCard>());
                    break;
                case CardType.Resource:
                    AsResource(card.As<Card>());
                    break;
                default:
                    break;
            }
        }

        public bool Maximized { get; set; } = false;

        public Action OnFlush { get; set; }

        public bool Flushed { get; set; } = false;

        public override void Click(PointerArgs args)
        {
            if(Maximized)
            {
                OnFlush?.Invoke();
                Flushed = true;
                return;
            }

            if (args.MouseButton == Dungeon.Control.Pointer.MouseButton.Right)
            {
                Toast.Show("Вы сбросили карту!");
                Player.Discard(this.Card);
            }
            else
            {
                base.Click(args);
            }
        }

        public CardSceneObject Minimize()
        {
            this.ScaleTo(.6);
            GuardShieldText.Top = 3.2;
            return this;
        }

        public CardSceneObject Maximize()
        {
            this.ScaleTo(1.5);
            this.Maximized = true;
            return this;
        }

        public override void Update()
        {
            switch (Card.CardType)
            {
                case CardType.Guardian:
                    UpdateAsGuard(Card.As<GuardCard>());
                    break;
                case CardType.Ability:
                    UpdateAsAbility(Card.As<AbilityCard>());
                    break;
                case CardType.Region:
                    UpdateAsRegion(Card.As<AreaCard>());
                    break;
                default:
                    break;
            }
        }

        private void AsResource(Card card)
        {
            this.Image = "Cards/Guardian/resource0.png".AsmImgRes();
        }

        private TextControl CardNameControl;
        private TextControl GuardShieldText;
        private void AsGuard(GuardCard guardCard)
        {
            CardNameControl = this.AddTextCenter(guardCard.Name.AsDrawText().InSize(12).WithWordWrap().Montserrat(), false, false);
            CardNameControl.Width = 4;
            CardNameControl.Height = 3;
            CardNameControl.Left = .5;
            CardNameControl.Top = .30;

            GuardShieldText = this.AddTextCenter(guardCard.Shield.ToString().AsDrawText().InSize(72).Triforce(), true, false);
            GuardShieldText.Top = 3.8;

            this.AddChild(new ImageControl($"Cards/Guardian/guard1.png".AsmImgRes())
            {
                AbsolutePosition = true,
                CacheAvailable = false,
                Width = this.Width,
                Height = this.Height
            });
        }

        private void AsAbility(AbilityCard abilityCard)
        {
            CardNameControl = this.AddTextCenter(abilityCard.Name.AsDrawText().InSize(12).WithWordWrap().Montserrat(), false, false);
            CardNameControl.Width = 4;
            CardNameControl.Height = 3;
            CardNameControl.Left = .5;
            CardNameControl.Top = .30;

            double l = .65;
            for (int i = 0; i < 5; i++)
            {
                var m = this.AddTextCenter((abilityCard.ResourceMultiplier * (i + 1)).ToString().AsDrawText().InSize(12).Montserrat(), false, false);
                m.Left = l;
                m.Top = 1.2;

                l += .77;
            }

            var abilDesc = abilityCard.GetAllTriggers().FirstOrDefault(a => a.Description != default);
            if (abilDesc != default)
            {
                var descText = this.AddTextCenter(abilDesc.Description.AsDrawText().InSize(10).WithWordWrap().Montserrat(),false,false);
                descText.Width = 3.5;
                descText.Left = .5;
                descText.Top = 3.7;
            }

            this.AddChild(new ImageControl($"Cards/Guardian/ability1.png".AsmImgRes())
            {
                AbsolutePosition = true,
                CacheAvailable = false,
                Width = this.Width,
                Height = this.Height
            });
        }

        private void AsRegion(AreaCard areaCard)
        {
            this.Height = 4.65625;
            this.Width = 7;
            this.Image = $"Cards/Areas/{areaCard.Image}.png".AsmImgRes();

            this.AddChild(new CardNum(() => areaCard.Rounds, 1, 1)
            {
                Left=.4,
                Top=.5
            });

            this.AddChild(new CardNum(() => areaCard.Size, 1, 1)
            {
                Left = 5.75,
                Top = 3.35
            });
        }

        private void UpdateAsGuard(GuardCard guardCard)
        {
            GuardShieldText.Text.SetText(guardCard.Shield.ToString());
        }

        private void UpdateAsAbility(AbilityCard abilityCard)
        {

        }

        private void UpdateAsRegion(AreaCard areaCard)
        {

        }

        private static readonly Dictionary<CardType, string> ImageMap = new Dictionary<CardType, string>()
        {
            { CardType.Resource, "resource" },
            { CardType.Region, "region" },
            { CardType.Ability, "ability" },
            { CardType.Guardian, "guard" }
        };
    }
}