namespace Rogue.Components
{
    public class GameContext : PropertyChangedBase
    {
        public static GameContext Current { get; } = new GameContext();
    }    
}