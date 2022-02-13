using Dungeon;
using Dungeon.Resources;
using Dungeon.View.Interfaces;
using Dungeon12.Entities.Talks;
using Dungeon12.SceneObjects.Talk;

namespace Dungeon12.Functions.ObjectFunctions
{
    public class DialogueFunction : IFunction
    {
        public string Name => nameof(DialogueFunction);

        public bool Call(ISceneLayer layer, string objectId)
        {
            var dialogue = ResourceLoader.LoadJson<Dialogue>($"Dialogs/{objectId}.json".AsmRes());
            dialogue.Id = objectId;
            layer.Scene.GetLayer("ui").AddObjectCenter(new DialogueSceneObject(dialogue));
            return true;
        }
    }
}