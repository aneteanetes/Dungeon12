using Rogue.View.Enums;

namespace Rogue.View.Interfaces
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