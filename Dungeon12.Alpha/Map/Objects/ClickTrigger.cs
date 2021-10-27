using Dungeon;
using Dungeon.Data.Attributes;
using Dungeon.View.Interfaces;
using Dungeon12.Data.Region;
using Dungeon12.Database.ClickTriggers;
using Dungeon12.Map.Infrastructure;
using Dungeon12.SceneObjects.Map;

namespace Dungeon12.Map.Objects
{
    [Template("ClickTrigger")]
    [DataClass(typeof(ClickTriggerData))]
    public class ClickTrigger : MapObject
    {
        public override bool Saveable => true;

        private UnknownITrigger Trigger;
        private object[] Arguments;

        protected override void Load(RegionPart regionPart)
        {
            var data = LoadData<ClickTriggerData>(regionPart);
            this.Icon = data.Icon;
            Trigger = data.TriggerName.Trigger();            
        }

        public override ISceneObject Visual()
        {
            return new ClickTriggerSceneObject(Global.GameState.Player, this);
        }

        public void Click() => Trigger.Unkown(Arguments);
    }
}