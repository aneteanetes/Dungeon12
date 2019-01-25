namespace Rogue.Components.Views
{
    using Avalonia;
    using Avalonia.Controls;
    using Avalonia.Input;
    using Avalonia.Markup.Xaml;
    using Avalonia.Media;
    using Rogue.Scenes;
    using Rogue.Scenes.Scenes;

    public abstract class Base<T> : Canvas
        where T : GameScene
    {
        public T Scene { get; private set; }

        public Base(T Scene)
        {
            this.Scene = Scene;
            AvaloniaXamlLoader.Load(this);

            SceneManager.OnVisualDestroy = () => SceneManager.keyDown -= OnKeyDownObj;

            SceneManager.keyDown += OnKeyDownObj;
        }

        protected void Switch<T1>()
        {
            Scene.Switch<T1>();
        }

        private void OnKeyDownObj(object e)
        {
            OnKeyDown((KeyEventArgs)e);
        }

        protected override void ArrangeCore(Rect finalRect)
        {
            //var rect = new Rect(new Point(640, 0), finalRect.Size);
            base.ArrangeCore(finalRect);
        }

        bool rendered = false;

        protected abstract void Initialize(DrawingContext context);

        public override void Render(DrawingContext context)
        {
            if (!rendered)
            {
                Initialize(context);
                rendered = true;
            }

            base.Render(context);
        }
    }
}
