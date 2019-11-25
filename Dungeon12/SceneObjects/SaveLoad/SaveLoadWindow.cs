using Dungeon;
using Dungeon.SceneObjects;
using Dungeon.SceneObjects.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon12.SceneObjects.SaveLoad
{
    public class SaveLoadWindow : EmptyHandleSceneControl
    {
        private bool isSave;

        public SaveLoadWindow(bool isSave, Action switchMain)
        {
            Image = "ui/vertical(17x24).png".AsmImgRes();
            this.Width = 24;
            this.Height = 17;

            this.AddMixin(new Scrollbar(16, v => RecalculatePositions())
            {
                Left = 22.5,
                Top=.5,
            });

            double top = .5;
            Dungeon.Data.Database.SavedGames().ForEach((savedGame,i) =>
            {
                var slot = new SaveLoadSlot(savedGame, isSave, switchMain)
                {
                    ItemIndex = i,
                    Left=.5,
                    Top=top
                };

                top += 3;

                Slots.Add(slot);
                this.AddControlCenter(slot, false, false);
            });
        }

        private List<SaveLoadSlot> Slots = new List<SaveLoadSlot>();

        private const double oneScrollSize = 3;

        private void RecalculatePositions()
        {
            var scrollIndex = this.Mixin<Scrollbar>().ScrollIndex;

            double top = (scrollIndex * oneScrollSize);
            foreach (var slotItem in Slots.OrderBy(x => x.ItemIndex))
            {
                slotItem.Top = top;
                top += oneScrollSize;
            }
        }
    }
}