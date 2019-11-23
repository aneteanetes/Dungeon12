using Dungeon;
using Dungeon.Control;
using Dungeon.Drawing;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon12.Drawing.SceneObjects.Main.CharacterBar;
using Dungeon12.Entities.Quests;
using Dungeon12.SceneObjects.Main.CharacterInfo.Journal;

namespace Dungeon12.SceneObjects.UI
{
    public class QuestDescoverSceneObject : HandleSceneControl<IQuest>
    {
        public override bool CacheAvailable => false;

        private TextControl titleControl;

        private bool _questBook;

        public QuestDescoverSceneObject(IQuest component, bool questBook = false) : base(component, false)
        {
            _questBook = questBook;
            this.Image = "ui/journal/descoverback.png".AsmImgRes();
            this.Width = 7;
            this.Height = .75;

            this.AddChild(new DarkRectangle()
            {
                Width = questBook ? 12 : 7,
                Left = questBook ? -2.5 : 0,
                Height = 2,
                Top = -1
            });
            titleControl = this.AddTextCenter(TitleText, false, false);
            titleControl.Top -= 1;

            this.AddChild(new ProgressQuestDiscover(component));

            var closeBtn = this.AddControlCenter(new PageButton(() => Discover(questBook), questBook ? "!" : "x", questBook ? "Отслеживать" : "Закрыть"), false, false);

            closeBtn.Top = -1;
            closeBtn.Left = 6.5;

            if (!questBook)
            {
                var openJournal = this.AddControlCenter(new PageButton(() =>
                {
                    JournalButton.OpenQuest(component);
                },"?", "Журнал"), false, false);

                openJournal.Top = -.5;
                openJournal.Left = 6.5;
            }
        }

        private void Discover(bool questBook)
        {
            if (!questBook)
            {
                Component.Discover = false;
                this.Destroy?.Invoke();
            }
            else 
            {
                if (Component.Discover)
                {
                    MessageBox.Show("Задание уже отслеживается!", this.ShowEffects);
                }
                else
                {
                    Component.Discover = true;
                    Global.Events.Raise(new QuestDiscoverEvent(Component));
                }
            }
        }

        private DrawText TitleText => ($"{(_questBook ? "Прогресс" : Component.Name)}: {Component.Progress}/{Component.MaxProgress}").AsDrawText().Montserrat();

        public override void Update()
        {
            titleControl.Text.SetText(TitleText.StringData);
        }

        private class ProgressQuestDiscover : SceneObject<IQuest>
        {
            public override bool CacheAvailable => false;

            public ProgressQuestDiscover(IQuest component) : base(component, false)
            {
                this.Image = "ui/journal/descoverprogress.png".AsmImgRes();
                this.Left = .5;
                this.Height = .75;
            }

            public override double Width => 7 * (((double)Component.Progress / Component.MaxProgress * 100) / 100); //7
        }
    }
}