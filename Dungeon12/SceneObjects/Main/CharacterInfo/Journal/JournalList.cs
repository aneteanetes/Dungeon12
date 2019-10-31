using Dungeon.Drawing.Impl;
using Dungeon.Drawing.SceneObjects;
using Dungeon.Drawing.SceneObjects.Map;
using Dungeon12.Drawing.SceneObjects.Main.CharacterInfo.Talants;
using System.Collections.Generic;
using Dungeon;

namespace Dungeon12.Drawing.SceneObjects.Main.CharacterInfo.Journal
{
    public class JournalList : HandleSceneControl
    {
        public override bool AbsolutePosition => true;

        public override bool CacheAvailable => false;

        public JournalList(PlayerSceneObject playerSceneObject)
        {
            this.Image = "Dungeon.Resources.Images.ui.vertical_title(17x12).png";

            this.Height = 17;
            this.Width = 12;

            var txt = this.AddTextCenter(new DrawText("Журнал"), true, false);
            txt.Top += 0.2;

            var tabs = new List<JournalTab>();

            var jcats = playerSceneObject.Avatar.Character.Journal.JournalCategories;

            foreach (var jcat in jcats)
            {
                var index = jcats.IndexOf(jcat);

                var tab = new JournalTab(this, jcat, playerSceneObject.Avatar.Character, index == 0)
                {
                    AbsolutePosition = true,
                    CacheAvailable = false,
                    Left = index * 3,
                    Top = 1.5,
                    ZIndex = this.ZIndex
                };
                tabs.Add(tab);
            }

            JournalTab.Flex(tabs.ToArray());
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
