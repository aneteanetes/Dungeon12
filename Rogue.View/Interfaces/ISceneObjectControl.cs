﻿namespace Rogue.View.Interfaces
{
    using Rogue.Control.Events;
    using Rogue.Control.Keys;

    public interface ISceneObjectControl : ISceneObject
    {
        void KeyDown(Key key, KeyModifiers modifier);

        void KeyUp(Key key, KeyModifiers modifier);

        void Focus();

        void Unfocus();

        void Click();

        ControlEventType[] CanHandle { get; }

        Key[] KeysHandle { get; }
    }
}