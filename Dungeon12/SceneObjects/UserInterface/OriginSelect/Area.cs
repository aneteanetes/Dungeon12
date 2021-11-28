using Dungeon.Control;
using Dungeon.SceneObjects;
using System;

namespace Dungeon12.SceneObjects.UserInterface.OriginSelect
{
    public class Area : EmptySceneControl
    {
        public override bool PerPixelCollision => true;

        public override bool IsBatch => true;

        public Area(Action focus, Action unfocus, string img, Action click)
        {
            this.click = click;
            this.focus = focus;
            this.unfocus = unfocus;
            this.Image = img;
            Width = 1000;
            Height = 629;
        }

        Action focus;
        Action unfocus;
        Action click;

        public override void Focus()
        {
            focus?.Invoke();
        }

        public override void Unfocus()
        {
            unfocus?.Invoke();
        }

        public override void Click(PointerArgs args)
        {
            click?.Invoke();
        }
    }
}