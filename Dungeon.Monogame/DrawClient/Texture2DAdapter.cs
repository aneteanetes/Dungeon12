using Dungeon.View.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Dungeon.Monogame
{
    internal class Texture2DAdapter : ITexture
    {
        Texture2D _texture;
        ISceneObject _sceneObject;
        Color[] data;

        private double actualWidth;
        private double actualHeight;

        public Texture2DAdapter(Texture2D texture, ISceneObject sceneObject)
        {
            _sceneObject = sceneObject;
            _texture = texture;
            _texture.Disposing += (s, e) =>
            {
                _texture = null;
                data = null;
            };
            data = new Color[texture.Width * texture.Height];
            _texture.GetData(data);

            actualWidth = texture.Width;
            actualHeight = texture.Height;
        }

        public object Texture => _texture;

        public bool Contains(Types.Dot point, Types.Dot actualSize)
        {
            if (actualSize.X != actualWidth || actualSize.Y != actualHeight)
            {
                //return ScaleTexture(actualSize);

                if (actualSize.X > _texture.Width)
                {
                    point.X /= actualSize.X / _texture.Width;
                }
                else
                {
                    point.X *= _texture.Width / actualSize.X;
                }

                if (actualSize.Y > _texture.Height)
                {
                    point.Y /= actualSize.Y / _texture.Height;
                }
                else
                {
                    point.Y *= _texture.Height / actualSize.Y;
                }
            }

            try
            {
                int idx = (int)Math.Floor(point.X + (Math.Round(point.Y) * actualWidth));
                return data[idx].A != 0;
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
        }
    }
}
