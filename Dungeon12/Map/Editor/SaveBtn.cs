namespace Dungeon12.Map.Editor
{
    using Dungeon;
    using Dungeon.Control;
    using Dungeon.Drawing;
    using Dungeon.Drawing.SceneObjects;
    using Dungeon12.Map.Editor.Field;

    public class SaveBtn : DarkRectangle
    {
        public override bool AbsolutePosition => true;
        private EditedGameField editedGameField;

        private int saves = 0;

        public SaveBtn(EditedGameField editedGameField)
        {
            this.editedGameField = editedGameField;

            this.Width = 5;
            this.Height = 1;

            this.AddTextCenter(new DrawText("Сохранить").Montserrat());
        }

        public override void Click(PointerArgs args)
        {
            saves++;
            Global.DrawClient.SaveObject(editedGameField, "map.png", new Dungeon.Types.Point(-20 * 32, 0), $"designcache{saves}");
            editedGameField.Save($"designcache{saves}");
        }
    }
}