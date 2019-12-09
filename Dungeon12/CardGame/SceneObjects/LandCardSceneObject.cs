using Dungeon12.Drawing.SceneObjects.Map;

namespace Dungeon12.CardGame.SceneObjects
{
    public class LandCardSceneObject : TooltipedSceneObject<Engine.CardGame>
    {
        CardSceneObject areaCard = default;

        public LandCardSceneObject(Engine.CardGame component) : base(component, "", false)
        {
            component.OnafterTurn += OnTurn;
            this.Height = 4.65625;
            this.Width = 7;
        }

        void OnTurn()
        {
            if (Component.AreaEnded)
            {
                areaCard?.Destroy();
            }

            if (areaCard != default)
            {
                areaCard.Destroy();
            }

            areaCard = new CardSceneObject(Component.CurrentArea,default)
            {
                DisableDrag = true
            };
            this.TooltipText = Component.CurrentArea.Name;
            this.AddChild(areaCard);
        }
    }
}