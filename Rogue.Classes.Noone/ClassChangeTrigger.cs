using Rogue.Conversations;
using Rogue.Drawing.SceneObjects.Map;
using Rogue.View.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Rogue.Classes
{
    public class ClassChangeTrigger : IConversationTrigger
    {
        private PlayerSceneObject SceneObject => PlayerSceneObject as PlayerSceneObject;

        private Map.GameMap Gmap => Gamemap as Map.GameMap;

        public object PlayerSceneObject { get; set; }

        public object Gamemap { get; set; }

        public IDrawText Execute(string[] args)
        {            
            Character from = SceneObject.Avatar.Character;

            var newClass = args[0];
            var newClassAssembly = args[1];

            // создаём новый экземпляр класса
            var to = newClass.GetInstanceFromAssembly<Character>(newClassAssembly);

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

            Global.Events.Raise(GlobalEvent.ClassChange);

            return "Rogue.Drawing.Impl.DrawText".GetInstanceFromAssembly<IDrawText>("Rogue.Drawing", "Класс поменяли");
        }
    }
}