using Dungeon.SceneObjects;
using Dungeon.Scenes;
using Nabunassar.Entities;
using Nabunassar.Entities.Characters;
using Nabunassar.SceneObjects.Base;
using Nabunassar.SceneObjects.UserInterface.Common;

namespace Nabunassar.Scenes.Creating.Character.Stats
{
    internal class StatsEditor : CreatePart
    {
        TextObject freepoints;

        public StatsEditor(Hero component) : base(component)
        {
            Width = 400;
            Height = 700;
            Top = 300;
            Left = 50;

            this.AddBorderMapBack(new BorderConfiguration()
            {
                ImagesPath = "UI/bordermin/panel-border-022.png",
                Size = 16,
                Padding = 2
            });

            var title = AddTextCenter(Global.Strings["Characteristics"].ToString().DefaultTxt(20));
            title.Top = 5;

            freepoints = AddTextCenter(Global.Strings["Free"].ToString().DefaultTxt(18));
            freepoints.Top = 35;

            var top = 50;
            var topOffset = 125;

            var con = this.AddChild(new StatEditor(component, 0));
            con.Top = top+25;
            con.Left = 25;

            var agi = this.AddChild(new StatEditor(component, 1));
            agi.Top = con.TopMax + top;
            agi.Left = 25;

            var @int = this.AddChild(new StatEditor(component, 2));
            @int.Top = agi.TopMax + top;
            @int.Left = 25;

            var dia = this.AddChild(new StatEditor(component, 3));
            dia.Top = @int.TopMax + top;
            dia.Left = 25;
        }

        public override bool Updatable => true;

        public override void Update(GameTimeLoop gameTime)
        {
            if (this.Visible == true && !Global.Game.Creation.StatsUnblocked)
                Global.Game.Creation.StatsUnblocked = true;

            freepoints.SetText(Global.Strings["Free"] + " : " + Component.PrimaryStats.FreePoints);
            base.Update(gameTime);
        }
    }
}
