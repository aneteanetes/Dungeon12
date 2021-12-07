using Dungeon.ECS;
using Dungeon.Types;
using Dungeon.View.Interfaces;
using Dungeon12.ECS.Components;
using Dungeon12.SceneObjects.Base;
using System.Collections.Generic;

namespace Dungeon12.ECS.Systems
{
    public class TooltipCustomSystem : ISystem
    {
        public ISceneLayer SceneLayer { get; set; }

        public bool IsApplicable(ISceneObject sceneObject)
        {
            return sceneObject is ITooltipedCustom;
        }

        Dictionary<ITooltipedCustom, Tooltip> Tooltips = new Dictionary<ITooltipedCustom, Tooltip>();

        public void ProcessFocus(ISceneObject sceneObject)
        {
            if (sceneObject is ITooltipedCustom tooltiped)
            {
                if (!tooltiped.ShowTooltip)
                    return;

                var tooltipPosition = new Point(sceneObject.ComputedPosition.X, sceneObject.ComputedPosition.Y - 20);

                var tooltip = tooltiped.GetTooltip();
                Tooltips[tooltiped] = tooltip;
                SceneLayer.AddObject(tooltip);

                tooltipPosition.X += sceneObject.Width / 2 - Global.DrawClient.MeasureText(tooltip.TooltipText).X / 2;

                tooltip.SetPosition(tooltipPosition);
                tooltip.Visible = true;
            }
        }

        public void ProcessUnfocus(ISceneObject sceneObject)
        {
            if (sceneObject is ITooltipedCustom tooltiped)
            {
                if (Tooltips.TryGetValue(tooltiped, out var tooltip))
                {
                    tooltip.Destroy?.Invoke();
                    SceneLayer.RemoveObject(tooltip);
                    Tooltips.Remove(tooltiped);
                }
            }
        }
    }
}