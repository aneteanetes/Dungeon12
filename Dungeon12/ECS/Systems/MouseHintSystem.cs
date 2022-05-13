using Dungeon.Control;
using Dungeon.Control.Pointer;
using Dungeon.ECS;
using Dungeon.Scenes;
using Dungeon.Types;
using Dungeon.View.Interfaces;
using Dungeon12.ECS.Components;
using Dungeon12.SceneObjects.Base;
using System.Collections.Generic;

namespace Dungeon12.ECS.Systems
{
    public class MouseHintSystem : ISystem
    {
        public ISceneLayer SceneLayer { get; set; }

        public bool IsApplicable(ISceneObject sceneObject)
        {
            return sceneObject is IMouseHint;
        }

        public void ProcessClick(PointerArgs pointerArgs, ISceneObject sceneObject)
        {
            if (pointerArgs.MouseButton == MouseButton.Right)
            {
                CreateHint(pointerArgs, sceneObject);
            }
        }

        public void ProcessFocus(ISceneObject sceneObject)
        {
            if (sceneObject is IMouseHint mouseHinted)
            {
                var mouse = Global.PointerLocation;
                if (mouse.MouseButton == MouseButton.Right && !mouse.Released)
                {
                    CreateHint(Global.PointerLocation,sceneObject);
                }
            }
        }

        public void ProcessGlobalClickRelease(PointerArgs pointerArgs) => DestroyHint(true);

        public void ProcessUnfocus(ISceneObject sceneObject)
        {
            DestroyHint();
        }

        private void CreateHint(PointerArgs args, ISceneObject sceneObject)
        {
            if (sceneObject is not IMouseHint mouseHintObject)
                return;
            var hint = mouseHintObject.CreateMouseHint();
            if (hint==null)
                return;

            var tooltipsys = SceneLayer.GetSystem<TooltipDrawTextSystem>();
            var tooltip = tooltipsys.GetTooltip(sceneObject);
            if (tooltip != default)
                tooltip.Visible = false;

            DestroyHint();

            hint.Host = sceneObject;

            var hintPos = new Point(args.X+15, args.Y);
            sceneObject.OnDestroy += () =>
            {
                if (hint != null)
                {
                    sceneObject.Layer.RemoveObject(hint);
                    ExistedHint = null;
                }
            };

            sceneObject.Layer.AddObject(hint);

            if (hintPos.Y < 0)
            {
                hintPos.Y = 5;
            }

            if (hint.Width + hintPos.X > Global.Resolution.Width)
            {
                var offset = (hint.Width + hintPos.X) - Global.Resolution.Width;
                hintPos.X -= offset;
            }

            if(hint.Height+hintPos.Y > Global.Resolution.Height-150)
            {
                hintPos.Y -= 100;
            }

            hint.Left = hintPos.X;
            hint.Top = hintPos.Y;

            ExistedHint = hint;
        }

        private GameHint ExistedHint;

        private void DestroyHint(bool clickrelease=false)
        {
            if (ExistedHint != null)
            {
                if (clickrelease)
                {
                    var tooltipsys = SceneLayer.GetSystem<TooltipDrawTextSystem>();
                    var tooltip = tooltipsys.GetTooltip(ExistedHint.Host);
                    if (tooltip != default)
                        tooltip.Visible = true;
                }

                ExistedHint.Destroy();
                ExistedHint = null;
            }
        }
    }
}