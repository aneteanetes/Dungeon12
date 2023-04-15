using Dungeon12.Entities.Map;
using Dungeon12.SceneObjects.RegionScreen;
using System.ComponentModel;

namespace Dungeon12.SceneObjects.MUD.ViewRegion
{
    internal class MUDMapView : SceneControl<Region>
    {
        public MUDMapView(Region region) : base(region)
        {
            Width = 400;
            Height = 400;

            this.AddBorderBack();

            //this.AddChild(new RegionViewTile(loc)
            //{
            //    Left= 3*50 +12,
            //    Top= 3*50 +12,
            //});

            //this.AddChild(new RegionViewTile(loc)
            //{
            //    Left= 5*50 +12,
            //    Top= 3*50 +12,
            //});

            var paluba = new Location()
            {
                Name="Палуба",
                Transitions=new List<LocationTransition>()
                {
                    new LocationTransition(){ Direction = Dungeon.Types.Direction.LeftUp, Name="Высадиться", State = TransitionState.Unknown}
                }
            };


            this.AddChild(new RegionViewTile(paluba)
            {
                Left= 3*50 +12,
                Top= 3*50 +12,
            });

            var kauti = new Location()
            {
                Name="Каюты",
                IsCurrent=true,
                Transitions=new List<LocationTransition>()
                {
                    new LocationTransition(){ Direction= Dungeon.Types.Direction.Left,Name="На палубу", State= TransitionState.Completed}
                }
            };

            this.AddChild(new RegionViewTile(kauti)
            {
                Left= 4*50 +12,
                Top= 3*50 +12,
            });

            var trum = new Location() { 
                Name="Трюм",
                Transitions=new List<LocationTransition>()
                {
                    new LocationTransition(){ Direction= Dungeon.Types.Direction.Up,Name="В трюм", State= TransitionState.Completed}
                }
            };

            this.AddChild(new RegionViewTile(trum)
            {
                Left= 4*50 +12,
                Top= 4*50 +12,
            });

            this.AddChild(new AreaTitle(region.Title));

            //this.AddChild(new RegionViewTile(loc)
            //{
            //    Left= 4*50 +12,
            //    Top= 4*50 +12,
            //});

            //this.AddChild(new RegionViewTile(loc)
            //{
            //    Left= 3*50 +12,
            //    Top= 5*50 +12,
            //});

            //this.AddChild(new RegionViewTile(loc)
            //{
            //    Left= 5*50 +12,
            //    Top= 5*50 +12,
            //});

            return;

            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    //if (x!=0 || y!=0)
                    //    loc=new Location() { Name=Guid.NewGuid().ToString() };

                    var loc = new Location()
                    {
                        Name=Guid.NewGuid().ToString(),
                        Transitions=new List<LocationTransition>()
                        {
                            new LocationTransition() { Direction =  Dungeon.Types.Direction.Up},
                            new LocationTransition() { Direction =  Dungeon.Types.Direction.Down},
                            new LocationTransition() { Direction=  Dungeon.Types.Direction.Left},
                            new LocationTransition() { Direction=  Dungeon.Types.Direction.Right},
                            new LocationTransition() { Direction=  Dungeon.Types.Direction.UpLeft},
                            new LocationTransition() { Direction=  Dungeon.Types.Direction.UpRight},
                            new LocationTransition() { Direction=  Dungeon.Types.Direction.DownLeft},
                            new LocationTransition() { Direction=  Dungeon.Types.Direction.DownRight},
                        },
                        X=x,
                        Y=y
                    };

                    this.AddChild(new RegionViewTile(loc)
                    {
                        Left= x*50 +12,
                        Top= y*50 +12,
                    });
                }
            }
        }
        public override void Throw(Exception ex)
        {
            throw ex;
        }
    }
}