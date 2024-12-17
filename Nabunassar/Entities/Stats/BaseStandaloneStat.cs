using Dungeon.Drawing;
using Dungeon.View.Interfaces;

namespace Nabunassar.Entities.Stats
{
    internal class BaseStandaloneStat : PersonaBinded
    {
        public double Value { get; protected set; }

        public Value AdditionalValue { get; protected set; } = new Value();

        protected virtual string TitleSource => this.GetType().Name;

        public IDrawText AsDrawText(int size = 12, bool title = false, DrawColor baseColor = null, DrawColor addColor = null)
        {
            if (baseColor == null)
                baseColor = Global.CommonColorLight;

            if (addColor == null)
                addColor = DrawColor.LightGreen;

            var now = new DrawText("").DefaultTxt(size).InColor(baseColor);

            if (title)
                now.SetText(TitleSource + " : ");

            now.AddText(Value.ToString());

            if (!AdditionalValue.IsEmpty)
                now.Append($"+{AdditionalValue.FlatValue.ToString()}".AsDrawText().DefaultTxt(size).InColor(addColor));


            return now;
        }
    }
}
