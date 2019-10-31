namespace ProjectMercury.Modifiers
{
    using System.ComponentModel;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Defines a Modifier which adjusts the hue of a Particles colour over time.
    /// </summary>
#if WINDOWS
    [TypeConverter("ProjectMercury.Design.Modifiers.HueShiftModifierTypeConverter, ProjectMercury.Design")]
#endif
    public sealed class HueShiftModifier : Modifier
    {
        /// <summary>
        /// The transformation matrices which convert from RGB to YIQ and back.
        /// </summary>
        static private Matrix YIQTransformMatrix, RGBTransformMatrix;

        /// <summary>
        /// Initializes the <see cref="HueShiftModifier"/> class.
        /// </summary>
        static HueShiftModifier()
        {
            HueShiftModifier.YIQTransformMatrix = new Matrix(0.299f, 0.587f, 0.114f, 0.000f,
                                                             0.596f, -.274f, -.321f, 0.000f,
                                                             0.211f, -.523f, 0.311f, 0.000f,
                                                             0.000f, 0.000f, 0.000f, 1.000f);

            Matrix.Invert(ref YIQTransformMatrix, out RGBTransformMatrix);
        }

        /// <summary>
        /// The amount to adjust the hue in degrees per second.
        /// </summary>
        public float HueShift;

        /// <summary>
        /// Returns a deep copy of the Modifier implementation.
        /// </summary>
        /// <returns></returns>
        public override Modifier DeepCopy()
        {
            return new HueShiftModifier
            {
                HueShift = this.HueShift
            };
        }

        /// <summary>
        /// Processes the particles.
        /// </summary>
        /// <param name="dt">Elapsed time in whole and fractional seconds.</param>
        /// <param name="particleArray">A pointer to an array of particles.</param>
        /// <param name="count">The number of particles which need to be processed.</param>
        protected internal override unsafe void Process(float dt, Particle* particleArray, int count)
        {
            // Create the transformation matrix...
            float h = ((this.HueShift * dt) * Calculator.Pi) / 180f;

            float u = Calculator.Cos(h);
            float w = Calculator.Sin(h);

            Matrix hueTransform = new Matrix(1f, 0f, 0f, 0f,
                                             0f,  u, -w, 0f,
                                             0f,  w,  u, 0f,
                                             0f, 0f, 0f, 1f);

            for (int i = 0; i < count; i++)
            {
                Particle* particle = (particleArray + i);

                Vector4 colour;

                // Convert the current colour of the particle to YIQ colour space...
                Vector4.Transform(ref particle->Colour, ref HueShiftModifier.YIQTransformMatrix, out colour);

                // Transform the colour in YIQ space...
                Vector4.Transform(ref colour, ref hueTransform, out colour);

                // Convert the colour back to RGB...
                Vector4.Transform(ref colour, ref HueShiftModifier.RGBTransformMatrix, out colour);

                // And apply back to the particle...
                particle->Colour.X = colour.X;
                particle->Colour.Y = colour.Y;
                particle->Colour.Z = colour.Z;
            }
        }
    }
}