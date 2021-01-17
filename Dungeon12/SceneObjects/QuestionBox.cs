using Dungeon;
using Dungeon.GameObjects;
using Dungeon12.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using Dungeon12.Drawing.SceneObjects;
using System;
using System.Collections.Generic;
using Dungeon.SceneObjects.Base;

namespace Dungeon12.SceneObjects
{
    public class QuestionBox : HandleSceneControl<QuestionBoxModel>
    {
        public override bool AbsolutePosition => true;

        public override bool CacheAvailable => false;

        public QuestionBox(QuestionBoxModel component, bool bindView = true) : base(component, bindView)
        {
            this.Image = "ui/vertical(17x24).png".AsmImg();
            this.Width = 16;
            this.Height = 7;
            this.Left = 40d / 2d - 16d / 2d;
            this.Top = 22.5d / 2d - 7d / 2d;

            var question = this.AddTextCenter(component.Text.AsDrawText().WithWordWrap().Montserrat().InSize(14), true, true);
            question.Width = 15;
            question.Left = 1;
            question.Top -= 2;

            var yesText = component.YesText.AsDrawText().InSize(14).Montserrat();
            var noText = component.NoText.AsDrawText().InSize(14).Montserrat();

            ISceneControl yes = default;
            if (yesText.Length <= 3)
            {
                var smallBtn = new SmallMetallButtonControl(yesText);
                smallBtn.OnClick = () =>
                {
                    this.Destroy?.Invoke();
                    component.Yes?.Invoke();
                };
                yes = smallBtn;
            }
            else
            {
                var bigBtn = new MetallButtonControl(yesText);
                bigBtn.OnClick = () =>
                {
                    this.Destroy?.Invoke();
                    component.Yes?.Invoke();
                };
                yes = bigBtn;
            }


            var yesBtn = this.AddControlCenter(yes);
            yesBtn.Top = 4;
            yesBtn.Left = 2;

            ISceneControl no = default;
            if (noText.Length <= 3)
            {
                var smallBtn = new SmallMetallButtonControl(noText);
                smallBtn.OnClick = () =>
                {
                    this.Destroy?.Invoke();
                    component.No?.Invoke();
                };
                no = smallBtn;
            }
            else
            {
                var bigBtn = new MetallButtonControl(noText);
                bigBtn.OnClick = () =>
                {
                    this.Destroy?.Invoke();
                    component.No?.Invoke();
                };
                no = bigBtn;
            }

            var noBtn = this.AddControlCenter(no);
            noBtn.Top = 4;
            noBtn.Left = 9;

            this.Destroy += () =>
            {
                Global.Freezer.World = null;
            };
        }

        public static QuestionBox Show(QuestionBoxModel model) => Show(model, Global.SceneManager.CurrentScene.ShowEffectsBinding);

        public static QuestionBox Show(QuestionBoxModel model, Action<List<ISceneObject>> publisher)
        {
            var questionBox = new QuestionBox(model);
            Global.Freezer.World = questionBox;
            publisher?.Invoke(questionBox.InList<ISceneObject>());
            return questionBox;
        }
    }

    public class QuestionBoxModel : GameComponent
    {
        public string Text { get; set; }

        public string YesText { get; set; } = "Да";

        public string NoText { get; set; } = "Нет";

        public Action Yes { get; set; }

        public Action No { get; set; }
    }
}
