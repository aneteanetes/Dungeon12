using Dungeon.GameObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon.SceneObjects
{
    /// <summary>
    /// Объект сцены без сущности
    /// <para>
    /// Использовать только для эффектов и прочих вспомогательных объектов сцены
    /// </para>
    /// <para>
    /// ПО сути - утечка. ПО факту - MVC :D
    /// </para>
    /// </summary>
    public class EmptySceneObject : SceneObject<GameComponentEmpty>
    {
        public EmptySceneObject() : base(GameComponentEmpty.Empty)
        {
        }

        public override void Throw(Exception ex)
        {
            throw ex;
        }
    }
}
