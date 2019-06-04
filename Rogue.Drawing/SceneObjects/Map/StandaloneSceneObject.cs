namespace Rogue.Drawing.SceneObjects.Map
{
    using Rogue.Control.Pointer;
    using Rogue.Entites.Animations;
    using Rogue.Map;
    using Rogue.Map.Objects;
    using System;

    public class StandaloneSceneObject : AnimatedSceneObject
    {
        public StandaloneSceneObject(MapObject mapObject, Func<int, AnimationMap, bool> requestNextFrame = null)
            : base(mapObject.Name, new Types.Rectangle
            {
                X = 0,
                Y = 0,
                Height = 32,
                Width = 32,
            },null)
        {
            this.Image = mapObject.Tileset;
            this.SetAnimation(mapObject.Animation);
        }

        protected override void DrawLoop()
        {
        }

        public override void Click(PointerArgs args)
        {
            System.Console.WriteLine("clicked standalone");
        }
    }
}
