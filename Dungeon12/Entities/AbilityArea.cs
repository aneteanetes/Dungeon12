namespace Dungeon12.Entities
{
    internal class AbilityArea
    {
        public AbilityArea(bool left = false, bool center = false, bool right = false, bool leftback = false, bool rightback = false, bool single = true, bool all=false, bool self=false,bool friendlytarget=false)
        {
            if (all)
            {
                All = true;
                return;
            }

            Left = left;
            Right = right;
            Center = center;
            LeftBack = leftback;
            RightBack = rightback;
            Single = single;
            Self = self;
        }

        public bool Self { get; set; }

        public bool All { get; set; }

        public bool Single { get; set; }

        public bool Left { get; set; }

        public bool Center { get; set; }

        public bool Right { get; set; }

        public bool LeftBack { get; set; }

        public bool RightBack { get; set; }
    }
}