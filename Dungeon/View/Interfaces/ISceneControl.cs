namespace Dungeon.View.Interfaces
{
    using Dungeon.Control;
    using Dungeon.Control.Gamepad;
    using Dungeon.Control.Keys;
    using Dungeon.Control.Pointer;
    using Dungeon.Types;
    using Dungeon.Utils;

    [Hidden]
    public interface ISceneControl : ISceneObject
    {
        bool Controlable { get; }

        void KeyDown(Key key, KeyModifiers modifier, bool hold);

        void KeyUp(Key key, KeyModifiers modifier);

        void StickMoveOnce(Direction direction, GamePadStick stick);

        void StickMove(Direction direction, GamePadStick stick);

        void GamePadButtonsPress(GamePadButton[] btns);

        void GamePadButtonsRelease(GamePadButton[] btns);

        void Focus();

        void Unfocus();

        bool IsInFocus { get; }

        void Click(PointerArgs args);

        void ClickRelease(PointerArgs args);

        void MouseMove(PointerArgs args);

        void GlobalMouseMove(PointerArgs args);

        void MouseWheel(MouseWheelEnum mouseWheel);

        void TextInput(string text);

        void GlobalClick(PointerArgs args);

        void GlobalClickRelease(PointerArgs args);

        ControlEventType[] CanHandle { get; }

        Key[] KeysHandle { get; }

        bool AllKeysHandle { get; }
    }
}
