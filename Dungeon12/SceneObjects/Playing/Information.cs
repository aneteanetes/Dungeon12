using Dungeon.SceneObjects;
using Dungeon.Varying;

namespace Dungeon12.SceneObjects.Playing
{
    internal class Information : SceneObject<Dungeon12.Game>
    {
        public Information(Dungeon12.Game component) : base(component)
        {
            this.Width = Variables.Get("InformationWindowW", 400);
            this.Height = Variables.Get("InformationWindowH", 1025);
            this.Left = Variables.Get("InformationWindowL", 1500);
            this.Top = Variables.Get("InformationWindowT", 70); ;

            this.AddBorderBack();
        }
    }
}
