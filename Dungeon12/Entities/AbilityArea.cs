namespace Dungeon12.Entities
{
    public class AbilityArea
    {
        public AbilityArea(bool left = false, bool center = false, bool right = false, bool leftback = false, bool rightback = false)
        {
            Left = left;
            Right = right;
            Center = center;
            LeftBack = leftback;
            RightBack = rightback;
        }

        public bool Left { get; set; }

        public bool Center { get; set; }

        public bool Right { get; set; }

        public bool LeftBack { get; set; }

        public bool RightBack { get; set; }
    }
}