using Dungeon.Control.Pointer;
using Dungeon12.Drawing.SceneObjects.Map;
using Dungeon12.Map.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.SceneObjects.Map
{
    public class ClickTriggerSceneObject : ClickActionSceneObject<ClickTrigger>
    {
        public ClickTriggerSceneObject(PlayerSceneObject playerSceneObject, ClickTrigger @object) : base(@object, @object.Name, true)
        {
            this.Image = @object.Image;
        }

        protected override void Action(MouseButton mouseButton)
        {
            @object.Click();
            this.Destroy?.Invoke();
        }
    }
}
