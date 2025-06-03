using Dungeon;
using Dungeon.Control;
using Dungeon.ECS;
using Dungeon.View.Interfaces;
using Nabunassar.ECS.Components;

namespace Nabunassar.ECS.Systems
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
                Global.GameClient.SetCursor(sceneObject.Layer.Scene.Resources, $"Cursors/{cursored.Cursor.ToString().ToLowerInvariant()}.png".AsmImg());
            }
        }

        public void ProcessUnfocus(ISceneObject sceneObject) => Global.GameClient.SetCursor(sceneObject.Layer.Scene.Resources, "Cursors/common.png".AsmImg());
    }
}
