using Dungeon;
using Dungeon.Control;
using Dungeon.Control.Pointer;
using Dungeon.Drawing;
using Dungeon12.Drawing.SceneObjects.Map;
using Dungeon12.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using Dungeon12.Entites.Journal;
using Dungeon12.SceneObjects.Main.CharacterInfo.Stats;
using System;
using System.Collections.Generic;

namespace Dungeon12.SceneObjects.Main.CharacterInfo.Journal
{
    internal class ScrollJournalContent : EmptySceneControl
    {
        public override bool AbsolutePosition => true;

        public override bool CacheAvailable => false;

        private List<TextControl> Texts = new List<TextControl>();
        private int page = 1;

        protected override ControlEventType[] Handles { get; } = new ControlEventType[]
        {
             ControlEventType.MouseWheel
        };

        public ScrollJournalContent(JournalEntry journalEntry)
        {
            this.Width = 12;
            this.Height = 17;
            this.Image = "ui/scrollback(17x12).png".AsmImg();

            var titleText = new DrawText(journalEntry.Display, new DrawColor(ConsoleColor.Black)) { Size = 24 }.Triforce();
            var title = this.AddTextCenter(titleText, vertical: false);
            title.Top += .5;

            var plusTop = (MeasureText(titleText).Y / 32) + 1;

            string text = journalEntry.Text;

            while (text != "")
            {
                var txt = new DrawText(text, new DrawColor(ConsoleColor.Black), true).Montserrat();
                txt = CutText(txt, 13);
                var page = new TextControl(txt);
                page.Width = 11;
                page.Top += plusTop;
                page.Left = 0.5;
                page.Visible = false;
                Texts.Add(page);

                text = text.Replace(txt.StringData, "");
            }

            Texts.ForEach(t => this.AddChild(t));
            Texts[0].Visible = true;

            if(journalEntry.Quest!=default)
            {
                var visualProgress = journalEntry.Quest.Visual();
                visualProgress.Left = 2.5;
                visualProgress.Top = 14;
                this.AddChild(visualProgress);

                var visualReward = journalEntry.Quest.Reward.Visual().As<ISceneControl>();
                visualReward.Top = 15;
                this.AddControlCenter(visualReward,false,false);
            }

            if (Texts.Count>1)
            {
                this.AddChild(new PageButton(PageBack, "<","Назад")
                {
                    Left = 10.5,
                    Top = 16
                });
                this.AddChild(new PageButton(PageForward, ">","Вперёд")
                {
                    Left = 11,
                    Top = 16
                });
            }
        }

        private void PageForward()
        {
            if (page < Texts.Count)
            {
                this.Texts[page - 1].Visible = false;
                page++;
                this.Texts[page - 1].Visible = true;
            }
        }

        private void PageBack()
        {
            if (page > 1)
            {
                this.Texts[page - 1].Visible = false;
                page--;
                this.Texts[page - 1].Visible = true;
            }
        }

        public override void MouseWheel(MouseWheelEnum mouseWheelEnum)
        {
            if(mouseWheelEnum== MouseWheelEnum.Up)
            {
                PageBack();
            }
            else
            {
                PageForward();
            }
        }
    }

    public class PageButton : EmptyTooltipedSceneObject
    {
        public override bool AbsolutePosition => true;

        public override bool CacheAvailable => false;

        private Action click;

        protected override ControlEventType[] Handles { get; } = new ControlEventType[]
        {
                ControlEventType.Click,
                ControlEventType.ClickRelease,
                ControlEventType.GlobalClickRelease,
                ControlEventType.Focus,
        };

        public PageButton(Action click, string text = "+",string tooltip=""):base(tooltip)
        {
            this.Width = .5;
            this.Height = .5;
            this.click = click;
            this.Image = "ui/checkbox/on.png".AsmImg();
            this.AddTextCenter(text.AsDrawText().InSize(10).Montserrat());
        }

        public override void Focus()
        {
            this.Image = "ui/checkbox/hover.png".AsmImg();
            base.Focus();
        }

        public override void Unfocus()
        {
            this.Image = "ui/checkbox/on.png".AsmImg();
            base.Unfocus();
        }

        public override void Click(PointerArgs args)
        {
            this.Image = "ui/checkbox/pressed.png".AsmImg();
            base.Click(args);
        }

        public override void ClickRelease(PointerArgs args)
        {
            this.Image = "ui/checkbox/on.png".AsmImg();
            click?.Invoke();
            base.ClickRelease(args);
        }

        public override void GlobalClickRelease(PointerArgs args)
        {
            this.Image = "ui/checkbox/on.png".AsmImg();
            base.ClickRelease(args);
        }
    }
}
