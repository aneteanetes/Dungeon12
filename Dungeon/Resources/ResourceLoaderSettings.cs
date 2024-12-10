using System;

namespace Dungeon.Resources
{
    public sealed class ResourceLoaderSettings
    {
        public bool ThrowIfNotFound { get; set; } = true;

        public Action<string> NotFoundAction { get; set; }

        public bool StretchResources { get; set; }

        public bool IsEmbeddedMode { get; set; }

        public bool NotDisposingResources { get; set; }

        public bool CacheImagesAndMasks { get; set; } = true;
    }
}
