using Dungeon;
using Dungeon.SceneObjects;
using Dungeon.SceneObjects.Base;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon12.SceneObjects.SaveLoad
{
    public class SaveLoadWindow : EmptyHandleSceneControl
    {
        private bool isSave;

        public SaveLoadWindow(bool isSave)
        {
            Image = "ui/vertical17x24.png".AsmImgRes();
            this.Width = 24;
            this.Height = 17;

            this.AddMixin(new Scrollbar(17, v => RecalculatePositions())
            {
                Left = 23,
            });

            Dungeon.Data.Database.SavedGames().ForEach((savedGame,i) =>
            {
                var slot = new SaveLoadSlot(savedGame)
                {
                    ItemIndex = i
                };

                Slots.Add(slot);
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