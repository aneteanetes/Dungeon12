using Dungeon.Types;
using Dungeon.View;
using System;

namespace Dungeon.Entities.Animations.Builders
{
    public class AnimationBuilder
    {
        internal Animation animation;
        string normal;
        string axis;
        Dot size;
        TimeSpan time;

        public virtual Animation Build()
        {
            return animation;
        }

        public AnimationBuilder() { }

        public static AnimationBuilder Bind(string tile, string tileAxis, TimeSpan time=default) => new AnimationBuilder()
        {
            normal = tile,
            axis = tileAxis,
            time=time
        };

        public AnimationBuilder BindSize(Dot size)
        {
            this.size = size;
            return this;
        }

        public static AnimationBuilder Create(string tile, string tileAxis)
        {
            var builder = Bind(tile, tileAxis);
            builder.animation = new Animation()
            {
                TileSet = tile,
                Time = builder.time
            };
            return builder;
        }

        public AnimationBuilder Create() => Create(normal, axis);

        public FramesAnimationBuilder CreateSize()
        {
            var builder = Create(normal, axis);
            return builder.InSize(size);
        }

        public FramesAnimationBuilder InSize(Dot size)
        {
            animation.Size = size;
            return new FramesAnimationBuilder(this);
        }

        public virtual AnimationBuilder Axis()
        {
            animation.TileSet = axis;
            return this;
        }

        public virtual AnimationBuilder Normal()
        {
            animation.TileSet = normal;
            return this;
        }
    }
}
