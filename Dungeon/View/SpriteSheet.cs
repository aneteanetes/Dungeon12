using Dungeon.Types;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon.View
{
    public class SpriteSheet
    {
        public string Image { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public Dictionary<string, Animation> Animations { get; set; } = new Dictionary<string, Animation>();

        public Dot DefaultFramePosition { get; set; }

        public static SpriteSheet Load(string json)
        {
            var jobj = JObject.Parse(json);

            var sheet = new SpriteSheet()
            {
                Image = jobj["textureAtlas"]["texture"].ToString(),
                Width = jobj["textureAtlas"]["regionWidth"].ToObject<int>(),
                Height = jobj["textureAtlas"]["regionHeight"].ToObject<int>(),
            };

            jobj["cycles"].Children().ForEach(c =>
            {
                var anim = new Animation()
                {
                    Name = (c as JProperty).Name,
                    Frames = new List<Dot>(),
                    Time = TimeSpan.FromSeconds(.5)
                };

                var frames = c.Last["frames"].ToObject<int[]>();

                var y = frames[0] / frames.Length;

                for (int i = 0; i < frames.Length; i++)
                {
                    anim.Frames.Add(new Dot(i * sheet.Width, y * sheet.Height));
                }

                sheet.Animations.Add(anim.Name, anim);
            });

            return sheet;
        }
    }
}