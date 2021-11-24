using System;

namespace Dungeon.Utils.AttributesForInformation
{
    /// <summary>
    /// Аттрибут указывающий что поле где-то будет установлено
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class InjectedAttribute : Attribute
    {
        
    }
}
