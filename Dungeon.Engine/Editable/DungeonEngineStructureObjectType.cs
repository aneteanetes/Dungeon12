using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon.Engine.Editable
{
    public enum DungeonEngineStructureObjectType
    {
        Layer = 0,
        Object = 1,
        TileMap = 2
    }

    public static class DungeonEngineSceneStructureTypeExtensions
    {
        public static bool CanContains(this DungeonEngineStructureObjectType host, DungeonEngineStructureObjectType applicant)
        {
            if (host == DungeonEngineStructureObjectType.Layer && applicant == DungeonEngineStructureObjectType.Object)
                return true;

            if (host == DungeonEngineStructureObjectType.Object && applicant == DungeonEngineStructureObjectType.Object)
                return true;

            return false;
        }

        public static bool CanInRoot(this DungeonEngineStructureObjectType applicant) => applicant != DungeonEngineStructureObjectType.Object;
    }
}