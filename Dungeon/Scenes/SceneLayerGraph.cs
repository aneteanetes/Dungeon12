using Dungeon.Control;
using Dungeon.Physics;
using System;

namespace Dungeon.Scenes
{
    public class SceneLayerGraph : PhysicalObject<SceneLayerGraph>
    {
        public SceneLayerGraph() 
        {
            this.Size = new PhysicalSize();
            this.Position = new PhysicalPosition();
            this.Containable = true;
        }

        public SceneLayer SceneLayer { get; set; }

        public SceneLayerGraph(SceneLayer layer)
        {
            SceneLayer = layer;
            Size = new SceneLayerGraphSize(layer);
            Position = new PhysicalPosition();
            this.Containable = true;
        }

        public SceneLayerGraph(PointerArgs pointerArgs)
        {
            this.Size = new PhysicalSize(1, 1);
            this.Position = new PhysicalPosition(pointerArgs.X, pointerArgs.Y);
        }

        protected override SceneLayerGraph Self => this;

        private class SceneLayerGraphSize : PhysicalSize
        {
            public SceneLayer sceneLayer { get; set; }

            public SceneLayerGraphSize(SceneLayer layer) => sceneLayer = layer;

            public override double Width
            {
                get => sceneLayer.Width;
                set { }
            }

            public override double Height
            {
                get => sceneLayer.Height;
                set { }
            }
        }
    }
}