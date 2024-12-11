﻿using Dungeon.Control;
using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using Nabunassar.ECS.Components;

namespace Nabunassar.SceneObjects.Stats
{
    internal class Close29x29 : EmptySceneControl, ITooltiped
    {
        public override void Throw(Exception ex)
        {
            throw ex;
        }

        public string TooltipText => "Закрыть";

        private ISceneObject _sceneObject;

        public Close29x29(ISceneObject sceneObject)
        {
            _sceneObject=sceneObject;

            Width = 29;
            Height = 29;

            Image = "UI/layout/location/x.png";
        }

        public override void Click(PointerArgs args)
        {
            _sceneObject.Destroy();
            base.Click(args);
        }
    }
}
