using Dungeon.View.Interfaces;
using System.Linq;

namespace Dungeon.Monogame
{
    public partial class XNADrawClient
    {
        private void UpdateLoop(Microsoft.Xna.Framework.GameTime gameTime)
        {
            var gameTimeLoop = new GameTimeLoop(gameTime.TotalGameTime, gameTime.ElapsedGameTime, gameTime.IsRunningSlowly);

            if (!blockControls)
            {
                UpdateMouseEvents();
                UpdateKeyboardEvents();
            }

            if (scene != default)
            {
                for (int i = 0; i < scene.Objects.Length; i++)
                {
                    var obj = scene.Objects[i];
                    if (DungeonGlobal.ComponentUpdateCompatibility)
                    {
                        if (obj.Updatable && InCamera(obj))
                            UpdateComponent(obj);
                    }
                    else
                    {
                        UpdateComponent(obj, gameTimeLoop);
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
            if(frameEnd)
            {
                sceneObject.Update();
            }

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