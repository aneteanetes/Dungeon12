using Dungeon.Control.Keys;
using Dungeon.SceneObjects;
using Dungeon12.SceneObjects.Stats;

namespace Dungeon12.SceneObjects.RegionScreen
{
    internal class ButtonPanel : EmptySceneControl
    {
        public ButtonPanel()
        {
            this.Width = 339;
            this.Height = 164;

            this.Left = 1565;
            this.Top = 887;

            this.AddChild(new ControlButton('e', "Персонажи (I)", HeroWindow));
            this.AddChild(new ControlButton('q', "Журнал", QuestWindow)
            {
                Left=88
            });
            this.AddChild(new ControlButton('p', "Книга порталов", QuestWindow, !Global.Game.Party.PortalsActive)
            {
                Left = 176
            });
            this.AddChild(new ControlButton('r', "Руны", RunesWindow)
            {
                Left = 264
            });


            this.AddChild(new ControlButton('s', "Инвентарь", CraftWindow)
            {
                Top = 89
            });
            this.AddChild(new ControlButton('f', "Заметки", JournalWindow)
            {
                Left = 88,
                Top = 89
            });
            this.AddChild(new ControlButton('c', "Профессии", CraftWindow)
            {
                Left = 176,
                Top = 89
            });
            this.AddChild(new ControlButton('o', "Меню", EscMenu)
            {
                Left = 264,
                Top = 89
            });
        }

        private void CraftWindow()
        {

        }

        public override void KeyDown(Key key, KeyModifiers modifier, bool hold)
        {
            if (key== Key.I)
                HeroWindow();
            base.KeyDown(key, modifier, hold);
        }

        private void HeroWindow()
        {
            Global.Windows.Activate<StatsWindow>(this.Layer);
        }

        private void QuestWindow()
        {

        }

        private void RunesWindow()
        {

        }

        private void JournalWindow()
        {

        }

        private void EscMenu()
        {

        }
    }
}
