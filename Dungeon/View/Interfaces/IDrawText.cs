namespace Dungeon.View.Interfaces
{
    using System.Collections.Generic;
    using Dungeon.Types;

    public interface IDrawText : IDrawContext, IGraph<IDrawText>
    {
        IEnumerable<IDrawText> Data { get; }

        string StringData { get; }

        /// <summary>
        /// Содержит ли внутри ноды
        /// </summary>
        bool IsEmptyInside { get; }
        
        int Length { get; }

        float Size { get; set; }

        float LetterSpacing { get; }

        void Append(IDrawText drawText);

        void Prepend(IDrawText drawText);

        void ReplaceAt(int index, IDrawText drawText);

        void Paint(IDrawColor drawColor, bool recursive=false);

        double Opacity { get; set; }

        string FontName { get; set; }

        string FontPath { get; set; }

        string FontAssembly { get; set; }

        void SetText(string value);

        bool Bold { get; set; }

        bool CenterAlign { get; }
    }
}