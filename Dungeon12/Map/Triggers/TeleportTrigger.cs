using Dungeon;
using Dungeon.Types;
using System.Linq;

namespace Dungeon12.Map.Triggers
{
    public class TeleportTrigger : ITrigger<bool, string[]>
    {
        public bool Trigger(string[] arg1)
        {
            var pos = arg1.ElementAtOrDefault(2) ?? arg1.ElementAtOrDefault(0);

            var destination = Point.FromString(pos);
            if (!destination.IsDefault)
            {
                Global.GameState.Player.StopMovings();
                Global.GameState.Map.SetPlayerLocation(destination);

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}