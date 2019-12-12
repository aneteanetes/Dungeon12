using Dungeon;
using Dungeon.Types;
using Dungeon12.SceneObjects;
using System.Linq;

namespace Dungeon12.Map.Triggers
{
    public class ExitDialogTrigger : ITrigger<bool, string[]>
    {
        public bool Trigger(string[] arg1)
        {
            var toastText = arg1.ElementAtOrDefault(0);
            if (toastText != default)
            {
                Toast.Show(toastText);
            }

            var conversationRemove = arg1.ElementAtOrDefault(1);
            if (conversationRemove != default)
            {
                Global.GameState.Character[conversationRemove + "DELETED"] = true;
            }

            return true;
        }
    }
}