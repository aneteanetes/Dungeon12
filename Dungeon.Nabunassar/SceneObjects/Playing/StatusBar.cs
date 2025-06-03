using Dungeon.SceneObjects;
using Dungeon.Varying;
using Nabunassar.Game;

namespace Nabunassar.SceneObjects.Playing
{
    internal class StatusBar : SceneObject<GameState>
    {
        public StatusBar(GameState component) : base(component)
        {
            this.Width = Variables.Get("StatusBarW", 1150);
            this.Height = Variables.Get("StatusBarH", 50);
            this.Left = Variables.Get("StatusBarL", 210);
            this.Top = Variables.Get("StatusBarT", 10); ;

            this.AddBorderBack();
        }
    }
}
