using Dungeon.Classes;
using Dungeon.Drawing.SceneObjects;
using Dungeon12.Entites.Journal;
using System.Collections.Generic;
using System.Linq;
using Dungeon;
using Dungeon.Drawing.Impl;
using Dungeon12.Drawing.SceneObjects.Main.CharacterBar;
using System;
using Dungeon.Control.Pointer;
using Dungeon.View.Interfaces;
using Dungeon.Control.Keys;
using Dungeon12.Drawing.SceneObjects.Main.CharacterInfo.Journal;
using Dungeon.Control.Events;
using Dungeon12.SceneObjects.Main.CharacterInfo.Journal;
using Dungeon.SceneObjects;
using Dungeon.SceneObjects.Mixins;
using Dungeon.SceneObjects.Base;

namespace Dungeon12.Drawing.SceneObjects.Main.CharacterInfo.Talants
{
    public class JournalTabContent : HandleSceneControl
    {
        public override bool CacheAvailable => false;

        public override bool AbsolutePosition => true;

        private static readonly HashSet<string> ExpandedGroups = new HashSet<string>();

        private HandleSceneControl currentOpenedScroll;

        private JournalCategory _jcategory;

        public JournalTabContent(JournalCategory jcategory, JournalList journalList, double left)
        {
            this.AddMixin(new Scrollbar(13, v => FillContent(left,v))
            {
                Left=10.5-left,
                Top=2
            });

            _jcategory = jcategory;
            JournalGroup.SlideIndex = 0;
            JournalGroup.SlideDownCount = 0;
            JournalGroup.groups.Clear();

            Global.Events.Subscribe<JournalWindowOnKeyProcessedEvent>(e =>
            {
                currentOpenedScroll = null;
            }, false);

            journalList.CanDestroyParentBinding = () => currentOpenedScroll == null;

            this.Width = 12;

            this.Height = 15;

            var groups = _jcategory.Content?.GroupBy(c => c.Group) ?? Enumerable.Empty<IGrouping<string, JournalEntry>>();
            JournalGrouped = groups.ToList();

            FillContent(left);

            var scrollbar = Mixin<Scrollbar>();
            scrollbar.CanDown = ContentSize >= 10;
            scrollbar.MaxDownIndex = (int)Math.Floor(ContentSize - 12);
        }

        private int ScrollShift
        {
            get
            {
                var commonHeight = JournalGroups.Sum(g =>
                {
                    return 1.5 + (g.IsExpanded ? g.JournalEntries.Sum(e => 1.5) : 0);
                }) + 1.5;
                return (int)Math.Round(commonHeight - 13.5, MidpointRounding.AwayFromZero);
            }
        }

        private double ContentSize => JournalGroups.Sum(g => { return 1.5 + (g.IsExpanded ? g.JournalEntries.Sum(e => 1.5) : 0); }) + 1.5;

        List<IGrouping<string, JournalEntry>> JournalGrouped = new List<IGrouping<string, JournalEntry>>();
        List<JournalGroup> JournalGroups = new List<JournalGroup>();

        private void FillContent(double left, MouseWheelEnum? vector=null)
        {
            this.RemoveChild<JournalGroup>();
            JournalGroups = new List<JournalGroup>();

            var scrollIndex = this.Mixin<Scrollbar>().ScrollIndex;

            double top = 2 - (scrollIndex*1.5);
            int i = 0;
            foreach (var group in JournalGrouped)
            {
                var index = JournalGrouped.IndexOf(group);
                var jgroup = new JournalGroup(group.Key, index, i, group, so =>
                {
                    so.Top -= 1.5;
                    so.Left -= left;
                    so.Left += 12;
                    if (currentOpenedScroll != default)
                    {
                        currentOpenedScroll.Destroy();
                    }
                    currentOpenedScroll = so;
                    this.AddChild(so);
                }, this.Mixin<Scrollbar>());
                jgroup.SlideNeed = SlideNeed(jgroup);
                jgroup.Left -= left;
                jgroup.Left += .5;
                jgroup.Top += 2 + top;

                JournalGroups.Add(jgroup);
                this.AddChild(jgroup);
                top += 1.5;

                //jgroup.OnExpand = () =>
                //{
                //    var scrollindex =this.GetMixinValue<int>(ScrollableMixin.ScrollBarIndex);
                //    ExpandedGroups.Add(group.Key);
                //    Mixin<Scrollbar>().Down.Active = ScrollShift > 0;
                //    Mixin<Scrollbar>().Up.Active = scrollindex > 0;

                //    if(ScrollShift < 0)
                //    {
                //        this.SetMixinValue(ScrollableMixin.ScrollBarMaxDown, 0);
                //        this.SetMixinValue<int>(ScrollableMixin.ScrollBarIndex, 0);
                //    }
                //    else
                //    {
                //        this.SetMixinValue(ScrollableMixin.ScrollBarMaxDown, ScrollShift);
                //    }
                //};
                //jgroup.OnCollapse = () =>
                //{
                //    var scrollindex = this.GetMixinValue<int>(ScrollableMixin.ScrollBarIndex);
                //    ExpandedGroups.Remove(group.Key);
                //    Mixin<Scrollbar>().Down.Active = ScrollShift > 0;
                //    Mixin<Scrollbar>().Up.Active = scrollindex > 0;
                //    if (ScrollShift > 0)
                //    {
                //        this.SetMixinValue(ScrollableMixin.ScrollBarMaxDown, 0);
                //        this.SetMixinValue<int>(ScrollableMixin.ScrollBarIndex, 0);
                //    }
                //    else
                //    {
                //        this.SetMixinValue(ScrollableMixin.ScrollBarMaxDown, ScrollShift);
                //    }
                //};
                //jgroup.AfterExpandAnother = () =>
                //{
                //    //jgroup.Visible = !(jgroup.Top + ScrollShift > 13);
                //};
                //jgroup.AfterCollapseAnother = jgroup.AfterExpandAnother;

            }

            var shift = ScrollShift;
            foreach (var group in JournalGrouped)
            {
                var jg = JournalGroups.FirstOrDefault(j => j.GroupName == group.Key);
                if (ExpandedGroups.Contains(group.Key))
                {
                    jg?.Expand();
                }
            }
        }

        protected override Key[] KeyHandles => new Key[]
        {
            Key.Escape
        };

        public override void KeyDown(Key key, KeyModifiers modifier, bool hold)
        {
            if (key == Key.Escape)
            {
                if (currentOpenedScroll != default)
                {
                    currentOpenedScroll.Destroy();
                }
                else
                {
                    base.KeyDown(key, modifier, hold);
                }
            }
        }

        private Func<bool> SlideNeed(JournalGroup jgroup)
        {
            return () =>
            {
                if (jgroup.GroupIndex <= JournalGroup.SlideIndex)
                {
                    return false;
                }
                return JournalGroup.SlideDownCount != 0;
            };
        }
    }
}