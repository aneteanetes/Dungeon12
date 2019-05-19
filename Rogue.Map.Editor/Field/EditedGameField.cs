namespace Rogue.Map.Editor.Field
{
    using Rogue.Control.Events;
    using Rogue.Control.Pointer;
    using Rogue.Drawing.SceneObjects;
    using Rogue.Drawing.SceneObjects.Base;
    using Rogue.Map.Editor.Objects;
    using Rogue.Types;
    using Rogue.View.Interfaces;
    using System;
    using System.Linq;

    public class EditedGameField : HandleSceneControl
    {
        public override bool CacheAvailable => false;
        private int lvl = 1;
        private bool obstruct = false;

        public EditedGameField()
        {
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

        public void SetLevel(int lvl) => this.lvl = lvl;

        public void SetObstruct(bool obstruct) => this.obstruct = obstruct;

        public readonly DesignField Field = new DesignField();

        public override void Click(PointerArgs args)
        {
            hold = true;

            int x = (int)Math.Truncate((args.X / 32) - (args.Offset.X / 32) - this.Left - (args.Offset.X / 32));
            int y = (int)Math.Truncate((args.Y / 32) - (args.Offset.Y / 32) - this.Top - (args.Offset.Y / 32));

            Console.WriteLine(args.MouseButton);

            if (args.MouseButton == MouseButton.Right)
            {
                var exists = Field[lvl][x][y];
                if (exists != null)
                {
                    this.RemoveChild(exists.SceneObject);
                    Field[lvl][x][y] = null;
                }
                return;
            }

            var canPut = Field[lvl][x][y] == null;

            if (!canPut)
            {
                canPut = Field[lvl][x][y].SceneObject.ImageRegion != current.ImageRegion;
                if (canPut)
                {
                    this.RemoveChild(Field[lvl][x][y].SceneObject);
                    Field[lvl][x][y] = null;
                }
            }            

            if (current != null && canPut)
            {
                Field[lvl][x][y] = new DesignCell(new ImageControl(current.Image)
                {
                    ImageRegion = current.ImageRegion,
                    Left = x,
                    Top = y,
                    Height = 1,
                    Width = 1,
                    Layer = lvl
                })
                {
                    Obstruction = this.obstruct
                };
                this.AddChild(Field[lvl][x][y].SceneObject);
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
