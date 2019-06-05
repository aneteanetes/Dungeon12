namespace Rogue.Drawing.SceneObjects.Dialogs.NPC
{
    using Rogue.Control.Keys;
    using Rogue.Conversations;
    using Rogue.Drawing.SceneObjects.Base;
    using Rogue.View.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class NPCDialogue :  HandleSceneControl
    {
        public override bool AbsolutePosition => true;

        private SubjectPanel subjectPanel;
        private AnswerPanel answerPanel;

        public NPCDialogue(Rogue.Map.Objects.NPC npc, Action<ISceneObject> destroyBinding, Action<ISceneObjectControl> controlBinding)
        {
            Global.FreezeWorld = this;

            answerPanel = new AnswerPanel() { DestroyBinding = destroyBinding, ControlBinding= controlBinding };
            subjectPanel = new SubjectPanel(npc, answerPanel.Select,this.ExitDialogue);

            this.AddChild(subjectPanel);
            this.AddChild(answerPanel);

            this.Height = 22.5;
            this.Width = 40;
        }

        public override void KeyDown(Key key, KeyModifiers modifier, bool hold)
        {
            if (key == Key.Escape)
            {
                ExitDialogue();
            }
        }

        private void ExitDialogue()
        {
            this.Destroy?.Invoke();
            Global.FreezeWorld = null;
        }

        protected override Key[] KeyHandles => new Key[] { Key.Escape };
    }
}
