namespace ProjectMercury.Controllers
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Defines a simple controller which queues triggers rather than applying them immediately.
    /// The next queued trigger is applied when the particle effect has an active particle count of zero.
    /// </summary>
    public sealed class TriggerQueueController : Controller
    {
        /// <summary>
        /// Gets the queued triggers which need to be applied to the particle effect.
        /// </summary>
        public Queue<Vector2> QueuedTriggers { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueueController"/> class.
        /// </summary>
        public TriggerQueueController()
        {
            this.QueuedTriggers = new Queue<Vector2>();
        }

        /// <summary>
        /// Called by the particle effect when it is triggered.
        /// </summary>
        /// <param name="position">The desired position of the trigger.</param>
        /// <remarks>This method should not be called directly, the ParticleEffect class
        /// will defer control to this method when the controller is assigned.</remarks>
        protected internal override void Trigger(ref Vector2 position)
        {
            this.QueuedTriggers.Enqueue(position);
        }

        /// <summary>
        /// Called by the particle effect when it is updated.
        /// </summary>
        /// <param name="deltaSeconds">Elapsed time in whole and fractional seconds.</param>
        /// <remarks>This method should not be called directly, the ParticleEffect class
        /// will defer control to this method when the controller is assigned.</remarks>
        protected internal override void Update(float deltaSeconds)
        {
            if (base.ParticleEffect != null)
                if (this.QueuedTriggers.Count > 0)
                    if (base.ParticleEffect.ActiveParticlesCount == 0)
                    {
                        Vector2 triggerPosition = this.QueuedTriggers.Dequeue();

                        for (int i = 0; i < this.ParticleEffect.Count; i++)
                            this.ParticleEffect[i].Trigger(ref triggerPosition);
                    }

            base.Update(deltaSeconds);
        }
    }
}