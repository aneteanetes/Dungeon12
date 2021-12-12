using Dungeon;
using Dungeon.SceneObjects;
using Dungeon12.Attributes;
using Dungeon12.Entities;
using Dungeon12.Entities.Enums;

namespace Dungeon12.SceneObjects.UserInterface.SpecSelect
{
    public class SpecSelectSceneObject : SceneControl<Hero>
    {
        public SpecSelectSceneObject(Hero component) : base(component)
        {
            Global.Freezer.Freeze(this);
            this.Destroy += () => Global.Freezer.Unfreeze();

            specs = this;

            var availableSpecs = Fraction.Vanguard.ToValue<AvailableSpecsAttribute, Spec[]>();

            this.Width = Global.Resolution.Width;
            this.Height = Global.Resolution.Height;

            var plate = this.AddChildCenter(new Plate(availableSpecs));
        }

        private static SpecSelectSceneObject specs;
        public static void Close()
        {
            specs.Destroy();
        }

        private class Plate : EmptySceneControl
        {
            public Plate(Spec[] specs)
            {
                this.Width = specs.Length * SpecCard.WidthCard;
                this.Width += specs.Length * SpecCard.CardSpace;
                this.Height = SpecCard.HeightCard;

                specs.ForEach((s, i) =>
                {
                    var card = this.AddChildCenter(new SpecCard(s), false);
                    card.Left = i * (SpecCard.WidthCard + SpecCard.CardSpace);
                });
            }
        }
    }
}