namespace Dungeon.Scenes
{
    using System.Collections.Generic;
    using System.Linq;
    using Dungeon.Control.Commands;
    using Dungeon.Control.Keys;
    using Dungeon.Scenes.Manager;

    public abstract class CommandScene : Scene
    {
        public List<Command> Commands = new List<Command>();

        public CommandScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        protected override void KeyPress(Key keyPressed, KeyModifiers keyModifiers, bool hold)
        {
            var commandPress = Commands.Where(x => x.Keys.Contains(keyPressed));

            foreach (var commandPressed in commandPress)
            {
                commandPressed.Run(keyPressed);
            }
        }
    }
}