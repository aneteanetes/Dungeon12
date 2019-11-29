namespace Dungeon12.Scenes.Menus
{
    using Dungeon;
    using Dungeon.Control.Keys;
    using Dungeon.Drawing;
    using Dungeon.Drawing.SceneObjects;
    using Dungeon.Map.Objects;
    using Dungeon.SceneObjects;
    using Dungeon.Scenes;
    using Dungeon.Scenes.Manager;
    using Dungeon12.CardGame.Scene;
    using Dungeon12.Drawing.SceneObjects;
    using Dungeon12.Map.Editor;
    using Dungeon12.Races.Perks;
    using Dungeon12.SceneObjects;
    using Dungeon12.Scenes.Game;
    using Dungeon12.Scenes.SaveLoad;
    using System;
    using System.Linq;

    public class Start : StartScene<SoloDuoScene, Game.Main, EditorScene, CardGameScene,Start, SaveLoadScene>
    {
        public override bool AbsolutePositionScene => true;

        public Start(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => true;

        private bool isGame;

        public override void Init()
        {
            isGame = Args?.ElementAtOrDefault(0) != default;

            Global.DrawClient.SetCursor("Cursors.common.png".PathImage());
            
            Global.AudioPlayer.Music("Audio/Music/maintheme.ogg".AsmNameRes(), new Dungeon.Audio.AudioOptions()
            {
                Repeat = true,
                Volume = 0.3
            });
            
            this.AddObject(new Background(true)
            {
                AbsolutePosition = true,
            });
            this.AddObject(new ImageControl("Dungeon12.Resources.Images.d12textM.png")
            {
                Top = 2f,
                Left = 10f,
                AbsolutePosition = true,
            });

            this.AddObject(new MetallButtonControl(isGame ? "Главное меню" : "Новая игра")
            {
                Left = 15.5f,
                Top = 8,
                OnClick = () =>
                {
                    if (isGame)
                    {
                        QuestionBox.Show(new QuestionBoxModel()
                        {
                            Text=$"Вы уверены что хотите выйти в главное меню?{Environment.NewLine}Весь не сохранённый прогресс пропадет.",
                            Yes = () =>
                            {
                                Global.RemoveSaveInMemmory();
                                this.ClearState();
                                Global.GameState = new Dungeon.Game.GameState();
                                SceneManager.Destroy<Main>();
                                this.Switch<Start>();
                            }
                        }, this.ShowEffectsBinding);
                    }
                    else
                    {
                        this.Switch<SoloDuoScene>();
                        Global.GameState.Reset();
                    }
                },
                AbsolutePosition=true,
            });

            this.AddObject(new MetallButtonControl("Загрузить")
            {
                Left = 15.5f,
                Top = 11,
                AbsolutePosition = true,
                OnClick = () =>
                {
                    this.Switch<SaveLoadScene>(isGame ? "game" : default);

                    //this.PlayerAvatar = new Avatar(new Dungeon12.Noone.Noone()
                    //{
                    //    Origin = Dungeon.Entities.Alive.Enums.Origins.Adventurer
                    //});
                    //this.PlayerAvatar.Character.Name = "Ваш персонаж";

                    //Global.AudioPlayer.Music("town", new Dungeon.Audio.AudioOptions()
                    //{
                    //    Repeat = true,
                    //    Volume = 0.3
                    //});

                    //this.Switch<Game.Main>();
                }
            });

            this.AddObject(new SmallMetallButtonControl(new DrawText("#") { Size = 40 }.Montserrat())
            {
                Left = 24,
                Top = 11,
                AbsolutePosition = true,
                OnClick = () =>
                {
                    this.Switch<EditorScene>();
                }
            });

            this.AddObject(new MetallButtonControl(isGame ? "Сохранить" : "Создатели")
            {
                Left = 15.5f,
                Top = 14,
                AbsolutePosition = true,
                OnClick = () =>
                {
                    this.Switch<SaveLoadScene>("game","saving");
                    //if (isGame)
                    //{
                    //    var saveName =  Dungeon.Data.Database.Save();
                    //    MessageBox.Show($"Игра сохранена: {saveName}", this.ShowEffectsBinding);
                    //}
                    //else
                    //{
                    //    //
                    //}
                }
            });

            this.AddObject(new MetallButtonControl(isGame? "Назад" : "Выход")
            {
                Left = 15.5f,
                Top = 17,
                AbsolutePosition = true,
                OnClick = () =>
                {
                    if (isGame)
                        this.Switch<Main>();
                    else
                        Global.Exit?.Invoke();
                }
            });
        }

        protected override void KeyPress(Key keyPressed, KeyModifiers keyModifiers, bool hold)
        {
            if (isGame && keyPressed == Key.Escape)
                this.Switch<Main>();
        }
    }
}