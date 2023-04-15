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
    internal class AbilityItemBig : SceneControl<Hero>, ITooltipedDrawText, IMouseHint, ICursored
    {
        public override void Throw(Exception ex)
        {
            throw ex;
        }
        Ability _ability;

        private string _title;

        public AbilityItemBig(Hero component, Ability ability, int idx=0) : base(component)
        {
            _ability = ability;
            _title = _ability.ClassName;
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
                            case 1: img = $"AbilitiesPeacefull/Landscape.tga"; _title = "Landscape"; break;
                            case 2: img = $"AbilitiesPeacefull/Eating.tga"; _title = "Eating"; break;
                            case 3: img = $"AbilitiesPeacefull/Repair.tga"; _title = "Repair"; break;
                            case 4: img = $"AbilitiesPeacefull/Smithing.tga"; _title = "Smithing"; break;
                            default:
                                break;
                        }
                        break;
                    case Archetype.Mage:
                        switch (idx)
                        {
                            case 1: img = $"AbilitiesPeacefull/Portals.tga"; _title = "Portals"; break;
                            case 2: img = $"AbilitiesPeacefull/Attension.tga"; _title = "Attension"; break;
                            case 3: img = $"AbilitiesPeacefull/Enchantment.tga"; _title = "Enchantment"; break;
                            case 4: img = $"AbilitiesPeacefull/Alchemy.tga"; _title = "Alchemy"; break;
                            default:
                                break;
                        }
                        break;
                    case Archetype.Thief:
                        switch (idx)
                        {
                            case 1: img = $"AbilitiesPeacefull/Traps.tga"; _title = "Traps"; break;
                            case 2: img = $"AbilitiesPeacefull/Lockpicking.tga"; _title = "Lockpicking"; break;
                            case 3: img = $"AbilitiesPeacefull/Stealing.tga"; _title = "Stealing"; break;
                            case 4: img = $"AbilitiesPeacefull/Leatherwork.tga"; _title = "Leatherwork"; break;
                            default:
                                break;
                        }
                        break;
                    case Archetype.Priest:
                        switch (idx)
                        {
                            case 1: img = $"AbilitiesPeacefull/Prayers.tga"; _title = "Prayers"; break;
                            case 2: img = $"AbilitiesPeacefull/FoodStoring.tga"; _title = "FoodStoring"; break;
                            case 3: img = $"AbilitiesPeacefull/Trade.tga"; _title = "Trade"; break;
                            case 4: img = $"AbilitiesPeacefull/Tailoring.tga"; _title = "Tailoring"; break;
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

        public IDrawText TooltipText => $"{Global.Strings[_title]}".AsDrawText().WithOpacity(1.1).Gabriela();

        public bool ShowTooltip => true;

        public CursorImage Cursor => CursorImage.Question;

        public ISceneObjectHosted CreateMouseHint()
            => new ObjectPanel(_ability.Name, _ability.Description,_ability.Area, _ability.Cooldown, _ability.GetTextParams());
    }
}
