using Dungeon.SceneObjects;
using Dungeon.Varying;

namespace Nabunassar.SceneObjects.Playing
{
    internal class Information : SceneObject<Nabunassar.Game>
    {
        public Information(Nabunassar.Game component) : base(component)
        {
            this.Width = Variables.Get("InformationWindowW", 400);
            this.Height = Variables.Get("InformationWindowH", 1025);
            this.Left = Variables.Get("InformationWindowL", 1500);
            this.Top = Variables.Get("InformationWindowT", 70); ;

            this.AddBorderBack();
        }
    }
}
