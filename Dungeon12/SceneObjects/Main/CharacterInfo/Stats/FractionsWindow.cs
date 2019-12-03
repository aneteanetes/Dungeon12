using Dungeon;
using Dungeon.Classes;
using Dungeon.Drawing;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using Dungeon12.SceneObjects.Main.CharacterInfo.Stats;
using Dungeon12.SceneObjects.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon12.SceneObjects.Main.CharacterInfo.Stats
{

    public class FractionsWindow : EmptyHandleSceneControl
    {
        public override bool CacheAvailable => false;

        public override bool AbsolutePosition => true;

        private Character character;

        public FractionsWindow(Character character)
        {
            this.Image = "Dungeon12.Resources.Images.ui.stats.png";
            this.Width = 6;
            this.Height = 16;

            this.character = character;

            var title = this.AddTextCenter(new DrawText("Репутация"), true, false);
            title.Top = .2;
            this.AddChild(new DarkRectangle() { Color = ConsoleColor.Black, Opacity = 1, Left = 0.5, Width = this.Width - 1, Height = 0.1, Top = 1.5 });

            var top = 4;
            foreach (var fraction in character.Fractions)
            {
                var progress = fraction.Progress;
                DrawText Text()
                {
                    var txt = $"Прогресс: {progress.Reputation}/{progress.ReputationMax}".AsDrawText().Montserrat();
                    txt.AppendNewLine();
                    txt.Append($"[{progress.Level.ToDisplay()}]".AsDrawText().InColor(ConsoleColor.Green).Montserrat());
                    return txt;
                }

                var bar = new ProgressBar(new ProgressBarModel()
                {
                    Name= fraction.Name,
                    Progress = () => (progress.Reputation, progress.ReputationMax),
                    ProgressText = Text,
                    BarOffsetDown=.3
                });
                this.AddChild(bar);
                bar.Scale = 0.75;
                bar.Left = 0.5;
                bar.Top = top;

                this.AddChild(new DarkRectangle() { Color = ConsoleColor.Black, Opacity = 1, Left = 0.5, Width = this.Width - 1, Height = 0.1, Top = top+1 });
                top += 3;
            }
        }
    }
}
