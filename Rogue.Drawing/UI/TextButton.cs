namespace Rogue.Drawing.UI
{
    using System;
    using Rogue.Types;
    using Rogue.View.Interfaces;

    public class TextButton : ISprite
    {
        public string Tileset => "Rogue.Resources.Images.ui.button{0}.png";

        public Rectangle Position { get; set; }

        public Rectangle Source => throw new NotImplementedException();

        public Action Click => throw new NotImplementedException();

        public Rectangle SourceFocus => throw new NotImplementedException();

        public Action Focus => throw new NotImplementedException();

        public Action Unfocus => throw new NotImplementedException();
    }
}
