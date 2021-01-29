using Dungeon.Data;
using Dungeon.Types;
using Dungeon12.Entities.Alive;
using Dungeon12.Entities.Animations;
using System;

namespace Dungeon12.GameObjects.Character
{
    public class CharacterEntity : Moveable, IPersist
    {
        /// <summary>
        /// deserialize ctor
        /// </summary>
        [Obsolete("deserialize ctor", true)]
        public CharacterEntity()
        {

        }

        public override double Speed => 70 / 100d;


        public Point Size { get; set; }

        public string ImageAxis { get; set; }

        public CharacterEntity(Point size, string tile, string tileAxis)
        {
            this.Image = tile;
            ImageAxis = tileAxis;

            var animap = new AnimationMap(size, Image, 3, ImageAxis);

            this.MoveUp = animap.Up();
            this.MoveDown = animap.Down();
            this.MoveLeft = animap.Left();
            this.MoveRight = animap.Right();
            this.MoveUpLeft = animap.UpLeft();
            this.MoveUpRight = animap.UpRight();
            this.MoveDownLeft = animap.DownLeft();
            this.MoveDownRight = animap.DownRight();

            this.DefaultFramePosition = new Rectangle()
            {
                X = 70,
                Y = 0,
                Height = 70,
                Width = 70
            };
        }
    }
}