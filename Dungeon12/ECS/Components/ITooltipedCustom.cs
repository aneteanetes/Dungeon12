using Dungeon.View.Interfaces;
using Nabunassar.SceneObjects.Base;

namespace Nabunassar.ECS.Components
{
    internal interface ITooltipedCustom
    {
        bool ShowTooltip { get; }

        ISceneObject GetTooltip();
    }
}