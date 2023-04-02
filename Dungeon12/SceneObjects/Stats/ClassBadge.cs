using Dungeon.SceneObjects;
using Dungeon12.ECS.Components;
using Dungeon12.Entities.Enums;

namespace Dungeon12.SceneObjects.Stats
{
    internal class ClassBadge : EmptySceneControl, ITooltiped
    {
        public override void Throw(Exception ex)
        {
            throw ex;
        }

        public ClassBadge(Archetype archetype) => Set(archetype);

        public void Set(Archetype archetype)
        {
            TooltipText=archetype.Display();
            Image=$"Enums/Archetype/{archetype}.png";
        }

        public string TooltipText { get; set; }
    }
}