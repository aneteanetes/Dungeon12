namespace Dungeon12.Map.Editor.Objects
{
    using Dungeon.Drawing;
    using Dungeon.SceneObjects;
    using Dungeon.View.Interfaces;

    public class DesignCell : Dungeon.Drawing.SceneObjects.ImageControl
    {
        public DesignCell(string img, bool obstruction) : base(img)
        {
            Obstruction = obstruction;
            if(Obstruction)
            {
                this.AddChild(new TextControl(new DrawText("*", new DrawColor(System.ConsoleColor.Red)))
                {
                    Left = 0.9,
                    ForceInvisible=true
                });
            }
        }

        public bool Obstruction { get; set; }
    }
}
