using Dungeon.Control;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects.Grouping;
using Dungeon12.ECS.Components;
using Dungeon12.Entities.Objects;

namespace Dungeon12.SceneObjects.MUD.Locations
{
    internal class ChipView : SceneControl<GameObject>, ITooltiped
    {
        public override bool PerPixelCollision => true;

        private ImageObject focus = null;

        public ChipView(GameObject component, bool focusable = true) : base(component)
        {
            if (focusable)
            {
                focus =this.AddChild(new ImageObject("Tiles/empty_invert.png"));
                focus.Visible=false;
            }

            Selected.OnSet+=value => focus.Visible=value;
        }

        public override void Click(PointerArgs args)
        {
            Selected.True();
            base.Click(args);
        }

        public ObjectGroupProperty Selected { get; set; } = new ObjectGroupProperty();

        public override double Width
        {
            get => base.Width;
            set
            {
                if (focus!=null)
                    focus.Width = value;
                base.Width=value;
            }
        }

        public override double Height
        {
            get => base.Height;
            set
            {
                if(focus!=null)
                    focus.Height = value;
                base.Height=value;
            }
        }

        public override string Image
        {
            get => Component.Image;
        }

        public string TooltipText => Component.Name;

        public override void Focus()
        {
            if (focus==null)
                return;

            focus.Visible=true;
            base.Focus();
        }

        public override void Unfocus()
        {
            if (Selected || focus==null)
                return;

            focus.Visible=false;
            base.Unfocus();
        }
    }
}