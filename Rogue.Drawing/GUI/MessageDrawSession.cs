using Rogue.Drawing.Impl;
using Rogue.Settings;
using Rogue.View.Interfaces;

namespace Rogue.Drawing.GUI
{
    public class MessageDrawSession : DrawSession
    {
        public MessageDrawSession()
        {
            this.AutoClear = false;
            this.DrawRegion = new Types.Rectangle
            {
                X = 1f,
                Y = DrawingSize.MapLines + 1.4f + 1.5f+.2f,
                Width = DrawingSize.MapChars + 1.5f,
                Height = 1
            };
        }

        public DrawingSize DrawingSize { get; set; } = new DrawingSize();
        public DrawText Message { get; set; }

        public override IDrawSession Run()
        {
            new InfoWindow()
            {
                Left = 0.25f,
                Top = DrawingSize.MapLines + 1.4f + 1.5f,
                Width = DrawingSize.MapChars + 1.5f,
                Height = 2

            }.Run().Publish();

            this.Message.Size = 25;
            this.Message.LetterSpacing = 13;
            this.Message.Region = this.DrawRegion;
            this.WanderingText.Add(this.Message);

            return base.Run();
        }
    }
}
