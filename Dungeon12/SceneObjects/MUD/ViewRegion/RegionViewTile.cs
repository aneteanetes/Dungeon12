using Dungeon;
using Dungeon.Control;
using Dungeon.Drawing;
using Dungeon.Drawing.SceneObjects;
using Dungeon.ECS;
using Dungeon.ECS.Impl;
using Dungeon.SceneObjects;
using Dungeon.SceneObjects.Base;
using Dungeon.Types;
using Dungeon.View.Interfaces;
using Dungeon12.ECS.Components;
using Dungeon12.Entities.Map;
using System.ComponentModel;

namespace Dungeon12.SceneObjects.MUD.ViewRegion
{
    internal class RegionViewTile : SceneControl<Location>, ITooltipedDrawText
    {
        private ImageControl mark;

        public RegionViewTile(Location component) : base(component)
        {
            this.Width = 25;
            this.Height = 25;

            var rect = this.AddChild(new DarkRectangle()
            {
                Color = new DrawColor(53, 149, 83),
                Width=this.Width,
                Height=this.Height,
                Opacity=1
            });

            this.AddChild(new DarkRectangle()
            {
                Color = new DrawColor(ConsoleColor.DarkGray),
                Width=this.Width,
                Height=this.Height,
                Depth=2,
                Opacity=1,
                Fill=false
            });

            mark = this.AddChild(new ImageControl("MUD/RegionView/TransitionView/current.png") { Width=26, Height=26, Left=10, Top=-10, TooltipText="(Вы здесь)", MousePerPixel=true });
            mark.VisibleFunction=() => Component.IsCurrent;

            this.Component.Transitions.ForEach(t =>
            {
                this.AddChild(new TransitionView(t));
            });
        }

        public IDrawText TooltipText => Component.Name.AsDrawText();

        public bool ShowTooltip => true;

        public override void Click(PointerArgs args)
        {
            Console.WriteLine($"tile {Component.Name} | X:{Component.X}, Y:{Component.Y}");
            //base.Click(args);
        }

        private class TransitionView : SceneControl<LocationTransition>
        {
            public TransitionView(LocationTransition component) : base(component)
            {
                bool isDiagonal = component.Direction.IsDiagonal();

                var square = 25;
                if (isDiagonal)
                    square=29;

                this.Width=square;
                this.Height=square;

                switch (component.Direction)
                {
                    case Direction.Up:
                        Top-=25;
                        break;
                    case Direction.Down:
                        Top+=25;
                        break;
                    case Direction.Left:
                        Left-=25;
                        break;
                    case Direction.Right:
                        Left+=25;
                        break;
                    case Direction.UpLeft:
                        Left-=27;
                        Top-=27;
                        break;
                    case Direction.UpRight:
                        Top-=27;
                        Left+=23;
                        break;
                    case Direction.DownLeft:
                        Top+=23;
                        Left-=27;
                        break;
                    case Direction.DownRight:
                        Top+=23;
                        Left+=23;
                        break;
                    default:
                        break;
                }

                var img = this.AddChild(new ImageControl($"MUD/RegionView/TransitionView/{component.Direction.ToStringUpDown().ToLowerInvariant()}/{component.State.ToString().ToLowerInvariant()}.png")
                {
                    PerPixelCollision =true,
                    Width=this.Width,
                    Height=this.Height,
                });
                img.Components.Add(new ECSComponent(typeof(ITooltiped), new object[] { component.Name }));

                return;
            }

            public override void Throw(Exception ex)
            {
                throw ex;
            }
        }
    }
}
