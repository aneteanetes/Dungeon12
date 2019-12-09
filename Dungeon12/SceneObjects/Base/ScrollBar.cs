namespace Dungeon12.SceneObjects.Base
{
    using Dungeon.Control;
    using Dungeon.Control.Pointer;
    using Dungeon.Drawing.SceneObjects;
    using Dungeon.GameObjects;
    using Dungeon.SceneObjects;
    using Dungeon.SceneObjects.Mixins;
    using Dungeon.View.Interfaces;
    using Dungeon12.Drawing.SceneObjects.Dialogs.Origin;
    using System;

    public class Scrollbar : ColoredRectangle<EmptyGameComponent>, IMixin
    {
        private Action<MouseWheelEnum> _redrawContent;

        private Scrollbar(double height):base(EmptyGameComponent.Empty)
        {
            Color = ConsoleColor.Black;
            Depth = 1;
            Fill = true;
            Opacity = 0.5;
            Round = 5;

            Height = height;// 13.5;
            Width = 1;
        }

        public Scrollbar(double height, Action <MouseWheelEnum> redrawContent) :this(height)
        {
            _redrawContent = redrawContent;
        }

        public Scrollbar(double height, Func<bool> OnUp, Func<bool> OnDown) : this(height)
        {
            AddArrows(OnUp, OnDown);
        }

        public void ScrollToTop(Func<bool> contentVisible)
        {
            for (int i = 0; i < ScrollIndex; i++)
            {
                Up.Click(default);
                if (contentVisible())
                    break;
            }
        }

        public Arrow Up { get; set; }

        public Arrow Down { get; set; }

        public int ScrollIndex { get; set; }

        public int MaxDownIndex { get; set; }

        public bool CanDown { set => Down.Active = value; }

        public bool CanUp { set => Up.Active = value; }

        public void InitAsMixin(ISceneObject owner)
        {
            if (!(owner is IHandleSceneControl handleSceneControl))
                return;

            var upBind = UpBinding(handleSceneControl, _redrawContent);
            var downBind = DownBinding(handleSceneControl, _redrawContent);
            AddArrows(upBind, downBind);

            handleSceneControl.AddHandle(ControlEventType.MouseWheel);
            handleSceneControl.AddDynamicEvent(nameof(HandleSceneControl<EmptyGameComponent>.MouseWheel), MouseWheelBinding(upBind, downBind));
        }

        private void AddArrows(Func<bool> upBind, Func<bool> downBind)
        {
            Up = new Arrow(upBind, "▲") { Top=-.5 /*Top = -.35*/ };
            Down = new Arrow(downBind, "▼") { Top = Height-1.5 /*Top + Height - 0.75 1.2*/ };

            this.AddChildCenter(Up, vertical: false);
            this.AddChildCenter(Down, vertical: false);
        }

        private static Action<MouseWheelEnum> MouseWheelBinding(Func<bool> up, Func<bool> down)
         => mouseWheelEnum =>
         {
             if (mouseWheelEnum == MouseWheelEnum.Up)
             {
                 up();
             }
             else
             {
                 down();
             }
         };

        private static Func<bool> UpBinding(IHandleSceneControl handleSceneControl, Action<MouseWheelEnum> redrawContent)
        => () =>
        {
            var scrollbar = handleSceneControl.Mixin<Scrollbar>();
            var minus = scrollbar.ScrollIndex > 0;
            if (minus)
            {
                scrollbar.ScrollIndex--;
                redrawContent(MouseWheelEnum.Up);
            }

            scrollbar.CanUp = scrollbar.ScrollIndex > 0;
            scrollbar.CanDown = scrollbar.ScrollIndex <= scrollbar.MaxDownIndex;
            return true;
        };

        private static Func<bool> DownBinding(IHandleSceneControl handleSceneControl, Action<MouseWheelEnum> redrawContent)
        => () =>
        {
            var scrollbar = handleSceneControl.Mixin<Scrollbar>();
            var plus = scrollbar.ScrollIndex < scrollbar.MaxDownIndex;
            if (plus)
            {
                scrollbar.ScrollIndex++;
                redrawContent(MouseWheelEnum.Down);
            }

            scrollbar.CanUp = scrollbar.ScrollIndex > 0;
            scrollbar.CanDown = scrollbar.ScrollIndex < scrollbar.MaxDownIndex;

            return true;
        };
    }
}
