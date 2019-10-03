using System;

namespace Rogue.Events
{
    /// <summary>
    /// Событие происходит при смене класса персонажа
    /// </summary>
    public class ClassChangeEvent : IEvent
    {
        public object PlayerSceneObject { get; set; }

        public object GameMap { get; set; }

        public object Character { get; set; }
    }
}
