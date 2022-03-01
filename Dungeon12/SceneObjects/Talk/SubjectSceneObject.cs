using Dungeon;
using Dungeon.Control;
using Dungeon.Drawing;
using Dungeon.SceneObjects;
using Dungeon12.Entities.Talks;
using System.Collections.Generic;

namespace Dungeon12.SceneObjects.Talk
{
    public class SubjectSceneObject : SceneControl<Subject>
    {
        private TextControl textControl;
        private static List<SubjectSceneObject> subjects = new List<SubjectSceneObject>();

        public SubjectSceneObject(Subject component) : base(component)
        {
            var text = component.Name.AsDrawText().Gabriela().InSize(20);
            var measure = DungeonGlobal.DrawClient.MeasureText(text);
            this.Width = measure.X + 5;
            this.Height = measure.Y + 5;
            textControl = this.AddTextCenter(text, vertical: false);

            subjects.Add(this);
            this.Destroy += () => subjects.Remove(this);

            component.End = () => End();
        }

        private bool isblocked;

        public void Block()
        {
            isblocked = true;
            textControl.Text.ForegroundColor = DrawColor.Gray;
        }

        public void Unblock()
        {
            isblocked = false;
            textControl.Text.ForegroundColor = DrawColor.White;
        }

        public override void Focus()
        {
            if (isblocked)
                return;

            textControl.Text.ForegroundColor = DrawColor.Yellow;

            base.Focus();
        }

        public override void Unfocus()
        {
            if (isblocked)
                return;

            textControl.Text.ForegroundColor = DrawColor.White;

            base.Unfocus();
        }

        public override void Click(PointerArgs args)
        {
            if (isblocked)
                return;

            foreach (var subj in subjects)
            {
                subj.Block();
            }

            textControl.Text.ForegroundColor = DrawColor.Yellow;

            MainTextSceneObject.Instance.Start(Component);

            
            base.Click(args);
        }

        public void End()
        {
            this.Destroy?.Invoke();
            foreach (var subj in subjects)
            {
                subj.Unblock();
            }

            Global.Game.State.Dialogues.Add(Component.SubjectId);
        }
    }
}