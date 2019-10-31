using Dungeon.View.Enums;

namespace Dungeon.View.Interfaces
{
    public interface IImageMask
    {
        MaskPattern Pattern { get; }

        IDrawColor Color { get; }

        bool CacheAvailable { get; }

        float Opacity { get; }

        bool Visible { get; }

        float AmountPercentage { get; }
    }
}