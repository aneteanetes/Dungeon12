namespace Dungeon12.SceneObjects.UserInterface.FractionSelect
{
    using Dungeon;
    using Dungeon.Drawing.SceneObjects;
    using Dungeon.SceneObjects;
    using Dungeon12.Entities;
    using Dungeon12.Entities.Enums;
    using Dungeon12.Entities.Zones;
    using Dungeon12.SceneObjects.UserInterface.Common;
    using System.Linq;

    public class FractionSelectSceneObject : SceneControl<Hero>
    {
        public FractionDescription desc;

        public static FractionSelectSceneObject Instance;

        public FractionSelectSceneObject(Hero component) : base(component)
        {
            Instance = this;
            DungeonGlobal.Freezer.Freeze(this);
            Destroy += () =>
            {
                DungeonGlobal.Freezer.Unfreeze();
            };

            Width = DungeonGlobal.Resolution.Width;
            Height = DungeonGlobal.Resolution.Height;

            var map = this.AddChildCenter(new Map(this));

            var mapborder = this.AddChildCenter(new ImageObject("Maps/mapbord.png".AsmImg())
            {
                Width = 1058,
                Height = 684
            });
            desc = this.AddChild(new FractionDescription());
            desc.Left = (mapborder.Left + 1000) - desc.Width;
            desc.Top = (mapborder.Top + 629) - desc.Height;

            this.AddChild(new MapCloseButton()
            {
                Left = mapborder.Left + mapborder.Width - 20,
                Top = 70,
                OnClick = Close
            });
            void Close()
            {
                this.Destroy?.Invoke();
            }

        }

        public override void UpdateSceneObject(GameTimeLoop gameTime)
        {
            if (Component.Fraction != null && this.Layer!=null && desc.IsEmpty)
                desc.Load(Component.Fraction.Value);
            base.UpdateSceneObject(gameTime);
        }

        private class Map : EmptySceneControl
        {
            public Map(FractionSelectSceneObject mainobj)
            {
                this.Width = 1000;
                this.Height = 629;

                this.Image = "Maps/mainlandback_ancient_cut.png".AsmImg();

                typeof(Fraction).All<Fraction>().ForEach(f =>
                {
                    AddChild(new FractionBadge(mainobj.Component, f, mainobj));
                });
            }
        }
    }
}