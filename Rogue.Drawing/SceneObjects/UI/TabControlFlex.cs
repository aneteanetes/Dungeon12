namespace Rogue.Drawing.SceneObjects.UI
{
    using Rogue.Control.Events;
    using Rogue.Control.Keys;
    using Rogue.Control.Pointer;
    using Rogue.Drawing.Impl;
    using Rogue.Drawing.SceneObjects.Base;
    using Rogue.View.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class TabControlFlex<TContent, TArgument, TTab> : TabControl<TContent, TArgument, TTab>
        where TContent : ISceneObject
        where TTab : TabControl<TContent, TArgument, TTab>
    {
        public string Title { get; set; }

        public TabControlFlex(SceneObject parent, bool active, TArgument argument = default, string title = null)
            : base(parent, active, argument, title)
        {
            this.Title = title;
        }

        private static IEnumerable<(double textWidth, TabControlFlex<TContent, TArgument, TTab> tab)> Measure(TabControlFlex<TContent, TArgument, TTab>[] all)
        {
            return all.Select(flexTab => (Global.DrawClient.MeasureText(new DrawText(flexTab.Title)).X / 32, flexTab));
        }
        
        public static void Flex(TabControlFlex<TContent, TArgument, TTab>[] all)
        {
            var allMeasured = Measure(all);

            var widthParent = all[0].parent.Width;
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
                flexTab.AddTextCenter(new DrawText(flexTab.Title), true, true);
            }
        }
    }
}