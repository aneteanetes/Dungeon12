using Dungeon;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using Dungeon12.ECS.Components;
using Dungeon12.Entities;
using Dungeon12.Entities.Abilities;
using Dungeon12.Entities.Enums;
using Dungeon12.SceneObjects.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.SceneObjects.HeroPanelObjs
{
    internal class AbilityItem : SceneControl<Hero>, ITooltipedDrawText, IMouseHint, ICursored
    {
        Ability _ability;

        public AbilityItem(Hero component, Ability ability, int idx=0) : base(component)
        {
            _ability = ability;
            this.Width = 85;
            this.Height = 85;

            this.Image = "UI/start/icon.png".AsmImg();

            var img = $"Abilities/{ability.ClassName}.tga";

            if (idx>0)
            {
                switch (component.Archetype)
                {
                    case Archetype.Warrior:
                        switch (idx)
                        {
                            case 1: img = $"AbilitiesPeacefull/Landscape.tga"; break;
                            case 2: img = $"AbilitiesPeacefull/Eating.tga"; break;
                            case 3: img = $"AbilitiesPeacefull/Repair.tga"; break;
                            case 4: img = $"AbilitiesPeacefull/Smithing.tga"; break;
                            default:
                                break;
                        }
                        break;
                    case Archetype.Mage:
                        switch (idx)
                        {
                            case 1: img = $"AbilitiesPeacefull/Portals.tga"; break;
                            case 2: img = $"AbilitiesPeacefull/Attension.tga"; break;
                            case 3: img = $"AbilitiesPeacefull/Enchantment.tga"; break;
                            case 4: img = $"AbilitiesPeacefull/Alchemy.tga"; break;
                            default:
                                break;
                        }
                        break;
                    case Archetype.Thief:
                        switch (idx)
                        {
                            case 1: img = $"AbilitiesPeacefull/Traps.tga"; break;
                            case 2: img = $"AbilitiesPeacefull/Lockpicking.tga"; break;
                            case 3: img = $"AbilitiesPeacefull/Stealing.tga"; break;
                            case 4: img = $"AbilitiesPeacefull/Leatherwork.tga"; break;
                            default:
                                break;
                        }
                        break;
                    case Archetype.Priest:
                        switch (idx)
                        {
                            case 1: img = $"AbilitiesPeacefull/Prayers.tga"; break;
                            case 2: img = $"AbilitiesPeacefull/FoodStoring.tga"; break;
                            case 3: img = $"AbilitiesPeacefull/Trade.tga"; break;
                            case 4: img = $"AbilitiesPeacefull/Tailoring.tga"; break;
                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }
            }

            this.AddChild(new ImageObject(img)
            {
                Width = 81,
                Height = 81,
                Left = 2,
                Top = 2
            });
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

        public IDrawText TooltipText => $"{Global.Strings[_ability.ClassName]}".AsDrawText().Gabriela();

        public bool ShowTooltip => true;

        public CursorImage Cursor => CursorImage.Question;

        public ISceneObjectHosted CreateMouseHint()
            => new GameHint(_ability.Name, _ability.Description,_ability.Area, _ability.Cooldown,.9, _ability.GetTextParams());
    }
}
