using Dungeon;
using Dungeon.Control;
using Dungeon.Drawing;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using System;

namespace SidusXII.SceneObjects.ItemSelector
{
    public class ItemBtn : EmptySceneControl
    {
        TextControl text;
        Action onSelect;

        public bool On { get; set; }

        private DrawColor UsualCollor;

        public override bool CacheAvailable => false;

        public GameEnum Value { get; set; }

        public ItemBtn(bool on = false, string text = " ", Action onSelect = default)
        {
            this.onSelect = onSelect;
            On = on;
            UsualCollor = new DrawColor(161, 132, 110);
            AddChild(new OnOffImage(this));
            Width = 340;
            Height = 85;

            this.text = AddTextCenter(text.AsDrawText().InColor(UsualCollor));
        }

        private class OnOffImage : ImageObject
        {
            public override bool CacheAvailable => false;

            private ItemBtn _listBtn;

            public OnOffImage(ItemBtn listBtn) : base("")
            {
                _listBtn = listBtn;
            }

            public override string Image => $"GUI/Parts/selectitem_o{(_listBtn.On ? "n" : "ff")}.png".AsmImg();

            public override double Width => _listBtn.On ? 340 : 330;

            public override double Height => _listBtn.On ? 85 : 83;
        }

        public Action OnClick { get; set; }

        public override void Focus()
        {
            if (!Active)
            {
                Left -= 5;
                Top -= 1;
            }
            onSelect?.Invoke();
            On = true;
            text.Color = DrawColor.White;
        }

        public override void Unfocus()
        {
            if (!Active)
            {
                Left += 5;
                Top += 1;
                On = false;
                text.Color = UsualCollor;
            }
        }

        private bool _active;
        public bool Active
        {
            get => _active;
            set
            {
                _active = value;
                if (value)
                {
                    On = true;
                    text.Color = DrawColor.White;
                }
                else
                {
                    text.Color = UsualCollor;
                }
            }
        }

        public override void Click(PointerArgs args)
        {
            OnClick?.Invoke();
            Active = !Active;
        }
    }
}