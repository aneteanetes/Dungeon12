namespace Rogue.Races.Perks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FastMember;
    using Rogue.Data.Perks;
    using Rogue.DataAccess;
    using Rogue.Drawing.Impl;
    using Rogue.Entites.Alive.Character;
    using Rogue.Perks;
    using Rogue.View.Interfaces;

    public class RacePerk : Perk
    {
        private TypeAccessor PlayerAccessor;
        
        private string _icon;
        private string _name;
        private string _description;

        public override string Icon => _icon;

        public override string Name => _name;

        public override IDrawColor BackgroundColor { get; set; }
        public override IDrawColor ForegroundColor { get; set; }

        public override string Description => _description;

        public void Apply(Player player)
        {
            var perk = Database.Entity<ValuePerk>(x => x.Identity == player.Race.ToString())
                .First();

            this._icon = perk.Icon;
            this._name = perk.Name;
            this._description = perk.Description;
            this.ForegroundColor = new DrawColor(perk.Color.R, perk.Color.G, perk.Color.B, perk.Color.A);

            PlayerAccessor = TypeAccessor.Create(player.GetType());

            this.Modify(player, true, perk.Effects);
        }

        public void Descard(Player player)
        {
            var perk = Database.Entity<ValuePerk>(x => x.Identity == player.Race.ToString())
                .First();

            this.Modify(player, false, perk.Effects);
        }

        private void Modify(Player player, bool positive, IEnumerable<Effect> effects)
        {
            foreach (var effect in effects)
            {
                Action<int> modify = null;

                var positiveEffect = effect.Positive;

                if (!positive)
                    positiveEffect = !positiveEffect;

                if (positiveEffect)
                {
                    if (effect.Property == "Resource")
                    {
                        modify = (v) => player.AddToResource(v);
                    }
                    else
                    {
                        modify = (v) => PlayerAccessor[player, effect.Property] = (long)PlayerAccessor[player, effect.Property] + v;
                    }
                }
                else
                {
                    if (effect.Property == "Resource")
                    {
                        modify = (v) => player.RemoveToResource(v);
                    }
                    else
                    {
                        modify = (v) => PlayerAccessor[player, effect.Property] = (long)PlayerAccessor[player, effect.Property] - v;
                    }
                }

                modify(effect.Value);
            }
        }

        protected override void CallApply(dynamic obj)
        {
            this.Apply(obj);
        }

        protected override void CallDiscard(dynamic obj)
        {
            this.Discard(obj);
        }
    }
}