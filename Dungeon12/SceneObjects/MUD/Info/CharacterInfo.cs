using Dungeon;
using Dungeon.Control;
using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using Dungeon12.ECS.Components;
using Dungeon12.Entities;
using Dungeon12.Entities.Enums;
using Dungeon12.SceneObjects.Base;
using Dungeon12.SceneObjects.Stats;

namespace Dungeon12.SceneObjects.MUD.Info
{
    internal class CharacterInfo : ActiveHeroControl
    {
        ClassBadge classBadge;

        public CharacterInfo()
        {
            Width = 400-NineSliceSettings.Default.Size;
            Height = 800-NineSliceSettings.Default.Size;

            this.AddChild(new HeroName() {  Top=10});
            this.AddChildCenter(new InfoAvatar()
            {
                Top = 50
            }, vertical: false);

            classBadge = this.AddChild(new ClassBadge(Component.Archetype)
            {
                Top = 88,
                Left=50
            });

            var specBadge = this.AddChild(new SpecBadge()
            {
                Top=85,
                Left=295
            });


        }

        public override void Update(GameTimeLoop gameTime)
        {
            classBadge.Set(Component.Archetype);
        }

        public override Hero Component => Global.Game.Party.Active;

        public override void Click(PointerArgs args) { }

        class PeaceAbility : ActiveHeroControl, ITooltiped, IMouseHint, ICursored
        {
            public string TooltipText => throw new NotImplementedException();

            public CursorImage Cursor => CursorImage.Hand;

            public ISceneObjectHosted CreateMouseHint()
            {
                throw new NotImplementedException();
            }
        }

        class SpecBadge : ActiveHeroControl, ITooltiped
        {
            public SpecBadge()
            {
                this.Width=60;
                this.Height=60;
            }

            public override string Image => $"Enums/Archetype/vanguard-{Component.Archetype.ToString().ToLowerInvariant()}.png".AsmImg();

            public string TooltipText => $"Бонусы специализации: нет";
        }

        class HeroName : ActiveHeroObject
        {
            TextObject text;
            public HeroName()
            {
                this.Width = 400;
                this.Height=35;
                text = this.AddTextCenter(Party.Active.Name.HeroName().InSize(30));
            }

            public override void Update(GameTimeLoop gameTime)
            {
                text.SetText(Component.Name);
                this.CenterChildText(text);
                base.Update(gameTime);
            }
        }

        class InfoAvatar : ActiveHeroControl, ITooltiped
        {
            public InfoAvatar()
            {
                this.Width=128;
                this.Height=128;
            }

            public string TooltipText => Component.Name;

            public override string Image => Component.Chip;

            public override Hero Component => Global.Game.Party.Active;
        }
    }

    internal class ActiveHeroControl : SceneControl<Hero>
    {
        public ActiveHeroControl() : base(null) { }

        public override Hero Component => Global.Game.Party.Active;
    }

    internal class ActiveHeroObject : SceneObject<Hero>
    {
        public ActiveHeroObject():base(null) { }

        public Party Party => Global.Game.Party;

        public override Hero Component => Global.Game.Party.Active;
    }
}
