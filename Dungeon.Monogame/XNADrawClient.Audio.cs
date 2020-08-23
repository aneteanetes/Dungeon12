namespace Dungeon.Monogame
{
    using Dungeon.Audio;
    using Dungeon.Resources;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Audio;
    using Microsoft.Xna.Framework.Media;
    using System;
    using System.Collections.Generic;
    using System.IO;

    public partial class XNADrawClient : Game, IAudioPlayer
    {
        public void Music(string name, AudioOptions audioOptions = null)
        {
#if DisableSound
            return;
#endif
            var song = LoadSong(name);
            MediaPlayer.Stop();
            MediaPlayer.Play(song);
            MediaPlayer.IsRepeating = true;// audioOptions?.Repeat ?? false;
            //MediaPlayer.Volume = (float)(audioOptions?.Volume ?? .3);
        }

        public void Effect(string effect, AudioOptions audioOptions = null)
        {
#if DisableSound
            return;
#endif
            try
            {
                var sound = LoadSound(effect).CreateInstance();
                var v= MediaPlayer.Volume - .2f;

                if (v < 0) v = 0;

                sound.Volume = v;// (float)(audioOptions?.Volume ?? .2);
                sound.Play();
            }
            catch { }
        }

        private readonly Dictionary<string, SoundEffect> soundEffectsCache = new Dictionary<string, SoundEffect>();

        private SoundEffect LoadSound(string name)
        {
            if (!soundEffectsCache.TryGetValue(name, out var sound))
            {
                var res = ResourceLoader.Load(name);
                sound = SoundEffect.FromStream(res.Stream);
                soundEffectsCache[name] = sound;
                res.OnDispose += () =>
                {
                    sound.Dispose();
                    soundEffectsCache.Remove(name);
                };
            }

            return sound;
        }

        /// <summary>
        /// 
        /// <para>
        /// [Кэшируемый]
        /// </para>
        /// </summary>
        public Song LoadSong(string name)
        {
            Song value = default;

            var song = ResourceLoader.Load(name);

            var tempPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp");
            var tempFilePath = Path.Combine(tempPath, name);

            if (!Directory.Exists(tempPath))
            {
                Directory.CreateDirectory(tempPath);
            }

            if (!File.Exists(tempFilePath))
            {
                using (var fileStream = File.Create(tempFilePath))
                {
                    song.Stream.CopyTo(fileStream);
                }
            }

            value = Song.FromUri(name, new Uri(tempFilePath));

            //song.Dispose += () =>
            //{
            //    value.Dispose();
            //};

            return value;
        }
    }
}