using System;

namespace Dungeon.Scenes
{
    /// <summary>
    /// Маркер, что эта сцена инстантиирующая и первая в игре.
    /// <para>Если требуется несколько экранов перед показом, их можно реализовать в этой сцене</para>
    /// <para>Если нужен экран загрузки отличающийся от стандартного, можно оставить его пустым, или сделать отдельный</para>
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class EntrySceneAttribute : Attribute
    {
    }
}
