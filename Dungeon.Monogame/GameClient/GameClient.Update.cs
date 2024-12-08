using Dungeon.View.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon.Monogame
{
    public partial class GameClient
    {
        private void UpdateLoop(Microsoft.Xna.Framework.GameTime gameTime)
        {
            var gameTimeLoop = new GameTimeLoop(gameTime.TotalGameTime, gameTime.ElapsedGameTime, gameTime.IsRunningSlowly);

            if (!blockControls && !SceneManager.IsSwitching)
            {
                if (!DungeonGlobal.GamePadConnected)
                    UpdateMouseEvents();
                UpdateKeyboardEvents(gameTime);
                UpdateGamepadEvents();
            }

            if (Scene != default)
            {
                foreach (var layer in SceneLayers)
                {
                    var controls = layer.Key.ActiveObjectControls;
                    for (int i = 0; i < controls.Count; i++)
                    {
                        var control = controls[i];
                        UpdateComponent(control, gameTimeLoop);
                    }
                }
                Scene.Update(gameTimeLoop);
            }
        }

        private void UpdateComponent(ISceneObject sceneObject, GameTimeLoop gameTimeLoop)
        {
            if (!sceneObject.Updatable)
                return;

            sceneObject.ComponentUpdateChainCall(gameTimeLoop);
        }
    }
}