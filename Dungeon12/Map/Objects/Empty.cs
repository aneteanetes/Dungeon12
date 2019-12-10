namespace Dungeon12.Map.Objects
{
    using Dungeon.Drawing.SceneObjects;
    using Dungeon.SceneObjects;
    using Dungeon.Types;
    using Dungeon.View.Interfaces;
    using Dungeon12.Data.Region;
    using Dungeon12.Map.Infrastructure;

    [Template(".")]
    public class Empty : MapObject
    {
        protected override MapObject Self => this;

        public override ISceneObject Visual()
        {
            if (this.Image != default)
            {
                return new ImageControl(this.Image)
                {
                    Left=this.Location.X,
                    Top=this.Location.Y
                };
            }
            return new EmptySceneObject();
        }

        protected override void Load(RegionPart regionPart)
        {
            base.Load(regionPart);
        }
    }
}