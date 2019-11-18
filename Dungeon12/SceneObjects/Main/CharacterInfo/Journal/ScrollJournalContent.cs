using Dungeon;
using Dungeon.Control;
using Dungeon.Drawing;
using Dungeon.SceneObjects;
using Dungeon12.Entites.Journal;
using Dungeon12.SceneObjects.Main.CharacterInfo.Stats;
using System;
using System.Collections.Generic;

namespace Dungeon12.SceneObjects.Main.CharacterInfo.Journal
{
    internal class ScrollJournalContent : EmptyHandleSceneControl
    {
        public override bool AbsolutePosition => true;

        public override bool CacheAvailable => false;

        private List<TextControl> Texts = new List<TextControl>();
        private int page = 1;

        public ScrollJournalContent(JournalEntry journalEntry)
        {
            this.Width = 12;
            this.Height = 17;
            this.Image = "ui/scrollback(17x12).png".AsmImgRes();

            var titleText = new DrawText(journalEntry.Display, new DrawColor(ConsoleColor.Black)) { Size = 24 }.Triforce();
            var title = this.AddTextCenter(titleText, vertical: false);
            title.Top += .5;

            var plusTop = (MeasureText(titleText).Y / 32) + 1;

            string text = journalEntry.Text;

            while (text!="")
            {
                var txt = new DrawText(text, new DrawColor(ConsoleColor.Black), true).Montserrat();
                txt = CutText(txt, 13);
                var page = new TextControl(txt);
                page.Width = 11;
                page.Top += plusTop;
                page.Left = 0.5;
                page.Visible = false;
                Texts.Add(page);

                text=text.Replace(txt.StringData, "");
            }

            Texts.ForEach(t => this.AddChild(t));
            Texts[0].Visible = true;

            if (Texts.Count>1)
            {
                this.AddChild(new PageButton(() =>
                 {
                     if (page > 1)
                     {
                         this.Texts[page-1].Visible = false;
                         page--;
                         this.Texts[page-1].Visible = true;
                     }
                 }, "<")
                {
                    Left=10.5,
                    Top=16.5
                });
                this.AddChild(new PageButton(() =>
                {
                    if (page < Texts.Count)
                    {
                        this.Texts[page-1].Visible = false;
                        page++;
                        this.Texts[page-1].Visible = true;
                    }
                }, ">")
                {
                    Left = 11,
                    Top = 16.5
                });
            }
        }

        public class PageButton : EmptyHandleSceneControl
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

            public PageButton(Action click, string text = "+")
            {
                this.Width = .5;
                this.Height = .5;
                this.click = click;
                this.Image = "ui/checkbox/on.png".AsmImgRes();
                this.AddTextCenter(text.AsDrawText().InSize(10).Montserrat());
            }

            public override void Focus()
            {
                this.Image = "ui/checkbox/hover.png".AsmImgRes();
                base.Focus();
            }

            public override void Unfocus()
            {
                this.Image = "ui/checkbox/on.png".AsmImgRes();
                base.Unfocus();
            }

            public override void Click(PointerArgs args)
            {
                this.Image = "ui/checkbox/pressed.png".AsmImgRes();
                base.Click(args);
            }

            public override void ClickRelease(PointerArgs args)
            {
                this.Image = "ui/checkbox/on.png".AsmImgRes();
                click?.Invoke();
                base.ClickRelease(args);
            }

            public override void GlobalClickRelease(PointerArgs args)
            {
                this.Image = "ui/checkbox/on.png".AsmImgRes();
                base.ClickRelease(args);
            }
        }
    }
}
