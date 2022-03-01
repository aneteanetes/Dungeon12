using Dungeon;
using Dungeon.Control;
using Dungeon.Drawing;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon.Types;
using Dungeon.View.Interfaces;
using Dungeon12.ECS.Components;
using Dungeon12.Entities;
using Dungeon12.Entities.Enums;
using Dungeon12.Entities.Talks;
using Dungeon12.SceneObjects.Base;

namespace Dungeon12.SceneObjects.Create
{
    public class ClassButton : SceneControl<Hero>, ITooltiped
    {
        Archetype _archetype;

        public ClassButton(Hero component, Archetype archetype) : base(component)
        {
            _archetype = archetype;
            this.Width = 55;
            this.Height = 55;

            this.Image = $"Enums/Archetype/{archetype}.png".AsmImg();
        }

        public IDrawText TooltipText => _archetype.Display().AsDrawText().Gabriela().InSize(12);

        public bool ShowTooltip => true;

        public void RefreshTooltip() { }
    }
}