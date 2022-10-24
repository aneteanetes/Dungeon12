using Dungeon;
using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using Dungeon12.SceneObjects.Base;
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

        private List<SceneObject<IDrawText>> texts = new List<SceneObject<IDrawText>>();

        public void AddParam(string name, object value)
        {
            var nam = Text(name);

            texts.Add(_sceneObject.AddChild(GetTextComponent(nam, _left, _top)));
            //ControlBinding?.Invoke(control);
            //_sceneObject.AddControlCenter

            var val = Value(value.ToString());
            var valmeasure = Global.GameClient.MeasureText(val);

            texts.Add(_sceneObject.AddText(val, (_left+_width)-(valmeasure.X+1), _top));

            _top+= Global.GameClient.MeasureText(nam).Y;
        }

        private static TextObjectHint GetTextComponent(IDrawText text, double left, double top)
        {
            var txt = text.ToString().Replace(":", "");
            return new TextObjectHint(text, Global.Strings[txt], Global.Strings[txt], Global.Strings.Description[txt])
            {
                Left= left,
                Top= top
            };
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

        public virtual void Destroy()
        {
            texts.ForEach(t=>t.Destroy());
        }
    }
}
