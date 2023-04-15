using Dungeon;
using Dungeon.SceneObjects;
using Dungeon12.Entities;

namespace Dungeon12.SceneObjects.MUD
{
    internal class ChatPanel : SceneObject<GameLog>
    {
        public ChatPanel(GameLog component) : base(component)
        {
            this.Width=1120;
            this.Height=200;

            this.AddBorderBack();

            double top = 5;

            for (int i = 0; i < 10; i++)
            {
                var txt = this.AddChild(
                    new TextObject($"{DateTime.Now:mm:ss} : {Guid.NewGuid()} {Guid.NewGuid()} {Guid.NewGuid()}"
                        .AsDrawText()
                        .InBold()
                        .Calibri()
                        .InSize(20)
                        .IsNew(true)
                        .InColor(Global.CommonColorLight))
                    {
                        Left=7,
                        Top=top
                    });
                top += 19;


            }
        }
    }
}
