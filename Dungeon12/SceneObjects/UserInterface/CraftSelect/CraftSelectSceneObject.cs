using Dungeon;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon12.Entities;
using Dungeon12.Entities.Enums;
using Dungeon12.Functions.ObjectFunctions;

namespace Dungeon12.SceneObjects.UserInterface.CraftSelect
{
    public class CraftSelectSceneObject : SceneControl<Hero>
    {
        public CraftSelectSceneObject(Hero component) : base(component)
        {
            Global.Freezer.Freeze(this);
            this.Width = 620;
            this.Height = 800;
            this.AddChild(new ImageObject("Other/pngegg.png".AsmImg()));

            var text = this.AddTextCenter("Укажите ваше образование:".AsDrawText().Gabriela().InSize(24).InColor(System.ConsoleColor.Black), vertical: false);
            text.Top = 166;

            typeof(Crafts).All<Crafts>().ForEach(c =>
            {
                this.AddChild(new CraftBadge(component, c));
            });

            this.AddChild(new CraftOptButton(true)
            {
                Left = 289,
                Top = 38,
                OnClick = Close
            });

            this.AddChild(new CraftOptButton()
            {
                Left = 295,
                Top = 618,
                OnClick = Select
            });
        }

        private void Close()
        {
            this.Destroy?.Invoke();
            Global.Freezer.Unfreeze();
        }

        private void Select()
        {
            if (Component.Profession != null)
            {
                Global.Game.Location.Polygon.P3.Load(new Entities.Map.Polygon
                {
                    Name = "Место службы",
                    Icon = "fractscroll.png",
                    Function = nameof(SelectFractionFunction)
                });
            }

            Close();
        }
    }
}
