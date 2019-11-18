using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon
{
    public static class TriggerExtensions
    {
        public static TTrigger Trigger<TTrigger>(this string className)
            where TTrigger : ITrigger
        {
            return className.GetInstanceFromAssembly<TTrigger>();
        }
    }
}
