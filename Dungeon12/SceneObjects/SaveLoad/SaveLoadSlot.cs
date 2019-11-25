using Dungeon;
using Dungeon.Data;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;

namespace Dungeon12.SceneObjects.SaveLoad
{
    public class SaveLoadSlot : HandleSceneControl<SaveModel>
    {
        public SaveLoadSlot(SaveModel component) : base(component, false)
        {
            this.Width = 23;
            this.Height = 3;

            this.AddChild(new DarkRectangle()
            {
                Width = 23,
                Height = 3
            });

            var name = this.AddTextCenter(component.IdentifyName.AsDrawText().Montserrat(), false, false);

            var location = this.AddTextCenter(component.RegionName.AsDrawText().Montserrat(), false, false);
            location.Top += 1;

            var @char = this.AddTextCenter($"{component.CharacterName} {component.ClassName} ({component.Level})".AsDrawText().Montserrat(), false, false);
            @char.Top += 2;
        }

        public int ItemIndex { get; set; }

        public override bool Visible
        {
            get
            {
                if (this.Top < 0)
                    return false;

                if (this.Top > 21)
                    return false;

                return true;
            }
        }
    }
}