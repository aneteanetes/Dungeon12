using Dungeon;
using Dungeon.Control.Keys;
using Dungeon.Drawing.SceneObjects.Map;
using Dungeon.Drawing.SceneObjects.UI;
using Dungeon.Events;
using Dungeon12.Drawing.SceneObjects.Main.CharacterInfo.Journal;
using Dungeon12.SceneObjects;
using Dungeon12.SceneObjects.Main.CharacterInfo.Journal;

namespace Dungeon12.Drawing.SceneObjects.Main.CharacterInfo
{
    public class JournalWindow : DraggableControl<JournalWindow>
    {
        public override bool CacheAvailable => false;

        public override bool AbsolutePosition => true;

        private readonly PlayerSceneObject playerSceneObject;

        private JournalList journalList;

        internal ScrollJournalContent scrollJournalContent;

        public JournalWindow(Player playerSceneObject) : base(default)
        {
            playerSceneObject.BlockMouse = true;
            this.Destroy += () => playerSceneObject.BlockMouse = false;
            this.playerSceneObject = playerSceneObject;
            
            this.Height = 17;
            this.Width = 12;

            this.Left = 3.5;
            this.Top = 2;

            journalList = new JournalList(playerSceneObject,this);
            this.AddChild(journalList);
        }

        protected override Key[] OverrideKeyHandles => new Key[] { Key.L };

        public override void KeyDown(Key key, KeyModifiers modifier, bool hold)
        {
            if (key == Key.L)
            {
                base.KeyDown(Key.Escape, modifier, hold);
            }

            if (key == Key.Escape && scrollJournalContent!=null)
            {
                scrollJournalContent.Destroy();
                scrollJournalContent = null;
                return;
            }

            base.KeyDown(key, modifier, hold);
        }

        public void OnEvent(ClassChangeEvent @event)
        {

        }

        protected override void CallOnEvent(dynamic obj)
        {
            OnEvent(obj);
        }
    }
}
