using Dungeon;
using Dungeon.Types;
using Dungeon.View.Interfaces;
using Dungeon12.Abilities;
using Dungeon12.Abilities.Enums;
using Dungeon12.Abilities.Scaling;
using Dungeon12.Bowman.Effects.Trap;
using Dungeon12.Drawing.SceneObjects.Map;
using Dungeon12.Entities.Alive;
using Dungeon12.Map;
using Dungeon12.Map.Objects;
using System;

namespace Dungeon12.Bowman.Abilities
{
    public class Trap : Ability<Bowman>
    {
        public override AbilityPosition AbilityPosition => AbilityPosition.E;

        public override Cooldown Cooldown { get; } = Cooldown.Make(5000, nameof(Trap));

        public override string Name => "Ловушка";

        public override ScaleRate<Bowman> Scale => new ScaleRate<Bowman>(x => x.AttackDamage * 1.2, x => x.ArmorPenetration * 3.4);

        public override AbilityActionAttribute ActionType => AbilityActionAttribute.Special;

        public override AbilityTargetType TargetType => AbilityTargetType.NonTarget;

        protected override bool CanUse(Bowman @class) => true; //cooldown checking

        protected override void Dispose(GameMap gameMap, Avatar avatar, Bowman @class)
        {
        }

        public override long Value => 20;

        protected override void Use(GameMap gameMap, Avatar avatar, Bowman @class)
        {
            @class.Energy.LeftHand -= 20;
            @class.Energy.RightHand -= 20;
            
            var trap = new TrapObject(@class, new Entities.Alive.Damage()
            {
                Amount = ScaledValue(@class),
                ArmorPenetration=@class.ArmorPenetration,
                Type= DamageType.Kenetic
            }, 2d + (@class.AttackSpeed * 1.2));

            trap.Location = avatar.Location.Copy();

            switch (avatar.VisionDirection)
            {
                case Direction.Up:
                    trap.Location.Y -= 1;
                    break;
                case Direction.Down:
                    trap.Location.Y += 1;
                    break;
                case Direction.UpLeft:
                case Direction.DownLeft:
                case Direction.Left:
                    trap.Location.X -= 1;
                    break;
                case Direction.DownRight:
                case Direction.UpRight:
                case Direction.Right:
                    trap.Location.X += 1;
                    break;
                default:
                    break;
            }

            gameMap.AddMapObject(trap,50,true);
        }
    }
}