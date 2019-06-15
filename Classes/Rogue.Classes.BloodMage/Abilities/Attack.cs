namespace Rogue.Classes.BloodMage.Abilities
{
    using Rogue.Abilities;
    using Rogue.Abilities.Scaling;
    using Rogue.Classes.BloodMage.Talants;
    using Rogue.Drawing.GUI;
    using Rogue.Map;
    using Rogue.Map.Objects;
    using Rogue.Physics;
    using Rogue.View.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Attack : Ability<BloodMage, BloodStream>
    {
        public override int Position => 0;

        public override string Name => "Атака";

        public override ScaleRate Scale => ScaleRate.Build(Entites.Enums.Scale.AbilityPower, 0.1);
        
        public override bool CastAvailable(BloodMage @class, BloodStream talants)
        {
            //return @class.Actions > 0;
            return true;
        }

        public override string Image => "Rogue.Classes.BloodMage.Images.Abilities.attack.png";

        protected override void InternalCast(BloodMage @class, BloodStream talants)
        {
            throw new NotImplementedException();
        }

        public override bool CastAvailable(Avatar avatar)
        {
            //return avatar.Character.Actions > 0;
            return true;
        }

        public override void Cast(GameMap map, Avatar avatar)
        {
            //avatar.Character.Actions -= 1;
            

            var rangeObject = new MapObject
            {
                Position = new Physics.PhysicalPosition
                {
                    X= avatar.Position.X-((avatar.Size.Width*2.5)/2),
                    Y = avatar.Position.Y - ((avatar.Size.Height * 2.5) / 2)
                },
                Size = avatar.Size
            };

            rangeObject.Size.Height *= 2.5;
            rangeObject.Size.Width *= 2.5;

            var enemies = map.Enemies(rangeObject);
            
            foreach (var enemy in enemies)
            {
                var value = (long)this.Value;

                enemy.Enemy.HitPoints -= value;

                if (enemy.Enemy.HitPoints <= 0)
                {
                    enemy.Die?.Invoke();
                }

                var critical = value > 10;

                this.UseEffects(new List<ISceneObject>()
                {
                    new PopupString(value.ToString()+(critical ? "!" : ""), critical ? ConsoleColor.Red : ConsoleColor.White,enemy.Location,25,critical ? 19 : 17,0.06)
                });
            }
        }

        public override double Value => Rogue.RandomRogue.Next(3,17);
    }
}