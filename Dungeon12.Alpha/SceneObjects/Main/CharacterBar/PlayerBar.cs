namespace Dungeon12.Drawing.SceneObjects.Main
{
    using Dungeon12.Drawing.SceneObjects.Map;
    using Dungeon12.Map;
    using Dungeon12.SceneObjects; using Dungeon.SceneObjects;
    using Dungeon.View.Interfaces;
    using Dungeon12.Drawing.SceneObjects.Main.CharacterBar;
    using Dungeon12.SceneObjects; using Dungeon.SceneObjects;
    using System;
    using System.Collections.Generic;

    public class PlayerBar : EmptySceneObject
    {
        public override bool AbsolutePosition => true;
        public override bool CacheAvailable => false;
        
        public PlayerBar(GameMap gamemap, Player playerSceneObject, Action<List<ISceneObject>> showEffects)
        {
            bool needSlide() => gamemap.InSafe(playerSceneObject.Avatar);

            this.AddChild(new TorchButton(playerSceneObject)
            {
                Left = -2,
                Top=0.5,
                SlideNeed= needSlide,
                SlideOffsetLeft=5
            });
            this.AddChild(new JournalButton(playerSceneObject)
            {
                Left = -1,
                Top = 0.5,
                SlideNeed = needSlide,
                SlideOffsetLeft = 5
            });
            this.AddChild(new CharButton(gamemap, playerSceneObject)
            {
                SlideNeed = needSlide,
                SlideOffsetLeft = 5
            });
            this.AddChild(new SkillsButton(playerSceneObject)
            {
                Left=11.5,
                SlideNeed = needSlide,
                SlideOffsetLeft = -5
            });
            this.AddChild(new TalantsButton(playerSceneObject)
            {
                Left=13,
                Top=0.5,
                SlideNeed = needSlide,
                SlideOffsetLeft = -5
            });
        }
    }
}
