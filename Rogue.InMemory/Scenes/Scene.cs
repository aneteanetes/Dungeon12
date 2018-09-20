namespace Rogue.InMemory.Scenes
{
    using Rogue.Entites.Alive.Character;
    using Rogue.Logging;
    using Rogue.Map;

    public class Scene : GlobalScene
    {
        public Player Player;

        public Location Map;

        public Logger Log;        
    }
}