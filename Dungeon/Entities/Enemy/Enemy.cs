namespace Dungeon.Entities.Enemy
{
    using Dungeon.Entities.Alive;
    using Dungeon.Types;

    public class Enemy : Moveable
    {
        public bool Aggressive { get; set; }

        public string DieImage { get; set; }

        public Rectangle DieImagePosition { get; set; }

        [FlowMethod]
        public void Damage(bool forward)
        {
            if (forward)
            {
                long dmg = GetFlowProperty<long>("Damage");

                HitPoints -= dmg;
                if (HitPoints <= 0)
                {
                    SetFlowProperty("EnemyDied", true);
                }
            }
        }
    }
}