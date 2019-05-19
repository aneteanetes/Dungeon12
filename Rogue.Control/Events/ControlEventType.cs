namespace Rogue.Control.Events
{
    public enum ControlEventType
    {
        Click,
        ClickRelease,
        MouseWheel,
        Text,
        Focus,
        Key,
        GlobalClick,
        GlobalClickRelease,
        /// <summary>
        /// А вот это ЕБАТЬ какое дорогое событие
        /// </summary>
        MouseMove
    }
}