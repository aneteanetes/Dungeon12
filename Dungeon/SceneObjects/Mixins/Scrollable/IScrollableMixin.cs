using Dungeon.Control.Events;
using Dungeon.Control.Pointer;
using Dungeon.SceneObjects.Base;
using Dungeon.Types;
using System;

namespace Dungeon.SceneObjects.Mixins
{
    public interface IScrollableMixin : IMixin
    {
    }

    public static class ScrollableMixin
    {
        public const string ScrollBar = "___scrollbar";
        public static readonly string ScrollBarIndex = $"{ScrollBar}.index";
        public static readonly string ScrollBarMaxDown = $"{ScrollBar}.maxDown";

        public static void Mix_ScrollableMixin(this HandleSceneControl handleSceneControl, Action<int> redrawContent, Point pos, double height)
        {
            var up = Up(handleSceneControl, redrawContent);
            var down = Down(handleSceneControl, redrawContent);

            handleSceneControl.SetMixinValue(ScrollBarMaxDown,0);
            handleSceneControl.SetMixinValue(ScrollBarIndex, 0);

            var scrollbar = new Scrollbar(height,up, down)
            {
                Left = pos.X,
                Top = pos.Y
            };

            handleSceneControl.AddMixin(scrollbar);

            handleSceneControl.AddChild(scrollbar);
            handleSceneControl.SetMixinValue(ScrollBar, scrollbar);

            handleSceneControl.AddHandle(ControlEventType.MouseWheel);
            handleSceneControl.AddDynamicEvent(nameof(handleSceneControl.MouseWheel), MouseWheel(handleSceneControl, up, down));
        }

        private static Action<MouseWheelEnum> MouseWheel(HandleSceneControl handleSceneControl, Func<bool> up, Func<bool> down)
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

        private static Func<bool> Up(HandleSceneControl handleSceneControl, Action<int> redrawContent)
        => () =>
        {
            var maxDown = handleSceneControl.GetMixinValue<int>(ScrollBarMaxDown);
            var index = handleSceneControl.GetMixinValue<int>(ScrollBarIndex);
            var minus = index > 0;
            if (minus)
            {
                handleSceneControl.SetMixinValue(ScrollBarIndex, --index);
                redrawContent(index);
            }

            var scrollbar = handleSceneControl.GetMixinValue<Scrollbar>(ScrollBar);

            scrollbar.Up.Active = !(index == 0);
            scrollbar.Down.Active = maxDown>index;

            return true;
        };

        private static Func<bool> Down(HandleSceneControl handleSceneControl, Action<int> redrawContent)
        => () =>
        {
            var maxDown = handleSceneControl.GetMixinValue<int>(ScrollBarMaxDown);
            var index = handleSceneControl.GetMixinValue<int>(ScrollBarIndex);
            var plus = index < maxDown;
            if (plus)
            {
                handleSceneControl.SetMixinValue(ScrollBarIndex, ++index);
                redrawContent(index);
            }
            var scrollbar = handleSceneControl.GetMixinValue<Scrollbar>(ScrollBar);

            scrollbar.Down.Active = !(index == maxDown);
            scrollbar.Up.Active = index>0;

            return true;
        };
    }
}
