using Rogue.Abilities;
using Rogue.Abilities.Enums;
using Rogue.Abilities.Scaling;
using Rogue.Map;
using Rogue.Map.Objects;
using Rogue.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rogue.Classes.Bowman.Abilities
{
    public class Dodge : Ability<Bowman>
    {
        public override AbilityPosition AbilityPosition => AbilityPosition.E;

        public override Cooldown Cooldown { get; } = Cooldown.Make(10000, nameof(Dodge));

        public override string Name => "Увернуться";

        public override ScaleRate Scale => ScaleRate.Build(Entites.Enums.Scale.AttackDamage);

        public override AbilityActionAttribute ActionType => AbilityActionAttribute.Special;

        public override AbilityTargetType TargetType => AbilityTargetType.NonTarget;

        protected override bool CanUse(Bowman @class) => true; //cooldown checking

        protected override void Dispose(GameMap gameMap, Avatar avatar, Bowman @class)
        {
        }

        protected override void Use(GameMap gameMap, Avatar avatar, Bowman @class)
        {
            var direction = avatar.VisionDirection.Opposite();
            var move = MoveAvatar(avatar)(direction);

            move(false);

            var plusSpeed = @class.AtackSpeed * 0.5;
            avatar.MovementSpeed += plusSpeed;

            Global.Time.Timer(nameof(Dodge) + nameof(Use))
                .Each(10+(plusSpeed*5))
                .Do(() =>
                {
                    move(true);
                    avatar.MovementSpeed -= plusSpeed;                    
                })
                .Auto();
        }

        private static Func<Direction, Action<bool>> MoveAvatar(Avatar avatar) => dir => remove => avatar.Flow(a => a.MoveStep(true), new { Direction = dir, Remove=remove });
    }
}