using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Nabunassar.Entities;
using Nabunassar.SceneObjects.Base;
using System.ComponentModel;

namespace Nabunassar.Scenes.Creating.Character
{
    internal class HeroPortrait : SceneObject<Hero>
    {
        ImageObject img = null;

        public HeroPortrait(Hero component) : base(component)
        {
            this.Height = 250;
            this.Width = 150;

            this.AddBorderMapBack(new BorderConfiguration()
            {
                ImagesPath = "UI/bordermin/bord7.png",
                Size = 16,
                Padding = 2,
                Opacity=0
            });
        }

        public override bool Updatable => true;

        public override void Update(GameTimeLoop gameTime)
        {
            if (Component != null)
            {
                this.Image = $"Portraits/{Component.Race}_f.jpg";
            }

            this.Visible = Component != null;

            base.Update(gameTime);
        }
    }
}

//bord7