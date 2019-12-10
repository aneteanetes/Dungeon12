using Dungeon;
using Dungeon12.Classes;
using Dungeon.Data.Attributes;
using Dungeon12.Data.Region;
using Dungeon12.Entities.Alive;
using Dungeon12.Game;
using Dungeon12.Map;
using Dungeon12.Map.Infrastructure;
using Dungeon12.SceneObjects;
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

        public override ISceneObject Visual()
        {
            return new BarrelSceneObject(Global.GameState.Player, this);
        }

        public void Use(Character alive)
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