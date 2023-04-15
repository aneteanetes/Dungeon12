﻿namespace Dungeon.View.Interfaces
{
    using Dungeon.Types;
    using System.Collections.Generic;

    public interface IDrawText : IGameComponent, IDrawContext, IGraph<IDrawText>
    {
        bool IsNew { get; set; }

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

        string CompiledFontName { get; set; }

        string FontPath { get; set; }

        string FontAssembly { get; set; }

        int LineSpacing { get; set; }

        void SetText(string value);

        void AddText(string value);

        void AddLine(string value);

        bool Bold { get; set; }

        bool CenterAlign { get; }

        bool WordWrap { get; set; }

        IDrawText Copy();
    }
}