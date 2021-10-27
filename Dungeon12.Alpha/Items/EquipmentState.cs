using Dungeon12.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Items
{
    public class EquipmentState
    {
        public void Reset() => AdditionalEquipments.Clear();

        /// <summary>
        /// Сериализация :(
        /// </summary>
        public List<ClassStat> AdditionalEquipments { get; set; } = new List<ClassStat>();

        public void AddEquip(ClassStat equipment) => AdditionalEquipments.Add(equipment);

        public void RemoveEqip(ClassStat equipment) => AdditionalEquipments.Remove(equipment);
    }
}
