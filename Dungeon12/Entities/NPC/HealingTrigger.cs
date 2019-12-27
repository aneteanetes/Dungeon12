using Dungeon;
using Dungeon.Drawing;
using Dungeon.View.Interfaces;
using Dungeon12.Conversations;
using Dungeon12.Drawing.SceneObjects.Map;
using Dungeon12.Items;
using Dungeon12.Map;
using Force.DeepCloner;
using System.Linq;

namespace Dungeon12.Entities
{
    public class HealingTrigger : ConversationTrigger
    {
        protected override IDrawText Trigger(PlayerSceneObject arg1, GameMap arg2, string[] arg3)
        {
            if (!int.TryParse(arg3.ElementAtOrDefault(0), out var gold))
            {
                return "В данный момент мы не оказываем услуги".AsDrawText();
            }

            var @char = Global.GameState.Character;

            if (@char.Gold<gold)
            {
                return "У вас недостаточно золота!".AsDrawText();
            }

            @char.Gold -= gold;
            @char.HitPoints = @char.MaxHitPoints;

            Global.AudioPlayer.Effect("Dungeon12.Servant.Resources.Audio.Sounds.heal.wav");

            return "Приходите ещё!".AsDrawText();
        }
    }
}
