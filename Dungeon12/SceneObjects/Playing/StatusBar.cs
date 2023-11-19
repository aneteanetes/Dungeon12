using Dungeon.SceneObjects;
using Dungeon.Varying;

namespace Dungeon12.SceneObjects.Playing
{
    internal class StatusBar : SceneObject<Dungeon12.Game>
    {
        public StatusBar(Dungeon12.Game component) : base(component)
        {
            this.Width = Variables.Get("StatusBarW", 1150);
            this.Height = Variables.Get("StatusBarH", 50);
            this.Left = Variables.Get("StatusBarL", 210);
            this.Top = Variables.Get("StatusBarT", 10); ;

            this.AddBorderBack();
        }
    }
}
