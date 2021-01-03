namespace Dungeon.Engine.Editable.ObjectTreeList
{
    public class ObjectTreeListItem
    {
        public string Name { get; set; }

        private string _image;

        public string Image
        {
            get => _image == default
                    ? template.Replace("@", "Cube_16x")
                    : _image;

            private set => _image = value;
        }

        private static string template = $"pack://siteoforigin:,,,/Icons/@.png";

        public void BindEmbeddedIcon(string name)
        {
            _image = template.Replace("@", name.Replace(".png", ""));
        }
    }
}