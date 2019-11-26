using Dungeon;
using Dungeon.Data;
using Dungeon.Drawing.SceneObjects;
using Dungeon.Map.Objects;
using Dungeon.SceneObjects;
using Dungeon.Scenes.Manager;
using Dungeon12.Drawing.SceneObjects;
using Newtonsoft.Json;
using System;

namespace Dungeon12.SceneObjects.SaveLoad
{
    public class SaveLoadSlot : HandleSceneControl<SaveModel>
    {
        public SaveLoadSlot(SaveModel component, bool isSave, Action switchMain) : base(component, false)
        {
            Image = "ui/dialogs/answerpanel.png".AsmImgRes();

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

                        input.Value = Dungeon.Data.Database.Save(saveGameName: value);
                        MessageBox.Show($"Игра {value} сохранена!", this.ShowEffects);
                        switchMain?.Invoke();
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
                        var data = JsonConvert.DeserializeObject<SavedGame>(Component.Data, Dungeon.Data.Database.GetSaveSerializeSettings());
                        SceneManager.Current.PlayerAvatar = new Avatar(data.Character.Character)
                        {
                            Location = data.Character.Location,
                            SceenPosition = Component.ScreenPosition
                        };

                        Global.Camera.SetCamera(Component.CameraOffset.X, component.CameraOffset.Y);

                        switchMain?.Invoke();
                    }
                });
            }
            else if (Component!=default)
            {
                this.AddChild(new MetallButtonControl("Перезаписать")
                {
                    Left = 13.25,
                    Top = .25,
                    AbsolutePosition = true,
                    OnClick = () =>
                    {
                        var overwrited = Global.Save(Component.Id);
                        MessageBox.Show($"Игра {component.Name} перезаписана!", this.ShowEffects);
                    }
                });
            }
        }

        public int ItemIndex { get; set; }

        public override bool Visible
        {
            get
            {
                if (this.Top < 0)
                    return false;

                if (this.Top > 21)
                    return false;

                return true;
            }
        }
    }
}