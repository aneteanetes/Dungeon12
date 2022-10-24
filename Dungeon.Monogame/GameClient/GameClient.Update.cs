using Dungeon.View.Interfaces;
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
                UpdateKeyboardEvents();
                UpdateGamepadEvents();
            }

            if (Scene != default)
            {
                foreach (var layer in Scene.Layers)
                {
                    for (int i = 0; i < layer.Objects.Length; i++)
                    {
                        var obj = layer.Objects[i];
                        UpdateComponent(obj, gameTimeLoop);
                    }
                }
            }
        }

        private void UpdateComponent(ISceneObject sceneObject, GameTimeLoop gameTimeLoop)
        {
            if (!sceneObject.Updatable)
                return;

            sceneObject.ComponentUpdateChainCall(gameTimeLoop);

            for (int i = 0; i < sceneObject.Children.Count; i++)
            {
                var child = sceneObject.Children.ElementAtOrDefault(i);
                if (child != null)
                {
                    UpdateComponent(child, gameTimeLoop);
                }
            }
        }
    }
}