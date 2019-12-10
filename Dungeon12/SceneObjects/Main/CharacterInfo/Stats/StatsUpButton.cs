using Dungeon12.Classes;
using Dungeon12.SceneObjects;
using Dungeon.SceneObjects;
using System;
using System.Collections.Generic;
using System.Text;
using Dungeon;
using Dungeon.Control;

namespace Dungeon12.SceneObjects.Main.CharacterInfo.Stats
{
    public class StatsUpButton : HandleSceneControl<Character>
    {
        public override bool AbsolutePosition => true;

        public override bool CacheAvailable => false;

        private Action<Character> _up;

        protected override ControlEventType[] Handles { get; } = new ControlEventType[]
        {
            ControlEventType.Click,
            ControlEventType.ClickRelease,
            ControlEventType.GlobalClickRelease,
            ControlEventType.Focus,
        };

        private Character character;

        public StatsUpButton(Character component,Action<Character> up, string text="+") : base(component,false)
        {
            character = component;
            this.Width = .5;
            this.Height = .5;
            _up = up;
            this.Image = "ui/checkbox/on.png".AsmImgRes();
            this.AddTextCenter(text.AsDrawText().InSize(10).Montserrat());
        }

        public override void Focus()
        {
            this.Image = "ui/checkbox/hover.png".AsmImgRes();
            base.Focus();
        }

        public override void Unfocus()
        {
            this.Image = "ui/checkbox/on.png".AsmImgRes();
            base.Unfocus();
        }

        public override void Click(PointerArgs args)
        {
            this.Image = "ui/checkbox/pressed.png".AsmImgRes();
            base.Click(args);
        }

        public override void ClickRelease(PointerArgs args)
        {
            this.Image = "ui/checkbox/on.png".AsmImgRes();
            _up?.Invoke(Component);
            base.ClickRelease(args);
        }

        public override void GlobalClickRelease(PointerArgs args)
        {
            this.Image = "ui/checkbox/on.png".AsmImgRes();
            base.ClickRelease(args);
        }

        public override bool Visible => character.FreeStatPoints > 0;
    }
}