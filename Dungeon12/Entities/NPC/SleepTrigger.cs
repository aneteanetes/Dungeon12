using Dungeon;
using Dungeon.View.Interfaces;
using Dungeon12.Conversations;
using Dungeon12.Drawing.SceneObjects.Map;
using Dungeon12.Map;
using System.Linq;

namespace Dungeon12.Entities
{
    public class SleepTrigger : ConversationTrigger
    {
        protected override IDrawText Trigger(PlayerSceneObject arg1, GameMap arg2, string[] arg3)
        {
            int.TryParse(arg3.ElementAtOrDefault(0), out var hours);

            var t = Global.Time.Clone();
            t.AddHourse(hours);
            Global.Time.Set(t);

            var @char = Global.GameState.Character;

            var hits = @char.MaxHitPoints;
            var restore = (hits / 10) * hours;
            @char.HitPoints += restore;

            return "Как вам спалось?".AsDrawText();
        }
    }
}
