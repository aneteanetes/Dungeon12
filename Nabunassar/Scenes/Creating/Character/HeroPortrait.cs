using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Nabunassar.Entities;
using Nabunassar.Entities.Enums;
using Nabunassar.SceneObjects.Base;
using Nabunassar.SceneObjects.UserInterface.Common;
using System.ComponentModel;

namespace Nabunassar.Scenes.Creating.Character
{
    internal class HeroPortrait : SceneObject<Hero>
    {
        ImageObject img = null;
        ClassicButton m, f;

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

            m = this.AddChild(new ClassicButton(Global.Strings["M"], 25, 25, 12,"bord15.png")
            {
                Left = this.Width,
                OnClick = () =>
                {
                    Component.Sex = Sex.Male;
                    this._sex = Sex.Male;
                }
            });

            f = this.AddChild(new ClassicButton(Global.Strings["F"], 25, 25, 12, "bord15.png")
            {
                Left = this.Width,
                Top = 30,
                OnClick = () =>
                {
                    Component.Sex = Sex.Female;
                    this._sex = Sex.Female;
                }
            });
        }

        private Sex _sex = Sex.Male;
        private string postfix => _sex == Sex.Male ? "m" : "f";

        public override bool Updatable => true;

        public override void Update(GameTimeLoop gameTime)
        {
            if (Component?.Race!=null)
            {
                this.Image = $"Portraits/{Component.Race}_{postfix}.jpg";
            }

            base.Update(gameTime);
        }
    }
}

//bord7