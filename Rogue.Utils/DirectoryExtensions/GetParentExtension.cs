namespace Rogue.Utils.DirectoryExtensions
{
    using System;
    using System.IO;

    public static class GetParentExtension
    {
        public static DirectoryInfo GetParent(this DirectoryInfo directoryInfo, Func<DirectoryInfo, bool> check)
        {
            if (directoryInfo.Parent == null)
                return directoryInfo;

            var parent = directoryInfo.Parent;

            if (!check(parent))
                return parent.GetParent(check);

            return parent;
        }
    }
}
