namespace Rogue.Map.Editor.Field
{
    using Rogue.Control.Events;
    using Rogue.Control.Pointer;
    using Rogue.Drawing.SceneObjects;
    using Rogue.Drawing.SceneObjects.Base;
    using Rogue.Types;
    using Rogue.View.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class EditedGameField : HandleSceneControl
    {
        public override bool CacheAvailable => false;

        public EditedGameField()
        {
            for (int x = 0; x < 100; x++)
            {
                inner[x] = new ISceneObject[100];
            }

            this.Width = 100;
            this.Height = 100;

            var border = new DarkRectangle
            {
                Fill = false,
                Color = ConsoleColor.White,
                Opacity = 1
            };
            border.Left = -0.5;
            border.Top = -0.5;
            border.Width = 100;
            border.Height = 100;

            this.AddChild(border);
        }

        private ImageControl current;

        public void Selecting(ImageControl imageControl) => current = imageControl;

        private readonly ISceneObject[][] inner = new ISceneObject[100][];

        public override void Click(PointerArgs args)
        {
            hold = true;

            int x = (int)Math.Truncate((args.X / 32) - (args.Offset.X / 32) - this.Left - (args.Offset.X / 32));
            int y = (int)Math.Truncate((args.Y / 32) - (args.Offset.Y / 32) - this.Top - (args.Offset.Y / 32));

            var canPut = inner[x][y] == null;

            if (!canPut)
            {
                canPut = inner[x][y].Image != current.Image;
            }            

            if (current != null && canPut)
            {
                inner[x][y] = new ImageControl(current.Image)
                {
                    ImageRegion = current.ImageRegion,
                    Left = x,
                    Top = y,
                    Height = 1,
                    Width = 1
                };
                this.AddChild(inner[x][y]);
            }
        }

        public override Rectangle CropPosition => new Rectangle
        {
            X = this.Position.X,
            Y = this.Position.Y,
            Height = this.Children.Skip(1).Max(c => c.Position.Y + c.Position.Height),
            Width = this.Children.Skip(1).Max(c => c.Position.X + c.Position.Width)
        };

        public override void GlobalClickRelease(PointerArgs args)
        {
            hold = false;
        }

        public override void MouseMove(PointerArgs args) => Click(args);

        bool hold = false;

        private ControlEventType[] FreeHandles => new ControlEventType[]
        {
            ControlEventType.Click,
            ControlEventType.GlobalClickRelease
        };

        private ControlEventType[] HoldHandles => new ControlEventType[]
        {
            ControlEventType.Click,
            ControlEventType.GlobalClickRelease,
                ControlEventType.MouseMove
        };

        protected override ControlEventType[] Handles => hold ? HoldHandles : FreeHandles;
    }
}
