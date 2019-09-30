namespace Rogue
{
    using Rogue.Classes;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Loader;

    public static class RogueClasses
    {
        private static bool ClassesLoaded = false;
        private static IEnumerable<Character> classes;
        public static IEnumerable<Character> All()
        {
            if (classes == null)
            {
                CheckAssemblyLoaded();

                   var classTypes = AppDomain.CurrentDomain.GetAssemblies()
                    .Where(x=>!x.IsDynamic)
                    .SelectMany(x => x.GetTypes()
                        .Where(t=>!t.IsAbstract)
                        .Where(t => typeof(Character).IsAssignableFrom(t)));

                classes = classTypes
                    .Select(x => (Character)x.New())
                    .Where(x => !string.IsNullOrEmpty(x.ClassName));
            }

            return classes;
        }

        private static void CheckAssemblyLoaded()
        {
            if (!ClassesLoaded)
            {
                ClassesLoaded = true;

                var files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "Rogue.Classes.*.dll");
                foreach (var file in files)
                {
                    AssemblyLoadContext.Default.LoadFromAssemblyPath(file);
                }
            }
        }

        public static Tto Transfer<Tfom,Tto>(Tfom from)
            where Tfom : Character
            where Tto : Character
        {
            // создаём новый экземпляр класса
            var to = typeof(Tto).New<Tto>();

            // убираем все перки которые имеют отношение к классу
            from.RemoveAll(p=>p.ClassDependent);

            to.Backpack = from.Backpack;
            to.Clothes= from.Clothes;
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

            return to;
        }
    }
}