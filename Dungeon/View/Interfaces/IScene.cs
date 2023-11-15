using Dungeon.ECS;
using Dungeon.Resources;
using System.Collections.Generic;

namespace Dungeon.View.Interfaces
{
    public interface IScene
    {
        string Uid { get; } 

        ISceneLayer[] Layers { get; }

        ISceneLayer GetLayer(string name);

        bool AbsolutePositionScene { get; }

        void Destroy();

        void Loaded();

        void AddSystem(ISystem system);

        TSystem GetSystem<TSystem>() where TSystem : ISystem;

        IEnumerable<ISystem> GetSystems();

        void LoadResources();

        Resource GetResource(string name);

        bool IsPreloadedScene {  get; }
    }
}