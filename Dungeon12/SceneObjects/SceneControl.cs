using Dungeon.Control;
using Dungeon.GameObjects;
using Dungeon.View.Interfaces;

namespace Dungeon12
{
    internal abstract class SceneControl<T> : Dungeon.SceneObjects.SceneControl<T> where T : class
    {
        public SceneControl(T component, bool bindView = true) : base(component, bindView)
        {
        }

        public override void Click(PointerArgs args)
        {
            Debugger.Bind(this);
            base.Click(args);
        }
    }

    internal abstract class EmptySceneControl : SceneControl<GameComponentEmpty>
    {
        public EmptySceneControl() : base(new GameComponentEmpty())
        {
        }
    }

    internal abstract class TextObjectControl : SceneControl<IDrawText>
    {
        public TextObjectControl(IDrawText component) : base(component)
        {
            Text = component;
        }

        public void SetText(IDrawText text) => Text = text;
    }
}
