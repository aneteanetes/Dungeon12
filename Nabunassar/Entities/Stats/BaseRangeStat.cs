using Dungeon.Drawing;
using Dungeon.View.Interfaces;
using Nabunassar.SceneObjects.Base;

namespace Nabunassar.Entities.Stats
{
    internal class BaseRangeStat : BaseStat
    {
        protected double BaseValue { get; set; }

        public double MaxValue { get; protected set; }

        public double NowValue { get; protected set; }

        public Value AdditionalMaxValue { get; set; } = new Value();

        protected virtual string TitleSource => Global.Strings[this.GetType().Name];

        public IDrawText AsDrawText(int size=12, bool title = false, DrawColor baseColor = null, DrawColor addColor = null)
        {
            if (baseColor == null)
                baseColor = Global.CommonColorLight;

            if (addColor == null)
                addColor = DrawColor.LightGreen;

            var now = new DrawText("").DefaultTxt(size).InColor(baseColor);

            if (title)
                now.SetText(TitleSource + " : ");

            now.AddText(NowValue.ToString());
            now.AddText("/");
            now.AddText(MaxValue.ToString());

            if (!AdditionalMaxValue.IsEmpty)
                now.Append($"+{AdditionalMaxValue.FlatValue.ToString()}".AsDrawText().DefaultTxt(size).InColor(addColor));


            return now;
        }
    }
}
