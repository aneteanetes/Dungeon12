using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon.Engine.Editable
{
    public enum StructureObjectType
    {
        Layer = 0,
        Object = 1
    }

    public static class DungeonEngineSceneStructureTypeExtensions
    {
        public static bool CanContains(this StructureObjectType host, StructureObjectType applicant)
        {
            if (host == StructureObjectType.Layer && applicant == StructureObjectType.Object)
                return true;

            if (host == StructureObjectType.Object && applicant == StructureObjectType.Object)
                return true;

            return false;
        }

        public static bool CanInRoot(this StructureObjectType applicant) => applicant != StructureObjectType.Object;
    }
}