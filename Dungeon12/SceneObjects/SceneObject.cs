using Dungeon12.Drawing.SceneObjects.UI;
using Dungeon.Scenes.Manager;
using Dungeon.View.Interfaces;

namespace Dungeon12.SceneObjects
{
    public abstract class SceneObject<TComponent> : Dungeon.SceneObjects.SceneObject<TComponent>
       where TComponent : class, IGameComponent
    {
        public SceneObject(TComponent component, bool bindView = true) : base(component, bindView)
        {
            // ЭТО ПИЗДЕЦ КОСТЫЛЬ
            var owner = Global.SceneManager.Preapering;

            if (owner != null)
            {
                if (this is DraggableControl draggableControl)
                { }
                else
                {
                    ZIndex = DragAndDropSceneControls.DraggableLayers;
                }
            }
        }
    }

    public abstract class HandleSceneControl<T> : Dungeon.SceneObjects.SceneControl<T>
        where T : class, IGameComponent
    {
        public HandleSceneControl(T component, bool bindView = true) : base(component, bindView)
        {
            // ЭТО ПИЗДЕЦ КОСТЫЛЬ
            var owner = Global.SceneManager.Preapering;

            if (owner != null)
            {
                if (this is DraggableControl draggableControl)
                { }
                else
                {
                    ZIndex = DragAndDropSceneControls.DraggableLayers;
                }
            }
        }
    }
}
