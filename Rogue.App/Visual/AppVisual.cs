namespace Rogue.App.Visual
{
    using Avalonia;
    using Avalonia.Input;
    using Avalonia.Media;
    using Avalonia.Media.Imaging;
    using Avalonia.Media.Immutable;
    using Avalonia.Visuals.Media.Imaging;
    using Avalonia.VisualTree;
    using Rogue.Resources;
    using Rogue.View.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    public class AppVisual : Avalonia.Controls.Control, IRenderTimeCriticalVisual, IDrawClient
    {
        public bool HasRenderTimeCriticalContent => true;
        public bool ThreadSafeHasNewFrame => true;
        private bool _newFrame = true;

        private readonly Rect DrawingDisplay = new Rect(0, 0, 1280, 720);

        #region frameSettings

        private int _frame;
        private TimeSpan _lastFps;
        private int _lastFpsFrame;
        private double _fps;
        Stopwatch _st = Stopwatch.StartNew();
        private Typeface _typeface = Typeface.Default;

        public static bool frameInfo = false;

        #endregion

        private Bitmap Display = null;

        private Bitmap Buffer = null;

        public void ThreadSafeRender(DrawingContext context, Size logicalSize, double scaling)
        {            
            FillBuffer(context);

            context.DrawImage(
                Display, 
                1, 
                DrawingDisplay, 
                DrawingDisplay, 
                BitmapInterpolationMode.HighQuality);

            DrawFrameInfo(context);
        }
        
        private void DrawFrameInfo(DrawingContext drawingContext)
        {
            if (frameInfo)
            {
                var nowTs = _st.Elapsed;
                var now = DateTime.Now;
                var fpsTimeDiff = (nowTs - _lastFps).TotalSeconds;
                if (fpsTimeDiff > 1)
                {
                    _fps = (_frame - _lastFpsFrame) / fpsTimeDiff;
                    _lastFpsFrame = _frame;
                    _lastFps = nowTs;
                }

                var text = $"Frame: {_frame}\nFPS: {_fps}\nNow: {now}";
                //text += $"\nTransform{context.CurrentTransform}";
                //text += $"\nContainer Transform{context.CurrentContainerTransform}";
                var fmt = new FormattedText()
                {
                    Text = text,
                    Typeface = _typeface
                };
                var back = new ImmutableSolidColorBrush(Colors.LightGray);
                var textBrush = new ImmutableSolidColorBrush(Colors.White);
                drawingContext.DrawText(textBrush, new Point(5, 5), fmt);
            }

            _frame++;
        }

        private void FillBuffer(DrawingContext drawingContext)
        {
            if (Display == null)
            {
                Splash(drawingContext);
            }
            else
            {
                _newFrame = false;
            }
        }

        private void Splash(DrawingContext context)
        {
            var splash = ResourceLoader.Load("Rogue.Resources.Images.d12.png");
            Display = new Bitmap(splash);
        }

        public void Draw(IEnumerable<IDrawSession> drawSessions)
        {
            throw new NotImplementedException();
        }

        public void Animate(IAnimationSession animationSession)
        {
            throw new NotImplementedException();
        }
    }
}