﻿using Dungeon;
using Dungeon.Classes;
using Dungeon.Drawing.SceneObjects.UI;
using Dungeon12.Entities.Quests;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon12.SceneObjects.UI
{
    public class Dungeon12PlayerUI : PlayerUI
    {
        public Dungeon12PlayerUI(Character player) : base(player)
        {
            player.As<Dungeon12Class>()
                .ActiveQuests
                .Where(aq => aq.Discover)
                .ForEach(AddDiscover);
        }

        public void OnEvent(QuestDiscoverEvent questGetEvent)
        {
            AddDiscover(questGetEvent.Quest);
        }

        protected override void CallOnEvent(dynamic obj) => this.OnEvent(obj);

        private const double space = 2;
        private double currentTop = 3.5;

        public void AddDiscover(IQuest quest)
        {
            this.AddChildCenter(new QuestDescoverSceneObject(quest)
            {
                Top = currentTop,
                Left = .25
            }, false, false).Destroy += () =>
              {
                  currentTop -= space;
                  Recalculate();
              };

            currentTop += space;
        }

        private void Recalculate()
        {
            this.GetChildren<QuestDescoverSceneObject>().ForEach(qdc =>
            {
                if (qdc.Top > currentTop + 3.5)
                    qdc.Top -= space;
            });
        }
    }
}