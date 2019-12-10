namespace Dungeon12.Drawing.SceneObjects.UI
{
    using Dungeon.Control.Events;
    using Dungeon.Control.Keys;
    using Dungeon.Control.Pointer;
    using Dungeon.Drawing;
    using Dungeon12.SceneObjects;
    using Dungeon.SceneObjects;
    using Dungeon.View.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Dungeon12;

    public abstract class TabControlFlex<TContent, TArgument, TTab> : TabControl<TContent, TArgument, TTab>
        where TContent : ISceneObject
        where TTab : TabControl<TContent, TArgument, TTab>
        where TArgument : class
    {
        public string Title { get; set; }
        private float _titleTextSize;

        public TabControlFlex(ISceneObject parent, bool active, TArgument argument = default, string title = null, float titleTextSize= 30f)
            : base(parent, active, argument, title)
        {
            _titleTextSize = titleTextSize;
            this.Title = title;
        }

        private static IEnumerable<(double textWidth, TabControlFlex<TContent, TArgument, TTab> tab)> Measure(TabControlFlex<TContent, TArgument, TTab>[] all)
        {
            return all.Select(flexTab => (Global.DrawClient.MeasureText(new DrawText(flexTab.Title) { Size= flexTab._titleTextSize }.Triforce()).X / 32, flexTab));
        }
        
        public static void Flex(TabControlFlex<TContent, TArgument, TTab>[] all)
        {
            var allMeasured = Measure(all);

            var widthParent = all.FirstOrDefault()?.parent?.Width ?? 0;
            var allWidth = allMeasured.Sum(t => t.textWidth);
            
            var needToDistribute = widthParent - allWidth;
                        
            double offset = 0;

            foreach (var (textWidth, tab) in allMeasured)
            {
                var flexTab = tab;
                flexTab.Width = textWidth;

                var proportionalSize = flexTab.Width / allWidth;

                flexTab.Left = offset;

                flexTab.Width += needToDistribute * proportionalSize;

                offset += flexTab.Width;

                flexTab.RemoveChild<TextControl>();
                flexTab.AddTextCenter(new DrawText(flexTab.Title) { Size= flexTab._titleTextSize }.Triforce(), true, true);
            }
        }
    }
}