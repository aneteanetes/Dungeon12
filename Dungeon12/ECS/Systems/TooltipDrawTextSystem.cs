using Dungeon;
using Dungeon.Control;
using Dungeon.ECS;
using Dungeon.Types;
using Dungeon.View.Interfaces;
using Dungeon12.ECS.Components;
using Dungeon12.SceneObjects.Base;

namespace Dungeon12.ECS.Systems
{
    internal class TooltipDrawTextSystem : ISystem
    {
        private static Dictionary<ITooltipedDrawText, Tooltip> Tooltips = new Dictionary<ITooltipedDrawText, Tooltip>();
        private static Dictionary<ITooltiped, Tooltip> Tooltips1 = new Dictionary<ITooltiped, Tooltip>();

        public Tooltip GetTooltip(ISceneObject sceneObject)
        {
            if (sceneObject is ITooltipedDrawText tooltipedDrawText)
            {
                if (Tooltips.TryGetValue(tooltipedDrawText, out var tooltip))
                    return tooltip;
            }
            if (sceneObject is ITooltiped tooltiped)
            {
                if (Tooltips1.TryGetValue(tooltiped, out var tooltip))
                    return tooltip;
            }

            return null;
        }

        public ISceneLayer SceneLayer { get; set; }

        public bool IsApplicable(ISceneObject sceneObject)
        {
            return
                sceneObject is ITooltipedDrawText
                || sceneObject is ITooltiped;
        }

        public void ProcessFocus(ISceneObject sceneObject)
        {
            if(sceneObject is ITooltiped tooltiped)
            {
                if (tooltiped.TooltipText.IsNotEmpty())
                    ProcessFocus(sceneObject, tooltiped);
            }

            if (sceneObject is ITooltipedDrawText tooltipedDrawText)
            {
                if (!tooltipedDrawText.ShowTooltip)
                    return;

                //tooltiped.RefreshTooltip();

                var pointer = Global.PointerLocation;

                var tooltipPosition = new Dot(pointer.X, pointer.Y-20);

                if (!Tooltips.TryGetValue(tooltipedDrawText, out var tooltip) )
                {
                    Tooltips.Add(tooltipedDrawText, null);
                    sceneObject.OnDestroy += () =>
                    {
                        Tooltips.Remove(tooltipedDrawText);
                        SceneLayer.RemoveObject(tooltip);
                    };

#warning ОБЪЕКТЫ ЛИШИЛИСЬ ABSOLUTE POSITION, ПОЭТОМУ ТУЛТИПЫ ДОЛЖНЫ БЫТЬ НА АБСОЛЮТНОМ СЛОЕ
                    tooltip = new Tooltip(tooltipedDrawText.TooltipText, tooltipPosition)
                    {
                        //LayerLevel = 100
                    };

                    SceneLayer.AddObject(tooltip);
                    Tooltips[tooltipedDrawText] = tooltip;
                }

                //tooltipPosition.X += sceneObject.Width / 2 - Global.GameClient.MeasureText(tooltip.Text.Text).X / 2;

                if (tooltipPosition.Y < 0)
                {
                    tooltipPosition.Y = 5;
                    tooltipPosition.X = sceneObject.Left + sceneObject.Width + 5;
                }

                tooltip.SetPosition(tooltipPosition);
                tooltip.Visible = true;
            }
        }

        private void ProcessFocus(ISceneObject sceneObject, ITooltiped tooltiped)
        {
            var pointer = Global.PointerLocation;

            var tooltipPosition = new Dot(pointer.X, pointer.Y-20);

            if (Tooltips1.TryGetValue(tooltiped, out var tooltip))
            {
                tooltip?.Destroy();
            }

            sceneObject.OnDestroy += () =>
            {
                Tooltips1.Remove(tooltiped);
                SceneLayer.RemoveObject(tooltip);
            };

#warning ОБЪЕКТЫ ЛИШИЛИСЬ ABSOLUTE POSITION, ПОЭТОМУ ТУЛТИПЫ ДОЛЖНЫ БЫТЬ НА АБСОЛЮТНОМ СЛОЕ

            tooltip = new Tooltip(tooltiped.TooltipText.Gabriela(), tooltipPosition,1)
            {
                LayerLevel = 100
            };

            SceneLayer.AddObject(tooltip);
            Tooltips1[tooltiped] = tooltip;


            //tooltipPosition.X += sceneObject.Width / 2 - Global.GameClient.MeasureText(tooltip.Text.Text).X / 2;

            if (tooltipPosition.Y < 0)
            {
                tooltipPosition.Y = 5;
                tooltipPosition.X = sceneObject.Left + sceneObject.Width + 5;
            }

            tooltip.SetPosition(tooltipPosition);
            tooltip.Visible = true;
        }

        public void ProcessUnfocus(ISceneObject sceneObject)
        {
            if(sceneObject is ITooltiped tooltiped)
            {
                if (Tooltips1.TryGetValue(tooltiped, out var tooltip))
                {
                    tooltip?.Destroy();
                }
            }

            if (sceneObject is ITooltipedDrawText tooltipedDrawText)
            {
                if (Tooltips.TryGetValue(tooltipedDrawText, out var tooltip))
                {
                    tooltip.Visible = false;
                }
            }
        }

        public void ProcessClick(PointerArgs pointerArgs, ISceneObject sceneObject) { }

        public void ProcessGlobalClickRelease(PointerArgs pointerArgs) { }
    }
}