using Dungeon.Entities.Animations;
using Dungeon.Types;
using Dungeon12.Entities.Animations.Builders;

namespace Dungeon12.Entities.Animations
{
    public class AnimationMap
    {
        int framesPerAnim;

        public AnimationMap(Point size, string animTile,int frames, string animTileAxis = default)
        {
            this.framesPerAnim = frames;
            bind = AnimationBuilder
                .Bind(animTile, animTileAxis)
                .BindSize(size);
        }

        AnimationBuilder bind;


        public Animation Down() => bind
            .CreateSize()
            .WithFrames(0,0,framesPerAnim, Direction.Right)
            .Build();

        public Animation Up() => bind
            .CreateSize()
            .WithFrames(0,3,framesPerAnim, Direction.Right)
            .Build();

        public Animation Left() => bind
            .CreateSize()
            .WithFrames(0, 1, framesPerAnim, Direction.Right)
            .Build();


        public Animation Right() => bind
            .CreateSize()
            .WithFrames(0, 2, framesPerAnim, Direction.Right)
            .Build();


        public Animation UpLeft() => bind
            .CreateSize()
            .WithFrames(0,1, framesPerAnim, Direction.Right)
            .Axis()
            .Build();


        public Animation UpRight() => bind
            .CreateSize()
            .WithFrames(0, 3, framesPerAnim, Direction.Right)
            .Axis()
            .Build();


        public Animation DownLeft() => bind
            .CreateSize()
            .WithFrames(0,0, framesPerAnim, Direction.Right)
            .Axis()
            .Build();


        public Animation DownRight() => bind
            .CreateSize()
            .WithFrames(0, 2, framesPerAnim, Direction.Right)
            .Axis()
            .Build();
    }
}
