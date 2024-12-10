using Dungeon.SceneObjects;
using Nabunassar.ECS.Components;
using Nabunassar.Entities.Enums;

namespace Nabunassar.SceneObjects.Stats
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