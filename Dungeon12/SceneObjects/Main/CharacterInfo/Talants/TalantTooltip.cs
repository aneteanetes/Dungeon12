using Dungeon.Abilities.Talants.NotAPI;
using Dungeon.Classes;
using Dungeon.Drawing.GUI;
using Dungeon.Drawing.Impl;
using Dungeon.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dungeon12.Drawing.SceneObjects.Main.CharacterInfo.Talants
{
    public class TalantTooltip : Tooltip
    {
        public override bool CacheAvailable => false;

        public override bool Interface => true;

        private readonly Character character;
        private readonly TalantBase talant;
        private double textPosTop = 0;

        public TalantTooltip(TalantBase talant, Character character, Point position) : base("", position, new DrawColor(System.ConsoleColor.Black))
        {
            this.character = character;
            this.talant = talant;
            Opacity = 0.8;

            this.Children.ForEach(c => c.Destroy?.Invoke());

            MeasureSize();
            this.Width = maxMeasure.X / 32;
            this.Height = textPosTop;
            textPosTop = 0; //мы измерили размер, но нам всё равно это нужно для того что бы определять где рисовать следующую надпись


            base.Left = position.X - this.Width / 4;
            this.Top = position.Y;

            Stats.ForEach(AddLine);
        }

        private IEnumerable<DrawText> Stats
        {
            get
            {
                List<DrawText> stats = new List<DrawText>
                {
                    Title,
                    Activatable,
                    Description
                };
                stats.AddRange(TalantDescriptions);

                return stats;
            }
        }

        private DrawText Title
        {
            get
            {
                var title = new DrawText(talant.Name, ConsoleColor.White) { Size = 20 }.Montserrat();
                //Measure(title);
                return title;
            }
        }

        private DrawText Activatable
        {
            get
            {
                var text = "Пассивный";
                ConsoleColor color = ConsoleColor.Yellow;

                if(talant.Activatable)
                {
                    text = "Активируемый";
                    if(talant.Group!=null)
                    {
                        text += ": " + talant.Group;
                    }
                    color = ConsoleColor.Cyan;
                }

                var title = new DrawText(text, color) { Size = 11 }.Montserrat();                
                return title;
            }
        }

        private DrawText Description => new DrawText(Environment.NewLine+talant.Description+ Environment.NewLine, ConsoleColor.White).Montserrat();
        
        private IEnumerable<DrawText> TalantDescriptions
        {
            get
            {
                return talant.TalantEffects(character).SelectMany(x => new List<DrawText>()
                {
                    new DrawText($"{x.Name} [{x.AbilityName}]{Environment.NewLine}", ConsoleColor.White){Size=14}.Montserrat(),
                    new DrawText(x.Description+Environment.NewLine, ConsoleColor.White).Montserrat()
                });
            }
        }

        private void AddLine(DrawText drawText)
        {
            if (drawText == null)
                return;

            var txt = this.AddTextCenter(drawText, vertical: false);
            txt.Top = textPosTop;
            textPosTop += MeasureText(txt.Text).Y / 32;
        }

        private void MeasureSize()
        {
            void MeasureLocal(DrawText drawText)
            {
                if (drawText == null)
                    return;

                var measure = this.MeasureText(drawText);
                textPosTop += measure.Y / 32;

                if (measure.X > maxMeasure.X)
                {
                    maxMeasure = measure;
                }
            }

            Stats.ForEach(MeasureLocal);
        }

        private Point maxMeasure = new Point();
    }
}
