using Dungeon.View.Enums;
using Dungeon.View.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Dungeon.Monogame
{
    internal class ImageMaskManager
    {
        readonly Dictionary<string, Dictionary<float, Texture2D>> MaskCache = new Dictionary<string, Dictionary<float, Texture2D>>();

        private (Texture2D image, SpriteEffects effects) ApplyImageMask(Texture2D image, ISceneObject sceneObject)
        {
            var mask = sceneObject.ImageMask;
            var progress = (float)Math.Round(mask.AmountPercentage * 0.01f, 2);
            SpriteEffects effects = mask.Pattern == MaskPattern.RadialClockwise
                ? SpriteEffects.None
                : SpriteEffects.FlipVertically;

            var uid = sceneObject.Image;

            Texture2D texture;
            void MaskTexture()
            {
                texture = MakeMask(image, progress, mask.Color.Convert(), mask.Opacity);
            }

            if (!mask.CacheAvailable)
            {
                MaskTexture();
            }
            else
            {
                if (MaskCache.TryGetValue(uid, out var progressCache))
                {
                    if (progressCache.TryGetValue(progress, out texture))
                    {
                        return (texture, effects);
                    }
                    else
                    {
                        MaskTexture();
                        progressCache.Add(progress, texture);
                    }
                }
                else
                {
                    MaskTexture();
                    MaskCache.Add(uid, new Dictionary<float, Texture2D>());
                    MaskCache[uid].Add(progress, texture);
                }
            }

            return (texture, effects);
        }

        private Texture2D MakeMask(Texture2D texture2D, float progress, Color overlayColor, float opacity)
        {
            var thisTex = new Texture2D(null, texture2D.Width, texture2D.Height);
            //find the centre pixel
            var centre = new Vector2(MathF.Ceiling(thisTex.Width / 2), MathF.Ceiling(thisTex.Height / 2));
            for (int y = 0; y < thisTex.Height; y++)
            {
                for (int x = 0; x < thisTex.Width; x++)
                {
                    //find the angle between the centre and this pixel (between -180 and 180)
                    var angle = MathHelper.ToDegrees(MathF.Atan2(x - centre.X, y - centre.Y));
                    if (angle < 0)
                    {
                        angle += 360; //change angles to go from 0 to 360
                    }
                    var pixColor = texture2D.GetPixel(x, y);
                    if (angle >= progress * 360.0)
                    {
                        //if the angle is less than the progress angle blend the overlay colour
                        pixColor = Color.Lerp(pixColor, overlayColor, 0.5f);
                        thisTex.SetPixel(x, y, pixColor);
                    }
                    else
                    {
                        thisTex.SetPixel(x, y, pixColor);
                    }
                }
            }
            return thisTex;
        }

        private void CacheImageMask(Texture2D image, ISceneObject sceneObject)
        {
            //var uid = sceneObject.Image;
            //var mask = sceneObject.ImageMask;

            //if (!MaskCache.ContainsKey(uid))
            //{
            //    MaskCache.Add(uid, new Dictionary<float, Texture2D>());
            //}
            //else
            //{
            //    return;
            //}

            //var cache = MaskCache[uid];
            //for (float i = 0f; i < 1; i += 0.01f)
            //{
            //    var v = (float)Math.Round(i,2);
            //    if (!cache.ContainsKey(v))
            //    {
            //        cache.Add(v, MakeMask(image, v, mask.Color.Convert(), mask.Opacity));
            //    }

            //}
        }
    }
}
