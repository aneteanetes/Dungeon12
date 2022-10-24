using Dungeon.Control;
using Dungeon.Control.Pointer;
using Dungeon.ECS;
using Dungeon.Types;
using Dungeon.View.Interfaces;
using Dungeon12.ECS.Components;
using Dungeon12.SceneObjects.Base;
using System.Collections.Generic;

namespace Dungeon12.ECS.Systems
{
    internal class TooltipCustomSystem : ISystem
    {
        public ISceneLayer SceneLayer { get; set; }

        public bool IsApplicable(ISceneObject sceneObject)
        {
            return sceneObject is ITooltipedCustom;
        }

        Dictionary<ITooltipedCustom, ISceneObject> Tooltips = new Dictionary<ITooltipedCustom, ISceneObject>();

        public void ProcessFocus(ISceneObject sceneObject)
        {
            if (sceneObject is ITooltipedCustom tooltiped)
            {
                if (!tooltiped.ShowTooltip)
                    return;

                var tooltipPosition = new Dot(sceneObject.ComputedPosition.X, sceneObject.ComputedPosition.Y - 20);

                var tooltip = tooltiped.GetTooltip();
                Tooltips[tooltiped] = tooltip;
                SceneLayer.AddObject(tooltip);

                tooltipPosition.X += sceneObject.Width / 2 - Global.GameClient.MeasureText(tooltip.Text).X / 2;
                if (tooltipPosition.Y < 0)
                {
                    tooltipPosition.Y = 5;
                    tooltipPosition.X = sceneObject.Left + sceneObject.Width + 5;
                }

                tooltip.Top = tooltipPosition.Y;
                tooltip.Left = tooltipPosition.X;
                tooltip.Visible = true;
            }
        }

        public void ProcessUnfocus(ISceneObject sceneObject)
        {
            if (sceneObject is ITooltipedCustom tooltiped)
            {
                if (Tooltips.TryGetValue(tooltiped, out var tooltip))
                {
                    tooltip.Destroy();
                    SceneLayer.RemoveObject(tooltip);
                    Tooltips.Remove(tooltiped);
                }
            }
        }

        public void ProcessClick(PointerArgs pointerArgs, ISceneObject sceneObject) { }

        public void ProcessGlobalClickRelease(PointerArgs pointerArgs) { }
    }
}