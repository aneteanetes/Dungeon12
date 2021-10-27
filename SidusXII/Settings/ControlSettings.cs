namespace SidusXII.Settings
{
    public class ControlSettings
    {
        public double MapScrollSpeed_GamePad { get; set; } = 2;

        public double MapScrollSpeed_Mouse { get; set; } = 25;

        public double MapScrollSpeed => Global.GamePadConnected ? MapScrollSpeed_GamePad : MapScrollSpeed_Mouse;
    }
}
