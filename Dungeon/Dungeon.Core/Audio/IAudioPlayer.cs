namespace Dungeon.Audio
{
    using System;

    public interface IAudioPlayer
    {
        void Music(string name, AudioOptions audioOptions = default);
        
        void Effect(string effect, AudioOptions audioOptions = default);
    }
}