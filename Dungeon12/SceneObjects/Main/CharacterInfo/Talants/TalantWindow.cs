using Dungeon.Control.Keys;
using Dungeon.Drawing;
using Dungeon.Drawing.SceneObjects.Map;
using Dungeon.Drawing.SceneObjects.UI;
using Dungeon.Events;
using Dungeon12.Drawing.SceneObjects.Main.CharacterInfo.Talants;
using MoreLinq;
using System.Collections.Generic;

namespace Dungeon12.Drawing.SceneObjects.Main.CharacterInfo
{
    public class TalantWindow : DraggableControl<TalantWindow>
    {
        public override bool CacheAvailable => false;

        public override bool AbsolutePosition => true;

        private readonly PlayerSceneObject playerSceneObject;

        public TalantWindow(PlayerSceneObject playerSceneObject)
        {
            playerSceneObject.BlockMouse = true;
            this.Destroy += () => playerSceneObject.BlockMouse = false;
            this.playerSceneObject = playerSceneObject;

            this.Image = "Dungeon12.Resources.Images.ui.vertical_title(17x12).png";

            this.Height = 17;
            this.Width = 12;

            this.Left = 22.5;
            this.Top = 2;

            var txt = this.AddTextCenter(new DrawText("Таланты"), true, false);
            txt.Top += 0.2;

            var talantTrees = playerSceneObject.GetTalantTrees();

            var tabs = new List<TalantTreeTab>();

            foreach (var talantTree in talantTrees)
            {
                var index = talantTrees.IndexOf(talantTree);

                var tab = new TalantTreeTab(this, talantTree,playerSceneObject.Avatar.Character, index == 0)
                {
                    AbsolutePosition = true,
                    CacheAvailable = false,
                    Left = index * 3,
                    Top = 1.5,
                    ZIndex = this.ZIndex
                };
                tabs.Add(tab);
            }

            TalantTreeTab.Flex(tabs.ToArray());
            tabs.ForEach((tab,i) =>
            {
                this.AddChild(tab);
                if (i == 0)
                {
                    tab.Open();
                }
            });
        }

        protected override Key[] OverrideKeyHandles => new Key[] { Key.V, Key.X };

        public override void KeyDown(Key key, KeyModifiers modifier, bool hold)
        {
            if (key == Key.V || key == Key.X)
            {
                base.KeyDown(Key.Escape, modifier, hold);
            }

            base.KeyDown(key, modifier, hold);
        }
    }
}
