using Dungeon.View.Interfaces;
using Dungeon12.SceneObjects.Stats;
using System;

namespace Dungeon12
{
    internal class GlobalWindows
    {
        public StatsWindow StatsWindow { get; set; }

        public void Activate<T>(ISceneLayer layer, bool ignoreOpened = false)
        {
            switch (typeof(T))
            {
                case Type stats when stats==typeof(StatsWindow):
                    {
                        if (StatsWindow==null)
                        {
                            layer.AddObjectCenter(StatsWindow=new StatsWindow(layer));
                            StatsWindow.OnDestroy+=() => StatsWindow=null;
                        }
                        else if (!ignoreOpened)
                        {
                            StatsWindow.Destroy();
                        }
                        break;
                    }
                default:
                    break;
            }
        }
    }
}