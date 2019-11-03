namespace Dungeon.Control
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
        /// А вот это ЕБАТЬ какое дорогое событие, потому что просчитывается движение ПО этому компоненту
        /// </summary>
        MouseMove,

        /// <summary>
        /// Менее дорогое событие чем <see cref="MouseMove"/> однако частое
        /// </summary>
        GlobalMouseMove
    }
}