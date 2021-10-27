namespace Dungeon12.Map.Editor.Toolbox
{
    using Dungeon.Drawing;
    using Dungeon.Drawing.SceneObjects;
    using Dungeon12.SceneObjects;
    using Dungeon.SceneObjects;
    using System;
    using Dungeon12.Drawing.SceneObjects;
    using Dungeon12;

    public class ToolboxControl : DarkRectangle
    {
        public override bool AbsolutePosition => true;

        private TextInputControl textInputControl;
        private TileSelector tileSelector;

        public ToolboxControl(Action<ImageObject> select, Action<int> level, Action<bool> obstruct, Action<bool> fullImage)
        {
            this.Height = 23;
            this.Width = 19;

            textInputControl = new TextInputControl(new DrawText("", new DrawColor(ConsoleColor.White)) { Size = 10 }.Montserrat(), 30,false,false)
            {
                OnEnter = s => LoadTileset()
            };
            this.AddChild(textInputControl);


            var levelControl = new TextInputControl(new DrawText("1", new DrawColor(ConsoleColor.White)) { Size = 15 }.Montserrat(), 1, false, false,true,true)
            {
                OnEnter = s =>
                {
                    if (int.TryParse(s, out var lvl))
                    {
                        level(lvl);
                    }
                },
                Validation = s =>
                {
                    if (s.Length > 1)
                        return false;

                    if (int.TryParse(s, out var num))
                    {
                        if (num <= 0)
                        {
                            return false;
                        }
                        return true;
                    }

                    return false;
                },
                Left = 6.5
            };
            levelControl.Top = 1;
            this.AddChild(levelControl);
            this.AddChild(new TextControl(new DrawText("Уровень") { Size = 15 }.Montserrat()) { Left = 7.5, Top = 1.25 });

            this.AddChild(new CheckBox(new DrawText("Препятствие") { Size=20}.Montserrat())
            {
                Left=11,
                Top=0.5,
                OnChange= obstruct
            });

            this.AddChild(new CheckBox(new DrawText("Не тайл") { Size = 20 }.Montserrat())
            {
                Left = 11,
                Top = 2,
                OnChange = v =>
                {
                    tileSelector.FullTile = v;
                    fullImage?.Invoke(v);
                }
            });

            tileSelector = new TileSelector(select)
            {
                Top = this.textInputControl.Height + 2
            };
            this.AddChild(tileSelector);
        }

        private void LoadTileset()
        {
            if (!string.IsNullOrEmpty(textInputControl.Value))
            {
                var tileset = $"Dungeon12.Resources.Images.Tiles.{textInputControl.Value}.png";
                tileSelector.Load(tileset);
            }
        }
    }
}
