using Dungeon.Control;
using Dungeon.GameObjects;
using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace Dungeon12
{
    internal class SceneControl<T> : Dungeon.SceneObjects.SceneControl<T> where T : class
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

    internal class EmptySceneControl : SceneControl<GameComponentEmpty>
    {
        public EmptySceneControl() : base(new GameComponentEmpty())
        {
        }
    }

    internal class TextObjectControl : SceneControl<IDrawText>
    {
        public TextObjectControl(IDrawText component) : base(component)
        {
            Text = component;
        }

        public void SetText(IDrawText text) => Text = text;
    }
}
