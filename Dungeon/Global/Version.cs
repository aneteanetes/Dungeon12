namespace Dungeon
{
    public class Version
    {
        public Version()
        {

        }

        public Version(int major, int minor, int revision)
        {
            Major = major;
            Minor = minor;
            Revision = revision;
        }

        public int Major { get; set; }

        public int Minor { get; set; }

        public int Revision { get; set; }

        public override string ToString()
        {
            return $"{Major}.{Minor}.{Revision}";
        }
    }
}