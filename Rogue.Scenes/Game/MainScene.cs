namespace Rogue.Scenes.Game
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Rogue.DataAccess;
    using Rogue.Drawing.GUI;
    using Rogue.Drawing.Labirinth;
    using Rogue.Map;
    using Rogue.Scenes.Scenes;

    public class MainScene : GameScene
    {
        public MainScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => false;

        public override void Draw()
        {
            if (this.Location == null)
                this.InitMap();

            Drawing.Draw.Session/*<CharMapDrawSession>()*/
                /*.Then*/<LabirinthDrawSession>(x=>x.Location=this.Location)
                .Publish();
        }

        private void InitMap()
        {
            this.Location = new Location();

            var persistMap = Database.Entity<Data.Maps.Map>(x => x.Identity == "Capital").First();

            this.Location.Name = persistMap.Name;

            foreach (var line in persistMap.Template.Split(Environment.NewLine))
            {
                var listLine = new List<Map.MapObject>();

                foreach (var @char in line)
                {
                    listLine.Add(MapObject.Create(@char.ToString()));
                }

                this.Location.Map.Add(listLine);
            }
        }
    }
}
