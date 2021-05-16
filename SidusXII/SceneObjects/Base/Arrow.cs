namespace SidusXII.SceneObjects.Base
{
    using Dungeon.Control;
    using Dungeon.Drawing;
    using Dungeon.SceneObjects;
    using Dungeon.View.Interfaces;
    using System;

    public class Arrow : EmptySceneControl
    {
        public override bool CacheAvailable => false;

        private readonly Func<bool> onClick;

        private readonly TextControl textControl;

        public bool Active { get; set; }

        public Arrow(Func<bool> onClick, string arrow)
        {
            Width = 1;
            Height = 1;

            this.onClick = onClick;

            textControl = AddTextCenter(new DrawText(arrow) { Size = 35 }.Montserrat(), true, false);
        }

        public override void Click(PointerArgs args)
        {
            if (Active)
            {
                onClick.Invoke();
            }
        }

        public override IDrawText Text
        {
            get
            {
                textControl.Text.ForegroundColor = new DrawColor(Active
                    ? ConsoleColor.White
                    : ConsoleColor.Gray);

                return base.Text;
            }

            protected set => base.Text = value;
        }
    }
}
