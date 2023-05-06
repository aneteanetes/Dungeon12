using Dungeon;
using Dungeon.Control;
using Dungeon.ECS;
using Dungeon.View.Interfaces;
using Dungeon12.ECS.Components;

namespace Dungeon12.ECS.Systems
{
    internal class CursorSystem : ISystem
    {
        public bool IsApplicable(ISceneObject sceneObject)
        {
            return sceneObject is ICursored;
        }

        public void ProcessClick(PointerArgs pointerArgs, ISceneObject sceneObject) { }

        public void ProcessGlobalClickRelease(PointerArgs pointerArgs) { }

        public void ProcessFocus(ISceneObject sceneObject)
        {
            if (sceneObject is ICursored cursored)
            {
                Global.GameClient.SetCursor($"Cursors/{cursored.Cursor.ToString().ToLowerInvariant()}.png".AsmImg());
            }
        }

        public void ProcessUnfocus(ISceneObject sceneObject) => Global.GameClient.SetCursor("Cursors/common.png".AsmImg());
    }
}
