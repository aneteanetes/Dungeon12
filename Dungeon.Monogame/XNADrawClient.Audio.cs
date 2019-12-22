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
            var song = LoadSong(name);
            MediaPlayer.Stop();
            MediaPlayer.Play(song);
            MediaPlayer.IsRepeating = true;// audioOptions?.Repeat ?? false;
            MediaPlayer.Volume = 0.3f;// (float)(audioOptions?.Volume ?? .3);
        }

        public void Effect(string effect, AudioOptions audioOptions = null)
        {
            try
            {
                var sound = LoadSound(effect).CreateInstance();
                sound.Volume = 0.2f;//(float)(audioOptions?.Volume ?? .1);
                sound.Play();
            }
            catch { }
        }

        private readonly Dictionary<string, SoundEffect> soundEffectsCache = new Dictionary<string, SoundEffect>();

        private SoundEffect LoadSound(string name)
        {
            if (!soundEffectsCache.TryGetValue(name, out var sound))
            {
                //try
                //{
                //    sound = Content.Load<SoundEffect>($@"Audio\Sound\{name}");
                //    soundEffectsCache[name] = sound;
                //}
                //catch (Exception ex)
                //{
                //    try
                //    {
                var res = ResourceLoader.Load(name);
                sound = SoundEffect.FromStream(res.Stream);
                soundEffectsCache[name] = sound;
                res.Dispose += () =>
                {
                    sound.Dispose();
                    soundEffectsCache.Remove(name);
                };
                //    }
                //    catch
                //    {
                //        throw ex;
                //    }
                //}
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
            if (!___LoadSongCache.TryGetValue(name, out var value))
            {
                value = default;
                try
                {
                    value = Content.Load<Song>($@"Audio\Music\{name}");
                }
                catch (Exception ex)
                {
                    try
                    {
                        var song = ResourceLoader.Load(name);

                        var tempPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp");
                        var tempFilePath = Path.Combine(tempPath, name);

                        if(!Directory.Exists(tempPath))
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

                        song.Dispose += () =>
                        {
                            value.Dispose();
                        };
                    }
                    catch 
                    {
                        throw ex;
                    }
                }


                ___LoadSongCache.Add(name, value);
            }

            return value;
        }
        private readonly Dictionary<string, Song> ___LoadSongCache = new Dictionary<string, Song>();
    }
}