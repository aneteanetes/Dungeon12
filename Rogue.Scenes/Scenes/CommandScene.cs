namespace Rogue.Scenes.Scenes
{
    using System.Collections.Generic;
    using System.Linq;
    using Rogue.Control.Commands;
    using Rogue.Control.Keys;

    public abstract class CommandScene : Scene
    {
        public List<Command> Commands = new List<Command>();

        public CommandScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        protected virtual void KeyPress(Key keyPressed, KeyModifiers keyModifiers) { }

        protected override void KeyPress(KeyArgs keyEventArgs)
        {
            var commandPress = Commands.Where(x => x.Keys.Contains(keyEventArgs.Key));

            foreach (var commandPressed in commandPress)
            {
                commandPressed.Run(keyEventArgs.Key);
            }

            this.KeyPress(keyEventArgs.Key, keyEventArgs.Modifiers);
        }
    }
}