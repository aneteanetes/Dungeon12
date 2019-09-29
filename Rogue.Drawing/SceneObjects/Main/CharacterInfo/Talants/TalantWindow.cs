using MoreLinq;
using Rogue.Abilities.Talants.TalantTrees;
using Rogue.Control.Keys;
using Rogue.Drawing.Impl;
using Rogue.Drawing.SceneObjects.Main.CharacterInfo.Talants;
using Rogue.Drawing.SceneObjects.Map;
using Rogue.Drawing.SceneObjects.UI;
using System.Collections.Generic;

namespace Rogue.Drawing.SceneObjects.Main.CharacterInfo
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

            this.Image = "Rogue.Resources.Images.ui.vertical_title(17x12).png";

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
