using Dungeon;
using Dungeon.Drawing.SceneObjects;
using Dungeon.View.Interfaces;
using Dungeon12.ECS.Components;
using Dungeon12.Entities;
using Dungeon12.Entities.Abilities;
using Dungeon12.Entities.Enums;
using Dungeon12.SceneObjects.Base;

namespace Dungeon12.SceneObjects.HeroPanelObjs
{
    internal class AbilityItemBig : SceneControl<Hero>, ITooltipedDrawText, IMouseHint, ICursored
    {
        public override void Throw(Exception ex)
        {
            throw ex;
        }
        Ability _ability;

        private string _title;

        private ImageObject _icon;

        public AbilityItemBig(Hero component, Ability ability, int idx=0) : base(component)
        {
            _ability = ability;
            _title = _ability.ClassName;
            this.Width = 64;
            this.Height = 64;

            this.Image = "UI/start/icon.png".AsmImg();

            var img = $"Abilities/{ability.ClassName}.tga";

            _icon = this.AddChild(new ImageObject(img)
            {
                Width = this.Width-4,
                Height = this.Height-4,
                Left = 2,
                Top = 2
            });
        }

        public override double Width
        {
            get => base.Width;
            set
            {
                if (_icon?.Width!=null)
                {
                    _icon.Width = value-4;
                }

                base.Width=value;
            }
        }

        public override double Height
        {
            get => base.Height;
            set
            {
                if (_icon?.Height!=null)
                {
                    _icon.Height = value-4;
                }
                base.Height=value;
            }
        }

        public override void Focus()
        {
            Image = "UI/start/classselector.png".AsmImg();
            base.Focus();
        }

        public override void Unfocus()
        {
            Image = "UI/start/icon.png".AsmImg();
            base.Unfocus();
        }

        public override bool Visible => Component.Archetype == _ability.Class;

        public IDrawText TooltipText => $"{Global.Strings[_title]}".AsDrawText().WithOpacity(1.1).Gabriela();

        public bool ShowTooltip => true;

        public CursorImage Cursor => CursorImage.Question;

        public ISceneObjectHosted CreateMouseHint()
            => new ObjectPanel(_ability.Name, _ability.Description,_ability.Area, _ability.Cooldown, _ability.GetTextParams());
    }
}
