namespace Rogue.Drawing.SceneObjects.Dialogs.NPC
{
    using Rogue.Control.Keys;
    using Rogue.Drawing.SceneObjects.Base;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class NPCDialogue :  HandleSceneControl
    {
        public override bool AbsolutePosition => true;

        private SubjectPanel subjectPanel;
        private AnswerPanel answerPanel;

        public NPCDialogue(Rogue.Map.Objects.NPC npc)
        {
            Global.FreezeWorld = this;

            subjectPanel = new SubjectPanel(npc);
            answerPanel = new AnswerPanel();

            this.AddChild(subjectPanel);
            this.AddChild(answerPanel);
        }

        public override void KeyDown(Key key, KeyModifiers modifier, bool hold)
        {
            if (key == Key.Escape)
            {
                this.Destroy?.Invoke();
                Global.FreezeWorld = null;
            }
        }
    }
}
