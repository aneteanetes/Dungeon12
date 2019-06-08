namespace Rogue.Drawing.SceneObjects.Main
{
    using Rogue.Drawing.SceneObjects.Main.CharacterBar;
    using Rogue.Drawing.SceneObjects.Map;
    using Rogue.Entites.Alive.Character;
    using Rogue.View.Interfaces;
    using System;
    using System.Collections.Generic;

    public class PlayerBar : SceneObject
    {
        public override bool AbsolutePosition => true;
        public override bool CacheAvailable => false;
        
        public PlayerBar(PlayerSceneObject playerSceneObject, Action<List<ISceneObject>> showEffects)
        {
            this.AddChild(new CharButton(playerSceneObject,showEffects));
            this.AddChild(new SkillsButton(playerSceneObject,showEffects)
            {
                Left=11.5
            });
        }
    }
}
