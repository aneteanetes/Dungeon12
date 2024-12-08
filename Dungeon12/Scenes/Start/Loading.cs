using Dungeon;
using Dungeon.Drawing;
using Dungeon.SceneObjects;

namespace Dungeon12.Scenes.Start
{
    internal class Loading : EmptySceneObject
    {
        TextObject dot1, dot2, dot3;

        public override bool Updatable => true;

        public int Counter = 0;

        public Loading()
        {
            this.Width=Global.Resolution.Width;
            this.Height=Global.Resolution.Height;


            var loadingtext = new DrawText("Loading", 300)
            {
                FontName = "NAVIEO Trial",
                FontAssembly = "Dungeon12"
            };
            var txt = new TextObject(loadingtext);
            var m = DungeonGlobal.GameClient.MeasureText(txt.Text); ;
            txt.Width = m.X;
            txt.Height = m.Y;

            this.AddChildCenter(txt);

            var textdot = loadingtext.Clone();
            textdot.SetText(".");


            dot1 = new TextObject(textdot);
            dot1.Visible = false;
            dot2 = new TextObject(textdot);
            dot2.Visible = false;
            dot3 = new TextObject(textdot);
            dot3.Visible = false;

            var dotmeasure = Global.GameClient.MeasureText(textdot);

            dot1.Width = dot2.Width = dot3.Width = dotmeasure.X;
            dot1.Height = dot2.Height = dot3.Height = dotmeasure.Y;

            this.AddChildCenter(dot1,vertical:false);
            dot1.Left -= dotmeasure.X +10;
            this.AddChildCenter(dot2, vertical: false);
            this.AddChildCenter(dot3, vertical: false);
            dot3.Left += dotmeasure.X +10;


            dot1.Top = dot2.Top = dot3.Top = this.Height - dotmeasure.Y - 75;
        }

        GameTimeLoop gameTimePrev;

        public override void Update(GameTimeLoop gameTime)
        {
            if (gameTime.TotalGameTime - gameTimePrev.TotalGameTime >= TimeSpan.FromSeconds(.8))
            {
                if (dot3.Visible)
                {
                    dot1.Visible = false;
                    dot2.Visible = false;
                    dot3.Visible = false;
                    Counter += 1;
                }
                else
                {
                    if (!dot1.Visible)
                    {
                        dot1.Visible = true;
                    }
                    else if (!dot2.Visible)
                    {
                        dot2.Visible = true;
                    }
                    else
                    {
                        dot3.Visible = true;
                    }
                }

                gameTimePrev = gameTime;
            }

            base.Update(gameTime);
        }
    }
}
