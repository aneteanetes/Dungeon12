using Dungeon;
using Dungeon.Control;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon12.ECS.Components;
using Dungeon12.Entities.Quests;
using System;

namespace Dungeon12.SceneObjects.RegionScreen
{
    public class QuestBar : SceneControl<QuestBook>
    {
        public QuestBar(QuestBook component) : base(component)
        {
            this.Left = 38;
            this.Top = 40;

            var top = 0d;

            foreach (var q in component.Quests)
            {
                var qplate = this.AddChild(new QuestPlate(q));
                qplate.Top = top;
                top += qplate.Height + 5d;
            }
        }

        private class QuestPlate : SceneControl<Quest>
        {
            public QuestPlate(Quest component) : base(component)
            {
                this.Width = 289;
                this.Height = component.Goals.Count * 32;

                this.Image = "UI/layout/qbar/back.png".AsmImg();

                this.AddChild(new PlateButton("i")
                {
                    Left = 247,
                    Top = 7
                });

                this.AddChild(new PlateButton("x")
                {
                    Left = 266,
                    Top = 7
                });

                this.AddChild(new ImageObject("UI/layout/qbar/line.png")
                {
                    Left = 6,
                    Top = 25
                });

                var title = this.AddTextCenter(component.Name.AsDrawText().SegoeUIBold().InSize(9).InColor(Global.CommonColor), false, false);
                title.Left = 7;
                title.Top = 5;

                var top = 27;
                foreach (var goal in component.Goals)
                {
                    this.AddChild(new GoalLine(goal)
                    {
                        Left = 8,
                        Top = top
                    });
                    top += 20;
                }
            }

            private class GoalLine : SceneObject<Goal>
            {
                private TextObject _counter;

                public GoalLine(Goal component) : base(component)
                {
                    this.Width = 289;
                    this.Height = 20;

                    this.AddTextCenter(component.Text.AsDrawText().SegoeUIBold().InSize(10).InColor(Global.CommonColor), false);

                    var counter = $"{component.Max}/{component.Max}".AsDrawText().SegoeUIBold().InSize(10).InColor(Global.CommonColor);
                    var measure = MeasureText(counter);

                    var actualCounter = counter.Copy();
                    actualCounter.SetText($"{component.Value}/{component.Max}");
                    _counter = this.AddTextCenter(actualCounter);
                    _counter.Left = 284 - 12 - measure.X;
                }
            }

            private class PlateButton : EmptySceneControl, ITooltiped
            {
                private string _img;
                public PlateButton(string img)
                {
                    _img = img;
                    this.Width = 15;
                    this.Height = 15;
                    this.Image = $"UI/layout/qbar/{img}.png".AsmImg();
                    TooltipText = (img == "i" ? "Открыть в журнале" : "Закрыть").AsDrawText();
                }

                public override void Focus()
                {
                    this.Image = $"UI/layout/qbar/{_img}a.png".AsmImg();
                    base.Focus();
                }

                public override void Unfocus()
                {
                    this.Image = $"UI/layout/qbar/{_img}.png".AsmImg();
                    base.Unfocus();
                }

                public Action OnClick { get; set; }

                public Dungeon.View.Interfaces.IDrawText TooltipText { get; set; }

                public bool ShowTooltip => true;

                public override void Click(PointerArgs args)
                {
                    OnClick?.Invoke();
                    base.Click(args);
                }
            }
        }
    }
}
