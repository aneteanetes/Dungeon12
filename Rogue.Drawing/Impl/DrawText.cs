using System;
using System.Collections.Generic;
using System.Linq;
using Rogue.Settings;
using Rogue.View.Interfaces;

namespace Rogue.Drawing.Impl
{
    public class DrawText : IDrawText
    {
        private readonly List<IDrawText> InnerText = new List<IDrawText>();

        public DrawText(string value)
        {
            this.data = value;
        }

        public DrawText(string value, DrawColor foregroundColor, DrawColor backgroundColor = null) : this(value)
        {
            this.BackgroundColor = backgroundColor;
            this.ForegroundColor = foregroundColor;
        }

        public DrawText(string value, IDrawColor foregroundColor, IDrawColor backgroundColor = null) : this(value)
        {
            this.BackgroundColor = backgroundColor;
            this.ForegroundColor = foregroundColor;
        }

        private string data = string.Empty;
        public string Data => data;//string.Join(Environment.NewLine, InnerText.Select(x => x.Data));

        public int CharsCount => this.Flat().Sum(x => x.Data.Length);

        public IDrawColor BackgroundColor { get; set; }
        public IDrawColor ForegroundColor { get; set; }

        public IDrawText This => this;

        public IEnumerable<IDrawText> Nodes => this.InnerText;


        public void Append(IDrawText drawText)
        {
            throw new NotImplementedException();
        }

        public void InsertAt(int index, IDrawText drawText)
        {
            if(this.InnerText.ElementAtOrDefault(index)==null)
            {
                for (int i = -1; i < index; i++)
                {
                    this.InnerText.Add(new DrawText(""));
                }
            }
            data += drawText.Data;
            this.InnerText[index] = drawText;
        }

        public void ReplaceAt(int index, IDrawText drawText)
        {
            throw new NotImplementedException();
        }

        public void Prepend(IDrawText drawText)
        {
            throw new NotImplementedException();
        }

        public static implicit operator DrawText(string value) => new DrawText(value);

        /// <summary>
        /// from container
        /// </summary>
        private static DrawingSize DrawingSize = null;

        /// <summary>
        /// Creates new drawing text with ' ' chars of full length
        /// </summary>
        /// <param name="length">if not set, then - <see cref="Rogue.Settings.DrawingSize.WindowChars"/> </param>
        /// <returns></returns>
        public static DrawText Empty(int length = 0)
        {
            if (length == 0)
                length = DrawingSize.WindowChars;

            return new DrawText(new string(Enumerable.Range(0, length).Select(x => ' ').ToArray()));

        }
    }
}
