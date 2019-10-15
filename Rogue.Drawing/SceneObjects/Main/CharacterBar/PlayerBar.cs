namespace Rogue.Drawing.SceneObjects.Main
{
    using Rogue.Drawing.SceneObjects.Main.CharacterBar;
    using Rogue.Drawing.SceneObjects.Map;
    using Rogue.Map;
    using Rogue.View.Interfaces;
    using System;
    using System.Collections.Generic;

    public class PlayerBar : SceneObject
    {
        public override bool AbsolutePosition => true;
        public override bool CacheAvailable => false;
        
        public PlayerBar(GameMap gamemap, PlayerSceneObject playerSceneObject, Action<List<ISceneObject>> showEffects)
        {
            bool needSlide() => gamemap.InSafe(playerSceneObject.Avatar);

            this.AddChild(new TorchButton(playerSceneObject, showEffects)
            {
                Left = -1,
                Top=0.5,
                SlideNeed= needSlide,
                SlideOffsetLeft=5
            });
            this.AddChild(new CharButton(gamemap, playerSceneObject, showEffects)
            {
                SlideNeed = needSlide,
                SlideOffsetLeft = 5
            });
            this.AddChild(new SkillsButton(playerSceneObject,showEffects)
            {
                Left=11.5,
                SlideNeed = needSlide,
                SlideOffsetLeft = -5
            });
            this.AddChild(new TalantsButton(playerSceneObject, showEffects)
            {
                Left=13,
                Top=0.5,
                SlideNeed = needSlide,
                SlideOffsetLeft = -5
            });
        }
    }
}
