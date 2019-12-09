namespace Dungeon12.Classes
{
    using Dungeon12.Items;

    public abstract partial class Character
    {
        private bool PutOnItem(Item @new, Item old)
        {
            old?.PutOff(this);
            @new.PutOn(this);
            return true;
        }

        private bool PutOffItem(Item now)
        {
            now.PutOff(this);
            return true;
        }
    }
}