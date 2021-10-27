using Dungeon;
using Dungeon.Control;
using Dungeon.Data;
using Dungeon.Drawing;
using Dungeon.Drawing.SceneObjects;
using Dungeon12.Map;
using Dungeon12.Map.Objects;
using Dungeon12.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon.Scenes.Manager;
using Dungeon12.Drawing.SceneObjects;
using Newtonsoft.Json;
using System;
using static Dungeon12.Global;
using Dungeon12.Events.Events;
using Dungeon12.Abilities;
using Dungeon12.Scenes.Game;

namespace Dungeon12.SceneObjects.SaveLoad
{
    public class SaveLoadSlot : Dungeon12.SceneObjects.SceneControl<SaveModel>
    {
        private SaveLoadWindow _saveLoadWindow;

        public SaveLoadSlot(SaveModel component, bool isSave, Action switchMain, SaveLoadWindow saveLoadWindow) : base(component, false)
        {
            _saveLoadWindow = saveLoadWindow;
            Image = "ui/dialogs/answerpanel.png".AsmImg();

            this.Width = 22;
            this.Height = 3;

            this.AddChild(new DarkRectangle()
            {
                Width = 23,
                Height = 3
            });

            if (component != default)
            {
                var name = this.AddTextCenter(component.Name.AsDrawText().Montserrat(), false, false);
                name.Top = .5;
                name.Left = .5;

                var location = this.AddTextCenter((component.RegionName + "   " + component.GameTime).AsDrawText().Montserrat(), false, false);
                location.Top += 1.25;
                location.Left = .5;

                var @char = this.AddTextCenter($"{component.CharacterName} {component.ClassName} ({component.Level})".AsDrawText().Montserrat(), false, false);
                @char.Top += 2;
                @char.Left = .5;
            }
            else
            {
                var input = new TextInputControl(DateTime.Now.ToString().AsDrawText().Montserrat(), 24, width: 10, height: 1, autofocus: false, onEnterOnBlur: true);
                input.Top = .5;
                input.Left = .5;
                input.Value = DateTime.Now.ToString();

                this.AddChild(input);
                this.AddChild(new MetallButtonControl("Сохранить")
                {
                    Left = 13.25,
                    Top = .25,
                    AbsolutePosition = true,
                    OnClick = () =>
                    {
                        var value = input.Value;
                        if (string.IsNullOrWhiteSpace(value))
                        {
                            value = DateTime.Now.ToString();
                        }

                        input.Value = Global.Save(saveGameName: value);
                        Toast.Show($"Игра {value} сохранена!", this.ShowInScene);
                        _saveLoadWindow.ReDraw();

                        input.FreeIfFreeze();
                    }
                });
            }

            if (!isSave)
            {
                this.AddChild(new MetallButtonControl("Загрузить")
                {
                    Left = 13.25,
                    Top = .25,
                    AbsolutePosition = true,
                    OnClick = () =>
                    {
                        Global.SceneManager.LoadingScreenCustom("FaithIsland").Then((Action<Dungeon.Types.Callback>)(cb =>
                        {
                            Global.SceneManager.Destroy<MainScene>();

                            Cooldown.ResetAll();
                            var data = JsonConvert.DeserializeObject<SavedGame>(base.Component.Data, Global.GetSaveSerializeSettings());
                            GameMap.DeferredMapObjects = data.MapDeferredOptions;
                            Global.GameState.RestorableRespawns = base.Component.RestorableRespawns;

                            Global.GameState.PlayerAvatar = new Avatar(data.Character.Character)
                            {
                                Location = data.Character.Location,
                                SceenPosition = base.Component.ScreenPosition
                            };

                            Global.Time.Set(data.Time);

                            Global.GameState.Equipment = data.EquipmentState;
                            Global.GameState.Character = data.Character.Character;
                            Global.GameState.Character.Reload();

                            Global.Camera.SetCamera(base.Component.CameraOffset.X, component.CameraOffset.Y);

                            Global.GameState.Map = new Dungeon12.Map.GameMap();
                            Global.GameState.Map.LoadRegion(data.Map);


                            var regionMap = new GameMap();
                            regionMap.LoadRegion(data.Region, true);
                            Global.GameState.Region = regionMap;

                            Global.Events.Raise(new GameLoadedEvent());

                            switchMain?.Invoke();

                            cb.Dispose();
                        }));
                    }
                });
            }
            else if (Component != default)
            {
                this.AddChild(new MetallButtonControl("Перезаписать")
                {
                    Left = 13.25,
                    Top = .25,
                    AbsolutePosition = true,
                    OnClick = () =>
                    {
                        var overwrited = Global.Save(Component.Id);
                        Toast.Show($"Игра {component.Name} перезаписана!", this.ShowInScene);
                        _saveLoadWindow.ReDraw();
                    }
                });
            }
        }

        public override void Click(PointerArgs args)
        {
            if (args.MouseButton == Dungeon.Control.Pointer.MouseButton.Right)
            {
                if (Component != default)
                {
                    QuestionBox.Show(new QuestionBoxModel()
                    {
                        Text = "Вы действительно хотите удалить эту сохранённую игру?",
                        Yes = Delete
                    }, ShowInScene);
                }
            }
        }

        private void Delete()
        {
            Global.RemoveSavedGame(this.Component.Id);
            this.Destroy?.Invoke();
            _saveLoadWindow.ReDraw();
        }

        public int ItemIndex { get; set; }

        public override bool Visible
        {
            get
            {
                if (this.Top < 0)
                    return false;

                if (this.Top > 15)
                    return false;

                return true;
            }
        }
    }
}