using Dungeon.Drawing.SceneObjects.Dialogs.Origin;

namespace Dungeon.SceneObjects.Base
{
    using Dungeon.Control;
    using Dungeon.Control.Pointer;
    using Dungeon.Drawing;
    using Dungeon.Drawing.SceneObjects;
    using Dungeon.SceneObjects.Mixins;
    using System;

    public class Scrollbar : ColoredRectangle, IMixin
    {
        private Action<MouseWheelEnum> _redrawContent;

        private Scrollbar(double height)
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

        public void InitAsMixin(SceneObject owner)
        {
            if (!(owner is HandleSceneControl handleSceneControl))
                return;

            var upBind = UpBinding(handleSceneControl, _redrawContent);
            var downBind = DownBinding(handleSceneControl, _redrawContent);
            AddArrows(upBind, downBind);

            handleSceneControl.AddHandle(ControlEventType.MouseWheel);
            handleSceneControl.AddDynamicEvent(nameof(handleSceneControl.MouseWheel), MouseWheelBinding(upBind, downBind));
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

        private static Func<bool> UpBinding(HandleSceneControl handleSceneControl, Action<MouseWheelEnum> redrawContent)
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

        private static Func<bool> DownBinding(HandleSceneControl handleSceneControl, Action<MouseWheelEnum> redrawContent)
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
