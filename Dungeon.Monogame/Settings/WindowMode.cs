namespace Dungeon.Monogame.Settings
{
    public enum WindowMode
    {
        /// <summary>
        /// Разрешение по умолчанию, будет в окне (не borderless)
        /// </summary>
        Windowed,

        /// <summary>
        /// В окне, окно будет расширено до разрешения экрана (для дебага чаще всего)
        /// </summary>
        WindowedScaled,

        /// <summary>
        /// На весь экран, с переключением разрешения монитора (hardware, не ОС)
        /// </summary>
        FullScreenHardware,

        /// <summary>
        /// На весь экран, используется масштабирование, borderless
        /// </summary>
        FullScreenSoftware,
    }
}
