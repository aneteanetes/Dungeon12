using Dungeon.Control;
using Dungeon.Drawing.SceneObjects;
using Nabunassar.Entities;
using Nabunassar.SceneObjects.Base;
using Nabunassar.Scenes.Creating.Heroes;

namespace Nabunassar.Scenes.Creating.Character
{
    internal class CreatePartCube : EmptySceneControl
    {
        protected override ControlEventType[] Handles => [ControlEventType.Focus, ControlEventType.Click];

        private CreatePart _part;
        private CreateHeroScene _scene;
        private string _hint;

        public CreatePartCube Next;

        public CreatePartCube(string img, string text, string activatehint, CreatePart showPart, CreateHeroScene scene)
        {
            showPart.Cube = this;
            Visible = false;

            _hint=activatehint;
            _part = showPart;
            _scene=scene;

            this.Height = 125;
            this.Width = 125;


            this.AddBorderMapBack(new BorderConfiguration()
            {
                ImagesPath = "UI/bordermin/bord7.png",
                Size = 16,
                Padding = 2,
                Opacity = .95
            });

            this.AddChild(new ImageObject(img) { Width = 125, Height = 125, Color = Global.CommonColor });

            var txt = this.AddTextCenter(text.DefaultTxt(30));
            txt.Top = this.Height;
        }

        public override void Focus()
        {
            this.SetCursor(SceneObjects.Cursors.Cursor.Pointer);
        }

        public override void Unfocus()
        {
            this.SetCursor(SceneObjects.Cursors.Cursor.Normal);
        }

        public override void Click(PointerArgs args)
        {
            if (_scene.ActivePart != _part)
            {
                if (_scene.ActivePart != null)
                    _scene.ActivePart.Visible = false;

                _scene.ActivePart = _part;
                _scene.ActivePart.Visible = true;
            }
            Global.Game.Creation.Hint = _hint;

            base.Click(args);
        }

        private void AccessNext()
        {
            if (Next != null)
                Next.Visible = true;
        }

    }
}
