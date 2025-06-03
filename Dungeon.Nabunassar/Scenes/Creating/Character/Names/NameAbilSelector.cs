using Dungeon.Control;
using Dungeon.Drawing.SceneObjects;
using Nabunassar.Entities;
using Nabunassar.SceneObjects.Base;

namespace Nabunassar.Scenes.Creating.Character.Names
{
    internal class NameAbilSelector : SceneControl<Hero>
    {
        protected override ControlEventType[] Handles => [ControlEventType.Focus, ControlEventType.Click];

        NameDescriptionBlock _descBlock;
        int _abilinameIdx;
        string _hint;

        public NameAbilSelector(Hero component, string icon, string hint, int abilinameIdx, NameDescriptionBlock descBlock) : base(component)
        {
            _abilinameIdx = abilinameIdx;
            _descBlock = descBlock;
            _hint = hint;

            Width = 45;
            Height = 45;

            AddChild(new ImageObject(icon) { Height = Height+2, Width = Width+2 });

            this.AddBorderMapBack(new BorderConfiguration()
            {
                ImagesPath = "UI/bordermin/bord2.png",
                Size = 16,
                Padding = 2,
                Opacity = 0
            });
        }

        public override void Focus()
        {
            SetCursor(SceneObjects.Cursors.Cursor.Pointer);
            base.Focus();
        }

        public override void Unfocus()
        {
            SetCursor(SceneObjects.Cursors.Cursor.Normal);
            base.Unfocus();
        }

        public override void Click(PointerArgs args)
        {
            Global.Game.Creation.SelectedAbilityName = _abilinameIdx;
            _descBlock.SetHint(_hint);
            base.Click(args);
        }
    }
}
