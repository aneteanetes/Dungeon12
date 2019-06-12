﻿using Rogue.Control.Events;
using Rogue.Control.Keys;
using Rogue.Control.Pointer;
using Rogue.View.Interfaces;
using System;
using System.Linq;

namespace Rogue.Drawing.SceneObjects.UI
{
    public class DraggableControl : HandleSceneControl
    {
        private static int draggableLayers = 1;

        public override int Layer => 50;

        public override bool CacheAvailable => false;

        public override bool AbsolutePosition => true;

        public DraggableControl()
        {
            this.ZIndex = ++draggableLayers;
            this.Destroy += () => Global.BlockSceneControls = false;
            Global.BlockSceneControls = true;
        }

        protected override Key[] KeyHandles => new Key[] { Key.Escape }.Concat(OverrideKeyHandles).ToArray();

        protected virtual Key[] OverrideKeyHandles => new Key[0];

        protected override ControlEventType[] Handles => new ControlEventType[]
        {
            ControlEventType.Click,
            ControlEventType.GlobalClickRelease,
            ControlEventType.MouseMove,
            ControlEventType.Key,
             ControlEventType.Focus
        }.Concat(OverrideHandles).ToArray();

        protected virtual ControlEventType[] OverrideHandles => new ControlEventType[0];
        
        private bool drag = false;

        private PointerArgs delta = null;

        public override void MouseMove(PointerArgs args)
        {
            if (drag)
            {
                var argsX = args.X / 32;
                var deltaX = delta.X / 32;

                var argsY = args.Y / 32;
                var deltaY = delta.Y / 32;

                if (argsX > deltaX)
                {
                    this.Left -= deltaX - argsX;
                }
                else if (argsX!=deltaX)
                {
                    this.Left += argsX - deltaX;
                }

                if (argsY > deltaY)
                {
                    this.Top -= deltaY - argsY;
                }
                else if (argsY != deltaY)
                {
                    this.Top += argsY - deltaY;
                }
            }

            delta = args;

            base.MouseMove(args);
        }

        public override void Click(PointerArgs args)
        {
            delta = args;
            drag = true;
            base.Click(args);
        }

        public override void GlobalClickRelease(PointerArgs args)
        {
            if (drag)
            {
                drag = false;
            }
            base.GlobalClickRelease(args);
        }

        public override void KeyDown(Key key, KeyModifiers modifier, bool hold)
        {
            if(key== Key.Escape)
            {
                this.Destroy?.Invoke();
            }
        }

        protected override void AddChild(ISceneObjectControl sceneObject)
        {
            sceneObject.ZIndex = this.ZIndex;
            base.AddChild(sceneObject);
        }
    }
}