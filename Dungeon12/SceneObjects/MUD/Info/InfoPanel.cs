using Dungeon.SceneObjects.Grouping;
using Dungeon12.SceneObjects.MUD.Locations;

namespace Dungeon12.SceneObjects.MUD.Info
{
    internal class InfoPanel : SceneControl<Game>
    {
        public InfoPanel(Game component) : base(component)
        {
            Width = 400;
            Height = 800;

            this.AddBorder();

            //var groupBuilder = new ObjectGroupBuilder<ChipView>()
            //    .Property(x => x.Selected);

            //var left = 15d;

            //foreach (var item in component.Party)
            //{
            //    this.AddChild(groupBuilder.Add(new ChipView(item)
            //    {
            //        Left = left,
            //        Top = 25,
            //        Width=100,
            //        Height=100
            //    }));

            //    left+=90;
            //}

            //var group = groupBuilder.Build();

            //group.Select();
        }
    }
}
