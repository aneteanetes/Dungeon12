using Dungeon.Drawing.SceneObjects.UI;
using Dungeon12.CardGame.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Dungeon;
using Dungeon12.CardGame.Interfaces;
using Dungeon.SceneObjects;
using Dungeon.Drawing.SceneObjects;

namespace Dungeon12.CardGame.SceneObjects
{
    public class CardSceneObject : DraggableControl<CardSceneObject>
    {
        private bool _counter = false;
        private Card card;

        public CardSceneObject(Card card, bool counter=false)
        {
            this.card = card;
            _counter = counter;
            this.Width= 4.65625;
            this.Height = 7;

            if (counter)
            {
                this.Image = "Cards/Guardian/region0.png".AsmImgRes();
            }
            else
            {
                this.Image = $"Cards/Guardian/{ImageMap[card.CardType]}0.png".AsmImgRes();
            }

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
                default:
                    break;
            }
        }

        public override void Update()
        {
            switch (card.CardType)
            {
                case CardType.Guardian:
                    UpdateAsGuard(card.As<GuardCard>());
                    break;
                case CardType.Ability:
                    UpdateAsAbility(card.As<AbilityCard>());
                    break;
                case CardType.Region:
                    UpdateAsRegion(card.As<AreaCard>());
                    break;
                default:
                    break;
            }
        }

        private TextControl CardNameControl;
        private TextControl GuardShieldText;
        private void AsGuard(GuardCard guardCard)
        {
            CardNameControl = this.AddTextCenter(guardCard.Name.AsDrawText().InSize(12).WithWordWrap().Montserrat(), false, false);
            CardNameControl.Left = .5;
            CardNameControl.Top = .30;

            GuardShieldText = this.AddTextCenter(guardCard.Shield.ToString().AsDrawText().InSize(30).Triforce(), true, false);
            GuardShieldText.Top = 5.5;

            this.AddChild(new ImageControl($"Cards/Guardian/guard1.png".AsmImgRes())
            {
                AbsolutePosition = true,
                CacheAvailable = false,
                Width=this.Width,
                Height=this.Height
            });
        }

        private void AsAbility(AbilityCard abilityCard)
        {

        }

        private void AsRegion(AreaCard areaCard)
        {

        }

        private void UpdateAsGuard(GuardCard guardCard)
        {

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
