using Rogue.Entites.Animations;

namespace Rogue.Classes.BloodMage.Animations.Moving
{
    public class BaseMove : AnimationMap
    {
        public BaseMove()
        {
            this.Size = new Types.Point
            {
                X = 32,
                Y = 32
            };

            this.TileSet = "Rogue.Classes.BloodMage.Images.Dolls.Character.png";
        }
    }
}