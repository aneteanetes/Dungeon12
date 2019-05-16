namespace Rogue.View.Interfaces
{
    using Rogue.Control.Events;
    using Rogue.Control.Keys;
    using Rogue.Control.Pointer;

    public interface ISceneObjectControl : ISceneObject
    {
        void KeyDown(Key key, KeyModifiers modifier, bool hold);

        void KeyUp(Key key, KeyModifiers modifier);

        void Focus();

        void Unfocus();

        void Click(PointerArgs args);

        void ClickRelease(PointerArgs args);

        void MouseWheel(MouseWheelEnum mouseWheel);

        void GlobalClick(PointerArgs args);

        void GlobalClickRelease(PointerArgs args);

        ControlEventType[] CanHandle { get; }

        Key[] KeysHandle { get; }

        bool AllKeysHandle { get; }
    }
}
