using Dungeon.SceneObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace VegaPQ.SceneObjects
{
    public class Game : EmptyHandleSceneControl
    {
        public Game()
        {
            this.Width = 16.875;
            this.Height = 30;

            var field = new GameField(8,8);
            field.Top = this.Height - field.Height - 1;

            this.AddChildCenter(field,vertical:false);
        }

        public override bool Updatable => true;
    }
}
