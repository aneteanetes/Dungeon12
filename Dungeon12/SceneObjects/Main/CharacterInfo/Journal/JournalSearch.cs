using Dungeon;
using Dungeon.Control;
using Dungeon.Drawing;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using System;

namespace Dungeon12.SceneObjects.Main.CharacterInfo.Journal
{
    public class JournalSearch : EmptyHandleSceneControl
    {
        public JournalSearch(Action<string> doFilter)
        {
            Width = 10;
            Height = 1;

            var input = new TextInputControl(new DrawText("").Montserrat(), 24, width: 8, height: 1, autofocus: false, onEnterOnBlur: true)
            {
                OnEnter = text => doFilter(text)
            };
            this.AddChild(input);
            this.AddChild(new SearchButton(input, doFilter)
            {
                Left = 8,
                AbsolutePosition = true,
                CacheAvailable = false
            });
            this.AddChild(new CrossButton(input, doFilter)
            {
                Left = 9,
                AbsolutePosition = true,
                CacheAvailable = false
            });
        }

        private class CrossButton : EmptyHandleSceneControl
        {
            private Action<string> _doFilter;
            private TextInputControl _textInput;
            public CrossButton(TextInputControl textInput, Action<string> doFilter)
            {
                this.Width = 1;
                this.Height = 1;

                _doFilter = doFilter;
                _textInput = textInput;
                Image = "ui/checkbox/on.png".AsmImgRes();
                this.AddChildImageCenter(new ImageControl("ui/cancelicon.png".AsmImgRes())
                {
                    AbsolutePosition = true,
                    CacheAvailable = false
                });
            }

            public override void Click(PointerArgs args)
            {
                _textInput.focus = false;
                _textInput.Value = "";
                _doFilter?.Invoke(_textInput.Value);
                base.Click(args);
            }

            public override void Focus()
            {
                Image = "ui/checkbox/hover.png".AsmImgRes();
                base.Focus();
            }

            public override void Unfocus()
            {
                Image = "ui/checkbox/on.png".AsmImgRes();
                base.Unfocus();
            }
        }

        private class SearchButton : EmptyHandleSceneControl
        {
            private Action<string> _doFilter;
            private TextInputControl _textInput;
            public SearchButton(TextInputControl textInput, Action<string> doFilter)
            {
                this.Width = 1;
                this.Height = 1;

                _doFilter = doFilter;
                _textInput = textInput;
                Image = "ui/checkbox/on.png".AsmImgRes();
                this.AddChildImageCenter(new ImageControl("ui/searchicon.png".AsmImgRes())
                {
                    AbsolutePosition = true,
                    CacheAvailable = false
                });
            }

            public override void Click(PointerArgs args)
            {
                _textInput.focus = false;
                _doFilter?.Invoke(_textInput.Value);
                base.Click(args);
            }

            public override void Focus()
            {
                Image= "ui/checkbox/hover.png".AsmImgRes();
                base.Focus();
            }

            public override void Unfocus()
            {
                Image = "ui/checkbox/on.png".AsmImgRes();
                base.Unfocus();
            }
        }
    }
}
