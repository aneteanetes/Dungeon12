using Dungeon;
using Dungeon12.SceneObjects; using Dungeon.SceneObjects;
using Dungeon.SceneObjects.Base; using Dungeon12.SceneObjects.Base;
using Dungeon.View.Interfaces;
using Dungeon12.Entites.Journal;
using Dungeon12.Entities.Quests;
using Dungeon12.SceneObjects.Main.CharacterInfo.Journal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon12.Drawing.SceneObjects.Main.CharacterInfo.Talants
{
    public class JournalTabContent : EmptyHandleSceneControl
    {
        public override bool CacheAvailable => false;

        public override bool AbsolutePosition => true;        

        private readonly List<JournalItem> AllItems = new List<JournalItem>();

        private double _left;

        public JournalTabContent(JournalCategory jcategory, double left, JournalWindow journalWindow, IQuest quest)
        {
            _left = left;
            AddScroll(left);
            AddSearch(left);
            var scrollbar = Mixin<Scrollbar>();

            ClearSlideState();

            this.Width = 12;
            this.Height = 15;

            var models = new List<JournalItemModel>();

            int i = 0;
            jcategory.Content?.GroupBy(c => c.Group).ForEach(g =>
            {
                models.Add(new JournalItemModel()
                {
                    Title = g.Key,
                    ItemIndex = i,
                    OnExpand = () =>
                    {
                        AllItems.Where(e => e.Model.Group == g.Key)
                        .ForEach(e =>
                            {
                                e.Collapsed = false;
                            });
                        scrollbar.MaxDownIndex = (int)Math.Floor(ContentSize - 12);
                        RecalculatePositions(left);
                    },
                    OnCollapse = () =>
                    {
                        AllItems.Where(e => e.Model.Group == g.Key)
                        .ForEach(e =>
                        {
                            e.Collapsed = true;
                        });
                        Mixin<Scrollbar>().ScrollToTop(ContentVisible);
                        scrollbar.MaxDownIndex = (int)Math.Floor(ContentSize - 12);
                        RecalculatePositions(left);
                    }
                });

                i++;

                foreach (var entry in g.OrderBy(z=>z.Order))
                {
                    models.Add(new JournalItemModel()
                    {
                        Title = entry.Display,
                        ItemIndex = i,
                        Group = g.Key,
                        JournalEntry = entry,
                        ShowEntryContent = scrollJournalContent =>
                        {
                            scrollJournalContent.Left += 12;

                            if (journalWindow.scrollJournalContent != null)
                            {
                                journalWindow.scrollJournalContent.Destroy();
                            }
                            journalWindow.scrollJournalContent = scrollJournalContent;
                            journalWindow.AddChild(scrollJournalContent);
                        }
                    });
                    i++;
                }
            });

            InitJournalItems(models, left);
            RecalculatePositions(left);

            scrollbar.CanDown = ContentSize >= 10;
            scrollbar.MaxDownIndex = (int)Math.Floor(ContentSize - 12);

            if(quest!=default)
            {
                var model = models.FirstOrDefault(m => m.JournalEntry?.Quest == quest);
                if(model != default)
                {
                    this.GetChildren<JournalItem>().FirstOrDefault(ji => ji.Model == model)?.Click(default);
                }
            }
        }

        private void AddScroll(double left)
        {
            this.AddMixin(new Scrollbar(13, v => RecalculatePositions(left))
            {
                Left = 10.5 - left,
                Top = 2
            });
        }

        private void AddSearch(double left)
        {
            this.AddChild(new JournalSearch(Filter)
            {
                AbsolutePosition=true,
                CacheAvailable=false,
                Left = .5-left,
                Top = 2.5
            });
        }

        private Func<bool> ContentVisible => () => AllItems.Any(ji => !ji.Collapsed && ji.Visible);

        private void Filter(string value)
        {
            value = value.Trim();

            if (string.IsNullOrWhiteSpace(value))
            {
                AllItems.Where(x => x.Model.JournalEntry == null)
                    .ForEach(i =>
                    {
                        i.Collapsed = false;
                    });
                AllItems.ForEach(i =>
                    {
                        if (JournalItem.Expanded.Contains(i.ItemIndex))
                        {
                            i.Collapse();
                            i.Expand();
                        }
                        else
                        {
                            i.Collapse();
                        }
                    });

                Mixin<Scrollbar>().MaxDownIndex = (int)Math.Floor(ContentSize - 12);
                Mixin<Scrollbar>().ScrollToTop(ContentVisible);
                RecalculatePositions(_left);
                return;
            }

            AllItems.Where(i => !i.Search(value))
                .ForEach(i =>
                {
                    i.Collapsed = true;
                });

            AllItems.Where(i => i.Search(value))
                .ForEach(i =>
                {
                    i.Collapsed = false;
                    if (i.Model.JournalEntry != null)
                    {
                        AllItems.Where(a => a.Model.Title == i.Model.Group).ForEach(a => a.Collapsed = false);
                    }
                });

            Mixin<Scrollbar>().MaxDownIndex = (int)Math.Floor(ContentSize - 12);
            RecalculatePositions(_left);
        }

        private static void ClearSlideState()
        {
            JournalItem.Expanded.Clear();
            JournalItem.SlideIndex = 0;
            JournalItem.SlideDownCount = 0;
            JournalItem.AllExpandable.Clear();
        }

        private double ContentSize => AllItems.Where(it => !it.Collapsed).Sum(g => 1.5) + 1.5;

        private void InitJournalItems(List<JournalItemModel> models, double left)
        {
            double top = 2;
            foreach (var model in models.OrderBy(x => x.ItemIndex))
            {
                var jItem = new JournalItem(model);
                jItem.SlideNeed = SlideNeed(jItem);
                jItem.Left -= left;
                jItem.Left += .5;
                jItem.Top += 2 + top;

                if (model.JournalEntry == null)
                {
                    JournalItem.AllExpandable.Add(jItem);
                }
                else
                {
                    jItem.Collapsed = true;
                }

                this.AddChild(jItem);
                AllItems.Add(jItem);
                top += 1.5;
            }
        }

        private void RecalculatePositions(double left)
        {
            var scrollIndex = this.Mixin<Scrollbar>().ScrollIndex;

            double top = 2 - (scrollIndex * 1.5);
            foreach (var jItem in AllItems.Where(ji => !ji.Collapsed).OrderBy(x => x.ItemIndex))
            {
                jItem.Top = 2 + top;
                top += 1.5;
            }
        }

        private Func<bool> SlideNeed(JournalItem jgroup)
        {
            return () =>
            {
                if (jgroup.ItemIndex <= JournalItem.SlideIndex)
                {
                    return false;
                }
                return JournalItem.SlideDownCount != 0;
            };
        }
    }
}