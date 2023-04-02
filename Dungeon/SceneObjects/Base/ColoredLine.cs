using Dungeon.Drawing.Impl;
using Dungeon.Drawing;
using Dungeon.View.Interfaces;
using System.Collections.Generic;
using Dungeon.Types;
using System;

namespace Dungeon.SceneObjects.Base
{
    public class ColoredLine : EmptySceneControl
    {
        public ColoredLine(double length, Direction direction, double depth)
        {
            Length= length;
            Direction= direction;
            Depth= depth;
            this.Width=length;
            this.Height=depth;
        }

        private DrawablePath drawablePath;

        private double _length;
        public double Length
        {
            get => _length;
            set
            {
                _length = value;
                if(this.drawablePath!= null)
                {
                    this.drawablePath.Paths[1]=GetTo();
                }
            }
        }

        private double _angle;
        /// <summary>
        /// В РАДИАНАХ
        /// </summary>
        public override double Angle
        {
            get => _angle;
            set
            {
                _angle = value;
                if (this.drawablePath!= null)
                {
                    this.drawablePath.Angle=_angle;
                }
            }
        }

        public override double Width { get => Length; set => Length=value; }

        public override void Throw(Exception ex)
        {
            throw ex;
        }

        public Direction Direction { get; set; }

        private Dot dotTo;
        private Dot GetTo()
        {
            switch (Direction)
            {
                case Direction.Up:
                    dotTo= new Dot(Left, Top-Length);
                    break;
                case Direction.Down:
                    dotTo= new Dot(Left, Top+Length);
                    break;
                case Direction.Left:
                    dotTo= new Dot(Left+Length, Top);
                    break;
                case Direction.Right:
                    dotTo= new Dot(Left-Length, Top);
                    break;
                case Direction.UpLeft:
                    dotTo= new Dot(Left, Top-Length);
                    this.Angle=-5.5;
                    break;
                case Direction.UpRight:
                    dotTo= new Dot(Left, Top-Length);
                    this.Angle=5.5;
                    break;
                case Direction.DownLeft:
                    dotTo= new Dot(Left, Top+Length);
                    this.Angle=5.5;
                    break;
                case Direction.DownRight:
                    dotTo= new Dot(Left, Top-Length);
                    this.Angle=-5.5;
                    break;
                default:
                    break;
            }

            return dotTo;
        }

        private double _depth;
        public double Depth
        {
            get
            {
                if(drawablePath!=default)
                    return drawablePath.Depth;
                return drawablePath?.Depth ?? _depth;
            }

            set
            {
                if (drawablePath!=default)
                    drawablePath.Depth=(float)value;
                _depth=value;
            }
        }

        public override IDrawablePath Path
        {
            get
            {
                if (drawablePath == null)
                {
                    var color = new DrawColor(this.Color)
                    {
                        Opacity = Opacity,
                        A = 255
                    };

                    drawablePath = new DrawablePath
                    {
                        BackgroundColor = color,
                        Depth = Depth,
                        PathPredefined = View.Enums.PathPredefined.Line,
                        Paths=new List<Dot>()
                        {
                            new Dot(){ X=Left, Y=Top },
                            GetTo()
                        }
                    };

                    drawablePath.Angle=this.Angle;
                }

                return drawablePath;
            }
        }
    }
}
