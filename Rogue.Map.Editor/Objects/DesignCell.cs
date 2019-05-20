namespace Rogue.Map.Editor.Objects
{
    using Rogue.Drawing.Impl;
    using Rogue.Drawing.SceneObjects;
    using Rogue.View.Interfaces;

    public class DesignCell : ImageControl
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
