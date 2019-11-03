namespace Dungeon12.Drawing.SceneObjects.Main
{
    using Dungeon.Drawing.SceneObjects.Map;
    using Dungeon.Map;
    using Dungeon.SceneObjects;
    using Dungeon.View.Interfaces;
    using Dungeon12.Drawing.SceneObjects.Main.CharacterBar;
    using Dungeon12.SceneObjects;
    using System;
    using System.Collections.Generic;

    public class PlayerBar : SceneObject
    {
        public override bool AbsolutePosition => true;
        public override bool CacheAvailable => false;
        
        public PlayerBar(GameMap gamemap, Player playerSceneObject, Action<List<ISceneObject>> showEffects)
        {
            bool needSlide() => gamemap.InSafe(playerSceneObject.Avatar);

            this.AddChild(new TorchButton(playerSceneObject, showEffects)
            {
                Left = -2,
                Top=0.5,
                SlideNeed= needSlide,
                SlideOffsetLeft=5
            });
            this.AddChild(new JournalButton(playerSceneObject, showEffects)
            {
                Left = -1,
                Top = 0.5,
                SlideNeed = needSlide,
                SlideOffsetLeft = 5
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
