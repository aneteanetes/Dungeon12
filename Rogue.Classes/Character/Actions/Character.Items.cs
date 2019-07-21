namespace Rogue.Classes
{
    using Rogue.Items;

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