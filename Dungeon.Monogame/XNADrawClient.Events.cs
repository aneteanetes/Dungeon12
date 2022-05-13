using Dungeon.View.Interfaces;
using System.Linq;

namespace Dungeon.Monogame
{
    public partial class XNADrawClient
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

            if (scene != default)
            {
                foreach (var layer in scene.Layers)
                {
                    for (int i = 0; i < layer.Objects.Length; i++)
                    {
                        var obj = layer.Objects[i];
                        if (DungeonGlobal.ComponentUpdateCompatibility)
                        {
                            if (obj.Updatable && (InCamera(obj) || obj.DrawPartInSight || obj.DrawOutOfSight))
                                UpdateComponent(obj);
                        }
                        else
                        {
                            UpdateComponent(obj, gameTimeLoop);
                        }
                    }
                }                
            }
        }

        private void UpdateComponent(ISceneObject sceneObject, GameTimeLoop gameTimeLoop)
        {
            sceneObject.Update(gameTimeLoop);

            for (int i = 0; i < sceneObject.Children.Count; i++)
            {
                var child = sceneObject.Children.ElementAtOrDefault(i);
                if (child != null)
                {
                    UpdateComponent(child, gameTimeLoop);
                }
            }
        }

        private void UpdateComponent(ISceneObject sceneObject)
        {
            //if(frameEnd)
            //{
                sceneObject.Update();
            //}

            for (int i = 0; i < sceneObject.Children.Count; i++)
            {
                var child = sceneObject.Children.ElementAtOrDefault(i);
                if (child != null)
                {
                    if (child.Updatable && InCamera(child))
                        UpdateComponent(child);
                }
            }
        }
    }
}