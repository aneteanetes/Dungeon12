using Dungeon.Control;
using Dungeon.Drawing;
using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using Dungeon12.SceneObjects.UserInterface.Common;
using System;

namespace Dungeon12.SceneObjects
{
    public static class PopUpExtensions
    {
        public static PopupText Popup<T>(this SceneObject<T> sceneObject, PointerArgs pointerArgs, IDrawText text, double speed=.5, double seconds=.7)
            where T : class
        {
            var popup = new PopupText(text, pointerArgs.AsPoint, speed: speed)
            {
                Time = TimeSpan.FromSeconds(0.7),
            };
            sceneObject.Layer.AddObject(popup);
            return popup;
        }
    }
}
