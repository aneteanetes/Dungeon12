namespace Rogue.Drawing.SceneObjects.Map
{
    using Rogue.Entites.Animations;
    using Rogue.Map;
    using Rogue.Map.Objects;
    using System;

    public class StandaloneSceneObject : AnimatedSceneObject
    {
        public StandaloneSceneObject(MapObject mapObject, Func<int, AnimationMap, bool> requestNextFrame = null)
            : base(new Types.Rectangle
            {
                X = 0,
                Y = 0,
                Height = 32,
                Width = 32,
            },requestNextFrame)
        {
            this.Image = mapObject.Tileset;
            this.SetAnimation(mapObject.Animation);
        }

        protected override void DrawLoop()
        {
        }

        public override void Click()
        {
            System.Console.WriteLine("clicked standalone");
        }
    }
}
