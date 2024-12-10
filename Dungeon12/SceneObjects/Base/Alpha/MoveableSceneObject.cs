﻿namespace Nabunassar.Drawing.SceneObjects.Map
{
    using Dungeon.Types;
    using Dungeon.View;
    using Nabunassar.Components;
    using System;
    using System.Collections.Generic;

    internal abstract class MoveableSceneObject<T> : AnimatedSceneObject<T>
        where T : class, IFrameAnimated
    {
        public MoveableSceneObject(T @object)
            : base(@object)
        {
            this.Image = Component.FrameAnimated.Tileset;
        }

        protected int moveDistance = 0;
        protected Direction move;

        public override bool Updatable => true;

        protected bool AutoMove => true;

        protected void Move(Direction dir)
        {
            //if (Component.PhysicalObject.Move(dir))
            //{
            //    var anim = MovementMap[dir];
            //    SetAnimation(anim(Component.FrameAnimated));
            //}
        }


        private Dictionary<Direction, Func<FrameAnimated, Animation>> MovementMap => new Dictionary<Direction, Func<FrameAnimated, Animation>>()
        {
            { Direction.Idle,x=>x.Idle },
            { Direction.Left,x=>x.MoveLeft },
            { Direction.Right,x=>x.MoveRight},
            { Direction.Down,x=>x.MoveDown },
            { Direction.Up,x=>x.MoveUp },
            { Direction.DownLeft,x=>x.MoveDown },
            { Direction.DownRight,x=>x.MoveDown },
            { Direction.UpLeft,x=>x.MoveUp },
            { Direction.UpRight,x=>x.MoveUp },
        };
    }
}
