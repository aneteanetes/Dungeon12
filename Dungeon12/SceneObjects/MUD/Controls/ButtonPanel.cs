using Dungeon;
using Dungeon.Control.Keys;
using Dungeon12.SceneObjects.Base;
using Dungeon12.SceneObjects.Stats;

namespace Dungeon12.SceneObjects.MUD.Controls
{
    internal class ButtonPanel : EmptySceneControl
    {
        public override void Throw(Exception ex)
        {
            throw ex;
        }

        public ButtonPanel()
        {
            Width = 400;
            Height = 250;

            this.Image="UI/mud/btnsback.jpg".AsmImg();

            this.AddBorderBack();

            AddChild(new ControlButton('e', "Персонажи (I)", HeroWindow)
            {
                Left=25,
                Top=40
            });
            AddChild(new ControlButton('q', "Журнал", QuestWindow)
            {
                Left = 120,
                Top=40
            });
            AddChild(new ControlButton('p', "Книга порталов", QuestWindow, !Global.Game.Party.PortalsActive)
            {
                Left = 210,
                Top=40
            });
            AddChild(new ControlButton('r', "Руны", RunesWindow)
            {
                Left = 300,
                Top=40
            });


            AddChild(new ControlButton('s', "Инвентарь", CraftWindow)
            {
                Top = 150,
                Left = 25
            });
            AddChild(new ControlButton('f', "Заметки", JournalWindow)
            {
                Left = 120,
                Top = 150
            });
            AddChild(new ControlButton('c', "Профессии", CraftWindow)
            {
                Left = 210,
                Top = 150
            });
            AddChild(new ControlButton('o', "Меню", EscMenu)
            {
                Left = 300,
                Top = 150
            });
        }

        private void CraftWindow()
        {

        }

        public override void KeyDown(Key key, KeyModifiers modifier, bool hold)
        {
            if (key == Key.I)
                HeroWindow();
            base.KeyDown(key, modifier, hold);
        }

        private void HeroWindow()
        {
            Global.Windows.Activate<StatsWindow>(Layer);
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
