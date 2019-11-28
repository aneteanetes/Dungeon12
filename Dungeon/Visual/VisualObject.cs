﻿using Dungeon.Game;
using Dungeon.Network;
using Dungeon.View.Interfaces;

namespace Dungeon
{
    public class VisualObject : NetObject, IVisual
    {
        public virtual ISceneObject Visual(GameState gameState) => default;
    }
}