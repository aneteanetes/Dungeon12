using Dungeon12.Map.Editor.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Map.Editor.Field
{
    public class GameFieldHistory
    {
        private EditedGameField _designField;

        public GameFieldHistory(EditedGameField designField) => _designField = designField;

        public FIFOStack<DesignCell> History { get; set; } = new FIFOStack<DesignCell>();

        public void Add(DesignCell designCell) => History.Push(designCell);

        public void Back()
        {
            var dcell = History.Pop();
            if (dcell != default)
            {
                int lvl = dcell.LayerLevel;
                int x = (int)dcell.Left;
                int y = (int)dcell.Top;

                _designField.RemoveChild(dcell);
                _designField.Field[lvl][x][y] = null;
            }
        }

        public void Remove(DesignCell designCell)
        {
            History.Remove(designCell);
        }
    }

    public class FIFOStack<T> : LinkedList<T>
        where T : class
    {
        public T Pop()
        {
            if (this.Count == 0)
                return default;

            T first = this.First?.Value;
            RemoveFirst();
            return first;
        }

        public void Push(T @object)
        {
            AddFirst(@object);
        }
    }
}