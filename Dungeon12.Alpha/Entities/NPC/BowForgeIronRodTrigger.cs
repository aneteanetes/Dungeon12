using Dungeon.View.Interfaces;
using Dungeon12.Drawing.SceneObjects.Map;
using Dungeon12.Entities;
using Dungeon12.Map;

namespace Dungeon12.Conversations
{
    public class BowForgeIronRodTrigger: TradeItemsTrigger
    {
        protected override IDrawText Trigger(PlayerSceneObject arg1, GameMap arg2, string[] arg3)
        {
            var @base =  base.Trigger(arg1, arg2, arg3);
            var forgeCount = Global.GameState.Character["BowUpdateForge"];

            if (forgeCount == null)
            {
                Global.GameState.Character["BowUpdateForge"] = 1;
            }
            else
            {
                Global.GameState.Character["BowUpdateForge"] = 2;
                Global.GameState.Character["BowUpdateForged"] = true;
            }

            return @base;
        }
    }
}
