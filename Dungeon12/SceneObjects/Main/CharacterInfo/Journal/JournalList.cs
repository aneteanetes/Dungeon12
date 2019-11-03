using Dungeon.Drawing.Impl;
using Dungeon.Drawing.SceneObjects.Map;
using Dungeon12.Drawing.SceneObjects.Main.CharacterInfo.Talants;
using System.Collections.Generic;
using Dungeon;
using Dungeon12.SceneObjects;
using System;
using Dungeon.SceneObjects;

namespace Dungeon12.Drawing.SceneObjects.Main.CharacterInfo.Journal
{
    public class JournalList : HandleSceneControl
    {
        public override bool AbsolutePosition => true;

        public override bool CacheAvailable => false;

        public bool CanDestroyParent => CanDestroyParentBinding();

        public Func<bool> CanDestroyParentBinding { get; set; } = () => true;

        public JournalList(Player playerSceneObject)
        {
            this.Image = "Dungeon12.Resources.Images.ui.vertical_title(17x12).png";

            this.Height = 17;
            this.Width = 12;

            var txt = this.AddTextCenter(new DrawText("Журнал"), true, false);
            txt.Top += 0.2;

            var tabs = new List<JournalTab>();

            var jcats = playerSceneObject.Avatar.Character.As<Dungeon12Class>().Journal.JournalCategories;

            foreach (var jcat in jcats)
            {
                var index = jcats.IndexOf(jcat);

                var tab = new JournalTab(this, jcat,this, playerSceneObject.Avatar.Character, index == 0)
                {
                    AbsolutePosition = true,
                    CacheAvailable = false,
                    Left = index * 3,
                    Top = 1.5,
                    ZIndex = this.ZIndex
                };
                tabs.Add(tab);
            }

            tabs.ForEach((tab, i) =>
            {
                this.AddChild(tab);
                if (i == 0)
                {
                    tab.Open();
                }
            });
        }
    }
}
