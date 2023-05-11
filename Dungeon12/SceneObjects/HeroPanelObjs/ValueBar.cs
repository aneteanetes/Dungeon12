using Dungeon;
using Dungeon.Drawing;
using Dungeon.Drawing.SceneObjects;
using Dungeon.Localization;
using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using Dungeon12.ECS.Components;
using Dungeon12.Entities;

namespace Dungeon12.SceneObjects.HeroPanelObjs
{
    internal class ValueBar : SceneControl<Hero>, ITooltiped
    {
        private bool _ishp;

        public ValueBar(Hero component, bool isHp) : base(component)
        {
            _ishp = isHp;
            this.Width=235;
            this.Height=17;

            var textProvide = () => (isHp ? component.Hp.ToString() : component.Endurance.ToString()).AsDrawText().InBold().InSize(13).Calibri();

            this.AddChild(new ValueBarColor(isHp)
            {
                _value=() =>
                {
                    if (isHp)
                        return component.Hp.Current / component.Hp.Max.FlatValue;
                    else
                        return component.Endurance / 100d;
                }
            });

            var label = this.AddTextCenter<BindedTextObject>(textProvide(), true, true);
            label._label=textProvide;
        }

        public string TooltipText => _ishp ? "HPs".Localized() : "EPs".Localized();

        private class ValueBarColor : ImageObject
        {
            public ValueBarColor(bool isHp)
                : base($"UI/char/{(isHp ? "hp" : "exp")}.png")
            {
                this.Top += 0.025;
                this.Left += 0.05;
                this.Height = 17;
            }

            public Func<double> _value;

            public override double Width
            {
                get => ((_value() * 100) / 100)*235;
                set { }
            }

            public override bool CacheAvailable => false;
        }

        private class BindedTextObject : TextObject
        {
            public Func<IDrawText> _label;

            public BindedTextObject(IDrawText component) : base(component)
            {
            }

            public override IDrawText Text
            {
                get => _label==null ? base.Text : _label();
            }
        }
    }
}