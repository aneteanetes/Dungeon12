using Dungeon.SceneObjects;
using Dungeon.Varying;
using Nabunassar.Game;

namespace Nabunassar.SceneObjects.Playing
{
    internal class Information : SceneObject<GameState>
    {
        public Information(GameState component) : base(component)
        {
            this.Width = Variables.Get("InformationWindowW", 400);
            this.Height = Variables.Get("InformationWindowH", 1025);
            this.Left = Variables.Get("InformationWindowL", 1500);
            this.Top = Variables.Get("InformationWindowT", 70); ;

            this.AddBorderBack();
        }
    }
}
