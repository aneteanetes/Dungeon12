using Dungeon.View.Interfaces;
using Dungeon12.Entities.Alive;
using Dungeon12.Map;
using Dungeon12.Map.Objects;
using System;

namespace Dungeon12.Bowman.Effects.Trap
{
    public class TrapObject : MapObject
    {
        private Damage damage;
        private Interactable hostes;

        public TrapObject(Interactable hostes, Damage dmg, double secondsAlive=2)
        {
            this.hostes = hostes;
            damage = dmg;

            Global.Time.Timer()
                .After(TimeSpan.FromSeconds(secondsAlive).TotalMilliseconds)
                .Do(() => this.Destroy?.Invoke())
                .Trigger();
        }

        public override bool Interactable => true;

        public override ISceneObject Visual() => new TrapSceneObject(this);

        public void Interact(NPCMap npc)
        {
            if (npc.IsEnemy)
            {
                npc.Entity.Damage(hostes, damage);
                this.Destroy?.Invoke();
            }
        }

        protected override void CallInteract(dynamic obj) => this.Interact(obj);
    }
}