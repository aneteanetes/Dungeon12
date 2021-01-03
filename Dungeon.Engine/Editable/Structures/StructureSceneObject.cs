using Dungeon.Engine.Editable.ObjectTreeList;
using Dungeon.Engine.Projects;

namespace Dungeon.Engine.Editable.Structures
{
    public class StructureSceneObject : StructureObject
    {
        public override StructureObjectType StructureType => StructureObjectType.Object;

        public StructureSceneObject()
        {
            this.BindEmbeddedIcon("DeploymentFileStatusBar5_16x");
        }

        public SceneObject SceneObject { get; set; }


        public override ObjectTreeListItem Clone()
        {
            var newCopy = new StructureSceneObject();

            FillClone(newCopy);

            newCopy.SceneObject = SceneObject.Clone();

            return newCopy;
        }
    }
}
