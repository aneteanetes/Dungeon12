using Dungeon;
using Dungeon.Control.Keys;
using Dungeon.Drawing.SceneObjects;
using Dungeon.Resources;
using Dungeon.Scenes;
using Dungeon.Scenes.Manager;
using Dungeon12.ECS.Systems;
using Dungeon12.SceneObjects.TCG;
using Dungeon12.TCG;

namespace Dungeon12.Scenes
{
    public class TCGScene : GameScene<StartScene>
    {
        public TCGScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => true;

        public override bool AbsolutePositionScene => false;
        public override void Initialize()
        {
            var background = this.CreateLayer("background");
            background.AddObject(new ImageObject("TCG/background.png".AsmImg())
            {
                Width = Global.Resolution.Width,
                Height = Global.Resolution.Height
            });

            background.AddObject(new ImageObject("TCG/deck.png".AsmImg())
            {
                Left=10,
                Top=603
            });

            background.AddObject(new ImageObject("TCG/hand.png".AsmImg())
            {
                Left = 325,
                Top = 600
            });

            background.AddObject(new ImageObject("TCG/table.png".AsmImg())
            {
                Left = 7,
                Top = 305
            });

            background.AddObject(new ImageObject("TCG/deck.png".AsmImg())
            {
                Left = 1270,
                Top = -3
            });

            background.AddObject(new ImageObject("TCG/hand.png".AsmImg())
            {
                Left = 4,
                Top = -3
            });

            var warriorcard = ResourceLoader.LoadJson<Card>($"Cards/warrior.json".AsmRes());

            var cards = this.CreateLayer("cards");
            cards.AddSystem(new TooltipSystem());
            cards.AddObjectCenter(new CardSceneObject(warriorcard));
        }

        protected override void KeyPress(Key keyPressed, KeyModifiers keyModifiers, bool hold)
        {
            if (keyPressed == Key.Escape)
                this.Switch<StartScene>();

            base.KeyPress(keyPressed, keyModifiers, hold);
        }
    }
}