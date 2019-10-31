/*  
 Copyright © 2009 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury.Emitters
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Defines an Emitter which releases Particles based on a mask array, typically from an image.
    /// </summary>
#if WINDOWS
    [TypeConverter("ProjectMercury.Design.Emitters.MaskEmitterTypeConverter, ProjectMercury.Design")]
#endif
    public sealed class MaskEmitter : Emitter
    {
        private byte[][] _mask;

        /// <summary>
        /// Gets or sets the mask array.
        /// </summary>
        /// <value>The mask array.</value>
        public byte[][] Mask
        {
            get { return this._mask; }
            set
            {
                this._mask = value;

                this.RecalculateMaskHits();

                this.Width = this.Mask.Length;
                this.Height = this.Mask[0].Length;
            }
        }

        private float _threshold;

        /// <summary>
        /// Gets or sets the threshold value above which samples in the mask will be used as release points.
        /// </summary>
        /// <value>The threshold value.</value>
        public float Threshold
        {
            get { return this._threshold; }
            set
            {
                this._threshold = value;

                if (this.Mask != null)
                    this.RecalculateMaskHits();
            }
        }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        public float Width { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>The height.</value>
        public float Height { get; set; }

        /// <summary>
        /// Gets or sets the content path to the mask texture.
        /// </summary>
        /// <value>The mask texture content path.</value>
        public string MaskTextureContentPath { get; set; }

        private Vector2[] MaskHits { get; set; }

        /// <summary>
        /// Recalculates the points on the mask array which will be used as release points.
        /// </summary>
        private void RecalculateMaskHits()
        {
            int width = this.Mask.Length;
            int height = this.Mask[0].Length;

            List<Vector2> maskHits = new List<Vector2>();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    byte maskSample = this.Mask[x][y];

                    if (maskSample / 255f >= this.Threshold)
                    {
                        maskHits.Add(new Vector2
                        {
                            X = (x / (float)width) - 0.5f,
                            Y = (y / (float)height) - 0.5f,
                        });
                    }
                }
            }

            this.MaskHits = maskHits.ToArray();
        }

        /// <summary>
        /// Applies a mask texture to the MaskEmitter.
        /// </summary>
        /// <param name="maskTexture">A texture reference representing the mask.</param>
        /// <remarks>This method will also change the Width and Height properties to match the dimensions
        /// of the mask texture.</remarks>
        public void ApplyMaskTexture(Texture2D maskTexture)
        {
            byte[][] mask = new byte[maskTexture.Width][];

            for (int i = 0; i < maskTexture.Height; i++)
                mask[i] = new byte[maskTexture.Height];

            for (int x = 0; x < maskTexture.Width; x++)
            {
                for (int y = 0; y < maskTexture.Height; y++)
                {
                    Rectangle sourceRectangle = new Rectangle(x, y, 1, 1);

                    Color[] retrievedColor = new Color[1];

                    maskTexture.GetData<Color>(0, sourceRectangle, retrievedColor, 0, 1);

                    Color color = retrievedColor[0];

                    int sum = color.R + color.G + color.B;

                    mask[x][y] = (byte)(sum / 3);
                }
            }

            this.Mask = mask;
        }

        /// <summary>
        /// Loads resources required by the Emitter via a ContentManager.
        /// </summary>
        /// <param name="content">The ContentManager used to load resources.</param>
        /// <exception cref="Microsoft.Xna.Framework.Content.ContentLoadException">Thrown if the asset defined
        /// in the ParticleTextureAssetName property could not be loaded.</exception>
        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);

            if (this.MaskTextureContentPath != null)
            {
                Texture2D maskTexture = content.Load<Texture2D>(this.MaskTextureContentPath);

                this.ApplyMaskTexture(maskTexture);
            }
        }

        /// <summary>
        /// Returns an unitialised deep copy of the Emitter.
        /// </summary>
        /// <returns>A deep copy of the Emitter.</returns>
        public override Emitter DeepCopy()
        {
            MaskEmitter clone = new MaskEmitter
            {
                Mask = (byte[][])this.Mask.Clone(),
                Threshold = this.Threshold,
                Width = this.Width,
                Height = this.Height,
            };

            base.CopyBaseFields(clone);

            return clone;
        }

        /// <summary>
        /// Generates an offset vector and force vector for a Particle when it is released.
        /// </summary>
        /// <param name="offset">The offset of the Particle from the trigger location.</param>
        /// <param name="force">A unit vector defining the initial force of the Particle.</param>
        protected override void GenerateOffsetAndForce(out Vector2 offset, out Vector2 force)
        {
            force = RandomHelper.NextUnitVector();

            offset = RandomHelper.ChooseOne(this.MaskHits);

            offset.X *= this.Width;
            offset.Y *= this.Height;
        }
    }
}