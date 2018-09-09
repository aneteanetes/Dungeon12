using System;
using System.Collections.Generic;
using System.Text;
using Rogue.Drawing.Impl;
using Rogue.Map;
using Rogue.Settings;
using Rogue.View.Interfaces;

namespace Rogue.Drawing
{
    public class RegionDrawSession : DrawSession
    {
        public IView<Biom> BiomView { get; set; }

        public DrawingSize DrawingSize { get; set; }

        private readonly string region;
        public RegionDrawSession(string region)
        {
            this.region = region;
            this.DrawRegion = new Types.Rectangle
            {
                X = 0,
                Y = 0,
                Width = DrawingSize.WindowChars,
                Height = DrawingSize.WindowLines
            };
        }

        public override IDrawSession Run()
        {
            var batch = new List<IDrawText>();

            var list = new List<char>(region);

            var from = 0;
            var to = DrawingSize.WindowChars;

            for (int i = 0; i < DrawingSize.WindowLines; i++)
            {
                var line = new string(list.GetRange(from, to).ToArray());
                batch.Add(new DrawText(line, BiomView.GetView().ForegroundColor));
            }

            this.Batch(0, 0, batch);

            return this;
        }
    }
}