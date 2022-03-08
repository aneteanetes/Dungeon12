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
    public class TooltipSystem : ISystem
    {
        private static Dictionary<ITooltiped, Tooltip> Tooltips = new Dictionary<ITooltiped, Tooltip>();

        public Tooltip GetTooltip(ISceneObject sceneObject)
        {
            if (sceneObject is ITooltiped tooltiped)
            {
                if (Tooltips.TryGetValue(tooltiped, out var tooltip))
                    return tooltip;
            }

            return null;
        }

        public ISceneLayer SceneLayer { get; set; }

        public bool IsApplicable(ISceneObject sceneObject)
        {
            return sceneObject is ITooltiped;
        }

        public void ProcessFocus(ISceneObject sceneObject)
        {
            if (sceneObject is ITooltiped tooltiped)
            {
                if (!tooltiped.ShowTooltip)
                    return;

                //tooltiped.RefreshTooltip();

                var tooltipPosition = new Point(sceneObject.ComputedPosition.X, sceneObject.ComputedPosition.Y - 20);

                if (!Tooltips.TryGetValue(tooltiped, out var tooltip) )
                {
                    Tooltips.Add(tooltiped, null);
                    sceneObject.Destroy += () =>
                    {
                        Tooltips.Remove(tooltiped);
                        SceneLayer.RemoveObject(tooltip);
                    };

                    tooltip = new Tooltip(tooltiped.TooltipText, tooltipPosition)
                    {
                        AbsolutePosition = sceneObject.AbsolutePosition,
                        LayerLevel = 100
                    };

                    SceneLayer.AddObject(tooltip);
                    Tooltips[tooltiped] = tooltip;
                }

                tooltipPosition.X += sceneObject.Width / 2 - Global.DrawClient.MeasureText(tooltip.TooltipText).X / 2;

                if (tooltipPosition.Y < 0)
                {
                    tooltipPosition.Y = 5;
                    tooltipPosition.X = sceneObject.Left + sceneObject.Width + 5;
                }

                tooltip.SetPosition(tooltipPosition);
                tooltip.Visible = true;
            }
        }

        public void ProcessUnfocus(ISceneObject sceneObject)
        {
            if (sceneObject is ITooltiped tooltiped)
            {
                if (Tooltips.TryGetValue(tooltiped, out var tooltip))
                {
                    tooltip.Visible = false;
                }
            }
        }

        public void ProcessClick(PointerArgs pointerArgs, ISceneObject sceneObject) { }

        public void ProcessGlobalClickRelease(PointerArgs pointerArgs) { }
    }
}