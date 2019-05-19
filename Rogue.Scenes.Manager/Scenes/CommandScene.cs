namespace Rogue.Scenes
{
    using System.Collections.Generic;
    using System.Linq;
    using Rogue.Control.Commands;
    using Rogue.Control.Keys;
    using Rogue.Scenes.Manager;

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