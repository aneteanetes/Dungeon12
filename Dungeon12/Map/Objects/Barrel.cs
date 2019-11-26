using Dungeon;
using Dungeon.Classes;
using Dungeon.Data.Attributes;
using Dungeon.Data.Region;
using Dungeon.Entities.Alive;
using Dungeon.Game;
using Dungeon.Map;
using Dungeon.Map.Infrastructure;
using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using Dungeon12.Database.Barrels;
using Dungeon12.SceneObjects.Map;
using System;
using System.Linq;

namespace Dungeon12.Map.Objects
{
    [Template("Barrel")]
    [DataClass(typeof(BarrelData))]
    public class Barrel : MapObject
    {
        public override bool Saveable => true;

        public override bool Obstruction => true;

        public ConsoleColor Color { get; set; }

        public string IdentifyName { get; set; }

        public bool Used { get; set; } 

        protected override void Load(RegionPart regionPart)
        {
            var data = LoadData<BarrelData>(regionPart);

            this.Color = data.Color;
            this.Name = data.Name;
            this.IdentifyName = regionPart.IdentifyName;
            this.Location = regionPart.Position;
        }

        public override ISceneObject Visual(GameState gameState)
        {
            return new BarrelSceneObject(gameState.Player, this);
        }

        public void Use(Dungeon12Class alive)
        {
            string text = default;

            switch (Color)
            {
                case ConsoleColor.Blue:
                    alive.Defence++;
                    text = "+1 Защита";
                    break;
                case ConsoleColor.Cyan:
                    alive.AttackPower++;
                    text = "+1 Сила атаки";
                    break;
                case ConsoleColor.DarkMagenta:
                    alive.Barrier++;
                    text = "+1 Барьер";
                    break;
                case ConsoleColor.Magenta:
                    alive.AbilityPower++;
                    text = "+1 Сила магии";
                    break;
                case ConsoleColor.Red:
                    alive.MaxHitPoints += 5;
                    alive.HitPoints+=5;
                    text = "+5 Максимального здоровья";
                    break;
                default:
                    break;
            }

            if (!alive.Journal.World.Any(j => j.Display == this.Name))
            {
                alive.Journal.World.Add(new Entites.Journal.JournalEntry()
                {
                    Group = "Бочки",
                    Text = text==default ? "Непредсказума" : $"{this.Name} прибавляет: {text} постоянно.",
                    Display = this.Name
                });
            }

            if (text != default)
            {
                var txt = text.AsDrawText().InSize(12).InColor(this.Color).Montserrat();
                var popup = new PopupString(txt, alive.MapObject.Location);
                alive.SceneObject.ShowInScene(popup.InList<ISceneObject>());
            }
        }
    }
}