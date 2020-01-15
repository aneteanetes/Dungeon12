using Dungeon.View.Interfaces;
using System.Linq;

namespace Dungeon.Monogame
{
    public partial class XNADrawClient
    {
        private void UpdateLoop()
        {
            UpdateMouseEvents();
            UpdateKeyboardEvents();

            for (int i = 0; i < scene.Objects.Length; i++)
            {
                var obj = scene.Objects[i];
                if (obj.Updatable && InCamera(obj))
                    UpdateComponent(obj);
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