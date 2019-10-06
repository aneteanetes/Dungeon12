using Rogue.Abilities;
using Rogue.Conversations;
using Rogue.Drawing.SceneObjects.Map;
using Rogue.Events;
using Rogue.Map;
using Rogue.View.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Rogue.Classes
{
    public class ClassChangeTrigger : IConversationTrigger
    {
        public object PlayerSceneObject { get; set; }

        public object Gamemap { get; set; }

        public IDrawText Execute(string[] args)
        {
            var SceneObject = this.PlayerSceneObject.As<PlayerSceneObject>();

            Character from = SceneObject.Avatar.Character;

            var newClass = args[0];
            var newClassAssembly = args[1];

            // создаём новый экземпляр класса
            var to = newClass.GetInstanceFromAssembly<Character>(newClassAssembly);

            //отключаем все пассивные способности
            from.PropertiesOfType<Ability>()
                .Where(a => a.CastType == Abilities.Enums.AbilityCastType.Passive)
                .ToList()
                .ForEach(a => a.Release(Gamemap.As<GameMap>(), SceneObject.Avatar));

            // убираем все перки которые имеют отношение к классу
            from.RemoveAll(p => p.ClassDependent);

            to.Backpack = from.Backpack;
            to.Clothes = from.Clothes;
            to.EXP = from.EXP;
            to.Gold = from.Gold;
            to.HitPoints = from.HitPoints;
            to.MaxHitPoints = from.MaxHitPoints;
            to.AbilityPower = from.AbilityPower;
            to.AttackPower = from.AttackPower;
            to.Barrier = from.Barrier;
            to.Defence = from.Defence;
            to.Idle = from.Idle;
            to.MinDMG = from.MinDMG;
            to.MaxDMG = from.MaxDMG;

            to.Race = from.Race;
            to.Name = from.Name;
            to.Level = from.Level;

            to.Recalculate();

            SceneObject.Avatar.Character = to;

            Global.Events.Raise(new ClassChangeEvent()
            {
                PlayerSceneObject = SceneObject,
                GameMap = Gamemap.As<GameMap>(),
                Character = SceneObject.Avatar.Character
            });

            return "Rogue.Drawing.Impl.DrawText".GetInstanceFromAssembly<IDrawText>("Rogue.Drawing", "Класс поменяли");
        }
    }
}