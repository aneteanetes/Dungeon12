namespace Dungeon12.SceneObjects.UserInterface
{
    using Dungeon;
    using Dungeon.Drawing.SceneObjects;
    using Dungeon.SceneObjects;
    using Dungeon12.Entities;
    using System;

    public class OriginSelectSceneObject : SceneControl<Hero>
    {
        public OriginSelectSceneObject(Hero component):base(component)
        {
            this.Width = Global.Resolution.Width;
            this.Height = Global.Resolution.Height;

            this.AddChildCenter(new ImageObject("Maps/mainlandback.png".AsmImg())
            {
                Width=1000,
                Height=629
            });

            this.AddChildCenter(new AreaObj());
        }

        private class Area : EmptySceneControl
        {
            public override bool PerPixelCollision => true;

            public override bool IsBatch => true;

            public Area(Action focus, Action unfocus, string img)
            {
                this.focus = focus;
                this.unfocus = unfocus;
                this.Image = img;
                Width = 1000;
                Height = 629;
            }

            Action focus;
            Action unfocus;

            public override void Focus()
            {
                focus?.Invoke();
            }

            public override void Unfocus()
            {
                unfocus?.Invoke();
            }
        }

        private class AreaObj : EmptySceneControl
        {
            private ImageObject border;

            public AreaObj()
            {
                Width = 1000;
                Height = 629;
                this.AddChild(new Area(FocusX, UnfocusX, "Maps/zaa.png".AsmImg()));
                this.AddChild(border = new ImageObject("Maps/zah.png".AsmImg())
                {
                    Width = 1000,
                    Height = 629,
                    Visible = false
                });
            }

            public void FocusX()
            {
                border.Visible = true;
            }

            public void UnfocusX()
            {
                border.Visible = false;
            }
        }
    }
}
