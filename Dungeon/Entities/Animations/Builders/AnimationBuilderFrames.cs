using Dungeon.Types;
using Dungeon.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dungeon.Entities.Animations.Builders
{
    public class FramesAnimationBuilder : AnimationBuilder
    {
        AnimationBuilder animationBuilder;

        public override Animation Build() => animationBuilder.Build();

        public FramesAnimationBuilder(AnimationBuilder builder)
        {
            animationBuilder = builder;
        }

        public AnimationBuilder WithFrames(params (int x, int y)[] poses)
        {
            var frames = new List<Point>();
            foreach (var pos in poses)
            {
                frames.Add(new Point(pos.x * animationBuilder.animation.Size.X, pos.y * animationBuilder.animation.Size.Y));
            }
            animationBuilder.animation.Frames = frames;

            return animationBuilder;
        }

        public AnimationBuilder WithFrames(int xStart, int yStart, int frames, Direction direction)
        {
            var frameMap = Enumerable.Range(0, frames)
                .Select<int, (int x, int y)>((v, i) =>
                  {
                      var index = i + 1;
                      switch (direction)
                      {
                          case Direction.Up: return (xStart, yStart - index);
                          case Direction.Down: return (xStart, yStart + index);
                          case Direction.Left: return (xStart - index, yStart);
                          case Direction.Right: return (xStart + index, yStart);
                          case Direction.UpLeft: return (xStart - index, yStart - index);
                          case Direction.UpRight: return (xStart + index, yStart - index);
                          case Direction.DownLeft: return (xStart - index, yStart + index);
                          case Direction.DownRight: return (xStart + index, yStart + index);
                          default: return (-1, -1);
                      }
                  })
                .ToArray();

            var minus = frameMap.FirstOrDefault(v => v.x == -1 || v.y == -1);
            if (minus != default)
                return animationBuilder;

            return WithFrames(frameMap);
        }

        public override AnimationBuilder Axis() => animationBuilder.Axis();

        public override AnimationBuilder Normal() => animationBuilder.Normal();
    }
}
