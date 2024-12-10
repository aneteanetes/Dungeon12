namespace Dungeon.Audio
{
    public interface IAudioPlayer
    {
        void Music(string name, AudioOptions audioOptions = default);
        
        void Sound(string effect, AudioOptions audioOptions = default);
    }
}