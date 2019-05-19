namespace Rogue.Map.Editor.Objects
{
    using Rogue.View.Interfaces;

    public class DesignCell
    {
        public DesignCell(ISceneObject sceneObject)
        {
            this.SceneObject = sceneObject;
        }

        public ISceneObject SceneObject { get; set; }

        public bool Obstruction { get; set; }
    }
}
