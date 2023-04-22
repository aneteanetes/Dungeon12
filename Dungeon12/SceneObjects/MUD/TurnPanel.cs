using Dungeon.SceneObjects.Grouping;
using Dungeon12.Entities;
using Dungeon12.SceneObjects.Base;
using Dungeon12.SceneObjects.MUD.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon12.SceneObjects.MUD
{
    internal class TurnPanel : SceneControl<TurnOrder>
    {
        public TurnPanel(TurnOrder component) : base(component)
        {
            this.Width=1120;
            this.Height=60;

            this.AddBorder();

           this.AddChildCenter(new Turns(component));
        }

        private class Turns : SceneControl<TurnOrder>
        {
            public Turns(TurnOrder component) : base(component)
            {
                this.Height=60;
                this.Width= 45*4;

                var groupBuilder = new ObjectGroupBuilder<ChipView>()
               .Property(x => x.Selected);

                var left = 5;

                foreach (var item in Global.Game.Party)
                {
                    this.AddChild(groupBuilder.Add(new ChipView(item)
                    {
                        Left = left,
                        Top = 5,
                        Width=50,
                        Height=50
                    }));

                    left+=45;
                }

                var group = groupBuilder.Build();

                group.Select();
            }
        }
    }
}
