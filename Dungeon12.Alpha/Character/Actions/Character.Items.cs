namespace Dungeon12.Classes
{
    using Dungeon12.Items;

    public abstract partial class Character
    {
        private bool PutOnItem(Item @new, Item old)
        {
            Global.AudioPlayer.Effect("invent.wav".AsmSoundRes());
            old?.PutOff(this);
            @new.PutOn(this);
            return true;
        }

        private bool PutOffItem(Item now)
        {
            Global.AudioPlayer.Effect("invent.wav".AsmSoundRes());
            now.PutOff(this);
            return true;
        }
    }
}