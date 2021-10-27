using Dungeon;
using Dungeon.Drawing;
using Dungeon.Transactions;
using Dungeon.View.Interfaces;
using Dungeon12.Conversations;
using Dungeon12.Drawing.SceneObjects.Map;
using Dungeon12.Items;
using Dungeon12.Map;
using Dungeon12.Map.Objects;
using Dungeon12.Noone;
using Force.DeepCloner;
using System.Linq;

namespace Dungeon12.Entities
{
    public class AlcoholTrigger : ConversationTrigger
    {
        protected override IDrawText Trigger(PlayerSceneObject arg1, GameMap arg2, string[] arg3)
        {
            if (!int.TryParse(arg3.ElementAtOrDefault(0), out var gold))
            {
                return "У вас должно быть чем платить".AsDrawText();
            }

            var @char = Global.GameState.Character;

            if (@char.Gold<gold)
            {
                return "У вас недостаточно золота!".AsDrawText();
            }

            @char.Gold -= gold;

            var avatar = Global.GameState.PlayerAvatar;
            var buff = new ArmorBuf(@char.Level * 5);

            Global.AudioPlayer.Effect("wav1611.wav".AsmSoundRes());
            avatar.AddState(buff);
            Global.Time.Timer("alcohol")
                .After(10000)
                .Do(() =>
                {
                    avatar.RemoveState(buff);
                })
                .Trigger();

            return "Правда чудесный напиток?".AsDrawText();
        }


        /// <summary>
        /// так то бафы должны действовать и на аватар тоже
        /// </summary>
        private class ArmorBuf : Applicable
        {
            public override string Image => "Images.Abilities.Defstand.buf.png".NoonePath();

            private long value;
            public ArmorBuf(long value) => this.value = value;

            public void Apply(Avatar avatar)
            {
                avatar.Character.AttackDamage += value;
                avatar.Character.AbilityPower += value;
                avatar.Character.Defence -= value;
                avatar.Character.Barrier -= value;
            }

            public void Discard(Avatar avatar)
            {
                avatar.Character.AttackDamage -= value;
                avatar.Character.AbilityPower -= value;
                avatar.Character.Defence += value;
                avatar.Character.Barrier += value;
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
}
