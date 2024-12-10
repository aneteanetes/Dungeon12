using Dungeon.Control;
using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using Nabunassar.SceneObjects.UserInterface.Common;
using System;

namespace Nabunassar.SceneObjects
{
    public static class PopUpExtensions
    {
        internal static PopupText Popup<T>(this SceneObject<T> sceneObject, PointerArgs pointerArgs, IDrawText text, double speed=.5, double seconds=.7)
            where T : class
        {
            var popup = new PopupText(text, pointerArgs.AsDot(), speed: speed)
            {
                Time = TimeSpan.FromSeconds(0.7),
            };
            sceneObject.Layer.AddObject(popup);
            return popup;
        }
    }
}
