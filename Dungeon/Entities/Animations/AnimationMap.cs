using Dungeon.Entities.Animations.Builders;
using Dungeon.Types;
using Dungeon.View;
using System;

namespace Dungeon.Entities.Animations
{
    public class AnimationMap
    {
        int framesPerAnim;

        public AnimationMap(Point size, string animTile, int frames, string animTileAxis = default, TimeSpan time = default)
        {
            framesPerAnim = frames;
            bind = AnimationBuilder
                .Bind(animTile, animTileAxis,time)
                .BindSize(size);
        }

        AnimationBuilder bind;


        public Animation Down() => new Lazy<Animation>(() => bind
            .CreateSize()
            .WithFrames(0, 0, framesPerAnim, Direction.Right)
            .Build()).Value;

        public Animation Up() => new Lazy<Animation>(() => bind
            .CreateSize()
            .WithFrames(0, 3, framesPerAnim, Direction.Right)
            .Build()).Value;

        public Animation Left() => new Lazy<Animation>(()=>bind
            .CreateSize()
            .WithFrames(0, 1, framesPerAnim, Direction.Right)
            .Build()).Value;


        public Animation Right() => new Lazy<Animation>(() => bind
            .CreateSize()
            .WithFrames(0, 2, framesPerAnim, Direction.Right)
            .Build()).Value;


        public Animation UpLeft() => new Lazy<Animation>(() => bind
            .CreateSize()
            .WithFrames(0, 1, framesPerAnim, Direction.Right)
            .Axis()
            .Build()).Value;


        public Animation UpRight() => new Lazy<Animation>(() => bind
            .CreateSize()
            .WithFrames(0, 3, framesPerAnim, Direction.Right)
            .Axis()
            .Build()).Value;


        public Animation DownLeft() => new Lazy<Animation>(() => bind
            .CreateSize()
            .WithFrames(0, 0, framesPerAnim, Direction.Right)
            .Axis()
            .Build()).Value;


        public Animation DownRight() => new Lazy<Animation>(() => bind
            .CreateSize()
            .WithFrames(0, 2, framesPerAnim, Direction.Right)
            .Axis()
            .Build()).Value;
    }
}
