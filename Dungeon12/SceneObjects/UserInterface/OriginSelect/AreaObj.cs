using Dungeon;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon12.Entities.Zones;

namespace Dungeon12.SceneObjects.UserInterface.OriginSelect
{
    public class AreaObj : SceneControl<Zone>
    {
        private ImageObject border;
        private AreaDescription description;

        public AreaObj(Zone zone, AreaDescription description) : base(zone)
        {
            this.description = description;

            Width = 1000;
            Height = 629;
            this.AddChild(new Area(FocusX, UnfocusX, $"Maps/{zone.ObjectId}a.png".AsmImg(), ClickArea));

            this.AddChild(border = new ImageObject($"Maps/{zone.ObjectId}h.png".AsmImg())
            {
                Width = 1000,
                Height = 629,
                Visible = false
            });
        }

        private static AreaObj @fixed = null;

        public void ClickArea()
        {
            if (Component.Selectable)
            {
                if (@fixed == null)
                {
                    description.FixOn(Component);
                    border.Visible = true;
                    @fixed = this;
                }
                else if (@fixed == this)
                {
                    description.FixOff(Component);
                    border.Visible = false;
                    @fixed = null;
                }
                else
                {
                    description.FixOff(Component);
                    @fixed.border.Visible = false;
                    @fixed = null;
                    ClickArea();
                }
            }
        }

        public void FocusX()
        {
            description.Refresh(Component);
            if (Component.Selectable)
                border.Visible = true;
        }

        public void UnfocusX()
        {
            description.Reset();
            if (Component.Selectable)
            {
                if (@fixed != this)
                    border.Visible = false;
            }
        }
    }
}