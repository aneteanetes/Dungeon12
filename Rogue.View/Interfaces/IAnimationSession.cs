namespace Rogue.View.Interfaces
{
    using System.Collections.Generic;

    /// <summary>
    /// Анимации отрисовываются в отдельном треде
    /// </summary>
    public interface IAnimationSession
    {
        IAnimationSession Run();
        
        /// <summary>
        /// Коллекция кадров, т.к. под текстурой могут быть другие текстуры
        /// </summary>
        IEnumerable<IEnumerable<IDrawable>> Frames { get; }

        void End();

        void Publish();
    }
}