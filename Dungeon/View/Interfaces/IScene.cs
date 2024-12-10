using Dungeon.ECS;
using Dungeon.Resources;
using System;
using System.Collections.Generic;

namespace Dungeon.View.Interfaces
{
    public interface IScene
    {
        string Uid { get; } 

        ISceneLayer[] Layers { get; }

        ResourceTable Resources { get; }

        ISceneLayer GetLayer(string name);

        bool AbsolutePositionScene { get; }

        void Destroy();

        void Loaded();

        void Update(GameTimeLoop gameTimeLoop);

        void AddSystem(ISystem system);

        TSystem GetSystem<TSystem>() where TSystem : ISystem;

        IEnumerable<ISystem> GetSystems();

        void Load();

        bool IsLoaded { get; set; }

        bool IsInitialized { get; set; }

        Resource GetResource(string name);
    }
}