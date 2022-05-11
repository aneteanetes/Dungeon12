using Dungeon;
using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using System.Collections.Generic;

namespace Dungeon12.SceneObjects.Stats
{
    internal class ParamBinder
    {
        double _left;
        double _width;
        double _top;

        StatsWindow _sceneObject;

        public ParamBinder(StatsWindow sceneObject, double left, double width, double top)
        {
            _left = left;
            _width = width;
            _top = top;
            _sceneObject = sceneObject;
        }

        private List<TextObject> texts = new List<TextObject>();

        public void AddParam(string name, object value)
        {
            var nam = Text(name);

            texts.Add(_sceneObject.AddText(nam, _left, _top));

            var val = Value(value.ToString());
            var valmeasure = Global.DrawClient.MeasureText(val);

            texts.Add(_sceneObject.AddText(val, (_left+_width)-(valmeasure.X+1), _top));

            _top+= Global.DrawClient.MeasureText(nam).Y;
        }

        public void AddEmpty()
        {
            _top+=16;
        }

        private IDrawText Text(string name)
        {
            return (name+":").SegoeUIBold().InSize(10).InColor(Global.CommonColorLight);
        }

        private IDrawText Value(string name)
        {
            return (name).SegoeUIBold().InSize(10).InColor(Global.CommonColorLight);
        }

        public void Destroy()
        {
            texts.ForEach(t=>t.Destroy());
        }
    }
}
