namespace ProjectMercury.Controllers
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines a collection of controller objects.
    /// </summary>
    public class ControllerCollection : List<Controller>
    {
        /// <summary>
        /// Gets or sets the particle effect which owns the collection.
        /// </summary>
        public ParticleEffect Owner { get; internal set; }

        /// <summary>
        /// Adds a controller to the collection.
        /// </summary>
        /// <param name="controller">The controller to add.</param>
        public new void Add(Controller controller)
        {
            if (base.Contains(controller) == false)
            {
                controller.ParticleEffect = this.Owner;

                base.Add(controller);
            }
        }

        /// <summary>
        /// Removes a controller from the collection.
        /// </summary>
        /// <param name="controller">The controller to remove.</param>
        public new void Remove(Controller controller)
        {
            if (base.Contains(controller))
            {
                controller.ParticleEffect = null;

                base.Remove(controller);
            }
        }
    }
}