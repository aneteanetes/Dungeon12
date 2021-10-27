using Dungeon.Entities.Animations;
using Dungeon.Types;

namespace Dungeon12.Entities.Animations.Builders
{
    public class AnimationBuilder
    {
        internal Animation animation;
        string normal;
        string axis;
        Point size;

        public virtual Animation Build() => animation;

        public AnimationBuilder() { }

        public static AnimationBuilder Bind(string tile, string tileAxis) => new AnimationBuilder()
        {
            normal = tile,
            axis = tileAxis
        };

        public AnimationBuilder BindSize(Point size)
        {
            this.size = size;
            return this;
        }

        public static AnimationBuilder Create(string tile, string tileAxis)
        {
            var builder = Bind(tile, tileAxis);
            builder.animation = new Animation()
            {
                TileSet = tile
            };
            return builder;
        }

        public AnimationBuilder Create() => Create(this.normal, this.axis);

        public FramesAnimationBuilder CreateSize()
        {
            var builder = Create(this.normal, this.axis);
            return builder.InSize(size);
        }

        public FramesAnimationBuilder InSize(Point size)
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
