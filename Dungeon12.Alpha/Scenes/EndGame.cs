namespace Dungeon12.Scenes.Menus
{
    using Dungeon;
    using Dungeon.Control.Keys;
    using Dungeon.Drawing.SceneObjects;
    using Dungeon.SceneObjects;
    using Dungeon.Scenes;
    using Dungeon.Scenes.Manager;
    using Dungeon12.Scenes.Game;

    public class EndGame : GameScene<Start, MainScene>
    {
        public EndGame(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => true;

        public override void Initialize()
        {
            this.AddObject(new ImageObject("Loading/Sacrifice.png".AsmImg()));
            this.AddObject(new EndSceneObject());
        }

        protected override void KeyPress(Key keyPressed, KeyModifiers keyModifiers, bool hold)
        {
            if (keyPressed == Key.Escape && !hold)
            {
                Global.SceneManager.Destroy<MainScene>();
                this.Switch<Start>();
            }
        }
    }

    public class EndSceneObject : EmptySceneObject
    {
        public EndSceneObject()
        {
            var text = $@"Лорд Веры начинает ритуал и собирается принести вас в жертву.
Начинается ритуал пробуждения святилищ по всей планете. Вокруг Острова Веры поднимается большой шторм.
В это время государство Трёх Островов подплывает к Острову Веры для проведения аудита.
Вместе с ними подплывает торговый корабль с материка заполненный новыми последователями Культа Крови.
Используя ужасную погоду как преимущество, пираты взяли курс на Остров Веры...

(Нажмите Esc для продолжения)";

            this.Width = 36;
            this.Left = 2;

            this.Top = 2;
            this.Height = 18;

            this.AddTextCenter(text.AsDrawText().InSize(12).Montserrat().WithWordWrap());
        }
    }
}