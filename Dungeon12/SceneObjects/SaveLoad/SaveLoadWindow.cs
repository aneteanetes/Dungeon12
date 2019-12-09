using Dungeon;
using Dungeon12.SceneObjects; using Dungeon.SceneObjects;
using Dungeon.SceneObjects.Base; using Dungeon12.SceneObjects.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon12.SceneObjects.SaveLoad
{
    public class SaveLoadWindow : EmptyHandleSceneControl
    {
        private bool _isSave;
        private Action _switchMain;

        public SaveLoadWindow(bool isSave, Action switchMain)
        {
            _switchMain = switchMain;
            _isSave = isSave;

            Image = "ui/vertical(17x24).png".AsmImgRes();
            this.Width = 24;
            this.Height = 17;

            ReDraw();
        }

        public void ReDraw()
        {
            Slots.Clear();
            this.ClearChildrens();


            this.AddMixin(new Scrollbar(16, v => RecalculatePositions())
            {
                Left = 22.5,
                Top = .5,
            });

            double top = 1;
            if (_isSave)
            {
                var emptySaveSlot = new SaveLoadSlot(default, _isSave, _switchMain, this)
                {
                    ItemIndex = -1,
                    Left = .5,
                    Top = top
                };

                top += 3;

                Slots.Add(emptySaveSlot);
                this.AddControlCenter(emptySaveSlot, false, false);
            }

            Global.SavedGames().ForEach((savedGame, i) =>
            {
                var slot = new SaveLoadSlot(savedGame, _isSave, _switchMain, this)
                {
                    ItemIndex = i,
                    Left = .5,
                    Top = top
                };

                top += 3;

                Slots.Add(slot);
                this.AddControlCenter(slot, false, false);
            });

            var scrollbar = this.Mixin<Scrollbar>();
            scrollbar.CanDown = Slots.Count > 5;
            scrollbar.MaxDownIndex = (Slots.Count - 5);
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