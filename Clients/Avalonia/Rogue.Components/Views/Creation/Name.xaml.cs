namespace Rogue.Components.Views.Creation
{
    using Avalonia.Controls;
    using Avalonia.Input;
    using Avalonia.Media;
    using Rogue.Scenes.Menus.Creation;

    internal class Name : Base<PlayerNameScene>
    {
        public Name(PlayerNameScene Scene) : base(Scene)
        {
        }

        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
            //Do work
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if(e.Key== Key.Escape)
            {
                this.Switch<Start>();
            }

            base.OnKeyDown(e);
        }

        protected override void Initialize(DrawingContext context)
        {
        }
    }
}
