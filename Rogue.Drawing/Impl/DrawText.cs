using System;
using System.Collections.Generic;
using System.Linq;
using Rogue.Settings;
using Rogue.View.Interfaces;

namespace Rogue.Drawing.Impl
{
    public class DrawText : IDrawText
    {
        /// <summary>
        /// ЭТО БЛЯДЬ ЛЕНТА ЁБАНЫЙ ТЫ ДУРАК
        /// </summary>
        private readonly List<IDrawText> InnerText = new List<IDrawText>();

        public DrawText(string value)
        {
            stringData = value;
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

        private string stringData;
        public string StringData
        {
            get
            {
                if (this.InnerText.Count == 0)
                    return this.stringData;
                else
                    return string.Join("", this.InnerText.Select(x => x.StringData));
            }
        }

        public IEnumerable<IDrawText> Data
        {
            get
            {
                if (this.InnerText.Count == 0)
                    return new IDrawText[] { this };

                return this.InnerText;
            }
        }

        public int CharsCount => this.Flat().Sum(x => x.StringData.Length);

        private IDrawColor foregroundColor;

        public IDrawColor BackgroundColor { get; set; }
        public IDrawColor ForegroundColor { get => foregroundColor ?? new DrawColor(ConsoleColor.White); set => this.foregroundColor = value; }

        public IDrawText This => this;

        public IEnumerable<IDrawText> Nodes => this.InnerText;


        public void Append(IDrawText drawText)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// write line с установкой картеки (по сути)
        /// </summary>
        /// <param name="index"></param>
        /// <param name="drawText"></param>
        public void InsertAt(int index, IDrawText drawText)
        {
            // если мы вставляем что-то, значит пути назад нет, 
            // затираем простое значение, и добавляем внутрь
            // часть себя что бы превратить в составное
            if(!string.IsNullOrEmpty(this.stringData))
            {
                this.InnerText.Add(new DrawText(this.stringData, this.ForegroundColor, this.BackgroundColor));
                this.stringData = null;
            }

            // если такого индекса нет, то добавлем новый 
            // пустой отрезок от последнего присутствуюшего
            // индекса, до требуемого индекса
            //if (!IndexExists(index))
            //{
            //    this.FillBeforeIndex(index);
            //    this.InnerText.Add(drawText);
            //    return;
            //}

            // если такой индекс есть
            // разрезаем существующий отрезок
            // сохраняем кусок который будет
            // вставляем в индекс наш отрезок
            // и прикручиваем туда оставшийся кусок

            var (segment, positionInLine) = ExistedSegment(index);
            var segmentStart = positionInLine - segment.StringData.Length;

            // если в нужном индексе начинается новый отрезок 
            // достаточно просто вставить новый туда же и всё
            // благополучно сместится
            if (segmentStart == index && segment.StringData.Length==drawText.StringData.Length)
            {
                this.InnerText.RemoveAt(index);
                this.InnerText.Insert(index, drawText);
                return;
            }

            // а вот тут значит нихуя не помогло и надо разбивать отрезок


            var nextSegment = new DrawText(segment.StringData.Substring(drawText.StringData.Length), segment.ForegroundColor, segment.BackgroundColor);

            var indexInListLine = this.InnerText.IndexOf(segment);
            this.InnerText.RemoveAt(indexInListLine);
            this.InnerText.Insert(indexInListLine, drawText);
            this.InnerText.Insert(indexInListLine + 1, nextSegment);

            //if (this.InnerText.ElementAtOrDefault(index)==null)
            //{
            //    for (int i = -1; i < index; i++)
            //    {
            //        this.InnerText.Add(new DrawText(""));
            //    }
            //}
            //data += drawText.Data;
            //this.InnerText[index] = drawText;
        }

        private (IDrawText segment, int positionInLine) ExistedSegment(int index)
        {
            int currentCharInLine = 0;

            foreach (var item in this.InnerText)
            {
                if (currentCharInLine >= index)
                    return (item, currentCharInLine);

                currentCharInLine += item.StringData.Length;

            }

            throw new Exception("Индекс такой вроде как есть, а вот найти его не получается.");
        }

        private bool IndexExists(int index)
        {
            var line = this.InnerText.Sum(x => x.StringData.Length);
            return line >= index;
        }

        private int LastIndex()
        {
            return this.InnerText.Sum(x => x.StringData.Length);
        }

        private void FillBeforeIndex(int index)
        {
            var last = this.LastIndex();
            var line = new string(Enumerable.Range(0, index - last).Select(x => ' ').ToArray());
            this.InnerText.Add(new DrawText(line));
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
