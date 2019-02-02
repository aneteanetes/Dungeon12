namespace Rogue.Scenes.Controls
{
    using Rogue.Control.Keys;
    using Rogue.View.Interfaces;

    public abstract class HandleSceneControl : SceneControl, ISceneObjectControl
    {
        public virtual void Click() { }

        public virtual void Focus() { }

        public virtual void Unfocus() { }

        public virtual void KeyDown(Key key, KeyModifiers modifier) { }

        public virtual void KeyUp(Key key, KeyModifiers modifier) { }
    }
}
