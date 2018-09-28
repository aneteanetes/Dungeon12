using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
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

        public int Length => this.Flat().Sum(x => x.StringData.Length);

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
        public void ReplaceAt(int index, IDrawText drawText)
        {
            // если мы заменяем что-то, значит пути назад нет, 
            // затираем простое значение, и добавляем внутрь
            // часть себя что бы превратить в составное
            if(!string.IsNullOrEmpty(this.stringData))
            {
                this.InnerText.Add(new DrawText(this.stringData, this.ForegroundColor, this.BackgroundColor));
                this.stringData = null;
            }

            //получаем все отрезки которые затрагивает новый
            var existed = ExistedElements(index, drawText);

            var first = existed.First();

            //если мы вставляем такую же длину как была в то же место - просто заменяем
            if (first.StartIndex == index && first.Text.Length == drawText.Length)
            {
                var inLineIndex = this.InnerText.IndexOf(first.Text);
                this.InnerText.RemoveAt(inLineIndex);
                this.InnerText.Insert(inLineIndex, drawText);
                return;
            }

            //проверяем надо ли отрезать слева
            DrawText newLeft = null;
            if (first.StartIndex != index)
            {
                newLeft = new DrawText(first.Text.StringData.Substring(0, index), first.Text.ForegroundColor, first.Text.BackgroundColor);
            }

            //проверяем надо ли отрезать справа
            if (first.EndIndex > (index + drawText.Length))
            {
                //элемент заканчивается дальше чем отрезок, надо отрезать правую часть
                //отрезаем от конца (нового) вставляемого элемента до конца строки
                var newRight = new DrawText(first.Text.StringData.Substring((index + drawText.Length)), first.Text.ForegroundColor, first.Text.BackgroundColor);

                var indexInListOriginalElement = this.InnerText.IndexOf(first.Text);
                this.InnerText.Remove(first.Text);

                var offset = 0;

                if (newLeft != null)
                {
                    this.InnerText.Insert(indexInListOriginalElement, newLeft);
                    offset += 1;
                }
                this.InnerText.Insert(indexInListOriginalElement + (offset), drawText);
                this.InnerText.Insert(indexInListOriginalElement + (offset * 2), newRight);

                // если мы отрезали справа, значит дальше нас не интересуют элементы, 
                // хотя они могли попасть из-за того что при проверке на существующий
                // мы обязаны смежные элементы включить в коллекцию (хуйзнает зачем)
                return;
            }

            //итак, стадия пиздеца когда у нас возможно есть кусок слева, и ещё хуева тонна претендентов на правую часть, или замену

            // что будем делать:
            // проверять можно ли полностью поглотить кусок
            //          если 
            //               можно: нахуй его из внутренней коллекции, мы знаем индекс первого элемента, просто уёбем его и ничего не потеряем
            //               нельзся: ну, заебись, мы нашли конец, теперь надо проверить на существование конца при обрезке
            //
            // ну всё, мы получили все начальные части, и конечные части, и убрали ненужные
            // теперь так же по условию существования начала и конца запушим всё в ленту
            //
            // и пиздец.
        }

        private class DrawTextPosition
        {
            public int StartIndex { get; set; }

            public IDrawText Text { get; set; }

            public int EndIndex { get => StartIndex + Text.StringData.Length; }
        }


        private IEnumerable<DrawTextPosition> ExistedElements(int index, IDrawText inserted)
        {
            List<DrawTextPosition> elements = new List<DrawTextPosition>();

            var futureElement = new DrawTextPosition
            {
                StartIndex = index,
                Text = inserted
            };

            var carry = 0;
            foreach (var item in this.InnerText)
            {
                carry += item.Length;

                var startElement = carry - item.Length == 0;

                if (startElement && carry > index)
                {
                    elements.Add(new DrawTextPosition
                    {
                        StartIndex = carry - item.Length,
                        Text = item
                    });
                }

                if (carry <= inserted.Length)
                {
                    elements.Add(new DrawTextPosition
                    {
                        StartIndex = carry - item.Length,
                        Text = item
                    });
                }
            }

            return elements.DistinctBy(x => x.Text);
        }

        private (IDrawText segment, int positionInLine) ExistedSegment(int index)
        {
            int currentCharInLine = 0;

            foreach (var item in this.InnerText)
            {
                currentCharInLine += item.StringData.Length;

                if (currentCharInLine > index)
                    return (item, currentCharInLine);


                ////опять же, костыль девелопмент
                //if (currentCharInLine >= index)
                //    return (item, currentCharInLine);

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
