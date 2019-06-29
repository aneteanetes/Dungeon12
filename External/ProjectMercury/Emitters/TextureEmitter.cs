/*  
 Copyright © 2009 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury.Emitters
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Obsolete.
    /// </summary>
#if WINDOWS
    [TypeConverter("ProjectMercury.Design.Emitters.TextureEmitterTypeConverter, ProjectMercury.Design")]
#endif
    [Obsolete("Replaced by MaskEmitter")]
    public class TextureEmitter : Emitter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextureEmitter"/> class.
        /// </summary>
        public TextureEmitter()
        {
            this.Scale = 1f;
            this.PixelOffsets = new Vector2[0];
            this.Threshold = 0.5f;
        }

        private Matrix ScaleMatrix;

        /// <summary>
        /// Gets or sets the scale factor of the texture (in screen space).
        /// </summary>
        public float Scale
        {
            get { return this.ScaleMatrix.M11; }
            set { this.ScaleMatrix = Matrix.CreateScale(value); }
        }

        private Vector2 TextureOrigin;
        
        private Vector2[] PixelOffsets;
        
        private Vector3[] PixelColours;
        
        private Texture2D _texture;

        /// <summary>
        /// Gets or sets the texture used to lookup particle release offsets.
        /// </summary>
        /// <value>The texture.</value>
        public Texture2D Texture
        {
            get { return this._texture; }
            set
            {
                Guard.ArgumentNull("Texture", value);

                if (this.Texture != value)
                {
                    this._texture = value;

                    this.CalculateEmissionPoints();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether particles should assume the colour of the underlying
        /// pixel in the texture
        /// </summary>
        public bool ApplyPixelColours { get; set; }

        /// <summary>
        /// Gets or sets the threshold over which pixels will trigger the release of particles.
        /// </summary>
        public float Threshold;

        /// <summary>
        /// Calculates the emission points.
        /// </summary>
        private void CalculateEmissionPoints()
        {
            if (this.Texture == null)
            {
                this.TextureOrigin = Vector2.Zero;
                
                Array.Resize(ref this.PixelOffsets, 0);
                Array.Resize(ref this.PixelColours, 0);
                
                return;
            }

            this.TextureOrigin = new Vector2(this.Texture.Width / 2, this.Texture.Height / 2);

            List<Vector2> offsets = new List<Vector2>();
            List<Vector3> colours = new List<Vector3>();

            Color[] pixels = new Color[this.Texture.Width * this.Texture.Height];

            this.Texture.GetData(pixels);

            int sourceIndex, destIndex = 0;

            byte minOpacity = Convert.ToByte(this.Threshold * 255f);

            for (int x = 0; x < this.Texture.Width; x++)
            {
                for (int y = 0; y < this.Texture.Height; y++)
                {
                    sourceIndex = this.Texture.Width * y + x;

                    if (pixels[sourceIndex].A >= minOpacity)
                    {
                        offsets.Add(new Vector2
                            { 
                                X = x - this.TextureOrigin.X,
                                Y = y - this.TextureOrigin.Y
                            });
                        
                        colours.Add(pixels[sourceIndex].ToVector3());

                        destIndex++;
                    }
                }
            }

            this.PixelOffsets = offsets.ToArray();
            this.PixelColours = colours.ToArray();
        }

        /// <summary>
        /// Returns an uninitialised deep copy of the Emitter.
        /// </summary>
        /// <returns>A deep copy of the Emitter.</returns>
        public override Emitter DeepCopy()
        {
            Emitter copy = new TextureEmitter
            {
                ApplyPixelColours = this.ApplyPixelColours,
                Scale = this.Scale,
                Texture = this.Texture,
                Threshold = this.Threshold
            };

            base.CopyBaseFields(copy);

            return copy;
        }

        /// <summary>
        /// Generates an offset vector and force vector for a Particle when it is released.
        /// </summary>
        /// <param name="offset">The offset of the Particle from the trigger location.</param>
        /// <param name="force">A unit vector defining the initial force of the Particle.</param>
        protected override void GenerateOffsetAndForce(out Vector2 offset, out Vector2 force)
        {
            int index = RandomHelper.NextInt(this.PixelOffsets.Length);

            offset = this.PixelOffsets[index];

            offset.X *= this.Scale;
            offset.Y *= this.Scale;

            if (this.ApplyPixelColours)
                base.ReleaseColour = this.PixelColours[index];

            force = RandomHelper.NextUnitVector();
        }
    }
}